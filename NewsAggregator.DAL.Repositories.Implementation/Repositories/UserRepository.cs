using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Implementation.Repositories;

namespace NewsAggregator.DAL.Repositories.Implementation
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(NewsAggregatorContext context)
            : base(context)
        {

        }
    }
}
