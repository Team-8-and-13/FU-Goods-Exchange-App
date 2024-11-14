﻿using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Base;
using FUExchange.Core.Constants;
using FUExchange.Core.Response;
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
            catch (BaseException.ErrorException ex)
            {
                return BadRequest(
                    new BaseResponseModel(
                        ex.StatusCode,
                         ex.ErrorDetail.ErrorCode.ToString(),
                         ex.ErrorDetail.ErrorMessage.ToString())
                    );
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryModelViews createCategoryModel)
        {
            
            try
            {
                await _categoryService.CreateCategory(createCategoryModel);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         "Tạo thành công."));
            }
            catch (BaseException.ErrorException ex)
            {
                return BadRequest(
                    new BaseResponseModel(
                        ex.StatusCode,
                         ex.ErrorDetail.ErrorCode.ToString(),
                         ex.ErrorDetail.ErrorMessage.ToString())
                    );
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, CreateCategoryModelViews updateCategoryModel)
        {
            try
            {
                await _categoryService.UpdateCategory(id, updateCategoryModel);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         "Sửa thành công."));
            }
            catch (BaseException.ErrorException ex)
            {
                return BadRequest(
                    new BaseResponseModel(
                        ex.StatusCode,
                         ex.ErrorDetail.ErrorCode.ToString(),
                         ex.ErrorDetail.ErrorMessage.ToString())
                    );
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            try
            {
                await _categoryService.DeleteCategory(id);
                return Ok(new BaseResponseModel(
                        StatusCodes.Status200OK,
                        ResponseCodeConstants.SUCCESS,
                        "Xóa thành công."));
            }
            catch (BaseException.ErrorException ex)
            {
                return BadRequest(
                    new BaseResponseModel(
                        ex.StatusCode,
                         ex.ErrorDetail.ErrorCode.ToString(),
                         ex.ErrorDetail.ErrorMessage.ToString())
                    );
            }
        }
    }
}