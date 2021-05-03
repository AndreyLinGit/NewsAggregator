using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Implementation
{
    public class NewsWithTagsRepository : Repository<NewsWithTags>
    {
        public NewsWithTagsRepository(NewsAggregatorContext context)
            : base(context)
        {

        }
    }
}
