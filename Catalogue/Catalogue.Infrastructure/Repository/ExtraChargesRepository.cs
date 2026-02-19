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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@catid", extraChargesLibrary.CatID),
                new MySqlParameter("@chargespaidbyid", extraChargesLibrary.ChargesPaidByID),
                new MySqlParameter("@name", extraChargesLibrary.Name),
                new MySqlParameter("@chargeson", extraChargesLibrary.ChargesOn),
                new MySqlParameter("@iscompulsary", extraChargesLibrary.IsCompulsary),
                new MySqlParameter("@chargesin", extraChargesLibrary.ChargesIn),
                new MySqlParameter("@percentagevalue", extraChargesLibrary.PercentageValue),
                new MySqlParameter("@amountvalue", extraChargesLibrary.AmountValue),
                new MySqlParameter("@maxamountvalue", extraChargesLibrary.MaxAmountValue),
                new MySqlParameter("@createdby", extraChargesLibrary.CreatedBy),
                new MySqlParameter("@createdat", extraChargesLibrary.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", extraChargesLibrary.Id),
                new MySqlParameter("@catid", extraChargesLibrary.CatID),
                new MySqlParameter("@chargespaidbyid", extraChargesLibrary.ChargesPaidByID),
                new MySqlParameter("@name", extraChargesLibrary.Name),
                new MySqlParameter("@chargeson", extraChargesLibrary.ChargesOn),
                new MySqlParameter("@iscompulsary", extraChargesLibrary.IsCompulsary),
                new MySqlParameter("@chargesin", extraChargesLibrary.ChargesIn),
                new MySqlParameter("@percentagevalue", extraChargesLibrary.PercentageValue),
                new MySqlParameter("@amountvalue", extraChargesLibrary.AmountValue),
                new MySqlParameter("@maxamountvalue", extraChargesLibrary.MaxAmountValue),
                new MySqlParameter("@modifiedby", extraChargesLibrary.ModifiedBy),
                new MySqlParameter("@modifiedat", extraChargesLibrary.ModifiedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", extraChargesLibrary.Id),
                new MySqlParameter("@deletedby", extraChargesLibrary.DeletedBy),
                new MySqlParameter("@deletedat", extraChargesLibrary.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", extraChargesLibrary.Id),
                new MySqlParameter("@catid", extraChargesLibrary.CatID),
                new MySqlParameter("@name", extraChargesLibrary.Name),
                new MySqlParameter("@searchText", extraChargesLibrary.searchText),
                new MySqlParameter("@categoryname", extraChargesLibrary.CategoryName),
                new MySqlParameter("@chargespaidbyname", extraChargesLibrary.ChargesPaidByName),
                new MySqlParameter("@isdeleted", extraChargesLibrary.IsDeleted),
                new MySqlParameter("@isCompulsary", extraChargesLibrary.IsCompulsary),
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
                var sqlParams = new List<MySqlParameter>() {
               
                new MySqlParameter("@CategoryId", CategoryId),

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
