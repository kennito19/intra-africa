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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class GetCartListRepository : IGetCartListRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;

        public GetCartListRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public  async Task<BaseResponse<List<GetCheckOutDetailsList>>> GetCartList(getChekoutCalculation cart)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@cartJson", cart.CartJson),
                 new MySqlParameter("@date", DateTime.Now.ToString()),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetCartDetails, GetCartParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<GetCheckOutDetailsList>> GetCartParserAsync(DbDataReader reader)
        {
            List<GetCheckOutDetailsList> lstCart = new List<GetCheckOutDetailsList>();
            while (await reader.ReadAsync())
            {
                lstCart.Add(new GetCheckOutDetailsList()
                {
                    RowNumber = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("RowNumber")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PageCount")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("RecordCount")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Id")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    UserId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId"))),
                    SessionId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SessionId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SessionId"))),
                    SellerProductMasterId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductMasterId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductMasterId"))),
                    SizeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeId"))),
                    Quantity = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Quantity")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    WarrantyId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WarrantyId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WarrantyId"))),
                    TempMRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TempMRP")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TempMRP"))),
                    TempSellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TempSellingPrice")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TempSellingPrice"))),
                    TempDiscount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TempDiscount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TempDiscount"))),
                    SubTotal = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTotal")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SubTotal"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    Type = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Type")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Type"))),
                    Fullname = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Fullname")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Fullname"))),
                    Username = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Username")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Username"))),
                    Phone = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Phone")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Phone"))),
                    UserStatus = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UserStatus")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserStatus"))),
                    ProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductId"))),
                    ProductGuid = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGuid")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGuid"))),
                    SellerProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    ProductPricemasterId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductPricemasterId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductPricemasterId"))),
                    ProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CustomeProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    ProductSkuCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSkuCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductSkuCode"))),
                    SellerSkuCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSkuCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSkuCode"))),
                    MRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    MarginIn = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginIn")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginIn"))),
                    MarginCost = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginCost")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MarginCost"))),
                    MarginPercentage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginPercentage")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MarginPercentage"))),
                    ItemMRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemMRP")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ItemMRP"))),
                    ItemSellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemSellingPrice")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ItemSellingPrice"))),
                    ItemDiscount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ItemDiscount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ItemDiscount"))),
                    TotalQty = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalQty")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalQty"))),
                    MOQ = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MOQ")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("MOQ"))),
                    Image = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Image")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Image"))),
                    Color = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Color")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Color"))),
                    Categoryid = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Categoryid")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Categoryid"))),
                    HSNCodeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCodeId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("HSNCodeId"))),
                    HSNCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode"))),
                    TaxValueId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxValueId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TaxValueId"))),
                    TaxRate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxRate")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxRate"))),
                    SellerId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId"))),
                    BrandId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandId"))),
                    WeightSlabId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlabId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WeightSlabId"))),
                    WeightSlab = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlab")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlab"))),
                    Size = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Size")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Size"))),
                    ProductStatus = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductStatus")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductStatus"))),
                    LiveStatus = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("LiveStatus")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("LiveStatus"))),
                    FlashSaleId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("FlashSaleId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("FlashSaleId"))),
                    ExtraDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                    TierPriceList = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TierPriceList")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TierPriceList"))),
                    ExtendedWarrantyList = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtendedWarrantyList")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtendedWarrantyList"))),
                    ParentCategoryList = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentCategoryList")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentCategoryList"))),
                   
                });
            }
            return lstCart;
        }
    }
}
