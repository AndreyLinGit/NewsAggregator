using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class RssSourseRepository : Repository<RssSource>
    {
        public RssSourseRepository(NewsAggregatorContext context)
            : base(context)
        {

        }
    }
}
