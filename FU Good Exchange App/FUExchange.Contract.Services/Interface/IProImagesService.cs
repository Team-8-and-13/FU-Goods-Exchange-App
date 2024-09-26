using FUExchange.Contract.Repositories.Entity;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.ModelViews.ProductImagesModelViews;
namespace FUExchange.Contract.Services.Interface
{
    public interface IProImagesService
    {
        Task CreateProductImage(CreateProductImageModelViews createproimg, string idPro);
        Task UpdateProductImage(string id, UpdateProductImageModelViews createproimg);
        Task<ProductImage> DeleteProductImage(string id);
        Task<IEnumerable<ProductImage>> GetImagesbyIdPro(string productId);
        Task<ProductImage> GetProductImageById(string id);
    }
}