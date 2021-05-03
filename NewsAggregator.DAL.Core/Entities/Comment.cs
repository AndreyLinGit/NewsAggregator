using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities.Abstract;

namespace NewsAggregator.DAL.Core.Entities
{
    public class Comment : IBaseEntity
    {
        public Guid Id { get; set; }

        public string Text { get; set; }
        public DateTime PublishTime { get; set; }
        public float Rating { get; set; }
        public Guid ParentCommentId { get; set; } 
        
        //FKs
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public Guid NewsId { get; set; }
        public virtual News News { get; set; }
    }
}
