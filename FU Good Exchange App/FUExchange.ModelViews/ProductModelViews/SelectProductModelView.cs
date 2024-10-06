namespace FUExchange.ModelViews.ProductModelViews
{
    public class SelectProductModelView
    {
        public required string Id { get; set; }
        public required Guid SellerId { get; set; }
        public required string CategoryId { get; set; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public required string Description { get; set; }
        public required string Image { get; set; }
    }
}
