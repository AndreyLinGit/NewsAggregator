using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Serviсes.Interfaces
{
    public delegate IWebParser WebParserResolver(string key);
    public interface IWebParser
    {
        Task<string> Parse(string url);
        Task<string> CleanParse(string url);
        Task<string> CleanSummary(SyndicationItem item);
    }
}
