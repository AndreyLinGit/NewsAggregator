using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.DAL.CQRS.Queries.CommentQueries
{
    public class GetCommentsByNewsIdQuery : IRequest<IEnumerable<CommentDto>>
    {
        public Guid NewsId { get; set; }

        public GetCommentsByNewsIdQuery(Guid newsId)
        {
            NewsId = newsId;
        }
    }
}
