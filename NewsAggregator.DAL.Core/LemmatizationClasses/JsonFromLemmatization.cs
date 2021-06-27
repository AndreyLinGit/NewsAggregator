using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core.LemmatizationClasses
{
    public class JsonFromLemmatization
    {
        public string Text { get; set; }
        public Annotations Annotations { get; set; }
    }
}
