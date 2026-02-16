using Order.Application.IRepositories;
using Order.Application.IServices;
using Order.Domain;
using Order.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public class ReportsService : IReportsService
    {
        private readonly IReportsRepository _reportsRepository;

        public ReportsService(IReportsRepository reportsRepository)
        {
            _reportsRepository = reportsRepository;
        }

        public Task<BaseResponse<List<OrderReports>>> GetOrderReport(string? SellerId = null, string? fromDate = null, string? toDate = null)
        {
            var data = _reportsRepository.GetOrderReport(SellerId, fromDate, toDate);
            return data;
        }

        public Task<BaseResponse<List<TopSellingProductsDto>>> GetTopSellingProducts(int top, string? SellerId = null, string? days = null)
        {
            var data = _reportsRepository.GetTopSellingProducts(top, SellerId, days);
            return data;
        }

        public Task<BaseResponse<List<TopSellingSellersDto>>> GetTopSellingSellers(int top, string? days = null)
        {
            var data = _reportsRepository.GetTopSellingSellers(top, days);
            return data;
        }
        
        public Task<BaseResponse<List<TopSellingBrandsDto>>> GetTopSellingBrands(int top, string? SellerId = null, string? days = null)
        {
            var data = _reportsRepository.GetTopSellingBrands(top, SellerId, days);
            return data;
        }

        public Task<BaseResponse<List<TopUsedCouponsDto>>> GetTopUsedCoupons(int top, string? SellerId = null, string? days = null)
        {
            var data = _reportsRepository.GetTopUsedCoupons(top, SellerId, days);
            return data;
        }
    }
}
