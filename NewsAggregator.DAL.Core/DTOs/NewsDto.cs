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
        public string Body { get; set; }
        public DateTime PublishTime { get; set; }
        public float Rating { get; set; }
        public string Url { get; set; }
    }
}
