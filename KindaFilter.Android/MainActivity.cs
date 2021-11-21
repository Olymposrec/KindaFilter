using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Firebase;
using Android.Views;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace KindaFilter.Droid
{
    [Activity(Label = "KindaFilter", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait, Icon = "@mipmap/icon", Theme = "@style/MainTheme", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            FirebaseApp.InitializeApp(Application.Context);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
          
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        //public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        //{
        //    Console.WriteLine("A key was pressed");
        //    if (keyCode == Keycode.A)
        //    {
        //        Console.WriteLine("A key was pressed");
        //    }
        //    return base.OnKeyDown(keyCode, e);
        //}
        public override bool DispatchKeyEvent(Android.Views.KeyEvent e)
        {
           
            int keycode = e.GetHashCode();
            int keyunicode = e.GetUnicodeChar(e.MetaState);
            char character = (char)keyunicode;

            Console.WriteLine("CHARACTER KEY:" + character, "KEYCODE:" + keycode);
            return base.DispatchKeyEvent(e);
        }
      
    }
}