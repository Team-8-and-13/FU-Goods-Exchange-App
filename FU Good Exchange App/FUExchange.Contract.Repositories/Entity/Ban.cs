using FUExchange.Core.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUExchange.Contract.Repositories.Entity
{
    public class Ban : BaseEntity
    {
        public string? ReportId { get; set; }
        public DateTime? Expires { get; set; }
        [ForeignKey("ReportId")]
        public Report? Report { get; set; }
    }
}
