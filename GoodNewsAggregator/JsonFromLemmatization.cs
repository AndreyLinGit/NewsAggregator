using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator
{
    public class JsonFromLemmatization
    {
        public string Text { get; set; }
        public Annotations Annotations { get; set; }
    }

    public class Annotations
    {
        public Lemma[] Lemma { get; set; }
    }

    public class Lemma
    {
        public int Start { get; set; }
        public int End { get; set; }
        public string Value { get; set; }
    }

}
