using FUExchange.Contract.Repositories.Entity;

namespace FUExchange.Contract.Repositories.IUOW
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync(string categoryId);
    }
}
