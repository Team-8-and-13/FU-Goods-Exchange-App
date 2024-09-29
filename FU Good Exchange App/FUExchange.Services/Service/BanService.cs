﻿using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.BanModelViews;
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

        public async Task<BasePaginatedList<Ban>> GetAllBans(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Ban>().Entities.Where(p => !p.DeletedTime.HasValue);
            return await _unitOfWork.GetRepository<Ban>().GetPagging(query, pageIndex, pageSize);
        }

        public async Task<BanModelView?> GetBan(string id)
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

            var ba = new BanModelView
            {
                ReportId = ban.Id,
                Expires = ban.Expires
            };

            return ba ??
                 throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "Không tìm thấy "); ;
        }

        public async Task CreateBan(CreateBanModelView createBanModel)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userName = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value);
            if (createBanModel == null)
            {
                throw new KeyNotFoundException("Invalid ban data.");
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
            existBan.LastUpdatedBy = userName.ToString();
            existBan.LastUpdatedTime = DateTime.Now;
            await _unitOfWork.SaveAsync();
        }


        public async Task<Ban> DeleteBan(string id)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userName = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserName")?.Value);
            var ban = await _unitOfWork.GetRepository<Ban>().GetByIdAsync(id);

            if (ban == null || ban.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Ban not found or has been deleted.");
            }

            ban.DeletedBy = userName.ToString();
            ban.DeletedTime = DateTime.Now;
            await _unitOfWork.GetRepository<Ban>().UpdateAsync(ban);
            await _unitOfWork.SaveAsync();
            return ban;
        }

        public async Task<BasePaginatedList<Ban>> GetBanPaginated(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Ban>().Entities;
            return await _unitOfWork.GetRepository<Ban>().GetPagging(query, pageIndex, pageSize);
        }

    
    }
}