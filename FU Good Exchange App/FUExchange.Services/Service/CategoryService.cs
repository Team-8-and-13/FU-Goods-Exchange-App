using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.ModelViews.ProductModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static FUExchange.Core.Base.BaseException;

namespace FUExchange.Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BasePaginatedList<Category>> GetAllCategories(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Category>().Entities.Where(p => !p.DeletedTime.HasValue);
            return await _unitOfWork.GetRepository<Category>().GetPagging(query, pageIndex, pageSize);
        }

        public async Task<CategoriesModelView?> GetCategoryById(string id)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }
            else if(category.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Category has been deleted.");
            }

            var cate = new CategoriesModelView
            {
                CategoryId = category.Id,
                Name = category.Name
            };

            return cate ??
                 throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Product"); ;
        }

        public async Task CreateCategory(CreateCategoryModelViews createCategoryModel)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            if(createCategoryModel == null)
            {
                throw new KeyNotFoundException("Invalid category data.");
            }
            var category = new Category
            {
                Name = createCategoryModel.Name,
                CreatedBy = userID.ToString(),
                CreatedTime = DateTime.Now
            };
            await _unitOfWork.GetRepository<Category>().InsertAsync(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategory(string id, CreateCategoryModelViews updateCategoryModel)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value); 
            var existingCategory = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);

            if (updateCategoryModel == null)
            {
                throw new KeyNotFoundException("Invalid category data.");
            }
            if (existingCategory == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }
            else if (existingCategory.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Category has been deleted.");
            }
            existingCategory.Name = updateCategoryModel.Name;
            existingCategory.LastUpdatedBy = userID.ToString();
            existingCategory.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.SaveAsync();
        }


        public async Task<Category> DeleteCategory(string id)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);

            if (category == null || category.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Category not found or has been deleted.");
            }

            category.DeletedBy = userID.ToString();
            category.DeletedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await _unitOfWork.SaveAsync();
            return category;
        }

        public async Task<BasePaginatedList<Category>> GetCategoryPaginated(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Category>().Entities;
            return await _unitOfWork.GetRepository<Category>().GetPagging(query, pageIndex, pageSize);
        }

        public async Task<BasePaginatedList<Product>> GetAllProductsbyIdCategory(string categoryId, int pageIndex, int pageSize)
        {
            var findIDCate = await _unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId);
            if (findIDCate == null)
            {
                throw new KeyNotFoundException("Category not found.");
            }
            else if(findIDCate.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Category has been deleted.");
            }
            var query = _unitOfWork.GetRepository<Product>().Entities
                                  .Where(p => !p.DeletedTime.HasValue && p.CategoryId == categoryId);
            if (!await query.AnyAsync())
            {
                throw new KeyNotFoundException("No products found for the given category.");
            }
            return await _unitOfWork.GetRepository<Product>().GetPagging(query, pageIndex, pageSize);
        }
    }
}
