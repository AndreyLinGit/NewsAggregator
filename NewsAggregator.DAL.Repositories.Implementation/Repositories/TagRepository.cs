using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class TagRepository : Repository<Tag>
    {
        public TagRepository(NewsAggregatorContext context)
            : base(context)
        {

        }
    }
}
