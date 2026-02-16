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
    public class ProductPriceMasterRepository : IProductPriceMasterRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;
        public ProductPriceMasterRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(ProductPriceMaster productPriceMaster)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@sellerproductid", productPriceMaster.SellerProductID),
                new SqlParameter("@mrp", productPriceMaster.MRP),
                new SqlParameter("@sellingprice", productPriceMaster.SellingPrice),
                new SqlParameter("@discount", productPriceMaster.Discount),
                new SqlParameter("@quantity", productPriceMaster.Quantity),
                new SqlParameter("@marginIn", productPriceMaster.MarginIn),
                new SqlParameter("@marginCost", productPriceMaster.MarginCost),
                new SqlParameter("@marginPercentage", productPriceMaster.MarginPercentage),
                new SqlParameter("@sizeid", productPriceMaster.SizeID),
                new SqlParameter("@createdby", productPriceMaster.CreatedBy),
                new SqlParameter("@createdat", productPriceMaster.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductPrice, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(ProductPriceMaster productPriceMaster)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", productPriceMaster.Id),
                new SqlParameter("@sellerproductid", productPriceMaster.SellerProductID),
                new SqlParameter("@mrp", productPriceMaster.MRP),
                new SqlParameter("@sellingprice", productPriceMaster.SellingPrice),
                new SqlParameter("@discount", productPriceMaster.Discount),
                new SqlParameter("@quantity", productPriceMaster.Quantity),
                new SqlParameter("@marginIn", productPriceMaster.MarginIn),
                new SqlParameter("@marginCost", productPriceMaster.MarginCost),
                new SqlParameter("@marginPercentage", productPriceMaster.MarginPercentage),
                new SqlParameter("@sizeid", productPriceMaster.SizeID),
                new SqlParameter("@modifiedby", productPriceMaster.ModifiedBy),
                new SqlParameter("@modifiedat", productPriceMaster.ModifiedAt),
                new SqlParameter("@deletedby", productPriceMaster.DeletedBy),
                new SqlParameter("@deletedat", productPriceMaster.DeletedAt),
                new SqlParameter("@isdeleted", productPriceMaster.IsDeleted),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductPrice, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(ProductPriceMaster productPriceMaster)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@sellerproductid", productPriceMaster.SellerProductID),
                new SqlParameter("@id", productPriceMaster.Id),
                new SqlParameter("@deletedby", productPriceMaster.DeletedBy),
                new SqlParameter("@deletedat", productPriceMaster.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductPrice, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ProductPriceMaster>>> Get(ProductPriceMaster productPriceMaster, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", productPriceMaster.Id),
                new SqlParameter("@productid", productPriceMaster.ProductID),
                new SqlParameter("@sellerproductid", productPriceMaster.SellerProductID),
                new SqlParameter("@sizeid", productPriceMaster.SizeID),
                new SqlParameter("@sizename", productPriceMaster.SizeName),
                new SqlParameter("@isdeleted", productPriceMaster.IsDeleted),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@PageSize", PageSize),

            };
                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                //SqlParameter newid = new SqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetProductPrice, sellerWisePriceMasterParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ProductPriceMaster>> sellerWisePriceMasterParserAsync(DbDataReader reader)
        {
            List<ProductPriceMaster> lstproductPriceMaster = new List<ProductPriceMaster>();
            while (await reader.ReadAsync())
            {
                lstproductPriceMaster.Add(new ProductPriceMaster()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    ProductID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    SellerProductID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductID"))),
                    MRP = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MRP")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("MRP"))),
                    SellingPrice = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellingPrice")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    Discount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Discount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    Quantity = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Quantity")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    MarginIn = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginIn")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginIn"))),
                    MarginCost = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginCost")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MarginCost"))),
                    MarginPercentage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MarginPercentage")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MarginPercentage"))),
                    SizeID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeID"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    SizeName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeName"))),
                    SizeTypeName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeTypeName"))),
                });
            }
            return lstproductPriceMaster;
        }
    }
}
