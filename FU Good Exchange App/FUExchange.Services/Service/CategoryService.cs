using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.ModelViews.ProductModelViews;
using Microsoft.EntityFrameworkCore;

namespace FUExchange.Services.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoriesModelView>> GetAllCategoriesAsync()
        {
            var categories = await _unitOfWork.GetRepository<Category>().Entities.Where(t => !t.DeletedTime.HasValue).ToListAsync();
            return categories.Select(cate => new CategoriesModelView
            {
                Name = cate.Name,
                CategoryId = cate.Id
            }).ToList();
        }

        public async Task<CategoriesModelView?> GetCategoryByIdAsync(string id)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            if (category == null || category.DeletedTime.HasValue)
            {
                return null;
            }

            var cate = new CategoriesModelView 
            { 
                CategoryId = category.Id, 
                Name = category.Name 
            };

            return cate;
        }

        public async Task CreateCategoryAsync(CreateCategoryModelViews createCategoryModel, string userId)
        {
            var category = new Category
            {
                Name = createCategoryModel.Name,
                CreatedBy = userId,
                CreatedTime = DateTime.Now
            };
            await _unitOfWork.GetRepository<Category>().InsertAsync(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(string id, CreateCategoryModelViews updateCategoryModel, string userId)
        {
            var existingCategory = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            if (existingCategory == null || existingCategory.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Category not found or has been deleted.");
            }
            existingCategory.Name = updateCategoryModel.Name;
            existingCategory.LastUpdatedBy = userId;
            existingCategory.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.SaveAsync();
        }


        public async Task<Category> DeleteCategoryAsync(string id, string userId)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            if (category == null) throw new Exception("Category not found");
            category.DeletedBy = userId;
            category.DeletedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await _unitOfWork.SaveAsync();
            return category;
        }

        public async Task<BasePaginatedList<Category>> GetCategoryPaginatedAsync(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Category>().Entities;
            return await _unitOfWork.GetRepository<Category>().GetPagging(query, pageIndex, pageSize);
        }

        public async Task<IEnumerable<SelectProductModelView>> GetAllProductsAsync(string categoryId)
        {
            var pro = await _unitOfWork.GetRepository<Product>().Entities.Where(t => !t.DeletedTime.HasValue && t.CategoryId == categoryId).ToListAsync();
            return pro.Select(cate => new SelectProductModelView
            {
                CategoryId = cate.CategoryId,
                Name = cate.Name,
                Description = cate.Description,
                Price = cate.Price,
            }).ToList();
        }

    }
}
