using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodNewsAggregator.Requests
{
    public class LoginRequest
    {
        public string EmailOrLogin { get; set; }
        public string Password { get; set; }
    }
}
