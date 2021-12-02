using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        MediaFile file;
        DBFirebase services;
        public string WebAPIKey = "AIzaSyDQveJfg1KOrqz5_vBmj8WeQL4dFWs-umA";
        public Profile()
        {
            InitializeComponent();
            GetProfileInfo();
            services = new DBFirebase();
        }

        public async void GetProfileInfo()
        {
            var savedFirebaseAuth = GetToken();
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
            FirebaseObject<User> fireBaseObject = await services.SetProfileInfo();
            if (fireBaseObject != null)
            {
                FirstName.Text = fireBaseObject.Object.FirstName.ToString();
                LastName.Text = fireBaseObject.Object.LastName.ToString();
                Phone.Text = fireBaseObject.Object.PhoneNumber.ToString();
                DisplayName.Text = fireBaseObject.Object.DisplayName.ToString();
                UploadImage.Source = fireBaseObject.Object.PhotoUrl.ToString();
                UploadImage.Aspect = Aspect.Fill;
                Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                SaveButton.IsEnabled = false;
            }
        }
        private async void Button_SaveData(object sender, System.EventArgs e)
        {
            if (file.GetStream() != null)
            {
                await services.SaveUserProfile(FirstName.Text.ToString(), LastName.Text.ToString(), Phone.Text.ToString(), DisplayName.Text.ToString(), file);
            }
            else
            {
                await DisplayAlert("Alert!", "Profile Image Required!", "OK!");
            }
        }

        private async void UpdateButton_Clicked(object sender, EventArgs e)
        {
            if (file.GetStream() != null)
            {
                await services.UpdateUserProfile(FirstName.Text.ToString(), LastName.Text.ToString(), Phone.Text.ToString(), DisplayName.Text.ToString(), file);
  
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

        private FirebaseAuth GetToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            var savedFirebaseAuth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
            var refreshedContent = authProvider.RefreshAuthAsync(savedFirebaseAuth);
            return savedFirebaseAuth;
        }
    }
}