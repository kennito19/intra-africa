using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;
using User.Domain.DTO;

namespace User.Application.IServices
{
    public interface IReportsService
    {
        Task<BaseResponse<List<WarehouseReport>>> GetWarehouseReport(string? SellerId = null, string? fromDate = null, string? toDate = null);
        Task<BaseResponse<List<GSTReport>>> GetGSTReport(string? SellerId = null, string? fromDate = null, string? toDate = null);
    }
}
