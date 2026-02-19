using Microsoft.Extensions.Configuration;
using Order.Application.IRepositories;
using Order.Domain;
using Order.Domain.Entity;
using Order.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using MySqlConnector;
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@id",orderInvoice.Id),
                new MySqlParameter("@packageid",orderInvoice.PackageID),
                new MySqlParameter("@orderid",orderInvoice.OrderID),
                new MySqlParameter("@orderitemids",orderInvoice.OrderItemIDs),
                new MySqlParameter("@invoiceno",orderInvoice.InvoiceNo),
                new MySqlParameter("@sellertradename",orderInvoice.SellerTradeName),
                new MySqlParameter("@sellerlegalname",orderInvoice.SellerLegalName),
                new MySqlParameter("@sellergstno",orderInvoice.SellerGSTNo),
                new MySqlParameter("@sellerregisteredaddressline1",orderInvoice.SellerRegisteredAddressLine1),
                new MySqlParameter("@sellerregisteredaddressline2",orderInvoice.SellerRegisteredAddressLine2),
                new MySqlParameter("@sellerregisteredlandmark",orderInvoice.SellerRegisteredLandmark),
                new MySqlParameter("@sellerregisteredpincode",orderInvoice.SellerRegisteredPincode),
                new MySqlParameter("@sellerregisteredcity",orderInvoice.SellerRegisteredCity),
                new MySqlParameter("@sellerregisteredstate",orderInvoice.SellerRegisteredState),
                new MySqlParameter("@sellerregisteredcountry",orderInvoice.SellerRegisteredCountry),
                new MySqlParameter("@sellerpickupaddressline1",orderInvoice.SellerPickupAddressLine1),
                new MySqlParameter("@sellerpickupaddressline2",orderInvoice.SellerPickupAddressLine2),
                new MySqlParameter("@sellerpickuplandmark",orderInvoice.SellerPickupLandmark),
                new MySqlParameter("@sellerpickuppincode",orderInvoice.SellerPickupPincode),
                new MySqlParameter("@sellerpickupcity",orderInvoice.SellerPickupCity),
                new MySqlParameter("@sellerpickupstate",orderInvoice.SellerPickupState),
                new MySqlParameter("@sellerpickupcountry",orderInvoice.SellerPickupCountry),
                new MySqlParameter("@sellerpickupcontactpersonname",orderInvoice.SellerPickupContactPersonName),
                new MySqlParameter("@sellerpickupcontactpersonmobileno",orderInvoice.SellerPickupContactPersonMobileNo),
                new MySqlParameter("@sellerPickupTaxNo",orderInvoice.SellerPickupTaxNo),
                new MySqlParameter("@invoiceamount",orderInvoice.InvoiceAmount),
                new MySqlParameter("@invoiceCodCharges",orderInvoice.InvoiceCodCharges),
                new MySqlParameter("@status",orderInvoice.Status),
                new MySqlParameter("@createdAt", orderInvoice.CreatedAt),
                new MySqlParameter("@createdBy", orderInvoice.CreatedBy),

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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id",orderInvoice.Id),

                new MySqlParameter("@sellertradename",orderInvoice.SellerTradeName),
                new MySqlParameter("@sellerlegalname",orderInvoice.SellerLegalName),
                new MySqlParameter("@sellergstno",orderInvoice.SellerGSTNo),
                new MySqlParameter("@sellerregisteredaddressline1",orderInvoice.SellerRegisteredAddressLine1),
                new MySqlParameter("@sellerregisteredaddressline2",orderInvoice.SellerRegisteredAddressLine2),
                new MySqlParameter("@sellerregisteredlandmark",orderInvoice.SellerRegisteredLandmark),
                new MySqlParameter("@sellerregisteredpincode",orderInvoice.SellerRegisteredPincode),
                new MySqlParameter("@sellerregisteredcity",orderInvoice.SellerRegisteredCity),
                new MySqlParameter("@sellerregisteredstate",orderInvoice.SellerRegisteredState),
                new MySqlParameter("@sellerregisteredcountry",orderInvoice.SellerRegisteredCountry),
                new MySqlParameter("@sellerpickupaddressline1",orderInvoice.SellerPickupAddressLine1),
                new MySqlParameter("@sellerpickupaddressline2",orderInvoice.SellerPickupAddressLine2),
                new MySqlParameter("@sellerpickuplandmark",orderInvoice.SellerPickupLandmark),
                new MySqlParameter("@sellerpickuppincode",orderInvoice.SellerPickupPincode),
                new MySqlParameter("@sellerpickupcity",orderInvoice.SellerPickupCity),
                new MySqlParameter("@sellerpickupstate",orderInvoice.SellerPickupState),
                new MySqlParameter("@sellerpickupcountry",orderInvoice.SellerPickupCountry),
                new MySqlParameter("@sellerpickupcontactpersonname",orderInvoice.SellerPickupContactPersonName),
                new MySqlParameter("@sellerpickupcontactpersonmobileno",orderInvoice.SellerPickupContactPersonMobileNo),
                new MySqlParameter("@sellerPickupTaxNo",orderInvoice.SellerPickupTaxNo),
                new MySqlParameter("@invoiceamount",orderInvoice.InvoiceAmount),
                new MySqlParameter("@invoiceCodCharges",orderInvoice.InvoiceCodCharges),
                new MySqlParameter("@status",orderInvoice.Status),
                new MySqlParameter("@modifiedby", orderInvoice.ModifiedBy),
                new MySqlParameter("@modifiedat", orderInvoice.ModifiedAt)

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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", orderInvoice.Id),
                new MySqlParameter("@deletedby", orderInvoice.DeletedBy),
                new MySqlParameter("@deletedat", orderInvoice.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", orderInvoice.Id),
                new MySqlParameter("@packageid", orderInvoice.PackageID),
                new MySqlParameter("@sellerId", orderInvoice.SellerId),
                new MySqlParameter("@orderid", orderInvoice.OrderID),
                new MySqlParameter("@orderitemids", orderInvoice.OrderItemIDs),
                new MySqlParameter("@invoiceno", orderInvoice.InvoiceNo),
                new MySqlParameter("@searchtext", orderInvoice.SearchText),
                new MySqlParameter("@status", orderInvoice.Status),
                new MySqlParameter("@isDeleted", orderInvoice.IsDeleted),
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
