using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.CQRS.Queries.RssSourceQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.RssSourcesHandlers
{
    public class GetAllRssSourcesQueryHandler : IRequestHandler<GetAllRssSourcesQuery, IEnumerable<RssSourceDto>>
    {
        private readonly NewsAggregatorContext _dbContext;

        public GetAllRssSourcesQueryHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<RssSourceDto>> Handle(GetAllRssSourcesQuery request,
            CancellationToken cancellationToken)
        {
            var sources = _dbContext.RssSources.Where(sourse => !string.IsNullOrEmpty(sourse.Name));
            var rssSourceDtos = new List<RssSourceDto>();
            foreach (var sourse in sources)
            {
                var dto = new RssSourceDto
                {
                    Id = sourse.Id,
                    Name = sourse.Name,
                    Url = sourse.Url
                };
                rssSourceDtos.Add(dto);
            }

            return rssSourceDtos;
        }
    }
}
