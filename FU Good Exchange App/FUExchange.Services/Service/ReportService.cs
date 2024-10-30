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
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("ID không được để trống.");
            }
            var report = await _unitOfWork.GetRepository<Report>().GetByIdAsync(id);
            if (report == null)
            {
                throw new KeyNotFoundException("Lỗi: Không tìm thấy công việc. Vui lòng kiểm tra lại ID.");
            }
            else if (report.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Lỗi: Công việc đã bị xóa và không còn khả dụng.");
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
            {
                throw new ArgumentNullException(nameof(updateReportRequest), "Dữ liệu yêu cầu không hợp lệ.");
            }
            // Kiểm tra Reason không được để trống hoặc chỉ chứa khoảng trắng
            if (string.IsNullOrWhiteSpace(updateReportRequest.Reason))
            {
                throw new KeyNotFoundException("Lý do là bắt buộc và không được chứa khoảng trắng.");
            }

            IHttpContextAccessor httpContext = new HttpContextAccessor();
            var user = httpContext.HttpContext?.User;

            var userIdClaim = user?.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                throw new ArgumentException("UserId không hợp lệ hoặc không tồn tại.", nameof(userIdClaim));
            }

            // Tìm báo cáo với ID cung cấp, kiểm tra xem báo cáo đã bị xóa hay chưa
            var existingReport = await _unitOfWork.GetRepository<Report>().Entities
                .Where(r => r.Id == id && !r.DeletedTime.HasValue)
                .FirstOrDefaultAsync();

            // Nếu không tìm thấy báo cáo, hoặc báo cáo đã bị xóa, trả về lỗi
            if (existingReport == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy báo cáo với ID '{id}' hoặc báo cáo đã bị xóa.");
            }

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
                throw new KeyNotFoundException( "UserId không hợp lệ.");

            // Tìm báo cáo dựa trên Id và kiểm tra xem nó chưa bị xóa
            var report = await _unitOfWork.GetRepository<Report>().Entities
                .Where(r => r.Id == id && !r.DeletedTime.HasValue)
                .FirstOrDefaultAsync();

            // Nếu không tìm thấy báo cáo, trả về lỗi rõ ràng hơn
            if (report == null)
            {
                throw new KeyNotFoundException( "Lỗi: ID báo cáo không hợp lệ hoặc báo cáo đã bị xóa.");
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
            if (reports == null || !reports.Any())
            {
                throw new KeyNotFoundException("Không có báo cáo nào tồn tại với lý do đã cung cấp.");
            }
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
            if (report == null)
            {
                throw new KeyNotFoundException("Không tìm thấy báo cáo với ID đã cung cấp.");
            }

            if (report.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Báo cáo đã bị xóa.");
            }

            return report.Status;
        }
        //Thêm API check trạng thái report cho admin
        public async Task<ReportStatusResponseModel> CheckReportStatusForAdminAsync(string id)
        {
            // Lấy báo cáo dựa trên ID
            var report = await _unitOfWork.GetRepository<Report>().GetByIdAsync(id);

            // Kiểm tra xem báo cáo có tồn tại hay không
            if (report == null)
            {
                throw new KeyNotFoundException("Không tìm thấy báo cáo với ID đã cung cấp.");
            }

            // Kiểm tra xem báo cáo đã bị xóa chưa
            if (report.DeletedTime.HasValue)
            {
                throw new KeyNotFoundException("Báo cáo đã bị xóa.");
            }
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
