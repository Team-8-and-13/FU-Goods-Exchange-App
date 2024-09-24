using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.ModelViews.ProductModelViews;

namespace FUExchange.Contract.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoriesModelView>> GetAllCategoriesAsync();
        Task<CategoriesModelView?> GetCategoryByIdAsync(string id);
        Task CreateCategoryAsync(CreateCategoryModelViews createCategoryModel, string userId);
        Task UpdateCategoryAsync(string id, CreateCategoryModelViews updateCategoryModel, string userId);
        Task<Category> DeleteCategoryAsync(string id, string userId); 
        Task<BasePaginatedList<Category>> GetCategoryPaginatedAsync(int pageIndex, int pageSize);
        Task<IEnumerable<SelectProductModelView>> GetAllProductsAsync(string categoryId);
    }
}
