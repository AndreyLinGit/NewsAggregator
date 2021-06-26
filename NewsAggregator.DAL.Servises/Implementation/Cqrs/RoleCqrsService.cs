using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.CQRS.Commands.RoleCommand;
using NewsAggregator.DAL.CQRS.Commands.UserCommand;
using NewsAggregator.DAL.CQRS.Queries.RoleQueries;
using NewsAggregator.DAL.CQRS.Queries.UserQueries;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation.Cqrs
{
    public class RoleCqrsService : IRoleService
    {
        private readonly IMediator _mediator;

        public RoleCqrsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RoleDto> GetUserRole(Guid roleId)
        {
            return await _mediator.Send(new GetUserRoleByIdQuery(roleId));
        }

        public async Task AddRoleToUser(Guid id)
        {
            await _mediator.Send(new AddRoleToUserCommand(id, "User"));
        }
    }
}
