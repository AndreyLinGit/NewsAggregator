using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsAggregator.DAL.Core.DTOs
{
    public class ImageDto
    {
        public string ImagePath { get; set; }
        public IFormFile ImageFile { get; set; }
        public byte[] ImageByteData { get; set; } 
    }
}
