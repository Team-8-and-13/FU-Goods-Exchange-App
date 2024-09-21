using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Repositories.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.Services.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _unitOfWork.GetRepository<Product>().Entities.Where(p => !p.DeletedTime.HasValue).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllApproveProductsAsync()
        {
            return await _unitOfWork.GetProductRepository().GetAllApproveAsync();
        }
        public async Task<Product?> GetProductByIdAsync(string id)
        {
            return await _unitOfWork.GetRepository<Product>().GetByIdAsync(id);
        }
        public async Task<Product> CreateProductAsync(Product product)
        {
            await _unitOfWork.GetRepository<Product>().InsertAsync(product);
            await _unitOfWork.SaveAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            product.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Product>().UpdateAsync(product);
            await _unitOfWork.SaveAsync();
            return product;
        }
        public async Task<Product> DeleteProductAsync(string id)
        {
            var product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(id);
            product.DeletedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Product>().UpdateAsync(product);
            await _unitOfWork.SaveAsync();
            return product;
        }

        public async Task RateProduct(string id, int star)
        {
            await _unitOfWork.GetProductRepository().RateProduct(id, star);
        }
        public async Task SoldProduct(string id)
        {
            await _unitOfWork.GetProductRepository().Sold(id);
        }
        public async Task ApproveProduct(string id)
        {
            await _unitOfWork.GetProductRepository().ApproveProduct(id);
        }
    }
}
