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
        private readonly IRepository<News> _newsRepository;
        private readonly IRepository<Comment> _commentRepository;
        private readonly IRepository<NewsWithTags> _newsWithTagsRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<RssSourse> _rssSourseRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<User> _userRepository;


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
            _newsRepository = newsRepository;
            _commentRepository = commentRepository;
            _newsWithTagsRepository = newsWithTagsRepository;
            _roleRepository = roleRepository;
            _rssSourseRepository = rssSourseRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
        }

        public IRepository<Comment> Comment => _commentRepository;

        public IRepository<News> News => _newsRepository;

        public IRepository<NewsWithTags> NewsWithTags => _newsWithTagsRepository;

        public IRepository<Role> Role => _roleRepository;

        public IRepository<RssSourse> RssSourse => _rssSourseRepository;

        public IRepository<Tag> Tag => _tagRepository;

        public IRepository<User> User => _userRepository;

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

    
}
