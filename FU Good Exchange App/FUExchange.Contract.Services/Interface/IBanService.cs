using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.PaggingItems;
using FUExchange.Core;
using FUExchange.ModelViews.BanModelViews;
using FUExchange.ModelViews.ProductModelViews;


namespace FUExchange.Contract.Services.Interface
{
    public interface IBanService
    {
        Task<PaginatedList<BanModelView>> GetAllBans(int pageIndex, int pageSize);
        Task<BanModelView> GetBan(string id);
        Task ApproveReport(string rpId, CreateBanModelView createBanModel);
        Task UpdateBan(string id, CreateBanModelView updateBanModel);
        Task DeleteBan(string id);
    }
}
