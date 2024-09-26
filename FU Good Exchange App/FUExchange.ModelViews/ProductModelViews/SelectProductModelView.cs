﻿namespace FUExchange.ModelViews.ProductModelViews
{
    public class SelectProductModelView
    {
        public required string CategoryId { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public required string Description { get; set; }
    }
}