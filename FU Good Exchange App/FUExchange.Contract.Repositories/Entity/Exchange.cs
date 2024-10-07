using FUExchange.Core.Base;
using FUExchange.Repositories.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUExchange.Contract.Repositories.Entity
{
    public class Exchange : BaseEntity
    {
        public string? ProductId { get; set; }
        public Guid? BuyerId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("BuyerId")]
        public ApplicationUser User { get; set; }
    }
}
