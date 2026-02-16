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
    public class OrderWiseExtraChargesRepository : IOrderWiseExtraChargesRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public OrderWiseExtraChargesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(OrderWiseExtraCharges orderWiseExtraCharges)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@orderid", orderWiseExtraCharges.OrderID),
                 new SqlParameter("@orderitemid", orderWiseExtraCharges.OrderItemID),
                new SqlParameter("@chargestype", orderWiseExtraCharges.ChargesType),
                new SqlParameter("@chargespaidby", orderWiseExtraCharges.ChargesPaidBy),
                new SqlParameter("@chargesin", orderWiseExtraCharges.ChargesIn),
                new SqlParameter("@chargesvalueinpercentage", orderWiseExtraCharges.ChargesValueInPercentage),
                new SqlParameter("@chargesvalueinamount", orderWiseExtraCharges.ChargesValueInAmount),
                new SqlParameter("@chargesmaxamount", orderWiseExtraCharges.ChargesMaxAmount),
                new SqlParameter("@taxonchargesamount", orderWiseExtraCharges.TaxOnChargesAmount),
                new SqlParameter("@chargesamountwithouttax", orderWiseExtraCharges.ChargesAmountWithoutTax),
                new SqlParameter("@totalCharges", orderWiseExtraCharges.TotalCharges),

                new SqlParameter("@createdBy", orderWiseExtraCharges.CreatedBy),
                new SqlParameter("@createdAt", orderWiseExtraCharges.CreatedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderWiseExtraCharges, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<BaseResponse<long>> Update(OrderWiseExtraCharges orderWiseExtraCharges)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", orderWiseExtraCharges.Id),

                new SqlParameter("@chargestype", orderWiseExtraCharges.ChargesType),
                new SqlParameter("@chargespaidby", orderWiseExtraCharges.ChargesPaidBy),
                new SqlParameter("@chargesin", orderWiseExtraCharges.ChargesIn),
                new SqlParameter("@chargesvalueinpercentage", orderWiseExtraCharges.ChargesValueInPercentage),
                new SqlParameter("@chargesvalueinamount", orderWiseExtraCharges.ChargesValueInAmount),
                new SqlParameter("@chargesmaxamount", orderWiseExtraCharges.ChargesMaxAmount),
                new SqlParameter("@taxonchargesamount", orderWiseExtraCharges.TaxOnChargesAmount),
                new SqlParameter("@chargesamountwithouttax", orderWiseExtraCharges.ChargesAmountWithoutTax),
                new SqlParameter("@totalCharges", orderWiseExtraCharges.TotalCharges),
                new SqlParameter("@modifiedBy", orderWiseExtraCharges.ModifiedBy),
                new SqlParameter("@modifiedAt", orderWiseExtraCharges.ModifiedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderWiseExtraCharges, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Delete(OrderWiseExtraCharges orderWiseExtraCharges)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", orderWiseExtraCharges.Id),
                new SqlParameter("@deletedby", orderWiseExtraCharges.DeletedBy),
                new SqlParameter("@deletedat", orderWiseExtraCharges.DeletedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderWiseExtraCharges, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<OrderWiseExtraCharges>>> Get(OrderWiseExtraCharges orderWiseExtraCharges, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", orderWiseExtraCharges.Id),
                new SqlParameter("@orderid", orderWiseExtraCharges.OrderID),
                new SqlParameter("@orderitemid", orderWiseExtraCharges.OrderItemID),
                new SqlParameter("@isDeleted", orderWiseExtraCharges.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetOrderWiseExtraCharges, orderWiseExtraChargesParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<OrderWiseExtraCharges>> orderWiseExtraChargesParserAsync(DbDataReader reader)
        {
            List<OrderWiseExtraCharges> lstorderWiseExtraCharges = new List<OrderWiseExtraCharges>();
            while (await reader.ReadAsync())
            {
                lstorderWiseExtraCharges.Add(new OrderWiseExtraCharges()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),

                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    OrderID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderID"))),
                    OrderItemID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderItemID"))),
                    ChargesType = Convert.ToString(reader.GetValue(reader.GetOrdinal("ChargesType"))),
                    ChargesPaidBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("ChargesPaidBy"))),
                    ChargesIn = Convert.ToString(reader.GetValue(reader.GetOrdinal("ChargesIn"))),
                    ChargesValueInPercentage = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ChargesValueInPercentage"))),
                    ChargesValueInAmount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ChargesValueInAmount"))),
                    ChargesMaxAmount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ChargesMaxAmount"))),
                    TaxOnChargesAmount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TaxOnChargesAmount"))),
                    ChargesAmountWithoutTax = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ChargesAmountWithoutTax"))),

                    TotalCharges = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalCharges")))) ? 0 : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TotalCharges"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),

                });
            }
            return lstorderWiseExtraCharges;
        }
    }
}
