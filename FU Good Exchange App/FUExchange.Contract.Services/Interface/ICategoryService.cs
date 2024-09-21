using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;

namespace FUExchange.Contract.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(string id);
        Task<Category> CreateCategoryAsync(Category category, string userId); 
        Task<Category> UpdateCategoryAsync(Category category, string userId); 
        Task<Category> DeleteCategoryAsync(string id, string userId); 
        Task<BasePaginatedList<Category>> GetCategoryPaginatedAsync(int pageIndex, int pageSize);
        Task<IEnumerable<Product>> GetAllProductsAsync(string categoryId);
    }
}
