using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
        }
        
        private ObservableCollection<CarouselImages> userCollection;
        public ObservableCollection<CarouselImages> UserCollection
        {
            get { return userCollection; }
            set
            {
                userCollection = value;
                OnPropertyChanged();
            }
            
        }
        public FirstEnterPage()
        {
            InitializeComponent();

            BindingContext = this;
            UserCollection = new ObservableCollection<CarouselImages>
            {
                new CarouselImages{images="family.png"},
                new CarouselImages{images="lock.png"},
                new CarouselImages{images="shield.png"},
                new CarouselImages{images="social.png"},
                new CarouselImages{images="worldwide.png"}
            };
        }

        private void Button_Clicked(object sender, EventArgs e)
        {

        }
    }
}