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
    public class GetPartOfNewsForRateQueryHandler : IRequestHandler<GetPartOfNewsForRateQuery, IEnumerable<NewsDto>>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetPartOfNewsForRateQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NewsDto>> Handle(GetPartOfNewsForRateQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.News.Where(news => news.Rating == null)
                .OrderByDescending(n => n.Rating)
                .Take(request.Count)
                .Select(news => _mapper.Map<NewsDto>(news))
                .ToListAsync(cancellationToken);
        }
    }
}
