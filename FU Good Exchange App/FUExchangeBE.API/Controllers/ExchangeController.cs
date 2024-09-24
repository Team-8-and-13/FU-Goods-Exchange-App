using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Base;
using FUExchange.Core.Constants;
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
        public async Task<IActionResult> GetAllExchange()
        {
            var exchanges = await _exchangeService.GetAllExchangeAsync();
            return Ok(new BaseResponse<IEnumerable<Exchange>>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: exchanges));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExchangeById(string id)
        {
            var EXC = await _exchangeService.GetExchangeByIdAsync(id);
            if (EXC == null)
                return NotFound();

            return Ok(EXC);
        }
        [HttpPost]
        public async Task<IActionResult> CreateExchange(ExchangeModelViews exchangemodelview)
        {
            if (exchangemodelview == null)
            {
                return BadRequest("Invalid category data.");
            }
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is not available in token.");
            }


            var exc = new Exchange
            {
                BuyerId =  Guid.Parse(exchangemodelview.BuyerId),
                ProductId = exchangemodelview.ProductId,
                CreatedBy = userId
            };

            await _exchangeService.CreateExchangeAsync(exc, userId);
            return Ok(new BaseResponse<string>(
             statusCode: StatusCodeHelper.OK,
             code: StatusCodeHelper.OK.ToString(),
             data: "Tao trao doi thanh cong!!"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Updatexchange(string id, ExchangeModelViews updateExchangeModel)
        {
            var existExchange = await _exchangeService.GetExchangeByIdAsync(id);
            if (existExchange == null)
                return NotFound();

            existExchange.BuyerId = Guid.Parse(updateExchangeModel.BuyerId);
            existExchange.ProductId = updateExchangeModel.ProductId;
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await _exchangeService.UpdateExchangeAsync(existExchange, userId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExchange(string id)
        {
            var userId = User.Identity?.Name;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            await _exchangeService.DeleteExchangeAsync(id, userId);
            return NoContent();
        }

    }
}
