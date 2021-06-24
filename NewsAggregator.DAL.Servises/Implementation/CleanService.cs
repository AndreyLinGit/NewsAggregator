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
        public async Task<string> Clean(string text)
        {
            Regex regex = new Regex(@"<[^>]*>");

            if (text != null)
            {
                return regex.Replace(text, string.Empty)
                    .Replace(@"&nbsp;", " ")
                    .Replace("&mdash;", " ")
                    .Replace("&laquo;", " ")
                    .Replace("&raquo;", " ")
                    .Replace("&hellip;", " ")
                    .Replace("&quot;", "")
                    .Replace("&amp;mda", "")
                    .Replace("&thinsp;", "")
                    .Replace("\n", "")
                    .Trim();
            }

            return string.Empty;
        }
    }
}
