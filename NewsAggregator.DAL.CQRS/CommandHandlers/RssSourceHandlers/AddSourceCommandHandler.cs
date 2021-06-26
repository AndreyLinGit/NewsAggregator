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
using NewsAggregator.DAL.CQRS.Commands.RssSourceCommand;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.RssSourceHandlers
{
    public class AddSourceCommandHandler : IRequestHandler<AddSourceCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddSourceCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddSourceCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.RssSources.AddAsync(_mapper.Map<RssSource>(request.RssSourceDto), cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
