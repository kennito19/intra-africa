using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class ReportsService :IReportsService
    {
        private readonly IReportsRepository _reportsRepository;

        public ReportsService(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        public Task<BaseResponse<List<ProductReport>>> GetProductReport(string? SellerId = null, string? fromDate = null, string? toDate = null)
        {
            var data = _reportsRepository.GetProductReport(SellerId, fromDate, toDate);
            return data;
        }
        public Task<BaseResponse<List<ProductDetailsReport>>> GetProductDetailsReport(string? productIds = null)
        {
            var data = _reportsRepository.GetProductDetailsReport(productIds);
            return data;
        }
    }
}
