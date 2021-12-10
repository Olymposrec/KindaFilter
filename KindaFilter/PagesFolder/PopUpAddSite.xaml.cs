using Firebase.Auth;
using Firebase.Database;
using KindaFilter.Services;
using Newtonsoft.Json;
using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpAddSite : Popup
    {
        DBFirebase services;
        public string WebAPIKey = "AIzaSyDQveJfg1KOrqz5_vBmj8WeQL4dFWs-umA";
        public PopUpAddSite()
        {
            InitializeComponent();
        }
        protected override object GetLightDismissResult()
        {
            return "Closed by LigtDismiss";
        }
        private async void AddSite_Click(object sender, EventArgs e)
        {
            services = new DBFirebase();
            if (BlockSite.Text != "")
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
                FirebaseClient fc =
                        new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");
                var savedFirebaseAuth = GetToken();
                var refreshedContent = authProvider.RefreshAuthAsync(savedFirebaseAuth);

                string siteUrl = BlockSite.Text;
                string siteID = savedFirebaseAuth.User.Email;
                string ProxyID = "0.0.0.0";
                string HttpsLink = "https://www." + siteUrl;
                string HttpLink = "http://www." + siteUrl;
                await services.AddUserBlockedSite(siteID,ProxyID,siteUrl,"www."+siteUrl,HttpsLink,HttpLink);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Null Value!", "Do not enter a null value!", "OK!");
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Dismiss("Closed by button");
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