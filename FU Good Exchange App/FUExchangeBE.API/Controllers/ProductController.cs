using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.ModelViews.ProductModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUExchangeBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "AdminPolicy")]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [Authorize(Policy = "ModeratorPolicy")]
        [HttpGet]
        [Route("Get_All_From_Moderator")]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("Get_All_From_User")]
        public async Task<IActionResult> GetAllApproveProducts()
        {
            var products = await _productService.GetAllApproveProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductModelView createProductModel)
        {
            if (createProductModel == null)
            {
                return BadRequest("Invalid product data.");
            }
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Unauthorized("User Name is not available in token.");
            }
            Guid sellerid = new Guid(User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            var product = new Product
            {
                CategoryId = createProductModel.CategoryId,
                Name = createProductModel.Name,
                Price = createProductModel.Price,
                Description = createProductModel.Description,
                SellerId = sellerid, // lấy userId
                CreatedBy = userName // Lấy userName từ token đã được authorize
            };

            var newProduct = await _productService.CreateProductAsync(product);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductModelView updateProductModel)
        {
            Guid userID = new Guid(User?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            Product existProduct = await _productService.GetProductByIdAsync(id);
            if (existProduct == null)
            {
                return NotFound();
            }
            if (existProduct.SellerId != userID) // Chỉ user tạo product mới được cập nhật product
            {
                return BadRequest();
            }
            existProduct.Name = updateProductModel.Name;
            existProduct.Price = updateProductModel.Price;
            existProduct.Description = updateProductModel.Description;
            existProduct.CategoryId = updateProductModel.CategoryId;
            existProduct.Image = updateProductModel.Image;
            existProduct.LastUpdatedBy = User.Identity?.Name;

            await _productService.UpdateProductAsync(existProduct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) {
                return NotFound();
            }
            product.DeletedBy = User.Identity?.Name;
            await _productService.DeleteProductAsync(id);
            return NoContent();
        }
        [HttpPut]
        [Route("api/controller/Rate_For_Product")]
        public async Task<IActionResult> RateProduct(string id, int star)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            if (star < 1 || star > 5)
            {
                return BadRequest();
            }
            await _productService.RateProduct(id, star);
            return NoContent();
        }

        [Authorize(Policy = "ModeratorPolicy")]
        [HttpPut]
        [Route("api/controller/Approve_Product")]
        public async Task<IActionResult> Approve(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            await _productService.ApproveProduct(id);
            return NoContent();
        }
    }
}
