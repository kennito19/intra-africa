using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using MySqlConnector;
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@taxId", taxmap.TaxId),
                new MySqlParameter("@taxTypeId", taxmap.TaxTypeId),
                new MySqlParameter("@taxMapBy", taxmap.TaxMapBy),
                new MySqlParameter("@SpecificState", taxmap.SpecificState),
                new MySqlParameter("@SpecifictaxTypeId", taxmap.SpecificTaxTypeId),
                new MySqlParameter("@createdBy", taxmap.CreatedBy),
                new MySqlParameter("@createdAt", taxmap.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id", taxmap.Id),
                    new MySqlParameter("@taxId", taxmap.TaxId),
                    new MySqlParameter("@taxTypeId", taxmap.TaxTypeId),
                    new MySqlParameter("@taxMapBy", taxmap.TaxMapBy),
                    new MySqlParameter("@SpecificState", taxmap.SpecificState),
                    new MySqlParameter("@SpecifictaxTypeId", taxmap.SpecificTaxTypeId),
                    new MySqlParameter("@createdby", taxmap.CreatedBy),
                    new MySqlParameter("@createdat", taxmap.CreatedAt),
                    new MySqlParameter("@modifiedBy", taxmap.ModifiedBy),
                    new MySqlParameter("@modifiedAt", taxmap.ModifiedAt),
                    new MySqlParameter("@isDeleted", taxmap.IsDeleted),
                    new MySqlParameter("@deletedby", taxmap.DeletedBy),
                    new MySqlParameter("@deletedat", taxmap.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", taxmap.Id),
                    new MySqlParameter("@deletedby", taxmap.DeletedBy),
                    new MySqlParameter("@deletedat", taxmap.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", taxmap.Id),
                new MySqlParameter("@taxId", taxmap.TaxId),
                new MySqlParameter("@taxValueId", taxmap.TaxValueId),
                new MySqlParameter("@tax", taxmap.Tax),
                new MySqlParameter("@taxTypeId", taxmap.TaxTypeId),
                new MySqlParameter("@taxType", taxmap.TaxType),
                new MySqlParameter("@taxMapBy", taxmap.TaxMapBy),
                new MySqlParameter("@isDeleted", taxmap.IsDeleted),
                new MySqlParameter("@searchtext", taxmap.Searchtext),
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
