using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.ModelViews.ReportModelsView
{
    public class ReportListResponseModel
    {
        public string Id { get; set; }
        public string Reason { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? LastUpdatedTime { get; set; }
    }
}
