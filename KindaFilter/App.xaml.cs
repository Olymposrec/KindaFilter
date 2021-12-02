using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.Diagnostics;
using Android.InputMethodServices;
using Android.Runtime;
using Android.Views;

[assembly:ExportFont("Birthstone-Regular.ttf")]

namespace KindaFilter
{
    public partial class App : Application
    {
        
        public App()
        {

            InitializeComponent();

            if (Device.RuntimePlatform==Device.Android || Device.RuntimePlatform==Device.UWP)
            {
                if (!string.IsNullOrEmpty(Preferences.Get("FirebaseRefreshToken","")))
                {
                    MainPage = new NavigationPage(new PagesFolder.TabbedPageBottom());
                }
                else
                {
                    MainPage = new NavigationPage(new PagesFolder.FirstEnterPage());
                }
            }
            else
            {
               
               MainPage = new NavigationPage(new PagesFolder.FirstEnterPage());
                
            }
            
            Device.SetFlags(new[] { "Brush_Experimental" });
            

        }

        protected override void OnStart()
        {
            Debug.WriteLine("OnStart PROGRAM BASLADI");

           
        }

        protected override void OnSleep()
        {

          
        }

        protected override void OnResume()
        {
            Debug.WriteLine("OnStart PROGRAM DEVAM EDIYOR");
        }

      

    }
}
