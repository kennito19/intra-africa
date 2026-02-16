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
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode","add"),
                    new SqlParameter("@userid",warehouse.UserID),
                    new SqlParameter("@gstinfoid",warehouse.GSTInfoId),
                    new SqlParameter("@name",warehouse.Name),
                    new SqlParameter("@contactpersonname",warehouse.ContactPersonName),
                    new SqlParameter("@contactpersonno",warehouse.ContactPersonMobileNo),
                    new SqlParameter("@add1",warehouse.AddressLine1),
                    new SqlParameter("@add2",warehouse.AddressLine2),
                    new SqlParameter("@landmark",warehouse.Landmark),
                    new SqlParameter("@pincode",warehouse.Pincode),
                    new SqlParameter("@cityid",warehouse.CityId),
                    new SqlParameter("@stateid",warehouse.StateId),
                    new SqlParameter("@countryid",warehouse.CountryId),
                    new SqlParameter("@status",warehouse.Status),
                    new SqlParameter("@createdBy",warehouse.CreatedBy),
                    new SqlParameter("@createdAt",warehouse.CreatedAt),
                    
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
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@id", warehouse.Id),
                    new SqlParameter("@name",warehouse.Name),
                    new SqlParameter("@contactpersonname",warehouse.ContactPersonName),
                    new SqlParameter("@contactpersonno",warehouse.ContactPersonMobileNo),
                    new SqlParameter("@add1",warehouse.AddressLine1),
                    new SqlParameter("@add2",warehouse.AddressLine2),
                    new SqlParameter("@landmark",warehouse.Landmark),
                    new SqlParameter("@pincode",warehouse.Pincode),
                    new SqlParameter("@cityid",warehouse.CityId),
                    new SqlParameter("@stateid",warehouse.StateId),
                    new SqlParameter("@countryid",warehouse.CountryId),
                    new SqlParameter("@status",warehouse.Status),
                    new SqlParameter("@modifiedBy", warehouse.ModifiedBy),
                    new SqlParameter("@modifiedAt", warehouse.ModifiedAt)
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
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id", warehouse.Id),
                    new SqlParameter("@userid", warehouse.UserID),
                    new SqlParameter("@gstinfoid", warehouse.GSTInfoId),
                    new SqlParameter("@deletedBy", warehouse.DeletedBy),
                    new SqlParameter("@deletedAt", warehouse.DeletedAt)
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
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", Mode),
                    new SqlParameter("@id", warehouse.Id),
                    new SqlParameter("@userid", warehouse.UserID),
                    new SqlParameter("@gstinfoid", warehouse.GSTInfoId),
                    new SqlParameter("@gstno", warehouse.GSTNo),
                    new SqlParameter("@searchtext", warehouse.searchText),
                    new SqlParameter("@name", warehouse.Name),
                    new SqlParameter("@contactpersonname", warehouse.ContactPersonName),
                    new SqlParameter("@contactpersonno", warehouse.ContactPersonMobileNo),
                    new SqlParameter("@pincode", warehouse.Pincode),
                    new SqlParameter("@cityid", warehouse.CityId),
                    new SqlParameter("@stateid", warehouse.StateId),
                    new SqlParameter("@countryid", warehouse.CountryId),
                    new SqlParameter("@status", warehouse.Status),
                    new SqlParameter("@statename", warehouse.StateName),
                    new SqlParameter("@cityname", warehouse.CityName),
                    new SqlParameter("@countryname", warehouse.CountryName),
                    new SqlParameter("@isDeleted", warehouse.IsDeleted),
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
