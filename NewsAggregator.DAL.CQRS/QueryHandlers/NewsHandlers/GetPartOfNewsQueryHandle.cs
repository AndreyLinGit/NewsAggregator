using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Queries.NewsQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.NewsHandlers
{
    public class GetPartOfNewsQueryHandle : IRequestHandler<GetPartOfNewsQuery, IEnumerable<NewsWithRssSourceNameDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _dbContext;

        public GetPartOfNewsQueryHandle(NewsAggregatorContext dbContext, IMapper autoMapper)
        {
            _dbContext = dbContext;
            _mapper = autoMapper;
        }
        public async Task<IEnumerable<NewsWithRssSourceNameDto>> Handle(GetPartOfNewsQuery request, CancellationToken cancellationToken)
        {
            var time = new DateTime();
            time = DateTime.Parse(request.DateTime);

            var newsCollection = _dbContext.News.Where(
                    news => news.PublishTime.CompareTo(time) < 0)
                .Include(rssSourceName => rssSourceName.RssSource)
                .OrderByDescending(n => n.PublishTime);
            
            var newsPartCollection = await newsCollection.Take(request.Count).Select(news => _mapper.Map<NewsWithRssSourceNameDto>(news)).ToListAsync(cancellationToken);

            return newsPartCollection;
        }
    }
}
