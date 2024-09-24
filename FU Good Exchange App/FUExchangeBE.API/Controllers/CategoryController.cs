using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Base;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.ModelViews.ProductModelViews;
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
            return Ok(new BaseResponse<IEnumerable<CategoriesModelView>>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: categories));
        }
        [HttpGet("{categoryId}/products")]
        public async Task<IActionResult> GetAllProductsAsync(string categoryId)
        {
            var products = await _categoryService.GetAllProductsAsync(categoryId);
            return Ok(new BaseResponse<IEnumerable<SelectProductModelView>>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: products));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound("Category not found or has been deleted.");

            return Ok(new BaseResponse<CategoriesModelView>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: category));
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
            await _categoryService.CreateCategoryAsync(createCategoryModel, userId);
            return Ok("Create succcess");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, CreateCategoryModelViews updateCategoryModel)
        {
            if (updateCategoryModel == null)
            {
                return BadRequest("Invalid category data.");
            }
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is not available in token.");
            }
            await _categoryService.UpdateCategoryAsync(id, updateCategoryModel, userId);
            return Ok("Update success"); 
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
