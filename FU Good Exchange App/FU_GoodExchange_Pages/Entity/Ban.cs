using FU_GooodExchange_Pages;
using System.ComponentModel.DataAnnotations.Schema;

namespace FU_GooodExchange_Pages
{
    public class Ban : BaseEntity
    {
        public string ReportId { get; set; } = string.Empty;
        public DateTime? Expires { get; set; }
        [ForeignKey("ReportId")]
        public virtual Report? Report { get; set; }
    }
}
