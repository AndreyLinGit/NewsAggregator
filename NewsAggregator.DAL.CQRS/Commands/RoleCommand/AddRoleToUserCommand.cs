using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace NewsAggregator.DAL.CQRS.Commands.RoleCommand
{
    public class AddRoleToUserCommand : IRequest<int>
    {
        public Guid UserId { get; set; }
        public string Role { get; set; }

        public AddRoleToUserCommand(Guid userId, string role)
        {
            UserId = userId;
            Role = role;
        }
    }
}
