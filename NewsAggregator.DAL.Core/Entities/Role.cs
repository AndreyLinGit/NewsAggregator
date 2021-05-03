using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsAggregator.DAL.Core.Entities.Abstract;

namespace NewsAggregator.DAL.Core.Entities
{
    public class Role : IBaseEntity
    {
        public Guid Id { get; set; }
        public int RoleNumber { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
