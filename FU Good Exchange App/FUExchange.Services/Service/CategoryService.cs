using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Core;
namespace FUExchange.Services.Service
{
    public class CategoryService
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


        public async Task<Category> CreateCategoryAsync(Category category)
        {
            await _unitOfWork.GetRepository<Category>().InsertAsync(category);
            await _unitOfWork.SaveAsync();
            return category;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            category.LastUpdatedTime = DateTime.Now;
            _unitOfWork.GetRepository<Category>().Update(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteCategoryAsync(string id)
        {
            await _unitOfWork.GetRepository<Category>().DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
        public async Task<BasePaginatedList<Category>> GetCategoryPaginatedAsync(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Category>().Entities;
            // Sử dụng phương thức GetPagging với IQueryable
            return await _unitOfWork.GetRepository<Category>().GetPagging(query, pageIndex, pageSize);
        }
    }
}
