using FUExchange.Contract.Repositories.Entity;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.ModelViews.ProductImagesModelViews;
namespace FUExchange.Contract.Services.Interface
{
    public interface IProImagesService
    {
        Task CreateProductImage(CreateProductImageModelViews createproimg, string idPro);
        Task UpdateProductImage(string id, UpdateProductImageModelViews createproimg);
        Task DeleteProductImage(string id);
        Task<IEnumerable<ProductImageModelViews>> GetImagesbyIdPro(string productId);
        Task<ProductImageModelViews> GetProductImageById(string id);
    }
}