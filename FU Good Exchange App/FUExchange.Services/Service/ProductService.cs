using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Utils;
using FUExchange.ModelViews.ProductModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace FUExchange.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.GetRepository<Product>().Entities.Where(p => !p.DeletedTime.HasValue).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllApproveProductsAsync()
        {
            return await _unitOfWork.GetProductRepository().GetAllApproveAsync();
        }
        public async Task<Product?> GetProductByIdAsync(string id)
        {
            return await _unitOfWork.GetRepository<Product>().GetByIdAsync(id);
        }
        public async Task CreateProduct(CreateProductModelView createProductModelView)
        {
            if (createProductModelView == null)
            {
                throw new Exception("Data error !!!");
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
                throw new Exception("Not found product need updating");
            }
            if (existProduct.SellerId != userID) // Chỉ user tạo product mới được cập nhật product
            {
                throw new Exception("No Authorization !!!");
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
                throw new Exception("Not found product !!!");
            }
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            var userName = User.Identity.Name;
            Guid userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            if (product.SellerId != userId)
            {
                throw new Exception("No authorization");
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
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception("Not found product");
            }
            if (star < 1 || star > 5)
            {
                throw new Exception("Bad request");
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
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id);
            if (product == null)
            {

                throw new Exception("Not found product");
            }
            else
            {
                await _unitOfWork.GetProductRepository().ApproveProduct(id);
            }
        }
    }
}
