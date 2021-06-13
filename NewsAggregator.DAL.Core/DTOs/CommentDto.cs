using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }
        public string UserLogin { get; set; }
        public float CommentRating { get; set; }
        public Guid NewsId { get; set; } //FK
        public Guid UserId { get; set; } //FK
        
    }
}
