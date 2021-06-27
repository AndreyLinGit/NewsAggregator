using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation.Parsers
{
    public class FourPdaParser : IWebParser
    {
        private readonly ICleanService _cleanService;

        public FourPdaParser(ICleanService cleanService)
        {
            _cleanService = cleanService;
        }

        public async Task<string> Parse(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var fullPage = web.Load(url);
            var nodeText = fullPage.DocumentNode.SelectSingleNode("//div[@class='content']");
            var htmlDocumentText = new HtmlDocument();
            if (nodeText == null)
            {
                return string.Empty;
            }
            htmlDocumentText.LoadHtml(nodeText.InnerHtml);
            var html = htmlDocumentText.DocumentNode.InnerHtml;

            var galContainer = htmlDocumentText.DocumentNode.SelectNodes("//div[@class ='GalWrap']");
            if (galContainer != null)
            {
                return string.Empty;
            }

            var sources = htmlDocumentText.DocumentNode.SelectNodes("//p[@class ='mb_source']");
            if (sources != null)
            {
                foreach (var eachNode in sources)
                {
                    html = html.Replace(eachNode.OuterHtml, string.Empty);
                }
            }

            var nodeHeaderImage = fullPage.DocumentNode.SelectSingleNode("//div[@class ='photo']");
            if (nodeHeaderImage != null)
            {
                html = html.Insert(0, nodeHeaderImage.OuterHtml);
            }

            return html;
        }

        public async Task<string> CleanParse(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);


            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='content']");
            return node != null ? await _cleanService.CleanBody(node.InnerText) : string.Empty;
        }

        public async Task<string> CleanSummary(SyndicationItem item)
        {
            Regex regex = new Regex(@"<[^>]*>");

            if (item.Summary != null)
            {
                return await _cleanService.CleanSummary(item.Summary.Text);
            }
            return string.Empty;
        }
    }
}
