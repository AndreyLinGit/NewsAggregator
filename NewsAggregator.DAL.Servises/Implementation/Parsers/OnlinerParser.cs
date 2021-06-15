using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class OnlinerParser : IWebParser
    {
        public async Task<string> Parse(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-text']");

            var hrIndex1 = node.ChildNodes.IndexOf(node.SelectSingleNode("//hr"));
            var integrationIndex1 = node.ChildNodes.IndexOf(node.SelectSingleNode("//p[@style]"));

            var hrIndex = node.ChildNodes.IndexOf(node.ChildNodes.LastOrDefault(n => n.InnerHtml.Contains(@"<hr>")));
            var integrationIndex = node.ChildNodes.IndexOf(node.ChildNodes.LastOrDefault(n => n.InnerHtml.Contains(@"<p style=""text - align: right; "">")));

            var deleteIndex = hrIndex1 > integrationIndex1 ? hrIndex1 : integrationIndex1;
            var node1 = new HtmlNode(HtmlNodeType.Element, new HtmlDocument(), 0);

            var htmlText = @"<div class=""news-text"">";

            var childNodes = node.ChildNodes;
            foreach (var childNode in childNodes)
            {
                if (node.ChildNodes.IndexOf(childNode) < deleteIndex)
                {
                    node.ChildNodes.Remove(childNode);
                    //htmlText += childNode.InnerHtml;
                }
            }


            var x = node1;
            return node != null ? node.InnerHtml : string.Empty;
            //return htmlText + @"</div>";
        }
    }
}
