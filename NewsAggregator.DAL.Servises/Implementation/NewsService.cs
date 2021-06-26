using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.OLE.Interop;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class NewsService : INewsService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public NewsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<IEnumerable<NewsWithRssSourceNameDto>> GetPartOfNews(int count, string lastGottenPublishTime)
        {
            DateTime time = new DateTime();
            time = DateTime.Parse(lastGottenPublishTime);
            var newsCollection = await _unitOfWork.News.FindBy(
                news => news.PublishTime.CompareTo(time) < 0,
                rssSourceName => rssSourceName.RssSource)
                .OrderByDescending(n => n.PublishTime)
                .Take(count)
                .ToListAsync();

            return newsCollection.Select(news => _mapper.Map<NewsWithRssSourceNameDto>(news));
        }

        public async Task AddRangeOfNews(IEnumerable<NewsDto> rangeOfNewsDtos)
        {
            await _unitOfWork.News.AddRange(rangeOfNewsDtos.Select(newsDto => _mapper.Map<News>(newsDto)));
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task<NewsDto> GetNewsById(Guid id)
        {
            var news = await _unitOfWork.News.GetById(id);
            return _mapper.Map<NewsDto>(news);
        }

        public async Task<bool> CheckUrl(string url)
        {
            return !_unitOfWork.News.FindBy(news => news.Url.Equals(url)).OrderBy(news => news.PublishTime).Take(500)
                .Any();
        }
    }
}
