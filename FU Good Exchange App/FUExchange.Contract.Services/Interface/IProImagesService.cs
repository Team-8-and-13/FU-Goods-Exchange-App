using FUExchange.Contract.Repositories.Entity;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.ModelViews.ProductImagesModelViews;
namespace FUExchange.Contract.Services.Interface
{
    public interface IProImagesService
    {
        Task CreateProductImageAsync(CreateProductImageModelViews createproimg, string idPro);
        Task UpdateProductIamgeAsync(string id, UpdateProductImageModelViews createproimg);
        Task<ProductImage> DeleteProductIamgeAsync(string id);
        Task<IEnumerable<ProductImage>> GetImagesbyIdPro(string productId);
        Task<ProductImage> GetProductImageByIdAsync(string id);
    }
}
