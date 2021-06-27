using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.CQRS.Queries.NewsQueries;
using NewsAggregator.DAL.CQRS.Queries.UserQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.NewsHandlers
{
    public class GetCheckUrlCollectionQueryHandler : IRequestHandler<GetCheckUrlCollectionQuery, IEnumerable<string>>
    {
        private readonly NewsAggregatorContext _dbContext;

        public GetCheckUrlCollectionQueryHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<string>> Handle(GetCheckUrlCollectionQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.News.Where(
                    news => news.PublishTime.CompareTo(DateTime.Now) < 0)
                .OrderByDescending(news => news.PublishTime)
                .Take(request.CheckCount)
                .AsNoTracking()
                .Select(news => news.Url)
                .ToListAsync(cancellationToken);
        }
    }
}
