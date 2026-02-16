using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Application.IServices;
using User.Domain.Entity;
using User.Domain;
using User.Domain.DTO;

namespace User.Application.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;

        public ReportsService(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        public Task<BaseResponse<List<WarehouseReport>>> GetWarehouseReport(string? SellerId = null, string? fromDate = null, string? toDate = null)
        {
            var data = _reportsRepository.GetWarehouseReport(SellerId, fromDate, toDate);
            return data;
        }

        public Task<BaseResponse<List<GSTReport>>> GetGSTReport(string? SellerId = null, string? fromDate = null, string? toDate = null)
        {
            var data = _reportsRepository.GetGSTReport(SellerId, fromDate, toDate);
            return data;
        }
    }
}
