using Firebase.Auth;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUp : ContentPage
    {
        public string WebAPIKey = "AIzaSyDQveJfg1KOrqz5_vBmj8WeQL4dFWs-umA";
       
        public SignUp()
        {
            InitializeComponent();
        }

        private async void SingUp_Button(object sender, EventArgs e)
        {
           

            if(Email.Text == "" || Pass.Text == "")
            {
                await DisplayAlert("Input Failed", "Invalid Login, try again", "OK");

            }
            else if(Pass.Text!=Confirm.Text)
            {
                await DisplayAlert("Validation!", "Password Not match", "Ok");
            }
            else
            {
                    var authProvider = new Firebase.Auth.FirebaseAuthProvider(new FirebaseConfig(WebAPIKey));
                try
                {
                    var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(Email.Text, Pass.Text,DisplayName.Text);
                    string getToken = auth.FirebaseToken;
                    await App.Current.MainPage.DisplayAlert("Alert", "Succesfuly!", "OK");
                    await Navigation.PushAsync(new LoginPage());

                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await App.Current.MainPage.DisplayAlert("Alert", "Something Went Wrong!", "OK");
                }
            }
        }
    }
}