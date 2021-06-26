using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.CQRS.Queries.RoleQueries
{
    public class GetUserRoleByIdQuery : IRequest<RoleDto>
    {
        public Guid RoleId { get; set; }

        public GetUserRoleByIdQuery(Guid roleId)
        {
            RoleId = roleId;
        }
    }
}
