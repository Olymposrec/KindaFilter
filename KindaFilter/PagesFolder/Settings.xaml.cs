

using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using KindaFilter.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public string WebAPIKey = "AIzaSyDQveJfg1KOrqz5_vBmj8WeQL4dFWs-umA";
        public Settings()
        {
            InitializeComponent();
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

            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));


            try
            {
                FirebaseClient fc = new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");

                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                var result = (await fc
                   .Child("Users")
                   .OnceAsync<UsersSettings>()).FirstOrDefault(a => a.Object.UserEmail == savedFirebaseAuth.User.Email);

                if (result != null)
                {
                    GetUserData();
                }
                else
                {
                    GetUserData();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Navigation.PushAsync(new LoginPage());
                await App.Current.MainPage.DisplayAlert("Alert", "something went wrong", "OK!");
            }

        }

        public async void GetUserData()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            if (SaveButton.Text == "Update")
            {
                
                try
                {
                    FirebaseClient fc = new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");

                    var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                    var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                    var toUpdate = (await fc
                    .Child("UsersSettings")
                    .OnceAsync<UsersSettings>()).Where(item => item.Object.UserEmail == savedFirebaseAuth.User.Email).FirstOrDefault();

                    UsersSettings upSettings = new UsersSettings()
                    {
                        UserEmail = savedFirebaseAuth.User.Email,
                        UsomProtect = UsomProtect.IsToggled.ToString(),
                        OwnProtect = OwnProtect.IsToggled.ToString(),
                        CutTheNetConnect = CutTheNetConnect.IsToggled.ToString(),
                        TellMeIfDangerSite = TellMeIfDangerSite.IsToggled.ToString(),
                        SaveRecords = TellMeIfDangerSite.IsToggled.ToString(),
                        TellMeIfSlang = TellMeIfDangerSite.IsToggled.ToString(),
                        BanSpecificWords = BanSpecificWords.IsToggled.ToString(),
                        TellMeIfUseBanWords = TellMeIfUseBanWords.IsToggled.ToString(),
                        SendDailyReport = SendDailyReport.IsToggled.ToString(),
                        SendEmailReport = SendEmailReport.IsToggled.ToString(),
                        SendWeaklyEmailReport = SendWeaklyEmailReport.IsToggled.ToString()
                    };
                    await fc.Child("UsersSettings")
                        .Child(toUpdate.Key)
                        .PutAsync(upSettings);



                    Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                    await DisplayAlert("Result!", "Successfully updated .", "Ok!");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else if (SaveButton.Text == "Save")
            {
                try
                {
                    SaveButton.Text = "Update";
                    FirebaseClient fc = new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");

                    var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                    var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                    var getMail = (await fc
                  .Child("Users")
                  .OnceAsync<User>()).Where(item => item.Object.Email == savedFirebaseAuth.User.Email).FirstOrDefault();

                    var resultUsersSettings = await fc.Child("UsersSettings").PostAsync(new UsersSettings()
                    {
                        UserEmail = getMail.Object.Email,
                        UsomProtect = UsomProtect.IsToggled.ToString(),
                        OwnProtect = OwnProtect.IsToggled.ToString(),
                        CutTheNetConnect = CutTheNetConnect.IsToggled.ToString(),
                        TellMeIfDangerSite = TellMeIfDangerSite.IsToggled.ToString(),
                        SaveRecords = TellMeIfDangerSite.IsToggled.ToString(),
                        TellMeIfSlang = TellMeIfDangerSite.IsToggled.ToString(),
                        BanSpecificWords = BanSpecificWords.IsToggled.ToString(),
                        TellMeIfUseBanWords = TellMeIfUseBanWords.IsToggled.ToString(),
                        SendDailyReport = SendDailyReport.IsToggled.ToString(),
                        SendEmailReport = SendEmailReport.IsToggled.ToString(),
                        SendWeaklyEmailReport = SendWeaklyEmailReport.IsToggled.ToString()
                    });
                    Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                    await DisplayAlert("Result!", "Successfully registered .", "Ok!");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}