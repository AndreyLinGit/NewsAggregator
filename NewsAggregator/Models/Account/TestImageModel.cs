﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NewsAggregator.Models.Account
{
    public class TestImageModel
    {
        public string ImagePath { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
