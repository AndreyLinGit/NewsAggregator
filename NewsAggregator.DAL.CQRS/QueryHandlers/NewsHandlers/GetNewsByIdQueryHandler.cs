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
using NewsAggregator.DAL.CQRS.Queries.NewsQueries;
using NewsAggregator.DAL.CQRS.Queries.RssSourceQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.NewsHandlers
{
    public class GetNewsByIdQueryHandler : IRequestHandler<GetNewsByIdQuery, NewsDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _dbContext;

        public GetNewsByIdQueryHandler(NewsAggregatorContext dbContext, IMapper autoMapper)
        {
            _dbContext = dbContext;
            _mapper = autoMapper;
        }

        public async Task<NewsDto> Handle(GetNewsByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _dbContext.News.Where(news => news.Id.Equals(request.Id))
                .Select(news => _mapper.Map<NewsDto>(news)).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
