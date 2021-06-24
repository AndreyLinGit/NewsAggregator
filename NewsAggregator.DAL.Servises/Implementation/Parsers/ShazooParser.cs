using System.ServiceModel.Syndication;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation.Parsers
{
    public class ShazooParser : IWebParser
    {
        private ICleanService _cleanService;

        public ShazooParser(ICleanService cleanService)
        {
            _cleanService = cleanService;
        }

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
            return node != null ? await _cleanService.Clean(node.InnerText) : string.Empty;
        }

        public async Task<string> CleanSummary(SyndicationItem item)
        {
            Regex regex = new Regex(@"<[^>]*>");

            if (item.Content != null)
            {
                var summary = await _cleanService.Clean(((TextSyndicationContent) item.Content).Text);
                return summary.Remove(summary.LastIndexOf("Больше")).Replace("...", " [...]");
            }

            return string.Empty;
        }
    }
}
