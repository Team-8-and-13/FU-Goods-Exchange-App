using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
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

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _unitOfWork.GetRepository<Category>().GetAllAsync();
        }

        public async Task<Category?> GetCategoryByIdAsync(string id)
        {
            return await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(Category category, string userId)
        {
            category.CreatedBy = userId;
            category.CreatedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Category>().InsertAsync(category);
            await _unitOfWork.SaveAsync();
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category, string userId)
        {
            category.LastUpdatedBy = userId;
            category.LastUpdatedTime = DateTime.Now;
            _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await _unitOfWork.SaveAsync();
            return category;
        }

        public async Task<Category> DeleteCategoryAsync(string id, string userId)
        {
            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(id);
            if (category == null) throw new Exception("Category not found");

            category.DeletedBy = userId;
            category.DeletedTime = DateTime.Now;

            _unitOfWork.GetRepository<Category>().UpdateAsync(category);
            await _unitOfWork.SaveAsync();
            return category;
        }

        public async Task<BasePaginatedList<Category>> GetCategoryPaginatedAsync(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Category>().Entities;
            return await _unitOfWork.GetRepository<Category>().GetPagging(query, pageIndex, pageSize);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync(string categoryId)
        {
            return await _unitOfWork.GetRepository<Product>()
                                     .Entities
                                     .Where(p => p.CategoryId == categoryId) 
                                     .ToListAsync();
        }


    }
}
