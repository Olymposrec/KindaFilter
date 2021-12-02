using KindaFilter.Models;
using KindaFilter.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace KindaFilter.ViewModels
{
    public class AddedDevicesViewModel : BindableObject
    {
        public string UserMail { get; set; }
        public string ChildEmail { get; set; }
        public bool isChild { get; set; }
        public bool RequestWaiting { get; set; }

        private DBFirebase services;

        private ObservableCollection<AddAsChildRequest> addedDevices = new ObservableCollection<AddAsChildRequest>();
        public ObservableCollection<AddAsChildRequest> NewAddedDevices
        {
            get { return addedDevices; }
            set
            {
                addedDevices = value;
                OnPropertyChanged();
            }
        }

        public AddedDevicesViewModel()
        {
            services = new DBFirebase();
            NewAddedDevices = services.GetDevices();
        }

        
    }
}
