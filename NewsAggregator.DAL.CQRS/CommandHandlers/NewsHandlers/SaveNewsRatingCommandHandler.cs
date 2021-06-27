using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.NewsCommand;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.NewsHandlers
{
    public class SaveNewsRatingCommandHandler : IRequestHandler<SaveNewsRatingCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;

        public SaveNewsRatingCommandHandler(NewsAggregatorContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(SaveNewsRatingCommand request, CancellationToken cancellationToken)
        {
            foreach (var pair in request.RatingDictionary)
            {
                var news = _dbContext.News.FirstOrDefault(news => news.Id.Equals(pair.Key));
                news.Rating = pair.Value;
            }
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
