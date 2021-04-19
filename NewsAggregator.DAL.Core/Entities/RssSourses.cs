using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core.Entities
{
    public class RssSourses
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

    }
}
