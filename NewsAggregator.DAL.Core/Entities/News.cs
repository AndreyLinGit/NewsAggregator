using System;
using System.Collections.Generic;
using NewsAggregator.DAL.Core.Entities.Abstract;

namespace NewsAggregator.DAL.Core.Entities
{
    public class News : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Article { get; set; }
        public string Summary { get; set; }
        public string Body { get; set; }
        public string CleanedBody { get; set; }
        public DateTime PublishTime { get; set; }
        public float Rating { get; set; }
        public string Url { get; set; }

        public Guid RssSourceId { get; set; }
        public virtual RssSource RssSource { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<NewsWithTags> NewsWithTagsCollection { get; set; }
    }
}
