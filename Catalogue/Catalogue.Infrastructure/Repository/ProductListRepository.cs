using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class ProductListRepository : IProductListRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;

        public ProductListRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<List<ProductListLibrary>>> get(ProductListLibrary productList, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@sellerid", productList.SellerId),
                new MySqlParameter("@brandid", productList.BrandId),
                new MySqlParameter("@categoryid", productList.CategoryId),
                new MySqlParameter("@status", productList.Status),
                new MySqlParameter("@live", productList.Live),
                new MySqlParameter("@isdeleted", productList.IsDeleted),
                new MySqlParameter("@searchtext", productList.SearchText),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetMasterProductListDetail, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ProductListLibrary>> LayoutParserAsync(DbDataReader reader)
        {
            List<ProductListLibrary> lstLayouts = new List<ProductListLibrary>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new ProductListLibrary()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Guid = Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    IsMasterProduct = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsMasterProduct"))),
                    ParentId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentId"))),
                    CategoryId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    AssiCategoryId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("AssiCategoryId"))),
                    ProductName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CustomeProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    CompanySKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    Image1 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Image1")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Image1"))),
                    MRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Quantity = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    CategoryName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    CategoryPathIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathIds"))),
                    CategoryPathNames = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames"))),
                    SellerProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    SellerId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId"))),
                    IsExistingProduct = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsExistingProduct")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsExistingProduct"))),
                    BrandId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandId"))),
                    BrandName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandName"))),
                    TotalQTY = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalQTY"))),
                    Status = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Status")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    Live = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Live")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("Live"))),
                    TotalVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalVariant")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalVariant"))),
                    ExtraDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                    IsAllowVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsAllowVariant")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowVariant"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    SizeVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeVariant")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeVariant"))),
                    ColorVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ColorVariant")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ColorVariant"))),
                    SpecificationVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificationVariant")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificationVariant"))),
                });
            }
            return lstLayouts;
        }


    }
}
