using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class CleanService : ICleanService
    {
        public async Task<string> CleanBody(string text)
        {
            if (text != null)
            {
                Regex regex = new Regex(@"<[^>]*>");
                var almostClean = regex.Replace(text, string.Empty)
                    .Replace(@"&nbsp;", " ")
                    .Replace("&mdash;", " ")
                    .Replace("&laquo;", " ")
                    .Replace("&raquo;", " ")
                    .Replace("&hellip;", " ")
                    .Replace("&quot;", "")
                    .Replace("&amp;mda", "")
                    .Replace("&amp;", "")
                    .Replace("&thinsp;", "")
                    .Replace("\n", "")
                    .Trim();

                char[] ch = { '.', '!', '?', '"', '\r' };
                var splitText = almostClean.Split(ch, StringSplitOptions.RemoveEmptyEntries);
                var aggregateText = splitText.Aggregate("", (current, n) => current + " " + $"{n.Trim()}");

                return aggregateText;
            }

            return string.Empty;
        }

        public async Task<string> CleanSummary(string text)
        {
            if (text != null)
            {
                Regex regex = new Regex(@"<[^>]*>");
                return regex.Replace(text, string.Empty)
                    .Replace(@"&nbsp;", " ")
                    .Replace("&mdash;", " ")
                    .Replace("&laquo;", " ")
                    .Replace("&raquo;", " ")
                    .Replace("&hellip;", " ")
                    .Replace("&quot;", "")
                    .Replace("&amp;mda", "")
                    .Replace("&amp;", "")
                    .Replace("&thinsp;", "")
                    .Replace("\n", "")
                    .Trim();
            }

            return string.Empty;
        }
    }
}
