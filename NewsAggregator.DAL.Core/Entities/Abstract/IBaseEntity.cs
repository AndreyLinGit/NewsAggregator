using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core.Entities.Abstract
{
    public interface IBaseEntity
    {
        Guid Id { get; set; }
    }
}
