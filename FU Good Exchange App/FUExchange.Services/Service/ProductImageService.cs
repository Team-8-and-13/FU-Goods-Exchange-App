using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.ModelViews.ProductImagesModelViews;
using Microsoft.AspNetCore.Http;
namespace FUExchange.Services.Service
{
    public class ProductImageService : IProImagesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateProductImageAsync(CreateProductImageModelViews createproimg, string idPro)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            var proImg = new ProductImage
            {
                CreatedBy = userID.ToString(),
                CreatedTime = DateTime.Now,
                Image = createproimg.Image,
                ProductId = idPro,
                Description = createproimg.Description
            };
            await _unitOfWork.GetRepository<ProductImage>().InsertAsync(proImg);
            await _unitOfWork.SaveAsync(); // Lưu thay đổi vào database
        }

        public async Task UpdateProductIamgeAsync(string id, UpdateProductImageModelViews updateproimg)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            var existingProimg = await _unitOfWork.GetRepository<ProductImage>().GetByIdAsync(id);

            if (existingProimg == null || existingProimg.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("ProductImage not found or has been deleted.");
            }

            existingProimg.Image = updateproimg.Image;
            existingProimg.Description = updateproimg.Description;
            existingProimg.LastUpdatedBy = userID.ToString();
            existingProimg.LastUpdatedTime = DateTime.Now;

            await _unitOfWork.SaveAsync();
        }

        public async Task<ProductImage> DeleteProductIamgeAsync(string id)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            var proimg = await _unitOfWork.GetRepository<ProductImage>().GetByIdAsync(id);
            if (proimg == null) throw new Exception("ProductImage not found");

            proimg.DeletedTime = DateTime.Now; 
            proimg.DeletedBy = userID.ToString();

            await _unitOfWork.GetRepository<ProductImage>().UpdateAsync(proimg);
            await _unitOfWork.SaveAsync();
            return proimg;
        }

        public async Task<IEnumerable<ProductImage>> GetImagesbyIdPro(string productId)
        {
            return await _unitOfWork.GetProductImagRepository().GetImagesbyIdPro(productId); // Gọi repository để lấy danh sách hình ảnh theo ProductId
        }

        public async Task<ProductImage> GetProductImageByIdAsync(string id)
        {
            return await _unitOfWork.GetProductImagRepository().GetProductImageByIdAsync(id); // Gọi repository để lấy một hình ảnh cụ thể theo Id
        }
    }
}
