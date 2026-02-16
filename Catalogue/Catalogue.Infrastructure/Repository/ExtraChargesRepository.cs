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
    public class ExtraChargesRepository:IExtraChargesRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public ExtraChargesRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> AddExtraCharges(ExtraChargesLibrary extraChargesLibrary)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@catid", extraChargesLibrary.CatID),
                new SqlParameter("@chargespaidbyid", extraChargesLibrary.ChargesPaidByID),
                new SqlParameter("@name", extraChargesLibrary.Name),
                new SqlParameter("@chargeson", extraChargesLibrary.ChargesOn),
                new SqlParameter("@iscompulsary", extraChargesLibrary.IsCompulsary),
                new SqlParameter("@chargesin", extraChargesLibrary.ChargesIn),
                new SqlParameter("@percentagevalue", extraChargesLibrary.PercentageValue),
                new SqlParameter("@amountvalue", extraChargesLibrary.AmountValue),
                new SqlParameter("@maxamountvalue", extraChargesLibrary.MaxAmountValue),
                new SqlParameter("@createdby", extraChargesLibrary.CreatedBy),
                new SqlParameter("@createdat", extraChargesLibrary.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Extracharges, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> UpdateExtraCharges(ExtraChargesLibrary extraChargesLibrary)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", extraChargesLibrary.Id),
                new SqlParameter("@catid", extraChargesLibrary.CatID),
                new SqlParameter("@chargespaidbyid", extraChargesLibrary.ChargesPaidByID),
                new SqlParameter("@name", extraChargesLibrary.Name),
                new SqlParameter("@chargeson", extraChargesLibrary.ChargesOn),
                new SqlParameter("@iscompulsary", extraChargesLibrary.IsCompulsary),
                new SqlParameter("@chargesin", extraChargesLibrary.ChargesIn),
                new SqlParameter("@percentagevalue", extraChargesLibrary.PercentageValue),
                new SqlParameter("@amountvalue", extraChargesLibrary.AmountValue),
                new SqlParameter("@maxamountvalue", extraChargesLibrary.MaxAmountValue),
                new SqlParameter("@modifiedby", extraChargesLibrary.ModifiedBy),
                new SqlParameter("@modifiedat", extraChargesLibrary.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Extracharges, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> DeleteExtraCharges(ExtraChargesLibrary extraChargesLibrary)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", extraChargesLibrary.Id),
                new SqlParameter("@deletedby", extraChargesLibrary.DeletedBy),
                new SqlParameter("@deletedat", extraChargesLibrary.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Extracharges, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ExtraChargesLibrary>>> GetExtraCharges(ExtraChargesLibrary extraChargesLibrary, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", extraChargesLibrary.Id),
                new SqlParameter("@catid", extraChargesLibrary.CatID),
                new SqlParameter("@name", extraChargesLibrary.Name),
                new SqlParameter("@searchText", extraChargesLibrary.searchText),
                new SqlParameter("@categoryname", extraChargesLibrary.CategoryName),
                new SqlParameter("@chargespaidbyname", extraChargesLibrary.ChargesPaidByName),
                new SqlParameter("@isdeleted", extraChargesLibrary.IsDeleted),
                new SqlParameter("@isCompulsary", extraChargesLibrary.IsCompulsary),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetExtracharges, ExtraChargesParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<List<ExtraChargesLibrary>>> GetCatExtraCharges(int CategoryId)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
               
                new SqlParameter("@CategoryId", CategoryId),

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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetCatExtracharges, ExtraChargesParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<ExtraChargesLibrary>> ExtraChargesParserAsync(DbDataReader reader)
        {
            List<ExtraChargesLibrary> lstextraCharges = new List<ExtraChargesLibrary>();
            while (await reader.ReadAsync())
            {
                lstextraCharges.Add(new ExtraChargesLibrary()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    CatID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CatID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CatID"))),
                    ChargesPaidByID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ChargesPaidByID"))),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    ChargesOn = Convert.ToString(reader.GetValue(reader.GetOrdinal("ChargesOn"))),
                    IsCompulsary = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsCompulsary"))),
                    ChargesIn = Convert.ToString(reader.GetValue(reader.GetOrdinal("ChargesIn"))),

                    PercentageValue = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PercentageValue"))),
                    AmountValue = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("AmountValue"))),
                    MaxAmountValue = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MaxAmountValue"))),

                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    CategoryName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    ChargesPaidByName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ChargesPaidByName"))),
                });
            }
            return lstextraCharges;
        }

       
    }
}
