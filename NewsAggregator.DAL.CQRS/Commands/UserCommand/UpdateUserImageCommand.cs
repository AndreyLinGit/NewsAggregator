using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.CQRS.Commands.UserCommand
{
    public class UpdateUserImageCommand : IRequest<int>
    {
        public Guid UserId { get; set; }
        public string ImagePath { get; set; }
        public UpdateUserImageCommand(Guid userId, string imagePath)
        {
            UserId = userId;
            ImagePath = imagePath;
        }
    }
}
