﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.ModelViews.ReportModelViews
{
    public class ReportRequestModel
    {
        public string UserId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
