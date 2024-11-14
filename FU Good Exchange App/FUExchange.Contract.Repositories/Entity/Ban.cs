using FUExchange.Core.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUExchange.Contract.Repositories.Entity
{
    public class Ban : BaseEntity
    {
        public string ReportId { get; set; } = string.Empty;
        public DateTime? Expires { get; set; }
        [ForeignKey("ReportId")]
        public virtual Report? Report { get; set; }
    }
}
