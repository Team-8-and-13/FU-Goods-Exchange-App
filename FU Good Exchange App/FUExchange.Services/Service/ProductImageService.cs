using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.IUOW;
using FUExchange.Contract.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FUExchange.Services.Service
{
    public class ProductImageService : IProImagesService
    {
        private readonly IProImagesRepository _proImagesRepository;
        public ProductImageService(IProImagesRepository proImagesRepository)
        {
            _proImagesRepository = proImagesRepository;
        }

        public async Task<ProductImage> CreateProductImageAsync(ProductImage proimg)
        {
            await _proImagesRepository.InsertAsync(proimg);
            await _proImagesRepository.SaveAsync(); // Lưu thay đổi vào database
            return proimg;
        }

        public async Task<ProductImage> UpdateProductIamgeAsync(ProductImage proimg)
        {
            proimg.LastUpdatedTime = DateTime.Now;
            await _proImagesRepository.UpdateAsync(proimg);
            await _proImagesRepository.SaveAsync(); // Lưu thay đổi vào database
            return proimg;
        }

        public async Task<ProductImage> DeleteProductIamgeAsync(string id)
        {
            var proimg = await _proImagesRepository.GetProductImageByIdAsync(id);
            if (proimg == null)
                return null; // Kiểm tra nếu hình ảnh không tồn tại
            proimg.DeletedTime = DateTime.Now; // Đánh dấu là đã bị xóa
            await _proImagesRepository.UpdateAsync(proimg);
            await _proImagesRepository.SaveAsync(); // Lưu thay đổi vào database
            return proimg;
        }

        public async Task<IEnumerable<ProductImage>> GetImagesbyIdPro(string productId)
        {
            return await _proImagesRepository.GetImagesbyIdPro(productId); // Gọi repository để lấy danh sách hình ảnh theo ProductId
        }

        public async Task<ProductImage> GetProductImageByIdAsync(string id)
        {
            return await _proImagesRepository.GetProductImageByIdAsync(id); // Gọi repository để lấy một hình ảnh cụ thể theo Id
        }
    }
}
