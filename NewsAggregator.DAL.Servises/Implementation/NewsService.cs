using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Servises.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class NewsService : INewsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NewsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<News>> GetAllNews()
        {
            return await _unitOfWork.News.Get().ToListAsync();
        }

        public async Task AddRangeOfNews(IEnumerable<News> rangeOfNews)
        {
            await _unitOfWork.News.AddRange(rangeOfNews);
        }
    }
}
