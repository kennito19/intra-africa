using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using Catalogue.Application.IRepositories;

namespace Catalogue.Infrastructure.Repository
{
    public class UserProductsRepository: IUserProductsRepository
    {

        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public UserProductsRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

       

        public async Task<BaseResponse<List<UserProductList>>> get(UserProductParams productList, int? PageIndex = 0, int? PageSize = 0)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                
                new SqlParameter("@categoryid", productList.CategoryId),
                new SqlParameter("@sellerids", productList.SellerIds),
                new SqlParameter("@brandids", productList.BrandIds),
                new SqlParameter("@searchtext", productList.searchTexts),
                new SqlParameter("@sizeids", productList.SizeIds),
                new SqlParameter("@colorids", productList.ColorIds),
                new SqlParameter("@productcollectionid", productList.productCollectionId),
                new SqlParameter("@guids",productList.guIds ),
                new SqlParameter("@minprice", productList.MinPrice),
                new SqlParameter("@maxprice", productList.MaxPrice),
                new SqlParameter("@mindiscount", productList.MinDiscount),
                new SqlParameter("@AvailableProductsOnly", productList.AvailableProductsOnly),
                new SqlParameter("@PriceSort", productList.PriceSort),
                new SqlParameter("@SpecTypeIds", productList.SpecTypeIds),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@PageSize", PageSize),
                new SqlParameter("@date", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetUserProducts, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<UserProductList>> LayoutParserAsync(DbDataReader reader)
        {
            List<UserProductList> lstLayouts = new List<UserProductList>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new UserProductList()
                {
                    PageCount = reader.IsDBNull(reader.GetOrdinal("PageCount")) ? (int?)null : Convert.ToInt32(reader["PageCount"]),

                    RowNumber = reader.IsDBNull(reader.GetOrdinal("RowNumber")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Guid = reader.IsDBNull(reader.GetOrdinal("Guid")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    RecordCount = reader.IsDBNull(reader.GetOrdinal("RecordCount")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    flag = reader.IsDBNull(reader.GetOrdinal("flag")) ? (char?)null : Convert.ToChar(reader.GetValue(reader.GetOrdinal("flag"))),
                    IsMasterProduct = reader.IsDBNull(reader.GetOrdinal("IsMasterProduct")) ? (bool?)null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsMasterProduct"))),
                    ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentId"))),
                    CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    AssiCategoryId = reader.IsDBNull(reader.GetOrdinal("AssiCategoryId")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("AssiCategoryId"))),
                    ProductName = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CustomeProductName = reader.IsDBNull(reader.GetOrdinal("CustomeProductName")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    CompanySKUCode =  reader.IsDBNull(reader.GetOrdinal("CompanySKUCode")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    Image1 = reader.IsDBNull(reader.GetOrdinal("Image1")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("Image1"))),
                    MRP = reader.IsDBNull(reader.GetOrdinal("MRP")) ? (decimal?)null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = reader.IsDBNull(reader.GetOrdinal("SellingPrice")) ? (decimal?)null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount =  reader.IsDBNull(reader.GetOrdinal("Discount")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Quantity = reader.IsDBNull(reader.GetOrdinal("MRP")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    CategoryName =  reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    CategoryPathIds = reader.IsDBNull(reader.GetOrdinal("CategoryPathIds")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathIds"))),
                    CategoryPathNames = reader.IsDBNull(reader.GetOrdinal("CategoryPathNames")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames"))),
                    SellerProductId =  reader.IsDBNull(reader.GetOrdinal("SellerProductId")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    SellerId = reader.IsDBNull(reader.GetOrdinal("SellerId")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId"))),
                    BrandId =  reader.IsDBNull(reader.GetOrdinal("BrandId")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandId"))),
                    BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandName"))),
                    TotalQty =  reader.IsDBNull(reader.GetOrdinal("TotalQty")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalQty"))),
                    Status =  reader.IsDBNull(reader.GetOrdinal("Status")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    Live = reader.IsDBNull(reader.GetOrdinal("Live")) ? (bool?)null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("Live"))),
                    ExtraDetails = reader.IsDBNull(reader.GetOrdinal("ExtraDetails")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                    TotalVariant = reader.IsDBNull(reader.GetOrdinal("TotalVariant")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalVariant"))),
                    CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedAt =  reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    F_CategoryId = reader.IsDBNull(reader.GetOrdinal("F_CategoryId")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("F_CategoryId"))),
                    F_CategoryName =  reader.IsDBNull(reader.GetOrdinal("F_CategoryName")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("F_CategoryName"))),
                    F_ProductCount =  reader.IsDBNull(reader.GetOrdinal("F_ProductCount")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("F_ProductCount"))),
                    F_BrandId =  reader.IsDBNull(reader.GetOrdinal("F_BrandId")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("F_BrandId"))),
                    F_BrandName = reader.IsDBNull(reader.GetOrdinal("F_BrandName")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("F_BrandName"))),
                    F_SizeID =  reader.IsDBNull(reader.GetOrdinal("F_SizeID")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("F_SizeID"))),
                    F_Size = reader.IsDBNull(reader.GetOrdinal("F_Size")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("F_Size"))),
                    F_Quantity = reader.IsDBNull(reader.GetOrdinal("F_Quantity")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("F_Quantity"))),
                    F_ColorID =  reader.IsDBNull(reader.GetOrdinal("F_ColorID")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("F_ColorID"))),
                    F_ColorName =  reader.IsDBNull(reader.GetOrdinal("F_ColorName")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("F_ColorName"))),
                    F_ColorCode =reader.IsDBNull(reader.GetOrdinal("F_ColorCode")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("F_ColorCode"))),
                    MinSellingPrice =reader.IsDBNull(reader.GetOrdinal("MinSellingPrice")) ? (decimal?)null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MinSellingPrice"))),
                    MaxSellingPrice =reader.IsDBNull(reader.GetOrdinal("MaxSellingPrice")) ? (decimal?)null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MaxSellingPrice"))),
                    FilterTypeId =reader.IsDBNull(reader.GetOrdinal("FilterTypeId")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("FilterTypeId"))),
                    FilterTypeName =reader.IsDBNull(reader.GetOrdinal("FilterTypeName")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("FilterTypeName"))),
                    FilterValueId =reader.IsDBNull(reader.GetOrdinal("FilterValueId")) ? (int?)null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("FilterValueId"))),
                    FilterValueName = reader.IsDBNull(reader.GetOrdinal("FilterValueName")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("FilterValueName"))),
                    Highlights = reader.IsDBNull(reader.GetOrdinal("Highlights")) ? string.Empty : Convert.ToString(reader.GetValue(reader.GetOrdinal("Highlights"))),
                });
            }
            return lstLayouts;
        }
    }
}
