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
    public class ManageFlashSalePriceMasterRepository : IManageFlashSalePriceMasterRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public ManageFlashSalePriceMasterRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(FlashSalePriceMasterLibrary flashSalePrice)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@sellerProductId", flashSalePrice.SellerProductId),
                new SqlParameter("@sellerWiseProductPriceMasterId", flashSalePrice.SellerWiseProductPriceMasterId),
                new SqlParameter("@collectionId", flashSalePrice.CollectionId),
                new SqlParameter("@collectionMappingId", flashSalePrice.CollectionMappingId),
                new SqlParameter("@mrp", flashSalePrice.MRP),
                new SqlParameter("@sellingprice", flashSalePrice.SellingPrice),
                new SqlParameter("@discount", flashSalePrice.Discount),
                new SqlParameter("@status", flashSalePrice.Status),
                new SqlParameter("@isselleroptin", flashSalePrice.IsSellerOptIn),
                new SqlParameter("@createdby", flashSalePrice.CreatedBy),
                new SqlParameter("@createdat", flashSalePrice.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageFlashSalePriceMaster, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(FlashSalePriceMasterLibrary flashSalePrice)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", flashSalePrice.Id),
                new SqlParameter("@sellerProductId", flashSalePrice.SellerProductId),
                new SqlParameter("@sellerWiseProductPriceMasterId", flashSalePrice.SellerWiseProductPriceMasterId),
                new SqlParameter("@collectionId", flashSalePrice.CollectionId),
                new SqlParameter("@collectionMappingId", flashSalePrice.CollectionMappingId),
                new SqlParameter("@mrp", flashSalePrice.MRP),
                new SqlParameter("@sellingprice", flashSalePrice.SellingPrice),
                new SqlParameter("@discount", flashSalePrice.Discount),
                new SqlParameter("@status", flashSalePrice.Status),
                new SqlParameter("@isselleroptin", flashSalePrice.IsSellerOptIn),
                new SqlParameter("@modifiedby", flashSalePrice.ModifiedBy),
                new SqlParameter("@modifiedat", flashSalePrice.ModifiedAt),
                new SqlParameter("@isDeleted", flashSalePrice.IsDeleted),
                new SqlParameter("@deletedby", flashSalePrice.DeletedBy),
                new SqlParameter("@deletedat", flashSalePrice.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageFlashSalePriceMaster, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(FlashSalePriceMasterLibrary flashSalePrice)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", flashSalePrice.Id),
                new SqlParameter("@deletedby", flashSalePrice.DeletedBy),
                new SqlParameter("@deletedat", flashSalePrice.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageFlashSalePriceMaster, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<FlashSalePriceMasterLibrary>>> get(FlashSalePriceMasterLibrary flashSalePrice, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", flashSalePrice.Id),
                new SqlParameter("@sellerProductId", flashSalePrice.SellerProductId),
                new SqlParameter("@sellerWiseProductPriceMasterId", flashSalePrice.SellerWiseProductPriceMasterId),
                new SqlParameter("@collectionId", flashSalePrice.CollectionId),
                new SqlParameter("@collectionMappingId", flashSalePrice.CollectionMappingId),
                new SqlParameter("@collectionName", flashSalePrice.CollectionName),
                new SqlParameter("@isselleroptin", flashSalePrice.IsSellerOptIn),
                new SqlParameter("@isdeleted", flashSalePrice.IsDeleted),
                new SqlParameter("@status", flashSalePrice.Status),
                new SqlParameter("@searchtext", flashSalePrice.SearchText),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageFlashSalePriceMaster, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<FlashSalePriceMasterLibrary>> LayoutParserAsync(DbDataReader reader)
        {
            List<FlashSalePriceMasterLibrary> flashSalePrice = new List<FlashSalePriceMasterLibrary>();
            while (await reader.ReadAsync())
            {
                flashSalePrice.Add(new FlashSalePriceMasterLibrary()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    SellerProductId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    SellerWiseProductPriceMasterId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerWiseProductPriceMasterId"))),
                    CollectionId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CollectionId"))),
                    CollectionMappingId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CollectionMappingId"))),
                    MRP = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    IsSellerOptIn = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsSellerOptIn"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    CollectionName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CollectionName"))),
                    SizeID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeID"))),
                    SizeName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeName"))),
                });
            }
            return flashSalePrice;
        }
    }
}
