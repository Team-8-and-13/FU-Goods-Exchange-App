using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Repositories.PaggingItems;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.ModelViews.ProductImagesModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static FUExchange.Core.Base.BaseException;
namespace FUExchange.Services.Service
{
    public class ProductImageService : IProImagesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductImageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateProductImage(CreateProductImageModelViews createproimg, string idPro)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            var idProduct = await _unitOfWork.GetRepository<Product>().GetByIdAsync(idPro);
            if (idProduct == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không hợp dữ liệu.");
            }
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

        public async Task UpdateProductImage(string id, UpdateProductImageModelViews updateproimg)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            var existingProimg = await _unitOfWork.GetRepository<ProductImage>().GetByIdAsync(id);
            if (updateproimg == null || existingProimg.Id != id)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Dữ liệu không hợp lệ.");
            }
            if (existingProimg == null || existingProimg.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy id ProductImage hoặc đã bị xóa.");
            }
            existingProimg.Image = updateproimg.Image;
            existingProimg.Description = updateproimg.Description;
            existingProimg.LastUpdatedBy = userID.ToString();
            existingProimg.LastUpdatedTime = DateTime.Now;

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteProductImage(string id)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            var proimg = await _unitOfWork.GetRepository<ProductImage>().GetByIdAsync(id);
            if (proimg.Id != id) 
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Id không tồn tại để xóa");
            }

            proimg.DeletedTime = DateTime.Now;
            proimg.DeletedBy = userID.ToString();

            await _unitOfWork.GetRepository<ProductImage>().UpdateAsync(proimg);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductImageModelViews>> GetImagesbyIdPro(string productId)
        {
            var existingProduct = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
            if (existingProduct == null || existingProduct.Id != productId)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "");
            }
            var productImages = await _unitOfWork.GetProductImagRepository().GetImagesbyIdPro(productId);
            if (productImages == null || !productImages.Any())
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy hình ảnh cho sản phẩm này");
            }
            var mappedList = productImages.Select(c => new ProductImageModelViews
            {
                Image = c.Image, 
                Description = c.Description 
            }).ToList();

            return mappedList;
        }

        public async Task<ProductImageModelViews> GetProductImageById(string id)
        {
            var existingProductImage = await _unitOfWork.GetRepository<ProductImage>()
                .Entities
                .Where(p => !p.DeletedTime.HasValue && p.Id == id)
                .FirstOrDefaultAsync();

            if (existingProductImage == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy id ProductImage");
            }

            return new ProductImageModelViews
            {
                Image = existingProductImage.Image,
                Description = existingProductImage.Description
            };
        }


    }
}