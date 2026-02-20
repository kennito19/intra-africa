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
    public class TaxTypeValueRespository:ITaxTypeValueRepository
    {
        private readonly IConfiguration _configuration;
        private MySqlConnection con;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public TaxTypeValueRespository(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);
        }

        public async Task<BaseResponse<long>> AddTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@taxTypeID", taxTypeValueLibrary.TaxTypeID),
                new MySqlParameter("@name", taxTypeValueLibrary.Name),
                new MySqlParameter("@value", taxTypeValueLibrary.Value),
                new MySqlParameter("@createdby", taxTypeValueLibrary.CreatedBy),
                new MySqlParameter("@createdat", taxTypeValueLibrary.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.TaxTypeValue, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> UpdateTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id", taxTypeValueLibrary.Id),
                    new MySqlParameter("@taxTypeID", taxTypeValueLibrary.TaxTypeID),
                    new MySqlParameter("@name", taxTypeValueLibrary.Name),
                    new MySqlParameter("@value", taxTypeValueLibrary.Value),
                    new MySqlParameter("@createdby", taxTypeValueLibrary.CreatedBy),
                    new MySqlParameter("@createdat", taxTypeValueLibrary.CreatedAt),
                    new MySqlParameter("@modifiedBy", taxTypeValueLibrary.ModifiedBy),
                    new MySqlParameter("@modifiedAt", taxTypeValueLibrary.ModifiedAt),
                    new MySqlParameter("@isDeleted", taxTypeValueLibrary.IsDeleted),
                    new MySqlParameter("@deletedby", taxTypeValueLibrary.DeletedBy),
                    new MySqlParameter("@deletedat", taxTypeValueLibrary.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.TaxTypeValue, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> DeleteTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", taxTypeValueLibrary.Id),
                    new MySqlParameter("@deletedby", taxTypeValueLibrary.DeletedBy),
                    new MySqlParameter("@deletedat", taxTypeValueLibrary.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.TaxTypeValue, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<TaxTypeValueLibrary>>> GetTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", taxTypeValueLibrary.Id),
                new MySqlParameter("@taxTypeID", taxTypeValueLibrary.TaxTypeID),
                new MySqlParameter("@name", taxTypeValueLibrary.Name),
                new MySqlParameter("@isdeleted", taxTypeValueLibrary.IsDeleted),
                new MySqlParameter("@searchtext", taxTypeValueLibrary.Searchtext),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetTaxTypeValue, TaxTypeValueParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<TaxTypeValueLibrary>> TaxTypeValueParserAsync(DbDataReader reader)
        {
            List<TaxTypeValueLibrary> lstTaxType = new List<TaxTypeValueLibrary>();
            while (await reader.ReadAsync())
            {
                lstTaxType.Add(new TaxTypeValueLibrary()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    TaxTypeID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TaxTypeID"))),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    Value = Convert.ToString(reader.GetValue(reader.GetOrdinal("Value"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    TaxType = Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxType"))),
                });
            }
            return lstTaxType;
        }
    }
}
