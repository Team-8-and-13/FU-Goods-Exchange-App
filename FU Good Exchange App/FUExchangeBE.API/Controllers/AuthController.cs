using Microsoft.AspNetCore.Mvc;
using FUExchange.ModelViews.AuthModelViews;
using FUExchange.Contract.Services.Interface;
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
            var token = await _authService.LoginAsync(model);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }
            return Ok(new { Token = token });
        }

        [HttpPost("new_account")]
        public async Task<IActionResult> Register(RegisterModelView model)
        {
            var result = await _authService.RegisterAsync(model);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest(result.Errors);
        }
    }
}