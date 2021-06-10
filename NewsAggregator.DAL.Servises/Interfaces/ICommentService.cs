using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.DTOs;
using NewsAggregator.DAL.Core.Entities;

namespace NewsAggregator.DAL.Serviсes.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetCommentsByNewsId(Guid id);
        Task AddComment(CommentDto comment);
    }
}
