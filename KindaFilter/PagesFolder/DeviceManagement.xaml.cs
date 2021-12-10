using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using KindaFilter.Models;
using KindaFilter.Services;
using KindaFilter.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceManagement : ContentPage
    {
        DBFirebase services;
        public string WebAPIKey = "AIzaSyDQveJfg1KOrqz5_vBmj8WeQL4dFWs-umA";
        public DeviceManagement()
        {
            InitializeComponent();
            BindingContext = new AddedDevicesViewModel();
            
        }

        private FirebaseAuth GetToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            var savedFirebaseAuth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
            var refreshedContent = authProvider.RefreshAuthAsync(savedFirebaseAuth);
            return savedFirebaseAuth;
        }

        private void ShowPopUp_AddDevice_Button(object sender, System.EventArgs e)
        {
             Navigation.ShowPopup(new PopUpAddDevice());
        }
        private void Accept_Button(object sender, EventArgs e)
        {
            Button button = (Button) sender;
            StackLayout listViewItem = (StackLayout)button.Parent;

            Label label_childemail = (Label)listViewItem.Children[1];
            Label label_usermail = (Label)listViewItem.Children[3];
            UpdateChild(label_usermail.Text.ToString(), label_childemail.Text.ToString());
        }

        private async void UpdateChild(string label_usermail, string label_childemail)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                FirebaseClient fc =
                        new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");
                var savedFirebaseAuth = GetToken();
                var refreshedContent = authProvider.RefreshAuthAsync(savedFirebaseAuth);
                var toUpdate = (await fc
                .Child("AddAsChildRequest")
                .OnceAsync<AddAsChildRequest>())
                .FirstOrDefault(item => item.Object.UserMail == label_usermail.ToString() && item.Object.ChildEmail == label_childemail.ToString());

                AddAsChildRequest upChild = new AddAsChildRequest()
                {
                    ChildEmail = label_childemail.ToString(),
                    UserMail = label_usermail.ToString(),
                    RequestWaiting = false,
                    isChild = true
                };

                await fc.Child("AddAsChildRequest")
                    .Child(toUpdate.Key)
                    .PutAsync(upChild);
                Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void Decline_Button(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            StackLayout listViewItem = (StackLayout)button.Parent;

            Label label_childemail = (Label)listViewItem.Children[1];
            Label label_usermail = (Label)listViewItem.Children[3];

            DeleteChild(label_usermail.Text.ToString(),label_childemail.Text.ToString());
        }

        private async void DeleteChild(string label_usermail, string label_childemail)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                FirebaseClient fc =
                    new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");
                var result = (await fc
                    .Child("AddAsChildRequest")
                    .OnceAsync<AddAsChildRequest>()).FirstOrDefault(a => a.Object.ChildEmail == label_childemail && a.Object.UserMail == label_usermail);

                await fc.Child("AddAsChildRequest")
                  .Child(result.Key).DeleteAsync();
                await App.Current.MainPage.DisplayAlert("Result", "Child Deleted", "OK!");

            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Result", "Something Went Wrong", "OK!");
            }
        }
    }
}