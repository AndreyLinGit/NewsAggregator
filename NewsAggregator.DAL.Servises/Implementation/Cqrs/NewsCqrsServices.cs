using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.NewsCommand;
using NewsAggregator.DAL.CQRS.Queries.NewsQueries;
using NewsAggregator.DAL.CQRS.QueryHandlers.NewsHandlers;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation.Cqrs
{
    public class NewsCqrsServices : INewsService
    {
        private readonly IMediator _mediator;

        public NewsCqrsServices(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<NewsWithRssSourceNameDto>> GetPartOfNews(int count, string lastGottenPublishTime)
        {
            return await _mediator.Send(new GetPartOfNewsQuery(count, lastGottenPublishTime));
        }

        public async Task AddRangeOfNews(IEnumerable<NewsDto> rangeOfNews)
        {
            await _mediator.Send(new AddRangeOfNewsCommand(rangeOfNews));
        }

        public async Task<NewsDto> GetNewsById(Guid id)
        {
            return await _mediator.Send(new GetNewsByIdQuery(id));
        }

        public async Task<bool> CheckUrl(string url)
        {
            return await _mediator.Send(new CheckUrlQuery(url));
        }
    }
}
