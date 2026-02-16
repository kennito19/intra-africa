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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class TaxTypeValueRespository:ITaxTypeValueRepository
    {
        private readonly IConfiguration _configuration;
        private SqlConnection con;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public TaxTypeValueRespository(IConfiguration configuration)
        {
            _configuration = configuration;
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);
        }

        public async Task<BaseResponse<long>> AddTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@taxTypeID", taxTypeValueLibrary.TaxTypeID),
                new SqlParameter("@name", taxTypeValueLibrary.Name),
                new SqlParameter("@value", taxTypeValueLibrary.Value),
                new SqlParameter("@createdby", taxTypeValueLibrary.CreatedBy),
                new SqlParameter("@createdat", taxTypeValueLibrary.CreatedAt),
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
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@id", taxTypeValueLibrary.Id),
                    new SqlParameter("@taxTypeID", taxTypeValueLibrary.TaxTypeID),
                    new SqlParameter("@name", taxTypeValueLibrary.Name),
                    new SqlParameter("@value", taxTypeValueLibrary.Value),
                    new SqlParameter("@createdby", taxTypeValueLibrary.CreatedBy),
                    new SqlParameter("@createdat", taxTypeValueLibrary.CreatedAt),
                    new SqlParameter("@modifiedBy", taxTypeValueLibrary.ModifiedBy),
                    new SqlParameter("@modifiedAt", taxTypeValueLibrary.ModifiedAt),
                    new SqlParameter("@isDeleted", taxTypeValueLibrary.IsDeleted),
                    new SqlParameter("@deletedby", taxTypeValueLibrary.DeletedBy),
                    new SqlParameter("@deletedat", taxTypeValueLibrary.DeletedAt),
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
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id", taxTypeValueLibrary.Id),
                    new SqlParameter("@deletedby", taxTypeValueLibrary.DeletedBy),
                    new SqlParameter("@deletedat", taxTypeValueLibrary.DeletedAt),
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", taxTypeValueLibrary.Id),
                new SqlParameter("@taxTypeID", taxTypeValueLibrary.TaxTypeID),
                new SqlParameter("@name", taxTypeValueLibrary.Name),
                new SqlParameter("@isdeleted", taxTypeValueLibrary.IsDeleted),
                new SqlParameter("@searchtext", taxTypeValueLibrary.Searchtext),
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
