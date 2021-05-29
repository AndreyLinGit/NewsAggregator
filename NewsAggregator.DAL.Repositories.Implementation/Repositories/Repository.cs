using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities.Abstract;
using NewsAggregator.DAL.Repositories.Interfaces;

namespace NewsAggregator.DAL.Repositories.Implementation.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class, IBaseEntity
    {
        protected readonly NewsAggregatorContext Db;
        protected readonly DbSet<T> Table;
        protected Repository(NewsAggregatorContext context)
        {
            Db = context;
            Table = Db.Set<T>();
        }

        public void Dispose()
        {
            Db?.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Add(T entity)
        {
            await Table.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> range)
        {
            await Table.AddRangeAsync(range);
        }

        public async Task<T> GetById(Guid id)
        {
            return await Table.FirstOrDefaultAsync(item => item.Id.Equals(id));
        }

        public DbSet<T> Get()
        {
            return Table;
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var result = Table.Where(predicate);
            if (includes.Any())
            {
                result = includes.Aggregate(result,
                    (current, include) 
                        => current.Include(include));
            }

            return result;
        }

        public void Update(T entity)
        {
            Table.Update(entity);
        }

        public Task DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
