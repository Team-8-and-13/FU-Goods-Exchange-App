using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.BanModelViews;
using FUExchange.ModelViews.CommentModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static FUExchange.Core.Base.BaseException;

namespace FUExchange.Services.Service
{
    public class BanService : IBanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BasePaginatedList<BanModelView>> GetAllBans(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Ban>().Entities.Where(p => !p.DeletedTime.HasValue);
            var paginatedList = await _unitOfWork.GetRepository<Ban>().GetPagging(query, pageIndex, pageSize);
            var mappedList = paginatedList.Items.Select(c => new BanModelView
            {
                ReportId=c.Id.ToString(),
                Expires=c.Expires
            }).ToList();
            return new BasePaginatedList<BanModelView>(mappedList, paginatedList.TotalItems, paginatedList.CurrentPage, paginatedList.PageSize);
        }
        

        public async Task<BanModelView?> GetBan(string id)
        {
            var ban = await _unitOfWork.GetRepository<Ban>().GetByIdAsync(id);
            if (ban == null)
            {
                throw new KeyNotFoundException("Không tìm thấy Ban.");
            }
            else if (ban.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Ban đã bị xóa.");
            }

<<<<<<< Updated upstream
            var banView = new BanModelView
            {
                ReportId=ban.Id,
                Expires=ban.Expires
            };

            return banView ??
                 throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy Ban"); 

=======
            return banmodelview ?? throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Not found. ");
>>>>>>> Stashed changes
        }

        public async Task CreateBan(CreateBanModelView createBanModel)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
<<<<<<< Updated upstream
            Guid userName = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value);
=======
            
            var rp = await _unitOfWork.GetRepository<Report>().GetByIdAsync(rpId) ?? 
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Report not found. ");
            
>>>>>>> Stashed changes
            if (createBanModel == null)
            {
                throw new KeyNotFoundException("Dữ liệu không hợp lệ.");
            }
            var ban = new Ban
            {
                ReportId=createBanModel.ReportId,
                Expires = createBanModel.Expires,
                CreatedBy = userName.ToString(),
                CreatedTime = DateTime.Now
            };
            await _unitOfWork.GetRepository<Ban>().InsertAsync(ban);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateBan(string id, CreateBanModelView updateBanModel)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userName = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value);

            var existBan = await _unitOfWork.GetRepository<Ban>().GetByIdAsync(id);

            if (updateBanModel == null)
            {
                throw new KeyNotFoundException("Dữ liệu không hợp lệ.");
            }
            if (existBan == null)
            {
                throw new KeyNotFoundException("Không tìm thấy Ban.");
            }
            else if (existBan.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Ban đã bị xóa.");
            }
            existBan.Expires = updateBanModel.Expires;
            existBan.LastUpdatedBy = userName.ToString();
            existBan.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.SaveAsync();
        }


        public async Task DeleteBan(string id)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userName = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value);
            var ban = await _unitOfWork.GetRepository<Ban>().GetByIdAsync(id);

            if (ban == null || ban.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Ban không tìm thấy hoặc đã bị xóa.");
            }

            ban.DeletedBy = userName.ToString();
            ban.DeletedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Ban>().UpdateAsync(ban);
            await _unitOfWork.SaveAsync();
        }

        public async Task<BasePaginatedList<BanModelView>> GetBanPaginated(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Ban>().Entities;
            var paginatedList = await _unitOfWork.GetRepository<Ban>().GetPagging(query, pageIndex, pageSize);
            var mappedList = paginatedList.Items.Select(c => new BanModelView
            {
                ReportId = c.Id.ToString(),
                Expires = c.Expires
            }).ToList();
            return new BasePaginatedList<BanModelView>(mappedList, paginatedList.TotalItems, paginatedList.CurrentPage, paginatedList.PageSize);
        }

    
    }
}
