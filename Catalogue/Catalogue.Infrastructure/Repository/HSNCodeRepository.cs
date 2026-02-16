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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class HSNCodeRepository:IHSNCodeRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public HSNCodeRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> addHSNCode(HSNCodeLibrary hSNCodeLibrary)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode","add"),
                    new SqlParameter("@hsncode",hSNCodeLibrary.HSNCode),
                    new SqlParameter("@description",hSNCodeLibrary.Description),
                    new SqlParameter("@createdBy", hSNCodeLibrary.CreatedBy),
                    new SqlParameter("@createdAt", hSNCodeLibrary.CreatedAt),
                    new SqlParameter("@modifiedBy", hSNCodeLibrary.ModifiedBy),
                    new SqlParameter("@modifiedAt", hSNCodeLibrary.ModifiedAt),
                    new SqlParameter("@deletedBy", hSNCodeLibrary.DeletedBy),
                    new SqlParameter("@deletedAt", hSNCodeLibrary.DeletedAt),
                    new SqlParameter("@isDeleted", hSNCodeLibrary.IsDeleted)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.HSNCode, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> updateHSNCode(HSNCodeLibrary hSNCodeLibrary)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@id", hSNCodeLibrary.Id),
                    new SqlParameter("@hsncode",hSNCodeLibrary.HSNCode),
                    new SqlParameter("@description",hSNCodeLibrary.Description),
                    new SqlParameter("@createdBy", hSNCodeLibrary.CreatedBy),
                    new SqlParameter("@createdAt", hSNCodeLibrary.CreatedAt),
                    new SqlParameter("@modifiedBy", hSNCodeLibrary.ModifiedBy),
                    new SqlParameter("@modifiedAt", hSNCodeLibrary.ModifiedAt),
                    new SqlParameter("@deletedBy", hSNCodeLibrary.DeletedBy),
                    new SqlParameter("@deletedAt", hSNCodeLibrary.DeletedAt),
                    new SqlParameter("@isDeleted", hSNCodeLibrary.IsDeleted)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.HSNCode, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> deleteHSNCode(HSNCodeLibrary hSNCodeLibrary)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id", hSNCodeLibrary.Id),
                    new SqlParameter("@deletedBy", hSNCodeLibrary.DeletedBy),
                    new SqlParameter("@deletedAt", hSNCodeLibrary.DeletedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.HSNCode, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<HSNCodeLibrary>>> getHSNCode(HSNCodeLibrary hSNCodeLibrary, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", Mode),
                    new SqlParameter("@id", hSNCodeLibrary.Id),
                    new SqlParameter("@hsncode",hSNCodeLibrary.HSNCode),
                    new SqlParameter("@isDeleted", hSNCodeLibrary.IsDeleted),
                    new SqlParameter("@searchtext", hSNCodeLibrary.Searchtext),
                    new SqlParameter("@pageIndex", PageIndex),
                    new SqlParameter("@PageSize", PageSize),

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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetHSNCode, HSNCodeParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<HSNCodeLibrary>> HSNCodeParserAsync(DbDataReader reader)
        {
            List<HSNCodeLibrary> lstHSNCodeLibrary  = new List<HSNCodeLibrary>();
            while (await reader.ReadAsync())
            {
                lstHSNCodeLibrary.Add(new HSNCodeLibrary()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    HSNCode = Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode"))),
                    Description = Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),
                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                });
            }
            return lstHSNCodeLibrary;
        }

    }
}
