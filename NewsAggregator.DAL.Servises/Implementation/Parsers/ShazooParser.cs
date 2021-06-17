using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class ShazooParser : IWebParser
    {
        public async Task<string> Parse(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var fullPage = web.Load(url);

            var nodeText = fullPage.DocumentNode.SelectSingleNode("/html/body/div[2]/div[2]/div[3]/div/div[1]/article/section[1]");
            var htmlDocumentText = new HtmlDocument();
            if (nodeText == null)
            {
                return string.Empty;
            }
            htmlDocumentText.LoadHtml(nodeText.InnerHtml);
            var html = htmlDocumentText.DocumentNode.InnerHtml;

            var nodeHeaderImage = fullPage.DocumentNode.SelectSingleNode("/html/body/div[2]/div[2]/div[2]/div[2]/div[3]");
            if (nodeHeaderImage != null)
            {
                var regex = new Regex(@"src=""([^""]*)\""");
                var link = regex.Match(nodeHeaderImage.OuterHtml).Groups[1].Value;
                var test = @"<div><img src=" + link + "></div>";
                html = html.Insert(0, test);
            }

            var updateBanner = fullPage.DocumentNode.SelectSingleNode("/html/body/div[2]/div[2]/div[3]/div/div[1]/article/section[1]/div");
            if (updateBanner != null)
            {
               html = html.Replace(updateBanner.OuterHtml, string.Empty);
               var h3Update = fullPage.DocumentNode.SelectSingleNode("/html/body/div[2]/div[2]/div[3]/div/div[1]/article/section[1]/h3");
               if (h3Update != null)
               {
                   html = html.Replace(h3Update.OuterHtml, string.Empty);
               }
            }

            return html;
        }

        public async Task<string> CleanParse(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@id='content']");
            return node != null ? node.InnerText : string.Empty;
        }
    }
}
