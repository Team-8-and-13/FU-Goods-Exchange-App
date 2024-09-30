using FUExchange.Contract.Repositories.Entity;
using FUExchange.Core;
using FUExchange.ModelViews.ReportModelViews;

namespace FUExchange.Contract.Services.Interface
{
    public interface IReportService
    {
        Task<BasePaginatedList<Report>> GetAllReports(int pageIndex, int pageSize);
        Task<ReportResponseModel?> GetReportById(string id);
        Task CreateReport(ReportRequestModel reportRequest);
        Task UpdateReport(string id, ReportRequestModel reportRequest);
        Task<Report> DeleteReport(string id);
        Task<IEnumerable<ReportResponseModel>> GetReportsByReason(string reason);
        Task<ReportResponseModel?> GetReportByIdAsync(string id);
        Task<bool> CheckReportStatus(string id);
        Task<ReportStatusResponseModel> CheckReportStatusForAdminAsync(string id);
    }
}
