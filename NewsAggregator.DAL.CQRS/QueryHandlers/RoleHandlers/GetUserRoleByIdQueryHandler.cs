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
using NewsAggregator.DAL.CQRS.Queries.RoleQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.RoleHandlers
{
    public class GetUserRoleByIdQueryHandler : IRequestHandler<GetUserRoleByIdQuery, RoleDto>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _dbContext;

        public GetUserRoleByIdQueryHandler(IMapper mapper, NewsAggregatorContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<RoleDto> Handle(GetUserRoleByIdQuery request, CancellationToken cancellationToken)
        {
            return _mapper.Map<RoleDto>(
                await _dbContext.Roles.FirstOrDefaultAsync(role => role.Id.Equals(request.RoleId), cancellationToken));
        }
    }
}
