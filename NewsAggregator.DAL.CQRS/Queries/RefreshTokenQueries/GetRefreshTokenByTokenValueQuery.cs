using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.CQRS.Queries.RefreshTokenQueries
{
    public class GetRefreshTokenByTokenValueQuery : IRequest<RefreshTokenDto>
    {
        public string TokenValue { get; set; }

        public GetRefreshTokenByTokenValueQuery(string tokenValue)
        {
            TokenValue = tokenValue;
        }
    }
}
