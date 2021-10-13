using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstEnterPage : ContentPage
    {
        public class CarouselImages
        {


            public ImageSource images { get; set; }
            public string messages { get; set; }

        }
        private ObservableCollection<CarouselImages> pageText;
        private ObservableCollection<CarouselImages> pageImages;
        public ObservableCollection<CarouselImages> PageImages
        {
            get { return pageImages; }
            set
            {
                pageImages = value;
                OnPropertyChanged();
            }

        }
        public ObservableCollection<CarouselImages> PageText
        {
            get { return pageText; }
            set
            {
                pageText = value;
                OnPropertyChanged();
            }

        }

      

        public bool IsFirstLaunch
        {
            get => Preferences.Get("IsFirstLaunch", true);
            set
            {
                Preferences.Set("IsFirstLaunch", false);
                
                OnPropertyChanged(nameof(IsFirstLaunch));
            }
        }

        public FirstEnterPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = this;
            PageImages = new ObservableCollection<CarouselImages>
            {
                new CarouselImages{images="family.png", messages="Family Friendly"},
                new CarouselImages{images="lock.png", messages="Safe And Lock"},
                new CarouselImages{images="shield.png",messages="For Safety"},
                new CarouselImages{images="social.png",messages="All Social Media"},
                new CarouselImages{images="worldwide.png",messages="World Wide Protection"}
            };
         
        }

        private void Button_SignUp(object sender, EventArgs e)
        {
            _ = Navigation.PushAsync(new LoginPage());
        }
    }
}