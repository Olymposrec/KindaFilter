using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using KindaFilter.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopUpAddDevice : Popup
    {
        public string WebAPIKey = "AIzaSyDQveJfg1KOrqz5_vBmj8WeQL4dFWs-umA";
        public bool isRequestWaiting;
        public PopUpAddDevice()
        {
            InitializeComponent();
        }

        private void ClosePopUp_Click(object sender, EventArgs e)
        {
            Dismiss(null);
        }

        public async Task<bool> CheckForRequest()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));

            try
            {
                FirebaseClient fc = new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");

                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                var result = (await fc
                   .Child("AddAsChildRequest")
                   .OnceAsync<AddAsChildRequest>()).FirstOrDefault(a => a.Object.ChildEmail == Email.Text);

                if (result.Object.RequestWaiting==true)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await App.Current.MainPage.DisplayAlert("Result", "Something Went Wrong", "OK!");
                return false;

            }
        }
        private async void AddChildDevice_ClickAsync(object sender, EventArgs e)
        {
            isRequestWaiting= await CheckForRequest();
            if (isRequestWaiting==true)
            {
                await App.Current.MainPage.DisplayAlert("Result", "Request Already Sent!", "OK!");
            }
            else
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
                try
                {
                    FirebaseClient fc =
                        new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");
                    var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth
                        .FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                    var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                    var result = await fc.Child("AddAsChildRequest").PostAsync(new AddAsChildRequest()
                    {
                        UserMail = savedFirebaseAuth.User.Email,
                        ChildEmail = Email.Text,
                        isChild = false,
                        RequestWaiting = true
                    });
                    Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                    Dismiss(null);
                    await App.Current.MainPage.DisplayAlert("Result", "Request Sent!", "OK!");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await App.Current.MainPage.DisplayAlert("Result", "Something Went Wrong", "OK!");
                }
            }

           
        }
    }
}