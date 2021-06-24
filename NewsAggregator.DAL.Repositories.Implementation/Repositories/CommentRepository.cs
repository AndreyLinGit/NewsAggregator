using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class CommentRepository : Repository<Comment>
    {
        public CommentRepository(NewsAggregatorContext context)
            : base(context)
        {

        }
    }
}
