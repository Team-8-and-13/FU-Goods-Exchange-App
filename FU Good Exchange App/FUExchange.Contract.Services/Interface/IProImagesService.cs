using FUExchange.Contract.Repositories.Entity;
namespace FUExchange.Contract.Services.Interface
{
    public interface IProImagesService
    {
        Task<ProductImage> CreateProductImageAsync(ProductImage proimg);
        Task<ProductImage> UpdateProductIamgeAsync(ProductImage proimg);
        Task<ProductImage> DeleteProductIamgeAsync(string id);
        Task<IEnumerable<ProductImage>> GetImagesbyIdPro(string productId); // Trả về danh sách hình ảnh theo ProductId
        Task<ProductImage> GetProductImageByIdAsync(string id);
    }
}
