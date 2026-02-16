using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Domain;
using User.Domain.Entity;
using User.Infrastructure.Helper;

namespace User.Infrastructure.Repository
{
    public class CartRepository:ICartRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public CartRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(Cart cart)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode","add"),
                    new SqlParameter("@userid",cart.UserId),
                    new SqlParameter("@sessionid",cart.SessionId),
                    new SqlParameter("@sellerproductmasterid",cart.SellerProductMasterId),
                    new SqlParameter("@sizeid",cart.SizeId),
                    new SqlParameter("@quantity",cart.Quantity),
                    new SqlParameter("@mrp",cart.TempMRP),
                    new SqlParameter("@sellingprice",cart.TempSellingPrice),
                    new SqlParameter("@discount",cart.TempDiscount),
                    new SqlParameter("@subtotal",cart.SubTotal),
                    new SqlParameter("@warrantyId",cart.WarrantyId),
                    new SqlParameter("@createdBy", cart.CreatedBy),
                    new SqlParameter("@createdAt", cart.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Cart, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> Update(Cart cart)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@id", cart.Id),
                    new SqlParameter("@userid",cart.UserId),
                    new SqlParameter("@sessionid",cart.SessionId),
                    new SqlParameter("@sellerproductmasterid",cart.SellerProductMasterId),
                    new SqlParameter("@sizeid",cart.SizeId),
                    new SqlParameter("@quantity",cart.Quantity),
                    new SqlParameter("@mrp",cart.TempMRP),
                    new SqlParameter("@sellingprice",cart.TempSellingPrice),
                    new SqlParameter("@discount",cart.TempDiscount),
                    new SqlParameter("@subtotal",cart.SubTotal),
                    new SqlParameter("@warrantyId",cart.WarrantyId),
                    new SqlParameter("@modifiedBy", cart.ModifiedBy),
                    new SqlParameter("@modifiedAt", cart.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Cart, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> AddUserIdinCart(AddCartUserId cart)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "updateUserId"),
                    new SqlParameter("@userid",cart.UserId),
                    new SqlParameter("@sessionid",cart.SessionId)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Cart, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> Delete(Cart cart, string? sellerProductIds = null)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id",cart.Id),
                    new SqlParameter("@sellerproductmasterid",cart.SellerProductMasterId),
                    new SqlParameter("@sessionid",cart.SessionId),
                    new SqlParameter("@userid",cart.UserId),
                    new SqlParameter("@sellerproductIds",sellerProductIds),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Cart, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<Cart>>> Get(Cart cart, int PageIndex, int PageSize, string Mode, string? sellerProductIds = null)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", Mode),
                    new SqlParameter("@id", cart.Id),
                    new SqlParameter("@userid",cart.UserId),
                    new SqlParameter("@sessionid",cart.SessionId),
                    new SqlParameter("@WarrantyId",cart.WarrantyId),
                    new SqlParameter("@searchtext",cart.searchText),
                    new SqlParameter("@sellerproductmasterid",cart.SellerProductMasterId),
                    new SqlParameter("@sellerproductIds",sellerProductIds),
                    new SqlParameter("@sizeid",cart.SizeId),
                    new SqlParameter("@pageIndex", PageIndex),
                    new SqlParameter("@PageSize", PageSize),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetCart, CartParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<Cart>> CartParserAsync(DbDataReader reader)
        {
            List<Cart> lstCart = new List<Cart>();
            while (await reader.ReadAsync())
            {
                lstCart.Add(new Cart()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    UserId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserId")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId"))),
                    SessionId = Convert.ToString(reader.GetValue(reader.GetOrdinal("SessionId"))),
                    SellerProductMasterId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductMasterId"))),
                    SizeId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SizeId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeId"))),
                    Quantity = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    TempMRP = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    TempSellingPrice = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    TempDiscount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    SubTotal = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SubTotal"))),
                    WarrantyId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("WarrantyId")).ToString()) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WarrantyId"))),
                    Fullname = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Fullname")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Fullname"))),
                    Username = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Username")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Username"))),
                    Phone = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Phone")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Phone"))),
                    UserStatus = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserStatus")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserStatus"))),

                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                });
            }
            return lstCart;
        }

        public async Task<BaseResponse<List<Cart>>> GetAbandonedCart(Cart cart, int PageIndex, int PageSize, string Mode, string? sellerProductIds = null)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", Mode),
                    new SqlParameter("@id", cart.Id),
                    new SqlParameter("@userid",cart.UserId),
                    new SqlParameter("@sessionid",cart.SessionId),
                    new SqlParameter("@WarrantyId",cart.WarrantyId),
                    new SqlParameter("@searchtext",cart.searchText),
                    new SqlParameter("@sellerproductmasterid",cart.SellerProductMasterId),
                    new SqlParameter("@sellerproductIds",sellerProductIds),
                    new SqlParameter("@sizeid",cart.SizeId),
                    new SqlParameter("@pageIndex", PageIndex),
                    new SqlParameter("@PageSize", PageSize),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetAbandonedCart, AbundantCartParserAsync, output, newid: null, message, sqlParams.ToArray());


            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task<List<Cart>> AbundantCartParserAsync(DbDataReader reader)
        {
            List<Cart> lstCart = new List<Cart>();
            while (await reader.ReadAsync())
            {
                lstCart.Add(new Cart()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    UserId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserId")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId"))),
                    SessionId = Convert.ToString(reader.GetValue(reader.GetOrdinal("SessionId"))),
                    SellerProductMasterId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductMasterId"))),
                    SizeId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SizeId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeId"))),
                    Quantity = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Quantity"))),
                    TempMRP = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("MRP"))),
                    TempSellingPrice = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SellingPrice"))),
                    TempDiscount = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("Discount"))),
                    SubTotal = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("SubTotal"))),
                    WarrantyId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("WarrantyId")).ToString()) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("WarrantyId"))),
                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                });
            }
            return lstCart;
        }

    }
}
