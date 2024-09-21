using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.Contract.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<IEnumerable<Product>> GetAllApproveProductsAsync();
        Task<Product?> GetProductByIdAsync(string id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<Product> DeleteProductAsync(string id);
        Task ApproveProduct(string id);
        Task SoldProduct(string id);
        Task RateProduct(string id, int star);
        //Task<BasePaginatedList<Product>> GetProductPaginatedAsync(int pageIndex, int pageSize);
    }
}
