using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Constants;
using FUExchange.Core.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUExchangeBE.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("Get_Notification_For_User")]
        public async Task<IActionResult> GetNotificationsForUser(int pageIndex = 1, int pageSize = 2)
        {
            var notifications = await _notificationService.GetNotificationsForUser(pageIndex, pageSize);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     notifications));
        }

        [HttpGet]
        [Route("Get_Notification_For_User_By_Id")]
        public async Task<IActionResult> GetNotificationById(string id)
        {
            var notification = await _notificationService.GetNotificationById(id);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     notification));
        }

        [HttpDelete]
        [Route("Get_Notification_For_User_By_Id")]
        public async Task<IActionResult> Delete(string id)
        {
            await _notificationService.DeleteNotification(id);
            return Ok(new BaseResponseModel(
                     StatusCodes.Status200OK,
                     ResponseCodeConstants.SUCCESS,
                     "Delete successfully"));
        }
    }
}
