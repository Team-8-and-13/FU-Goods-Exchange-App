using FUExchange.ModelViews.UserModelViews;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Constants;
using FUExchange.Core.Response;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using static FUExchange.Core.Base.BaseException;
using FUExchange.Core.Base;

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

        [HttpGet("Get_All_Users")]
        public async Task<IActionResult> GetAllUsers(int pageIndex = 1, int pageSize = 10)
        {
            var users = await _userService.GetAll(pageIndex, pageSize);
            return Ok(new BaseResponseModel(
                StatusCodes.Status200OK,
                ResponseCodeConstants.SUCCESS,
                users));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userService.GetById(id);
                return Ok(new BaseResponseModel(
                    StatusCodes.Status200OK,
                    ResponseCodeConstants.SUCCESS,
                    user));
            }
            catch (ErrorException ex)
            {
                return NotFound(new BaseResponseModel(
                    StatusCodes.Status404NotFound,
                    ResponseCodeConstants.NOT_FOUND,
                    data: "Nguười dùng không tồn tại"));
            }
        }



        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserModel createUserModel)
        {
            var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var isSuccess = await _userService.CreateUser(createUserModel, adminId);
            if (isSuccess)
            {
                return Ok(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.OK,
                    code: StatusCodeHelper.OK.ToString(),
                    data: "Tạo người dùng thành công."));
            }

            return BadRequest(new BaseResponse<string>(
                statusCode: StatusCodeHelper.BadRequest,
                code: StatusCodeHelper.BadRequest.ToString(),
                data: "Tạo người dùng không thành công."));
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, UpdateUserModel updateUserModel)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get admin ID from token

                var isSuccess = await _userService.UpdateUser(userId, updateUserModel, adminId);
                if (isSuccess)
                {
                    return Ok(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.OK,
                    code: StatusCodeHelper.OK.ToString(),
                    data: "Cập nhật người dùng thành công."));
                }
                else
                {
                    return NotFound(new BaseResponse<string>(
                    statusCode: StatusCodeHelper.BadRequest,
                    code: StatusCodeHelper.BadRequest.ToString(),
                     data: "Cập nhật người dùng không thành công."));
                }
            }
            catch (ErrorException ex)
            {
                return NotFound(new BaseResponseModel(
                    StatusCodes.Status404NotFound,
                    ResponseCodeConstants.NOT_FOUND,
                    data: "Nguười dùng không tồn tại"));
            }

        }
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var adminId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value; // Get admin ID from token
                var isSuccess = await _userService.DeleteUser(userId, adminId);

                if (isSuccess)
                {
                    return Ok(new BaseResponse<string>(
                statusCode: StatusCodeHelper.OK,
                code: StatusCodeHelper.OK.ToString(),
                data: "Xóa người dùng thành công."));
                }

                else
                {
                    return BadRequest(new BaseResponse<string>(
                        statusCode: StatusCodeHelper.BadRequest,
                        code: StatusCodeHelper.BadRequest.ToString(),
                        data: "Xóa người dùng không thành công."
                    ));
                }
            }
            catch (ErrorException ex)
            {
                return NotFound(new BaseResponseModel(
                    StatusCodes.Status404NotFound,
                    ResponseCodeConstants.NOT_FOUND,
                    data: "Nguười dùng không tồn tại"));
            }
        }


    }
}
