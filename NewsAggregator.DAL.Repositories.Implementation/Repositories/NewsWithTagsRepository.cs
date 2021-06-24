using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class NewsWithTagsRepository : Repository<NewsWithTags>
    {
        public NewsWithTagsRepository(NewsAggregatorContext context)
            : base(context)
        {

        }
    }
}
