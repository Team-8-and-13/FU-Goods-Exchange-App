using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.ModelViews.ReportModelsView
{
    public class UpdateReportRequestModel
    {
        public string Reason { get; set; } = string.Empty;
        public bool Status { get; set; }
    }
}
