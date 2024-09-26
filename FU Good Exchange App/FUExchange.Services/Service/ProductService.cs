using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.Core.Utils;
using FUExchange.ModelViews.ProductModelViews;
using FUExchange.Repositories.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static FUExchange.Core.Base.BaseException;


namespace FUExchange.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<BasePaginatedList<Product>> GetAllProductsFromModerator(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Product>().Entities.Where(p => !p.DeletedTime.HasValue);
            return await _unitOfWork.GetRepository<Product>().GetPagging(query, pageIndex, pageSize);
        }
        public async Task<BasePaginatedList<Product>> GetAllProductsFromUser(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Product>().Entities.Where(p => !p.DeletedTime.HasValue && p.Approve);
            return await _unitOfWork.GetRepository<Product>().GetPagging(query, pageIndex, pageSize);
        }
        public async Task<Product?> GetProductByIdAsync(string id)
        {
            return await _unitOfWork.GetRepository<Product>().GetByIdAsync(id) ??
                 throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Product");
        }
        public async Task CreateProduct(CreateProductModelView createProductModelView)
        {
            if (createProductModelView == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Error Product");
            }
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            var product = new Product
            {
                CategoryId = createProductModelView.CategoryId,
                Name = createProductModelView.Name,
                Price = createProductModelView.Price,
                Description = createProductModelView.Description,
                SellerId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value), // lấy userId
                CreatedBy = User.Identity.Name // Lấy userName từ token đã được authorize
            };

            await _unitOfWork.GetRepository<Product>().InsertAsync(product);
        }

        public async Task UpdateProduct(string productId, UpdateProductModelView updateProductModelView)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            Product existProduct = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId);
            if (existProduct == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Product");
            }
            if (existProduct.SellerId != userID) // Chỉ user tạo product mới được cập nhật product
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "Unauthorized !!!");
            }
            existProduct.Name = updateProductModelView.Name;
            existProduct.Price = updateProductModelView.Price;
            existProduct.Description = updateProductModelView.Description;
            existProduct.CategoryId = updateProductModelView.CategoryId;
            existProduct.Image = updateProductModelView.Image;
            existProduct.LastUpdatedBy = userID.ToString();

            await _unitOfWork.GetRepository<Product>().UpdateAsync(existProduct);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteProduct(string id)
        {
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id);
            if (product == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Product");
            }
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            var userName = User.Identity.Name;
            Guid userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            if (product.SellerId != userId)
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "Unauthorized !!!");
            }
            else
            {
                product.DeletedBy = userName;
                await _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                await _unitOfWork.SaveAsync();
            }          
        }

        public async Task RateProduct(string id, int star)
        {
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Product");
            if (star < 1 || star > 5)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Số sao đánh giá chỉ được xác định trong khoảng 1 đến 5");
            }
            else
            {
                await _unitOfWork.GetProductRepository().RateProduct(id, star);
            }           
        }
        public async Task SoldProduct(string id)
        {
            await _unitOfWork.GetProductRepository().Sold(id);
        }
        public async Task ApproveProduct(string id)
        {
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Product");
            
            await _unitOfWork.GetProductRepository().ApproveProduct(id);
            
        }
    }
}
