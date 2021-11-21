using KindaFilter.Models;
using KindaFilter.Services;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace KindaFilter.ViewModels
{
    public class UsersInfoView: BindableObject
    {
        public string Uid { get; set; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }

        public string Img { get; set; }

        private DBFirebase services;

        public ICommand  AddUserInfoCommand { get; set; }

        private ObservableCollection<UsersInfo> _usersInfos = new ObservableCollection<UsersInfo>();
        public ObservableCollection<UsersInfo> UsersInfos
        {
            get { return _usersInfos; }
            set
            {
                _usersInfos = value;
                OnPropertyChanged();
            }
        }

        public UsersInfoView()
        {
            services = new DBFirebase();
            UsersInfos = services.getUsersInfo();
        }

        public async Task AddUsersInfoAsync(string DisplayName,string UserEmail,string FirstName, string LastName, string PhoneNumber, string Image)
        {
            await services.AddUsersInfo(DisplayName, UserEmail, FirstName, LastName, PhoneNumber,Image);
        }
    }
}
