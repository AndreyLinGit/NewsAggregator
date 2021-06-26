using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core.DTOs
{
    public class MailRequest
    {
        public string UserId { get; set; }
        public string ToEmail { get; set; }
        public string Subject { get; set; } 
        public string Link { get; set; }
    }
}
