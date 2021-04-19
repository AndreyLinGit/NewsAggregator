using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core.Entities
{
    public class Tag
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public virtual ICollection<NewsWithTags> NewsWithTagsCollection { get; set; }
    }
}
