using AutoMapper;
using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Repositories.PaggingItems;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.NotificationModelViews;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FUExchange.Core.Base.BaseException;

namespace FUExchange.Services.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
        }
        public async Task<PaginatedList<NotificationDisplay>> GetNotificationsForUser(int pageIndex, int pageSize)
        {
            var user = _contextAccessor.HttpContext?.User ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Đăng nhập để xem thông báo");
            Guid userid = new Guid(user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            IQueryable<Notification> query = _unitOfWork.GetRepository<Notification>().Entities.Where(n => n.UserId == userid).OrderByDescending(c => c.CreatedTime);
            List<NotificationDisplay> notifications = await query.ProjectToListAsync<NotificationDisplay>(_mapper.ConfigurationProvider);
            return PaginatedList<NotificationDisplay>.Create(notifications, pageIndex, pageSize);
        }

        public async Task<NotificationDisplay?> GetNotificationById(string id)
        {
            Notification notification = await _unitOfWork.GetRepository<Notification>().GetByIdAsync(id) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy thông báo");
            NotificationDisplay notificationDisplay = _mapper.Map<NotificationDisplay>(notification);
            return notificationDisplay;
        }
        public async Task DeleteNotification(string id)
        {
            var user = _contextAccessor.HttpContext?.User ??
               throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Đăng nhập để xem thông báo");
            Guid userid = new Guid(user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            
            Notification notification = await _unitOfWork.GetRepository<Notification>().GetByIdAsync(id) ??
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy thông báo");
            
            if(userid != notification.UserId)
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "Bạn không có quyền xóa thông báo này !");
            }

            await _unitOfWork.GetRepository<Notification>().DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}
