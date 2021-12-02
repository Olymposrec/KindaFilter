
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using KindaFilter.Models;
using KindaFilter.PagesFolder;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KindaFilter.Services
{
    public class DBFirebase
    {
        FirebaseClient client;
        public string WebAPIKey = "AIzaSyDQveJfg1KOrqz5_vBmj8WeQL4dFWs-umA";

        public DBFirebase()
        {
            client = new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");
        }
        public ObservableCollection<AddAsChildRequest> GetDevices()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            var savedFirebaseAuth = GetToken();
            var refreshedContent =  authProvider.RefreshAuthAsync(savedFirebaseAuth);
            var addedChild = client
              .Child("AddAsChildRequest")
              .AsObservable<AddAsChildRequest>()
              .AsObservableCollection();
            
           
            if (addedChild != null)
            {
                return addedChild;
            }
            else
            {
                return null;
            }
        }
        private FirebaseAuth GetToken()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            var savedFirebaseAuth = JsonConvert.DeserializeObject<FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
            var refreshedContent = authProvider.RefreshAuthAsync(savedFirebaseAuth);
            return savedFirebaseAuth;
        }
        public ObservableCollection<UsersInfo> GetUsersInfo()
        {
            var usersInfo = client
              .Child("Users")
              .AsObservable<UsersInfo>()
              .AsObservableCollection();

            if (usersInfo != null)
                return usersInfo;
            else
                return null;
        }
        public async Task AddUsersInfo(string displayName,string userEmail,string firstName, string lastName, string phoneNumber, string image)
        {
            UsersInfo user = new UsersInfo() {DisplayName = displayName, UserEmail=userEmail,FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber, ProfileImage = image };
            
            await client
                .Child("Users")
                .PostAsync(user);
        }
        public async Task<FirebaseObject<User>> SetProfileInfo()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));

            try
            {
                FirebaseClient fc = new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");

                var savedFirebaseAuth = GetToken();
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                var result = (await fc
                   .Child("Users")
                   .OnceAsync<User>()).FirstOrDefault(a => a.Object.Email == savedFirebaseAuth.User.Email);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PushAsync(new LoginPage());
                return null;
            }
        }
        public async Task UpdateUserProfile(string FirstName, string LastName, string Phone, string DisplayName, Plugin.Media.Abstractions.MediaFile file)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                FirebaseClient fc =
                    new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");
                var savedFirebaseAuth = GetToken();
                var refreshedContent = authProvider.RefreshAuthAsync(savedFirebaseAuth);
                var toUpdate = (await fc
                .Child("Users")
                .OnceAsync<User>()).Where(item => item.Object.Email == savedFirebaseAuth.User.Email).FirstOrDefault();

                User upUser = new User()
                {
                    Email = savedFirebaseAuth.User.Email,
                    FirstName = FirstName,
                    LastName = LastName,
                    PhoneNumber = Phone,
                    DisplayName = DisplayName,
                    PhotoUrl = await StoreImages(file.GetStream())
                };
                await fc.Child("Users")
                    .Child(toUpdate.Key)
                    .PutAsync(upUser);



                Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                await App.Current.MainPage.DisplayAlert("Result!", "Successfully updated .", "Ok!");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task SaveUserProfile(string FirstName, string LastName, string Phone, string DisplayName, Plugin.Media.Abstractions.MediaFile file)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                FirebaseClient fc =
                    new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");
                var savedFirebaseAuth = GetToken();
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                var result = await fc.Child("Users").PostAsync(new User()
                {
                    Email = savedFirebaseAuth.User.Email,
                    FirstName = FirstName,
                    LastName = LastName,
                    PhoneNumber = Phone,
                    DisplayName = DisplayName,
                    PhotoUrl = await StoreImages(file.GetStream())
                });
                Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Result!", "Successfully registered .", "Ok!");
                

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private async Task<string> StoreImages(Stream imageStream)
        {
            var timeSpan = DateTime.Now.TimeOfDay;
            var storageImage = await new FirebaseStorage("kinda-filter.appspot.com")
                .Child("KindaFilterProfileImage")
                .Child(timeSpan + "img.jpg")
                .PutAsync(imageStream);
            string imgurl = storageImage;
            return imgurl;
        }
        public async Task<bool> CheckForRequest(string Email)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));

            try
            {
                FirebaseClient fc = new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");

                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                var result = (await fc
                   .Child("AddAsChildRequest")
                   .OnceAsync<AddAsChildRequest>()).FirstOrDefault(a => a.Object.ChildEmail == Email);

                if (result.Object.RequestWaiting == true)
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
                return false;

            }
        }
        public async Task AddChildDevice(bool isTrue, string mail)
        {
           
            if (isTrue == true)
            {
                await App.Current.MainPage.DisplayAlert("Result", "Request Already Sent!", "OK!");
            }
            else if (isTrue == false || isTrue.Equals(null))
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
                        ChildEmail = mail,
                        isChild = false,
                        RequestWaiting = true
                    });
                    Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                    await App.Current.MainPage.DisplayAlert("Result", "Request Sent!", "OK!");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await App.Current.MainPage.DisplayAlert("Result", "Something Went Wrong", "OK!");
                }
            }
        }
        public async Task<FirebaseObject<UsersSettings>> SetUserSettings()
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

                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task UpdateUserSettings(Switch UsomProtect, Switch OwnProtect, Switch CutTheNetConnect
            , Switch TellMeIfDangerSite, Switch SaveRecords, Switch TellMeIfSlang, Switch BanSpecificWords, Switch TellMeIfUseBanWords
            , Switch SendDailyReport, Switch SendEmailReport, Switch SendWeaklyEmailReport)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
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
                    SaveRecords = SaveRecords.IsToggled.ToString(),
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
                await App.Current.MainPage.DisplayAlert("Result!", "Successfully updated .", "Ok!");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task SaveUserSettings(Switch UsomProtect, Switch OwnProtect, Switch CutTheNetConnect
            , Switch TellMeIfDangerSite, Switch SaveRecords, Switch TellMeIfSlang, Switch BanSpecificWords, Switch TellMeIfUseBanWords
            , Switch SendDailyReport, Switch SendEmailReport, Switch SendWeaklyEmailReport)
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            try
            {
                FirebaseClient fc = new FirebaseClient("https://kinda-filter-default-rtdb.europe-west1.firebasedatabase.app/");

                var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
                var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
                var resultUsersSettings = await fc.Child("UsersSettings").PostAsync(new UsersSettings()
                {
                    UserEmail = savedFirebaseAuth.User.Email,
                    UsomProtect = UsomProtect.IsToggled.ToString(),
                    OwnProtect = OwnProtect.IsToggled.ToString(),
                    CutTheNetConnect = CutTheNetConnect.IsToggled.ToString(),
                    TellMeIfDangerSite = TellMeIfDangerSite.IsToggled.ToString(),
                    SaveRecords = SaveRecords.IsToggled.ToString(),
                    TellMeIfSlang = TellMeIfDangerSite.IsToggled.ToString(),
                    BanSpecificWords = BanSpecificWords.IsToggled.ToString(),
                    TellMeIfUseBanWords = TellMeIfUseBanWords.IsToggled.ToString(),
                    SendDailyReport = SendDailyReport.IsToggled.ToString(),
                    SendEmailReport = SendEmailReport.IsToggled.ToString(),
                    SendWeaklyEmailReport = SendWeaklyEmailReport.IsToggled.ToString()
                });
                Preferences.Set("FirebaseRefreshToken", JsonConvert.SerializeObject(refreshedContent));
                await App.Current.MainPage.DisplayAlert("Result!", "Successfully registered .", "Ok!");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
