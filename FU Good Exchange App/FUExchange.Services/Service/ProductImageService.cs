using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.ProductImagesModelViews;
using Microsoft.AspNetCore.Http;
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
            var idProduct = await _unitOfWork.GetProductRepository().GetByIdAsync(idPro);
            if (idProduct == null)
            {
                throw new KeyNotFoundException("Find not id product.");
                //throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Invalid product image data.");
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

            if (updateproimg == null)
            {
                throw new KeyNotFoundException("Invalid product image data.");
                //throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Invalid product image data.");
            }

            var existingProimg = await _unitOfWork.GetRepository<ProductImage>().GetByIdAsync(id);

            if (existingProimg == null)
            {
                throw new KeyNotFoundException("Find not id ProductImage.");
                //throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy id ProductImage.");
            }
            else if (existingProimg.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("ProductImage was deleted.");
                //throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "ProductImage này đã bị xóa");
            }

            existingProimg.Image = updateproimg.Image;
            existingProimg.Description = updateproimg.Description;
            existingProimg.LastUpdatedBy = userID.ToString();
            existingProimg.LastUpdatedTime = DateTime.Now;

            await _unitOfWork.SaveAsync();
        }

        public async Task<ProductImage> DeleteProductImage(string id)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            var proimg = await _unitOfWork.GetRepository<ProductImage>().GetByIdAsync(id);
            if (proimg == null)
            {
                throw new KeyNotFoundException("Find not id ProductImage.");
                //throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy id ProductImage");
            }

            proimg.DeletedTime = DateTime.Now;
            proimg.DeletedBy = userID.ToString();

            await _unitOfWork.GetRepository<ProductImage>().UpdateAsync(proimg);
            await _unitOfWork.SaveAsync();
            return proimg;
        }

        public async Task<IEnumerable<ProductImage>> GetImagesbyIdPro(string productId)
        {
            var existingProimg = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
            if (existingProimg == null)
            {
                throw new KeyNotFoundException("Find not id Product.");
                //throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy id Product");
            }
            return await _unitOfWork.GetProductImagRepository().GetImagesbyIdPro(productId);
        }

        public async Task<ProductImage> GetProductImageById(string id)
        {
            var existingProimg = await _unitOfWork.GetRepository<ProductImage>().GetByIdAsync(id);
            if (existingProimg == null)
            {
                throw new KeyNotFoundException("Find not id ProductImage.");
                //throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy id ProductImage"); ;
            }
            return await _unitOfWork.GetProductImagRepository().GetByIdAsync(id);
            //return await _unitOfWork.GetRepository<ProductImage>().GetByIdAsync(id);
        }
    }
}