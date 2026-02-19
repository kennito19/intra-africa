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
using System.Transactions;
using System.Xml.Linq;
using System.Data.Common;

namespace Order.Infrastructure.Repository
{
    public class OrderRefundRepository : IOrderRefundRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public OrderRefundRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(OrderRefund orderRefund)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@ordercancelreturnid", orderRefund.OrderCancelReturnID),
                new MySqlParameter("@orderid", orderRefund.OrderID),
                new MySqlParameter("@orderitemid", orderRefund.OrderItemID),
                new MySqlParameter("@refundamount", orderRefund.RefundAmount),
                new MySqlParameter("@transactionid", orderRefund.TransactionID),
                new MySqlParameter("@comment", orderRefund.Comment),
                new MySqlParameter("@status", orderRefund.Status),
                new MySqlParameter("@createdBy", orderRefund.CreatedBy),
                new MySqlParameter("@createdAt", orderRefund.CreatedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderRefund, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(OrderRefund orderRefund)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),

                new MySqlParameter("@id", orderRefund.Id),
                new MySqlParameter("@ordercancelreturnid", orderRefund.OrderCancelReturnID),
                new MySqlParameter("@orderid", orderRefund.OrderID),
                new MySqlParameter("@orderitemid", orderRefund.OrderItemID),
                new MySqlParameter("@refundamount", orderRefund.RefundAmount),
                new MySqlParameter("@transactionid", orderRefund.TransactionID),
                new MySqlParameter("@comment", orderRefund.Comment),
                new MySqlParameter("@status", orderRefund.Status),

                new MySqlParameter("modifiedBy", orderRefund.ModifiedBy),
                new MySqlParameter("modifiedAt", orderRefund.ModifiedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderRefund, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(OrderRefund orderRefund)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", orderRefund.Id),

                new MySqlParameter("@deletedby", orderRefund.DeletedBy),
                new MySqlParameter("@deletedat", orderRefund.DeletedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderRefund, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<OrderRefund>>> Get(OrderRefund orderRefund, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", orderRefund.Id),
                new MySqlParameter("@ordercancelreturnid", orderRefund.OrderCancelReturnID),
                new MySqlParameter("@orderid", orderRefund.OrderID),
                new MySqlParameter("@orderitemid", orderRefund.OrderItemID),
                new MySqlParameter("@transactionid", orderRefund.TransactionID),
                new MySqlParameter("@status", orderRefund.Status),
                new MySqlParameter("@searchtext", orderRefund.searchText),

                new MySqlParameter("@isDeleted", orderRefund.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetOrderRefund, orderRefundParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private async Task<List<OrderRefund>> orderRefundParserAsync(DbDataReader reader)
        {
            List<OrderRefund> lstorderRefund = new List<OrderRefund>();
            while (await reader.ReadAsync())
            {
                lstorderRefund.Add(new OrderRefund()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),

                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    OrderCancelReturnID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderCancelReturnID"))),
                    OrderID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderID"))),
                    OrderItemID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderItemID"))),
                    RefundAmount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RefundAmount"))),
                    TransactionID = Convert.ToString(reader.GetValue(reader.GetOrdinal("TransactionID"))),
                    Comment = Convert.ToString(reader.GetValue(reader.GetOrdinal("Comment"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),

                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    OrderNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderNo"))),
                    UserName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserName"))),
                    UserPhoneNo = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserPhoneNo")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserPhoneNo"))),
                    UserEmail = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserEmail")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserEmail"))),
                    ProductID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    ProductGUID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGUID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGUID"))),
                    ProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    ProductSKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSKUCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSKUCode"))),

                });
            }
            return lstorderRefund;
        }

    }
}
