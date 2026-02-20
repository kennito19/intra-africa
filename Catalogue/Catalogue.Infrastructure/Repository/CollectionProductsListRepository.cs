using Catalogue.Application.IRepositories;
using Catalogue.Domain;
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
    public class CollectionProductsListRepository : ICollectionProductsListRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;

        public CollectionProductsListRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<List<CollectionProductsList>>> get(CollectionProductsList productList, int PageIndex, int PageSize)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@id", productList.Id),
                new MySqlParameter("@collectionId", productList.CollectionId),
                new MySqlParameter("@categoryid", productList.CategoryId),
                new MySqlParameter("@sellerid", productList.SellerId),
                new MySqlParameter("@brandid", productList.BrandId),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetCollectionProductList, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<CollectionProductsList>> LayoutParserAsync(DbDataReader reader)
        {
            List<CollectionProductsList> lstLayouts = new List<CollectionProductsList>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new CollectionProductsList()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Guid = Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    IsMasterProduct = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsMasterProduct")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsMasterProduct"))),
                    ParentId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentId"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    ProductName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CustomeProductName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    CompanySKUCode = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    Image1 = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Image1")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Image1"))),
                    MRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Quantity = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Quantity")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    CategoryName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    CategoryPathIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathIds")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathIds"))),
                    CategoryPathNames = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames"))),
                    SellerProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    SellerId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId"))),
                    BrandId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BrandId"))),
                    Status = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Status")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    Live = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Live")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("Live"))),
                    SellerSKU = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSKU")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSKU"))),
                    ManufacturedDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ManufacturedDate")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ManufacturedDate"))),
                    ExpiryDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExpiryDate")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExpiryDate"))),
                    ExtraDetails = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ExtraDetails"))),
                });
            }
            return lstLayouts;
        }
    }
}
