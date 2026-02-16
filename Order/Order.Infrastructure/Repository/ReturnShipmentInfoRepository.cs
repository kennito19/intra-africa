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
    public class ReturnShipmentInfoRepository : IReturnShipmentInfoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public ReturnShipmentInfoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(ReturnShipmentInfo returnShipmentInfo)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),

                 new SqlParameter("@orderid", returnShipmentInfo.OrderID),
                 new SqlParameter("@orderitemid", returnShipmentInfo.OrderItemID),
                 new SqlParameter("@ordercancelreturnid", returnShipmentInfo.OrderCancelReturnID),
                 new SqlParameter("@qty", returnShipmentInfo.Qty),
                 new SqlParameter("@paymentmode", returnShipmentInfo.PaymentMode),
                 new SqlParameter("@length", returnShipmentInfo.Length),
                 new SqlParameter("@width", returnShipmentInfo.Width),
                 new SqlParameter("@height", returnShipmentInfo.Height),
                 new SqlParameter("@weight", returnShipmentInfo.Weight),
                 new SqlParameter("@returnvalueamount", returnShipmentInfo.ReturnValueAmount),
                 new SqlParameter("@packagedescription", returnShipmentInfo.PackageDescription),
                 new SqlParameter("@isshipmentinitiate", returnShipmentInfo.IsShipmentInitiate),
                 new SqlParameter("@ispaymentsuccess", returnShipmentInfo.IsPaymentSuccess),
                 new SqlParameter("@courierid", returnShipmentInfo.CourierID),
                 new SqlParameter("@serviceid", returnShipmentInfo.ServiceID),
                 new SqlParameter("@servicetype", returnShipmentInfo.ServiceType),
                 new SqlParameter("@pickupcontactpersonname", returnShipmentInfo.PickupContactPersonName),
                 new SqlParameter("@pickupcontactpersonmobileno", returnShipmentInfo.PickupContactPersonMobileNo),
                 new SqlParameter("@pickupcontactpersonemailid", returnShipmentInfo.PickupContactPersonEmailID),
                 new SqlParameter("@pickupcompanyname", returnShipmentInfo.PickupCompanyName),
                 new SqlParameter("@pickupaddressline1", returnShipmentInfo.PickupAddressLine1),
                 new SqlParameter("@pickupaddressline2", returnShipmentInfo.PickupAddressLine2),
                 new SqlParameter("@pickuplandmark", returnShipmentInfo.PickupLandmark),
                 new SqlParameter("@pickuppincode", returnShipmentInfo.PickupPincode),
                 new SqlParameter("@pickupcity", returnShipmentInfo.PickupCity),
                 new SqlParameter("@pickupstate", returnShipmentInfo.PickupState),
                 new SqlParameter("@pickupcountry", returnShipmentInfo.PickupCountry),
                 new SqlParameter("@dropcontactpersonname", returnShipmentInfo.DropContactPersonName),
                 new SqlParameter("@dropcontactpersonmobileno", returnShipmentInfo.DropContactPersonMobileNo),
                 new SqlParameter("@dropcontactpersonemailid", returnShipmentInfo.DropContactPersonEmailID),
                 new SqlParameter("@dropcompanyname", returnShipmentInfo.DropCompanyName),
                 new SqlParameter("@dropaddressline1", returnShipmentInfo.DropAddressLine1),
                 new SqlParameter("@dropaddressline2", returnShipmentInfo.DropAddressLine2),
                 new SqlParameter("@droplandmark", returnShipmentInfo.DropLandmark),
                 new SqlParameter("@droppincode", returnShipmentInfo.DropPincode),
                 new SqlParameter("@dropcity", returnShipmentInfo.DropCity),
                 new SqlParameter("@dropstate", returnShipmentInfo.DropState),
                 new SqlParameter("@dropcountry", returnShipmentInfo.DropCountry),
                 new SqlParameter("@shipmentid", returnShipmentInfo.ShipmentID),
                 new SqlParameter("@shipmentorderid", returnShipmentInfo.ShipmentOrderID),
                 new SqlParameter("@shippingpartner", returnShipmentInfo.ShippingPartner),
                 new SqlParameter("@couriername", returnShipmentInfo.CourierName),
                 new SqlParameter("@shippingamountfrompartner", returnShipmentInfo.ShippingAmountFromPartner),
                 new SqlParameter("@awbno", returnShipmentInfo.AwbNo),
                 new SqlParameter("@isshipmentsheduledbyadmin", returnShipmentInfo.IsShipmentSheduledByAdmin),
                 new SqlParameter("@pickuplocationid", returnShipmentInfo.PickupLocationID),
                 new SqlParameter("@errormessage", returnShipmentInfo.ErrorMessage),
                 new SqlParameter("@forwardlable", returnShipmentInfo.ForwardLable),
                 new SqlParameter("@returnlable", returnShipmentInfo.ReturnLable),
                new SqlParameter("@shipmentTrackingNo", returnShipmentInfo.ShipmentTrackingNo),
                new SqlParameter("@trackingNo", returnShipmentInfo.TrackingNo),
                new SqlParameter("@shipmentInfo", returnShipmentInfo.ShipmentInfo),

                new SqlParameter("@createdBy", returnShipmentInfo.CreatedBy),
                new SqlParameter("@createdAt", returnShipmentInfo.CreatedAt),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ReturnShipmentInfo, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(ReturnShipmentInfo returnShipmentInfo)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),

                new SqlParameter("@id", returnShipmentInfo.Id),
                new SqlParameter("@paymentmode", returnShipmentInfo.PaymentMode),
                new SqlParameter("@length", returnShipmentInfo.Length),
                new SqlParameter("@width", returnShipmentInfo.Width),
                new SqlParameter("@height", returnShipmentInfo.Height),
                new SqlParameter("@weight", returnShipmentInfo.Weight),
                new SqlParameter("@returnvalueamount", returnShipmentInfo.ReturnValueAmount),
                new SqlParameter("@packagedescription", returnShipmentInfo.PackageDescription),
                new SqlParameter("@isshipmentinitiate", returnShipmentInfo.IsShipmentInitiate),
                new SqlParameter("@ispaymentsuccess", returnShipmentInfo.IsPaymentSuccess),
                new SqlParameter("@courierid", returnShipmentInfo.CourierID),
                new SqlParameter("@serviceid", returnShipmentInfo.ServiceID),
                new SqlParameter("@servicetype", returnShipmentInfo.ServiceType),
                new SqlParameter("@pickupcontactpersonname", returnShipmentInfo.PickupContactPersonName),
                new SqlParameter("@pickupcontactpersonmobileno", returnShipmentInfo.PickupContactPersonMobileNo),
                new SqlParameter("@pickupcontactpersonemailid", returnShipmentInfo.PickupContactPersonEmailID),
                new SqlParameter("@pickupcompanyname", returnShipmentInfo.PickupCompanyName),
                new SqlParameter("@pickupaddressline1", returnShipmentInfo.PickupAddressLine1),
                new SqlParameter("@pickupaddressline2", returnShipmentInfo.PickupAddressLine2),
                new SqlParameter("@pickuplandmark", returnShipmentInfo.PickupLandmark),
                new SqlParameter("@pickuppincode", returnShipmentInfo.PickupPincode),
                new SqlParameter("@pickupcity", returnShipmentInfo.PickupCity),
                new SqlParameter("@pickupstate", returnShipmentInfo.PickupState),
                new SqlParameter("@pickupcountry", returnShipmentInfo.PickupCountry),
                new SqlParameter("@dropcontactpersonname", returnShipmentInfo.DropContactPersonName),
                new SqlParameter("@dropcontactpersonmobileno", returnShipmentInfo.DropContactPersonMobileNo),
                new SqlParameter("@dropcontactpersonemailid", returnShipmentInfo.DropContactPersonEmailID),
                new SqlParameter("@dropcompanyname", returnShipmentInfo.DropCompanyName),
                new SqlParameter("@dropaddressline1", returnShipmentInfo.DropAddressLine1),
                new SqlParameter("@dropaddressline2", returnShipmentInfo.DropAddressLine2),
                new SqlParameter("@droplandmark", returnShipmentInfo.DropLandmark),
                new SqlParameter("@droppincode", returnShipmentInfo.DropPincode),
                new SqlParameter("@dropcity", returnShipmentInfo.DropCity),
                new SqlParameter("@dropstate", returnShipmentInfo.DropState),
                new SqlParameter("@dropcountry", returnShipmentInfo.DropCountry),
                new SqlParameter("@shipmentid", returnShipmentInfo.ShipmentID),
                new SqlParameter("@shipmentorderid", returnShipmentInfo.ShipmentOrderID),
                new SqlParameter("@shippingpartner", returnShipmentInfo.ShippingPartner),
                new SqlParameter("@couriername", returnShipmentInfo.CourierName),
                new SqlParameter("@shippingamountfrompartner", returnShipmentInfo.ShippingAmountFromPartner),
                new SqlParameter("@awbno", returnShipmentInfo.AwbNo),
                new SqlParameter("@isshipmentsheduledbyadmin", returnShipmentInfo.IsShipmentSheduledByAdmin),
                new SqlParameter("@pickuplocationid", returnShipmentInfo.PickupLocationID),
                new SqlParameter("@errormessage", returnShipmentInfo.ErrorMessage),
                new SqlParameter("@forwardlable", returnShipmentInfo.ForwardLable),
                new SqlParameter("@returnlable", returnShipmentInfo.ReturnLable),
                new SqlParameter("@shipmentTrackingNo", returnShipmentInfo.ShipmentTrackingNo),
                new SqlParameter("@trackingNo", returnShipmentInfo.TrackingNo),
                new SqlParameter("@shipmentInfo", returnShipmentInfo.ShipmentInfo),
                new SqlParameter("@modifiedBy", returnShipmentInfo.ModifiedBy),
                new SqlParameter("@modifiedAt", returnShipmentInfo.ModifiedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ReturnShipmentInfo, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(ReturnShipmentInfo returnShipmentInfo)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),

                new SqlParameter("@id", returnShipmentInfo.Id),

                new SqlParameter("@deletedby", returnShipmentInfo.DeletedBy),
                new SqlParameter("@deletedat", returnShipmentInfo.DeletedAt),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ReturnShipmentInfo, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ReturnShipmentInfo>>> Get(ReturnShipmentInfo returnShipmentInfo, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", returnShipmentInfo.Id),
                new SqlParameter("@orderid", returnShipmentInfo.OrderID),
                new SqlParameter("@orderitemid", returnShipmentInfo.OrderItemID),
                new SqlParameter("@ordercancelreturnid", returnShipmentInfo.OrderCancelReturnID),
             
                new SqlParameter("@isDeleted", returnShipmentInfo.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetReturnShipmentInfo, returnShipmentInfoParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ReturnShipmentInfo>> returnShipmentInfoParserAsync(DbDataReader reader)
        {
            List<ReturnShipmentInfo> lstreturnShipmentInfo = new List<ReturnShipmentInfo>();
            while (await reader.ReadAsync())
            {
                lstreturnShipmentInfo.Add(new ReturnShipmentInfo()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),

                    OrderID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderID"))),
                    OrderItemID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderItemID"))),
                    OrderCancelReturnID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderCancelReturnID"))),
                    Qty = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Qty"))),
                    PaymentMode = Convert.ToString(reader.GetValue(reader.GetOrdinal("PaymentMode"))),
                    
                    Length = Convert.ToString(reader.GetValue(reader.GetOrdinal("Length"))),
                    Width = Convert.ToString(reader.GetValue(reader.GetOrdinal("Width"))),
                    Height = Convert.ToString(reader.GetValue(reader.GetOrdinal("Height"))),
                    Weight = Convert.ToString(reader.GetValue(reader.GetOrdinal("Weight"))),
                    ReturnValueAmount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ReturnValueAmount"))),
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
            return lstreturnShipmentInfo;
        }

    }
}
