using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities.Abstract;

namespace NewsAggregator.DAL.Core.Entities
{
    public class RssSource : IBaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public ICollection<News> NewsCollection { get; set; }

    }
}
