using FUExchange.Contract.Repositories.PaggingItems;
using FUExchange.ModelViews.NotificationModelViews;

namespace FUExchange.Contract.Services.Interface
{
    public interface INotificationService
    {
        Task<PaginatedList<NotificationDisplay>> GetNotificationsForUser(int pageIndex, int pageSize);
        Task<NotificationDisplay?> GetNotificationById(string id);
        Task DeleteNotification(string id);
    }
}
