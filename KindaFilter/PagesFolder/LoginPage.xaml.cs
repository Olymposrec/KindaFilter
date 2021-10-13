using GalaSoft.MvvmLight.Views;
using KindaFilter.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
        }

        void Button_SignUp(object sender, EventArgs e)
        {
            Navigation.PushAsync(new KindaFilter.PagesFolder.SignUp());
        }

        private void Button_LogIn(object sender, EventArgs e)
        {
            if (Email.Text != "admin@admin.com" || Password.Text!= "admin")
            {
                DisplayAlert("Error", "Invalid Login, try again", "OK");
            }
            else
            {
                Navigation.PushAsync(new KindaFilter.PagesFolder.HomePage());
            }
        }

      
    }
}