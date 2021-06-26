using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities.Abstract;

namespace NewsAggregator.DAL.Core.Entities
{
    public class RefreshToken : IBaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime ExpiresUtc { get; set; }

        [Required]
        public string Token { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
