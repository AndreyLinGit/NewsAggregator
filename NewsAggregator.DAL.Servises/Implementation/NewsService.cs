using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.DTOs;
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

        public async Task AddRangeOfNews(IEnumerable<NewsDto> rangeOfNewsDtos)
        {
            var newsRange = new List<News>();
            foreach (var newsDto in rangeOfNewsDtos)
            {
                var news = new News
                {
                    Id = newsDto.Id,
                    Article = newsDto.Article,
                    Body = newsDto.Body,
                    PublishTime = newsDto.PublishTime,
                    RssSourceId = newsDto.RssSourceId,
                    Url = newsDto.Url
                };
                newsRange.Add(news);
            }
            await _unitOfWork.News.AddRange(newsRange);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
