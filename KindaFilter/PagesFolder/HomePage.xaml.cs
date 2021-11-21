using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Firebase.Auth;
using Firebase.Database;
using KindaFilter.Models;
using System.Linq;
using KindaFilter.ViewModels;
using KindaFilter.Services;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public string WebAPIKey= "AIzaSyDQveJfg1KOrqz5_vBmj8WeQL4dFWs-umA";
        FirebaseClient client;
        public HomePage()
        {
            
            InitializeComponent();
            BindingContext = new UsersInfoView();
            GetProfileInformationAndRefreshToken();
        }

       private async void GetProfileInformationAndRefreshToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));

            try
           {
                client = new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");
                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                var result = (await client
                    .Child("Users")
                    .OnceAsync<User>()).FirstOrDefault(a => a.Object.Email == savedFirebaseAuth.User.Email);
                 
                
                if (result != null)
                {
                    
                    EMail.Text = result.Object.Email.ToString();
                    FirstName.Text = result.Object.FirstName.ToString();
                    Surname.Text = result.Object.LastName.ToString();
                    Phone.Text = result.Object.PhoneNumber.ToString();
                    UserName.Text = result.Object.DisplayName.ToString();
                    ProfileImage.Source = result.Object.PhotoUrl.ToString();

                    Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                }else
                {
                    EMail.Text = savedFirebaseAuth.User.Email;
                    UserName.Text = savedFirebaseAuth.User.DisplayName;
                    Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                }
               
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Navigation.PushAsync(new LoginPage());
                await App.Current.MainPage.DisplayAlert("Alert", "something went wrong", "OK!");
            }
        }

        private void Button_LogOut(object sender, EventArgs e)
        {
            Preferences.Remove("FirebaseRefreshToken");
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}