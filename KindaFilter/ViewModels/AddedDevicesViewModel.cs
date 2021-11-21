using Firebase.Auth;
using Firebase.Database;
using KindaFilter.Models;
using KindaFilter.Services;
using MvvmHelpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace KindaFilter.ViewModels
{
    public class AddedDevicesViewModel : BaseViewModel
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
