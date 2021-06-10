using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.DTOs;

namespace NewsAggregator.Models.Comment
{
    public class CommentsListViewModel
    {
        public Guid NewsId { get; set; }
        public IEnumerable<CommentDto> Comments { get; set; }
    }
}
