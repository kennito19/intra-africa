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
using System.Diagnostics.Metrics;
using System.Net.Mail;
using System.Xml.Linq;
using System.Data.Common;

namespace Order.Infrastructure.Repository
{
    public class OrderCancelReturnRepository : IOrderCancelReturnRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public OrderCancelReturnRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(OrderCancelReturn orderCancelReturn)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),

                new MySqlParameter("@orderid", orderCancelReturn.OrderID),
                new MySqlParameter("@orderitemid", orderCancelReturn.OrderItemID),
                new MySqlParameter("@neworderno", orderCancelReturn.NewOrderNo),
                new MySqlParameter("@qty", orderCancelReturn.Qty),
                new MySqlParameter("@actionid", orderCancelReturn.ActionID),
                new MySqlParameter("@exchangeproductid", orderCancelReturn.ExchangeProductID),
                new MySqlParameter("@exchangesizeId", orderCancelReturn.ExchangeSizeId),
                new MySqlParameter("@exchangesize", orderCancelReturn.ExchangeSize),
                new MySqlParameter("@exchangepricediff", orderCancelReturn.ExchangePriceDiff),
                new MySqlParameter("@userid", orderCancelReturn.UserId),
                new MySqlParameter("@username", orderCancelReturn.UserName ),
                new MySqlParameter("@userphoneno", orderCancelReturn.UserPhoneNo),
                new MySqlParameter("@useremail", orderCancelReturn.UserEmail),
                new MySqlParameter("@usergstno", orderCancelReturn.UserGSTNo),
                new MySqlParameter("@addressline1", orderCancelReturn.AddressLine1),
                new MySqlParameter("@addressline2", orderCancelReturn.AddressLine2),
                new MySqlParameter("@landmark", orderCancelReturn.Landmark),
                new MySqlParameter("@pincode", orderCancelReturn.Pincode),
                new MySqlParameter("@city", orderCancelReturn.City),
                new MySqlParameter("@state", orderCancelReturn.State),
                new MySqlParameter("@country", orderCancelReturn.Country),
                new MySqlParameter("@issue", orderCancelReturn.Issue ),
                new MySqlParameter("@reason", orderCancelReturn.Reason ),
                new MySqlParameter("@comment", orderCancelReturn.Comment ),
                new MySqlParameter("@paymentmode", orderCancelReturn.PaymentMode),
                new MySqlParameter("@attachment", orderCancelReturn.Attachment),
                new MySqlParameter("@refundamount", orderCancelReturn.RefundAmount),
                new MySqlParameter("@refundtype", orderCancelReturn.RefundType),
                new MySqlParameter("@bankname", orderCancelReturn.BankName),
                new MySqlParameter("@bankbranch", orderCancelReturn.BankBranch),
                new MySqlParameter("@bankifsccode", orderCancelReturn.BankIFSCCode),
                new MySqlParameter("@bankaccountno", orderCancelReturn.BankAccountNo),
                new MySqlParameter("@accounttype", orderCancelReturn.AccountType),
                new MySqlParameter("@accountholdername", orderCancelReturn.AccountHolderName),
                new MySqlParameter("@approvedbyid", orderCancelReturn.ApprovedByID),
                new MySqlParameter("@approvedbyname", orderCancelReturn.ApprovedByName),
                new MySqlParameter("@status", orderCancelReturn.Status),
                new MySqlParameter("@refundstatus", orderCancelReturn.RefundStatus),

                new MySqlParameter("@createdAt", orderCancelReturn.CreatedAt),
                new MySqlParameter("@createdBy", orderCancelReturn.CreatedBy)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderCancelReturn, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(OrderCancelReturn orderCancelReturn)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),

                new MySqlParameter("@id", orderCancelReturn.Id),
                new MySqlParameter("@qty", orderCancelReturn.Qty),
                new MySqlParameter("@actionid", orderCancelReturn.ActionID),
                new MySqlParameter("@exchangeproductid", orderCancelReturn.ExchangeProductID),
                new MySqlParameter("@exchangesizeId", orderCancelReturn.ExchangeSizeId),
                new MySqlParameter("@exchangesize", orderCancelReturn.ExchangeSize),
                new MySqlParameter("@exchangepricediff", orderCancelReturn.ExchangePriceDiff),
                new MySqlParameter("@userid", orderCancelReturn.UserId),
                new MySqlParameter("@username", orderCancelReturn.UserName ),
                new MySqlParameter("@userphoneno", orderCancelReturn.UserPhoneNo),
                new MySqlParameter("@useremail", orderCancelReturn.UserEmail),
                new MySqlParameter("@usergstno", orderCancelReturn.UserGSTNo),
                new MySqlParameter("@addressline1", orderCancelReturn.AddressLine1),
                new MySqlParameter("@addressline2", orderCancelReturn.AddressLine2),
                new MySqlParameter("@landmark", orderCancelReturn.Landmark),
                new MySqlParameter("@pincode", orderCancelReturn.Pincode),
                new MySqlParameter("@city", orderCancelReturn.City),
                new MySqlParameter("@state", orderCancelReturn.State),
                new MySqlParameter("@country", orderCancelReturn.Country),
                new MySqlParameter("@issue", orderCancelReturn.Issue ),
                new MySqlParameter("@reason", orderCancelReturn.Reason ),
                new MySqlParameter("@comment", orderCancelReturn.Comment ),
                new MySqlParameter("@paymentmode", orderCancelReturn.PaymentMode),
                new MySqlParameter("@attachment", orderCancelReturn.Attachment),
                new MySqlParameter("@refundamount", orderCancelReturn.RefundAmount),
                new MySqlParameter("@refundtype", orderCancelReturn.RefundType),
                new MySqlParameter("@bankname", orderCancelReturn.BankName),
                new MySqlParameter("@bankbranch", orderCancelReturn.BankBranch),
                new MySqlParameter("@bankifsccode", orderCancelReturn.BankIFSCCode),
                new MySqlParameter("@bankaccountno", orderCancelReturn.BankAccountNo),
                new MySqlParameter("@accounttype", orderCancelReturn.AccountType),
                new MySqlParameter("@accountholdername", orderCancelReturn.AccountHolderName),
                new MySqlParameter("@approvedbyid", orderCancelReturn.ApprovedByID),
                new MySqlParameter("@approvedbyname", orderCancelReturn.ApprovedByName),
                new MySqlParameter("@status", orderCancelReturn.Status),
                new MySqlParameter("@refundstatus", orderCancelReturn.RefundStatus),

                new MySqlParameter("@modifiedby", orderCancelReturn.ModifiedBy),
                new MySqlParameter("@modifiedat", orderCancelReturn.ModifiedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderCancelReturn, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(OrderCancelReturn orderCancelReturn)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", orderCancelReturn.Id),

                new MySqlParameter("@deletedby", orderCancelReturn.DeletedBy),
                new MySqlParameter("@deletedat", orderCancelReturn.DeletedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderCancelReturn, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<OrderCancelReturn>>> Get(OrderCancelReturn orderCancelReturn, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", orderCancelReturn.Id),
                new MySqlParameter("@orderid", orderCancelReturn.OrderID),
                new MySqlParameter("@orderitemid", orderCancelReturn.OrderItemID),
                new MySqlParameter("@neworderno", orderCancelReturn.NewOrderNo),
                new MySqlParameter("@orderno", orderCancelReturn.OrderNo),
                new MySqlParameter("@selelrId", orderCancelReturn.SellerID),
                new MySqlParameter("@brandId", orderCancelReturn.BrandID),
                new MySqlParameter("@actionid", orderCancelReturn.ActionID),
                new MySqlParameter("@userid", orderCancelReturn.UserId),
                new MySqlParameter("@status", orderCancelReturn.Status),
                new MySqlParameter("@refundstatus", orderCancelReturn.RefundStatus),
                new MySqlParameter("@searchtext", orderCancelReturn.searchText),

                new MySqlParameter("@isDeleted", orderCancelReturn.IsDeleted),
                new MySqlParameter("@withCancel", orderCancelReturn.WithCancel),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetOrderCancelReturn, orderCancelReturnParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private async Task<List<OrderCancelReturn>> orderCancelReturnParserAsync(DbDataReader reader)
        {
            List<OrderCancelReturn> lstorderCancelReturn = new List<OrderCancelReturn>();
            while (await reader.ReadAsync())
            {
                lstorderCancelReturn.Add(new OrderCancelReturn()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),


                    OrderID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderID"))),
                    OrderItemID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderItemID"))),
                    NewOrderNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserGSTNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("NewOrderNo"))),
                    Qty = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Qty"))),
                    ActionID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ActionID"))),

                    ExchangeProductID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExchangeProductID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ExchangeProductID"))),
                    ExchangeSizeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExchangeSizeId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ExchangeSizeId"))),
                    ExchangeSize = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserGSTNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExchangeSize"))),
                    ExchangePriceDiff = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExchangePriceDiff")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ExchangePriceDiff"))),

                    UserId = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId"))),
                    UserName = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserName"))),
                    UserPhoneNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserPhoneNo"))),
                    UserEmail = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserEmail"))),
                    UserGSTNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserGSTNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserGSTNo"))),
                    AddressLine1 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AddressLine1")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AddressLine1"))),
                    AddressLine2 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AddressLine2")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AddressLine2"))),
                    Landmark = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Landmark")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Landmark"))),
                    Pincode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Pincode")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Pincode"))),
                    City = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("City")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("City"))),
                    State = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("State")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("State"))),
                    Country = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Country")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Country"))),
                    Issue = Convert.ToString(reader.GetValue(reader.GetOrdinal("Issue"))),
                    Reason = Convert.ToString(reader.GetValue(reader.GetOrdinal("Reason"))),
                    Comment = Convert.ToString(reader.GetValue(reader.GetOrdinal("Comment"))),
                    PaymentMode = Convert.ToString(reader.GetValue(reader.GetOrdinal("PaymentMode"))),
                    Attachment = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Attachment")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Attachment"))),
                    RefundAmount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("RefundAmount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("RefundAmount"))),
                    RefundType = Convert.ToString(reader.GetValue(reader.GetOrdinal("RefundType"))),
                    BankName = Convert.ToString(reader.GetValue(reader.GetOrdinal("BankName"))),
                    BankBranch = Convert.ToString(reader.GetValue(reader.GetOrdinal("BankBranch"))),
                    BankIFSCCode = Convert.ToString(reader.GetValue(reader.GetOrdinal("BankIFSCCode"))),
                    BankAccountNo = Convert.ToString(reader.GetValue(reader.GetOrdinal("BankAccountNo"))),
                    AccountType = Convert.ToString(reader.GetValue(reader.GetOrdinal("AccountType"))),
                    AccountHolderName = Convert.ToString(reader.GetValue(reader.GetOrdinal("AccountHolderName"))),
                    ApprovedByID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ApprovedByID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ApprovedByID"))),
                    ApprovedByName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ApprovedByName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ApprovedByName"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    RefundStatus = Convert.ToString(reader.GetValue(reader.GetOrdinal("RefundStatus"))),



                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    ReturnAction = Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnAction"))),
                    OrderNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo"))),
                    OrderBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderBy"))),
                    TotalAmount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalAmount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalAmount"))),
                    PaidAmount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PaidAmount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PaidAmount"))),
                    OrderPaymentMode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderPaymentMode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderPaymentMode"))),
                    BrandID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandID"))),
                    SellerID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    ItemTotalAmount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemTotalAmount")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ItemTotalAmount"))),
                    ItemSubTotal = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemSubTotal")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ItemSubTotal"))),
                });
            }
            return lstorderCancelReturn;
        }

    }
}
