using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

[assembly:ExportFont("Birthstone-Regular.ttf")]

namespace KindaFilter
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            KindaFilter.PagesFolder.FirstEnterPage isItFirst = new PagesFolder.FirstEnterPage();

            if (isItFirst.IsFirstLaunch==true)
            {
                MainPage = new NavigationPage(new PagesFolder.FirstEnterPage());
            }
            else
            {
                MainPage = new NavigationPage(new PagesFolder.LoginPage());
            }

        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
