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
using System.Xml.Linq;
using System.Data.Common;

namespace Order.Infrastructure.Repository
{
    public class OrderTrackDetailsRepository : IOrderTrackDetailsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public OrderTrackDetailsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(OrderTrackDetails orderTrackDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@OrderID", orderTrackDetails.OrderID),
                new SqlParameter("@OrderItemID", orderTrackDetails.OrderItemID),
                new SqlParameter("@OrderStage", orderTrackDetails.OrderStage),
                new SqlParameter("@OrderStatus", orderTrackDetails.OrderStatus),
                new SqlParameter("@OrderTrackDetail", orderTrackDetails.OrderTrackDetail),
                new SqlParameter("@TrackDate", orderTrackDetails.TrackDate),
                new SqlParameter("@RejectionType", orderTrackDetails.RejectionType),
                new SqlParameter("@RejectionBy", orderTrackDetails.RejectionBy),
                new SqlParameter("@ReasonForRejection", orderTrackDetails.ReasonForRejection),
                new SqlParameter("@Comment", orderTrackDetails.Comment),

                new SqlParameter("@createdBy", orderTrackDetails.CreatedBy),
                new SqlParameter("@createdAt", orderTrackDetails.CreatedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderTrackDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(OrderTrackDetails orderTrackDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),

                new SqlParameter("@id", orderTrackDetails.Id),
                new SqlParameter("@orderstage", orderTrackDetails.OrderStage),
                new SqlParameter("@orderstatus", orderTrackDetails.OrderStatus),
                new SqlParameter("@ordertrackdetail", orderTrackDetails.OrderTrackDetail),
                new SqlParameter("@trackdate", orderTrackDetails.TrackDate),
                new SqlParameter("@rejectiontype", orderTrackDetails.RejectionType),
                new SqlParameter("@rejectionby", orderTrackDetails.RejectionBy),
                new SqlParameter("@reasonforrejection", orderTrackDetails.ReasonForRejection),
                new SqlParameter("@comment", orderTrackDetails.Comment),
                new SqlParameter("@modifiedBy", orderTrackDetails.ModifiedBy),
                new SqlParameter("@modifiedAt", orderTrackDetails.ModifiedAt),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderTrackDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(OrderTrackDetails orderTrackDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", orderTrackDetails.Id),
                new SqlParameter("@deletedby", orderTrackDetails.DeletedBy),
                new SqlParameter("@deletedat", orderTrackDetails.DeletedAt)



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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderTrackDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<OrderTrackDetails>>> Get(OrderTrackDetails orderTrackDetails, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", orderTrackDetails.Id),
                new SqlParameter("@orderid", orderTrackDetails.OrderID),
                new SqlParameter("@orderitemid", orderTrackDetails.OrderItemID),
                new SqlParameter("@isDeleted", orderTrackDetails.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetOrderTrackDetails, orderTrackDetailsParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<OrderTrackDetails>> orderTrackDetailsParserAsync(DbDataReader reader)
        {
            List<OrderTrackDetails> lstorderTrackDetails = new List<OrderTrackDetails>();
            while (await reader.ReadAsync())
            {
                lstorderTrackDetails.Add(new OrderTrackDetails()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),

                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    OrderID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderID"))),
                    OrderItemID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderItemID"))),
                    
                    OrderStage = Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderStage"))),
                    OrderStatus = Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderStatus"))),
                    OrderTrackDetail = Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderTrackDetail"))),
                    TrackDate = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("TrackDate"))),
                    RejectionType = Convert.ToString(reader.GetValue(reader.GetOrdinal("RejectionType"))),
                    RejectionBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("RejectionBy"))),
                    ReasonForRejection = Convert.ToString(reader.GetValue(reader.GetOrdinal("ReasonForRejection"))),
                    Comment = Convert.ToString(reader.GetValue(reader.GetOrdinal("Comment"))),

                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),

                });
            }
            return lstorderTrackDetails;
        }

    }
}
