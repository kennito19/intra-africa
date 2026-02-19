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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catalogue.Infrastructure.Repository
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public ReportsRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<List<ProductReport>>> GetProductReport(string? SellerId = null, string? fromDate = null, string? toDate = null)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "ProductReport"),
                    new MySqlParameter("@sellerId", SellerId),
                    new MySqlParameter("@fromdate", fromDate),
                    new MySqlParameter("@todate",toDate),

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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Reports, GetProductReportParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<ProductDetailsReport>>> GetProductDetailsReport(string? productIds = null)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "productDetails"),
                    new MySqlParameter("@productids", productIds),

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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Reports, GetProductDetailsReportParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<ProductReport>> GetProductReportParserAsync(DbDataReader reader)
        {
            List<ProductReport> lstKYCDetails = new List<ProductReport>();
            while (await reader.ReadAsync())
            {
                lstKYCDetails.Add(new ProductReport()
                {
                    PathNames = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("PathNames")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PathNames"))),
                    TaxName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TaxName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxName"))),
                    HSNCode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("HSNCode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode"))),
                    ProductId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductId"))),
                    ProductName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProductName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    CustomeProductName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CustomeProductName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CustomeProductName"))),
                    CompanySKUCode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CompanySKUCode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    SellerSKU = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SellerSKU")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerSKU"))),
                    PackingLength = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("PackingLength")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingLength"))),
                    PackingBreadth = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("PackingBreadth")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingBreadth"))),
                    PackingHeight = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("PackingHeight")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingHeight"))),
                    PackingWeight = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("PackingWeight")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("PackingWeight"))),
                    SellerDisplayName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SellerDisplayName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerDisplayName"))),
                    SellerLegalName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SellerLegalName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerLegalName"))),
                    SellerTradeName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SellerTradeName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerTradeName"))),
                    SellerEmail = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SellerEmail")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerEmail"))),
                    SellerPhoneNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SellerPhoneNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerPhoneNo"))),
                    ShipmentChargesPaidBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ShipmentChargesPaidBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentChargesPaidBy"))),
                    SellerStatus = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SellerStatus")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerStatus"))),
                    BrandName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("BrandName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("BrandName"))),
                    Color = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Color")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Color"))),
                    MRP = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Quantity = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    SizeType = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SizeType")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeType"))),
                    Size = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Size")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Size"))),
                    WarehouseName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("WarehouseName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("WarehouseName"))),
                    WarehouseQty = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WarehouseQty"))),
                    Status = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Status")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    Live = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("Live")))
                });
            }
            return lstKYCDetails;
        }

        private async Task<List<ProductDetailsReport>> GetProductDetailsReportParserAsync(DbDataReader reader)
        {
            List<ProductDetailsReport> lstKYCDetails = new List<ProductDetailsReport>();
            while (await reader.ReadAsync())
            {
                lstKYCDetails.Add(new ProductDetailsReport()
                {
                    ProductId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("id"))),
                    ProductGuid = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Guid")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    ProductName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProductName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                    ProductSKU = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CompanySKUCode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CompanySKUCode"))),
                    ProductImage = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProductImages")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductImages"))),
                    
                });
            }
            return lstKYCDetails;
        }
    }
}
