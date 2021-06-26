using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace NewsAggregator.DAL.CQRS.Queries.UserQueries
{
    public class GetUserEmailByRefreshTokenQuery : IRequest<string>
    {
        public string Token { get; set; }

        public GetUserEmailByRefreshTokenQuery(string token)
        {
            Token = token;
        }
    }
}
