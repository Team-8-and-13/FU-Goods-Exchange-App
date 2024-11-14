using Microsoft.AspNetCore.Mvc;
using FUExchange.ModelViews.AuthModelViews;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Base;
using FUExchange.Core.Constants;
using System;
using System.Threading.Tasks;

namespace FUExchangeBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("auth_account")]
        public async Task<IActionResult> Login(LoginModelView model)
        {
            try
            {
                var token = await _authService.LoginAsync(model);
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized(new BaseResponse<string>(
                        statusCode: StatusCodeHelper.Unauthorized,
                        code: StatusCodeHelper.Unauthorized.ToString(),
                        data: "Đăng nhập thất bại!"));
                }

                return Ok(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.OK,
                    code: StatusCodeHelper.OK.ToString(),
                    data: token));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: ResponseCodeConstants.ERROR,
                    data: ex.Message));
            }
        }

        [HttpPost("new_account")]
        public async Task<IActionResult> Register(RegisterModelView model)
        {
            try
            {
                var result = await _authService.RegisterAsync(model);
                if (result.Succeeded)
                {
                    return Ok(new BaseResponse<string>(
                        statusCode: StatusCodeHelper.OK,
                        code: StatusCodeHelper.OK.ToString(),
                        data: "Đăng ký thành công."));
                }

                return BadRequest(new BaseResponse<object>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: StatusCodeHelper.BadRequest.ToString(),
                    data: result.Errors));
            }
            catch (Exception ex)
            {
                return BadRequest(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: ResponseCodeConstants.ERROR,
                    data: ex.Message));
            }
        }
    }
}
