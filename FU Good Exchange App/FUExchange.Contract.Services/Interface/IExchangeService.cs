using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using FUExchange.ModelViews.ExchangeModelViews;

namespace FUExchange.Contract.Services.Interface
{
    public interface IExchangeService
    {
        Task<BasePaginatedList<ExchangeModelViews>> GetAllExchangeAsync(int pageIndex, int pageSize);
        Task<ExchangeModelViews?> GetExchangeByIdAsync(string id);
        Task CreateExchangeAsync(CreateExchangeModelViews exc);
        Task UpdateExchangeAsync(ExchangeModelViews exc, string userId);
        Task DeleteExchangeAsync(string id);
        Task<BasePaginatedList<Exchange>> GetExchangePaginatedAsync(int pageIndex, int pageSize);
        Task UpdateExchangeAsync(string id, ExchangeModelViews updateExchangeModel);
    }
}
