using Sitemap.Models.SitemapConfiguration.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;


namespace Sitemap.Models.SitemapConfiguration
{
    /// <summary>
    ///  a Class which extends ActionResult and give's us XML directly by calling Action of Controller
    /// </summary>
    public class SitemapResult : ActionResult
    {
        private readonly IEnumerable<ISitemapItem> items;
        private readonly ISitemapGenerator generator;

        public SitemapResult(IEnumerable<ISitemapItem> items) : this(items, new SitemapGenerator())
        {
        }

        public SitemapResult(IEnumerable<ISitemapItem> items, ISitemapGenerator generator)
        {

            this.items = items;
            this.generator = generator;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentType = "text/xml";
            response.ContentEncoding = Encoding.UTF8;

            using (var writer = new XmlTextWriter(response.Output))
            {
                writer.Formatting = Formatting.Indented;
                var sitemap = generator.GenerateSiteMap(items);

                sitemap.WriteTo(writer);
            }

            //context.HttpContext.Response.ContentType = "text/xml";
            //using (XmlWriter writer = XmlWriter.Create(context.HttpContext.Response.Output))
            //{
            //    writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
            //    foreach (var SiteMapItem in this.items)
            //    {
            //        writer.WriteStartElement("url");
            //        writer.WriteElementString("loc", string.Format(this._Website + "{0}", SiteMapItem.Url));
            //        if (SiteMapItem.LastModified != null)
            //        {
            //            writer.WriteElementString("lastmod", string.Format("{0:yyyy-MM-dd}", SiteMapItem.LastModified));
            //        }
            //        writer.WriteElementString("changefreq", "daily");
            //        writer.WriteElementString("priority", SiteMapItem.Priority.ToString());
            //        writer.WriteEndElement();
            //    }
            //    writer.WriteEndElement();
            //    writer.Flush();
            //    writer.Close();
            //}
        }
    }
}