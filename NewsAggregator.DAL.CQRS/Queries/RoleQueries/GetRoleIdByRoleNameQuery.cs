using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace NewsAggregator.DAL.CQRS.Queries.RoleQueries
{
    public class GetRoleIdByRoleNameQuery : IRequest<Guid>
    {
        public string RoleName { get; set; }

        public GetRoleIdByRoleNameQuery(string roleName)
        {
            RoleName = roleName;
        }
    }
}
