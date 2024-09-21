using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUExchangeBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]

    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
        [HttpGet("{categoryId}/products")]
        public async Task<IActionResult> GetAllProductsAsync(string categoryId)
        {
            var products = await _categoryService.GetAllProductsAsync(categoryId);
            if (products == null || !products.Any()) // Kiểm tra nếu không có sản phẩm nào
                return NotFound(); // Trả về 404 nếu không tìm thấy sản phẩm
            return Ok(products); // Trả về 200 và danh sách sản phẩm
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryModelViews createCategoryModel)
        {
            if (createCategoryModel == null)
            {
                return BadRequest("Invalid category data.");
            }
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is not available in token.");
            }
            

            var category = new Category
            {
                Name = createCategoryModel.Name,
                CreatedBy = userId // Lấy idUser từ token đã được authorize
            };

            var newCategory = await _categoryService.CreateCategoryAsync(category, userId);
            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Id }, newCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, CreateCategoryModelViews updateCategoryModel)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
                return NotFound();

            existingCategory.Name = updateCategoryModel.Name;
            var userId = User.Identity?.Name;
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await _categoryService.UpdateCategoryAsync(existingCategory, userId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var userId = User.Identity?.Name;
            //var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await _categoryService.DeleteCategoryAsync(id, userId);
            return NoContent();
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetCategoryPaginated([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 2)
        {
            var paginatedCategory = await _categoryService.GetCategoryPaginatedAsync(pageIndex, pageSize);
            return Ok(paginatedCategory);
        }
    }
}
