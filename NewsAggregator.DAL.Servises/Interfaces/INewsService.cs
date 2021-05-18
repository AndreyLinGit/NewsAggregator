using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Servises.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<News>> GetAllNews();
        Task AddRangeOfNews(IEnumerable<News> rangeOfNews);
    }
}