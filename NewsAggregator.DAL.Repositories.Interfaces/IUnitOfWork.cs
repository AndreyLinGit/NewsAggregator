using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<Comment> Comment { get; }
        public IRepository<News> News { get; }
        public IRepository<Role> Role { get; }
        public IRepository<RssSource> RssSourse { get; }
        public IRepository<User> User { get; }
        Task<int> SaveChangeAsync();
    }
}
