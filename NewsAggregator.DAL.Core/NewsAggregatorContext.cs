﻿using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core
{
    public class NewsAggregatorContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsWithTags> NewsWithTags { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RssSourse> RssSourses { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<User> Users { get; set; }

        public NewsAggregatorContext(DbContextOptions<NewsAggregatorContext> options) : base(options)
        {

        }

    }
}
