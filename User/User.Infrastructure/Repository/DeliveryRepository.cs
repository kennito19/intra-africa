using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Domain.Entity;
using User.Domain;
using User.Infrastructure.Helper;

namespace User.Infrastructure.Repository
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public DeliveryRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(DeliveryLibrary deliveryLibrary)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@countryid", deliveryLibrary.CountryID),
                new SqlParameter("@stateid", deliveryLibrary.StateID),
                new SqlParameter("@cityid", deliveryLibrary.CityID),
                new SqlParameter("@locality", deliveryLibrary.Locality),
                new SqlParameter("@pincode", deliveryLibrary.Pincode),
                new SqlParameter("@deliveryDays", deliveryLibrary.DeliveryDays),
                new SqlParameter("@status", deliveryLibrary.Status),
                new SqlParameter("@isCODActive", deliveryLibrary.IsCODActive),
                new SqlParameter("@createdby", deliveryLibrary.CreatedBy),
                new SqlParameter("@createdat", deliveryLibrary.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Delivery, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(DeliveryLibrary deliveryLibrary)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", deliveryLibrary.Id),
                new SqlParameter("@countryid", deliveryLibrary.CountryID),
                new SqlParameter("@stateid", deliveryLibrary.StateID),
                new SqlParameter("@cityid", deliveryLibrary.CityID),
                new SqlParameter("@locality", deliveryLibrary.Locality),
                new SqlParameter("@pincode", deliveryLibrary.Pincode),
                new SqlParameter("@deliveryDays", deliveryLibrary.DeliveryDays),
                new SqlParameter("@status", deliveryLibrary.Status),
                new SqlParameter("@isCODActive", deliveryLibrary.IsCODActive),
                new SqlParameter("@modifiedby", deliveryLibrary.ModifiedBy),
                new SqlParameter("@modifiedat", deliveryLibrary.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Delivery, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(DeliveryLibrary deliveryLibrary)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", deliveryLibrary.Id),
                new SqlParameter("@deletedby", deliveryLibrary.DeletedBy),
                new SqlParameter("@deletedat", deliveryLibrary.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Delivery, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<DeliveryLibrary>>> Get(DeliveryLibrary deliveryLibrary, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", deliveryLibrary.Id),
                new SqlParameter("@countryid", deliveryLibrary.CountryID),
                new SqlParameter("@stateid", deliveryLibrary.StateID),
                new SqlParameter("@cityid", deliveryLibrary.CityID),
                new SqlParameter("@searchtext", deliveryLibrary.searchText),
                new SqlParameter("@locality", deliveryLibrary.Locality),
                new SqlParameter("@pincode", deliveryLibrary.Pincode),
                new SqlParameter("@countryname", deliveryLibrary.CountryName),
                new SqlParameter("@statename", deliveryLibrary.StateName),
                new SqlParameter("@cityname", deliveryLibrary.CityName),
                new SqlParameter("@status", deliveryLibrary.Status),
                new SqlParameter("@isdeleted", deliveryLibrary.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetDelivery, DeliveryParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<DeliveryLibrary>> DeliveryParserAsync(DbDataReader reader)
        {
            List<DeliveryLibrary> lstdeliveryLibrary = new List<DeliveryLibrary>();
            while (await reader.ReadAsync())
            {
                lstdeliveryLibrary.Add(new DeliveryLibrary()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    CountryID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CountryID"))),
                    StateID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("StateID"))),
                    CityID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CityID"))),
                    Locality = Convert.ToString(reader.GetValue(reader.GetOrdinal("Locality"))),
                    Pincode = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Pincode"))),
                    DeliveryDays = Convert.ToString(reader.GetValue(reader.GetOrdinal("DeliveryDays"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    IsCODActive = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsCODActive"))),

                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    CountryName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CountryName"))),
                    StateName = Convert.ToString(reader.GetValue(reader.GetOrdinal("StateName"))),
                    CityName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CityName"))),
                });
            }
            return lstdeliveryLibrary;
        }
    }
}
