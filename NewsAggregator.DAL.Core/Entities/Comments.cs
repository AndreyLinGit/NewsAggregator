using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core.Entities
{
    public class Comments
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
        public DateTime PublishTime { get; set; }
        public float Rating { get; set; }

        //Create FKs
        public Guid ParentCommentId { get; set; } 
        public Guid UserId { get; set; }
        public Guid NewsId { get; set; }
    }
}
