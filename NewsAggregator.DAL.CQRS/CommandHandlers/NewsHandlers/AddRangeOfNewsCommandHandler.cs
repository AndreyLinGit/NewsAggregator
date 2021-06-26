using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.NewsCommand;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.NewsHandlers
{
    public class AddRangeOfNewsCommandHandler : IRequestHandler<AddRangeOfNewsCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddRangeOfNewsCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddRangeOfNewsCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.News.AddRangeAsync(request.NewsDtos.Select(newsDto => _mapper.Map<News>(newsDto)), cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
