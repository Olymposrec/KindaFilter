
using KindaFilter.Models;
using KindaFilter.ViewModels;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace KindaFilter.PagesFolder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeviceManagement : ContentPage
    {
        public DeviceManagement()
        {
            InitializeComponent();
            BindingContext = new AddedDevicesViewModel();
        }

        private void ShowPopUp_AddDevice_Button(object sender, System.EventArgs e)
        {
            
            Navigation.ShowPopup(new PopUpAddDevice());
        }
    }
}