using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.CQRS.Commands.UserCommand
{
    public class AddUserCommand : IRequest<int>
    {
        public UserDto UserDto { get; set; }

        public AddUserCommand(UserDto userDto)
        {
            UserDto = userDto;
        }
    }
}

