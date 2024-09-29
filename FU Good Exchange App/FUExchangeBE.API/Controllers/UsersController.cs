using Microsoft.AspNetCore.Mvc;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Base;
using FUExchange.ModelViews.UserModelViews;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FUExchangeBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "AdminPolicy")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAll();
            return Ok(BaseResponse<IList<UserResponseModel>>.OkResponse(users));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetById(id);
            if (user == null) return NotFound();

            return Ok(BaseResponse<UserResponseModel>.OkResponse(user));
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserModel model)
        {
            var adminId = User.Identity?.Name;
            var result = await _userService.CreateUser(model, adminId);
            if (result) return Ok(BaseResponse<bool>.OkResponse("Taoj nguoi dung thanh cong"));

            return BadRequest(BaseResponse<bool>.ErrorResponse("Failed to create user"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserModel model)
        {
            var adminId = User.Identity?.Name; // Get the admin's ID
            var result = await _userService.UpdateUser(id, model, adminId);
            if (result) return Ok(BaseResponse<bool>.OkResponse("Sua nguoi dung thanh cong"));

            return BadRequest(BaseResponse<bool>.ErrorResponse("Failed to update user"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var adminId = User.Identity?.Name; // Get the admin's ID
            var result = await _userService.DeleteUser(id, adminId);
            if (result) return Ok(BaseResponse<bool>.OkResponse("Xoa nguoi dung thanh cong"));

            return BadRequest(BaseResponse<bool>.ErrorResponse("Failed to delete user"));
        }
    }
}
