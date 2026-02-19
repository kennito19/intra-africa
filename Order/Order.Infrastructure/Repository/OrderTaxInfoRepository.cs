using Microsoft.Extensions.Configuration;
using Order.Application.IRepositories;
using Order.Domain;
using Order.Domain.Entity;
using Order.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using MySqlConnector;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Order.Infrastructure.Repository
{
    public class OrderTaxInfoRepository : IOrderTaxInfoRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public OrderTaxInfoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(OrderTaxInfo orderTaxInfo)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@orderid", orderTaxInfo.OrderID),

                new MySqlParameter("@orderitemid", orderTaxInfo.OrderItemID),
                new MySqlParameter("@productid", orderTaxInfo.ProductID),
                new MySqlParameter("@sellerproductid", orderTaxInfo.SellerProductID),
                new MySqlParameter("@shippingcharge", orderTaxInfo.ShippingCharge),
                new MySqlParameter("@shippingzone", orderTaxInfo.ShippingZone),
                new MySqlParameter("@taxonshipping", orderTaxInfo.TaxOnShipping),
                new MySqlParameter("@commissionin", orderTaxInfo.CommissionIn),
                new MySqlParameter("@commissionrate", orderTaxInfo.CommissionRate),
                new MySqlParameter("@commissionamount", orderTaxInfo.CommissionAmount),
                new MySqlParameter("@taxoncommission", orderTaxInfo.TaxOnCommission),
                new MySqlParameter("@netearn", orderTaxInfo.NetEarn),
                new MySqlParameter("@ordertaxrateid", orderTaxInfo.OrderTaxRateId),
                new MySqlParameter("@ordertaxrate", orderTaxInfo.OrderTaxRate),
                new MySqlParameter("@hsncode", orderTaxInfo.HSNCode),
                new MySqlParameter("@shipmentby", orderTaxInfo.ShipmentBy),
                new MySqlParameter("@shipmentpaidby", orderTaxInfo.ShipmentPaidBy),
                new MySqlParameter("@issellerwithgstatordertime", orderTaxInfo.IsSellerWithGSTAtOrderTime),
                new MySqlParameter("@weightslab", orderTaxInfo.WeightSlab),


                new MySqlParameter("@createdBy", orderTaxInfo.CreatedBy),
                new MySqlParameter("@createdAt", orderTaxInfo.CreatedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderTaxInfo, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(OrderTaxInfo orderTaxInfo)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", orderTaxInfo.Id),

                new MySqlParameter("@shippingcharge", orderTaxInfo.ShippingCharge),
                new MySqlParameter("@shippingzone", orderTaxInfo.ShippingZone),
                new MySqlParameter("@taxonshipping", orderTaxInfo.TaxOnShipping),
                new MySqlParameter("@commissionin", orderTaxInfo.CommissionIn),
                new MySqlParameter("@commissionrate", orderTaxInfo.CommissionRate),
                new MySqlParameter("@commissionamount", orderTaxInfo.CommissionAmount),
                new MySqlParameter("@taxoncommission", orderTaxInfo.TaxOnCommission),
                new MySqlParameter("@netearn", orderTaxInfo.NetEarn),
                new MySqlParameter("@ordertaxrateid", orderTaxInfo.OrderTaxRateId),
                new MySqlParameter("@ordertaxrate", orderTaxInfo.OrderTaxRate),
                new MySqlParameter("@hsncode", orderTaxInfo.HSNCode),
                new MySqlParameter("@shipmentby", orderTaxInfo.ShipmentBy),
                new MySqlParameter("@shipmentpaidby", orderTaxInfo.ShipmentPaidBy),
                new MySqlParameter("@issellerwithgstatordertime", orderTaxInfo.IsSellerWithGSTAtOrderTime),
                new MySqlParameter("@weightslab", orderTaxInfo.WeightSlab),

                new MySqlParameter("@modifiedBy", orderTaxInfo.ModifiedBy),
                new MySqlParameter("@modifiedAt", orderTaxInfo.ModifiedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderTaxInfo, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(OrderTaxInfo orderTaxInfo)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", orderTaxInfo.Id),
                new MySqlParameter("@deletedBy", orderTaxInfo.DeletedBy),
                new MySqlParameter("@deletedAt", orderTaxInfo.DeletedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.OrderTaxInfo, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<OrderTaxInfo>>> Get(OrderTaxInfo orderTaxInfo, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", orderTaxInfo.Id),
                new MySqlParameter("@orderid", orderTaxInfo.OrderID),
                new MySqlParameter("@orderitemid", orderTaxInfo.OrderItemID),
                new MySqlParameter("@productid", orderTaxInfo.ProductID),
                new MySqlParameter("@sellerproductid", orderTaxInfo.SellerProductID),
                new MySqlParameter("@isDeleted", orderTaxInfo.IsDeleted),
                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@PageSize", PageSize),

            };
                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                //MySqlParameter newid = new MySqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetOrderTaxInfo, orderTaxParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<OrderTaxInfo>> orderTaxParserAsync(DbDataReader reader)
        {
            List<OrderTaxInfo> lstorderTaxInfo = new List<OrderTaxInfo>();
            while (await reader.ReadAsync())
            {
                lstorderTaxInfo.Add(new OrderTaxInfo()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),

                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    OrderID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderID"))),
                    OrderItemID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderItemID"))),
                    ProductID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductID"))),
                    SellerProductID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SellerProductID"))),
                    ShippingCharge = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingCharge")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("ShippingCharge"))),
                    ShippingZone = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingZone")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShippingZone"))),
                    TaxOnShipping = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxOnShipping")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TaxOnShipping"))),
                    CommissionIn = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CommissionIn")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CommissionIn"))),
                    CommissionRate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CommissionRate")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CommissionRate"))),
                    CommissionAmount = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CommissionAmount")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("CommissionAmount"))),
                    TaxOnCommission = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TaxOnCommission")))) ? null : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("TaxOnCommission"))),
                    NetEarn = Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("NetEarn"))),
                    OrderTaxRateId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("OrderTaxRateId"))),
                    OrderTaxRate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderTaxRate")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OrderTaxRate"))),
                    HSNCode = Convert.ToString(reader.GetValue(reader.GetOrdinal("HSNCode"))),
                    ShipmentBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentBy"))),
                    ShipmentPaidBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentPaidBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ShipmentPaidBy"))),
                    IsSellerWithGSTAtOrderTime = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsSellerWithGSTAtOrderTime"))),
                    WeightSlab = Convert.ToString(reader.GetValue(reader.GetOrdinal("WeightSlab"))),


                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),

                });
            }
            return lstorderTaxInfo;
        }


    }
}
