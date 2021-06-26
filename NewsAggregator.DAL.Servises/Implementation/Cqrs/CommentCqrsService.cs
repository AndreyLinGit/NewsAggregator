using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.CQRS.Commands.CommentCommand;
using NewsAggregator.DAL.CQRS.Queries.CommentQueries;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation.Cqrs
{
    public class CommentCqrsService : ICommentService
    {
        private readonly IMediator _mediator;

        public CommentCqrsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByNewsId(Guid id)
        {
            return await _mediator.Send(new GetCommentsByNewsIdQuery(id));
        }

        public async Task AddComment(CommentDto comment)
        {
            await _mediator.Send(new AddCommentCommand(comment));
        }
    }
}
