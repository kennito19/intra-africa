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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@OrderID", orderTrackDetails.OrderID),
                new MySqlParameter("@OrderItemID", orderTrackDetails.OrderItemID),
                new MySqlParameter("@OrderStage", orderTrackDetails.OrderStage),
                new MySqlParameter("@OrderStatus", orderTrackDetails.OrderStatus),
                new MySqlParameter("@OrderTrackDetail", orderTrackDetails.OrderTrackDetail),
                new MySqlParameter("@TrackDate", orderTrackDetails.TrackDate),
                new MySqlParameter("@RejectionType", orderTrackDetails.RejectionType),
                new MySqlParameter("@RejectionBy", orderTrackDetails.RejectionBy),
                new MySqlParameter("@ReasonForRejection", orderTrackDetails.ReasonForRejection),
                new MySqlParameter("@Comment", orderTrackDetails.Comment),

                new MySqlParameter("@createdBy", orderTrackDetails.CreatedBy),
                new MySqlParameter("@createdAt", orderTrackDetails.CreatedAt)

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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),

                new MySqlParameter("@id", orderTrackDetails.Id),
                new MySqlParameter("@orderstage", orderTrackDetails.OrderStage),
                new MySqlParameter("@orderstatus", orderTrackDetails.OrderStatus),
                new MySqlParameter("@ordertrackdetail", orderTrackDetails.OrderTrackDetail),
                new MySqlParameter("@trackdate", orderTrackDetails.TrackDate),
                new MySqlParameter("@rejectiontype", orderTrackDetails.RejectionType),
                new MySqlParameter("@rejectionby", orderTrackDetails.RejectionBy),
                new MySqlParameter("@reasonforrejection", orderTrackDetails.ReasonForRejection),
                new MySqlParameter("@comment", orderTrackDetails.Comment),
                new MySqlParameter("@modifiedBy", orderTrackDetails.ModifiedBy),
                new MySqlParameter("@modifiedAt", orderTrackDetails.ModifiedAt),

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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", orderTrackDetails.Id),
                new MySqlParameter("@deletedby", orderTrackDetails.DeletedBy),
                new MySqlParameter("@deletedat", orderTrackDetails.DeletedAt)



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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", orderTrackDetails.Id),
                new MySqlParameter("@orderid", orderTrackDetails.OrderID),
                new MySqlParameter("@orderitemid", orderTrackDetails.OrderItemID),
                new MySqlParameter("@isDeleted", orderTrackDetails.IsDeleted),
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
