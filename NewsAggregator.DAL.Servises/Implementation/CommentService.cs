using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByNewsId(Guid id)
        {
            var comments = await _unitOfWork.Comment.FindBy(comment => comment.NewsId.Equals(id), comment => comment.User).ToListAsync();
            return comments.Select(comment => _mapper.Map<CommentDto>(comment));
        }
        public async Task AddComment(CommentDto comment)
        {
            await _unitOfWork.Comment.Add(_mapper.Map<Comment>(comment));
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
