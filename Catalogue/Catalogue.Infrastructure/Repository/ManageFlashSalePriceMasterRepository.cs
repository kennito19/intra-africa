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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@sellerProductId", flashSalePrice.SellerProductId),
                new MySqlParameter("@sellerWiseProductPriceMasterId", flashSalePrice.SellerWiseProductPriceMasterId),
                new MySqlParameter("@collectionId", flashSalePrice.CollectionId),
                new MySqlParameter("@collectionMappingId", flashSalePrice.CollectionMappingId),
                new MySqlParameter("@mrp", flashSalePrice.MRP),
                new MySqlParameter("@sellingprice", flashSalePrice.SellingPrice),
                new MySqlParameter("@discount", flashSalePrice.Discount),
                new MySqlParameter("@status", flashSalePrice.Status),
                new MySqlParameter("@isselleroptin", flashSalePrice.IsSellerOptIn),
                new MySqlParameter("@createdby", flashSalePrice.CreatedBy),
                new MySqlParameter("@createdat", flashSalePrice.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", flashSalePrice.Id),
                new MySqlParameter("@sellerProductId", flashSalePrice.SellerProductId),
                new MySqlParameter("@sellerWiseProductPriceMasterId", flashSalePrice.SellerWiseProductPriceMasterId),
                new MySqlParameter("@collectionId", flashSalePrice.CollectionId),
                new MySqlParameter("@collectionMappingId", flashSalePrice.CollectionMappingId),
                new MySqlParameter("@mrp", flashSalePrice.MRP),
                new MySqlParameter("@sellingprice", flashSalePrice.SellingPrice),
                new MySqlParameter("@discount", flashSalePrice.Discount),
                new MySqlParameter("@status", flashSalePrice.Status),
                new MySqlParameter("@isselleroptin", flashSalePrice.IsSellerOptIn),
                new MySqlParameter("@modifiedby", flashSalePrice.ModifiedBy),
                new MySqlParameter("@modifiedat", flashSalePrice.ModifiedAt),
                new MySqlParameter("@isDeleted", flashSalePrice.IsDeleted),
                new MySqlParameter("@deletedby", flashSalePrice.DeletedBy),
                new MySqlParameter("@deletedat", flashSalePrice.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", flashSalePrice.Id),
                new MySqlParameter("@deletedby", flashSalePrice.DeletedBy),
                new MySqlParameter("@deletedat", flashSalePrice.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", flashSalePrice.Id),
                new MySqlParameter("@sellerProductId", flashSalePrice.SellerProductId),
                new MySqlParameter("@sellerWiseProductPriceMasterId", flashSalePrice.SellerWiseProductPriceMasterId),
                new MySqlParameter("@collectionId", flashSalePrice.CollectionId),
                new MySqlParameter("@collectionMappingId", flashSalePrice.CollectionMappingId),
                new MySqlParameter("@collectionName", flashSalePrice.CollectionName),
                new MySqlParameter("@isselleroptin", flashSalePrice.IsSellerOptIn),
                new MySqlParameter("@isdeleted", flashSalePrice.IsDeleted),
                new MySqlParameter("@status", flashSalePrice.Status),
                new MySqlParameter("@searchtext", flashSalePrice.SearchText),
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
