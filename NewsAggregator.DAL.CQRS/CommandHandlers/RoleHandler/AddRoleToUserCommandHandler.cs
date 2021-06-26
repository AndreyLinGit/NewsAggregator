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
using NewsAggregator.DAL.CQRS.Commands.RoleCommand;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.RoleHandler
{
    public class AddRoleToUserCommandHandler : IRequestHandler<AddRoleToUserCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddRoleToUserCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(request.UserId),
                cancellationToken);
            user.RoleId = (await _dbContext.Roles.FirstOrDefaultAsync(role => role.Name.Equals(request.Role),
                cancellationToken)).Id;
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
