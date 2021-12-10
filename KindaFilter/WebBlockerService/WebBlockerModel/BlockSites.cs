using System;
using System.Collections.Generic;
using System.Text;

namespace KindaFilter.WebBlockerService.WebBlockerModel
{
    public class BlockSites
    {
        public string SiteID { get; set; }
        public string ProxyID { get; set; }
        public string DomainLink { get; set; }
        public string FullLink { get; set; }
        public string HttpLink { get; set; }
        public string HttpsLink { get; set; }

    }
}
