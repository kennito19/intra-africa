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
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode","add"),
                    new MySqlParameter("@hsncode",hSNCodeLibrary.HSNCode),
                    new MySqlParameter("@description",hSNCodeLibrary.Description),
                    new MySqlParameter("@createdBy", hSNCodeLibrary.CreatedBy),
                    new MySqlParameter("@createdAt", hSNCodeLibrary.CreatedAt),
                    new MySqlParameter("@modifiedBy", hSNCodeLibrary.ModifiedBy),
                    new MySqlParameter("@modifiedAt", hSNCodeLibrary.ModifiedAt),
                    new MySqlParameter("@deletedBy", hSNCodeLibrary.DeletedBy),
                    new MySqlParameter("@deletedAt", hSNCodeLibrary.DeletedAt),
                    new MySqlParameter("@isDeleted", hSNCodeLibrary.IsDeleted)
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id", hSNCodeLibrary.Id),
                    new MySqlParameter("@hsncode",hSNCodeLibrary.HSNCode),
                    new MySqlParameter("@description",hSNCodeLibrary.Description),
                    new MySqlParameter("@createdBy", hSNCodeLibrary.CreatedBy),
                    new MySqlParameter("@createdAt", hSNCodeLibrary.CreatedAt),
                    new MySqlParameter("@modifiedBy", hSNCodeLibrary.ModifiedBy),
                    new MySqlParameter("@modifiedAt", hSNCodeLibrary.ModifiedAt),
                    new MySqlParameter("@deletedBy", hSNCodeLibrary.DeletedBy),
                    new MySqlParameter("@deletedAt", hSNCodeLibrary.DeletedAt),
                    new MySqlParameter("@isDeleted", hSNCodeLibrary.IsDeleted)
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", hSNCodeLibrary.Id),
                    new MySqlParameter("@deletedBy", hSNCodeLibrary.DeletedBy),
                    new MySqlParameter("@deletedAt", hSNCodeLibrary.DeletedAt)
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", Mode),
                    new MySqlParameter("@id", hSNCodeLibrary.Id),
                    new MySqlParameter("@hsncode",hSNCodeLibrary.HSNCode),
                    new MySqlParameter("@isDeleted", hSNCodeLibrary.IsDeleted),
                    new MySqlParameter("@searchtext", hSNCodeLibrary.Searchtext),
                    new MySqlParameter("@pageIndex", PageIndex),
                    new MySqlParameter("@PageSize", PageSize),

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
