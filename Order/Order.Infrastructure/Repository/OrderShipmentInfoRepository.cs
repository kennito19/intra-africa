using Microsoft.Extensions.Configuration;
using Order.Application.IRepositories;
using Order.Domain;
using Order.Domain.Entity;
using Order.Infrastructure.Helper;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace Order.Infrastructure.Repository
{
    public class OrderShipmentInfoRepository : IOrderShipmentInfoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public OrderShipmentInfoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(OrderShipmentInfo orderShipmentInfo)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),

                new SqlParameter("@sellerid", orderShipmentInfo.SellerID),
                new SqlParameter("@orderid", orderShipmentInfo.OrderID),
                new SqlParameter("@orderitemids", orderShipmentInfo.OrderItemIDs),
                new SqlParameter("@packageid", orderShipmentInfo.PackageID),
                new SqlParameter("@paymentmode", orderShipmentInfo.PaymentMode),
                new SqlParameter("@length", orderShipmentInfo.Length),
                new SqlParameter("@width", orderShipmentInfo.Width),
                new SqlParameter("@height", orderShipmentInfo.Height),
                new SqlParameter("@weight", orderShipmentInfo.Weight),
                new SqlParameter("@invoiceamount", orderShipmentInfo.InvoiceAmount),
                new SqlParameter("@invoiceCodCharges", orderShipmentInfo.InvoiceCodCharges),
                new SqlParameter("@packagedescription", orderShipmentInfo.PackageDescription),
                new SqlParameter("@isshipmentinitiate", orderShipmentInfo.IsShipmentInitiate),
                new SqlParameter("@ispaymentsuccess", orderShipmentInfo.IsPaymentSuccess),
                new SqlParameter("@courierid", orderShipmentInfo.CourierID),
                new SqlParameter("@serviceid", orderShipmentInfo.ServiceID),
                new SqlParameter("@servicetype", orderShipmentInfo.ServiceType),
                new SqlParameter("@pickupcontactpersonname", orderShipmentInfo.PickupContactPersonName),
                new SqlParameter("@pickupcontactpersonmobileno", orderShipmentInfo.PickupContactPersonMobileNo),
                new SqlParameter("@pickupcontactpersonemailid", orderShipmentInfo.PickupContactPersonEmailID),
                new SqlParameter("@pickupcompanyname", orderShipmentInfo.PickupCompanyName),
                new SqlParameter("@pickupaddressline1", orderShipmentInfo.PickupAddressLine1),
                new SqlParameter("@pickupaddressline2", orderShipmentInfo.PickupAddressLine2),
                new SqlParameter("@pickuplandmark", orderShipmentInfo.PickupLandmark),
                new SqlParameter("@pickuppincode", orderShipmentInfo.PickupPincode),
                new SqlParameter("@pickupcity", orderShipmentInfo.PickupCity),
                new SqlParameter("@pickupstate", orderShipmentInfo.PickupState),
                new SqlParameter("@pickupcountry", orderShipmentInfo.PickupCountry),
                new SqlParameter("@pickupTaxNo", orderShipmentInfo.PickupTaxNo),
                new SqlParameter("@dropcontactpersonname", orderShipmentInfo.DropContactPersonName),
                new SqlParameter("@dropcontactpersonmobileno", orderShipmentInfo.DropContactPersonMobileNo),
                new SqlParameter("@dropcontactpersonemailid", orderShipmentInfo.DropContactPersonEmailID),
                new SqlParameter("@dropcompanyname", orderShipmentInfo.DropCompanyName),
                new SqlParameter("@dropaddressline1", orderShipmentInfo.DropAddressLine1),
                new SqlParameter("@dropaddressline2", orderShipmentInfo.DropAddressLine2),
                new SqlParameter("@droplandmark", orderShipmentInfo.DropLandmark),
                new SqlParameter("@droppincode", orderShipmentInfo.DropPincode),
                new SqlParameter("@dropcity", orderShipmentInfo.DropCity),
                new SqlParameter("@dropstate", orderShipmentInfo.DropState),
                new SqlParameter("@dropcountry", orderShipmentInfo.DropCountry),
                new SqlParameter("@dropTaxNo", orderShipmentInfo.DropTaxNo),
                new SqlParameter("@shipmentid", orderShipmentInfo.ShipmentID),
                new SqlParameter("@shipmentorderid", orderShipmentInfo.ShipmentOrderID),
                new SqlParameter("@shippingpartner", orderShipmentInfo.ShippingPartner),
                new SqlParameter("@couriername", orderShipmentInfo.CourierName),
                new SqlParameter("@shippingamountfrompartner", orderShipmentInfo.ShippingAmountFromPartner),
                new SqlParameter("@awbno", orderShipmentInfo.AwbNo),
                new SqlParameter("@isshipmentsheduledbyadmin", orderShipmentInfo.IsShipmentSheduledByAdmin),
                new SqlParameter("@pickuplocationid", orderShipmentInfo.PickupLocationID),
                new SqlParameter("@errormessage", orderShipmentInfo.ErrorMessage),
                new SqlParameter("@forwardlable", orderShipmentInfo.ForwardLable),
                new SqlParameter("@returnlable", orderShipmentInfo.ReturnLable),
                new SqlParameter("@shipmentTrackingNo", orderShipmentInfo.ShipmentTrackingNo),
                new SqlParameter("@trackingNo", orderShipmentInfo.TrackingNo),
                new SqlParameter("@shipmentInfo", orderShipmentInfo.ShipmentInfo),


                new SqlParameter("@createdBy", orderShipmentInfo.CreatedBy),
                new SqlParameter("@createdAt", orderShipmentInfo.CreatedAt),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderShipmentInfo, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(OrderShipmentInfo orderShipmentInfo)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),

                 new SqlParameter("@paymentmode", orderShipmentInfo.PaymentMode),
                new SqlParameter("@length", orderShipmentInfo.Length),
                new SqlParameter("@width", orderShipmentInfo.Width),
                new SqlParameter("@height", orderShipmentInfo.Height),
                new SqlParameter("@weight", orderShipmentInfo.Weight),
                new SqlParameter("@invoiceamount", orderShipmentInfo.InvoiceAmount),
                new SqlParameter("@invoiceCodCharges", orderShipmentInfo.InvoiceCodCharges),
                new SqlParameter("@packagedescription", orderShipmentInfo.PackageDescription),
                new SqlParameter("@isshipmentinitiate", orderShipmentInfo.IsShipmentInitiate),
                new SqlParameter("@ispaymentsuccess", orderShipmentInfo.IsPaymentSuccess),
                new SqlParameter("@courierid", orderShipmentInfo.CourierID),
                new SqlParameter("@serviceid", orderShipmentInfo.ServiceID),
                new SqlParameter("@servicetype", orderShipmentInfo.ServiceType),
                new SqlParameter("@pickupcontactpersonname", orderShipmentInfo.PickupContactPersonName),
                new SqlParameter("@pickupcontactpersonmobileno", orderShipmentInfo.PickupContactPersonMobileNo),
                new SqlParameter("@pickupcontactpersonemailid", orderShipmentInfo.PickupContactPersonEmailID),
                new SqlParameter("@pickupcompanyname", orderShipmentInfo.PickupCompanyName),
                new SqlParameter("@pickupaddressline1", orderShipmentInfo.PickupAddressLine1),
                new SqlParameter("@pickupaddressline2", orderShipmentInfo.PickupAddressLine2),
                new SqlParameter("@pickuplandmark", orderShipmentInfo.PickupLandmark),
                new SqlParameter("@pickuppincode", orderShipmentInfo.PickupPincode),
                new SqlParameter("@pickupcity", orderShipmentInfo.PickupCity),
                new SqlParameter("@pickupstate", orderShipmentInfo.PickupState),
                new SqlParameter("@pickupcountry", orderShipmentInfo.PickupCountry),
                new SqlParameter("@pickupTaxNo", orderShipmentInfo.PickupTaxNo),
                new SqlParameter("@dropcontactpersonname", orderShipmentInfo.DropContactPersonName),
                new SqlParameter("@dropcontactpersonmobileno", orderShipmentInfo.DropContactPersonMobileNo),
                new SqlParameter("@dropcontactpersonemailid", orderShipmentInfo.DropContactPersonEmailID),
                new SqlParameter("@dropcompanyname", orderShipmentInfo.DropCompanyName),
                new SqlParameter("@dropaddressline1", orderShipmentInfo.DropAddressLine1),
                new SqlParameter("@dropaddressline2", orderShipmentInfo.DropAddressLine2),
                new SqlParameter("@droplandmark", orderShipmentInfo.DropLandmark),
                new SqlParameter("@droppincode", orderShipmentInfo.DropPincode),
                new SqlParameter("@dropcity", orderShipmentInfo.DropCity),
                new SqlParameter("@dropstate", orderShipmentInfo.DropState),
                new SqlParameter("@dropcountry", orderShipmentInfo.DropCountry),
                new SqlParameter("@dropTaxNo", orderShipmentInfo.DropTaxNo),
                new SqlParameter("@shipmentid", orderShipmentInfo.ShipmentID),
                new SqlParameter("@shipmentorderid", orderShipmentInfo.ShipmentOrderID),
                new SqlParameter("@shippingpartner", orderShipmentInfo.ShippingPartner),
                new SqlParameter("@couriername", orderShipmentInfo.CourierName),
                new SqlParameter("@shippingamountfrompartner", orderShipmentInfo.ShippingAmountFromPartner),
                new SqlParameter("@awbno", orderShipmentInfo.AwbNo),
                new SqlParameter("@isshipmentsheduledbyadmin", orderShipmentInfo.IsShipmentSheduledByAdmin),
                new SqlParameter("@pickuplocationid", orderShipmentInfo.PickupLocationID),
                new SqlParameter("@errormessage", orderShipmentInfo.ErrorMessage),
                new SqlParameter("@forwardlable", orderShipmentInfo.ForwardLable),
                new SqlParameter("@returnlable", orderShipmentInfo.ReturnLable),
                new SqlParameter("@shipmentTrackingNo", orderShipmentInfo.ShipmentTrackingNo),
                new SqlParameter("@trackingNo", orderShipmentInfo.TrackingNo),
                new SqlParameter("@shipmentInfo", orderShipmentInfo.ShipmentInfo),

                new SqlParameter("@modifiedBy", orderShipmentInfo.ModifiedBy),
                new SqlParameter("@modifiedAt", orderShipmentInfo.ModifiedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderShipmentInfo, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(OrderShipmentInfo orderShipmentInfo)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),

                new SqlParameter("@id", orderShipmentInfo.Id),

                new SqlParameter("@deletedby", orderShipmentInfo.DeletedBy),
                new SqlParameter("@deletedat", orderShipmentInfo.DeletedAt),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderShipmentInfo, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<OrderShipmentInfo>>> Get(OrderShipmentInfo orderShipmentInfo, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", orderShipmentInfo.Id),
                new SqlParameter("@sellerid", orderShipmentInfo.SellerID),
                new SqlParameter("@orderid", orderShipmentInfo.OrderID),
                new SqlParameter("@orderitemids", orderShipmentInfo.OrderItemIDs),
                new SqlParameter("@packageid", orderShipmentInfo.PackageID),
                new SqlParameter("@isDeleted", orderShipmentInfo.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetOrderShipmentInfo, OrderShipmentInfoParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<OrderShipmentInfo>> OrderShipmentInfoParserAsync(DbDataReader reader)
        {
            List<OrderShipmentInfo> lstorders = new List<OrderShipmentInfo>();
            while (await reader.ReadAsync())
            {
                lstorders.Add(new OrderShipmentInfo()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),

                    SellerID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    OrderID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderID"))),
                    OrderItemIDs = Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderItemIDs"))),
                    PackageID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PackageID"))),
                    PaymentMode = Convert.ToString(reader.GetValue(reader.GetOrdinal("PaymentMode"))),
                    Length = Convert.ToString(reader.GetValue(reader.GetOrdinal("Length"))),
                    Width = Convert.ToString(reader.GetValue(reader.GetOrdinal("Width"))),
                    Height = Convert.ToString(reader.GetValue(reader.GetOrdinal("Height"))),
                    Weight = Convert.ToString(reader.GetValue(reader.GetOrdinal("Weight"))),
                    InvoiceAmount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("InvoiceAmount"))),
                    InvoiceCodCharges = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("InvoiceCodCharges")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("InvoiceCodCharges"))),
                    PackageDescription = Convert.ToString(reader.GetValue(reader.GetOrdinal("PackageDescription"))),
                    IsShipmentInitiate = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsShipmentInitiate"))),
                    IsPaymentSuccess = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsPaymentSuccess"))),
                    CourierID = Convert.ToString(reader.GetValue(reader.GetOrdinal("CourierID"))),
                    ServiceID = Convert.ToString(reader.GetValue(reader.GetOrdinal("ServiceID"))),
                    ServiceType = Convert.ToString(reader.GetValue(reader.GetOrdinal("ServiceType"))),
                    PickupContactPersonName = Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupContactPersonName"))),
                    PickupContactPersonMobileNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupContactPersonMobileNo"))),
                    PickupContactPersonEmailID = Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupContactPersonEmailID"))),
                    PickupCompanyName = Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupCompanyName"))),
                    PickupAddressLine1 = Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupAddressLine1"))),
                    PickupAddressLine2 = Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupAddressLine2"))),
                    PickupLandmark = Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupLandmark"))),
                    PickupPincode = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PickupPincode"))),
                    PickupCity = Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupCity"))),
                    PickupState = Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupState"))),
                    PickupCountry = Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupCountry"))),
                    PickupTaxNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupTaxNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupTaxNo"))),
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
                    ShipmentID = Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentID"))),
                    ShipmentOrderID = Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentOrderID"))),
                    ShippingPartner = Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingPartner"))),
                    CourierName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CourierName"))),
                    ShippingAmountFromPartner = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingAmountFromPartner")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ShippingAmountFromPartner"))),
                    AwbNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("AwbNo"))),
                    IsShipmentSheduledByAdmin = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsShipmentSheduledByAdmin"))),
                    PickupLocationID = Convert.ToString(reader.GetValue(reader.GetOrdinal("PickupLocationID"))),
                    ErrorMessage = Convert.ToString(reader.GetValue(reader.GetOrdinal("ErrorMessage"))),
                    ForwardLable = Convert.ToString(reader.GetValue(reader.GetOrdinal("ForwardLable"))),
                    ReturnLable = Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnLable"))),
                    ShipmentTrackingNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentTrackingNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentTrackingNo"))),
                    TrackingNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TrackingNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TrackingNo"))),
                    ShipmentInfo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentInfo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentInfo"))),

                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),

                });
            }
            return lstorders;
        }
    }
}