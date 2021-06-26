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
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.CQRS.Queries.RefreshTokenQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.RefreshTokenHandlers
{
    public class GetRefreshTokenByTokenValueQueryHandler : IRequestHandler<GetRefreshTokenByTokenValueQuery, RefreshTokenDto>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public GetRefreshTokenByTokenValueQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<RefreshTokenDto> Handle(GetRefreshTokenByTokenValueQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<RefreshTokenDto>(
                await _dbContext.RefreshTokens.AsNoTracking()
                    .FirstOrDefaultAsync(rt => rt.Token.Equals(request.TokenValue),
                        cancellationToken));
        }
    }
}
