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

namespace Order.Infrastructure.Repository
{
    public class OrderInvoiceRepository : IOrderInvoiceRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public OrderInvoiceRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(OrderInvoice orderInvoice)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@id",orderInvoice.Id),
                new SqlParameter("@packageid",orderInvoice.PackageID),
                new SqlParameter("@orderid",orderInvoice.OrderID),
                new SqlParameter("@orderitemids",orderInvoice.OrderItemIDs),
                new SqlParameter("@invoiceno",orderInvoice.InvoiceNo),
                new SqlParameter("@sellertradename",orderInvoice.SellerTradeName),
                new SqlParameter("@sellerlegalname",orderInvoice.SellerLegalName),
                new SqlParameter("@sellergstno",orderInvoice.SellerGSTNo),
                new SqlParameter("@sellerregisteredaddressline1",orderInvoice.SellerRegisteredAddressLine1),
                new SqlParameter("@sellerregisteredaddressline2",orderInvoice.SellerRegisteredAddressLine2),
                new SqlParameter("@sellerregisteredlandmark",orderInvoice.SellerRegisteredLandmark),
                new SqlParameter("@sellerregisteredpincode",orderInvoice.SellerRegisteredPincode),
                new SqlParameter("@sellerregisteredcity",orderInvoice.SellerRegisteredCity),
                new SqlParameter("@sellerregisteredstate",orderInvoice.SellerRegisteredState),
                new SqlParameter("@sellerregisteredcountry",orderInvoice.SellerRegisteredCountry),
                new SqlParameter("@sellerpickupaddressline1",orderInvoice.SellerPickupAddressLine1),
                new SqlParameter("@sellerpickupaddressline2",orderInvoice.SellerPickupAddressLine2),
                new SqlParameter("@sellerpickuplandmark",orderInvoice.SellerPickupLandmark),
                new SqlParameter("@sellerpickuppincode",orderInvoice.SellerPickupPincode),
                new SqlParameter("@sellerpickupcity",orderInvoice.SellerPickupCity),
                new SqlParameter("@sellerpickupstate",orderInvoice.SellerPickupState),
                new SqlParameter("@sellerpickupcountry",orderInvoice.SellerPickupCountry),
                new SqlParameter("@sellerpickupcontactpersonname",orderInvoice.SellerPickupContactPersonName),
                new SqlParameter("@sellerpickupcontactpersonmobileno",orderInvoice.SellerPickupContactPersonMobileNo),
                new SqlParameter("@sellerPickupTaxNo",orderInvoice.SellerPickupTaxNo),
                new SqlParameter("@invoiceamount",orderInvoice.InvoiceAmount),
                new SqlParameter("@invoiceCodCharges",orderInvoice.InvoiceCodCharges),
                new SqlParameter("@status",orderInvoice.Status),
                new SqlParameter("@createdAt", orderInvoice.CreatedAt),
                new SqlParameter("@createdBy", orderInvoice.CreatedBy),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderInvoice, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(OrderInvoice orderInvoice)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id",orderInvoice.Id),

                new SqlParameter("@sellertradename",orderInvoice.SellerTradeName),
                new SqlParameter("@sellerlegalname",orderInvoice.SellerLegalName),
                new SqlParameter("@sellergstno",orderInvoice.SellerGSTNo),
                new SqlParameter("@sellerregisteredaddressline1",orderInvoice.SellerRegisteredAddressLine1),
                new SqlParameter("@sellerregisteredaddressline2",orderInvoice.SellerRegisteredAddressLine2),
                new SqlParameter("@sellerregisteredlandmark",orderInvoice.SellerRegisteredLandmark),
                new SqlParameter("@sellerregisteredpincode",orderInvoice.SellerRegisteredPincode),
                new SqlParameter("@sellerregisteredcity",orderInvoice.SellerRegisteredCity),
                new SqlParameter("@sellerregisteredstate",orderInvoice.SellerRegisteredState),
                new SqlParameter("@sellerregisteredcountry",orderInvoice.SellerRegisteredCountry),
                new SqlParameter("@sellerpickupaddressline1",orderInvoice.SellerPickupAddressLine1),
                new SqlParameter("@sellerpickupaddressline2",orderInvoice.SellerPickupAddressLine2),
                new SqlParameter("@sellerpickuplandmark",orderInvoice.SellerPickupLandmark),
                new SqlParameter("@sellerpickuppincode",orderInvoice.SellerPickupPincode),
                new SqlParameter("@sellerpickupcity",orderInvoice.SellerPickupCity),
                new SqlParameter("@sellerpickupstate",orderInvoice.SellerPickupState),
                new SqlParameter("@sellerpickupcountry",orderInvoice.SellerPickupCountry),
                new SqlParameter("@sellerpickupcontactpersonname",orderInvoice.SellerPickupContactPersonName),
                new SqlParameter("@sellerpickupcontactpersonmobileno",orderInvoice.SellerPickupContactPersonMobileNo),
                new SqlParameter("@sellerPickupTaxNo",orderInvoice.SellerPickupTaxNo),
                new SqlParameter("@invoiceamount",orderInvoice.InvoiceAmount),
                new SqlParameter("@invoiceCodCharges",orderInvoice.InvoiceCodCharges),
                new SqlParameter("@status",orderInvoice.Status),
                new SqlParameter("@modifiedby", orderInvoice.ModifiedBy),
                new SqlParameter("@modifiedat", orderInvoice.ModifiedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderInvoice, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(OrderInvoice orderInvoice)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", orderInvoice.Id),
                new SqlParameter("@deletedby", orderInvoice.DeletedBy),
                new SqlParameter("@deletedat", orderInvoice.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderInvoice, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<OrderInvoice>>> Get(OrderInvoice orderInvoice, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", orderInvoice.Id),
                new SqlParameter("@packageid", orderInvoice.PackageID),
                new SqlParameter("@sellerId", orderInvoice.SellerId),
                new SqlParameter("@orderid", orderInvoice.OrderID),
                new SqlParameter("@orderitemids", orderInvoice.OrderItemIDs),
                new SqlParameter("@invoiceno", orderInvoice.InvoiceNo),
                new SqlParameter("@searchtext", orderInvoice.SearchText),
                new SqlParameter("@status", orderInvoice.Status),
                new SqlParameter("@isDeleted", orderInvoice.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetOrderInvoice, orderInvoiceParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private async Task<List<OrderInvoice>> orderInvoiceParserAsync(DbDataReader reader)
        {
            List<OrderInvoice> orderInvoice = new List<OrderInvoice>();
            while (await reader.ReadAsync())
            {
                orderInvoice.Add(new OrderInvoice()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),

                    PackageID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PackageID"))),
                    OrderID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderID"))),
                    OrderItemIDs = Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderItemIDs"))),
                    InvoiceNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("InvoiceNo"))),
                    SellerTradeName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerTradeName"))),
                    SellerLegalName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerLegalName"))),
                    SellerGSTNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerGSTNo"))),
                    SellerRegisteredAddressLine1 = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerRegisteredAddressLine1"))),
                    SellerRegisteredAddressLine2 = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerRegisteredAddressLine2"))),
                    SellerRegisteredLandmark = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerRegisteredLandmark"))),
                    SellerRegisteredPincode = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerRegisteredPincode"))),
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
                    InvoiceCodCharges = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("InvoiceCodCharges")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("InvoiceCodCharges"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),



                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),

                    OrderNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo"))),
                    PackageNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackageNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PackageNo"))),
                    NoOfPackage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("NoOfPackage")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("NoOfPackage"))),
                    SellerId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId"))),


                });
            }
            return orderInvoice;
        }

    }
}
