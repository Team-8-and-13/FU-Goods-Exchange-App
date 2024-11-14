using FUExchange.Core.Base;
using FUExchange.Repositories.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUExchange.Contract.Repositories.Entity
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
