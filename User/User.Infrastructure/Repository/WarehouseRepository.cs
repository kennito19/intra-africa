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

    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public WarehouseRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(Warehouse warehouse)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode","add"),
                    new MySqlParameter("@userid",warehouse.UserID),
                    new MySqlParameter("@gstinfoid",warehouse.GSTInfoId),
                    new MySqlParameter("@name",warehouse.Name),
                    new MySqlParameter("@contactpersonname",warehouse.ContactPersonName),
                    new MySqlParameter("@contactpersonno",warehouse.ContactPersonMobileNo),
                    new MySqlParameter("@add1",warehouse.AddressLine1),
                    new MySqlParameter("@add2",warehouse.AddressLine2),
                    new MySqlParameter("@landmark",warehouse.Landmark),
                    new MySqlParameter("@pincode",warehouse.Pincode),
                    new MySqlParameter("@cityid",warehouse.CityId),
                    new MySqlParameter("@stateid",warehouse.StateId),
                    new MySqlParameter("@countryid",warehouse.CountryId),
                    new MySqlParameter("@status",warehouse.Status),
                    new MySqlParameter("@createdBy",warehouse.CreatedBy),
                    new MySqlParameter("@createdAt",warehouse.CreatedAt),
                    
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Warehouse, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BaseResponse<long>> Update(Warehouse warehouse)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id", warehouse.Id),
                    new MySqlParameter("@name",warehouse.Name),
                    new MySqlParameter("@contactpersonname",warehouse.ContactPersonName),
                    new MySqlParameter("@contactpersonno",warehouse.ContactPersonMobileNo),
                    new MySqlParameter("@add1",warehouse.AddressLine1),
                    new MySqlParameter("@add2",warehouse.AddressLine2),
                    new MySqlParameter("@landmark",warehouse.Landmark),
                    new MySqlParameter("@pincode",warehouse.Pincode),
                    new MySqlParameter("@cityid",warehouse.CityId),
                    new MySqlParameter("@stateid",warehouse.StateId),
                    new MySqlParameter("@countryid",warehouse.CountryId),
                    new MySqlParameter("@status",warehouse.Status),
                    new MySqlParameter("@modifiedBy", warehouse.ModifiedBy),
                    new MySqlParameter("@modifiedAt", warehouse.ModifiedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Warehouse, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BaseResponse<long>> Delete(Warehouse warehouse)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", warehouse.Id),
                    new MySqlParameter("@userid", warehouse.UserID),
                    new MySqlParameter("@gstinfoid", warehouse.GSTInfoId),
                    new MySqlParameter("@deletedBy", warehouse.DeletedBy),
                    new MySqlParameter("@deletedAt", warehouse.DeletedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Warehouse, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<Warehouse>>> Get(Warehouse warehouse, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", Mode),
                    new MySqlParameter("@id", warehouse.Id),
                    new MySqlParameter("@userid", warehouse.UserID),
                    new MySqlParameter("@gstinfoid", warehouse.GSTInfoId),
                    new MySqlParameter("@gstno", warehouse.GSTNo),
                    new MySqlParameter("@searchtext", warehouse.searchText),
                    new MySqlParameter("@name", warehouse.Name),
                    new MySqlParameter("@contactpersonname", warehouse.ContactPersonName),
                    new MySqlParameter("@contactpersonno", warehouse.ContactPersonMobileNo),
                    new MySqlParameter("@pincode", warehouse.Pincode),
                    new MySqlParameter("@cityid", warehouse.CityId),
                    new MySqlParameter("@stateid", warehouse.StateId),
                    new MySqlParameter("@countryid", warehouse.CountryId),
                    new MySqlParameter("@status", warehouse.Status),
                    new MySqlParameter("@statename", warehouse.StateName),
                    new MySqlParameter("@cityname", warehouse.CityName),
                    new MySqlParameter("@countryname", warehouse.CountryName),
                    new MySqlParameter("@isDeleted", warehouse.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetWarehouse, WarehouseParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<Warehouse>> WarehouseParserAsync(DbDataReader reader)
        {
            List<Warehouse> lstWarehouse = new List<Warehouse>();
            while (await reader.ReadAsync())
            {
                lstWarehouse.Add(new Warehouse()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    UserID = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserID")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserID"))),
                    GSTInfoId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTInfoId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("GSTInfoId"))),
                    Name = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Name")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    ContactPersonName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ContactPersonName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ContactPersonName"))),
                    ContactPersonMobileNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ContactPersonMobileNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ContactPersonMobileNo"))),
                    AddressLine1 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AddressLine1")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AddressLine1"))),
                    AddressLine2 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AddressLine2")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AddressLine2"))),
                    Landmark = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Landmark")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Landmark"))),
                    Pincode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Pincode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Pincode"))),
                    StateId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("StateId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("StateId"))),
                    CityId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CityId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CityId"))),
                    CountryId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CountryId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CountryId"))),
                    Status = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Status")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    CreatedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CreatedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    CountryName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CountryName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CountryName"))),
                    StateName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("StateName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("StateName"))),
                    CityName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CityName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CityName"))),
                    GSTNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTNo"))),

                    FirstName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("FirstName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("FirstName"))),
                    LastName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("LastName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LastName"))),
                    UserStatus = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserStatus")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserStatus"))),
                    ProfileImage = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ProfileImage")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ProfileImage"))),
                    Email = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Email")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Email"))),
                    Gender = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Gender")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Gender"))),
                    Phone = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Phone")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Phone"))),
                    IsEmailConfirmed = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsEmailConfirmed")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsEmailConfirmed"))),
                    IsPhoneConfirmed = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsPhoneConfirmed")).ToString()) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsPhoneConfirmed"))),
                });
            }
            return lstWarehouse;
        }
    }
}
