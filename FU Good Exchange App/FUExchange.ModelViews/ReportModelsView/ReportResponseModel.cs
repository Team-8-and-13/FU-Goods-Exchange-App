using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.ModelViews.ReportModelsView
{
    public class ReportResponseModel
    {
        public string Id { get; set; }
        public string Reason { get; set; }
        public bool Status { get; set; }
        public Guid? UserId { get; set; }
    }
}
