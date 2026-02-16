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
using System.Data.SqlClient;
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
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "add"),
                    new SqlParameter("@productid", sellerProduct.ProductID),
                    new SqlParameter("@sellerid", sellerProduct.SellerID),
                    new SqlParameter("@brandid", sellerProduct.BrandID),
                    new SqlParameter("@skucode", sellerProduct.SKUCode),
                    new SqlParameter("@issizewisepricevariant", sellerProduct.IsSizeWisePriceVariant),
                    new SqlParameter("@isexistingproduct", sellerProduct.IsExistingProduct),
                    new SqlParameter("@live", sellerProduct.Live),
                    new SqlParameter("@status", sellerProduct.Status),
                    new SqlParameter("@manufactureddate", sellerProduct.ManufacturedDate),
                    new SqlParameter("@expirydate", sellerProduct.ExpiryDate),
                    new SqlParameter("@extradetails", sellerProduct.ExtraDetails),
                    new SqlParameter("@moq", sellerProduct.MOQ),
                    new SqlParameter("@packinglength", sellerProduct.PackingLength),
                    new SqlParameter("@packingbreadth", sellerProduct.PackingBreadth),
                    new SqlParameter("@packingheight", sellerProduct.PackingHeight),
                    new SqlParameter("@packingweight", sellerProduct.PackingWeight),
                    new SqlParameter("@weightslabid", sellerProduct.WeightSlabId),
                    new SqlParameter("@createdby", sellerProduct.CreatedBy),
                    new SqlParameter("@createdat", sellerProduct.CreatedAt),
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
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@id", sellerProduct.Id),
                    new SqlParameter("@productid", sellerProduct.ProductID),
                    new SqlParameter("@sellerid", sellerProduct.SellerID),
                    new SqlParameter("@brandid", sellerProduct.BrandID),
                    new SqlParameter("@skucode", sellerProduct.SKUCode),
                    new SqlParameter("@issizewisepricevariant", sellerProduct.IsSizeWisePriceVariant),
                    new SqlParameter("@isexistingproduct", sellerProduct.IsExistingProduct),
                    new SqlParameter("@live", sellerProduct.Live),
                    new SqlParameter("@status", sellerProduct.Status),
                    new SqlParameter("@manufactureddate", sellerProduct.ManufacturedDate),
                    new SqlParameter("@expirydate", sellerProduct.ExpiryDate),
                    new SqlParameter("@extradetails", sellerProduct.ExtraDetails),
                    new SqlParameter("@moq", sellerProduct.MOQ),
                    new SqlParameter("@packinglength", sellerProduct.PackingLength),
                    new SqlParameter("@packingbreadth", sellerProduct.PackingBreadth),
                    new SqlParameter("@packingheight", sellerProduct.PackingHeight),
                    new SqlParameter("@packingweight", sellerProduct.PackingWeight),
                    new SqlParameter("@weightslabid", sellerProduct.WeightSlabId),
                    new SqlParameter("@modifiedBy", sellerProduct.ModifiedBy),
                    new SqlParameter("@modifiedAt", sellerProduct.ModifiedAt),
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
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id", sellerProduct.Id),
                    new SqlParameter("@deletedby", sellerProduct.DeletedBy),
                    new SqlParameter("@deletedat", sellerProduct.DeletedAt),
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
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "archived"),
                    new SqlParameter("@sellerid", sellerProduct.SellerID),
                    new SqlParameter("@modifiedby", sellerProduct.ModifiedBy),
                    new SqlParameter("@modifiedat", sellerProduct.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.SellerProducts, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<SellerProduct>>> GetSellerProduct(SellerProduct sellerProduct, int PageIndex, int PageSize, string Mode, bool? isArchive = null)
        {
            var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", sellerProduct.Id),
                new SqlParameter("@productid", sellerProduct.ProductID),
                new SqlParameter("@productMasterid", sellerProduct.ProductMasterId),
                new SqlParameter("@sellerid", sellerProduct.SellerID),
                new SqlParameter("@brandid", sellerProduct.BrandID),
                new SqlParameter("@categoryid", sellerProduct.CategoryId),
                new SqlParameter("@weightSlabId", sellerProduct.WeightSlabId),
                new SqlParameter("@skucode", sellerProduct.SKUCode),
                new SqlParameter("@companyskucode", sellerProduct.CompanySKUCode),
                new SqlParameter("@issizewisepricevariant", sellerProduct.IsSizeWisePriceVariant),
                new SqlParameter("@isexistingproduct", sellerProduct.IsExistingProduct),
                new SqlParameter("@live", sellerProduct.Live),
                new SqlParameter("@status", sellerProduct.Status),
                new SqlParameter("@fromdate", sellerProduct.FromDate),
                new SqlParameter("@todate", sellerProduct.ToDate),
                new SqlParameter("@isdeleted", sellerProduct.IsDeleted),
                new SqlParameter("@isarchive", isArchive),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@pageSize", PageSize),

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
            var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", sellerProduct.ProductId),
                new SqlParameter("@sellerProductId", sellerProduct.SellerProductId),
                new SqlParameter("@guid", sellerProduct.ProductGuid),
                new SqlParameter("@isdeleted", isDeleted),
                new SqlParameter("@status", sellerProduct.Status),
                new SqlParameter("@live", sellerProduct.LiveStatus),
                new SqlParameter("@archive", isArchive),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@pageSize", PageSize),

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
                var sqlParams = new List<SqlParameter>();
                if (ExtraDetails.Mode == "updateSeller")
                {
                    sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", ExtraDetails.Mode),
                    new SqlParameter("@FullName", ExtraDetails.FullName),
                    new SqlParameter("@UserName", ExtraDetails.UserName),
                    new SqlParameter("@PhoneNumber", ExtraDetails.PhoneNumber),
                    new SqlParameter("@SellerStatus", ExtraDetails.SellerStatus),
                    new SqlParameter("@sellerid", ExtraDetails.SellerId),
                    };
                }
                else if(ExtraDetails.Mode == "updateSellerKyc")
                {
                    sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", ExtraDetails.Mode),
                    new SqlParameter("@DisplayName", ExtraDetails.DisplayName),
                    new SqlParameter("@DigitalSign", ExtraDetails.DigitalSign),
                    new SqlParameter("@ShipmentBy", ExtraDetails.ShipmentBy),
                    new SqlParameter("@ShipmentChargesPaidBy", ExtraDetails.ShipmentChargesPaidBy),
                    new SqlParameter("@ShipmentChargesPaidByName", ExtraDetails.ShipmentChargesPaidByName),
                    new SqlParameter("@sellerid", ExtraDetails.SellerId),
                    };
                }
                else if (ExtraDetails.Mode == "updateSellerGST")
                {
                    sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", ExtraDetails.Mode),
                    new SqlParameter("@TradeName", ExtraDetails.TradeName),
                    new SqlParameter("@LegalName", ExtraDetails.LegalName),
                    new SqlParameter("@sellerid", ExtraDetails.SellerId),
                    };
                }
                else if (ExtraDetails.Mode == "updateBrands")
                {
                    sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", ExtraDetails.Mode),
                    new SqlParameter("@BrandName", ExtraDetails.BrandName),
                    new SqlParameter("@BrandStatus", ExtraDetails.BrandStatus),
                    new SqlParameter("@Logo", ExtraDetails.BrandLogo),
                    new SqlParameter("@brandid", ExtraDetails.BrandId)
                    };
                }
                else if (ExtraDetails.Mode == "updateAssignBrands")
                {
                    sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", ExtraDetails.Mode),
                    new SqlParameter("@AssignBrandStatus", ExtraDetails.AssignBrandStatus),
                    new SqlParameter("@sellerid", ExtraDetails.SellerId),
                    new SqlParameter("@brandid", ExtraDetails.BrandId)
                    };
                }

                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
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
