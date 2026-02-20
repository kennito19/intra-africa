using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using MySqlConnector;
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
        private readonly MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public DeliveryRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(DeliveryLibrary deliveryLibrary)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@countryid", deliveryLibrary.CountryID),
                new MySqlParameter("@stateid", deliveryLibrary.StateID),
                new MySqlParameter("@cityid", deliveryLibrary.CityID),
                new MySqlParameter("@locality", deliveryLibrary.Locality),
                new MySqlParameter("@pincode", deliveryLibrary.Pincode),
                new MySqlParameter("@deliveryDays", deliveryLibrary.DeliveryDays),
                new MySqlParameter("@status", deliveryLibrary.Status),
                new MySqlParameter("@isCODActive", deliveryLibrary.IsCODActive),
                new MySqlParameter("@createdby", deliveryLibrary.CreatedBy),
                new MySqlParameter("@createdat", deliveryLibrary.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", deliveryLibrary.Id),
                new MySqlParameter("@countryid", deliveryLibrary.CountryID),
                new MySqlParameter("@stateid", deliveryLibrary.StateID),
                new MySqlParameter("@cityid", deliveryLibrary.CityID),
                new MySqlParameter("@locality", deliveryLibrary.Locality),
                new MySqlParameter("@pincode", deliveryLibrary.Pincode),
                new MySqlParameter("@deliveryDays", deliveryLibrary.DeliveryDays),
                new MySqlParameter("@status", deliveryLibrary.Status),
                new MySqlParameter("@isCODActive", deliveryLibrary.IsCODActive),
                new MySqlParameter("@modifiedby", deliveryLibrary.ModifiedBy),
                new MySqlParameter("@modifiedat", deliveryLibrary.ModifiedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", deliveryLibrary.Id),
                new MySqlParameter("@deletedby", deliveryLibrary.DeletedBy),
                new MySqlParameter("@deletedat", deliveryLibrary.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", deliveryLibrary.Id),
                new MySqlParameter("@countryid", deliveryLibrary.CountryID),
                new MySqlParameter("@stateid", deliveryLibrary.StateID),
                new MySqlParameter("@cityid", deliveryLibrary.CityID),
                new MySqlParameter("@searchtext", deliveryLibrary.searchText),
                new MySqlParameter("@locality", deliveryLibrary.Locality),
                new MySqlParameter("@pincode", deliveryLibrary.Pincode),
                new MySqlParameter("@countryname", deliveryLibrary.CountryName),
                new MySqlParameter("@statename", deliveryLibrary.StateName),
                new MySqlParameter("@cityname", deliveryLibrary.CityName),
                new MySqlParameter("@status", deliveryLibrary.Status),
                new MySqlParameter("@isdeleted", deliveryLibrary.IsDeleted),
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
