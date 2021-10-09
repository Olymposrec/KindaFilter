using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly:ExportFont("Birthstone-Regular.ttf")]

namespace KindaFilter
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new PagesFolder.FirstEnterPage());
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
