using System;
using System.Collections.Generic;

namespace NewsAggregator.DAL.Core.Entities
{
    public class News
    {
        public Guid Id { get; set; }

        public string Article { get; set; }
        public string Body { get; set; }
        public DateTime PublishTime { get; set; }
        public float Rating { get; set; }
        public string Url { get; set; }

        public Guid RssSourseId { get; set; }
        public virtual RssSourse RssSourse { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<NewsWithTags> NewsWithTagsCollection { get; set; }
    }
}
