using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Base;
using FUExchange.Core.Constants;
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
            var productImage = await _proimgService.GetProductImageByIdAsync(id);
            if (productImage == null)
            {
                return NotFound("find not image");
            }
            return Ok(productImage);
        }

        [HttpGet("by-product/{productId}")]
        public async Task<IActionResult> GetImagesByProductId(string productId)
        {
            var productImages = await _proimgService.GetImagesbyIdPro(productId); 
            if (productImages == null)
            {
                return NotFound("find not image"); 
            }
            return Ok(productImages); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductImage(CreateProductImageModelViews createProductImageModel, string idPro)
        {
            if (createProductImageModel == null)
            {
                return BadRequest("Invalid product image data.");
            }
            await _proimgService.CreateProductImageAsync(createProductImageModel, idPro);
            return Ok(new BaseResponse<string>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: "Create sucessfully."));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductImage(string id, UpdateProductImageModelViews updateProductImageModel)
        {
            if (updateProductImageModel == null)
            {
                return BadRequest("Invalid product image data.");
            }
            await _proimgService.UpdateProductIamgeAsync(id, updateProductImageModel);
            return Ok(new BaseResponse<string>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: "Update sucessfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductImage(string id)
        {
            await _proimgService.DeleteProductIamgeAsync(id);
            return Ok(new BaseResponse<string>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: "Delete sucessfully."));
        }
    }
}
