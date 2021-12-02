using KindaFilter.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Firebase.Auth;
using Newtonsoft.Json;
using Xamarin.Essentials;
using KindaFilter.Services;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private readonly IAuth auth;
        DBFirebase services;
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            auth = DependencyService.Get<IAuth>();
            services = new DBFirebase();
        }

        private async void Button_Signup(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUp());
        }

        private async void Button_LogIn(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                if (Email.Text == "" || Password.Text == "")
                {
                    await DisplayAlert("Input Failed", "Invalid Login, try again", "OK");

                }
                else
                {
                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(services.WebAPIKey.ToString()));
                    try
                    {
                        var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email.Text,Password.Text);
                        
                        var content = await auth.GetFreshAuthAsync();
                        
                        var serializeContent = JsonConvert.SerializeObject(content);
                        
                        Preferences.Set("FirebaseRefreshToken", serializeContent);
                        await Navigation.PushAsync(new TabbedPageBottom());
                    }catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await App.Current.MainPage.DisplayAlert("Authentication Failed", "Invalid Login, try again", "OK");
                    }
                    
                }
            }
            else if (Device.RuntimePlatform == Device.UWP)
            {
                if(Email.Text!=null && Password.Text!=null)
                {
                    var authProvider = new FirebaseAuthProvider(new FirebaseConfig(services.WebAPIKey.ToString()));
                    try
                    {
                        var auth = await authProvider.SignInWithEmailAndPasswordAsync(Email.Text,Password.Text);
                        var content = await auth.GetFreshAuthAsync();
                        var serializeContent = JsonConvert.SerializeObject(content);
                        Preferences.Set("FirebaseRefreshToken", serializeContent);
                        await Navigation.PushAsync(new TabbedPageBottom());
                    }catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        await App.Current.MainPage.DisplayAlert("Authentication Failed", "Invalid Login, try again", "OK");
                    }
                    //await Navigation.PushAsync(new TabbedPageBottom());
                }
                else
                {
                    await DisplayAlert("Fields cannot be left blank !", "Log In Failed", "OK");
                }
            }
            else
            {
                await DisplayAlert("The platform on which you use the program is not supported.", "OS Problem.", "OK");
            }
           
          
            
        }

      
    }
}