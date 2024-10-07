using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Base;
using FUExchange.Core.Constants;
using FUExchange.Core.Response;
using FUExchange.ModelViews.CategoryModelViews;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductImageById(string id)
        {
            try
            {
                var productImage = await _proimgService.GetProductImageById(id);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         productImage));
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

        [HttpGet("by-product/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(string productId)
        {
                var productImages = await _proimgService.GetImagesbyIdPro(productId);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         productImages));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductImage(CreateProductImageModelViews createProductImageModel, string idPro)
        {
                await _proimgService.CreateProductImage(createProductImageModel, idPro);
                return Ok(new BaseResponse<string>(
                 statusCode: StatusCodeHelper.OK,
                 code: StatusCodeHelper.OK.ToString(),
                 data: "Thêm thành công"));
           
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductImage(string id, UpdateProductImageModelViews updateProductImageModel)
        {
                await _proimgService.UpdateProductImage(id, updateProductImageModel);
                return Ok(new BaseResponse<string>(
                 statusCode: StatusCodeHelper.OK,
                 code: StatusCodeHelper.OK.ToString(),
                 data: "Sửa thành công"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductImage(string id)
        {
                await _proimgService.DeleteProductImage(id);
                return Ok(new BaseResponse<string>(
                 statusCode: StatusCodeHelper.OK,
                 code: StatusCodeHelper.OK.ToString(),
                 data: "Xóa thành công"));
            
        }
    }
}