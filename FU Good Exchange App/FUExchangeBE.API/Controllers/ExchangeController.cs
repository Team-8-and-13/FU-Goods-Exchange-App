using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Base;
using FUExchange.Core.Constants;
using FUExchange.Core.Response;
using FUExchange.ModelViews.ExchangeModelViews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUExchangeBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class ExchangeController : Controller
    {
        private readonly IExchangeService _exchangeService;
        public ExchangeController(IExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllExchange(int pageIndex = 1, int pageSize = 2)
        {
            var exc = await _exchangeService.GetAllExchangeAsync(pageIndex, pageSize);
            return Ok(exc);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExchangeById(string id)
        {
            var exc = await _exchangeService.GetExchangeByIdAsync(id);
            return Ok(exc);
        }
        [HttpPost]
        public async Task<IActionResult> CreateExchange(CreateExchangeModelViews exchangemodelview)
        {
            await _exchangeService.CreateExchangeAsync(exchangemodelview);
            return Ok(new BaseResponse<string>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: "Tao trao doi thanh cong!!"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Updatexchange(string id, ExchangeModelViews updateExchangeModel)
        {
            await _exchangeService.UpdateExchangeAsync(id, updateExchangeModel);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     "Cập nhật thành công"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExchange(string id)
        {
            await _exchangeService.DeleteExchangeAsync(id);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     "Xóa trao đổi thành công!!!"));
        }

    }
}
