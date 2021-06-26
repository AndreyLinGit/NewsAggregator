using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Serviсes.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsWithRssSourceNameDto>> GetPartOfNews(int count, string lastGottenPublishTime);
        Task AddRangeOfNews(IEnumerable<NewsDto> rangeOfNews);
        Task<NewsDto> GetNewsById(Guid id);
        Task<bool> CheckUrl(string url);
    }
}