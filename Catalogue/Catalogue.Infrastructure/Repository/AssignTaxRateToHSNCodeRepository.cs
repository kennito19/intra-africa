using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
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
        MySqlConnection con;

        public AssignTaxRateToHSNCodeRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(AssignTaxRateToHSNCodeLibrary rateToHSNCode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "add"),
                    new MySqlParameter("@hsncodeid", rateToHSNCode.HsnCodeId),
                    new MySqlParameter("@taxvalueid", rateToHSNCode.TaxValueId),
                    new MySqlParameter("@createdby", rateToHSNCode.CreatedBy),
                    new MySqlParameter("@createdat", rateToHSNCode.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id", rateToHSNCode.Id),
                    new MySqlParameter("@hsncodeid", rateToHSNCode.HsnCodeId),
                    new MySqlParameter("@taxvalueid", rateToHSNCode.TaxValueId),
                    new MySqlParameter("@modifiedby", rateToHSNCode.ModifiedBy),
                    new MySqlParameter("@modifiedat", rateToHSNCode.ModifiedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", rateToHSNCode.Id),
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", Mode),
                    new MySqlParameter("@id", rateToHSNCode.Id),
                    new MySqlParameter("@hsncodeid", rateToHSNCode.HsnCodeId),
                    new MySqlParameter("@taxvalueid", rateToHSNCode.TaxValueId),
                    new MySqlParameter("@hsncode", rateToHSNCode.HsnCode),
                    new MySqlParameter("@taxname", rateToHSNCode.TaxName),
                    new MySqlParameter("@searchtext", rateToHSNCode.SearchText),
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
