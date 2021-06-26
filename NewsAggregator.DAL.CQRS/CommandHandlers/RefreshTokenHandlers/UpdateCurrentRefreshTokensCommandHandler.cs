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
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.RefreshTokenCommand;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.RefreshTokenHandlers
{
    public class UpdateCurrentRefreshTokensCommandHandler : IRequestHandler<UpdateCurrentRefreshTokensCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateCurrentRefreshTokensCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateCurrentRefreshTokensCommand request, CancellationToken cancellationToken)
        {
            //remove only for 1 device support
            var currentRefreshTokens = await _dbContext.RefreshTokens.AsNoTracking()
                .Where(token => token.UserId.Equals(request.UserId))
                .ToArrayAsync(cancellationToken: cancellationToken);

            foreach (var token in currentRefreshTokens)
            {
                _dbContext.RefreshTokens.Remove(token);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            var rtEntity = _mapper.Map<RefreshToken>(request.RefreshTokenDto);
            await _dbContext.AddAsync(rtEntity, cancellationToken);

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
