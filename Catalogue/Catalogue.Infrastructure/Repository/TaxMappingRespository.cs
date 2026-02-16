using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class TaxMappingRespository : ITaxMappingRespository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public TaxMappingRespository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> AddTaxMapping(TaxMapping taxmap)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@taxId", taxmap.TaxId),
                new SqlParameter("@taxTypeId", taxmap.TaxTypeId),
                new SqlParameter("@taxMapBy", taxmap.TaxMapBy),
                new SqlParameter("@SpecificState", taxmap.SpecificState),
                new SqlParameter("@SpecifictaxTypeId", taxmap.SpecificTaxTypeId),
                new SqlParameter("@createdBy", taxmap.CreatedBy),
                new SqlParameter("@createdAt", taxmap.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.TaxMapping, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> UpdateTaxMapping(TaxMapping taxmap)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@id", taxmap.Id),
                    new SqlParameter("@taxId", taxmap.TaxId),
                    new SqlParameter("@taxTypeId", taxmap.TaxTypeId),
                    new SqlParameter("@taxMapBy", taxmap.TaxMapBy),
                    new SqlParameter("@SpecificState", taxmap.SpecificState),
                    new SqlParameter("@SpecifictaxTypeId", taxmap.SpecificTaxTypeId),
                    new SqlParameter("@createdby", taxmap.CreatedBy),
                    new SqlParameter("@createdat", taxmap.CreatedAt),
                    new SqlParameter("@modifiedBy", taxmap.ModifiedBy),
                    new SqlParameter("@modifiedAt", taxmap.ModifiedAt),
                    new SqlParameter("@isDeleted", taxmap.IsDeleted),
                    new SqlParameter("@deletedby", taxmap.DeletedBy),
                    new SqlParameter("@deletedat", taxmap.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.TaxMapping, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> DeleteTaxMapping(TaxMapping taxmap)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id", taxmap.Id),
                    new SqlParameter("@deletedby", taxmap.DeletedBy),
                    new SqlParameter("@deletedat", taxmap.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.TaxMapping, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<TaxMapping>>> GetTaxMapping(TaxMapping taxmap, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", taxmap.Id),
                new SqlParameter("@taxId", taxmap.TaxId),
                new SqlParameter("@taxValueId", taxmap.TaxValueId),
                new SqlParameter("@tax", taxmap.Tax),
                new SqlParameter("@taxTypeId", taxmap.TaxTypeId),
                new SqlParameter("@taxType", taxmap.TaxType),
                new SqlParameter("@taxMapBy", taxmap.TaxMapBy),
                new SqlParameter("@isDeleted", taxmap.IsDeleted),
                new SqlParameter("@searchtext", taxmap.Searchtext),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetTaxMapping, TaxMapParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<TaxMapping>> TaxMapParserAsync(DbDataReader reader)
        {
            List<TaxMapping> lstTaxType = new List<TaxMapping>();
            while (await reader.ReadAsync())
            {
                lstTaxType.Add(new TaxMapping()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    TaxId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TaxId"))),
                    Tax = Convert.ToString(reader.GetValue(reader.GetOrdinal("Tax"))),
                    TaxTypeId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TaxTypeId"))),
                    TaxType = Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxType"))),
                    TaxValueId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxValueId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TaxValueId"))),
                    TaxMapBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxMapBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxMapBy"))),
                    SpecificState = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificState")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificState"))),
                    SpecificTaxTypeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificTaxTypeId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SpecificTaxTypeId"))),
                    SpecificTaxType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificTaxType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificTaxType"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                });
            }
            return lstTaxType;
        }

    }
}
