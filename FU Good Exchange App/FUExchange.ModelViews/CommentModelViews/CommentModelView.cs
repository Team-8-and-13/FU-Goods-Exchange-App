﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.ModelViews.CommentModelViews
{
    public class CommentModelView
    {
        public required string CommentId { get; set; }
        public required string CommentText { get; set; }
    }
}