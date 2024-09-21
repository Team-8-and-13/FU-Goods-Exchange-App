using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using FUExchange.ModelViews.UserModelViews;


namespace FUExchange.Contract.Services.Interface
{
    public interface IExchangeService
    {
        Task<IEnumerable<Exchange>> GetAllExchangeAsync();
        Task<Exchange?> GetExchangeByIdAsync(string id);
        Task CreateExchangeAsync(Exchange exc, string userId);
        Task<Exchange> UpdateExchangeAsync(Exchange exc, string userId);
        Task<Exchange> DeleteExchangeAsync(string id, string userId);
        Task<BasePaginatedList<Exchange>> GetExchangePaginatedAsync(int pageIndex, int pageSize);
    }
}
