using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Queries.RssSourceQueries;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class RssSourceCqrsServiсe : IRssSourceService
    {
        private readonly IMediator _mediator;

        public RssSourceCqrsServiсe(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task AddSource(RssSourceDto source)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RssSourceDto>> GetAllSources()
        {
            return await _mediator.Send(new GetAllRssSourcesQuery()); //??
        }

        public Task GetNewsFromSources()
        {
            throw new NotImplementedException();
        }
    }
}
