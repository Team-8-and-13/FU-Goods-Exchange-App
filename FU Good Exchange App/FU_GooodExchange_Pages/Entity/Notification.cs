using FU_GooodExchange_Pages;
using System.ComponentModel.DataAnnotations.Schema;

namespace FU_GooodExchange_Pages
{
    public class Notification : BaseEntity
    {
        public Guid UserId { get; set; }
        public string ProductId { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }
}
