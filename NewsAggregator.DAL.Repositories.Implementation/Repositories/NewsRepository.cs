using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Implementation
{
    public class NewsRepository : Repository<News>
    {
        public NewsRepository(NewsAggregatorContext context)
            : base(context)
        {

        }
    }
}
