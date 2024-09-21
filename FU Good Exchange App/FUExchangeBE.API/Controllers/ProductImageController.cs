using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.ModelViews.ProductImagesModelViews;
using FUExchange.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FUExchangeBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class ProductImageController : ControllerBase
    {
        private readonly IProImagesService _proimgService;

        public ProductImageController(IProImagesService proimgService)
        {
            _proimgService = proimgService;
        }

        // Lấy hình ảnh sản phẩm theo Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductImageById(string id)
        {
            var productImage = await _proimgService.GetProductImageByIdAsync(id); // Gọi service để lấy ảnh theo id
            if (productImage == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy ảnh
            }
            return Ok(productImage); // Trả về 200 cùng với dữ liệu ảnh
        }

        // Lấy danh sách hình ảnh theo ProductId
        [HttpGet("by-product/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(string productId)
        {
            var productImages = await _proimgService.GetImagesbyIdPro(productId); // Gọi service để lấy danh sách hình ảnh theo ProductId
            if (productImages == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy ảnh
            }
            return Ok(productImages); // Trả về 200 cùng với danh sách hình ảnh
        }

        // Tạo mới một ảnh sản phẩm
        [HttpPost]
        public async Task<IActionResult> CreateProductImage(CreateProductImageModelViews createProductImageModel)
        {
            if (createProductImageModel == null)
            {
                return BadRequest("Invalid product image data."); // Trả về 400 nếu dữ liệu không hợp lệ
            }

            var userName = User.Identity?.Name; // Lấy userName từ token đã authorize
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("User Name is not available in token."); // Trả về 401 nếu không có userName
            }

            var proimg = new ProductImage
            {
                ProductId = createProductImageModel.ProductId,
                Description = createProductImageModel.Description,
                Image = createProductImageModel.Image,
                CreatedBy = userName // Gán người tạo ảnh từ token
            };

            var newProductImg = await _proimgService.CreateProductImageAsync(proimg); // Gọi service để tạo mới ảnh
            return CreatedAtAction(nameof(GetProductImageById), new { id = newProductImg.Id }, newProductImg); // Trả về 201 cùng với URL của ảnh mới
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductImage(string id, UpdateProductImageModelViews updateProductImageModel)
        {
            if (updateProductImageModel == null)
            {
                return BadRequest("Invalid product image data.");
            }

            var existingProductImage = await _proimgService.GetProductImageByIdAsync(id);
            if (existingProductImage == null)
            {
                return NotFound();
            }

            existingProductImage.Description = updateProductImageModel.Description;
            existingProductImage.Image = updateProductImageModel.Image;
            existingProductImage.LastUpdatedBy = User.Identity?.Name;

            var updatedProductImg = await _proimgService.UpdateProductIamgeAsync(existingProductImage); // Gọi service để cập nhật ảnh
            return Ok(updatedProductImg); // Trả về 200 cùng với ảnh đã cập nhật
        }

        // Xóa một ảnh sản phẩm (Đánh dấu đã xóa)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductImage(string id)
        {
            var existingProductImage = await _proimgService.GetProductImageByIdAsync(id);
            if (existingProductImage == null)
            {
                return NotFound(); // Trả về 404 nếu không tìm thấy ảnh
            }
            existingProductImage.DeletedBy = User.Identity?.Name;
            var deletedProductImage = await _proimgService.DeleteProductIamgeAsync(id); // Gọi service để đánh dấu đã xóa
            return Ok(deletedProductImage); // Trả về 200 cùng với ảnh đã xóa
        }
    }
}
