using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
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
        private readonly MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public CartRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(Cart cart)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode","add"),
                    new MySqlParameter("@userid",cart.UserId),
                    new MySqlParameter("@sessionid",cart.SessionId),
                    new MySqlParameter("@sellerproductmasterid",cart.SellerProductMasterId),
                    new MySqlParameter("@sizeid",cart.SizeId),
                    new MySqlParameter("@quantity",cart.Quantity),
                    new MySqlParameter("@mrp",cart.TempMRP),
                    new MySqlParameter("@sellingprice",cart.TempSellingPrice),
                    new MySqlParameter("@discount",cart.TempDiscount),
                    new MySqlParameter("@subtotal",cart.SubTotal),
                    new MySqlParameter("@warrantyId",cart.WarrantyId),
                    new MySqlParameter("@createdBy", cart.CreatedBy),
                    new MySqlParameter("@createdAt", cart.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id", cart.Id),
                    new MySqlParameter("@userid",cart.UserId),
                    new MySqlParameter("@sessionid",cart.SessionId),
                    new MySqlParameter("@sellerproductmasterid",cart.SellerProductMasterId),
                    new MySqlParameter("@sizeid",cart.SizeId),
                    new MySqlParameter("@quantity",cart.Quantity),
                    new MySqlParameter("@mrp",cart.TempMRP),
                    new MySqlParameter("@sellingprice",cart.TempSellingPrice),
                    new MySqlParameter("@discount",cart.TempDiscount),
                    new MySqlParameter("@subtotal",cart.SubTotal),
                    new MySqlParameter("@warrantyId",cart.WarrantyId),
                    new MySqlParameter("@modifiedBy", cart.ModifiedBy),
                    new MySqlParameter("@modifiedAt", cart.ModifiedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "updateUserId"),
                    new MySqlParameter("@userid",cart.UserId),
                    new MySqlParameter("@sessionid",cart.SessionId)
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id",cart.Id),
                    new MySqlParameter("@sellerproductmasterid",cart.SellerProductMasterId),
                    new MySqlParameter("@sessionid",cart.SessionId),
                    new MySqlParameter("@userid",cart.UserId),
                    new MySqlParameter("@sellerproductIds",sellerProductIds),
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", Mode),
                    new MySqlParameter("@id", cart.Id),
                    new MySqlParameter("@userid",cart.UserId),
                    new MySqlParameter("@sessionid",cart.SessionId),
                    new MySqlParameter("@WarrantyId",cart.WarrantyId),
                    new MySqlParameter("@searchtext",cart.searchText),
                    new MySqlParameter("@sellerproductmasterid",cart.SellerProductMasterId),
                    new MySqlParameter("@sellerproductIds",sellerProductIds),
                    new MySqlParameter("@sizeid",cart.SizeId),
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", Mode),
                    new MySqlParameter("@id", cart.Id),
                    new MySqlParameter("@userid",cart.UserId),
                    new MySqlParameter("@sessionid",cart.SessionId),
                    new MySqlParameter("@WarrantyId",cart.WarrantyId),
                    new MySqlParameter("@searchtext",cart.searchText),
                    new MySqlParameter("@sellerproductmasterid",cart.SellerProductMasterId),
                    new MySqlParameter("@sellerproductIds",sellerProductIds),
                    new MySqlParameter("@sizeid",cart.SizeId),
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
