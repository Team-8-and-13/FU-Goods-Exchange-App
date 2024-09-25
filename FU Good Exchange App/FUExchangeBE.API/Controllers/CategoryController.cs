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
            var categories = await _categoryService.GetAllCategories();
            return Ok(new BaseResponse<IEnumerable<CategoriesModelView>>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: categories));
        }
        [HttpGet("{categoryId}/products")]
        public async Task<IActionResult> GetAllProducts(string categoryId)
        {
            var products = await _categoryService.GetAllProducts(categoryId);
            return Ok(new BaseResponse<IEnumerable<SelectProductModelView>>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: products));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var category = await _categoryService.GetCategoryById(id);
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
            await _categoryService.CreateCategory(createCategoryModel);
            return Ok(new BaseResponse<string>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: "Create sucessfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, CreateCategoryModelViews updateCategoryModel)
        {
            if (updateCategoryModel == null)
            {
                return BadRequest("Invalid category data.");
            }
            await _categoryService.UpdateCategory(id, updateCategoryModel);
            return Ok(new BaseResponse<string>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: "Update sucessfully."));
        }



        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok(new BaseResponse<string>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: "Delete sucessfully."));
        }

        [HttpGet("paged")]
        public async Task<IActionResult> GetCategoryPaginated([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 2)
        {
            var paginatedCategory = await _categoryService.GetCategoryPaginated(pageIndex, pageSize);
            return Ok(paginatedCategory);
        }
    }
}
