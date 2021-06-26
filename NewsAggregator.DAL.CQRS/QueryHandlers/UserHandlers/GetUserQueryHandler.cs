using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.CQRS.Queries.UserQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.UserHandlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _dbContext;

        public GetUserQueryHandler(IMapper mapper, NewsAggregatorContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            if (request.Id != null)
            {
                return _mapper.Map<UserDto>(await _dbContext.Users.Where(user => user.Id.Equals(request.Id))
                    .FirstOrDefaultAsync(cancellationToken));
            }
            if (request.Email != null)
            {
                return _mapper.Map<UserDto>(await _dbContext.Users.Where(user => user.Email.Equals(request.Email))
                    .FirstOrDefaultAsync(cancellationToken));
            }
            if (request.Login != null)
            {
                return _mapper.Map<UserDto>(await _dbContext.Users.Where(user => user.Login.Equals(request.Login))
                    .FirstOrDefaultAsync(cancellationToken));
            }

            return null;
        }
    }
}
