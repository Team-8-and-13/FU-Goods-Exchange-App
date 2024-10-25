using System.Security.Claims;
using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
using FUExchange.Contract.Services.Interface;
using FUExchange.Core;
using FUExchange.Core.Constants;
using FUExchange.ModelViews.ReportModelsView;
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
            if (report == null || report.DeletedTime.HasValue)
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không tìm thấy công việc hoặc đã bị xóa.");

            return new ReportResponseModel
            {
                Id = report.Id.ToString(),
                Reason = report.Reason,
                Status = report.Status
            };
        }
        public async Task<BasePaginatedList<ReportListResponseModel>> GetAllReports(int pageIndex, int pageSize)
        {
            if (pageIndex <= 0)
            {
                throw new ArgumentException("pageIndex phải lớn hơn 0.");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentException("pageSize phải lớn hơn 0.");
            }

            if (pageIndex >= pageSize)
            {
                throw new ArgumentException("pageIndex phải nhỏ hơn pageSize.");
            }

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
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không tìm thấy công việc.");
            }
            else if (report.DeletedTime.HasValue)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Công việc đã bị xóa.");
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
            if (reportRequest == null || string.IsNullOrEmpty(reportRequest.UserId))
            {
                throw new ArgumentNullException(nameof(reportRequest.UserId), "UserId là bắt buộc.");
            }

            if (string.IsNullOrEmpty(reportRequest.Reason))
            {
                throw new ArgumentNullException(nameof(reportRequest.Reason), "Lý do là bắt buộc.");
            }
            // Chuyển đổi từ string sang Guid
            if (!Guid.TryParse(reportRequest.UserId, out Guid userId))
            {
                throw new ArgumentException("Định dạng UserId không hợp lệ.", nameof(reportRequest.UserId));
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
            if (updateReportRequest == null)
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Dữ liệu yêu cầu không hợp lệ.");

            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var user = httpContext.HttpContext?.User;

            var userIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId))
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "UserId không hợp lệ.");

            var existingReport = await _unitOfWork.GetRepository<Report>().Entities
                .Where(r => r.Id == id && !r.DeletedTime.HasValue)
                .FirstOrDefaultAsync()
                ?? throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không tìm thấy báo cáo hoặc báo cáo đã bị xóa.");

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
            if (!Guid.TryParse(userIdClaim, out Guid userId))
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "UserId không hợp lệ.");

            // Tìm báo cáo dựa trên Id và kiểm tra xem nó chưa bị xóa
            var report = await _unitOfWork.GetRepository<Report>().Entities
                 .Where(r => r.Id == id && !r.DeletedTime.HasValue)
                 .FirstOrDefaultAsync() ?? throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không tìm thấy công việc hoặc đã bị xóa.");
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
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không tìm thấy công việc hoặc đã bị xóa.");
            }

            return report.Status;
        }
        //Thêm API check trạng thái report cho admin
        public async Task<ReportStatusResponseModel> CheckReportStatusForAdminAsync(string id)
        {
            var report = await _unitOfWork.GetRepository<Report>().GetByIdAsync(id)
                ?? throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Không tìm thấy báo cáo hoặc đã bị xóa.");

            if (report.DeletedTime.HasValue)
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Báo cáo đã bị xóa.");
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
