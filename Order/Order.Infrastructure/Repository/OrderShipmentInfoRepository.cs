using Microsoft.Extensions.Configuration;
using Order.Application.IRepositories;
using Order.Domain;
using Order.Domain.Entity;
using Order.Infrastructure.Helper;
using System.Data;
using System.Data.Common;
using MySqlConnector;

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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),

                new MySqlParameter("@sellerid", orderShipmentInfo.SellerID),
                new MySqlParameter("@orderid", orderShipmentInfo.OrderID),
                new MySqlParameter("@orderitemids", orderShipmentInfo.OrderItemIDs),
                new MySqlParameter("@packageid", orderShipmentInfo.PackageID),
                new MySqlParameter("@paymentmode", orderShipmentInfo.PaymentMode),
                new MySqlParameter("@length", orderShipmentInfo.Length),
                new MySqlParameter("@width", orderShipmentInfo.Width),
                new MySqlParameter("@height", orderShipmentInfo.Height),
                new MySqlParameter("@weight", orderShipmentInfo.Weight),
                new MySqlParameter("@invoiceamount", orderShipmentInfo.InvoiceAmount),
                new MySqlParameter("@invoiceCodCharges", orderShipmentInfo.InvoiceCodCharges),
                new MySqlParameter("@packagedescription", orderShipmentInfo.PackageDescription),
                new MySqlParameter("@isshipmentinitiate", orderShipmentInfo.IsShipmentInitiate),
                new MySqlParameter("@ispaymentsuccess", orderShipmentInfo.IsPaymentSuccess),
                new MySqlParameter("@courierid", orderShipmentInfo.CourierID),
                new MySqlParameter("@serviceid", orderShipmentInfo.ServiceID),
                new MySqlParameter("@servicetype", orderShipmentInfo.ServiceType),
                new MySqlParameter("@pickupcontactpersonname", orderShipmentInfo.PickupContactPersonName),
                new MySqlParameter("@pickupcontactpersonmobileno", orderShipmentInfo.PickupContactPersonMobileNo),
                new MySqlParameter("@pickupcontactpersonemailid", orderShipmentInfo.PickupContactPersonEmailID),
                new MySqlParameter("@pickupcompanyname", orderShipmentInfo.PickupCompanyName),
                new MySqlParameter("@pickupaddressline1", orderShipmentInfo.PickupAddressLine1),
                new MySqlParameter("@pickupaddressline2", orderShipmentInfo.PickupAddressLine2),
                new MySqlParameter("@pickuplandmark", orderShipmentInfo.PickupLandmark),
                new MySqlParameter("@pickuppincode", orderShipmentInfo.PickupPincode),
                new MySqlParameter("@pickupcity", orderShipmentInfo.PickupCity),
                new MySqlParameter("@pickupstate", orderShipmentInfo.PickupState),
                new MySqlParameter("@pickupcountry", orderShipmentInfo.PickupCountry),
                new MySqlParameter("@pickupTaxNo", orderShipmentInfo.PickupTaxNo),
                new MySqlParameter("@dropcontactpersonname", orderShipmentInfo.DropContactPersonName),
                new MySqlParameter("@dropcontactpersonmobileno", orderShipmentInfo.DropContactPersonMobileNo),
                new MySqlParameter("@dropcontactpersonemailid", orderShipmentInfo.DropContactPersonEmailID),
                new MySqlParameter("@dropcompanyname", orderShipmentInfo.DropCompanyName),
                new MySqlParameter("@dropaddressline1", orderShipmentInfo.DropAddressLine1),
                new MySqlParameter("@dropaddressline2", orderShipmentInfo.DropAddressLine2),
                new MySqlParameter("@droplandmark", orderShipmentInfo.DropLandmark),
                new MySqlParameter("@droppincode", orderShipmentInfo.DropPincode),
                new MySqlParameter("@dropcity", orderShipmentInfo.DropCity),
                new MySqlParameter("@dropstate", orderShipmentInfo.DropState),
                new MySqlParameter("@dropcountry", orderShipmentInfo.DropCountry),
                new MySqlParameter("@dropTaxNo", orderShipmentInfo.DropTaxNo),
                new MySqlParameter("@shipmentid", orderShipmentInfo.ShipmentID),
                new MySqlParameter("@shipmentorderid", orderShipmentInfo.ShipmentOrderID),
                new MySqlParameter("@shippingpartner", orderShipmentInfo.ShippingPartner),
                new MySqlParameter("@couriername", orderShipmentInfo.CourierName),
                new MySqlParameter("@shippingamountfrompartner", orderShipmentInfo.ShippingAmountFromPartner),
                new MySqlParameter("@awbno", orderShipmentInfo.AwbNo),
                new MySqlParameter("@isshipmentsheduledbyadmin", orderShipmentInfo.IsShipmentSheduledByAdmin),
                new MySqlParameter("@pickuplocationid", orderShipmentInfo.PickupLocationID),
                new MySqlParameter("@errormessage", orderShipmentInfo.ErrorMessage),
                new MySqlParameter("@forwardlable", orderShipmentInfo.ForwardLable),
                new MySqlParameter("@returnlable", orderShipmentInfo.ReturnLable),
                new MySqlParameter("@shipmentTrackingNo", orderShipmentInfo.ShipmentTrackingNo),
                new MySqlParameter("@trackingNo", orderShipmentInfo.TrackingNo),
                new MySqlParameter("@shipmentInfo", orderShipmentInfo.ShipmentInfo),


                new MySqlParameter("@createdBy", orderShipmentInfo.CreatedBy),
                new MySqlParameter("@createdAt", orderShipmentInfo.CreatedAt),

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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),

                 new MySqlParameter("@paymentmode", orderShipmentInfo.PaymentMode),
                new MySqlParameter("@length", orderShipmentInfo.Length),
                new MySqlParameter("@width", orderShipmentInfo.Width),
                new MySqlParameter("@height", orderShipmentInfo.Height),
                new MySqlParameter("@weight", orderShipmentInfo.Weight),
                new MySqlParameter("@invoiceamount", orderShipmentInfo.InvoiceAmount),
                new MySqlParameter("@invoiceCodCharges", orderShipmentInfo.InvoiceCodCharges),
                new MySqlParameter("@packagedescription", orderShipmentInfo.PackageDescription),
                new MySqlParameter("@isshipmentinitiate", orderShipmentInfo.IsShipmentInitiate),
                new MySqlParameter("@ispaymentsuccess", orderShipmentInfo.IsPaymentSuccess),
                new MySqlParameter("@courierid", orderShipmentInfo.CourierID),
                new MySqlParameter("@serviceid", orderShipmentInfo.ServiceID),
                new MySqlParameter("@servicetype", orderShipmentInfo.ServiceType),
                new MySqlParameter("@pickupcontactpersonname", orderShipmentInfo.PickupContactPersonName),
                new MySqlParameter("@pickupcontactpersonmobileno", orderShipmentInfo.PickupContactPersonMobileNo),
                new MySqlParameter("@pickupcontactpersonemailid", orderShipmentInfo.PickupContactPersonEmailID),
                new MySqlParameter("@pickupcompanyname", orderShipmentInfo.PickupCompanyName),
                new MySqlParameter("@pickupaddressline1", orderShipmentInfo.PickupAddressLine1),
                new MySqlParameter("@pickupaddressline2", orderShipmentInfo.PickupAddressLine2),
                new MySqlParameter("@pickuplandmark", orderShipmentInfo.PickupLandmark),
                new MySqlParameter("@pickuppincode", orderShipmentInfo.PickupPincode),
                new MySqlParameter("@pickupcity", orderShipmentInfo.PickupCity),
                new MySqlParameter("@pickupstate", orderShipmentInfo.PickupState),
                new MySqlParameter("@pickupcountry", orderShipmentInfo.PickupCountry),
                new MySqlParameter("@pickupTaxNo", orderShipmentInfo.PickupTaxNo),
                new MySqlParameter("@dropcontactpersonname", orderShipmentInfo.DropContactPersonName),
                new MySqlParameter("@dropcontactpersonmobileno", orderShipmentInfo.DropContactPersonMobileNo),
                new MySqlParameter("@dropcontactpersonemailid", orderShipmentInfo.DropContactPersonEmailID),
                new MySqlParameter("@dropcompanyname", orderShipmentInfo.DropCompanyName),
                new MySqlParameter("@dropaddressline1", orderShipmentInfo.DropAddressLine1),
                new MySqlParameter("@dropaddressline2", orderShipmentInfo.DropAddressLine2),
                new MySqlParameter("@droplandmark", orderShipmentInfo.DropLandmark),
                new MySqlParameter("@droppincode", orderShipmentInfo.DropPincode),
                new MySqlParameter("@dropcity", orderShipmentInfo.DropCity),
                new MySqlParameter("@dropstate", orderShipmentInfo.DropState),
                new MySqlParameter("@dropcountry", orderShipmentInfo.DropCountry),
                new MySqlParameter("@dropTaxNo", orderShipmentInfo.DropTaxNo),
                new MySqlParameter("@shipmentid", orderShipmentInfo.ShipmentID),
                new MySqlParameter("@shipmentorderid", orderShipmentInfo.ShipmentOrderID),
                new MySqlParameter("@shippingpartner", orderShipmentInfo.ShippingPartner),
                new MySqlParameter("@couriername", orderShipmentInfo.CourierName),
                new MySqlParameter("@shippingamountfrompartner", orderShipmentInfo.ShippingAmountFromPartner),
                new MySqlParameter("@awbno", orderShipmentInfo.AwbNo),
                new MySqlParameter("@isshipmentsheduledbyadmin", orderShipmentInfo.IsShipmentSheduledByAdmin),
                new MySqlParameter("@pickuplocationid", orderShipmentInfo.PickupLocationID),
                new MySqlParameter("@errormessage", orderShipmentInfo.ErrorMessage),
                new MySqlParameter("@forwardlable", orderShipmentInfo.ForwardLable),
                new MySqlParameter("@returnlable", orderShipmentInfo.ReturnLable),
                new MySqlParameter("@shipmentTrackingNo", orderShipmentInfo.ShipmentTrackingNo),
                new MySqlParameter("@trackingNo", orderShipmentInfo.TrackingNo),
                new MySqlParameter("@shipmentInfo", orderShipmentInfo.ShipmentInfo),

                new MySqlParameter("@modifiedBy", orderShipmentInfo.ModifiedBy),
                new MySqlParameter("@modifiedAt", orderShipmentInfo.ModifiedAt)

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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),

                new MySqlParameter("@id", orderShipmentInfo.Id),

                new MySqlParameter("@deletedby", orderShipmentInfo.DeletedBy),
                new MySqlParameter("@deletedat", orderShipmentInfo.DeletedAt),

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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", orderShipmentInfo.Id),
                new MySqlParameter("@sellerid", orderShipmentInfo.SellerID),
                new MySqlParameter("@orderid", orderShipmentInfo.OrderID),
                new MySqlParameter("@orderitemids", orderShipmentInfo.OrderItemIDs),
                new MySqlParameter("@packageid", orderShipmentInfo.PackageID),
                new MySqlParameter("@isDeleted", orderShipmentInfo.IsDeleted),
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