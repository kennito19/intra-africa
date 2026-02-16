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
    public class ArchiveProductListRepository : IArchiveProductListRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public ArchiveProductListRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<List<ArchiveProductList>>> get(ArchiveProductList productList, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", productList.ProductId),
                new SqlParameter("@guid", productList.Guid),
                new SqlParameter("@parentid", productList.ProductMasterId),
                new SqlParameter("@categoryid", productList.CategoryId),
                new SqlParameter("@brandId", productList.BrandID),
                new SqlParameter("@sellerId", productList.SellerID),
                new SqlParameter("@assicategoryid", productList.AssiCategoryId),
                new SqlParameter("@companyskucode", productList.CompanySKUCode),
                new SqlParameter("@sellerskucode", productList.SellerSKUCode),
                new SqlParameter("@searchtext", productList.SearchText),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetArchiveProducts, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ArchiveProductList>> LayoutParserAsync(DbDataReader reader)
        {
            List<ArchiveProductList> lstLayouts = new List<ArchiveProductList>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new ArchiveProductList()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    ProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductId"))),
                    Guid = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    ProductMasterId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductMasterId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductMasterId"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    AssiCategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AssiCategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("AssiCategoryId"))),
                    TaxValueId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxValueId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TaxValueId"))),
                    HSNCodeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCodeId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("HSNCodeId"))),
                    ProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CustomeProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    CompanySKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    SellerSKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSKUCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSKUCode"))),
                    SellerProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    BrandID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandID"))),
                    SellerID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerID"))),
                    Status = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Status")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    ExtraDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                    WeightSlabId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlabId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WeightSlabId"))),
                    PriceMasterId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PriceMasterId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PriceMasterId"))),
                    MRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Quantity = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Quantity")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    ProductImage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImage")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImage"))),
                    CategoryName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    CategoryPathIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathIds"))),
                    CategoryPathNames = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames"))),
                    
                });
            }
            return lstLayouts;
        }
    }
}
