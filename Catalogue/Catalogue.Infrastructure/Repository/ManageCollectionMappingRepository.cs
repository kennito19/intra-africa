using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
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
    public class ManageCollectionMappingRepository : IManageCollectionMappingRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;

        public ManageCollectionMappingRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(ManageCollectionMappingLibrary collectionMapping)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "add"),
                    new MySqlParameter("@collectionId", collectionMapping.CollectionId),
                    new MySqlParameter("@productId", collectionMapping.ProductId),
                    new MySqlParameter("@status", collectionMapping.Status),
                    new MySqlParameter("@createdby", collectionMapping.CreatedBy),
                new MySqlParameter("@createdat", collectionMapping.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageCollectionMapping, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(ManageCollectionMappingLibrary collectionMapping)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", collectionMapping.Id),
                new MySqlParameter("@collectionId", collectionMapping.CollectionId),
                new MySqlParameter("@productId", collectionMapping.ProductId),
                new MySqlParameter("@status", collectionMapping.Status),
                new MySqlParameter("@modifiedby", collectionMapping.ModifiedBy),
                new MySqlParameter("@modifiedat", collectionMapping.ModifiedAt),
                new MySqlParameter("@deletedby", collectionMapping.DeletedBy),
                new MySqlParameter("@deletedat", collectionMapping.DeletedAt),
                new MySqlParameter("@isdeleted", collectionMapping.IsDeleted),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageCollectionMapping, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageCollectionMappingLibrary collectionMapping)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", collectionMapping.Id),
                new MySqlParameter("@deletedby", collectionMapping.DeletedBy),
                new MySqlParameter("@deletedat", collectionMapping.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageCollectionMapping, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ManageCollectionMappingLibrary>>> get(ManageCollectionMappingLibrary collectionMapping, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", collectionMapping.Id),
                new MySqlParameter("@collectionid", collectionMapping.CollectionId),
                new MySqlParameter("@productid", collectionMapping.ProductId),
                new MySqlParameter("@sellerid", collectionMapping.SellerId),
                new MySqlParameter("@collectionname", collectionMapping.CollectionName),
                new MySqlParameter("@productname", collectionMapping.ProductName),
                new MySqlParameter("@isdeleted", collectionMapping.IsDeleted),
                new MySqlParameter("@searchtext", collectionMapping.SearchText),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageCollectionMapping, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ManageCollectionMappingLibrary>> LayoutParserAsync(DbDataReader reader)
        {
            List<ManageCollectionMappingLibrary> collectionMapping = new List<ManageCollectionMappingLibrary>();
            while (await reader.ReadAsync())
            {
                collectionMapping.Add(new ManageCollectionMappingLibrary()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    CollectionId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CollectionId"))),
                    ProductId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductId"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    CollectionName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CollectionName"))),
                    ParentId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentId"))),
                    CategoryId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    AssiCategoryId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("AssiCategoryId"))),
                    ProductName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CustomeProductName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    CompanySKUCode = Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    Image1 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Image1")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Image1"))),
                    ExtraDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                    MRP = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Quantity = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    CategoryName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    CategoryPathIds = Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathIds"))),
                    CategoryPathNames = Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames"))),
                    SellerProductId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    SellerId = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId"))),
                    BrandId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandId"))),
                    BrandName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandName"))),
                    TotalQty = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalQty"))),
                    ProductStatus = Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductStatus"))),
                    ProductLive = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductLive")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("ProductLive"))),
                    TotalVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalVariant")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalVariant"))),
                    SaleMRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SaleMRP")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SaleMRP"))),
                    SaleSellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SaleSellingPrice")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SaleSellingPrice"))),
                    SaleDiscount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SaleDiscount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SaleDiscount"))),
                    SaleStatus = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SaleStatus")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SaleStatus"))),
                    IsSellerOptIn = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsSellerOptIn")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsSellerOptIn"))),
                });
            }
            return collectionMapping;
        }

    }
}
