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
            var report = await _unitOfWork.GetRepository<Report>().GetByIdAsync(id);
            if (report == null || report.DeletedTime.HasValue)
                throw new KeyNotFoundException("Report not found or has been deleted.");

            return new ReportResponseModel
            {
                Id = report.Id.ToString(),
                Reason = report.Reason,
                Status = report.Status
            };
        }

        public async Task<BasePaginatedList<ReportListResponseModel>> GetAllReports(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Report>().Entities
                .Where(r => !r.DeletedTime.HasValue);

            var reports = await _unitOfWork.GetRepository<Report>().GetPagging(query, pageIndex, pageSize);

            return new BasePaginatedList<ReportListResponseModel>(
                reports.Items.Select(r => new ReportListResponseModel
                {
                    Id = r.Id.ToString(),
                    Reason = r.Reason,
                    Status = r.Status,
                    CreatedTime = r.CreatedTime,
                    LastUpdatedTime = r.LastUpdatedTime
                }).ToList(),
                reports.TotalCount,
                pageIndex,
                pageSize
            );
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
                CreatedBy = userId.ToString(),
                CreatedTime = DateTime.Now
            };

            await _unitOfWork.GetRepository<Report>().InsertAsync(report);
            await _unitOfWork.SaveAsync();
        }


        public async Task UpdateReport(string id, UpdateReportRequestModel updateReportRequest)
        {
            if (updateReportRequest == null) throw new ArgumentNullException(nameof(updateReportRequest));
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var user = httpContext.HttpContext?.User;

            var userIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId)) throw new KeyNotFoundException("UserId is invalid.");

            var existingReport = await _unitOfWork.GetRepository<Report>().Entities
                .Where(r => r.Id == id && !r.DeletedTime.HasValue)
                .FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Report not found or has been deleted.");

            // Cập nhật thông tin của báo cáo
            existingReport.Reason = updateReportRequest.Reason;
            existingReport.Status = updateReportRequest.Status;
            existingReport.LastUpdatedBy = userId.ToString();
            existingReport.LastUpdatedTime = DateTime.Now;

            await _unitOfWork.SaveAsync();
        }

        public async Task<Report> DeleteReport(string id)
        {
            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var user = httpContext.HttpContext?.User;

            // Lấy UserId từ claims và chuyển đổi thành Guid
            var userIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId)) throw new KeyNotFoundException("UserId is invalid.");

            // Tìm báo cáo dựa trên Id và kiểm tra xem nó chưa bị xóa
            var report = await _unitOfWork.GetRepository<Report>().Entities
                .Where(r => r.Id == id && !r.DeletedTime.HasValue)
                .FirstOrDefaultAsync() ?? throw new KeyNotFoundException("Report not found or has been deleted.");

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
            var report = await _unitOfWork.GetRepository<Report>().GetByIdAsync(id)
              ?? throw new KeyNotFoundException("Report not found or has been deleted.");

            // Nếu muốn thêm kiểm tra DeletedTime trong lệnh throw:
            if (report.DeletedTime.HasValue)
                throw new KeyNotFoundException("Report has been deleted.");
            return new ReportStatusResponseModel
            {
                ReportId = report.Id.ToString(),
                Status = report.Status,
                CreatedBy = report.CreatedBy!,
                CreatedTime = report.CreatedTime,
                LastUpdatedBy = report.LastUpdatedBy!,
                LastUpdatedTime = report.LastUpdatedTime
            };
        }

    }
}
