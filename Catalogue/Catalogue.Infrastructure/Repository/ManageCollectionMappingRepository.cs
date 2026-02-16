using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
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
    public class ManageCollectionMappingRepository : IManageCollectionMappingRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public ManageCollectionMappingRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(ManageCollectionMappingLibrary collectionMapping)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "add"),
                    new SqlParameter("@collectionId", collectionMapping.CollectionId),
                    new SqlParameter("@productId", collectionMapping.ProductId),
                    new SqlParameter("@status", collectionMapping.Status),
                    new SqlParameter("@createdby", collectionMapping.CreatedBy),
                new SqlParameter("@createdat", collectionMapping.CreatedAt),
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", collectionMapping.Id),
                new SqlParameter("@collectionId", collectionMapping.CollectionId),
                new SqlParameter("@productId", collectionMapping.ProductId),
                new SqlParameter("@status", collectionMapping.Status),
                new SqlParameter("@modifiedby", collectionMapping.ModifiedBy),
                new SqlParameter("@modifiedat", collectionMapping.ModifiedAt),
                new SqlParameter("@deletedby", collectionMapping.DeletedBy),
                new SqlParameter("@deletedat", collectionMapping.DeletedAt),
                new SqlParameter("@isdeleted", collectionMapping.IsDeleted),
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", collectionMapping.Id),
                new SqlParameter("@deletedby", collectionMapping.DeletedBy),
                new SqlParameter("@deletedat", collectionMapping.DeletedAt),
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", collectionMapping.Id),
                new SqlParameter("@collectionid", collectionMapping.CollectionId),
                new SqlParameter("@productid", collectionMapping.ProductId),
                new SqlParameter("@sellerid", collectionMapping.SellerId),
                new SqlParameter("@collectionname", collectionMapping.CollectionName),
                new SqlParameter("@productname", collectionMapping.ProductName),
                new SqlParameter("@isdeleted", collectionMapping.IsDeleted),
                new SqlParameter("@searchtext", collectionMapping.SearchText),
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
