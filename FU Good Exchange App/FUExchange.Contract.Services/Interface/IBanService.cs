using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using FUExchange.ModelViews.BanModelViews;


namespace FUExchange.Contract.Services.Interface
{
    public interface IBanService
    {
        Task<BasePaginatedList<Ban>> GetAllBans(int pageIndex, int pageSize);
        Task<Ban> GetBan(string id);
        Task ApproveReport(string rpId, CreateBanModelView createBanModel);
        Task UpdateBan(string id, CreateBanModelView updateBanModel);
        Task<Ban> DeleteBan(string id);
        Task<BasePaginatedList<Ban>> GetBanPaginated( int pageIndex, int pageSize);
    }
}
