using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using FUExchange.ModelViews.BanModelViews;


namespace FUExchange.Contract.Services.Interface
{
    public interface IBanService
    {
<<<<<<< Updated upstream
        Task<BasePaginatedList<BanModelView>> GetAllBans(int pageIndex, int pageSize);
        Task<BanModelView?> GetBan(string id);
        Task CreateBan(CreateBanModelView createBanModel);
=======
        Task<BasePaginatedList<BanModelView>> GetAllBans(int pageIndex, int pageSize);
        Task<BanModelView> GetBan(string id);
        Task ApproveReport(string rpId, CreateBanModelView createBanModel);
>>>>>>> Stashed changes
        Task UpdateBan(string id, CreateBanModelView updateBanModel);
        Task DeleteBan(string id);
        Task<BasePaginatedList<BanModelView>> GetBanPaginated( int pageIndex, int pageSize);
    }
}
