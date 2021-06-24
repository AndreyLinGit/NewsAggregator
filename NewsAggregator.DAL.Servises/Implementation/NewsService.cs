using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Serviсes.Interfaces;

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
            return await _unitOfWork.News.Get().OrderBy(news => news.PublishTime).ToListAsync();
        }

        public async Task<IEnumerable<NewsWithRssSourceNameDto>> GetPartOfNews(int count, string lastGottenPublishTime)
        {
            DateTime time = new DateTime();
            time = DateTime.Parse(lastGottenPublishTime);
            var newsCollection = await _unitOfWork.News.FindBy(
                news => news.PublishTime.CompareTo(time) < 0,
                rssSourceName => rssSourceName.RssSource)
                .OrderByDescending(n => n.PublishTime)
                .ToListAsync();
            
            var newsPartCollection = newsCollection.Take(count);

            var newsWithRssSourceName = new List<NewsWithRssSourceNameDto>();
            foreach (var singleNews in newsPartCollection)
            {
                newsWithRssSourceName.Add(new NewsWithRssSourceNameDto
                {
                    Article = singleNews.Article,
                    Body = singleNews.Body,
                    Id = singleNews.Id,
                    PublishTime = singleNews.PublishTime,
                    Rating = singleNews.Rating,
                    RssSourceName = singleNews.RssSource.Name,
                    Summary = singleNews.Summary,
                    Url = singleNews.Url
                });
            }

            return newsWithRssSourceName;
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
                    Summary = newsDto.Summary,
                    Body = newsDto.Body,
                    CleanedBody = newsDto.CleanedBody,
                    PublishTime = newsDto.PublishTime,
                    RssSourceId = newsDto.RssSourceId,
                    Url = newsDto.Url
                };
                newsRange.Add(news);
            }
            await _unitOfWork.News.AddRange(newsRange);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<NewsDto> GetNewsById(Guid id)
        {
            var news = await _unitOfWork.News.GetById(id);
            return new NewsDto
            {
                Article = news.Article,
                Summary = news.Summary,
                Body = news.Body,
                CleanedBody = news.CleanedBody,
                Id = news.Id,
                PublishTime = news.PublishTime,
                Rating = news.Rating,
                RssSourceId = news.RssSourceId,
                Url = news.Url
            };
        }
    }
}
