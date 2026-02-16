using Order.Domain;
using Order.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IRepositories
{
    public interface IReportsRepository
    {
        Task<BaseResponse<List<OrderReports>>> GetOrderReport(string? SellerId = null, string? fromDate = null, string? toDate = null);
        Task<BaseResponse<List<TopSellingProductsDto>>> GetTopSellingProducts(int top, string? SellerId = null, string? days = null);
        Task<BaseResponse<List<TopSellingSellersDto>>> GetTopSellingSellers(int top, string? days = null);
        Task<BaseResponse<List<TopSellingBrandsDto>>> GetTopSellingBrands(int top, string? SellerId = null, string? days = null);
        Task<BaseResponse<List<TopUsedCouponsDto>>> GetTopUsedCoupons(int top, string? SellerId = null, string? days = null);
    }
}
