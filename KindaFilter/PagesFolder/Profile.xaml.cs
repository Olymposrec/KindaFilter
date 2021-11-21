

using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using KindaFilter.Models;
using KindaFilter.Services;
using KindaFilter.ViewModels;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        MediaFile file;
        public string WebAPIKey = "AIzaSyDQveJfg1KOrqz5_vBmj8WeQL4dFWs-umA";
        public Profile()
        {
            InitializeComponent();
            BindingContext =  new UsersInfoView();
            GetProfileInfo();


        }

        public async void GetProfileInfo()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));

            try
            {
                FirebaseClient fc = new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");

                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                var result = (await fc
                   .Child("Users")
                   .OnceAsync<User>()).FirstOrDefault(a => a.Object.Email == savedFirebaseAuth.User.Email);

                if (result != null)
                {
                    FirstName.Text = result.Object.FirstName.ToString();
                    LastName.Text = result.Object.LastName.ToString();
                    Phone.Text = result.Object.PhoneNumber.ToString();
                    DisplayName.Text = result.Object.DisplayName.ToString();
                    UploadImage.Source = result.Object.PhotoUrl.ToString();
                    Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));

                    SaveButton.IsEnabled = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Navigation.PushAsync(new LoginPage());
                await App.Current.MainPage.DisplayAlert("Alert", "something went wrong", "OK!");
            }
        }
        private async void Button_SaveData(object sender, System.EventArgs e)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                FirebaseClient fc = 
                    new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");
                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth
                    .FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                var result = await fc.Child("Users").PostAsync(new User() {
                    Email = savedFirebaseAuth.User.Email,
                    FirstName = FirstName.Text,
                    LastName = LastName.Text,
                    PhoneNumber = Phone.Text,
                    DisplayName = DisplayName.Text,
                    PhotoUrl = await StoreImages(file.GetStream())
                });
                Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                await DisplayAlert("Result!", "Successfully registered .", "Ok!");
                SaveButton.IsEnabled = false;

            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void UpdateButton_Clicked(object sender, EventArgs e)
        {
            if (file.GetStream() != null)
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
                try
                {
                    FirebaseClient fc = 
                        new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");
                    var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences
                        .Get("FirebaseRefreshToken", ""));
                    var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                    var toUpdate = (await fc
                    .Child("Users")
                    .OnceAsync<User>()).Where(item => item.Object.Email == savedFirebaseAuth.User.Email).FirstOrDefault();

                    User upUser = new User()
                    {
                        Email = savedFirebaseAuth.User.Email,
                        FirstName = FirstName.Text,
                        LastName = LastName.Text,
                        PhoneNumber = Phone.Text,
                        DisplayName = DisplayName.Text,
                        PhotoUrl = await StoreImages(file.GetStream())
                    };
                    await fc.Child("Users")
                        .Child(toUpdate.Key)
                        .PutAsync(upUser);



                    Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                    await DisplayAlert("Result!", "Successfully updated .", "Ok!");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                await DisplayAlert("Alert!", "Profile Image Required!", "OK!");
            }
            
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
              
            await CrossMedia.Current.Initialize();
            try
            {
                file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });
                if (file == null)
                    return;
                UploadImage.Source = ImageSource.FromStream(() =>
                {
                    var imageStream = file.GetStream();
                    return imageStream;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        public async Task<string> StoreImages(Stream imageStream)
        {
            var timeSpan = DateTime.Now.TimeOfDay;
            var storageImage = await new FirebaseStorage("kinda-filter.appspot.com")
                .Child("KindaFilterProfileImage")
                .Child(timeSpan+"img.jpg")
                .PutAsync(imageStream);
            string imgurl = storageImage;
            return imgurl;
        }

    }
}