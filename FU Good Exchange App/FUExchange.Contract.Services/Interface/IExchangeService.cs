using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using FUExchange.ModelViews.ExchangeModelViews;
using FUExchange.ModelViews.ExchangeModelViews;


namespace FUExchange.Contract.Services.Interface
{
    public interface IExchangeService
    {
        Task<BasePaginatedList<Exchange>> GetAllExchangeAsync(int pageIndex, int pageSize);
        Task<Exchange?> GetExchangeByIdAsync(string id);
        Task CreateExchangeAsync(CreateExchangeModelViews exc);
        Task UpdateExchangeAsync(ExchangeModelViews exc, string userId);
        Task DeleteExchangeAsync(string id);
        Task<BasePaginatedList<Exchange>> GetExchangePaginatedAsync(int pageIndex, int pageSize);
        Task UpdateExchangeAsync(string id, ExchangeModelViews updateExchangeModel);
    }
}
