using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     product));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductModelView createProductModel)
        {
            await _productService.CreateProduct(createProductModel);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     "Create successfully."));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductModelView updateProductModel)
        {
            await _productService.UpdateProduct(id, updateProductModel);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     "Update successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _productService.DeleteProduct(id);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     "Delete successfully"));
        }
        [HttpPut]
        [Route("api/controller/Rate_For_Product")]
        public async Task<IActionResult> RateProduct(string id, int star)
        {
            await _productService.RateProduct(id, star);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     "Rate successfully"));
        }

        [Authorize(Policy = "ModeratorPolicy")]
        [HttpPut]
        [Route("api/controller/Approve_Product")]
        public async Task<IActionResult> Approve(string id)
        {
            await _productService.ApproveProduct(id);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     "Approve successfully"));
        }
    }
}
