using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using NewsAggregator.DAL.Core;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.UserCommand;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.UserHandlers
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public AddUserCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            await _dbContext.Users.AddAsync(new User
            {
                Email = request.UserDto.Email,
                HashPass = request.UserDto.HashPass,
                Id = request.UserDto.Id,
                ImagePath = "default",
                Login = request.UserDto.Login
            }, cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
