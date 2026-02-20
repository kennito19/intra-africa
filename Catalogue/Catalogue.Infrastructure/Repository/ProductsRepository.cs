using Catalogue.Application.IRepositories;
using Catalogue.Domain;
using Catalogue.Domain.DTO;
using Catalogue.Domain.Entity;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Catalogue.Infrastructure.Repository
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;

        public ProductsRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(Products products)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@ismasterproduct", products.IsMasterProduct),
                new MySqlParameter("@parentid", products.ParentId),
                new MySqlParameter("@categoryid", products.CategoryId),
                new MySqlParameter("@assicategoryid", products.AssiCategoryId),
                new MySqlParameter("@taxvalueid", products.TaxValueId),
                new MySqlParameter("@hsncodeid", products.HSNCodeId),
                new MySqlParameter("@productname", products.ProductName),
                new MySqlParameter("@customeproductname", products.CustomeProductName),
                new MySqlParameter("@companyskucode", products.CompanySKUCode),
                new MySqlParameter("@description", products.Description),
                new MySqlParameter("@highlights", products.Highlights),
                new MySqlParameter("@metadescription", products.MetaDescription),
                new MySqlParameter("@metatitle", products.MetaTitle),
                new MySqlParameter("@keywords", products.Keywords),
                new MySqlParameter("@productlength", products.ProductLength),
                new MySqlParameter("@productbreadth", products.ProductBreadth),
                new MySqlParameter("@productweight", products.ProductWeight),
                new MySqlParameter("@productheight", products.ProductHeight),
                new MySqlParameter("@createdby", products.CreatedBy),
                new MySqlParameter("@createdat", products.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Products, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(Products products)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", products.Id),
                new MySqlParameter("@taxvalueid", products.TaxValueId),
                new MySqlParameter("@hsncodeid", products.HSNCodeId),
                new MySqlParameter("@productname", products.ProductName),
                new MySqlParameter("@customeproductname", products.CustomeProductName),
                new MySqlParameter("@companyskucode", products.CompanySKUCode),
                new MySqlParameter("@description", products.Description),
                new MySqlParameter("@highlights", products.Highlights),
                new MySqlParameter("@metadescription", products.MetaDescription),
                new MySqlParameter("@metatitle", products.MetaTitle),
                new MySqlParameter("@keywords", products.Keywords),
                new MySqlParameter("@productlength", products.ProductLength),
                new MySqlParameter("@productbreadth", products.ProductBreadth),
                new MySqlParameter("@productweight", products.ProductWeight),
                new MySqlParameter("@productheight", products.ProductHeight),
                new MySqlParameter("@modifiedby", products.ModifiedBy),
                new MySqlParameter("@modifiedat", products.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Products, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Delete(Products products)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", products.Id),
                new MySqlParameter("@deletedby", products.DeletedBy),
                new MySqlParameter("@deletedat", products.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Products, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<Products>>> get(Products products, bool Getparent = false, bool Getchild = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", products.Id),
                new MySqlParameter("@guid", products.Guid),
                new MySqlParameter("@ismasterproduct", products.IsMasterProduct),
                new MySqlParameter("@parentid", products.ParentId),
                new MySqlParameter("@categoryid", products.CategoryId),
                new MySqlParameter("@pathIds", products.CategoryPathIds),
                new MySqlParameter("@assicategoryid", products.AssiCategoryId),
                new MySqlParameter("@companyskucode", products.CompanySKUCode),
                new MySqlParameter("@hsncode", products.HSNCode),
                new MySqlParameter("@getparent", Getparent),
                new MySqlParameter("@getchild", Getchild),
                new MySqlParameter("@isdeleted", products.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetProducts, productsParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<Products>> productsParserAsync(DbDataReader reader)
        {
            List<Products> lstproducts = new List<Products>();
            while (await reader.ReadAsync())
            {
                lstproducts.Add(new Products()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Guid = Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    IsMasterProduct = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsMasterProduct"))),
                    ParentId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentId"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    AssiCategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AssiCategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("AssiCategoryId"))),
                    TaxValueId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxValueId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TaxValueId"))),
                    HSNCodeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCodeId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("HSNCodeId"))),
                    ProductName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CustomeProductName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    CompanySKUCode = Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    Description = Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),
                    Highlights = Convert.ToString(reader.GetValue(reader.GetOrdinal("Highlights"))),
                    MetaDescription = Convert.ToString(reader.GetValue(reader.GetOrdinal("MetaDescription"))),
                    MetaTitle = Convert.ToString(reader.GetValue(reader.GetOrdinal("MetaTitle"))),
                    Keywords = Convert.ToString(reader.GetValue(reader.GetOrdinal("Keywords"))),
                    ProductLength = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductLength")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ProductLength"))),
                    ProductBreadth = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductBreadth")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ProductBreadth"))),
                    ProductWeight = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductWeight")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ProductWeight"))),
                    ProductHeight = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductHeight")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ProductHeight"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    CategoryName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    CategoryPathIds = Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathIds"))),
                    CategoryPathNames = Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames"))),
                    HSNCode = Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode"))),
                    //TaxName = Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxName"))),
                    TaxValue = Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxValue"))),
                });
            }
            return lstproducts;
        }


        public async Task<BaseResponse<List<UserProductDetails>>> getUserProductDetails(string ProductGuid)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@productGuid", ProductGuid),
                new MySqlParameter("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetUserProductDetails, UserproductsParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<UserProductDetails>> UserproductsParserAsync(DbDataReader reader)
        {
            List<UserProductDetails> lstproducts = new List<UserProductDetails>();
            while (await reader.ReadAsync())
            {
                lstproducts.Add(new UserProductDetails()
                {
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductId"))),
                    Guid = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    MasterProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MasterProductId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("MasterProductId"))),
                    SellerProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    PriceMasterId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PriceMasterId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PriceMasterId"))),
                    SellerID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    BrandID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandID"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    AssiCategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AssiCategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("AssiCategoryId"))),
                    HSNCodeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCodeId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("HSNCodeId"))),
                    TaxValueId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxValueId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TaxValueId"))),
                    IsSizeWisePriceVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsSizeWisePriceVariant")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsSizeWisePriceVariant"))),
                    IsExistingProduct = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsExistingProduct")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsExistingProduct"))),
                    ProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CustomeProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    CompanySKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    SKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SKUCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SKUCode"))),
                    Description = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Description")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),
                    Highlights = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Highlights")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Highlights"))),
                    Live = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Live")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("Live"))),
                    Status = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Status")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    MOQ = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MOQ")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("MOQ"))),
                    ManufacturedDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ManufacturedDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ManufacturedDate"))),
                    ExpiryDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExpiryDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ExpiryDate"))),
                    ExtraDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                    MRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    MarginIn = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginIn")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginIn"))),
                    MarginCost = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginCost")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MarginCost"))),
                    MarginPercentage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginPercentage")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MarginPercentage"))),
                    Quantity = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Quantity")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    SizeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeID"))),
                    SizeName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeName"))),
                    SizeTypeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeTypeId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeTypeId"))),
                    SizeTypeName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TypeName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TypeName"))),
                    CategoryName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    CategoryPathIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathIds"))),
                    CategoryPathNames = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames"))),
                    MetaDescription = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MetaDescription")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MetaDescription"))),
                    MetaTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MetaTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MetaTitle"))),
                });
            }
            return lstproducts;
        }


        public async Task<BaseResponse<List<AddInExistingProductList>>> getAddInExistingProductList(AddInExistingProductList addInExisting, int PageIndex, int PageSize)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@sellerId", addInExisting.SellerID),
                new MySqlParameter("@categoryId", addInExisting.CategoryId),
                new MySqlParameter("@brandId", addInExisting.BrandID),
                new MySqlParameter("@companySKUCode", addInExisting.CompanySKUCode),
                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@PageSize", PageSize),
                new MySqlParameter("@searchtext", addInExisting.SearchText),

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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetAddInExistingList, AddInExistingproductsParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<AddInExistingProductList>> AddInExistingproductsParserAsync(DbDataReader reader)
        {
            List<AddInExistingProductList> lstproducts = new List<AddInExistingProductList>();
            while (await reader.ReadAsync())
            {
                lstproducts.Add(new AddInExistingProductList()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Id")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    ProductID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    ProductMasterID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductMasterID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductMasterID"))),
                    ProductGuid = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGuid")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGuid"))),
                    SellerProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    SellerID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    BrandID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandID"))),
                    AssiCategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AssiCategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("AssiCategoryId"))),
                    TaxValueId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxValueId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TaxValueId"))),
                    HSNCodeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCodeId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("HSNCodeId"))),
                    ProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CustomeProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    CompanySKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    SellerSKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSKUCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSKUCode"))),
                    CategoryPathids = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathids")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathids"))),
                    CategoryPathName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathName"))),
                    CategoryName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    IsExistingProduct = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsExistingProduct")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsExistingProduct"))),
                    IsSizeWisePriceVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsSizeWisePriceVariant")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsSizeWisePriceVariant"))),
                    ExtraDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                    ProductImage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImage")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImage"))),
                    Status = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Status")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    Live = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Live")))) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("Live"))),
                    BrandName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandName"))),
                    SizeType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeType")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeType"))),
                    Size = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Size")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Size"))),
                    Color = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Color")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Color"))),
                    ProductVariants = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductVariants")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductVariants"))),
                });
            }
            return lstproducts;
        }

        public async Task<BaseResponse<List<ProductCompare>>> getProductCompare(string SellerProductId)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@SellerProductId", SellerProductId),


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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetProductCompare, productCompare, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ProductCompare>> productCompare(DbDataReader reader)
        {
            List<ProductCompare> lstproducts = new List<ProductCompare>();
            while (await reader.ReadAsync())
            {
                lstproducts.Add(new ProductCompare()
                {
                    ProductGuid = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    ProductID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Id")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Id"))),
                    ProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    Name = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Name")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    Description = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Description")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),
                    Highlights = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Highlights")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Highlights"))),
                    SellerProductID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductID"))),
                    SellerID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    BrandID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandID"))),
                    MRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount"))),
                    ProductImage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImage")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImage"))),
                    ProductSpec = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSpec")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSpec"))),
                    Color = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Color")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Color"))),
                    ProductSize = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSize")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSize"))),
                    ProductVarint = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductVarint")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductVarint")))

                });
            }
            return lstproducts;
        }

        public async Task<BaseResponse<List<ProductCompareBrand>>> getProductCompareBrand(int CategoryId)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@mode", "getBrands"),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetBrandsAndProductForProductCompare, BrandsForProductCompare, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<ProductCompareBrand>> BrandsForProductCompare(DbDataReader reader)
        {
            List<ProductCompareBrand> lstbrands = new List<ProductCompareBrand>();
            while (await reader.ReadAsync())
            {
                lstbrands.Add(new ProductCompareBrand()
                {
                    BrandName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandName"))),
                    BrandId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandId")))

                });
            }
            return lstbrands;
        }

        public async Task<BaseResponse<List<ProductCompareBrandProduct>>> getProductCompareBrandProduct(int CategoryId, int BrandId)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@mode", "getProducts"),
                new MySqlParameter("@CategoryId", CategoryId),
                new MySqlParameter("@BrandId", BrandId),


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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetBrandsAndProductForProductCompare, ProductForProductCompare, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<ProductCompareBrandProduct>> ProductForProductCompare(DbDataReader reader)
        {
            List<ProductCompareBrandProduct> lstproducts = new List<ProductCompareBrandProduct>();
            while (await reader.ReadAsync())
            {
                lstproducts.Add(new ProductCompareBrandProduct()
                {
                    ProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    SellerProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    ProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductId")))

                });
            }
            return lstproducts;
        }

        public async Task<BaseResponse<List<ProductBulkDetails>>> getProductBulkDetails(ProductBulkDetailsParams detailsParams)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@categoryid", detailsParams.CategoryId),
                new MySqlParameter("@brandId", detailsParams.BrandId),


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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetProductBulkDetails, ProductBulkDetailsAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<List<ProductBulkDetails>> ProductBulkDetailsAsync(DbDataReader reader)
        {
            List<ProductBulkDetails> lstDetails = new List<ProductBulkDetails>();
            while (await reader.ReadAsync())
            {
                lstDetails.Add(new ProductBulkDetails()
                {
                    Category = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Category")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Category"))),
                    AssignSpectoCat = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignSpectoCat")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignSpectoCat"))),
                    AssignSizeValtoCat = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignSizeValtoCat")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignSizeValtoCat"))),
                    AssignSpecValtoCat = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignSpecValtoCat")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignSpecValtoCat"))),
                    AssignTaxRateToHSNCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignTaxRateToHSNCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AssignTaxRateToHSNCode"))),
                    WeightSlab = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlab")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlab"))),
                    Color = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Color")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Color"))),
                    SellerProduct = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProduct")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProduct"))),

                });
            }
            return lstDetails;
        }

        public async Task<BaseResponse<List<ProductBulkDownload>>> getProductBulkDownload(int CategoryId, int BrandId, string SellerId)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@CategoryId", CategoryId),
                new MySqlParameter("@BrandID", BrandId),
                new MySqlParameter("@SellerID", SellerId),


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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetProductListForBulkDownload, ProductBulkDownloadAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ProductBulkDownload>> ProductBulkDownloadAsync(DbDataReader reader)
        {
            List<ProductBulkDownload> lstDetails = new List<ProductBulkDownload>();
            while (await reader.ReadAsync())
            {
                lstDetails.Add(new ProductBulkDownload()
                {
 
                    flag = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("flag")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("flag"))),
                    ProductID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    SellerProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    SellerSKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSKUCode")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSKUCode"))),
                    PackingLength = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingLength")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingLength"))),
                    PackingBreadth = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingBreadth")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingBreadth"))),
                    PackingHeight = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingHeight")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingHeight"))),
                    PackingWeight = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingWeight")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingWeight"))),
                    WeightSlabId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlabId")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlabId"))),
                    HSNCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode"))),
                    CustomeProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    CompanySKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    Description = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Description")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),
                    Highlights = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Highlights")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("Highlights"))),
                    Keywords = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Keywords")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("Keywords"))),
                    ColorName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ColorName")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("ColorName"))),
                    MRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Quantity = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Quantity")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    WarehouseProductQty = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WarehouseProductQty")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("WarehouseProductQty"))),
                    WarehouseId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WarehouseId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WarehouseId"))),
                    SizeName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeName")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeName"))),
                    Image = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Image")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("Image"))),
                    SpecID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SpecID"))),
                    SpecName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecName")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecName"))),
                    SpecTypeName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecTypeName")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecTypeName"))),
                    SpecValueName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecValueName")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecValueName"))),
                    Value = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Value")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("Value"))),

                });
            }
            return lstDetails;
        }

        public async Task<BaseResponse<List<ProductBulkDownloadForStock>>> getProductBulkDownloadForStock(string? SellerId)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@SellerID", SellerId),


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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetProductListForBulkDownloadForStock, ProductBulkDownloadForStockAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ProductBulkDownloadForStock>> ProductBulkDownloadForStockAsync(DbDataReader reader)
        {
            List<ProductBulkDownloadForStock> lstDetails = new List<ProductBulkDownloadForStock>();
            while (await reader.ReadAsync())
            {
                lstDetails.Add(new ProductBulkDownloadForStock()
                {
                    flag = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("flag")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("flag"))),
                    ProductID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    SellerProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    SellerSKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSKUCode")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSKUCode"))),
                    CustomeProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    CompanySKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    MRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Quantity = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Quantity")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    WarehouseProductQty = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WarehouseProductQty")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("WarehouseProductQty"))),
                    WarehouseId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WarehouseId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WarehouseId"))),
                    SizeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeId")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeId"))),
                    SizeName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeName")))) ? "" : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeName"))),

                });
            }
            return lstDetails;
        }
    }
}
