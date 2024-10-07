using FUExchange.Core.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUExchange.Contract.Repositories.Entity
{
    public class ProductImage : BaseEntity
    {
        public string? ProductId { get; set; }
        [Required]
        public string Image {  get; set; } = string.Empty;
        public string? Description { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }

    }
}
