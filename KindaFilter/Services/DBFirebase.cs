using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using KindaFilter.Models;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;

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
                var result = client
                   .Child("AddAsChildRequest")
                   .AsObservable<AddAsChildRequest>()
                   .AsObservableCollection();
                return result;
            
        }
 
        public ObservableCollection<UsersInfo> getUsersInfo()
        {
            var usersInfo = client
              .Child("Users")
              .AsObservable<UsersInfo>()
              .AsObservableCollection();
            return usersInfo;
        }
        public async Task GetCurrentUserProfile()
        {
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
            var savedFirebaseAuth = JsonConvert.DeserializeObject<Firebase.Auth.FirebaseAuth>(Preferences.Get("FirebaseRefreshToken", ""));
            var refreshedContent = await authProvider.RefreshAuthAsync(savedFirebaseAuth);
            var result = (await client
                .Child("Users")
                .OnceAsync<UsersInfo>())
                .Where(item => item.Object.UserEmail == savedFirebaseAuth.User.Email).Select(item =>
                new UsersInfo
                {
                    UserEmail = item.Object.UserEmail,
                    FirstName = item.Object.FirstName,
                    LastName = item.Object.LastName,
                    PhoneNumber = item.Object.PhoneNumber,
                    DisplayName = item.Object.FirstName,
                    ProfileImage = item.Object.ProfileImage
                }).ToList();
        }
        public async Task AddUsersInfo(string displayName,string userEmail,string firstName, string lastName, string phoneNumber, string image)
        {
            UsersInfo user = new UsersInfo() {DisplayName = displayName, UserEmail=userEmail,FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber, ProfileImage = image };
            
            await client
                .Child("Users")
                .PostAsync(user);
        }

    }
}
