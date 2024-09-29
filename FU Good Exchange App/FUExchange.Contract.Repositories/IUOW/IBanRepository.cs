using FUExchange.Contract.Repositories.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUExchange.Contract.Repositories.IUOW
{
    internal interface IBanRepository
    {
        Task<IEnumerable<Ban>> GetAllReportAsync(string reportId);
    }
}
