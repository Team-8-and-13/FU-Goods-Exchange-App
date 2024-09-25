using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.ModelViews.ProductModelViews;

namespace FUExchange.Contract.Services.Interface
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoriesModelView>> GetAllCategories();
        Task<CategoriesModelView?> GetCategoryById(string id);
        Task CreateCategory(CreateCategoryModelViews createCategoryModel);
        Task UpdateCategory(string id, CreateCategoryModelViews updateCategoryModel);
        Task<Category> DeleteCategory(string id);
        Task<BasePaginatedList<Category>> GetCategoryPaginated(int pageIndex, int pageSize);
        Task<IEnumerable<SelectProductModelView>> GetAllProducts(string categoryId);
    }
}
