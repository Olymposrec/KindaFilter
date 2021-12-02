
using KindaFilter.Services;
using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace KindaFilter.PagesFolder
{
    public partial class PopUpAddDevice : Popup
    {
        public bool isRequestWaiting;
        DBFirebase services;
        public PopUpAddDevice()
        {
            InitializeComponent();
            
        }
        protected override object GetLightDismissResult()
        {
            return "Closed by LigtDismiss";
        }
        private void ClosePopUp_Click(object sender, EventArgs e)
        {
            Dismiss("Closed by button");
        }

        private async void AddChildDevice_ClickAsync(object sender, EventArgs e)
        {
            services = new DBFirebase();
            if (Email.Text != "")
            {
                isRequestWaiting = await services.CheckForRequest(Email.Text);
                await services.AddChildDevice(isRequestWaiting, Email.Text);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert("Null Value!", "Do not enter a null value!", "OK!");
            }
        }
    }
}