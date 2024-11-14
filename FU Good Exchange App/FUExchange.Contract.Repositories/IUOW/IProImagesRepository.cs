using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.Contract.Repositories.IUOW
{
    public interface IProImagesRepository : IGenericRepository<ProductImage>
    {
        Task<IEnumerable<ProductImage>> GetImagesbyIdPro(string productId); // Trả về danh sách hình ảnh theo ProductId
        Task<ProductImage> GetProductImageByIdAsync(string id);
    }
}
