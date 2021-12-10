using KindaFilter.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace KindaFilter.WebBlockerService.WebBlocker
{
    public class ReadSitesFromXML
    {
        DBFirebase services;

        //One Time Code
        //public async void GetSitesFromXML()
        //{
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(@"blockedsites.xml");

        //    XmlNodeList nodes = doc.SelectNodes("url-list/url-info/url");
        //    services = new DBFirebase();
            
        //    foreach (XmlNode node in nodes)
        //    {
        //        string siteUrl =node.InnerText.ToString();
        //        string siteID = node.PreviousSibling.InnerText.ToString();
        //        string ProxyID = "0.0.0.0";
        //        string HttpsLink = "https://www." + siteUrl;
        //        string HttpLink = "http://www." + siteUrl;
        //        await services.AddBlockedSite(siteID, ProxyID, siteUrl, "www." + siteUrl, HttpLink, HttpsLink);

        //    }
        //}
    }
}
