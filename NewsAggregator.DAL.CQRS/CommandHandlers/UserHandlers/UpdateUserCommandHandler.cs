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
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.CQRS.Commands.UserCommand;

namespace NewsAggregator.DAL.CQRS.CommandHandlers.UserHandlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserImageCommand, int>
    {
        private readonly NewsAggregatorContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(NewsAggregatorContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<int> Handle(UpdateUserImageCommand request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id.Equals(request.UserId),
                cancellationToken);
            user.ImagePath = request.ImagePath;
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
