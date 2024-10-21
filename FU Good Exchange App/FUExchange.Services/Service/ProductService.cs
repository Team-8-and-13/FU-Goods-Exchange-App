using AutoMapper;
using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Repositories.PaggingItems;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.ProductModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;
using static FUExchange.Core.Base.BaseException;


namespace FUExchange.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<BasePaginatedList<Product>> GetAllProductsFromModerator(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Product>().Entities.Where(p => !p.DeletedTime.HasValue).OrderByDescending(c => c.CreatedTime);
            return await _unitOfWork.GetRepository<Product>().GetPagging(query, pageIndex, pageSize);
        }
        public async Task<PaginatedList<SelectProductModelView>> GetAllProductsFromUser(int pageIndex, int pageSize)
        {
            IQueryable<Product> query = _unitOfWork.GetRepository<Product>().Entities.Where(p => !p.DeletedTime.HasValue && p.Approve && !p.Sold).OrderByDescending(c => c.CreatedTime);
            List<SelectProductModelView> products = await query.ProjectToListAsync<SelectProductModelView>(_mapper.ConfigurationProvider);
            return PaginatedList<SelectProductModelView>.Create(products, pageIndex, pageSize);
        }
        public async Task<SelectProductModelView?> GetProductByIdAsync(string id)
        {
            Product? product = await _unitOfWork.GetRepository<Product>().Entities.Where(c => c.Id == id && c.Approve == true && !c.DeletedTime.HasValue && !c.Sold).FirstOrDefaultAsync() ??
                 throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy sản phẩm");
            SelectProductModelView responeProduct = _mapper.Map<SelectProductModelView>(product);
            return responeProduct;
        }
        public async Task<SelectProductModelView?> GetProductByCommentId(string id)
        {
            Comment? comment = await _unitOfWork.GetRepository<Comment>().GetByIdAsync(id) ?? 
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy bình luận");

            Product? product = await _unitOfWork.GetRepository<Product>().Entities.Where(c => c.Id == comment.ProductId && c.Approve == true && !c.DeletedTime.HasValue && !c.Sold).FirstOrDefaultAsync() ??
                 throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy sản phẩm");
            SelectProductModelView responeProduct = _mapper.Map<SelectProductModelView>(product);
            return responeProduct;
        }

        public async Task CreateProduct(CreateProductModelView createProductModelView)
        {
            //Lấy thông tin user hiện hành
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var user = httpContext.HttpContext?.User;

            if (string.IsNullOrEmpty(createProductModelView.CategoryId))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không cho phép bỏ trống trường CategoryId");
            }
            if (string.IsNullOrEmpty(createProductModelView.Name))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không cho phép bỏ trống trường Name");
            }
            if (string.IsNullOrEmpty(createProductModelView.Description))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không cho phép bỏ trống trường Description");
            }
            if (createProductModelView.Price == 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không cho phép nhập giá trị là 0");
            }

            var product = new Product
            {
                CategoryId = createProductModelView.CategoryId,
                Name = createProductModelView.Name,
                Price = createProductModelView.Price < 0 ? throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.INVALID_INPUT, "Giá tiền phải là số nguyên dương") : createProductModelView.Price,
                Description = createProductModelView.Description,
                SellerId = new Guid(user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value), // lấy userId
                CreatedBy = user.Identity?.Name // Lấy userName từ token đã được authorize
            };

            await _unitOfWork.GetRepository<Product>().InsertAsync(product);
        }

        public async Task UpdateProduct(string productId, UpdateProductModelView updateProductModelView)
        {
            //Lấy thông tin user hiện hành
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            Product? existProduct = await _unitOfWork.GetRepository<Product>().GetByIdAsync(productId) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy sản phẩm");
            
            if (existProduct.SellerId != userID) // Chỉ user tạo product mới được cập nhật product
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "Bạn không có quyền chỉnh sửa sản phẩm này");
            }
            
            //Cập nhật hoặc giữ nguyên giá trị cho sản phẩm
            existProduct.Name = !string.IsNullOrEmpty(updateProductModelView.Name) ? updateProductModelView.Name : existProduct.Name;
            existProduct.Price = (updateProductModelView.Price != 0 || updateProductModelView.Price < 0) ? updateProductModelView.Price : existProduct.Price;
            existProduct.Description = !string.IsNullOrEmpty(updateProductModelView.Description) ? updateProductModelView.Description : existProduct.Description;
            existProduct.CategoryId = !string.IsNullOrEmpty(updateProductModelView.CategoryId) ? updateProductModelView.CategoryId : existProduct.CategoryId;
            existProduct.Image = !string.IsNullOrEmpty(updateProductModelView.Image) ? updateProductModelView.Image : existProduct.Image;
            existProduct.LastUpdatedBy = userID.ToString();

            await _unitOfWork.GetRepository<Product>().UpdateAsync(existProduct);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteProduct(string id)
        {
            Product? product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy sản phẩm");
            
            //Lấy thông tin user hiện hành
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            var userName = User.Identity.Name;
            Guid userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            
            if (product.SellerId != userId)
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "Bạn không có quyền xóa sản phẩm này!");
            }
            else
            {
                product.DeletedBy = userName;//id 
                product.DeletedTime = DateTime.Now;
                await _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                await _unitOfWork.SaveAsync();
            }          
        }

        public async Task RateProduct(string id, int star)
        {
            Product? product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy sản phẩm");
            if (star < 1 || star > 5)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Số sao đánh giá chỉ được xác định trong khoảng 1 đến 5");
            }
            else
            {
                product.Rating += 1;
                product.TotalStar = product.TotalStar + star;
                product.NumberOfStar = Convert.ToDouble(string.Format("{0:00.0}", (double)product.TotalStar / product.Rating));
                await _unitOfWork.GetRepository<Product>().UpdateAsync(product);
                await _unitOfWork.SaveAsync(); ;
            }           
        }
        public async Task SoldProduct(string id)
        {
            Product? product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy sản phẩm");
            
            product.Sold = true;
            await _unitOfWork.GetRepository<Product>().UpdateAsync(product);
            await _unitOfWork.SaveAsync();
        }
        public async Task ApproveProduct(string id)
        {
            Product? product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy sản phẩm");

            product.Approve = true;
            await _unitOfWork.GetRepository<Product>().UpdateAsync(product);
            await _unitOfWork.SaveAsync();

        }
    }
}
