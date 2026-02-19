using Catalogue.Application.IRepositories;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using MySqlConnector;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data.Common;

namespace Catalogue.Infrastructure.Repository
{
    public class ProductViewRepository : IProductViewRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public ProductViewRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(ProductView productView)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                 new MySqlParameter("@ProductId", productView.ProductId),
                new MySqlParameter("@ProductGUID", productView.ProductGUID),
                new MySqlParameter("@SellerId", productView.SellerId),
                new MySqlParameter("@SellerProductId", productView.SellerProductId),
                new MySqlParameter("@UserId", productView.UserId),
                new MySqlParameter("@CreatedAt", productView.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductView, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ProductView>>> get(ProductView productView, int PageIndex, int PageSize, string Mode)
        {

            var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@ProductId", productView.ProductId),
                new MySqlParameter("@SellerId", productView.SellerId),
                new MySqlParameter("@UserId", productView.UserId),
                new MySqlParameter("@SellerProductId", productView.SellerProductId),
                new MySqlParameter("@fromdate", productView.fromDate),
                new MySqlParameter("@todate", productView.toDate),
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

            return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetProductView, ProductViewParserAsync, output, newid: null, message, sqlParams.ToArray());
        }

        private async Task<List<ProductView>> ProductViewParserAsync(DbDataReader reader)
        {
            List<ProductView> lstProductView = new List<ProductView>();
            while (await reader.ReadAsync())
            {
                lstProductView.Add(new ProductView()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    ProductId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductId"))),
                    ProductGUID = Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductGUID"))),
                    SellerId = Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerId"))),
                    SellerProductId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductId"))),
                    UserId = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                });
            }
            return lstProductView;
        }
    }
}
