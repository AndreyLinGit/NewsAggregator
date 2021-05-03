using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;

namespace NewsAggregator.DAL.Repositories.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NewsAggregatorContext Db;
        public IRepository<Comment> Comment { get; }

        public IRepository<News> News { get; }
        public IRepository<NewsWithTags> NewsWithTags { get; }
        public IRepository<Role> Role { get; }
        public IRepository<RssSourse> RssSourse { get; }
        public IRepository<Tag> Tag { get; }
        public IRepository<User> User { get; }
        public UnitOfWork(NewsAggregatorContext context,
            IRepository<News> newsRepository,
            IRepository<Comment> commentRepository,
            IRepository<NewsWithTags> newsWithTagsRepository,
            IRepository<Role> roleRepository,
            IRepository<RssSourse> rssSourseRepository,
            IRepository<Tag> tagRepository,
            IRepository<User> userRepository)
        {
            Db = context;
            News = newsRepository;
            Comment = commentRepository;
            NewsWithTags = newsWithTagsRepository;
            Role = roleRepository;
            RssSourse = rssSourseRepository;
            Tag = tagRepository;
            User = userRepository;
        }
        public void Dispose()
        {
            Db?.Dispose();
            GC.SuppressFinalize(this);
        }
        
        public async Task<int> SaveChangeAsync()
        {
            return await Db.SaveChangesAsync();
        }
    }

    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangeAsync();
    }
}
