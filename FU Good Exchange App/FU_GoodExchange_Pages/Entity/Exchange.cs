using FU_GooodExchange_Pages;
using System.ComponentModel.DataAnnotations.Schema;

namespace FU_GooodExchange_Pages
{
    public class Exchange : BaseEntity
    {
        public string ProductId { get; set; } = string.Empty;
        public Guid BuyerId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
        [ForeignKey("BuyerId")]
        public virtual ApplicationUser? User { get; set; }
    }
}
