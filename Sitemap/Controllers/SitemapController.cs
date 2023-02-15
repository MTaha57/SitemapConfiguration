using Sitemap.Models.SitemapConfiguration.Implementations;
using Sitemap.Models.SitemapConfiguration;
using Sitemap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Sitemap.Controllers
{
    public class SitemapController : Controller
    {
        // return result as xml file  
        public ActionResult Index()
        {
            var sitemapItems = new List<SitemapItem> {
            new SitemapItem(Url.Action("index", "home"), changeFrequency: SitemapChangeFrequency.Always, priority: 1.0),
            new SitemapItem(Url.Action("about", "home"), lastModified: DateTime.Now),
            new SitemapItem(PathUtils.CombinePaths(Request.Url.GetLeftPart(UriPartial.Authority), "/home/list"))
            };

            return new SitemapResult(sitemapItems);
        }

        // Save file on root folder of project with name => Sitemap.xml
        // You can access it using url => https://localhost/Sitemap.xml
        public ActionResult GenerateSiteMap()
        {

            var sitemapItems = new List<SitemapItem> {
            new SitemapItem(Url.Action("index", "home"), changeFrequency: SitemapChangeFrequency.Always, priority: 1.0),
            new SitemapItem(Url.Action("about", "home"), lastModified: DateTime.Now),
            new SitemapItem(PathUtils.CombinePaths(Request.Url.GetLeftPart(UriPartial.Authority), "/home/list"))
            };

            SitemapGenerator sg = new SitemapGenerator();
            var doc = sg.GenerateSiteMap(sitemapItems);

            doc.Save(Server.MapPath("~/Sitemap.xml"));

            return RedirectToAction("Index", "Home");
        }


        // Add dynamically sitemap element on file and update it.  
        public ActionResult AddNewSitemapElement()
        {
            SitemapGenerator sg = new SitemapGenerator();
            //create a sitemap item
            var siteMapItem = new SitemapItem(Url.Action("NewAdded", "NewController"), changeFrequency: SitemapChangeFrequency.Always, priority: 1.0);

            //Get the XElement from SitemapGenerator.CreateItemElement
            var NewItem = sg.CreateItemElement(siteMapItem);

            //create XMLdocument element to add the new node in the file
            XmlDocument document = new XmlDocument();

            //load the already created XML file
            document.Load(Server.MapPath("~/Sitemap.xml"));

            //convert XElement into XmlElement
            XmlElement childElement = document.ReadNode(NewItem.CreateReader()) as XmlElement;
            XmlNode parentNode = document.SelectSingleNode("urlset");

            //This line of code get's urlset with it's last child and append the new Child just before the last child
            document.GetElementsByTagName("urlset")[0].InsertBefore(childElement, document.GetElementsByTagName("urlset")[0].LastChild);

            //save the updated file
            document.Save(Server.MapPath("~/Sitemap.xml"));

            return RedirectToAction("Index", "Home");
        }
    }
}