using Catalogue.Application.IRepositories;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class ReturnPolicyDetailRepository:IReturnPolicyDetailRepository
    {
        private readonly MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public ReturnPolicyDetailRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> AddReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "add"),
                    new MySqlParameter("@returnpolicyid", returnPolicyDetail.ReturnPolicyID),
                    new MySqlParameter("@validitydays", returnPolicyDetail.ValidityDays),
                    new MySqlParameter("@title", returnPolicyDetail.Title),
                    new MySqlParameter("@covers", returnPolicyDetail.Covers),
                    new MySqlParameter("@description", returnPolicyDetail.Description),
                    new MySqlParameter("@createdby", returnPolicyDetail.CreatedBy),
                    new MySqlParameter("@createdat", returnPolicyDetail.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id", returnPolicyDetail.Id),
                    new MySqlParameter("@returnpolicyid", returnPolicyDetail.ReturnPolicyID),
                    new MySqlParameter("@validitydays", returnPolicyDetail.ValidityDays),
                    new MySqlParameter("@title", returnPolicyDetail.Title),
                    new MySqlParameter("@covers", returnPolicyDetail.Covers),
                    new MySqlParameter("@description", returnPolicyDetail.Description),
                    new MySqlParameter("@modifiedBy", returnPolicyDetail.ModifiedBy),
                    new MySqlParameter("@modifiedAt", returnPolicyDetail.ModifiedAt),
                };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", returnPolicyDetail.Id),
                    new MySqlParameter("@deletedby", returnPolicyDetail.DeletedBy),
                    new MySqlParameter("@deletedat", returnPolicyDetail.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ReturnPolicyDetail, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ReturnPolicyDetail>>> GetReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail, int PageIndex, int PageSize, string Mode)
        {
            var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", returnPolicyDetail.Id),
                new MySqlParameter("@returnpolicyid", returnPolicyDetail.ReturnPolicyID),
                new MySqlParameter("@name", returnPolicyDetail.ReturnPolicy),
                new MySqlParameter("@isdeleted", returnPolicyDetail.IsDeleted),
                new MySqlParameter("@searchtext", returnPolicyDetail.Searchtext),
                new MySqlParameter("@title", returnPolicyDetail.Title),
                new MySqlParameter("@days", returnPolicyDetail.days),
                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@pageSize", PageSize),

            };
            MySqlParameter output = new MySqlParameter();
            output.ParameterName = "@output";
            output.Direction = ParameterDirection.Output;
            output.MySqlDbType = MySqlDbType.Int32;

            MySqlParameter message = new MySqlParameter();
            message.ParameterName = "@message";
            message.Direction = ParameterDirection.Output;
            message.MySqlDbType = MySqlDbType.VarChar;
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
