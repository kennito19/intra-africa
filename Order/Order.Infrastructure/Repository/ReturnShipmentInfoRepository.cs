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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),

                 new MySqlParameter("@orderid", returnShipmentInfo.OrderID),
                 new MySqlParameter("@orderitemid", returnShipmentInfo.OrderItemID),
                 new MySqlParameter("@ordercancelreturnid", returnShipmentInfo.OrderCancelReturnID),
                 new MySqlParameter("@qty", returnShipmentInfo.Qty),
                 new MySqlParameter("@paymentmode", returnShipmentInfo.PaymentMode),
                 new MySqlParameter("@length", returnShipmentInfo.Length),
                 new MySqlParameter("@width", returnShipmentInfo.Width),
                 new MySqlParameter("@height", returnShipmentInfo.Height),
                 new MySqlParameter("@weight", returnShipmentInfo.Weight),
                 new MySqlParameter("@returnvalueamount", returnShipmentInfo.ReturnValueAmount),
                 new MySqlParameter("@packagedescription", returnShipmentInfo.PackageDescription),
                 new MySqlParameter("@isshipmentinitiate", returnShipmentInfo.IsShipmentInitiate),
                 new MySqlParameter("@ispaymentsuccess", returnShipmentInfo.IsPaymentSuccess),
                 new MySqlParameter("@courierid", returnShipmentInfo.CourierID),
                 new MySqlParameter("@serviceid", returnShipmentInfo.ServiceID),
                 new MySqlParameter("@servicetype", returnShipmentInfo.ServiceType),
                 new MySqlParameter("@pickupcontactpersonname", returnShipmentInfo.PickupContactPersonName),
                 new MySqlParameter("@pickupcontactpersonmobileno", returnShipmentInfo.PickupContactPersonMobileNo),
                 new MySqlParameter("@pickupcontactpersonemailid", returnShipmentInfo.PickupContactPersonEmailID),
                 new MySqlParameter("@pickupcompanyname", returnShipmentInfo.PickupCompanyName),
                 new MySqlParameter("@pickupaddressline1", returnShipmentInfo.PickupAddressLine1),
                 new MySqlParameter("@pickupaddressline2", returnShipmentInfo.PickupAddressLine2),
                 new MySqlParameter("@pickuplandmark", returnShipmentInfo.PickupLandmark),
                 new MySqlParameter("@pickuppincode", returnShipmentInfo.PickupPincode),
                 new MySqlParameter("@pickupcity", returnShipmentInfo.PickupCity),
                 new MySqlParameter("@pickupstate", returnShipmentInfo.PickupState),
                 new MySqlParameter("@pickupcountry", returnShipmentInfo.PickupCountry),
                 new MySqlParameter("@dropcontactpersonname", returnShipmentInfo.DropContactPersonName),
                 new MySqlParameter("@dropcontactpersonmobileno", returnShipmentInfo.DropContactPersonMobileNo),
                 new MySqlParameter("@dropcontactpersonemailid", returnShipmentInfo.DropContactPersonEmailID),
                 new MySqlParameter("@dropcompanyname", returnShipmentInfo.DropCompanyName),
                 new MySqlParameter("@dropaddressline1", returnShipmentInfo.DropAddressLine1),
                 new MySqlParameter("@dropaddressline2", returnShipmentInfo.DropAddressLine2),
                 new MySqlParameter("@droplandmark", returnShipmentInfo.DropLandmark),
                 new MySqlParameter("@droppincode", returnShipmentInfo.DropPincode),
                 new MySqlParameter("@dropcity", returnShipmentInfo.DropCity),
                 new MySqlParameter("@dropstate", returnShipmentInfo.DropState),
                 new MySqlParameter("@dropcountry", returnShipmentInfo.DropCountry),
                 new MySqlParameter("@shipmentid", returnShipmentInfo.ShipmentID),
                 new MySqlParameter("@shipmentorderid", returnShipmentInfo.ShipmentOrderID),
                 new MySqlParameter("@shippingpartner", returnShipmentInfo.ShippingPartner),
                 new MySqlParameter("@couriername", returnShipmentInfo.CourierName),
                 new MySqlParameter("@shippingamountfrompartner", returnShipmentInfo.ShippingAmountFromPartner),
                 new MySqlParameter("@awbno", returnShipmentInfo.AwbNo),
                 new MySqlParameter("@isshipmentsheduledbyadmin", returnShipmentInfo.IsShipmentSheduledByAdmin),
                 new MySqlParameter("@pickuplocationid", returnShipmentInfo.PickupLocationID),
                 new MySqlParameter("@errormessage", returnShipmentInfo.ErrorMessage),
                 new MySqlParameter("@forwardlable", returnShipmentInfo.ForwardLable),
                 new MySqlParameter("@returnlable", returnShipmentInfo.ReturnLable),
                new MySqlParameter("@shipmentTrackingNo", returnShipmentInfo.ShipmentTrackingNo),
                new MySqlParameter("@trackingNo", returnShipmentInfo.TrackingNo),
                new MySqlParameter("@shipmentInfo", returnShipmentInfo.ShipmentInfo),

                new MySqlParameter("@createdBy", returnShipmentInfo.CreatedBy),
                new MySqlParameter("@createdAt", returnShipmentInfo.CreatedAt),

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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),

                new MySqlParameter("@id", returnShipmentInfo.Id),
                new MySqlParameter("@paymentmode", returnShipmentInfo.PaymentMode),
                new MySqlParameter("@length", returnShipmentInfo.Length),
                new MySqlParameter("@width", returnShipmentInfo.Width),
                new MySqlParameter("@height", returnShipmentInfo.Height),
                new MySqlParameter("@weight", returnShipmentInfo.Weight),
                new MySqlParameter("@returnvalueamount", returnShipmentInfo.ReturnValueAmount),
                new MySqlParameter("@packagedescription", returnShipmentInfo.PackageDescription),
                new MySqlParameter("@isshipmentinitiate", returnShipmentInfo.IsShipmentInitiate),
                new MySqlParameter("@ispaymentsuccess", returnShipmentInfo.IsPaymentSuccess),
                new MySqlParameter("@courierid", returnShipmentInfo.CourierID),
                new MySqlParameter("@serviceid", returnShipmentInfo.ServiceID),
                new MySqlParameter("@servicetype", returnShipmentInfo.ServiceType),
                new MySqlParameter("@pickupcontactpersonname", returnShipmentInfo.PickupContactPersonName),
                new MySqlParameter("@pickupcontactpersonmobileno", returnShipmentInfo.PickupContactPersonMobileNo),
                new MySqlParameter("@pickupcontactpersonemailid", returnShipmentInfo.PickupContactPersonEmailID),
                new MySqlParameter("@pickupcompanyname", returnShipmentInfo.PickupCompanyName),
                new MySqlParameter("@pickupaddressline1", returnShipmentInfo.PickupAddressLine1),
                new MySqlParameter("@pickupaddressline2", returnShipmentInfo.PickupAddressLine2),
                new MySqlParameter("@pickuplandmark", returnShipmentInfo.PickupLandmark),
                new MySqlParameter("@pickuppincode", returnShipmentInfo.PickupPincode),
                new MySqlParameter("@pickupcity", returnShipmentInfo.PickupCity),
                new MySqlParameter("@pickupstate", returnShipmentInfo.PickupState),
                new MySqlParameter("@pickupcountry", returnShipmentInfo.PickupCountry),
                new MySqlParameter("@dropcontactpersonname", returnShipmentInfo.DropContactPersonName),
                new MySqlParameter("@dropcontactpersonmobileno", returnShipmentInfo.DropContactPersonMobileNo),
                new MySqlParameter("@dropcontactpersonemailid", returnShipmentInfo.DropContactPersonEmailID),
                new MySqlParameter("@dropcompanyname", returnShipmentInfo.DropCompanyName),
                new MySqlParameter("@dropaddressline1", returnShipmentInfo.DropAddressLine1),
                new MySqlParameter("@dropaddressline2", returnShipmentInfo.DropAddressLine2),
                new MySqlParameter("@droplandmark", returnShipmentInfo.DropLandmark),
                new MySqlParameter("@droppincode", returnShipmentInfo.DropPincode),
                new MySqlParameter("@dropcity", returnShipmentInfo.DropCity),
                new MySqlParameter("@dropstate", returnShipmentInfo.DropState),
                new MySqlParameter("@dropcountry", returnShipmentInfo.DropCountry),
                new MySqlParameter("@shipmentid", returnShipmentInfo.ShipmentID),
                new MySqlParameter("@shipmentorderid", returnShipmentInfo.ShipmentOrderID),
                new MySqlParameter("@shippingpartner", returnShipmentInfo.ShippingPartner),
                new MySqlParameter("@couriername", returnShipmentInfo.CourierName),
                new MySqlParameter("@shippingamountfrompartner", returnShipmentInfo.ShippingAmountFromPartner),
                new MySqlParameter("@awbno", returnShipmentInfo.AwbNo),
                new MySqlParameter("@isshipmentsheduledbyadmin", returnShipmentInfo.IsShipmentSheduledByAdmin),
                new MySqlParameter("@pickuplocationid", returnShipmentInfo.PickupLocationID),
                new MySqlParameter("@errormessage", returnShipmentInfo.ErrorMessage),
                new MySqlParameter("@forwardlable", returnShipmentInfo.ForwardLable),
                new MySqlParameter("@returnlable", returnShipmentInfo.ReturnLable),
                new MySqlParameter("@shipmentTrackingNo", returnShipmentInfo.ShipmentTrackingNo),
                new MySqlParameter("@trackingNo", returnShipmentInfo.TrackingNo),
                new MySqlParameter("@shipmentInfo", returnShipmentInfo.ShipmentInfo),
                new MySqlParameter("@modifiedBy", returnShipmentInfo.ModifiedBy),
                new MySqlParameter("@modifiedAt", returnShipmentInfo.ModifiedAt)

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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),

                new MySqlParameter("@id", returnShipmentInfo.Id),

                new MySqlParameter("@deletedby", returnShipmentInfo.DeletedBy),
                new MySqlParameter("@deletedat", returnShipmentInfo.DeletedAt),

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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", returnShipmentInfo.Id),
                new MySqlParameter("@orderid", returnShipmentInfo.OrderID),
                new MySqlParameter("@orderitemid", returnShipmentInfo.OrderItemID),
                new MySqlParameter("@ordercancelreturnid", returnShipmentInfo.OrderCancelReturnID),
             
                new MySqlParameter("@isDeleted", returnShipmentInfo.IsDeleted),
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
