using KindaFilter.Services;
using KindaFilter.WebBlockerService.WebBlockerModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace KindaFilter.ViewModels
{
    public class BlockSitesViewModel : BindableObject
    {
        public string SiteID { get; set; }
        public string ProxyID { get; set; }
        public string DomainLink { get; set; }
        public string FullLink { get; set; }
        public string HttpLink { get; set; }
        public string HttpsLink { get; set; }

        private DBFirebase services;

        private ObservableCollection<BlockSites> blockedSites= new ObservableCollection<BlockSites>();
        public ObservableCollection<BlockSites> NewBlockedSites
        {
            get { return blockedSites; }
            set
            {
                blockedSites = value;
                OnPropertyChanged();
            }
        }

        public BlockSitesViewModel()
        {
            services = new DBFirebase();
            NewBlockedSites = services.GetBlockSites();
        }
    }
}
