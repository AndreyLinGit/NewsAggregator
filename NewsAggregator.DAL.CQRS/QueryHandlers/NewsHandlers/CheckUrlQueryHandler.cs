using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MediatR.Pipeline;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries.NewsQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.NewsHandlers
{
    public class CheckUrlQueryHandler : IRequestHandler<CheckUrlQuery, bool>
    {
        private readonly NewsAggregatorContext _dbContext;

        public CheckUrlQueryHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Handle(CheckUrlQuery request, CancellationToken cancellationToken)
        {
            return _dbContext.News.Where(news => news.Url.Equals(request.NewsUrl)).OrderBy(news => news.PublishTime).Take(500)
                .Any();
        }
    }
}
