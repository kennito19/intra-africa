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
    public class CommissionChargesRepository : ICommissionChargesRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public CommissionChargesRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(CommissionChargesLibrary commissionCharges)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@catid", commissionCharges.CatID),
                new SqlParameter("@sellerid", commissionCharges.SellerID),
                new SqlParameter("@brandid", commissionCharges.BrandID),
                new SqlParameter("@chargeson", commissionCharges.ChargesOn),
                new SqlParameter("@chargesin", commissionCharges.ChargesIn),
                new SqlParameter("@amountvalue", commissionCharges.AmountValue),
                new SqlParameter("@iscompulsary", commissionCharges.IsCompulsary),
                new SqlParameter("@createdby", commissionCharges.CreatedBy),
                new SqlParameter("@createdat", commissionCharges.CreatedAt),
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", commissionCharges.ID),
                new SqlParameter("@catid", commissionCharges.CatID),
                new SqlParameter("@sellerid", commissionCharges.SellerID),
                new SqlParameter("@brandid", commissionCharges.BrandID),
                new SqlParameter("@chargeson", commissionCharges.ChargesOn),
                new SqlParameter("@chargesin", commissionCharges.ChargesIn),
                new SqlParameter("@amountvalue", commissionCharges.AmountValue),
                new SqlParameter("@iscompulsary", commissionCharges.IsCompulsary),
                new SqlParameter("@modifiedby", commissionCharges.ModifiedBy),
                new SqlParameter("@modifiedat", commissionCharges.ModifiedAt),
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", commissionCharges.ID),
                new SqlParameter("@deletedby", commissionCharges.DeletedBy),
                new SqlParameter("@deletedat", commissionCharges.DeletedAt),
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
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", commissionCharges.ID),
                new SqlParameter("@catid", commissionCharges.CatID),
                new SqlParameter("@sellerid", commissionCharges.SellerID),
                new SqlParameter("@brandid", commissionCharges.BrandID),
                new SqlParameter("@chargeson", commissionCharges.ChargesOn),
                new SqlParameter("@isdeleted", commissionCharges.IsDeleted),
                new SqlParameter("@isCompulsary", commissionCharges.IsCompulsary),
                new SqlParameter("@getOnlyCategoryRecord", commissionCharges.OnlyCategories),
                new SqlParameter("@getOnlySellerRecord", commissionCharges.OnlySellers),
                new SqlParameter("@getOnlyBrandRecord", commissionCharges.OnlyBrands),
                new SqlParameter("@searchtext", commissionCharges.Searchtext),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@PageSize", PageSize),

            };
                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                //SqlParameter newid = new SqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@catid", commissionCharges.CatID),
                new SqlParameter("@sellerid", commissionCharges.SellerID),
                new SqlParameter("@brandid", commissionCharges.BrandID)

            };
                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                //SqlParameter newid = new SqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
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
