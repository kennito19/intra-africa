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
    public class AddressRepository : IAddressRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public AddressRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(Address address)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode","add"),
                    new SqlParameter("@userid",address.UserId),
                    new SqlParameter("@addressType",address.AddressType),
                    new SqlParameter("@fullName",address.FullName),
                    new SqlParameter("@mobileNo",address.MobileNo),
                    new SqlParameter("@add1",address.AddressLine1),
                    new SqlParameter("@add2",address.AddressLine2),
                    new SqlParameter("@landmark",address.Landmark),
                    new SqlParameter("@pincode",address.Pincode),
                    new SqlParameter("@stateid",address.StateId),
                    new SqlParameter("@cityid",address.CityId),
                    new SqlParameter("@countryid",address.CountryId),
                    new SqlParameter("@gstNo",address.GSTNo),
                    new SqlParameter("@status",address.Status),
                    new SqlParameter("@setDefault",address.SetDefault),
                    new SqlParameter("@createdBy",address.CreatedBy),
                    new SqlParameter("@createdAt",address.CreatedAt),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Address, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<BaseResponse<long>> Update(Address address)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@id", address.Id),
                    new SqlParameter("@userid",address.UserId),
                    new SqlParameter("@addressType",address.AddressType),
                    new SqlParameter("@fullName",address.FullName),
                    new SqlParameter("@mobileNo",address.MobileNo),
                    new SqlParameter("@add1",address.AddressLine1),
                    new SqlParameter("@add2",address.AddressLine2),
                    new SqlParameter("@landmark",address.Landmark),
                    new SqlParameter("@pincode",address.Pincode),
                    new SqlParameter("@stateid",address.StateId),
                    new SqlParameter("@cityid",address.CityId),
                    new SqlParameter("@countryid",address.CountryId),
                    new SqlParameter("@gstNo",address.GSTNo),
                    new SqlParameter("@status",address.Status),
                    new SqlParameter("@setDefault",address.SetDefault),
                    new SqlParameter("@modifiedBy", address.ModifiedBy),
                    new SqlParameter("@modifiedAt", address.ModifiedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Address, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> Delete(Address address)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id", address.Id)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Address, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<Address>>> Get(Address address, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", Mode),
                    new SqlParameter("@id", address.Id),
                    new SqlParameter("@userid", address.UserId),
                    new SqlParameter("@fullName", address.FullName),
                    new SqlParameter("@mobileNo", address.MobileNo),
                    new SqlParameter("@pincode", address.Pincode),
                    new SqlParameter("@cityid", address.CityId),
                    new SqlParameter("@stateid", address.StateId),
                    new SqlParameter("@countryid", address.CountryId),
                    new SqlParameter("@status", address.Status),
                    new SqlParameter("@statename", address.StateName),
                    new SqlParameter("@cityname", address.CityName),
                    new SqlParameter("@countryname", address.CountryName),
                    new SqlParameter("@searchtext", address.searchText),
                    new SqlParameter("@setDefault", address.SetDefault),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetAddress, AddressParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<Address>> AddressParserAsync(DbDataReader reader)
        {
            List<Address> lstAddress = new List<Address>();
            while (await reader.ReadAsync())
            {
                lstAddress.Add(new Address()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    UserId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserId")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId"))),
                    AddressType = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AddressType")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AddressType"))),
                    FullName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("FullName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("FullName"))),
                    MobileNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("MobileNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MobileNo"))),
                    AddressLine1 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AddressLine1")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AddressLine1"))),
                    AddressLine2 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AddressLine2")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AddressLine2"))),
                    Landmark = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Landmark")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Landmark"))),
                    Pincode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Pincode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Pincode"))),
                    StateId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("StateId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("StateId"))),
                    CityId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CityId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CityId"))),
                    CountryId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CountryId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CountryId"))),
                    GSTNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTNo"))),
                    Status = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Status")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    CreatedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CreatedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CreatedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    SetDefault = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("SetDefault")).ToString()) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("SetDefault"))),
                    CountryName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CountryName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CountryName"))),
                    StateName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("StateName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("StateName"))),
                    CityName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CityName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CityName"))),

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
            return lstAddress;
        }

    }
}
