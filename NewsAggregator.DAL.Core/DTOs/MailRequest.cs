using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core.DTOs
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; } //?
        public string Link { get; set; }
        //public List<IFormFile> Attachments { get; set; }
    }
}
