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
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.CQRS.Queries.CommentQueries;

namespace NewsAggregator.DAL.CQRS.QueryHandlers.CommentsHandlers
{
    public class GetCommentsByNewsIdQueryHandler : IRequestHandler<GetCommentsByNewsIdQuery, IEnumerable<CommentDto>>
    {
        private readonly IMapper _mapper;
        private readonly NewsAggregatorContext _dbContext;

        public GetCommentsByNewsIdQueryHandler(IMapper mapper, NewsAggregatorContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<CommentDto>> Handle(GetCommentsByNewsIdQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Comments.Where(comment => comment.NewsId.Equals(request.NewsId)).Select(comment => _mapper.Map<CommentDto>(comment)).ToListAsync(cancellationToken);
        }
    }
}
