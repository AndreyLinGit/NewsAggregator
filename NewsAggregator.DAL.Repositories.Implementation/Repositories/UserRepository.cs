using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(NewsAggregatorContext context)
            : base(context)
        {

        }
    }
}
