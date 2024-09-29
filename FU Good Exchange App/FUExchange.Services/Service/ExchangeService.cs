using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.ExchangeModelViews;
using Microsoft.AspNetCore.Http;
using static FUExchange.Core.Base.BaseException;

namespace FUExchange.Services.Service
{
    public class ExchangeService : IExchangeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ExchangeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateExchangeAsync(CreateExchangeModelViews excmodel)
        {
            if (excmodel == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, ErrorMessages.ERROR);
            }

            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;

            var Exc = new Exchange
            {
                ProductId = excmodel.ProductId,
                BuyerId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value),
                CreatedBy = User.Identity?.Name,
                CreatedTime = DateTime.Now
            };

            await _unitOfWork.GetRepository<Exchange>().InsertAsync(Exc);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteExchangeAsync(string id)
        {
            var EXC = await _unitOfWork.GetRepository<Exchange>().GetByIdAsync(id);
            if (EXC == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Trao đổi");
            }
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            var userName = User.Identity.Name;
            Guid userId = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            if (EXC.BuyerId != userId)
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "Unauthorized !!!");
            }
            else
            {
                EXC.DeletedBy = userName;
                await _unitOfWork.GetRepository<Exchange>().UpdateAsync(EXC);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<BasePaginatedList<Exchange>> GetAllExchangeAsync(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Exchange>().Entities.Where(p => !p.DeletedTime.HasValue);
            return await _unitOfWork.GetRepository<Exchange>().GetPagging(query, pageIndex, pageSize);
        }

        public async Task<Exchange?> GetExchangeByIdAsync(string id)
        {
            return await _unitOfWork.GetRepository<Exchange>().GetByIdAsync(id) ??
            throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy trao đổi");
        }

        public Task<BasePaginatedList<Exchange>> GetExchangePaginatedAsync(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task UpdateExchangeAsync(ExchangeModelViews exc, string userId)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateExchangeAsync(string id, ExchangeModelViews updateExchangeModel)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            var existingExchange = await _unitOfWork.GetRepository<Exchange>().GetByIdAsync(id);

            if (existingExchange == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Trao đổi");
            }
            if (updateExchangeModel == null)
                throw new KeyNotFoundException("Không có dữ liệu hợp lệ");
            existingExchange.ProductId = updateExchangeModel.ProductId;
            existingExchange.BuyerId = Guid.Parse(updateExchangeModel.BuyerId);
            existingExchange.LastUpdatedBy = userID.ToString();
            existingExchange.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Exchange>().UpdateAsync(existingExchange);
            await _unitOfWork.SaveAsync();
        }
    }
}
