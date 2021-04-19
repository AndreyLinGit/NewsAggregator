using System;

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

        public Guid RssSourseId { get; set; }//Create FK
        public Guid CommentsId { get; set; }
        public Guid TagsId { get; set; }
    }
}
