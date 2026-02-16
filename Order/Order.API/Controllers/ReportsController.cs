using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.IServices;
using Order.Domain;
using Order.Domain.DTO;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportsService;
        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        [HttpGet("getOrderReport")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<OrderReports>>> getOrderReport(string? sellerId = null, string? fromDate = null, string? toDate = null)
        {
            var data = await _reportsService.GetOrderReport(sellerId, fromDate, toDate);
            return data;
        }

        [HttpGet("getTopSellingProducts")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<TopSellingProductsDto>>> GetTopSellingProducts(int top = 5, string? sellerId = null, string? days = null)
        {
            var data = await _reportsService.GetTopSellingProducts(top, sellerId, days);
            return data;
        }

        [HttpGet("getTopSellingSellers")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<TopSellingSellersDto>>> GetTopSellingSellers(int top = 5, string? days = null)
        {
            var data = await _reportsService.GetTopSellingSellers(top, days);
            return data;
        }

        [HttpGet("getTopSellingBrands")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<TopSellingBrandsDto>>> GetTopSellingBrands(int top = 5, string? sellerId = null, string? days = null)
        {
            var data = await _reportsService.GetTopSellingBrands(top, sellerId, days);
            return data;
        }

        [HttpGet("getTopUsedCoupons")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<TopUsedCouponsDto>>> GetTopUsedCoupons(int top = 5, string? sellerId = null, string? days = null)
        {
            var data = await _reportsService.GetTopUsedCoupons(top, sellerId, days);
            return data;
        }
    }
}
