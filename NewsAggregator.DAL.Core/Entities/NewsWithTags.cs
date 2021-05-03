using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities.Abstract;

namespace NewsAggregator.DAL.Core.Entities
{
    public class NewsWithTags : IBaseEntity
    {
        public Guid Id { get; set; }

        //FKs for many-to-many relationship
        public Guid TagsId { get; set; }
        public virtual Tag Tag { get; set; }
        public Guid NewsId { get; set; }
        public virtual News News {get;set;}

    }
}
