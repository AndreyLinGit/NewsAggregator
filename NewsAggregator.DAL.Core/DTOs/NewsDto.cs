using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core.DTOs
{
    public class NewsDto
    {
        public Guid Id { get; set; }

        public string Article { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string CleanedBody { get; set; }
        public DateTime PublishTime { get; set; }
        public int? Rating { get; set; }
        public Guid RssSourceId { get; set; }
        public string Url { get; set; }
        
    }
}
