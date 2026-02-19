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
    public class GSTInfoRepository : IGSTInfoRepository
    {
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public GSTInfoRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(GSTInfo gSTInfo)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode","add"),
                    new MySqlParameter("@userid",gSTInfo.UserID),
                    new MySqlParameter("@gstno",gSTInfo.GSTNo),
                    new MySqlParameter("@legalname",gSTInfo.LegalName),
                    new MySqlParameter("@tradename",gSTInfo.TradeName),
                    new MySqlParameter("@gsttype",gSTInfo.GSTType),
                    new MySqlParameter("@gstdoc",gSTInfo.GSTDoc),
                    new MySqlParameter("@add1",gSTInfo.RegisteredAddressLine1),
                    new MySqlParameter("@add2",gSTInfo.RegisteredAddressLine2),
                    new MySqlParameter("@landmark",gSTInfo.RegisteredLandmark),
                    new MySqlParameter("@pincode",gSTInfo.RegisteredPincode),
                    new MySqlParameter("@regcityid",gSTInfo.RegisteredCityId),
                    new MySqlParameter("@regstateid",gSTInfo.RegisteredStateId),
                    new MySqlParameter("@regcountryid",gSTInfo.RegisteredCountryId),
                    new MySqlParameter("@tcsno",gSTInfo.TCSNo),
                    new MySqlParameter("@status",gSTInfo.Status),
                    new MySqlParameter("@isHeadOffice",gSTInfo.IsHeadOffice),
                    new MySqlParameter("@createdBy",gSTInfo.CreatedBy),
                    new MySqlParameter("@createdAt",gSTInfo.CreatedAt),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GSTInfo, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BaseResponse<long>> Update(GSTInfo gSTInfo)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@userid",gSTInfo.UserID),
                    new MySqlParameter("@gstno",gSTInfo.GSTNo),
                    new MySqlParameter("@legalname",gSTInfo.LegalName),
                    new MySqlParameter("@tradename",gSTInfo.TradeName),
                    new MySqlParameter("@gsttype",gSTInfo.GSTType),
                    new MySqlParameter("@gstdoc",gSTInfo.GSTDoc),
                    new MySqlParameter("@add1",gSTInfo.RegisteredAddressLine1),
                    new MySqlParameter("@add2",gSTInfo.RegisteredAddressLine2),
                    new MySqlParameter("@landmark",gSTInfo.RegisteredLandmark),
                    new MySqlParameter("@pincode",gSTInfo.RegisteredPincode),
                    new MySqlParameter("@regcityid",gSTInfo.RegisteredCityId),
                    new MySqlParameter("@regstateid",gSTInfo.RegisteredStateId),
                    new MySqlParameter("@regcountryid",gSTInfo.RegisteredCountryId),
                    new MySqlParameter("@tcsno",gSTInfo.TCSNo),
                    new MySqlParameter("@status",gSTInfo.Status),
                    new MySqlParameter("@isHeadOffice",gSTInfo.IsHeadOffice),
                    new MySqlParameter("@modifiedBy", gSTInfo.ModifiedBy),
                    new MySqlParameter("@modifiedAt", gSTInfo.ModifiedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GSTInfo, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BaseResponse<long>> Delete(GSTInfo gSTInfo)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", gSTInfo.Id),
                    new MySqlParameter("@userid", gSTInfo.UserID),
                    new MySqlParameter("@deletedBy", gSTInfo.DeletedBy),
                    new MySqlParameter("@deletedAt", gSTInfo.DeletedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GSTInfo, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<GSTInfo>>> Get(GSTInfo gSTInfo, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", Mode),
                    new MySqlParameter("@id", gSTInfo.Id),
                    new MySqlParameter("@userid", gSTInfo.UserID),
                    new MySqlParameter("@gstno", gSTInfo.GSTNo),
                    new MySqlParameter("@pincode", gSTInfo.RegisteredPincode),
                    new MySqlParameter("@regcityid", gSTInfo.RegisteredCityId),
                    new MySqlParameter("@regstateid", gSTInfo.RegisteredStateId),
                    new MySqlParameter("@regcountryid", gSTInfo.RegisteredCountryId),
                    new MySqlParameter("@searchtext", gSTInfo.searchText),
                    new MySqlParameter("@status", gSTInfo.Status),
                    new MySqlParameter("@statename", gSTInfo.StateName),
                    new MySqlParameter("@cityname", gSTInfo.CityName),
                    new MySqlParameter("@countryname", gSTInfo.CountryName),
                    new MySqlParameter("@isDeleted", gSTInfo.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetGSTInfo, GSTInfoParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<GSTInfo>> GSTInfoParserAsync(DbDataReader reader)
        {
            List<GSTInfo> lstGSTInfo = new List<GSTInfo>();
            while (await reader.ReadAsync())
            {
                lstGSTInfo.Add(new GSTInfo()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    UserID = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserID")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserID"))),
                    GSTNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTNo"))),
                    LegalName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("LegalName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LegalName"))),
                    TradeName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TradeName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TradeName"))),
                    GSTType = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTType")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTType"))),
                    GSTDoc = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTDoc")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTDoc"))),
                    RegisteredAddressLine1 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine1")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine1"))),
                    RegisteredAddressLine2 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine2")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine2"))),
                    RegisteredLandmark = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredLandmark")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredLandmark"))),
                    RegisteredPincode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredPincode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredPincode"))),
                    RegisteredStateId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredStateID")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RegisteredStateID"))),
                    RegisteredCityId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredCityId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RegisteredCityId"))),
                    RegisteredCountryId = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredCountryId")).ToString()) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RegisteredCountryId"))),
                    TCSNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TCSNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TCSNo"))),
                    Status = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Status")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    CreatedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CreatedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    IsHeadOffice = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("IsHeadOffice")).ToString()) ? false : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsHeadOffice"))),
                    CountryName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CountryName"))),
                    StateName = Convert.ToString(reader.GetValue(reader.GetOrdinal("StateName"))),
                    CityName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CityName"))),

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
            return lstGSTInfo;
        }
    }
}   
