using Microsoft.Extensions.Configuration;
using Order.Application.IRepositories;
using Order.Domain;
using Order.Domain.DTO;
using Order.Domain.Entity;
using Order.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public OrderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(Orders orders)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@orderno", orders.OrderNo),
                new MySqlParameter("@userid", orders.UserId),
                new MySqlParameter("@sellerid", orders.SellerID),
                new MySqlParameter("@username", orders.UserName),
                new MySqlParameter("@userphoneno", orders.UserPhoneNo),
                new MySqlParameter("@useremail", orders.UserEmail),
                new MySqlParameter("@useraddressline1", orders.UserAddressLine1),
                new MySqlParameter("@useraddressline2", orders.UserAddressLine2),
                new MySqlParameter("@userlandmark", orders.UserLandmark),
                new MySqlParameter("@userpincode", orders.UserPincode),
                new MySqlParameter("@usercity", orders.UserCity),
                new MySqlParameter("@userstate", orders.UserState),
                new MySqlParameter("@usercountry", orders.UserCountry),
                new MySqlParameter("@usergstno", orders.UserGSTNo),
                new MySqlParameter("@paymentmode", orders.PaymentMode),
                new MySqlParameter("@totalshippingcharge", orders.TotalShippingCharge),
                new MySqlParameter("@totalextracharges", orders.TotalExtraCharges),
                new MySqlParameter("@totalamount", orders.TotalAmount),
                new MySqlParameter("@iscouponapplied", orders.IsCouponApplied),
                new MySqlParameter("@coupon", orders.Coupon),
                new MySqlParameter("@coupontdiscount", orders.CoupontDiscount),
                new MySqlParameter("@coupontdetails", orders.CoupontDetails),
                new MySqlParameter("@codCharge", orders.CODCharge),
                new MySqlParameter("@paidamount", orders.PaidAmount),
                new MySqlParameter("@issale", orders.IsSale),
                new MySqlParameter("@saletype", orders.SaleType),
                new MySqlParameter("@orderdate", orders.OrderDate),
                new MySqlParameter("@deliverydate", orders.DeliveryDate),
                new MySqlParameter("@status", orders.Status),
                new MySqlParameter("@paymentinfo", orders.PaymentInfo),
                new MySqlParameter("@orderby", orders.OrderBy),
                new MySqlParameter("@isretailer", orders.IsRetailer),
                new MySqlParameter("@isvertualretailer", orders.IsVertualRetailer),
                new MySqlParameter("@isreplace", orders.IsReplace),
                new MySqlParameter("@parentid", orders.ParentID),
                new MySqlParameter("@orderRefNo", orders.OrderReferenceNo),
                new MySqlParameter("@createdBy", orders.CreatedBy),
                new MySqlParameter("@createdAt", orders.CreatedAt),

            };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter newid = new MySqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Orders, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(Orders orders)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", orders.Id),
                new MySqlParameter("@paymentmode", orders.PaymentMode),
                new MySqlParameter("@totalshippingcharge", orders.TotalShippingCharge),
                new MySqlParameter("@totalextracharges", orders.TotalExtraCharges),
                new MySqlParameter("@totalamount", orders.TotalAmount),
                new MySqlParameter("@paidamount", orders.PaidAmount),
                new MySqlParameter("@orderdate", orders.OrderDate),
                new MySqlParameter("@deliverydate", orders.DeliveryDate),
                new MySqlParameter("@status", orders.Status),
                new MySqlParameter("@paymentinfo", orders.PaymentInfo),
                new MySqlParameter("@orderRefNo", orders.OrderReferenceNo),
              //new MySqlParameter("@isreplace", orders.IsReplace),
                new MySqlParameter("@modifiedBy", orders.ModifiedBy),
                new MySqlParameter("@modifiedAt", orders.ModifiedAt)

            };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter newid = new MySqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Orders, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(Orders orders)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", orders.Id),

                new MySqlParameter("@deletedby", orders.DeletedBy),
                new MySqlParameter("@deletedat", orders.DeletedAt),

            };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter newid = new MySqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Orders, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<Orders>>> Get(Orders orders, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", orders.Id),
                new MySqlParameter("@guid", orders.Guid),
                new MySqlParameter("@orderno", orders.OrderNo),
                new MySqlParameter("@orderRefNo", orders.OrderReferenceNo),
                new MySqlParameter("@userid", orders.UserId),
                new MySqlParameter("@sellerid", orders.SellerID),
                new MySqlParameter("@status", orders.Status),
                new MySqlParameter("@searchtext", orders.SearchText),
                new MySqlParameter("@fromdate", orders.FromDate),
                new MySqlParameter("@todate", orders.ToDate),
                new MySqlParameter("@coupon", orders.Coupon),
                new MySqlParameter("@notInstatus", orders.NotInStatus),
                new MySqlParameter("@isDeleted", orders.IsDeleted),
                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@PageSize", PageSize),

            };
                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                //MySqlParameter newid = new MySqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetOrders, orderParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private async Task<List<Orders>> orderParserAsync(DbDataReader reader)
        {
            List<Orders> lstorders = new List<Orders>();
            while (await reader.ReadAsync())
            {
                lstorders.Add(new Orders()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Guid = Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    OrderNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo"))),
                    SellerID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    UserId = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId"))),
                    UserName = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserName"))),
                    UserPhoneNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserPhoneNo"))),
                    UserEmail = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserEmail"))),
                    UserAddressLine1 = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserAddressLine1"))),
                    UserAddressLine2 = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserAddressLine2"))),
                    UserLandmark = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserLandmark"))),
                    UserPincode = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("UserPincode"))),
                    UserCity = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserCity"))),
                    UserState = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserState"))),
                    UserCountry = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserCountry"))),
                    UserGSTNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserGSTNo"))),
                    PaymentMode = Convert.ToString(reader.GetValue(reader.GetOrdinal("PaymentMode"))),
                    TotalShippingCharge = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalShippingCharge"))),
                    TotalExtraCharges = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalExtraCharges"))),
                    TotalAmount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalAmount"))),
                    IsCouponApplied = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsCouponApplied"))),
                    Coupon = Convert.ToString(reader.GetValue(reader.GetOrdinal("Coupon"))),
                    CoupontDiscount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CoupontDiscount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CoupontDiscount"))),
                    CoupontDetails = Convert.ToString(reader.GetValue(reader.GetOrdinal("CoupontDetails"))),
                    CODCharge = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CODCharge")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CODCharge"))),
                    PaidAmount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PaidAmount"))),
                    IsSale = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsSale"))),
                    SaleType = Convert.ToString(reader.GetValue(reader.GetOrdinal("SaleType"))),
                    OrderDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("OrderDate"))),
                    DeliveryDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeliveryDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeliveryDate"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    PaymentInfo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PaymentInfo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PaymentInfo"))),
                    OrderBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderBy"))),
                    IsRetailer = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsRetailer")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsRetailer"))),
                    IsVertualRetailer = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsVertualRetailer")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsVertualRetailer"))),
                    IsReplace = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsReplace")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsReplace"))),
                    ParentID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentID"))),
                    OrderReferenceNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderReferenceNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderReferenceNo"))),



                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted")))

                });
            }
            return lstorders;
        }

        public async Task<BaseResponse<List<InvoiceDto>>> GetInvoice(string? Packageid, string? OrderNo)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {

                new MySqlParameter("@packageid", Packageid),
                new MySqlParameter("@orderNo", OrderNo),


            };
                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                //MySqlParameter newid = new MySqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetInvoice, invoiceParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<InvoiceDto>> invoiceParserAsync(DbDataReader reader)
        {
            List<InvoiceDto> lstInvoice = new List<InvoiceDto>();
            while (await reader.ReadAsync())
            {
                lstInvoice.Add(new InvoiceDto()
                {
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    OrderID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderID"))),
                    OrderNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo"))),
                    OrderItemIDs = Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderItemIDs"))),
                    InvoiceNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("InvoiceNo"))),
                    SellerTradeName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerTradeName"))),
                    SellerLegalName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerLegalName"))),
                    SellerGSTNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerGSTNo"))),
                    SellerRegisteredAddressLine1 = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerRegisteredAddressLine1"))),
                    SellerRegisteredAddressLine2 = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerRegisteredAddressLine2"))),
                    SellerRegisteredLandmark = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerRegisteredLandmark"))),
                    SellerRegisteredPincode = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerRegisteredPincode"))),
                    SellerRegisteredCity = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerRegisteredCity"))),
                    SellerRegisteredState = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerRegisteredState"))),
                    SellerRegisteredCountry = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerRegisteredCountry"))),
                    SellerPickupAddressLine1 = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerPickupAddressLine1"))),
                    SellerPickupAddressLine2 = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerPickupAddressLine2"))),
                    SellerPickupLandmark = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerPickupLandmark"))),
                    SellerPickupPincode = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerPickupPincode"))),
                    SellerPickupCity = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerPickupCity"))),
                    SellerPickupState = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerPickupState"))),
                    SellerPickupCountry = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerPickupCountry"))),
                    SellerPickupContactPersonName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerPickupContactPersonName"))),
                    SellerPickupContactPersonMobileNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerPickupContactPersonMobileNo"))),
                    SellerPickupTaxNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerPickupTaxNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerPickupTaxNo"))),
                    InvoiceAmount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("InvoiceAmount"))),
                    InvoiceDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("InvoiceDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("InvoiceDate"))),
                    SellerID = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    BrandID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandID"))),
                    ProductID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    SellerProductID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductID"))),
                    ProductName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    ProductSKUCode = Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSKUCode"))),
                    MRP = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Qty = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Qty"))),
                    TotalAmount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalAmount"))),
                    PriceTypeID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PriceTypeID"))),
                    PriceType = Convert.ToString(reader.GetValue(reader.GetOrdinal("PriceType"))),
                    SizeID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeID"))),
                    SizeValue = Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeValue"))),
                    IsCouponApplied = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsCouponApplied"))),
                    Coupon = Convert.ToString(reader.GetValue(reader.GetOrdinal("Coupon"))),
                    CoupontDiscount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CoupontDiscount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CoupontDiscount"))),
                    CoupontDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CoupontDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CoupontDetails"))),
                    ShippingZone = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingZone")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingZone"))),
                    ShippingCharge = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingCharge")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingCharge"))),
                    ShippingChargePaidBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingChargePaidBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingChargePaidBy"))),
                    SubTotal = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SubTotal"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    OrderDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("OrderDate"))),
                    DropContactPersonName = Convert.ToString(reader.GetValue(reader.GetOrdinal("DropContactPersonName"))),
                    DropContactPersonMobileNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("DropContactPersonMobileNo"))),
                    DropContactPersonEmailID = Convert.ToString(reader.GetValue(reader.GetOrdinal("DropContactPersonEmailID"))),
                    DropCompanyName = Convert.ToString(reader.GetValue(reader.GetOrdinal("DropCompanyName"))),
                    DropAddressLine1 = Convert.ToString(reader.GetValue(reader.GetOrdinal("DropAddressLine1"))),
                    DropAddressLine2 = Convert.ToString(reader.GetValue(reader.GetOrdinal("DropAddressLine2"))),
                    DropLandmark = Convert.ToString(reader.GetValue(reader.GetOrdinal("DropLandmark"))),
                    DropPincode = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("DropPincode"))),
                    DropCity = Convert.ToString(reader.GetValue(reader.GetOrdinal("DropCity"))),
                    DropState = Convert.ToString(reader.GetValue(reader.GetOrdinal("DropState"))),
                    DropCountry = Convert.ToString(reader.GetValue(reader.GetOrdinal("DropCountry"))),
                    DropTaxNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DropTaxNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DropTaxNo"))),
                    TaxOnShipping = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TaxOnShipping"))),
                    OrderTaxRate = Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderTaxRate"))),
                    OrderTaxRateId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderTaxRateId"))),
                    HSNCode = Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode"))),
                    PaymentMode = Convert.ToString(reader.GetValue(reader.GetOrdinal("PaymentMode"))),
                    AwbNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("AwbNo"))),
                    ShippingPartner = Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingPartner"))),
                    CourierName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CourierName"))),
                    NoOfPackage = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("NoOfPackage"))),
                    ShippingDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ShippingDate"))),
                    Weight = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Weight"))),
                    WarrantyTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WarrantyTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("WarrantyTitle"))),
                    ActualWarrantyPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ActualWarrantyPrice")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ActualWarrantyPrice"))),
                    WarrantyQty = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WarrantyQty")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WarrantyQty"))),
                    WarrantyYear = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WarrantyYear")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WarrantyYear"))),
                    TotalActualWarrantyPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalActualWarrantyPrice")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalActualWarrantyPrice"))),
                    ExtrachargesName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtrachargesName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtrachargesName"))),
                    TotalExtracharges = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalExtracharges")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalExtracharges"))),
                    CODCharge = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CODCharge")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CODCharge"))),
                    ProductSeriesNos = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSeriesNos")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSeriesNos"))),
                });
            }
            return lstInvoice;
        }
    }
}
