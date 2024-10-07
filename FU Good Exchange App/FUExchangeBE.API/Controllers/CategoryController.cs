﻿using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Base;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.CategoryModelViews;
using FUExchange.ModelViews.ProductModelViews;
using FUExchange.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        /// <summary>

        [HttpGet]
        public async Task<IActionResult> GetAllCategories(int pageIndex = 1, int pageSize = 2)
        {
            var categories = await _categoryService.GetAllCategories(pageIndex, pageSize);
            return Ok(new BaseResponse<BasePaginatedList<CategoriesModelView>>(
                    statusCode: StatusCodeHelper.OK,
                    code: StatusCodeHelper.OK.ToString(),
                    data: categories
                ));
        }

        [HttpGet("{categoryId}/products")]
        public async Task<IActionResult> GetAllProductsByIDCategory(string categoryId, int pageIndex = 1, int pageSize = 2)
        {
            try
            {
                var products = await _categoryService.GetAllProductsbyIdCategory(categoryId, pageIndex, pageSize);
                return Ok(new BaseResponse<BasePaginatedList<ProductByCategoryModelViews>>(
                    statusCode: StatusCodeHelper.OK,
                    code: StatusCodeHelper.OK.ToString(),
                    data: products
                ));
            }
            catch (BaseException.ErrorException ex)
            {
                return StatusCode(ex.StatusCode, new BaseResponse<string>(
                    statusCode: (StatusCodeHelper)ex.StatusCode,
                    code: ex.ErrorDetail.ErrorCode,
                    data: ex.ErrorDetail.ErrorMessage?.ToString() ?? "Lỗi!"
                ));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: ResponseCodeConstants.NOT_FOUND,
                    data: ex.Message
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BaseResponse<string>(
                    statusCode: StatusCodeHelper.ServerError,
                    code: "server_error",
                    data: ex.Message
                ));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                return Ok(new BaseResponse<CategoriesModelView>(
                 statusCode: StatusCodeHelper.OK,
                 code: StatusCodeHelper.OK.ToString(),
                 data: category));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: ResponseCodeConstants.NOT_FOUND,
                    data: ex.Message
                ));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryModelViews createCategoryModel)
        {
            
            try
            {
                await _categoryService.CreateCategory(createCategoryModel);
                return Ok(new BaseResponse<string>(
                 statusCode: StatusCodeHelper.OK,
                 code: StatusCodeHelper.OK.ToString(),
                 data: "Tạo thành công"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: ResponseCodeConstants.NOT_FOUND,
                    data: ex.Message
                ));
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, CreateCategoryModelViews updateCategoryModel)
        {
            try
            {
                await _categoryService.UpdateCategory(id, updateCategoryModel);
                return Ok(new BaseResponse<string>(
                 statusCode: StatusCodeHelper.OK,
                 code: StatusCodeHelper.OK.ToString(),
                 data: "Sửa thành công"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: ResponseCodeConstants.NOT_FOUND,
                    data: ex.Message
                ));
            }            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return Ok(new BaseResponse<string>(
                 statusCode: StatusCodeHelper.OK,
                 code: StatusCodeHelper.OK.ToString(),
                 data: "Xóa thành công"));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: ResponseCodeConstants.NOT_FOUND,
                    data: ex.Message
                ));
            }
        }
    }
}