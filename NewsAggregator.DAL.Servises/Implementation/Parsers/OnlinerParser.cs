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
        public async Task<string> Parse(string url)
        {
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(url);

            var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-text']");

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(node.InnerHtml);



            var hrIndex1 = node.ChildNodes.IndexOf(node.SelectSingleNode("//hr"));
            var integrationIndex1 = node.ChildNodes.IndexOf(node.SelectSingleNode("//p[@style]"));

           

            var html = htmlDocument.DocumentNode.InnerHtml;

            var divsWidget = htmlDocument.DocumentNode.SelectNodes("//div[@class ='news-widget']");

            if (divsWidget != null)
            {
                if (divsWidget.Count != 0)
                {
                    foreach (var eachNode in divsWidget)
                    {
                        html = html.Replace(eachNode.OuterHtml, string.Empty);
                    }
                }
            }

            var divsWidgetSpecial = htmlDocument.DocumentNode.SelectNodes("//div[@class ='news-widget news-widget_special']");

            if (divsWidgetSpecial != null)
            {
                if (divsWidgetSpecial.Count != 0)
                {
                    foreach (var eachNode in divsWidgetSpecial)
                    {
                        html = html.Replace(eachNode.OuterHtml, string.Empty);
                    }
                }
            }

            var h3Catalog = htmlDocument.DocumentNode.SelectNodes("//h3");

            if (h3Catalog != null)
            {
                if (h3Catalog.Count != 0)
                {
                    foreach (var eachNode in h3Catalog)
                    {
                        if (eachNode.SelectNodes("//a") != null)
                        {
                            html = html.Replace(eachNode.OuterHtml, string.Empty);
                        }
                        
                    }
                }
            }


            string test = string.Empty;

            if (hrIndex1 < integrationIndex1 & hrIndex1 != -1)
            {
                test = "<hr>";
                html = html.Remove(html.IndexOf(test));
            }
            else if (integrationIndex1 != -1)
            {
                test = node.SelectSingleNode("//p[@style]").OuterHtml;
                html = html.Remove(html.IndexOf(test));
            }


            

            return html;
            //return htmlText + @"</div>";

            //HtmlWeb web = new HtmlWeb();
            //var htmlDoc = web.Load(url);

            //var node = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='news-text']");
            //var stringHtml = node.InnerHtml;



            

            //if (divs != null) 
            //{
            //    if (divs.Count != 0)
            //    {
            //        foreach (var div in divs)
            //        {
            //            div.Remove();
            //        }
            //    }
            //}


            //var clear = htmlDocument.DocumentNode.OuterHtml;

            ////var AcceptableTags = "i|b|u|sup|sub|ol|ul|li|br|h2|h3|h4|h5|span|div|p|a|img|blockquote";
            ////var WhiteListPattern = @"</?(?(?=" + AcceptableTags + ")notag|[a-zA-Z0-9]+)(?:\\s[a-zA-Z0-9\\-]+=?(?:([\"\"']?).*?\\1?)?)*\\s*/?>";
            ////var clear = Regex.Replace(stringHtml, WhiteListPattern, "", RegexOptions.Compiled);
            ////var sanitizer = new HtmlSanitizer();
            ////var clear = sanitizer.Sanitize(stringHtml);

            //return clear;
        }
    }

    public static class HtmlSanitizeExtension
    {
        private const string HTML_TAG_PATTERN = @"(?'tag_start'</?)(?'tag'\w+)((\s+(?'attr'(?'attr_name'\w+)(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+)))?)+\s*|\s*)(?'tag_end'/?>)";

    /// <summary>
    /// A dictionary of allowed tags and their respectived allowed attributes.  If no
    /// attributes are provided, all attributes will be stripped from the allowed tag
    /// </summary>
    public static Dictionary<string, List<string>> ValidHtmlTags = new Dictionary<string, List<string>> {
            { "p", new List<string>() },
            { "br", new List<string>() },
            { "strong", new List<string>() },
            { "ul", new List<string>() },
            { "li", new List<string>() },
            { "a", new List<string> { "href", "target" } }
        };
        /// <summary>
        /// Extension filters your HTML to the whitelist specified in the ValidHtmlTags dictionary
        /// </summary>
        public static string FilterHtmlToWhitelist(this string text)
        {
            Regex htmlTagExpression = new Regex(HTML_TAG_PATTERN, RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return htmlTagExpression.Replace(text, m =>
            {
                if (!ValidHtmlTags.ContainsKey(m.Groups["tag"].Value))
                    return String.Empty;

                StringBuilder generatedTag = new StringBuilder(m.Length);
                Group tagStart = m.Groups["tag_start"];
                Group tagEnd = m.Groups["tag_end"];
                Group tag = m.Groups["tag"];
                Group tagAttributes = m.Groups["attr"];
                generatedTag.Append(tagStart.Success? tagStart.Value : "<");
                generatedTag.Append(tag.Value);
                foreach (Capture attr in tagAttributes.Captures)
                {
                    int indexOfEquals = attr.Value.IndexOf('=');
                    // don't proceed any futurer if there is no equal sign or just an equal sign
                    if (indexOfEquals< 1)
                        continue;
                    string attrName = attr.Value.Substring(0, indexOfEquals);
                    // check to see if the attribute name is allowed and write attribute if it is
                    if (ValidHtmlTags[tag.Value].Contains(attrName))
                    {
                        generatedTag.Append(' ');
                        generatedTag.Append(attr.Value);
                    }
                }
                generatedTag.Append(tagEnd.Success ? tagEnd.Value : ">");
                return generatedTag.ToString();
            });
        }
    }

}
