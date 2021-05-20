using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class FourPdaParser : IWebParser
    {
        public string Parse(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);

            
            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='content']");
            return node != null ? node.InnerText : string.Empty;
        }
    }
}
