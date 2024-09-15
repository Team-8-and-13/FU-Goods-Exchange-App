using FUExchange.Core.Base;
using FUExchange.Repositories.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUExchange.Contract.Repositories.Entity
{
    public class Product : BaseEntity
    {
        public Guid? SellerId { get; set; }
        public string? CategoryId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public double NumberOfStar {  get; set; } 
        public bool Approve {  get; set; }
        public bool Sold {  get; set; }
        public int Rating { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [ForeignKey("SellerId")]
        public ApplicationUser User { get; set; }
    }
}
