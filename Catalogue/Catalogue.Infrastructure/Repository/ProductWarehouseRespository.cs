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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class ProductWarehouseRespository : IProductWarehouseRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public ProductWarehouseRespository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> AddProductWarehouse(ProductWarehouse productWarehouse)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "add"),
                    new SqlParameter("@sellerproductid", productWarehouse.SellerProductID),
                    new SqlParameter("@productid", productWarehouse.ProductID),
                    new SqlParameter("@sellerwiseproductpricemasterid", productWarehouse.SellerWiseProductPriceMasterID),
                    new SqlParameter("@warehouseid", productWarehouse.WarehouseID),
                    new SqlParameter("@warehousename", productWarehouse.WarehouseName),
                    new SqlParameter("@productquantity", productWarehouse.ProductQuantity),
                    new SqlParameter("@createdby", productWarehouse.CreatedBy),
                    new SqlParameter("@createdat", productWarehouse.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductWareHouse, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> UpdateProductWarehouse(ProductWarehouse productWarehouse)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@id", productWarehouse.Id),
                    new SqlParameter("@sellerproductid", productWarehouse.SellerProductID),
                    new SqlParameter("@productid", productWarehouse.ProductID),
                    new SqlParameter("@sellerwiseproductpricemasterid", productWarehouse.SellerWiseProductPriceMasterID),
                    new SqlParameter("@warehouseid", productWarehouse.WarehouseID),
                    new SqlParameter("@warehousename", productWarehouse.WarehouseName),
                    new SqlParameter("@productquantity", productWarehouse.ProductQuantity),
                    new SqlParameter("@modifiedBy", productWarehouse.ModifiedBy),
                    new SqlParameter("@modifiedAt", productWarehouse.ModifiedAt),
                    new SqlParameter("@deletedby", productWarehouse.DeletedBy),
                    new SqlParameter("@deletedat", productWarehouse.DeletedAt),
                    new SqlParameter("@isdeleted", productWarehouse.IsDeleted),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductWareHouse, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> DeleteProductWarehouse(ProductWarehouse productWarehouse)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id", productWarehouse.Id),
                    new SqlParameter("@deletedby", productWarehouse.DeletedBy),
                    new SqlParameter("@deletedat", productWarehouse.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductWareHouse, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ProductWarehouse>>> GetProductWarehouse(ProductWarehouse productWarehouse, int PageIndex, int PageSize, string Mode)
        {
            var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", productWarehouse.Id),
                new SqlParameter("@sellerproductid", productWarehouse.SellerProductID),
                new SqlParameter("@sellerwiseproductpricemasterid", productWarehouse.SellerWiseProductPriceMasterID),
                new SqlParameter("@productid", productWarehouse.ProductID),
                new SqlParameter("@warehouseid", productWarehouse.WarehouseID),
                new SqlParameter("@warehousename", productWarehouse.WarehouseName),
                new SqlParameter("@isdeleted", productWarehouse.IsDeleted),
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

            return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetProductWareHouse, ProductWarehouseParserAsync, output, newid: null, message, sqlParams.ToArray());
        }
        private async Task<List<ProductWarehouse>> ProductWarehouseParserAsync(DbDataReader reader)
        {
            List<ProductWarehouse> lstProductWarehouse = new List<ProductWarehouse>();
            while (await reader.ReadAsync())
            {
                lstProductWarehouse.Add(new ProductWarehouse()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    SellerProductID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductID"))),
                    SellerWiseProductPriceMasterID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerWiseProductPriceMasterID"))),
                    ProductID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    WarehouseID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WarehouseID"))),
                    WarehouseName = Convert.ToString(reader.GetValue(reader.GetOrdinal("WarehouseName"))),
                    ProductQuantity = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductQuantity"))),
                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    ProductName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductName"))),
                });
            }
            return lstProductWarehouse;
        }

        public async Task<BaseResponse<List<SizeWiseWarehouse>>> GetSizeWiseWarehouse(SizeWiseWarehouse productWarehouse)
        {
            var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@sellerProductId", productWarehouse.SellerProductId),
                new SqlParameter("@productid", productWarehouse.ProductId),
                new SqlParameter("@sizeid", productWarehouse.SizeId)
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

            return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetSizeWiseWarehouse, GetSizeWiseWarehouseParserAsync, output, newid: null, message, sqlParams.ToArray());
        }
        private async Task<List<SizeWiseWarehouse>> GetSizeWiseWarehouseParserAsync(DbDataReader reader)
        {
            List<SizeWiseWarehouse> lstProductWarehouse = new List<SizeWiseWarehouse>();
            while (await reader.ReadAsync())
            {
                lstProductWarehouse.Add(new SizeWiseWarehouse()
                {
                    ProductWarehouseId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Id")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    ProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    SellerProductId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SellerProductID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductID"))),
                    WarehouseId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("WarehouseID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WarehouseID"))),
                    SizeId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeID")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeID"))),
                    Quantity = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ProductQuantity")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductQuantity")))
                });
            }
            return lstProductWarehouse;
        }

        public async Task<BaseResponse<long>> UpdateWarehouseStock(int warehouseid)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@warehouseid", warehouseid)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.UpdateWarehouseStock, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
