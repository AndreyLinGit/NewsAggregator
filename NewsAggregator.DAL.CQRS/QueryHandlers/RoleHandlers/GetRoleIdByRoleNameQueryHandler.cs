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
using NewsAggregator.DAL.CQRS.Queries.RoleQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.RoleHandlers
{
    public class GetRoleIdByRoleNameQueryHandler : IRequestHandler<GetRoleIdByRoleNameQuery, Guid>
    {
        private readonly NewsAggregatorContext _dbContext;

        public GetRoleIdByRoleNameQueryHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(GetRoleIdByRoleNameQuery request, CancellationToken cancellationToken)
        {
            return (await _dbContext.Roles.FirstOrDefaultAsync(role => role.Name.Equals(request.RoleName),
                cancellationToken)).Id;
        }
    }
}
