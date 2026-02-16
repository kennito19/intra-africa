using Catalogue.Application.IRepositories;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class ReturnPolicyDetailRepository:IReturnPolicyDetailRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public ReturnPolicyDetailRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> AddReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "add"),
                    new SqlParameter("@returnpolicyid", returnPolicyDetail.ReturnPolicyID),
                    new SqlParameter("@validitydays", returnPolicyDetail.ValidityDays),
                    new SqlParameter("@title", returnPolicyDetail.Title),
                    new SqlParameter("@covers", returnPolicyDetail.Covers),
                    new SqlParameter("@description", returnPolicyDetail.Description),
                    new SqlParameter("@createdby", returnPolicyDetail.CreatedBy),
                    new SqlParameter("@createdat", returnPolicyDetail.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ReturnPolicyDetail, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> UpdateReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@id", returnPolicyDetail.Id),
                    new SqlParameter("@returnpolicyid", returnPolicyDetail.ReturnPolicyID),
                    new SqlParameter("@validitydays", returnPolicyDetail.ValidityDays),
                    new SqlParameter("@title", returnPolicyDetail.Title),
                    new SqlParameter("@covers", returnPolicyDetail.Covers),
                    new SqlParameter("@description", returnPolicyDetail.Description),
                    new SqlParameter("@modifiedBy", returnPolicyDetail.ModifiedBy),
                    new SqlParameter("@modifiedAt", returnPolicyDetail.ModifiedAt),
                };

                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ReturnPolicyDetail, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> DeleteReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id", returnPolicyDetail.Id),
                    new SqlParameter("@deletedby", returnPolicyDetail.DeletedBy),
                    new SqlParameter("@deletedat", returnPolicyDetail.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ReturnPolicyDetail, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ReturnPolicyDetail>>> GetReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail, int PageIndex, int PageSize, string Mode)
        {
            var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", returnPolicyDetail.Id),
                new SqlParameter("@returnpolicyid", returnPolicyDetail.ReturnPolicyID),
                new SqlParameter("@name", returnPolicyDetail.ReturnPolicy),
                new SqlParameter("@isdeleted", returnPolicyDetail.IsDeleted),
                new SqlParameter("@searchtext", returnPolicyDetail.Searchtext),
                new SqlParameter("@title", returnPolicyDetail.Title),
                new SqlParameter("@days", returnPolicyDetail.days),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@pageSize", PageSize),

            };
            SqlParameter output = new SqlParameter();
            output.ParameterName = "@output";
            output.Direction = ParameterDirection.Output;
            output.SqlDbType = SqlDbType.Int;

            SqlParameter message = new SqlParameter();
            message.ParameterName = "@message";
            message.Direction = ParameterDirection.Output;
            message.SqlDbType = SqlDbType.NVarChar;
            message.Size = 50;

            return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetReturnPolicyDetail, ReturnPolicyDetailParserAsync, output, newid: null, message, sqlParams.ToArray());
        }

        private async Task<List<ReturnPolicyDetail>> ReturnPolicyDetailParserAsync(DbDataReader reader)
        {
            List<ReturnPolicyDetail> lstReturnPolicyDetail = new List<ReturnPolicyDetail>();
            while (await reader.ReadAsync())
            {
                lstReturnPolicyDetail.Add(new ReturnPolicyDetail()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    ReturnPolicyID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ReturnPolicyID"))),
                    ValidityDays = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ValidityDays"))),
                    Title = Convert.ToString(reader.GetValue(reader.GetOrdinal("Title"))),
                    Covers = Convert.ToString(reader.GetValue(reader.GetOrdinal("Covers"))),
                    Description = Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),
                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    ReturnPolicy = Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicy"))),
                });
            }
            return lstReturnPolicyDetail;
        }
    }
}
