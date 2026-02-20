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
    public class TaxTypeRepository:ITaxTypeRespository
    {
        private readonly MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public TaxTypeRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> AddTaxType(TaxTypeLibrary taxTypeLibrary)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@taxType", taxTypeLibrary.TaxType),
                new MySqlParameter("@parentId", taxTypeLibrary.ParentId),
                new MySqlParameter("@createdby", taxTypeLibrary.CreatedBy),
                new MySqlParameter("@createdat", taxTypeLibrary.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.TaxType, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> UpdateTaxType(TaxTypeLibrary taxTypeLibrary)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id", taxTypeLibrary.Id),
                    new MySqlParameter("@taxType", taxTypeLibrary.TaxType),
                    new MySqlParameter("@parentId", taxTypeLibrary.ParentId),
                    new MySqlParameter("@createdby", taxTypeLibrary.CreatedBy),
                    new MySqlParameter("@createdat", taxTypeLibrary.CreatedAt),
                    new MySqlParameter("@modifiedBy", taxTypeLibrary.ModifiedBy),
                    new MySqlParameter("@modifiedAt", taxTypeLibrary.ModifiedAt),
                    new MySqlParameter("@isDeleted", taxTypeLibrary.IsDeleted),
                    new MySqlParameter("@deletedby", taxTypeLibrary.DeletedBy),
                    new MySqlParameter("@deletedat", taxTypeLibrary.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.TaxType, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> DeleteTaxType(TaxTypeLibrary taxTypeLibrary)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", taxTypeLibrary.Id),
                    new MySqlParameter("@deletedby", taxTypeLibrary.DeletedBy),
                    new MySqlParameter("@deletedat", taxTypeLibrary.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.TaxType, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<TaxTypeLibrary>>> GetTaxType(TaxTypeLibrary taxTypeLibrary, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", taxTypeLibrary.Id),
                new MySqlParameter("@taxType", taxTypeLibrary.TaxType),
                new MySqlParameter("@parentId", taxTypeLibrary.ParentId),
                new MySqlParameter("@isdeleted", taxTypeLibrary.IsDeleted),
                new MySqlParameter("@searchtext", taxTypeLibrary.Searchtext),
                new MySqlParameter("@getParent", Getparent),
                new MySqlParameter("@getchild", Getchild),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetTaxType, TaxTypeParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<TaxTypeLibrary>> TaxTypeParserAsync(DbDataReader reader)
        {
            List<TaxTypeLibrary> lstTaxType = new List<TaxTypeLibrary>();
            while (await reader.ReadAsync())
            {
                lstTaxType.Add(new TaxTypeLibrary()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    TaxType = Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxType"))),
                    ParentId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentId"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    ParentName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentName"))),
                });
            }
            return lstTaxType;
        }
    }
}
