using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Firebase.Auth;
using Firebase.Database;
using System.Linq;
using KindaFilter.ViewModels;
using KindaFilter.Services;
using KindaFilter.WebBlockerService.WebBlocker;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        DBFirebase services;
        public HomePage()
        {
            
            InitializeComponent();
            services = new DBFirebase();
            GetProfileInfo();

            //One Time Code
            //ReadSitesFromXML xx = new ReadSitesFromXML();
            //xx.GetSitesFromXML();
        }

       private async void GetProfileInfo()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(services.WebAPIKey.ToString()));
            var savedFirebaseAuth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
            var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
            FirebaseObject<User> fireBaseObject = await services.SetProfileInfo();

            if (fireBaseObject != null)
            {
                EMail.Text = fireBaseObject.Object.Email.ToString();
                FirstName.Text = fireBaseObject.Object.FirstName.ToString();
                Surname.Text = fireBaseObject.Object.LastName.ToString();
                Phone.Text = fireBaseObject.Object.PhoneNumber.ToString();
                UserName.Text = fireBaseObject.Object.DisplayName.ToString();
                ProfileImage.Source = fireBaseObject.Object.PhotoUrl.ToString();

                Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
            }else
            {
                EMail.Text = savedFirebaseAuth.User.Email;
                UserName.Text = savedFirebaseAuth.User.DisplayName;
                Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
            }   
          
        }
        private void Button_LogOut(object sender, EventArgs e)
        {
            Preferences.Remove("FirebaseRefreshToken");
            App.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}