using FUExchange.Contract.Repositories.Entity;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUExchangeBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Policy = "Admin")]

    public class CategoryController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
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
            var category = new Category
            {

                Name = createCategoryModel.Name,
            };
            var newCategory = await _categoryService.CreateCategoryAsync(category);
            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Id }, newCategory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, CreateCategoryModelViews updateCategoryModel)
        {
            var existingCategory = await _categoryService.GetCategoryByIdAsync(id);
            if (existingCategory == null)
                return NotFound();

            existingCategory.Name = updateCategoryModel.Name;

            await _categoryService.UpdateCategoryAsync(existingCategory);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategoryAsync(id);
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
