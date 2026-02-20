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
    public class CommissionChargesRepository : ICommissionChargesRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;

        public CommissionChargesRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(CommissionChargesLibrary commissionCharges)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@catid", commissionCharges.CatID),
                new MySqlParameter("@sellerid", commissionCharges.SellerID),
                new MySqlParameter("@brandid", commissionCharges.BrandID),
                new MySqlParameter("@chargeson", commissionCharges.ChargesOn),
                new MySqlParameter("@chargesin", commissionCharges.ChargesIn),
                new MySqlParameter("@amountvalue", commissionCharges.AmountValue),
                new MySqlParameter("@iscompulsary", commissionCharges.IsCompulsary),
                new MySqlParameter("@createdby", commissionCharges.CreatedBy),
                new MySqlParameter("@createdat", commissionCharges.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Commission, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(CommissionChargesLibrary commissionCharges)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", commissionCharges.ID),
                new MySqlParameter("@catid", commissionCharges.CatID),
                new MySqlParameter("@sellerid", commissionCharges.SellerID),
                new MySqlParameter("@brandid", commissionCharges.BrandID),
                new MySqlParameter("@chargeson", commissionCharges.ChargesOn),
                new MySqlParameter("@chargesin", commissionCharges.ChargesIn),
                new MySqlParameter("@amountvalue", commissionCharges.AmountValue),
                new MySqlParameter("@iscompulsary", commissionCharges.IsCompulsary),
                new MySqlParameter("@modifiedby", commissionCharges.ModifiedBy),
                new MySqlParameter("@modifiedat", commissionCharges.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Commission, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(CommissionChargesLibrary commissionCharges)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", commissionCharges.ID),
                new MySqlParameter("@deletedby", commissionCharges.DeletedBy),
                new MySqlParameter("@deletedat", commissionCharges.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Commission, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<CommissionChargesLibrary>>> get(CommissionChargesLibrary commissionCharges, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", commissionCharges.ID),
                new MySqlParameter("@catid", commissionCharges.CatID),
                new MySqlParameter("@sellerid", commissionCharges.SellerID),
                new MySqlParameter("@brandid", commissionCharges.BrandID),
                new MySqlParameter("@chargeson", commissionCharges.ChargesOn),
                new MySqlParameter("@isdeleted", commissionCharges.IsDeleted),
                new MySqlParameter("@isCompulsary", commissionCharges.IsCompulsary),
                new MySqlParameter("@getOnlyCategoryRecord", commissionCharges.OnlyCategories),
                new MySqlParameter("@getOnlySellerRecord", commissionCharges.OnlySellers),
                new MySqlParameter("@getOnlyBrandRecord", commissionCharges.OnlyBrands),
                new MySqlParameter("@searchtext", commissionCharges.Searchtext),
                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@PageSize", PageSize),

            };
                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                //MySqlParameter newid = new MySqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetCommission, commissionChargesParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<CommissionChargesLibrary>> commissionChargesParserAsync(DbDataReader reader)
        {
            List<CommissionChargesLibrary> lstcommissionChargesLibrary = new List<CommissionChargesLibrary>();
            while (await reader.ReadAsync())
            {
                lstcommissionChargesLibrary.Add(new CommissionChargesLibrary()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    ID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ID"))),
                    CatID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CatID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CatID"))),
                    SellerID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    BrandID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandID"))),
                    ChargesOn = Convert.ToString(reader.GetValue(reader.GetOrdinal("ChargesOn"))),
                    ChargesIn = Convert.ToString(reader.GetValue(reader.GetOrdinal("ChargesIn"))),
                    AmountValue = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AmountValue")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("AmountValue"))),
                    IsCompulsary = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsCompulsary"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    CategoryName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    BrandName = null,
                    SellerName = null,
                });
            }
            return lstcommissionChargesLibrary;
        }


        public async Task<BaseResponse<List<CommissionChargesLibrary>>> getCategoryWiseCommission(CommissionChargesLibrary commissionCharges)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@catid", commissionCharges.CatID),
                new MySqlParameter("@sellerid", commissionCharges.SellerID),
                new MySqlParameter("@brandid", commissionCharges.BrandID)

            };
                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                //MySqlParameter newid = new MySqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetCommissionByCategoryId, commissionChargesByCategoryIdParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<CommissionChargesLibrary>> commissionChargesByCategoryIdParserAsync(DbDataReader reader)
        {
            List<CommissionChargesLibrary> lstcommissionChargesLibrary = new List<CommissionChargesLibrary>();
            while (await reader.ReadAsync())
            {
                lstcommissionChargesLibrary.Add(new CommissionChargesLibrary()
                {
                    ID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ID"))),
                    CatID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CatID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CatID"))),
                    SellerID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    BrandID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandID"))),
                    ChargesOn = Convert.ToString(reader.GetValue(reader.GetOrdinal("ChargesOn"))),
                    ChargesIn = Convert.ToString(reader.GetValue(reader.GetOrdinal("ChargesIn"))),
                    AmountValue = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AmountValue")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("AmountValue"))),
                    IsCompulsary = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsCompulsary"))),
                });
            }
            return lstcommissionChargesLibrary;
        }

    }
}
