using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using FUExchange.ModelViews.ProductModelViews;
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
        Task CreateProduct(CreateProductModelView createProductModelView);
        Task UpdateProduct(string productId, UpdateProductModelView updateProductModelView);
        Task DeleteProduct(string id);
        Task ApproveProduct(string id);
        Task SoldProduct(string id);
        Task RateProduct(string id, int star);
        //Task<BasePaginatedList<Product>> GetProductPaginatedAsync(int pageIndex, int pageSize);
    }
}
