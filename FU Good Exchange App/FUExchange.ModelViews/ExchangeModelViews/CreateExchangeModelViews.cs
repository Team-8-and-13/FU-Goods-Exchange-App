using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.ModelViews.ExchangeModelViews
{
    public class CreateExchangeModelViews
    {
        public required Guid BuyerId { get; set; }
        public required string? ProductId { get; set; }
    }
}
