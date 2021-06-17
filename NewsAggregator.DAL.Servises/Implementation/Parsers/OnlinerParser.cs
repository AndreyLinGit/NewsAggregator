using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.VisualStudio.Shell.Interop;
using NewsAggregator.DAL.Serviсes.Interfaces;
using Vereyon.Web;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class OnlinerParser : IWebParser
    {
        public async Task<string> Parse(string url) //Add header image!
        {
            HtmlWeb web = new HtmlWeb();
            var fullPage = web.Load(url);

            var nodeText = fullPage.DocumentNode.SelectSingleNode("//div[@class='news-text']");
            var htmlDocumentText = new HtmlDocument();
            htmlDocumentText.LoadHtml(nodeText.InnerHtml); //Don't have enough time!
            htmlDocumentText.DocumentNode.Descendants()
                .Where(n => n.Name == "script")
                .ToList()
                .ForEach(n => n.Remove());
            var html = htmlDocumentText.DocumentNode.InnerHtml;

            var h2Catalog = htmlDocumentText.DocumentNode.SelectNodes("//h2");
            if (h2Catalog != null)
            {
                foreach (var eachNode in h2Catalog)
                {
                    var bannedLink = @"https://catalog.onliner.by/";
                    //var regex = new Regex(@"\<a href([^=]*)\=");
                    //var r = regex.Match(eachNode.OuterHtml).Groups[1].Value != string.Empty;
                    if (eachNode.OuterHtml.Contains(bannedLink))
                    {
                        html = html.Replace(eachNode.OuterHtml, string.Empty);
                    }
                }
            }

            var h3Catalog = htmlDocumentText.DocumentNode.SelectNodes("//h3");
            if (h3Catalog != null)
            {
                foreach (var eachNode in h3Catalog)
                {
                    var bannedLink = @"https://catalog.onliner.by/";
                    //var regex = new Regex(@"\<a href([^=]*)\=");
                    //var r = regex.Match(eachNode.OuterHtml).Groups[1].Value != string.Empty;
                    if (eachNode.OuterHtml.Contains(bannedLink))
                    {
                        var htmlBefore = html;
                        var itPartWillBeDeleted = eachNode.OuterHtml;
                        html = html.Replace(eachNode.OuterHtml, string.Empty);
                        var htmlAfter = html;
                    }
                }
                //foreach (var eachNode in h3Catalog)
                //{
                //    var linkNodes = eachNode.SelectNodes("//a");
                //    if (linkNodes != null)
                //    {
                //        var bannedLink = @"https://catalog.onliner.by/";
                //        foreach (var node in linkNodes)
                //        {
                //            if (node.OuterHtml.Contains(bannedLink))
                //            {
                //                //html = html.Replace(eachNode.OuterHtml, string.Empty);
                //            }
                //            else
                //            {
                //                //var regex = new Regex(@"\<a href([^=]*)\=);
                //                //var text = regex.Match(node.OuterHtml).Groups[1].Value;
                //                //var test = node.OuterHtml.Replace(node.OuterHtml, text);
                //                //html = html.Replace(node.OuterHtml, test); //??
                //            }
                //        }
                //    }
                //}
            }

            var divsSocial = htmlDocumentText.DocumentNode.SelectSingleNode("//div[@class ='news-incut news-incut_extended news-incut_position_right news-incut_shift_top news-helpers_hide_tablet']");
            if (divsSocial != null)
            {
                html = html.Replace(divsSocial.OuterHtml, string.Empty);
            }

            var referenceNode = nodeText.SelectSingleNode("//div[@class ='news-reference']");
            if (referenceNode != null)
            {
                html = html.Replace(referenceNode.OuterHtml, string.Empty);
            }

            var nodeHeaderImage = fullPage.DocumentNode.SelectSingleNode("//div[@class ='news-header__image']");
            if (nodeHeaderImage != null)
            {
                var regex = new Regex(@"\(([^)]*)\)");
                var link = regex.Match(nodeHeaderImage.OuterHtml).Groups[1];
                var test = @"<div><img src=" + link + "></div>";
                html = html.Insert(0, test);
            }


            var hrIndex = nodeText.ChildNodes.IndexOf(nodeText.SelectSingleNode("//hr"));
            var integrationIndex = nodeText.ChildNodes.IndexOf(nodeText.SelectSingleNode("//p[@style]"));
            if (hrIndex < integrationIndex & hrIndex != -1)
            {
                html = html.Remove(html.IndexOf("<hr>"));
            }
            else if (integrationIndex != -1)
            {
                html = html.Remove(html.IndexOf(nodeText.SelectSingleNode("//p[@style]").OuterHtml));
            }

            var divsWidget = htmlDocumentText.DocumentNode.SelectNodes("//div[@class ='news-widget']");
            if (divsWidget != null)
            {
                foreach (var eachNode in divsWidget)
                {
                    html = html.Replace(eachNode.OuterHtml, string.Empty);
                }
            }

            var divsWidgetSpecial =
                htmlDocumentText.DocumentNode.SelectNodes("//div[@class ='news-widget news-widget_special']");
            if (divsWidgetSpecial != null)
            {
                foreach (var eachNode in divsWidgetSpecial)
                {
                    html = html.Replace(eachNode.OuterHtml, string.Empty);
                }
            }

            

            var banners = htmlDocumentText.DocumentNode.SelectNodes("//div[@class ='news-media news-media_condensed']");
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

            var votes = htmlDocumentText.DocumentNode.SelectNodes("//div[@class ='news-vote']");
            if (votes != null)
            {
                foreach (var eachNode in votes)
                {
                    html = html.Replace(eachNode.OuterHtml, string.Empty);
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
    