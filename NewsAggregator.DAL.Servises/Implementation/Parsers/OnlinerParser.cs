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

            var deleteIndex = hrIndex1 > integrationIndex1 ? hrIndex1 : integrationIndex1;
            var node1 = new HtmlNode(HtmlNodeType.Element, new HtmlDocument(), 0);

            var x = node.ChildNodes;
            node.RemoveChild(node.SelectSingleNode("//hr"), true);
            //foreach (var childNode in x)
            //{
            //    var childNodeIndex = node.ChildNodes.IndexOf(childNode);
            //    if (childNodeIndex >= deleteIndex)
            //    {
            //        childNode.ParentNode.ChildNodes.Remove(childNodeIndex);
            //        //htmlText += childNode.InnerHtml;
            //    }
            //}


            var a = string.Empty;

            return node != null ? node.InnerHtml : string.Empty;
            //return htmlText + @"</div>";
        }
    }
}
