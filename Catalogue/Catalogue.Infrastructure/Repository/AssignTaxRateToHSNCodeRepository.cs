using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Catalogue.Infrastructure.Repository
{
    public class AssignTaxRateToHSNCodeRepository : ITaxRateToHSNCodeRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public AssignTaxRateToHSNCodeRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(AssignTaxRateToHSNCodeLibrary rateToHSNCode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "add"),
                    new SqlParameter("@hsncodeid", rateToHSNCode.HsnCodeId),
                    new SqlParameter("@taxvalueid", rateToHSNCode.TaxValueId),
                    new SqlParameter("@createdby", rateToHSNCode.CreatedBy),
                    new SqlParameter("@createdat", rateToHSNCode.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignTaxRateToHSNCode, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(AssignTaxRateToHSNCodeLibrary rateToHSNCode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@id", rateToHSNCode.Id),
                    new SqlParameter("@hsncodeid", rateToHSNCode.HsnCodeId),
                    new SqlParameter("@taxvalueid", rateToHSNCode.TaxValueId),
                    new SqlParameter("@modifiedby", rateToHSNCode.ModifiedBy),
                    new SqlParameter("@modifiedat", rateToHSNCode.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignTaxRateToHSNCode, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<BaseResponse<long>> Delete(AssignTaxRateToHSNCodeLibrary rateToHSNCode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id", rateToHSNCode.Id),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignTaxRateToHSNCode, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<AssignTaxRateToHSNCodeLibrary>>> get(AssignTaxRateToHSNCodeLibrary rateToHSNCode, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", Mode),
                    new SqlParameter("@id", rateToHSNCode.Id),
                    new SqlParameter("@hsncodeid", rateToHSNCode.HsnCodeId),
                    new SqlParameter("@taxvalueid", rateToHSNCode.TaxValueId),
                    new SqlParameter("@hsncode", rateToHSNCode.HsnCode),
                    new SqlParameter("@taxname", rateToHSNCode.TaxName),
                    new SqlParameter("@searchtext", rateToHSNCode.SearchText),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetAssignTaxRateToHSNCode, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<AssignTaxRateToHSNCodeLibrary>> LayoutParserAsync(DbDataReader reader)
        {
            List<AssignTaxRateToHSNCodeLibrary> lstLayouts = new List<AssignTaxRateToHSNCodeLibrary>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new AssignTaxRateToHSNCodeLibrary()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    HsnCode = Convert.ToString(reader.GetValue(reader.GetOrdinal("HsnCode"))),
                    HsnCodeId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("HsnCodeId"))),
                    TaxType = Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxType"))),
                    TaxName = Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxName"))),
                    TaxValueId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TaxValueId"))),
                    TaxTypeValue = Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxTypeValue"))),
                    DisplayName = Convert.ToString(reader.GetValue(reader.GetOrdinal("DisplayName"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                });
            }
            return lstLayouts;
        }


    }
}
