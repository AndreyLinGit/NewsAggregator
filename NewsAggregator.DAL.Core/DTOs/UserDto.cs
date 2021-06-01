using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities.Abstract;

namespace NewsAggregator.DAL.Core.DTOs
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public string HashPass { get; set; }
        public bool IsConfirmed { get; set; } 
        public Guid RoleId { get; set; }

    }
}
