using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core.DTOs
{
    public class RefreshTokenDto
    {
        public Guid Id { get; set; }

        public DateTime ExpiresUtc { get; set; }

        public string Token { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid UserId { get; set; }

    }
}
