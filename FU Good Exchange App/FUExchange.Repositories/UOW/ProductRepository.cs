using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.IUOW;
using FUExchange.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace FUExchange.Repositories.UOW
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public readonly DatabaseContext _context;
        public ProductRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllApproveAsync()
        {
            return await _context.Products.Where(p => p.Approve == true).ToListAsync();
        }
        public async Task ApproveProduct(string id)
        {
            var product = await _context.Products.FindAsync(id);
            product.Approve = true;
            await SaveAsync();
        }
        public async Task Sold(string id)
        {
            var product = await _context.Products.FindAsync(id);
            product.Sold = true;
            await SaveAsync();
        }
        public async Task RateProduct(string id, int star)
        {
            Product? product = await _context.Products.FindAsync(id);
            product.Rating += 1;
            product.NumberOfStar = (double)(product.NumberOfStar + star) / product.Rating;
            await SaveAsync();
        }
    }
}
