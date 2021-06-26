using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.CQRS.Commands.RefreshTokenCommand
{
    public class UpdateCurrentRefreshTokensCommand : IRequest<int>
    {
        public Guid UserId { get; set; }
        public RefreshTokenDto RefreshTokenDto { get; set; }

        public UpdateCurrentRefreshTokensCommand(Guid userId, RefreshTokenDto refreshTokenDto)
        {
            UserId = userId;
            RefreshTokenDto = refreshTokenDto;
        }

    }
}
