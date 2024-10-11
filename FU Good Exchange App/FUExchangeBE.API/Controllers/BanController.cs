using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Base;
using FUExchange.Core.Constants;
using FUExchange.Core.Response;
using FUExchange.ModelViews.BanModelViews;
using FUExchange.Services.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUExchangeBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "ModeratorPolicy")]

    public class BanController : Controller
    {
        private readonly IBanService _banService;

        public BanController(IBanService banService)
        {
            _banService = banService;
        }
        /// <summary>

        [HttpGet]
        public async Task<IActionResult> GetAllBans(int pageIndex = 1, int pageSize = 2)
        {
            var bans = await _banService.GetAllBans(pageIndex, pageSize);
            return Ok(bans);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBan(string id)
        {    
                var ban = await _banService.GetBan(id);
                return Ok(new BaseResponseModel(
                         StatusCodes.Status200OK,
                         ResponseCodeConstants.SUCCESS,
                         ban));
           
        }

        [HttpPost]
        public async Task<IActionResult> ApproveReport(string rpId, CreateBanModelView createBanModel)
        {

            try
            {
                await _banService.ApproveReport(rpId, createBanModel);
                return Ok(new BaseResponse<string>(
                 statusCode: StatusCodeHelper.OK,
                 code: StatusCodeHelper.OK.ToString(),
                 data: "Ban sucessfully."));
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
        public async Task<IActionResult> UpdateBan(string id, CreateBanModelView updateBanModel)
        {
            try
            {
                await _banService.UpdateBan(id, updateBanModel);
                return Ok(new BaseResponse<string>(
                 statusCode: StatusCodeHelper.OK,
                 code: StatusCodeHelper.OK.ToString(),
                 data: "Update sucessfully."));
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
        public async Task<IActionResult> DeleteBan(string id)
        {
            try
            {
                await _banService.DeleteBan(id);
                return Ok(new BaseResponse<string>(
                 statusCode: StatusCodeHelper.OK,
                 code: StatusCodeHelper.OK.ToString(),
                 data: "Delete sucessfully."));
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
