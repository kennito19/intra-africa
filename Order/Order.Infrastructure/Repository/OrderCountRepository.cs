using Microsoft.Extensions.Configuration;
using Order.Application.IRepositories;
using Order.Domain;
using Order.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using MySqlConnector;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order.Domain.Entity;

namespace Order.Infrastructure.Repository
{
    public class OrderCountRepository : IOrderCountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;

        public OrderCountRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<List<OrdersCount>>> get(string? sellerId, string? userId, string? days)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@date", DateTime.Now.ToString()),
                    new MySqlParameter("@sellerid", sellerId),
                    new MySqlParameter("@userid", userId),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetOrdersCount, OrderCountParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<OrdersCount>> OrderCountParserAsync(DbDataReader reader)
        {
            List<OrdersCount> lstOrderCount = new List<OrdersCount>();
            while (await reader.ReadAsync())
            {
                lstOrderCount.Add(new OrdersCount()
                {
                    TotalOrders = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalOrders")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalOrders"))),
                    Pending = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Pending")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Pending"))),
                    Confirmed = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Confirmed")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Confirmed"))),
                    PartialConfirmed = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PartialConfirmed")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PartialConfirmed"))),
                    Packed = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Packed")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Packed"))),
                    PartialPacked = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PartialPacked")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PartialPacked"))),
                    Shipped = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Shipped")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Shipped"))),
                    PartialShipped = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PartialShipped")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PartialShipped"))),
                    Delivered = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Delivered")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Delivered"))),
                    PartialDelivered = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PartialDelivered")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PartialDelivered"))),
                    Cancelled = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Cancelled")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Cancelled"))),
                    Replaced = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Replaced")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Replaced"))),
                    Returned = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Returned")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Returned"))),
                    Exchanged = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Exchanged")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Exchanged"))),
                    
                });
            }
            return lstOrderCount;
        }
    }
}
