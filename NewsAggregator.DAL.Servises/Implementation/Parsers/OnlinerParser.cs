using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NewsAggregator.DAL.Serviсes.Interfaces;
using Vereyon.Web;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class OnlinerParser : IWebParser
    {
        public async Task<string> Parse(string url) //Add header image!
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);
            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-text']");
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(node.InnerHtml);                        //Don't have enough time!
            var html = htmlDocument.DocumentNode.InnerHtml;

            var hrIndex = node.ChildNodes.IndexOf(node.SelectSingleNode("//hr"));
            var integrationIndex = node.ChildNodes.IndexOf(node.SelectSingleNode("//p[@style]"));
            if (hrIndex < integrationIndex & hrIndex != -1)
            {
                html = html.Remove(html.IndexOf("<hr>"));
            }
            else if (integrationIndex != -1)
            {
                html = html.Remove(html.IndexOf(node.SelectSingleNode("//p[@style]").OuterHtml));
            }

            var divsWidget = htmlDocument.DocumentNode.SelectNodes("//div[@class ='news-widget']");
            if (divsWidget != null)
            {
                foreach (var eachNode in divsWidget)
                {
                    html = html.Replace(eachNode.OuterHtml, string.Empty);
                }
            }

            var divsWidgetSpecial =
                htmlDocument.DocumentNode.SelectNodes("//div[@class ='news-widget news-widget_special']");
            if (divsWidgetSpecial != null)
            {
                foreach (var eachNode in divsWidgetSpecial)
                {
                    html = html.Replace(eachNode.OuterHtml, string.Empty);
                }
            }

            var h3Catalog = htmlDocument.DocumentNode.SelectNodes("//h3");
            if (h3Catalog != null)
            {
                foreach (var eachNode in h3Catalog)
                {
                    if (eachNode.SelectNodes("//a") != null)
                    {
                        html = html.Replace(eachNode.OuterHtml, string.Empty);
                    }
                }
            }

            var banners = htmlDocument.DocumentNode.SelectNodes("//div[@class ='news-media news-media_condensed']");
            if (banners != null)
            {
                var strngeBanner = @"https://content.onliner.by/news/1100x5616/6b32576e87d7e1a6bdd110117671ada3.jpeg";  //Ask about!!!
                foreach (var eachNode in banners)
                {
                    if (eachNode.OuterHtml.Contains(strngeBanner))
                    {
                        html = html.Replace(eachNode.OuterHtml, string.Empty);
                        string x = eachNode.InnerHtml;
                    }
                    var testtest = eachNode.InnerHtml;
                    if (eachNode.OuterHtml.Contains(@"<a href="))
                    {
                        html = html.Replace(eachNode.OuterHtml, string.Empty);
                    }
                }
            }

            return html;
        }

        public async Task<string> CleanParse(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-text']");
            return node != null ? node.InnerText : string.Empty;
        }
    }
}
    