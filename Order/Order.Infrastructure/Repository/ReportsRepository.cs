using Microsoft.Extensions.Configuration;
using Order.Domain;
using Order.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order.Domain.DTO;
using Order.Application.IRepositories;

namespace Order.Infrastructure.Repository
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public ReportsRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<List<OrderReports>>> GetOrderReport(string? SellerId = null, string? fromDate = null, string? toDate = null)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "OrderReport"),
                    new MySqlParameter("@sellerId", SellerId),
                    new MySqlParameter("@fromdate", fromDate),
                    new MySqlParameter("@todate",toDate),

                };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Reports, GetOrderReportParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<TopSellingProductsDto>>> GetTopSellingProducts(int top, string? SellerId = null, string? days = null)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "TopSellingProducts"),
                    new MySqlParameter("@top", top),
                    new MySqlParameter("@sellerId", SellerId),
                    new MySqlParameter("@days", days),

                };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Reports, GetTopSellingProductsParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<TopSellingSellersDto>>> GetTopSellingSellers(int top, string? days = null)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "TopSellingSellers"),
                    new MySqlParameter("@top", top),
                    new MySqlParameter("@days", days),

                };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Reports, GetTopSellingSellersParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<TopSellingBrandsDto>>> GetTopSellingBrands(int top, string? SellerId = null, string? days = null)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "TopSellingBrands"),
                    new MySqlParameter("@top", top),
                    new MySqlParameter("@sellerId", SellerId),
                    new MySqlParameter("@days", days),

                };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Reports, GetTopSellingBrandsParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<TopUsedCouponsDto>>> GetTopUsedCoupons(int top, string? SellerId = null, string? days = null)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "TopUsedCoupons"),
                    new MySqlParameter("@top", top),
                    new MySqlParameter("@sellerId", SellerId),
                    new MySqlParameter("@days", days),

                };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Reports, GetTopUsedCouponsParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<OrderReports>> GetOrderReportParserAsync(DbDataReader reader)
        {
            List<OrderReports> lstKYCDetails = new List<OrderReports>();
            while (await reader.ReadAsync())
            {
                lstKYCDetails.Add(new OrderReports()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    OrderItemId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("OrderItemId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderItemId"))),
                    OrderId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("OrderId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderId"))),
                    OrderNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("OrderNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo"))),
                    OrderBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("OrderBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderBy"))),
                    OrderDate = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("OrderDate")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("OrderDate"))),
                    SellerId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SellerId")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId"))),
                    BrandId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("BrandId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandId"))),
                    ProductId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProductId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductId"))),
                    ProductGUID = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProductGUID")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGUID"))),
                    ProductName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProductName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    SellerProductId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SellerProductId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    ProductSKUCode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProductSKUCode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSKUCode"))),
                    MRP = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("MRP")).ToString()) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SellingPrice")).ToString()) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Discount")).ToString()) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Qty = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Qty")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Qty"))),
                    IsCouponApplied = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsCouponApplied")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsCouponApplied"))),
                    Coupon = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Coupon")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Coupon"))),
                    CoupontDiscount = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CoupontDiscount")).ToString()) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CoupontDiscount"))),
                    ShippingCharge = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ShippingCharge")).ToString()) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ShippingCharge"))),
                    ShippingChargePaidBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ShippingChargePaidBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingChargePaidBy"))),
                    ShippingZone = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ShippingZone")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingZone"))),
                    UserName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserName"))),
                    UserPhoneNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserPhoneNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserPhoneNo"))),
                    UserEmail = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserEmail")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserEmail"))),
                    UserAddressLine1 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserAddressLine1")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserAddressLine1"))),
                    UserAddressLine2 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserAddressLine2")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserAddressLine2"))),
                    UserLandmark = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserLandmark")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserLandmark"))),
                    UserCity = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserCity")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserCity"))),
                    UserState = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserState")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserState"))),
                    UserPincode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserPincode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserPincode"))),
                    orderStatus = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("orderStatus")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("orderStatus"))),
                    Status = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Status")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    ItemTotalAmount = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ItemTotalAmount")).ToString()) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ItemTotalAmount"))),
                    ItemSubTotal = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ItemSubTotal")).ToString()) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ItemSubTotal"))),
                    PaidAmount = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("PaidAmount")).ToString()) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PaidAmount"))),
                    TotalAmount = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TotalAmount")).ToString()) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalAmount"))),
                    PaymentMode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("PaymentMode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PaymentMode"))),
                    CODCharge = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CODCharge")).ToString()) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CODCharge"))),
                    CreatedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CreatedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),

                });
            }
            return lstKYCDetails;
        }

        private async Task<List<TopSellingProductsDto>> GetTopSellingProductsParserAsync(DbDataReader reader)
        {
            List<TopSellingProductsDto> lstKYCDetails = new List<TopSellingProductsDto>();
            while (await reader.ReadAsync())
            {
                lstKYCDetails.Add(new TopSellingProductsDto()
                {
                    ProductID = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProductID")).ToString()) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    ProductGUID = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProductGUID")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGUID"))),
                    TotalOrders = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TotalOrders")).ToString()) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalOrders"))),
                    TotalSell = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TotalSell")).ToString()) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalSell"))),
                });
            }
            return lstKYCDetails;
        }

        private async Task<List<TopSellingSellersDto>> GetTopSellingSellersParserAsync(DbDataReader reader)
        {
            List<TopSellingSellersDto> lstKYCDetails = new List<TopSellingSellersDto>();
            while (await reader.ReadAsync())
            {
                lstKYCDetails.Add(new TopSellingSellersDto()
                {
                    SellerID = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SellerID")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    TotalOrders = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TotalOrders")).ToString()) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalOrders"))),
                    TotalSell = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TotalSell")).ToString()) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalSell"))),
                });
            }
            return lstKYCDetails;
        }

        private async Task<List<TopSellingBrandsDto>> GetTopSellingBrandsParserAsync(DbDataReader reader)
        {
            List<TopSellingBrandsDto> lstKYCDetails = new List<TopSellingBrandsDto>();
            while (await reader.ReadAsync())
            {
                lstKYCDetails.Add(new TopSellingBrandsDto()
                {
                    BrandID = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("BrandID")).ToString()) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandID"))),
                    TotalOrders = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TotalOrders")).ToString()) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalOrders"))),
                    TotalSell = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TotalSell")).ToString()) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalSell"))),
                });
            }
            return lstKYCDetails;
        }
        private async Task<List<TopUsedCouponsDto>> GetTopUsedCouponsParserAsync(DbDataReader reader)
        {
            List<TopUsedCouponsDto> lstKYCDetails = new List<TopUsedCouponsDto>();
            while (await reader.ReadAsync())
            {
                lstKYCDetails.Add(new TopUsedCouponsDto()
                {
                    Coupon = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Coupon")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Coupon"))),
                    CoupontDetails = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CoupontDetails")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CoupontDetails"))),
                    TotalOrders = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TotalOrders")).ToString()) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalOrders"))),
                    TotalSell = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TotalSell")).ToString()) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalSell"))),
                });
            }
            return lstKYCDetails;
        }
    }
}
