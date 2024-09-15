using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;

namespace FUExchange.Contract.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(string id);
        Task CreateCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(string id);
        Task<BasePaginatedList<Category>> GetCategoryPaginatedAsync(int pageIndex, int pageSize);
    }
}
