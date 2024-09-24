using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Base;
using FUExchange.Core.Constants;
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
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("Get_All_From_User")]
        public async Task<IActionResult> GetAllApproveProducts()
        {
            var products = await _productService.GetAllApproveProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(CreateProductModelView createProductModel)
        {
            await _productService.CreateProduct(createProductModel);
            return Ok(new BaseResponse<string>(
              statusCode: StatusCodeHelper.OK,
              code: StatusCodeHelper.OK.ToString(),
              data: "Create product sucessfully."));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductModelView updateProductModel)
        {
            await _productService.UpdateProduct(id, updateProductModel);
            return Ok(new BaseResponse<string>(
              statusCode: StatusCodeHelper.OK,
              code: StatusCodeHelper.OK.ToString(),
              data: "Update product sucessfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _productService.DeleteProduct(id);
            return Ok(new BaseResponse<string>(
              statusCode: StatusCodeHelper.OK,
              code: StatusCodeHelper.OK.ToString(),
              data: "Delete product sucessfully."));
        }
        [HttpPut]
        [Route("api/controller/Rate_For_Product")]
        public async Task<IActionResult> RateProduct(string id, int star)
        {
            await _productService.RateProduct(id, star);
            return Ok(new BaseResponse<string>(
              statusCode: StatusCodeHelper.OK,
              code: StatusCodeHelper.OK.ToString(),
              data: "Rate product sucessfully."));
        }

        [Authorize(Policy = "ModeratorPolicy")]
        [HttpPut]
        [Route("api/controller/Approve_Product")]
        public async Task<IActionResult> Approve(string id)
        {
            await _productService.ApproveProduct(id);
            return Ok(new BaseResponse<string>(
              statusCode: StatusCodeHelper.OK,
              code: StatusCodeHelper.OK.ToString(),
              data: "Approve product sucessfully."));
        }
    }
}
