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
using NewsAggregator.DAL.Core.Mapping;
using NewsAggregator.DAL.CQRS.Queries.RssSourceQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.RssSourcesHandlers
{
    public class GetAllRssSourcesQueryHandler : IRequestHandler<GetAllRssSourcesQuery, IEnumerable<RssSourceDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _dbContext;

        public GetAllRssSourcesQueryHandler(NewsAggregatorContext dbContext, IMapper autoMapper)
        {
            _dbContext = dbContext;
            _mapper = autoMapper;
        }

        public async Task<IEnumerable<RssSourceDto>> Handle(GetAllRssSourcesQuery request,
            CancellationToken cancellationToken)
        {
            return await _dbContext.RssSources.Where(sourse => !string.IsNullOrEmpty(sourse.Name))
                .Select(sourse => _mapper.Map<RssSourceDto>(sourse)).ToListAsync(cancellationToken);
        }
    }
}
