using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.ReportModelsView;
using FUExchange.ModelViews.ReportModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static FUExchange.Core.Base.BaseException;

namespace FUExchange.Services.Service
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReportService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ReportResponseModel?> GetReportByIdAsync(string id)
        {
            var report = await _unitOfWork.GetRepository<Report>().GetByIdAsync(id) ??  throw new KeyNotFoundException("Report not found.");
            if (report.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Report has been deleted.");
            }

            return new ReportResponseModel
            {
                Id = report.Id.ToString(),
                Reason = report.Reason,
                Status = report.Status
            };
        }
        public async Task<ReportResponseModel?> GetReportById(string id)
        {
            var report = await _unitOfWork.GetRepository<Report>().GetByIdAsync(id);
            if (report == null)
            {
                throw new KeyNotFoundException("Report not found.");
            }
            else if (report.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Report has been deleted.");
            }

            return new ReportResponseModel
            {
                Id = report.Id, // Nếu Id là Guid
                UserId = report.UserId, // Gán UserId đúng kiểu Guid
                Reason = report.Reason,
                Status = report.Status
            };
        }

        public async Task<BasePaginatedList<Report>> GetAllReports(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Report>().Entities.Where(r => !r.DeletedTime.HasValue);
            return await _unitOfWork.GetRepository<Report>().GetPagging(query, pageIndex, pageSize);
        }

        

        public async Task CreateReport(ReportRequestModel reportRequest)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);
            if (reportRequest == null || string.IsNullOrEmpty(reportRequest.UserId))
            {
                throw new ArgumentException("UserId is required.");
            }

            // Chuyển đổi từ string sang Guid
            if (!Guid.TryParse(reportRequest.UserId, out Guid userId))
            {
                throw new ArgumentException("Invalid UserId format.");
            }

            var report = new Report
            {
                UserId = userId,
                Reason = reportRequest.Reason,
                Status = false,
                CreatedBy = userID.ToString(),
                CreatedTime = DateTime.Now
            };

            await _unitOfWork.GetRepository<Report>().InsertAsync(report);
            await _unitOfWork.SaveAsync();
        }



        public async Task UpdateReport(string id, UpdateReportRequestModel updateReportRequest)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var User = httpContext.HttpContext?.User;
            Guid userID = new Guid(User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value);

            var existingReport = await _unitOfWork.GetRepository<Report>().GetByIdAsync(id);

            if (updateReportRequest == null)
            {
                throw new KeyNotFoundException("Invalid report data.");
            }
            if (existingReport == null)
            {
                throw new KeyNotFoundException("Report not found.");
            }
            else if (existingReport.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Report has been deleted.");
            }

            // Cập nhật thông tin của báo cáo
            existingReport.Reason = updateReportRequest.Reason;
            existingReport.Status = updateReportRequest.Status;
            existingReport.LastUpdatedBy = userID.ToString(); // Gán LastUpdatedBy
            existingReport.LastUpdatedTime = DateTime.Now;   // Gán LastUpdatedTime

            await _unitOfWork.SaveAsync();
        }

        public async Task<Report> DeleteReport(string id)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var user = httpContext.HttpContext?.User;

            // Lấy UserId từ claims
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            // Chuyển đổi UserId từ string sang Guid
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                throw new KeyNotFoundException("UserId is invalid.");
            }

            var report = await _unitOfWork.GetRepository<Report>().GetByIdAsync(id);

            if (report == null || report.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Report not found or has been deleted.");
            }

            // Gán DeletedBy và DeletedTime trước khi xóa
            report.DeletedBy = userId.ToString();
            report.DeletedTime = DateTime.Now;

            await _unitOfWork.GetRepository<Report>().UpdateAsync(report);
            await _unitOfWork.SaveAsync();

            return report;
        }

        public async Task<IEnumerable<ReportResponseModel>> GetReportsByReason(string reason)
        {
            var reports = await _unitOfWork.GetRepository<Report>().Entities
                .Where(r => r.Reason.Contains(reason) && !r.DeletedTime.HasValue)
                .ToListAsync();

            return reports.Select(r => new ReportResponseModel
            {
                Id = r.Id.ToString(),
                Reason = r.Reason,
                Status = r.Status
            });
        }
        //CheckReportStatus
        public async Task<bool> CheckReportStatus(string reportId)
        {
            var report = await _unitOfWork.GetRepository<Report>().GetByIdAsync(reportId);
            if (report == null || report.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Report not found or has been deleted.");
            }

            return report.Status;
        }
        //Thêm API check trạng thái report cho admin
        public async Task<ReportStatusResponseModel> CheckReportStatusForAdminAsync(string id)
        {
            var report = await _unitOfWork.GetRepository<Report>().GetByIdAsync(id);
            if (report == null || report.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Report not found or has been deleted.");
            }

            return new ReportStatusResponseModel
            {
                ReportId = report.Id.ToString(),
                Status = report.Status,
                CreatedBy = report.CreatedBy,
                CreatedTime = report.CreatedTime,
                LastUpdatedBy = report.LastUpdatedBy,
                LastUpdatedTime = report.LastUpdatedTime
            };
        }

    }
}
