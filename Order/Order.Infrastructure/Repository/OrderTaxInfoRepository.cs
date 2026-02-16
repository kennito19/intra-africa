using Microsoft.Extensions.Configuration;
using Order.Application.IRepositories;
using Order.Domain;
using Order.Domain.Entity;
using Order.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@orderid", orderTaxInfo.OrderID),

                new SqlParameter("@orderitemid", orderTaxInfo.OrderItemID),
                new SqlParameter("@productid", orderTaxInfo.ProductID),
                new SqlParameter("@sellerproductid", orderTaxInfo.SellerProductID),
                new SqlParameter("@shippingcharge", orderTaxInfo.ShippingCharge),
                new SqlParameter("@shippingzone", orderTaxInfo.ShippingZone),
                new SqlParameter("@taxonshipping", orderTaxInfo.TaxOnShipping),
                new SqlParameter("@commissionin", orderTaxInfo.CommissionIn),
                new SqlParameter("@commissionrate", orderTaxInfo.CommissionRate),
                new SqlParameter("@commissionamount", orderTaxInfo.CommissionAmount),
                new SqlParameter("@taxoncommission", orderTaxInfo.TaxOnCommission),
                new SqlParameter("@netearn", orderTaxInfo.NetEarn),
                new SqlParameter("@ordertaxrateid", orderTaxInfo.OrderTaxRateId),
                new SqlParameter("@ordertaxrate", orderTaxInfo.OrderTaxRate),
                new SqlParameter("@hsncode", orderTaxInfo.HSNCode),
                new SqlParameter("@shipmentby", orderTaxInfo.ShipmentBy),
                new SqlParameter("@shipmentpaidby", orderTaxInfo.ShipmentPaidBy),
                new SqlParameter("@issellerwithgstatordertime", orderTaxInfo.IsSellerWithGSTAtOrderTime),
                new SqlParameter("@weightslab", orderTaxInfo.WeightSlab),


                new SqlParameter("@createdBy", orderTaxInfo.CreatedBy),
                new SqlParameter("@createdAt", orderTaxInfo.CreatedAt)

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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", orderTaxInfo.Id),

                new SqlParameter("@shippingcharge", orderTaxInfo.ShippingCharge),
                new SqlParameter("@shippingzone", orderTaxInfo.ShippingZone),
                new SqlParameter("@taxonshipping", orderTaxInfo.TaxOnShipping),
                new SqlParameter("@commissionin", orderTaxInfo.CommissionIn),
                new SqlParameter("@commissionrate", orderTaxInfo.CommissionRate),
                new SqlParameter("@commissionamount", orderTaxInfo.CommissionAmount),
                new SqlParameter("@taxoncommission", orderTaxInfo.TaxOnCommission),
                new SqlParameter("@netearn", orderTaxInfo.NetEarn),
                new SqlParameter("@ordertaxrateid", orderTaxInfo.OrderTaxRateId),
                new SqlParameter("@ordertaxrate", orderTaxInfo.OrderTaxRate),
                new SqlParameter("@hsncode", orderTaxInfo.HSNCode),
                new SqlParameter("@shipmentby", orderTaxInfo.ShipmentBy),
                new SqlParameter("@shipmentpaidby", orderTaxInfo.ShipmentPaidBy),
                new SqlParameter("@issellerwithgstatordertime", orderTaxInfo.IsSellerWithGSTAtOrderTime),
                new SqlParameter("@weightslab", orderTaxInfo.WeightSlab),

                new SqlParameter("@modifiedBy", orderTaxInfo.ModifiedBy),
                new SqlParameter("@modifiedAt", orderTaxInfo.ModifiedAt)

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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", orderTaxInfo.Id),
                new SqlParameter("@deletedBy", orderTaxInfo.DeletedBy),
                new SqlParameter("@deletedAt", orderTaxInfo.DeletedAt)
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", orderTaxInfo.Id),
                new SqlParameter("@orderid", orderTaxInfo.OrderID),
                new SqlParameter("@orderitemid", orderTaxInfo.OrderItemID),
                new SqlParameter("@productid", orderTaxInfo.ProductID),
                new SqlParameter("@sellerproductid", orderTaxInfo.SellerProductID),
                new SqlParameter("@isDeleted", orderTaxInfo.IsDeleted),
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
