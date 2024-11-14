using FU_GooodExchange_Pages;
using System.ComponentModel.DataAnnotations;

namespace FU_GooodExchange_Pages
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
