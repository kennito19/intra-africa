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
    public class ProductsCountRepository : IProductsCountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;

        public ProductsCountRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<List<ProductCounts>>> get(string? sellerId, string? days)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@date", DateTime.Now.ToString()),
                    new MySqlParameter("@sellerid", sellerId),
                    new MySqlParameter("@days", days),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetProductsCount, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ProductCounts>> LayoutParserAsync(DbDataReader reader)
        {
            List<ProductCounts> lstLayouts = new List<ProductCounts>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new ProductCounts()
                {
                    Total = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalProducts")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalProducts"))),
                    Unique = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UniqueProducts")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("UniqueProducts"))),
                    Active = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ActiveProducts")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ActiveProducts"))),
                    InExisting = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ExistingProducts")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ExistingProducts"))),
                    InRequest = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("RequestedProducts")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RequestedProducts"))),
                    Inactivate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("InactiveProducts")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("InactiveProducts"))),
                    InBulkUpload = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("BulkUploadProducts")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("BulkUploadProducts"))),
                    TotalStocks = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalStocks")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalStocks"))),
                    TotalActiveStocks = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalActiveStocks")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalActiveStocks"))),
                    //InOutOfStock = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OutOfStockProducts")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OutOfStockProducts"))),

                });
            }
            return lstLayouts;
        }
    }
}
