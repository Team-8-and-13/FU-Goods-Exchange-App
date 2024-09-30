using FUExchange.Contract.Repositories.Entity;
using FUExchange.Contract.Repositories.Interface;
namespace FUExchange.Contract.Repositories.IUOW
{
    public interface IReportRepository : IGenericRepository<Report>
    {
        Task<IEnumerable<Report>> GetAllApproveAsync();
        Task Reason(string id);
        Task Status(string id);
       
    } 
}
