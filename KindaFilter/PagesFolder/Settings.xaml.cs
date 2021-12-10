using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using KindaFilter.Models;
using KindaFilter.Services;
using Newtonsoft.Json;
using System;
using System.Linq;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        DBFirebase services;
        public string WebAPIKey = "AIzaSyDQveJfg1KOrqz5_vBmj8WeQL4dFWs-umA";
        public Settings()
        {
            InitializeComponent();
            services = new DBFirebase();
            SetUserSettings();
        }
        public async void SetUserSettings()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));


            try
            {
                FirebaseClient fc = new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");

                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                var result = (await fc
                   .Child("UsersSettings")
                   .OnceAsync<UsersSettings>()).FirstOrDefault(a => a.Object.UserEmail == savedFirebaseAuth.User.Email);

                if (result != null)
                {
                    UsomProtect.IsToggled = bool.Parse(result.Object.UsomProtect);
                    OwnProtect.IsToggled = bool.Parse(result.Object.OwnProtect);
                    CutTheNetConnect.IsToggled = bool.Parse(result.Object.CutTheNetConnect);
                    TellMeIfDangerSite.IsToggled = bool.Parse(result.Object.TellMeIfDangerSite);
                    SaveRecords.IsToggled = bool.Parse(result.Object.SaveRecords);
                    TellMeIfSlang.IsToggled = bool.Parse(result.Object.TellMeIfSlang);
                    BanSpecificWords.IsToggled = bool.Parse(result.Object.BanSpecificWords);
                    TellMeIfUseBanWords.IsToggled = bool.Parse(result.Object.TellMeIfUseBanWords);
                    SendDailyReport.IsToggled = bool.Parse(result.Object.SendDailyReport);
                    SendEmailReport.IsToggled = bool.Parse(result.Object.SendEmailReport);
                    SendWeaklyEmailReport.IsToggled = bool.Parse(result.Object.SendWeaklyEmailReport);
                    SaveButton.Text = "Update";
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Navigation.PushAsync(new LoginPage());
                await App.Current.MainPage.DisplayAlert("Alert", "something went wrong", "OK!");
            }
        }
        private async void SaveButton_Clicked(object sender, System.EventArgs e)
        {

            if (SaveButton.Text == "Update")
            {
                await services.UpdateUserSettings(UsomProtect, OwnProtect, CutTheNetConnect
                                    , TellMeIfDangerSite, TellMeIfSlang, SaveRecords, TellMeIfUseBanWords
                                    , BanSpecificWords, SendDailyReport, SendEmailReport, SendWeaklyEmailReport);
            }
            else if (SaveButton.Text == "Save")
            {
                SaveButton.Text = "Update";
                await services.SaveUserSettings(UsomProtect, OwnProtect, CutTheNetConnect
                                     , TellMeIfDangerSite, TellMeIfSlang, SaveRecords, TellMeIfUseBanWords
                                     , BanSpecificWords, SendDailyReport, SendEmailReport, SendWeaklyEmailReport);
            }

        }

        private void AddSite_Clicked(object sender, EventArgs e)
        {
            Navigation.ShowPopup(new PopUpAddSite());
        }
    }
}