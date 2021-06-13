using System;

namespace NewsAggregator.Models.Account
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public float Rating { get; set; }
        public string ImagePath { get; set; }
        public Guid? RoleId { get; set; }
    }
}
