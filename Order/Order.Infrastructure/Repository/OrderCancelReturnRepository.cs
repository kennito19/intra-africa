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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),

                new SqlParameter("@orderid", orderCancelReturn.OrderID),
                new SqlParameter("@orderitemid", orderCancelReturn.OrderItemID),
                new SqlParameter("@neworderno", orderCancelReturn.NewOrderNo),
                new SqlParameter("@qty", orderCancelReturn.Qty),
                new SqlParameter("@actionid", orderCancelReturn.ActionID),
                new SqlParameter("@exchangeproductid", orderCancelReturn.ExchangeProductID),
                new SqlParameter("@exchangesizeId", orderCancelReturn.ExchangeSizeId),
                new SqlParameter("@exchangesize", orderCancelReturn.ExchangeSize),
                new SqlParameter("@exchangepricediff", orderCancelReturn.ExchangePriceDiff),
                new SqlParameter("@userid", orderCancelReturn.UserId),
                new SqlParameter("@username", orderCancelReturn.UserName ),
                new SqlParameter("@userphoneno", orderCancelReturn.UserPhoneNo),
                new SqlParameter("@useremail", orderCancelReturn.UserEmail),
                new SqlParameter("@usergstno", orderCancelReturn.UserGSTNo),
                new SqlParameter("@addressline1", orderCancelReturn.AddressLine1),
                new SqlParameter("@addressline2", orderCancelReturn.AddressLine2),
                new SqlParameter("@landmark", orderCancelReturn.Landmark),
                new SqlParameter("@pincode", orderCancelReturn.Pincode),
                new SqlParameter("@city", orderCancelReturn.City),
                new SqlParameter("@state", orderCancelReturn.State),
                new SqlParameter("@country", orderCancelReturn.Country),
                new SqlParameter("@issue", orderCancelReturn.Issue ),
                new SqlParameter("@reason", orderCancelReturn.Reason ),
                new SqlParameter("@comment", orderCancelReturn.Comment ),
                new SqlParameter("@paymentmode", orderCancelReturn.PaymentMode),
                new SqlParameter("@attachment", orderCancelReturn.Attachment),
                new SqlParameter("@refundamount", orderCancelReturn.RefundAmount),
                new SqlParameter("@refundtype", orderCancelReturn.RefundType),
                new SqlParameter("@bankname", orderCancelReturn.BankName),
                new SqlParameter("@bankbranch", orderCancelReturn.BankBranch),
                new SqlParameter("@bankifsccode", orderCancelReturn.BankIFSCCode),
                new SqlParameter("@bankaccountno", orderCancelReturn.BankAccountNo),
                new SqlParameter("@accounttype", orderCancelReturn.AccountType),
                new SqlParameter("@accountholdername", orderCancelReturn.AccountHolderName),
                new SqlParameter("@approvedbyid", orderCancelReturn.ApprovedByID),
                new SqlParameter("@approvedbyname", orderCancelReturn.ApprovedByName),
                new SqlParameter("@status", orderCancelReturn.Status),
                new SqlParameter("@refundstatus", orderCancelReturn.RefundStatus),

                new SqlParameter("@createdAt", orderCancelReturn.CreatedAt),
                new SqlParameter("@createdBy", orderCancelReturn.CreatedBy)

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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),

                new SqlParameter("@id", orderCancelReturn.Id),
                new SqlParameter("@qty", orderCancelReturn.Qty),
                new SqlParameter("@actionid", orderCancelReturn.ActionID),
                new SqlParameter("@exchangeproductid", orderCancelReturn.ExchangeProductID),
                new SqlParameter("@exchangesizeId", orderCancelReturn.ExchangeSizeId),
                new SqlParameter("@exchangesize", orderCancelReturn.ExchangeSize),
                new SqlParameter("@exchangepricediff", orderCancelReturn.ExchangePriceDiff),
                new SqlParameter("@userid", orderCancelReturn.UserId),
                new SqlParameter("@username", orderCancelReturn.UserName ),
                new SqlParameter("@userphoneno", orderCancelReturn.UserPhoneNo),
                new SqlParameter("@useremail", orderCancelReturn.UserEmail),
                new SqlParameter("@usergstno", orderCancelReturn.UserGSTNo),
                new SqlParameter("@addressline1", orderCancelReturn.AddressLine1),
                new SqlParameter("@addressline2", orderCancelReturn.AddressLine2),
                new SqlParameter("@landmark", orderCancelReturn.Landmark),
                new SqlParameter("@pincode", orderCancelReturn.Pincode),
                new SqlParameter("@city", orderCancelReturn.City),
                new SqlParameter("@state", orderCancelReturn.State),
                new SqlParameter("@country", orderCancelReturn.Country),
                new SqlParameter("@issue", orderCancelReturn.Issue ),
                new SqlParameter("@reason", orderCancelReturn.Reason ),
                new SqlParameter("@comment", orderCancelReturn.Comment ),
                new SqlParameter("@paymentmode", orderCancelReturn.PaymentMode),
                new SqlParameter("@attachment", orderCancelReturn.Attachment),
                new SqlParameter("@refundamount", orderCancelReturn.RefundAmount),
                new SqlParameter("@refundtype", orderCancelReturn.RefundType),
                new SqlParameter("@bankname", orderCancelReturn.BankName),
                new SqlParameter("@bankbranch", orderCancelReturn.BankBranch),
                new SqlParameter("@bankifsccode", orderCancelReturn.BankIFSCCode),
                new SqlParameter("@bankaccountno", orderCancelReturn.BankAccountNo),
                new SqlParameter("@accounttype", orderCancelReturn.AccountType),
                new SqlParameter("@accountholdername", orderCancelReturn.AccountHolderName),
                new SqlParameter("@approvedbyid", orderCancelReturn.ApprovedByID),
                new SqlParameter("@approvedbyname", orderCancelReturn.ApprovedByName),
                new SqlParameter("@status", orderCancelReturn.Status),
                new SqlParameter("@refundstatus", orderCancelReturn.RefundStatus),

                new SqlParameter("@modifiedby", orderCancelReturn.ModifiedBy),
                new SqlParameter("@modifiedat", orderCancelReturn.ModifiedAt)

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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", orderCancelReturn.Id),

                new SqlParameter("@deletedby", orderCancelReturn.DeletedBy),
                new SqlParameter("@deletedat", orderCancelReturn.DeletedAt)

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
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", orderCancelReturn.Id),
                new SqlParameter("@orderid", orderCancelReturn.OrderID),
                new SqlParameter("@orderitemid", orderCancelReturn.OrderItemID),
                new SqlParameter("@neworderno", orderCancelReturn.NewOrderNo),
                new SqlParameter("@orderno", orderCancelReturn.OrderNo),
                new SqlParameter("@selelrId", orderCancelReturn.SellerID),
                new SqlParameter("@brandId", orderCancelReturn.BrandID),
                new SqlParameter("@actionid", orderCancelReturn.ActionID),
                new SqlParameter("@userid", orderCancelReturn.UserId),
                new SqlParameter("@status", orderCancelReturn.Status),
                new SqlParameter("@refundstatus", orderCancelReturn.RefundStatus),
                new SqlParameter("@searchtext", orderCancelReturn.searchText),

                new SqlParameter("@isDeleted", orderCancelReturn.IsDeleted),
                new SqlParameter("@withCancel", orderCancelReturn.WithCancel),
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
