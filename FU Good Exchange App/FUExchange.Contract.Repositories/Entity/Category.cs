using FUExchange.Core.Base;
using System.ComponentModel.DataAnnotations;

namespace FUExchange.Contract.Repositories.Entity
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
