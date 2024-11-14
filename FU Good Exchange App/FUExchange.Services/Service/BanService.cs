using AutoMapper;
using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Repositories.PaggingItems;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.BanModelViews;
using FUExchange.ModelViews.ProductModelViews;
using Microsoft.AspNetCore.Http;
using static FUExchange.Core.Base.BaseException;

namespace FUExchange.Services.Service
{
    public class BanService : IBanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<BanModelView>> GetAllBans(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Ban>().Entities.Where(p => !p.DeletedTime.HasValue).OrderByDescending(c => c.CreatedTime); ;
            List<BanModelView> bans = await query.ProjectToListAsync<BanModelView>(_mapper.ConfigurationProvider);
            return PaginatedList<BanModelView>.Create(bans, pageIndex, pageSize);
        }

        public async Task<BanModelView> GetBan(string id)
        {
            var ban = await _unitOfWork.GetRepository<Ban>().GetByIdAsync(id);
            if (ban == null)
            {
                throw new KeyNotFoundException("Ban not found.");
            }
            else if (ban.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Ban has been deleted.");
            }
            
            BanModelView banmodelview = _mapper.Map<BanModelView>(ban);

            return banmodelview ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy ");
        }

        public async Task ApproveReport(string rpId, CreateBanModelView createBanModel)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            
            var rp = await _unitOfWork.GetRepository<Report>().GetByIdAsync(rpId) ?? 
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Report");
            
            if (createBanModel == null)
            {
                throw new KeyNotFoundException("Invalid ban data.");
            }
            var ban = new Ban
            {
                ReportId=rp.Id,
                Expires = createBanModel.Expires,
                CreatedBy = User.Identity.Name,
                CreatedTime = DateTime.Now
            };
            rp.Status = true;
            await _unitOfWork.GetRepository<Ban>().InsertAsync(ban);
            await _unitOfWork.GetRepository<Report>().UpdateAsync(rp);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateBan(string id, CreateBanModelView updateBanModel)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;

            var existBan = await _unitOfWork.GetRepository<Ban>().GetByIdAsync(id);

            if (updateBanModel == null)
            {
                throw new KeyNotFoundException("Invalid ban data.");
            }
            if (existBan == null)
            {
                throw new KeyNotFoundException("Ban not found.");
            }
            else if (existBan.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Ban has been deleted.");
            }
            existBan.Expires = updateBanModel.Expires;
            existBan.LastUpdatedBy = User.Identity.Name;
            existBan.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.SaveAsync();
        }


        public async Task DeleteBan(string id)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            var ban = await _unitOfWork.GetRepository<Ban>().GetByIdAsync(id);

            if (ban == null || ban.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Ban not found or has been deleted.");
            }

            var rp = await _unitOfWork.GetRepository<Report>().GetByIdAsync(ban.ReportId);
            rp.Status = false;

            ban.DeletedBy = User.Identity.Name;
            ban.DeletedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Ban>().UpdateAsync(ban);
            await _unitOfWork.GetRepository<Report>().UpdateAsync(rp);
            await _unitOfWork.SaveAsync();
        }

    
    }
}
