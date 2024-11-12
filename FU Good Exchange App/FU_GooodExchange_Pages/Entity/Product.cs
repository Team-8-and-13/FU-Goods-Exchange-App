using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FU_GooodExchange_Pages
{
    public class Product : BaseEntity
    {
        public Guid SellerId { get; set; }
        public string CategoryId { get; set; } = string.Empty;
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
        public int TotalStar { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
        [ForeignKey("SellerId")]
        public virtual ApplicationUser? User { get; set; }
    }
}
