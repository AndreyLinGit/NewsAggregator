using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;
using NewsAggregator.DAL.Repositories.Interfaces;
using NewsAggregator.DAL.Serviсes.Interfaces;

namespace NewsAggregator.DAL.Serviсes.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByNewsId(Guid id)
        {
            var comments = await _unitOfWork.Comment.FindBy(coment => coment.NewsId.Equals(id)).ToListAsync();
            var commentsDto = new List<CommentDto>();
            foreach (var comment in comments)
            {
                var user = await _unitOfWork.User.GetById(comment.UserId);
                var commentDto = new CommentDto()
                {
                    Id = comment.Id,
                    Text = comment.Text,
                    Created = comment.PublishTime,
                    NewsId = comment.NewsId,
                    UserId = comment.UserId,
                    UserLogin = user.Login,
                    CommentRating = comment.Rating
                };
                commentsDto.Add(commentDto);
            }

            return commentsDto;
        }
        public async Task AddComment(CommentDto comment)
        {
            var commentEntity = new Comment
            {
                Id = comment.Id,
                NewsId = comment.NewsId,
                PublishTime = comment.Created,
                Text = comment.Text,
                UserId = comment.UserId
            };
            await _unitOfWork.Comment.Add(commentEntity);
            await _unitOfWork.SaveChangeAsync();
        }
    }
}
