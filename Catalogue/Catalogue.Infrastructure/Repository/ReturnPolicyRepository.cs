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
    public class ReturnPolicyRepository:IReturnPolicyRepository
    {
        private readonly MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public ReturnPolicyRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> AddReturnPolicy(ReturnPolicy returnPolicy)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "add"),
                    new MySqlParameter("@name", returnPolicy.Name),
                    new MySqlParameter("@createdby", returnPolicy.CreatedBy),
                    new MySqlParameter("@createdat", returnPolicy.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ReturnPolicy, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> UpdateReturnPolicy(ReturnPolicy returnPolicy)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id", returnPolicy.Id),
                    new MySqlParameter("@name", returnPolicy.Name),
                    new MySqlParameter("@modifiedBy", returnPolicy.ModifiedBy),
                    new MySqlParameter("@modifiedAt", returnPolicy.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ReturnPolicy, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> DeleteReturnPolicy(ReturnPolicy returnPolicy)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", returnPolicy.Id),
                    new MySqlParameter("@deletedby", returnPolicy.DeletedBy),
                    new MySqlParameter("@deletedat", returnPolicy.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ReturnPolicy, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ReturnPolicy>>> GetReturnPolicy(ReturnPolicy returnPolicy, int PageIndex, int PageSize, string Mode)
        {
            var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", returnPolicy.Id),
                new MySqlParameter("@name", returnPolicy.Name),
                new MySqlParameter("@isdeleted", returnPolicy.IsDeleted),
                new MySqlParameter("@searchtext", returnPolicy.Searchtext),
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

            return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetReturnPolicy, ReturnPolicyParserAsync, output, newid: null, message, sqlParams.ToArray());
        }
        private async Task<List<ReturnPolicy>> ReturnPolicyParserAsync(DbDataReader reader)
        {
            List<ReturnPolicy> lstReturnPolicy = new List<ReturnPolicy>();
            while (await reader.ReadAsync())
            {
                lstReturnPolicy.Add(new ReturnPolicy()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                });
            }
            return lstReturnPolicy;
        }
    }
}
