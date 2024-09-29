using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.ReportModelViews;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
                Id = report.Id.ToString(),
                Reason = report.Reason,
                Status = report.Status
            };
        }

        public async Task<BasePaginatedList<Report>> GetAllReports(int pageIndex, int pageSize)
        {
            var query = _unitOfWork.GetRepository<Report>().Entities.Where(r => !r.DeletedTime.HasValue);
            return await _unitOfWork.GetRepository<Report>().GetPagging(query, pageIndex, pageSize);
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
                Id = report.Id.ToString(),
                Reason = report.Reason,
                Status = report.Status
            };
        }

        //public async Task CreateReport(ReportRequestModel reportRequest)
        //{
        //    if (reportRequest == null)
        //    {
        //        throw new ArgumentNullException(nameof(reportRequest), "Invalid report data.");
        //    }

        //    var report = new Report
        //    {
        //        UserId = reportRequest.UserId,
        //        Reason = reportRequest.Reason,
        //        Status = false
        //    };

        //    await _unitOfWork.GetRepository<Report>().InsertAsync(report);
        //    await _unitOfWork.SaveAsync();
        //}
        public async Task CreateReport(ReportRequestModel reportRequest)
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

            if (reportRequest == null)
            {
                throw new KeyNotFoundException("Invalid report data.");
            }

            var report = new Report
            {
                UserId = userId, // Sử dụng userId đã lấy
                Reason = reportRequest.Reason,
                Status = false,
                CreatedBy = userId.ToString(), // Nếu bạn có trường CreatedBy
                CreatedTime = DateTime.Now // Nếu bạn có trường CreatedTime
            };

            await _unitOfWork.GetRepository<Report>().InsertAsync(report);
            await _unitOfWork.SaveAsync();
        }


        //public async Task UpdateReport(string id, ReportRequestModel reportRequest)
        //{
        //    if (reportRequest == null)
        //    {
        //        throw new ArgumentNullException(nameof(reportRequest), "Invalid report data.");
        //    }

        //    var existingReport = await _unitOfWork.GetRepository<Report>().GetByIdAsync(id);
        //    if (existingReport == null)
        //    {
        //        throw new KeyNotFoundException("Report not found.");
        //    }
        //    else if (existingReport.DeletedTime.HasValue)
        //    {
        //        throw new KeyNotFoundException("Report has been deleted.");
        //    }

        //    existingReport.Reason = reportRequest.Reason;
        //    existingReport.Status = reportRequest.Status;

        //    await _unitOfWork.SaveAsync();
        //}
        public async Task UpdateReport(string id, ReportRequestModel updateReportRequest)
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
            existingReport.LastUpdatedBy = userId.ToString(); // Gán LastUpdatedBy
            existingReport.LastUpdatedTime = DateTime.Now;   // Gán LastUpdatedTime

            await _unitOfWork.SaveAsync();
        }

        //public async Task<Report> DeleteReport(string id)
        //{
        //    var report = await _unitOfWork.GetRepository<Report>().GetByIdAsync(id);
        //    if (report == null || report.DeletedTime.HasValue)
        //    {
        //        throw new KeyNotFoundException("Report not found or has been deleted.");
        //    }

        //    report.DeletedTime = DateTime.Now; // hoặc gán cho DeletedBy nếu bạn cần
        //    await _unitOfWork.GetRepository<Report>().UpdateAsync(report);
        //    await _unitOfWork.SaveAsync();
        //    return report;
        //}
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
    }
}
