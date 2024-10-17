using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Base;
using FUExchange.Core.Constants;
using FUExchange.Core.Response;
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
        public async Task<IActionResult> GetAllProducts(int pageIndex = 1, int pageSize = 2)
        {
            var products = await _productService.GetAllProductsFromModerator(pageIndex, pageSize);
            return Ok(products);
        }

        [HttpGet]
        [Route("Get_All_From_User")]
        public async Task<IActionResult> GetAllApproveProducts(int pageIndex = 1, int pageSize = 2)
        {
            var products = await _productService.GetAllProductsFromUser(pageIndex, pageSize);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     products));
        }

        [HttpGet]
        [Route("Get_Product_By_ProductId")]
        public async Task<IActionResult> GetProductById(string id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         product));
            }
            catch (BaseException.ErrorException ex) {
                return BadRequest(
                    new BaseResponseModel(
                        ex.StatusCode,
                         ex.ErrorDetail.ErrorCode.ToString(),
                         ex.ErrorDetail.ErrorMessage.ToString())
                    );
            }

            
        }

        [HttpGet]
        [Route("Get_Product_By_CommentId")]
        public async Task<IActionResult> GetProductByCommentId(string CommentId)
        {
            try
            {
                var product = await _productService.GetProductByCommentId(CommentId);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         product));
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
        public async Task<IActionResult> CreateProduct(CreateProductModelView createProductModel)
        {
            try
            {
                await _productService.CreateProduct(createProductModel);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         "Create successfully."));
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

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductModelView updateProductModel)
        {
            try
            {
                await _productService.UpdateProduct(id, updateProductModel);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         "Update successfully"));
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
                await _productService.DeleteProduct(id);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         "Delete successfully"));
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
        [HttpPut]
        [Route("api/controller/Rate_For_Product")]
        public async Task<IActionResult> RateProduct(string id, int star)
        {
            try
            {
                await _productService.RateProduct(id, star);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         "Rate successfully"));
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

        [Authorize(Policy = "ModeratorPolicy")]
        [HttpPut]
        [Route("api/controller/Approve_Product")]
        public async Task<IActionResult> Approve(string id)
        {
            try
            {
                await _productService.ApproveProduct(id);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         "Approve successfully"));
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
