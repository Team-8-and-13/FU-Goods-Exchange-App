using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.ModelViews.NotificationModelViews
{
    public class NotificationDisplay
    {
        public required string Id { get; set; }
        public required string ProductId { get; set; }
        public required string Content { get; set; }
        public required DateTime CreatedTime { get; set; }
    }
}
