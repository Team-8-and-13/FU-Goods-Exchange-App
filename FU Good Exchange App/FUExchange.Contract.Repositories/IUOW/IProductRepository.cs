using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
namespace FUExchange.Contract.Repositories.IUOW
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllApproveAsync();
        Task ApproveProduct(string id);
        Task Sold(string id);
        Task RateProduct(string id, int star);
    }
}
