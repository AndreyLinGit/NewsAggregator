﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NewsAggregator.Models.Comment
{
    public class CreateCommentViewModel
    {
        public Guid NewsId { get; set; }
        public string CommentText { get; set; }
    }
}
