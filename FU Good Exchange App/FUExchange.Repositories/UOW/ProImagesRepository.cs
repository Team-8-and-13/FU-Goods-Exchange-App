using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.IUOW;
using FUExchange.Repositories.Context;
using Microsoft.EntityFrameworkCore;

namespace FUExchange.Repositories.UOW
{
    public class ProImagesRepository : GenericRepository<ProductImage>, IProImagesRepository
    {
        public readonly DatabaseContext _context;
        public ProImagesRepository(DatabaseContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ProductImage>> GetImagesbyIdPro(string productId)
        {
            return await _context.ProductImages
                                 .Where(img => img.ProductId == productId && img.DeletedTime == null)
                                 .ToListAsync();
        }
        public async Task<ProductImage> GetProductImageByIdAsync(string id)
        {
            return await _context.ProductImages.FindAsync(id);
        }
    }
    
}
