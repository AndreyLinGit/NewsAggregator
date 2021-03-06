using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.Entities.Abstract;

namespace NewsAggregator.DAL.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class, IBaseEntity
    {
        Task Add(T entity);
        Task AddRange(IEnumerable<T> range);
        Task<T> GetById(Guid id);
        DbSet<T> Get();
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        void Update(T entity);
        Task DeleteById(Guid id);
    }
}
