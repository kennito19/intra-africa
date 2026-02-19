using Catalogue.Application.IRepositories;
using Catalogue.Domain;
using Catalogue.Domain.DTO;
using Catalogue.Domain.Entity;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class SellerProductRepository:ISellerProductRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public SellerProductRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> AddSellerProduct(SellerProduct sellerProduct)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "add"),
                    new MySqlParameter("@productid", sellerProduct.ProductID),
                    new MySqlParameter("@sellerid", sellerProduct.SellerID),
                    new MySqlParameter("@brandid", sellerProduct.BrandID),
                    new MySqlParameter("@skucode", sellerProduct.SKUCode),
                    new MySqlParameter("@issizewisepricevariant", sellerProduct.IsSizeWisePriceVariant),
                    new MySqlParameter("@isexistingproduct", sellerProduct.IsExistingProduct),
                    new MySqlParameter("@live", sellerProduct.Live),
                    new MySqlParameter("@status", sellerProduct.Status),
                    new MySqlParameter("@manufactureddate", sellerProduct.ManufacturedDate),
                    new MySqlParameter("@expirydate", sellerProduct.ExpiryDate),
                    new MySqlParameter("@extradetails", sellerProduct.ExtraDetails),
                    new MySqlParameter("@moq", sellerProduct.MOQ),
                    new MySqlParameter("@packinglength", sellerProduct.PackingLength),
                    new MySqlParameter("@packingbreadth", sellerProduct.PackingBreadth),
                    new MySqlParameter("@packingheight", sellerProduct.PackingHeight),
                    new MySqlParameter("@packingweight", sellerProduct.PackingWeight),
                    new MySqlParameter("@weightslabid", sellerProduct.WeightSlabId),
                    new MySqlParameter("@createdby", sellerProduct.CreatedBy),
                    new MySqlParameter("@createdat", sellerProduct.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.SellerProducts, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> UpdateSellerProduct(SellerProduct sellerProduct)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id", sellerProduct.Id),
                    new MySqlParameter("@productid", sellerProduct.ProductID),
                    new MySqlParameter("@sellerid", sellerProduct.SellerID),
                    new MySqlParameter("@brandid", sellerProduct.BrandID),
                    new MySqlParameter("@skucode", sellerProduct.SKUCode),
                    new MySqlParameter("@issizewisepricevariant", sellerProduct.IsSizeWisePriceVariant),
                    new MySqlParameter("@isexistingproduct", sellerProduct.IsExistingProduct),
                    new MySqlParameter("@live", sellerProduct.Live),
                    new MySqlParameter("@status", sellerProduct.Status),
                    new MySqlParameter("@manufactureddate", sellerProduct.ManufacturedDate),
                    new MySqlParameter("@expirydate", sellerProduct.ExpiryDate),
                    new MySqlParameter("@extradetails", sellerProduct.ExtraDetails),
                    new MySqlParameter("@moq", sellerProduct.MOQ),
                    new MySqlParameter("@packinglength", sellerProduct.PackingLength),
                    new MySqlParameter("@packingbreadth", sellerProduct.PackingBreadth),
                    new MySqlParameter("@packingheight", sellerProduct.PackingHeight),
                    new MySqlParameter("@packingweight", sellerProduct.PackingWeight),
                    new MySqlParameter("@weightslabid", sellerProduct.WeightSlabId),
                    new MySqlParameter("@modifiedBy", sellerProduct.ModifiedBy),
                    new MySqlParameter("@modifiedAt", sellerProduct.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.SellerProducts, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> DeleteSellerProduct(SellerProduct sellerProduct)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", sellerProduct.Id),
                    new MySqlParameter("@deletedby", sellerProduct.DeletedBy),
                    new MySqlParameter("@deletedat", sellerProduct.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.SellerProducts, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> ArchivedSellerProduct(SellerProduct sellerProduct)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "archived"),
                    new MySqlParameter("@sellerid", sellerProduct.SellerID),
                    new MySqlParameter("@modifiedby", sellerProduct.ModifiedBy),
                    new MySqlParameter("@modifiedat", sellerProduct.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.SellerProducts, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<SellerProduct>>> GetSellerProduct(SellerProduct sellerProduct, int PageIndex, int PageSize, string Mode, bool? isArchive = null)
        {
            var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", sellerProduct.Id),
                new MySqlParameter("@productid", sellerProduct.ProductID),
                new MySqlParameter("@productMasterid", sellerProduct.ProductMasterId),
                new MySqlParameter("@sellerid", sellerProduct.SellerID),
                new MySqlParameter("@brandid", sellerProduct.BrandID),
                new MySqlParameter("@categoryid", sellerProduct.CategoryId),
                new MySqlParameter("@weightSlabId", sellerProduct.WeightSlabId),
                new MySqlParameter("@skucode", sellerProduct.SKUCode),
                new MySqlParameter("@companyskucode", sellerProduct.CompanySKUCode),
                new MySqlParameter("@issizewisepricevariant", sellerProduct.IsSizeWisePriceVariant),
                new MySqlParameter("@isexistingproduct", sellerProduct.IsExistingProduct),
                new MySqlParameter("@live", sellerProduct.Live),
                new MySqlParameter("@status", sellerProduct.Status),
                new MySqlParameter("@fromdate", sellerProduct.FromDate),
                new MySqlParameter("@todate", sellerProduct.ToDate),
                new MySqlParameter("@isdeleted", sellerProduct.IsDeleted),
                new MySqlParameter("@isarchive", isArchive),
                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@pageSize", PageSize),

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

            return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetSellerProducts, SellerProductParserAsync, output, newid: null, message, sqlParams.ToArray());
        }
        private async Task<List<SellerProduct>> SellerProductParserAsync(DbDataReader reader)
        {
            List<SellerProduct> lstSellerProduct = new List<SellerProduct>();
            while (await reader.ReadAsync())
            {
                lstSellerProduct.Add(new SellerProduct()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    ProductID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    ProductMasterId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductMasterId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductMasterId"))),
                    SellerID = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    BrandID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandID"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    SKUCode = Convert.ToString(reader.GetValue(reader.GetOrdinal("SKUCode"))),
                    CompanySKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    IsSizeWisePriceVariant = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsSizeWisePriceVariant"))),
                    IsExistingProduct = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsExistingProduct"))),
                    Live = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("Live"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    ManufacturedDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ManufacturedDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ManufacturedDate"))),
                    ExpiryDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExpiryDate")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ExpiryDate"))),
                    ExtraDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                    PackingLength = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingLength")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PackingLength"))),
                    PackingBreadth = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingBreadth")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PackingBreadth"))),
                    PackingHeight = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingHeight")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PackingHeight"))),
                    PackingWeight = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingWeight")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PackingWeight"))),
                    WeightSlabId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlabId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WeightSlabId"))),
                    MOQ = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MOQ")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("MOQ"))),
                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    ProductName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    WeightSlabName = Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlab"))),
                });
            }
            return lstSellerProduct;
        }

        public async Task<BaseResponse<List<SellerProductDetails>>> GetSellerProductDetails(SellerProductDetails sellerProduct, int PageIndex, int PageSize, string Mode, bool? isDeleted = null, bool? isArchive = null)
        {
            var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", sellerProduct.ProductId),
                new MySqlParameter("@sellerProductId", sellerProduct.SellerProductId),
                new MySqlParameter("@guid", sellerProduct.ProductGuid),
                new MySqlParameter("@isdeleted", isDeleted),
                new MySqlParameter("@status", sellerProduct.Status),
                new MySqlParameter("@live", sellerProduct.LiveStatus),
                new MySqlParameter("@archive", isArchive),
                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@pageSize", PageSize),

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

            return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetSellerProductDetails, SellerProductDetailsParserAsync, output, newid: null, message, sqlParams.ToArray());
        }
        private async Task<List<SellerProductDetails>> SellerProductDetailsParserAsync(DbDataReader reader)
        {
            List<SellerProductDetails> lstSellerProduct = new List<SellerProductDetails>();
            while (await reader.ReadAsync())
            {
                lstSellerProduct.Add(new SellerProductDetails()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    ProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductId"))),
                    ProductGuid = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGuid")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGuid"))),
                    ProductMasterId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductMasterId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductMasterId"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    SellerProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    SellerId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId"))),
                    BrandId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandId"))),
                    PricemasterId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PricemasterId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PricemasterId"))),
                    ProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CustomeProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    ProductSkuCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSkuCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSkuCode"))),
                    SellerSkuCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSkuCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSkuCode"))),
                    IsSizeWisePriceVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsSizeWisePriceVariant")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsSizeWisePriceVariant"))),
                    PackingLength = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingLength")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PackingLength"))),
                    PackingBreadth = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingBreadth")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PackingBreadth"))),
                    PackingHeight = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingHeight")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PackingHeight"))),
                    PackingWeight = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingWeight")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("PackingWeight"))),
                    MOQ = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MOQ")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("MOQ"))),
                    AssiCategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AssiCategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("AssiCategoryId"))),
                    WeightSlabId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlabId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WeightSlabId"))),
                    WeightSlab = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlab")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlab"))),
                    LocalCharges = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LocalCharges")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("LocalCharges"))),
                    ZonalCharges = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ZonalCharges")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ZonalCharges"))),
                    NationalCharges = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("NationalCharges")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("NationalCharges"))),
                    HSNCodeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCodeId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("HSNCodeId"))),
                    HSNCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode"))),
                    TaxValueId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxValueId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TaxValueId"))),
                    TaxTypeID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxTypeID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TaxTypeID"))),
                    TaxValue = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxValue")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxValue"))),
                    SizeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeId"))),
                    SizeName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeName"))),
                    MRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Quantity = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Quantity")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    MarginIn = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginIn")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginIn"))),
                    MarginCost = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginCost")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MarginCost"))),
                    MarginPercentage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginPercentage")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MarginPercentage"))),
                    ProductImage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImage")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImage"))),
                    ExtraDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                    Status = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Status")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    LiveStatus = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LiveStatus")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("LiveStatus"))),
                });
            }
            return lstSellerProduct;
        }

        public async Task<BaseResponse<long>> UpdateProductExtraDetails(ProductExtraDetailsDto ExtraDetails)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>();
                if (ExtraDetails.Mode == "updateSeller")
                {
                    sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", ExtraDetails.Mode),
                    new MySqlParameter("@FullName", ExtraDetails.FullName),
                    new MySqlParameter("@UserName", ExtraDetails.UserName),
                    new MySqlParameter("@PhoneNumber", ExtraDetails.PhoneNumber),
                    new MySqlParameter("@SellerStatus", ExtraDetails.SellerStatus),
                    new MySqlParameter("@sellerid", ExtraDetails.SellerId),
                    };
                }
                else if(ExtraDetails.Mode == "updateSellerKyc")
                {
                    sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", ExtraDetails.Mode),
                    new MySqlParameter("@DisplayName", ExtraDetails.DisplayName),
                    new MySqlParameter("@DigitalSign", ExtraDetails.DigitalSign),
                    new MySqlParameter("@ShipmentBy", ExtraDetails.ShipmentBy),
                    new MySqlParameter("@ShipmentChargesPaidBy", ExtraDetails.ShipmentChargesPaidBy),
                    new MySqlParameter("@ShipmentChargesPaidByName", ExtraDetails.ShipmentChargesPaidByName),
                    new MySqlParameter("@sellerid", ExtraDetails.SellerId),
                    };
                }
                else if (ExtraDetails.Mode == "updateSellerGST")
                {
                    sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", ExtraDetails.Mode),
                    new MySqlParameter("@TradeName", ExtraDetails.TradeName),
                    new MySqlParameter("@LegalName", ExtraDetails.LegalName),
                    new MySqlParameter("@sellerid", ExtraDetails.SellerId),
                    };
                }
                else if (ExtraDetails.Mode == "updateBrands")
                {
                    sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", ExtraDetails.Mode),
                    new MySqlParameter("@BrandName", ExtraDetails.BrandName),
                    new MySqlParameter("@BrandStatus", ExtraDetails.BrandStatus),
                    new MySqlParameter("@Logo", ExtraDetails.BrandLogo),
                    new MySqlParameter("@brandid", ExtraDetails.BrandId)
                    };
                }
                else if (ExtraDetails.Mode == "updateAssignBrands")
                {
                    sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", ExtraDetails.Mode),
                    new MySqlParameter("@AssignBrandStatus", ExtraDetails.AssignBrandStatus),
                    new MySqlParameter("@sellerid", ExtraDetails.SellerId),
                    new MySqlParameter("@brandid", ExtraDetails.BrandId)
                    };
                }

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.SellerProducts, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
