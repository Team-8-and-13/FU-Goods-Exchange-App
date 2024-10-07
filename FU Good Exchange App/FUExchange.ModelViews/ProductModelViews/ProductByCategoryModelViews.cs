using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.ModelViews.ProductModelViews
{
    public class ProductByCategoryModelViews
    {
        public required string CategoryName { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public required string Description { get; set; } = string.Empty;
    }
}
