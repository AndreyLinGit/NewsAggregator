using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.Models
{
    public class NewsDetailModel
    {
        public Guid Id { get; set; }
        public string Article { get; set; }
        public string Body { get; set; }
        public DateTime PublishTime { get; set; }
        public int? Rating { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}
