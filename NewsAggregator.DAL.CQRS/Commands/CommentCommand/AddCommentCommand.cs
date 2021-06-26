using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.CQRS.Commands.CommentCommand
{
    public class AddCommentCommand : IRequest<int>
    {
        public CommentDto CommentDto { get; set; }

        public AddCommentCommand(CommentDto commentDto)
        {
            CommentDto = commentDto;
        }
    }
}
