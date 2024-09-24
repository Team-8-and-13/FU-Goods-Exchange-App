using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;

namespace FUExchange.Services.Service
{
    public class ExchangeService : IExchangeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExchangeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateExchangeAsync(Exchange exc, string userId)
        {
            exc.CreatedBy = userId;
            exc.CreatedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Exchange>().InsertAsync(exc);
            await _unitOfWork.SaveAsync();
        }

        public async Task<Exchange> DeleteExchangeAsync(string id, string userId)
        {
            var exc = await _unitOfWork.GetRepository<Exchange>().GetByIdAsync(id);
            if (exc == null) throw new Exception("Exchange not found");

            exc.DeletedBy = userId;
            exc.DeletedTime = DateTime.Now;

            await _unitOfWork.GetRepository<Exchange>().UpdateAsync(exc);
            await _unitOfWork.SaveAsync();
            return exc;
        }

        public async Task<IEnumerable<Exchange>> GetAllExchangeAsync()
        {
            return await _unitOfWork.GetRepository<Exchange>().GetAllAsync();
        }

        public async Task<Exchange?> GetExchangeByIdAsync(string id)
        {
            return await _unitOfWork.GetRepository<Exchange>().GetByIdAsync(id);
        }

        public Task<BasePaginatedList<Exchange>> GetExchangePaginatedAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public async Task<Exchange> UpdateExchangeAsync(Exchange exc, string userId)
        {
            exc.LastUpdatedBy = userId;
            exc.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Exchange>().UpdateAsync(exc);
            await _unitOfWork.SaveAsync();
            return exc;
        }
    }
}
