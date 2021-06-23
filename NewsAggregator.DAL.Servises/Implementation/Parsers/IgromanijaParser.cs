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
    public class IgromanijaParser: IWebParser
    {
        public async Task<string> Parse(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var fullPage = web.Load(url);
            var nodeText = fullPage.DocumentNode.SelectSingleNode("//div[@class='universal_content clearfix']");
            var htmlDocumentText = new HtmlDocument();
            if (nodeText == null)
            {
                return string.Empty;
            }
            htmlDocumentText.LoadHtml(nodeText.InnerHtml);
            var html = htmlDocumentText.DocumentNode.InnerHtml;

            var blockMore = htmlDocumentText.DocumentNode.SelectNodes("//div[@class ='uninote console']");
            if (blockMore != null)
            {
                foreach (var eachNode in blockMore)
                {
                    html = html.Replace(eachNode.OuterHtml, string.Empty);
                }
            }

            var linkToOtherNews = htmlDocumentText.DocumentNode.SelectNodes("//div[@class ='grayblock_nom']");
            if (linkToOtherNews != null)
            {
                foreach (var eachNode in linkToOtherNews)
                {
                    html = html.Replace(eachNode.OuterHtml, string.Empty);
                }
            }

            var nodeHeaderImage = fullPage.DocumentNode.SelectSingleNode("//div[@class ='main_pic_container']");
            if (nodeHeaderImage != null)
            {
                var regex = new Regex(@"\(([^)]*)\)");
                var link = regex.Match(nodeHeaderImage.OuterHtml).Groups[1].Value;
                var test = @"<div><img src=" + link + "></div>";
                html = html.Insert(0, test);
            }



            return html;
        }

        public async Task<string> CleanParse(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);


            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='universal_content clearfix']");
            return node != null ? node.InnerText : string.Empty;
        }
    }
}
