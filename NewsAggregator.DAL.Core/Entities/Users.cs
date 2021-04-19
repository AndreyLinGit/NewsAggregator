using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core.Entities
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string HashPass { get; set; }

        public Guid RoleId { get; set; } //Create FK
        public Guid CommentId { get; set; }
    }
}
