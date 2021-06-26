using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.CQRS.Commands.RefreshTokenCommand;
using NewsAggregator.DAL.CQRS.Queries.RefreshTokenQueries;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation.Cqrs
{
    public class RefreshTokenCqrsService : IRefreshTokenService
    {
        private readonly IMediator _mediator;

        public RefreshTokenCqrsService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<RefreshTokenDto> GenerateRefreshToken(Guid userId)
        {
            var newRefreshToken = new RefreshTokenDto()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Token = Guid.NewGuid().ToString(),
                CreationDate = DateTime.Now.ToUniversalTime(),
                ExpiresUtc = DateTime.Now.AddHours(10)
            };
            await _mediator.Send(new UpdateCurrentRefreshTokensCommand(userId, newRefreshToken));

            return newRefreshToken;
        }

        public async Task<bool> CheckIsRefreshTokenIsValid(string requestToken)
        {
            var rt = await _mediator.Send(new GetRefreshTokenByTokenValueQuery(requestToken));

            return rt != null && rt.ExpiresUtc >= DateTime.Now;
        }
    }
}
