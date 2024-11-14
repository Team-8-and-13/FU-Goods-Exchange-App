using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.ExchangeModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using static FUExchange.Core.Base.BaseException;


namespace FUExchange.Services.Service
{
    public class ExchangeService : IExchangeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ExchangeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task CreateExchangeAsync(CreateExchangeModelViews exc)
        {
            if (exc == null)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, ErrorMessages.ERROR);
            }
            Product? product = await _unitOfWork.GetRepository<Product>().GetByIdAsync(exc.ProductId) ??
                 throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy sản phẩm");
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            var userid = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            if(userid != product.SellerId)
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.NOT_FOUND, "Bạn không có quyền tạo lịch sử giao dịch cho sản phẩm này!");
            }

            var Exc = new Exchange
            {
                ProductId = exc.ProductId,
                BuyerId = exc.BuyerId,
                CreatedBy = userid.ToString(),
                CreatedTime = DateTime.Now
            };

            product.Sold = true;

            await _unitOfWork.GetRepository<Product>().UpdateAsync(product);
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
            if (EXC.CreatedBy != userId.ToString())
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, ResponseCodeConstants.UNAUTHORIZED, "Bạn không có quyền xóa giao dịch này !!!");
            }
            else
            {
                EXC.DeletedBy = userId.ToString();
                EXC.DeletedTime = DateTime.Now;
                await _unitOfWork.GetRepository<Exchange>().UpdateAsync(EXC);
                await _unitOfWork.SaveAsync();
            }
        }

        public async Task<BasePaginatedList<ExchangeModelViews>> GetAllExchangeAsync(int pageIndex, int pageSize)
        {

            var query = _unitOfWork.GetRepository<Exchange>().Entities.Where(p => !p.DeletedTime.HasValue);
            var totalCount = await query.CountAsync();
            var paginatedItems = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var mappedItems = paginatedItems.Select(exchange => new ExchangeModelViews
            {
                BuyerId = exchange.BuyerId.ToString(),
                ProductId = exchange.ProductId
            }).ToList();
            return new BasePaginatedList<ExchangeModelViews>(mappedItems, totalCount, pageIndex, pageSize);
        }

        public async Task<ExchangeModelViews?> GetExchangeByIdAsync(string id)
        {
            var exc = await _unitOfWork.GetRepository<Exchange>().GetByIdAsync(id);
            if (exc == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy trao đổi!!!");
            }
            var exchange = new ExchangeModelViews
            {
                BuyerId = exc.BuyerId.ToString(),
                ProductId = exc.ProductId
            };

            return exchange ??
                 throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Trao đổi"); ;

        }

        public Task<BasePaginatedList<Exchange>> GetExchangePaginatedAsync(int pageIndex, int pageSize)
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
