using Microsoft.Extensions.Configuration;
using Order.Application.IRepositories;
using Order.Domain;
using Order.Domain.Entity;
using Order.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Order.Domain.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IO;

namespace Order.Infrastructure.Repository
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public OrderItemsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(OrderItems orderItems)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@orderid", orderItems.OrderID),
                new SqlParameter("@suborderno", orderItems.SubOrderNo),
                new SqlParameter("@sellerid", orderItems.SellerID),
                new SqlParameter("@brandid", orderItems.BrandID),
                new SqlParameter("@catId", orderItems.CategoryId),
                new SqlParameter("@productid", orderItems.ProductID),
                new SqlParameter("@productGuid", orderItems.ProductGUID),
                new SqlParameter("@sellerproductid", orderItems.SellerProductID),
                new SqlParameter("@productname", orderItems.ProductName),
                new SqlParameter("@productskucode", orderItems.ProductSKUCode),
                new SqlParameter("@mrp", orderItems.MRP),
                new SqlParameter("@sellingprice", orderItems.SellingPrice),
                new SqlParameter("@discount", orderItems.Discount),
                new SqlParameter("@qty", orderItems.Qty),
                new SqlParameter("@totalamount", orderItems.TotalAmount),
                new SqlParameter("@pricetypeid", orderItems.PriceTypeID),
                new SqlParameter("@pricetype", orderItems.PriceType),
                new SqlParameter("@sizeid", orderItems.SizeID),
                new SqlParameter("@sizevalue", orderItems.SizeValue),
                new SqlParameter("@iscouponapplied", orderItems.IsCouponApplied),
                new SqlParameter("@coupon", orderItems.Coupon),
                new SqlParameter("@coupontdiscount", orderItems.CoupontDiscount),
                new SqlParameter("@coupontdetails", orderItems.CoupontDetails),

                new SqlParameter("@shippingzone", orderItems.ShippingZone),
                new SqlParameter("@shippingcharge", orderItems.ShippingCharge),
                new SqlParameter("@shippingchargepaidby", orderItems.ShippingChargePaidBy),

                new SqlParameter("@subtotal", orderItems.SubTotal),
                new SqlParameter("@status", orderItems.Status),
                new SqlParameter("@wherehouseid", orderItems.WherehouseId),
                new SqlParameter("@isreplace", orderItems.IsReplace),
                new SqlParameter("@parentid", orderItems.ParentID),

                new SqlParameter("@returnpolicyname", orderItems.ReturnPolicyName),
                new SqlParameter("@returnpolicytitle", orderItems.ReturnPolicyTitle),
                new SqlParameter("@returnpolicycovers", orderItems.ReturnPolicyCovers),
                new SqlParameter("@returnpolicydescription", orderItems.ReturnPolicyDescription),
                new SqlParameter("@returnvaliddays", orderItems.ReturnValidDays),
                new SqlParameter("@returnvalidtilldate", orderItems.ReturnValidTillDate),

                new SqlParameter("@color", orderItems.ColorName),
                new SqlParameter("@image", orderItems.ProductImage),
                new SqlParameter("@extradetails", orderItems.ExtraDetails),

                new SqlParameter("@createdBy", orderItems.CreatedBy),
                new SqlParameter("@createdAt", orderItems.CreatedAt)

            };

                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                SqlParameter newid = new SqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderItems, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(OrderItems orderItems)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", orderItems.Id),
                new SqlParameter("@status", orderItems.Status),
                new SqlParameter("@wherehouseid", orderItems.WherehouseId),
                new SqlParameter("@returnvalidtilldate", orderItems.ReturnValidTillDate),
                new SqlParameter("@modifiedBy", orderItems.ModifiedBy),
                new SqlParameter("@modifiedAt", orderItems.ModifiedAt)

            };

                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                SqlParameter newid = new SqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderItems, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(OrderItems orderItems)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", orderItems.Id),
                new SqlParameter("@deletedBy", orderItems.DeletedBy),
                new SqlParameter("@deletedAt", orderItems.DeletedAt)
            };

                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                SqlParameter newid = new SqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderItems, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<OrderItems>>> Get(OrderItems orderItems, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", orderItems.Id),
                new SqlParameter("@orderid", orderItems.OrderID),
                new SqlParameter("@sellerid", orderItems.SellerID),
                new SqlParameter("@guid", orderItems.Guid),
                new SqlParameter("@productGuid", orderItems.ProductGUID),
                new SqlParameter("@suborderno", orderItems.SubOrderNo),
                new SqlParameter("@isDeleted", orderItems.IsDeleted),
                new SqlParameter("@sellerProductID", orderItems.SellerProductID),
                new SqlParameter("@productId", orderItems.ProductID),
                new SqlParameter("@catId", orderItems.CategoryId),
                new SqlParameter("@status", orderItems.Status),
                new SqlParameter("@notInstatus", orderItems.NotInStatus),
                new SqlParameter("@fromdate", orderItems.FromDate),
                new SqlParameter("@todate", orderItems.ToDate),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@PageSize", PageSize),

            };
                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                //SqlParameter newid = new SqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetOrderItems, orderItemsParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<OrderItems>> orderItemsParserAsync(DbDataReader reader)
        {
            List<OrderItems> lstorderItems = new List<OrderItems>();
            while (await reader.ReadAsync())
            {
                lstorderItems.Add(new OrderItems()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),

                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    OrderID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderID"))),
                    Guid = Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    SubOrderNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("SubOrderNo"))),
                    SellerID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    BrandID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandID"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    ProductID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    ProductGUID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGUID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGUID"))),
                    SellerProductID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductID"))),
                    ProductName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    ProductSKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSKUCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSKUCode"))),
                    MRP = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Qty = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Qty"))),
                    TotalAmount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalAmount"))),

                    PriceTypeID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PriceTypeID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PriceTypeID"))),
                    PriceType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PriceType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PriceType"))),
                    SizeID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeID"))),
                    SizeValue = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeValue")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeValue"))),

                    IsCouponApplied = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsCouponApplied")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsCouponApplied"))),
                    Coupon = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Coupon")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Coupon"))),
                    CoupontDiscount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CoupontDiscount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CoupontDiscount"))),
                    CoupontDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CoupontDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CoupontDetails"))),

                    ShippingZone = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingZone")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingZone"))),
                    ShippingCharge = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingCharge")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ShippingCharge"))),
                    ShippingChargePaidBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingChargePaidBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingChargePaidBy"))),

                    SubTotal = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SubTotal"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    WherehouseId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WherehouseId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WherehouseId"))),
                    IsReplace = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsReplace")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsReplace"))),
                    ParentID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentID"))),

                    ReturnPolicyName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyName"))),
                    ReturnPolicyTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyTitle"))),
                    ReturnPolicyCovers = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyCovers")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyCovers"))),
                    ReturnPolicyDescription = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyDescription")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyDescription"))),
                    ReturnValidDays = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnValidDays")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ReturnValidDays"))),
                    ReturnValidTillDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnValidTillDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ReturnValidTillDate"))),

                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsDeleted")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    OrderDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("OrderDate"))),
                    OrderBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderBy"))),
                    OrderNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo"))),
                    OrderStatus = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderStatus")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderStatus"))),
                    OrderCodCharges = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderCodCharges")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("OrderCodCharges"))),

                    ColorName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ColorName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ColorName"))),
                    ProductImage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImage")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImage"))),
                    ExtraDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                });
            }
            return lstorderItems;
        }

        public async Task<BaseResponse<List<OrderItemDetails>>> GetOrderDetails(OrderItemDetails orderItemDetails, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@orderid", orderItemDetails.OrderId),
                new SqlParameter("@orderItemId", orderItemDetails.OrderItemId),
                new SqlParameter("@sellerid", orderItemDetails.SellerID),
                new SqlParameter("@guid", orderItemDetails.Guid),
                new SqlParameter("@productGuid", orderItemDetails.ProductGUID),
                new SqlParameter("@orderNo", orderItemDetails.OrderNo),
                new SqlParameter("@suborderno", orderItemDetails.SubOrderNo),
                new SqlParameter("@productId", orderItemDetails.ProductID),
                new SqlParameter("@itemStatus", orderItemDetails.ItemStatus),
                new SqlParameter("@orderStatus", orderItemDetails.OrderStatus),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@PageSize", PageSize),

            };
                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                //SqlParameter newid = new SqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetOrderDetails, orderItemsDetailsParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<OrderItemDetails>> orderItemsDetailsParserAsync(DbDataReader reader)
        {
            List<OrderItemDetails> lstorderItemDetails = new List<OrderItemDetails>();
            while (await reader.ReadAsync())
            {
                lstorderItemDetails.Add(new OrderItemDetails()
                {
                    RowNumber = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("RowNumber")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PageCount")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("RecordCount")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),

                    OrderTrackDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderTrackDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderTrackDetails"))),
                    OrderWiseProductVariantMapping = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderWiseProductVariantMapping")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderWiseProductVariantMapping"))),
                    OrderWiseProductSeriesNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderWiseProductSeriesNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderWiseProductSeriesNo"))),
                    OrderWiseExtendedWarranty = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderWiseExtendedWarranty")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderWiseExtendedWarranty"))),
                    OrderWiseExtraCharges = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderWiseExtraCharges")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderWiseExtraCharges"))),
                    WeightSlab = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlab")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlab"))),
                    IsSellerWithGSTAtOrderTime = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsSellerWithGSTAtOrderTime")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsSellerWithGSTAtOrderTime"))),
                    HSNCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode"))),
                    OrderTaxRate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderTaxRate")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderTaxRate"))),
                    OrderTaxRateId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderTaxRateId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderTaxRateId"))),
                    NetEarn = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("NetEarn")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("NetEarn"))),
                    TaxOnCommission = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxOnCommission")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TaxOnCommission"))),
                    CommissionAmount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CommissionAmount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CommissionAmount"))),
                    CommissionRate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CommissionRate")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CommissionRate"))),
                    CommissionIn = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CommissionIn")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CommissionIn"))),
                    OtaxOnShipping = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OtaxOnShipping")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("OtaxOnShipping"))),
                    OtaxShipmentBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OtaxShipmentBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OtaxShipmentBy"))),
                    OtaxShipmentPaidBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OtaxShipmentPaidBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OtaxShipmentPaidBy"))),
                    OtaxShippingZone = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OtaxShippingZone")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OtaxShippingZone"))),
                    OtaxShippingCharge = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OtaxShippingCharge")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("OtaxShippingCharge"))),
                    OrderTaxInfoId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderTaxInfoId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderTaxInfoId"))),
                    PackageAmount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackageAmount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PackageAmount"))),
                    PackageCodCharges = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackageCodCharges")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PackageCodCharges"))),
                    TotalItems = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalItems")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalItems"))),
                    OrderItemIDs = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderItemIDs")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderItemIDs"))),
                    PackageNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackageNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PackageNo"))),
                    NoOfPackage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("NoOfPackage")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("NoOfPackage"))),
                    PackageId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackageId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PackageId"))),
                    ExtraDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                    ProductImage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImage")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImage"))),
                    ColorName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ColorName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ColorName"))),
                    ReturnValidTillDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnValidTillDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ReturnValidTillDate"))),
                    ReturnValidDays = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnValidDays")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ReturnValidDays"))),
                    ReturnPolicyDescription = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyDescription")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyDescription"))),
                    ReturnPolicyCovers = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyCovers")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyCovers"))),
                    ReturnPolicyTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyTitle"))),
                    ReturnPolicyName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyName"))),
                    ItemParentId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemParentId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ItemParentId"))),
                    ItemReplace = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemReplace")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("ItemReplace"))),
                    WherehouseId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WherehouseId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WherehouseId"))),
                    ItemStatus = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemStatus")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemStatus"))),
                    SubTotal = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTotal")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SubTotal"))),
                    ShippingChargePaidBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingChargePaidBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingChargePaidBy"))),
                    ShippingCharge = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingCharge")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ShippingCharge"))),
                    ShippingZone = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingZone")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingZone"))),
                    ItemCoupontDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemCoupontDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemCoupontDetails"))),
                    ItemCouponDiscount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemCouponDiscount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ItemCouponDiscount"))),
                    ItemCoupon = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemCoupon")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemCoupon"))),
                    ItemIsCouponApplied = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemIsCouponApplied")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("ItemIsCouponApplied"))),
                    SizeValue = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeValue")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeValue"))),
                    SizeID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeID"))),
                    PriceType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PriceType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PriceType"))),
                    PriceTypeID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PriceTypeID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PriceTypeID"))),
                    ItemTotalAmount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemTotalAmount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ItemTotalAmount"))),
                    Qty = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Qty")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Qty"))),
                    Discount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    SellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    MRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    ProductSKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSKUCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSKUCode"))),
                    ProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    SellerProductID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductID"))),
                    ProductGUID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGUID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGUID"))),
                    ProductID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    BrandID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandID"))),
                    SubOrderNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SubOrderNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SubOrderNo"))),
                    OrderItemId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderItemId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderItemId"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    DeliveryDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeliveryDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeliveryDate"))),
                    OrderDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("OrderDate"))),
                    OrderReferenceNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderReferenceNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderReferenceNo"))),
                    ParentID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentID"))),
                    IsReplace = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsReplace")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsReplace"))),
                    IsVertualRetailer = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsVertualRetailer")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsVertualRetailer"))),
                    IsRetailer = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsRetailer")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsRetailer"))),
                    OrderBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderBy"))),
                    PaymentInfo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PaymentInfo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PaymentInfo"))),
                    OrderStatus = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderStatus")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderStatus"))),
                    SaleType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SaleType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SaleType"))),
                    IsSale = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsSale")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsSale"))),
                    PaidAmount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PaidAmount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PaidAmount"))),
                    CODCharge = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CODCharge")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CODCharge"))),
                    CoupontDiscount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CoupontDiscount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CoupontDiscount"))),
                    Coupon = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Coupon")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Coupon"))),
                    IsCouponApplied = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsCouponApplied")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsCouponApplied"))),
                    TotalAmount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalAmount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalAmount"))),
                    TotalExtraCharges = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalExtraCharges")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalExtraCharges"))),
                    TotalShippingCharge = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalShippingCharge")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalShippingCharge"))),
                    PaymentMode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PaymentMode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PaymentMode"))),
                    UserGSTNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserGSTNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserGSTNo"))),
                    UserPincode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserPincode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserPincode"))),
                    UserCountry = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserCountry")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserCountry"))),
                    UserState = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserState")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserState"))),
                    UserCity = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserCity")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserCity"))),
                    UserLandmark = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserLandmark")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserLandmark"))),
                    UserAddressLine2 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserAddressLine2")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserAddressLine2"))),
                    UserAddressLine1 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserAddressLine1")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserAddressLine1"))),
                    UserEmail = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserEmail")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserEmail"))),
                    UserPhoneNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserPhoneNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserPhoneNo"))),
                    UserName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserName"))),
                    UserId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId"))),
                    SellerID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    OrderNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo"))),
                    Guid = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    OrderId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderId")))




                });
            }
            return lstorderItemDetails;
        }
    }
}
