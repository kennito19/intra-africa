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
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode","add"),
                    new SqlParameter("@userid",gSTInfo.UserID),
                    new SqlParameter("@gstno",gSTInfo.GSTNo),
                    new SqlParameter("@legalname",gSTInfo.LegalName),
                    new SqlParameter("@tradename",gSTInfo.TradeName),
                    new SqlParameter("@gsttype",gSTInfo.GSTType),
                    new SqlParameter("@gstdoc",gSTInfo.GSTDoc),
                    new SqlParameter("@add1",gSTInfo.RegisteredAddressLine1),
                    new SqlParameter("@add2",gSTInfo.RegisteredAddressLine2),
                    new SqlParameter("@landmark",gSTInfo.RegisteredLandmark),
                    new SqlParameter("@pincode",gSTInfo.RegisteredPincode),
                    new SqlParameter("@regcityid",gSTInfo.RegisteredCityId),
                    new SqlParameter("@regstateid",gSTInfo.RegisteredStateId),
                    new SqlParameter("@regcountryid",gSTInfo.RegisteredCountryId),
                    new SqlParameter("@tcsno",gSTInfo.TCSNo),
                    new SqlParameter("@status",gSTInfo.Status),
                    new SqlParameter("@isHeadOffice",gSTInfo.IsHeadOffice),
                    new SqlParameter("@createdBy",gSTInfo.CreatedBy),
                    new SqlParameter("@createdAt",gSTInfo.CreatedAt),

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
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "update"),
                    new SqlParameter("@userid",gSTInfo.UserID),
                    new SqlParameter("@gstno",gSTInfo.GSTNo),
                    new SqlParameter("@legalname",gSTInfo.LegalName),
                    new SqlParameter("@tradename",gSTInfo.TradeName),
                    new SqlParameter("@gsttype",gSTInfo.GSTType),
                    new SqlParameter("@gstdoc",gSTInfo.GSTDoc),
                    new SqlParameter("@add1",gSTInfo.RegisteredAddressLine1),
                    new SqlParameter("@add2",gSTInfo.RegisteredAddressLine2),
                    new SqlParameter("@landmark",gSTInfo.RegisteredLandmark),
                    new SqlParameter("@pincode",gSTInfo.RegisteredPincode),
                    new SqlParameter("@regcityid",gSTInfo.RegisteredCityId),
                    new SqlParameter("@regstateid",gSTInfo.RegisteredStateId),
                    new SqlParameter("@regcountryid",gSTInfo.RegisteredCountryId),
                    new SqlParameter("@tcsno",gSTInfo.TCSNo),
                    new SqlParameter("@status",gSTInfo.Status),
                    new SqlParameter("@isHeadOffice",gSTInfo.IsHeadOffice),
                    new SqlParameter("@modifiedBy", gSTInfo.ModifiedBy),
                    new SqlParameter("@modifiedAt", gSTInfo.ModifiedAt)
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
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "delete"),
                    new SqlParameter("@id", gSTInfo.Id),
                    new SqlParameter("@userid", gSTInfo.UserID),
                    new SqlParameter("@deletedBy", gSTInfo.DeletedBy),
                    new SqlParameter("@deletedAt", gSTInfo.DeletedAt)
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
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", Mode),
                    new SqlParameter("@id", gSTInfo.Id),
                    new SqlParameter("@userid", gSTInfo.UserID),
                    new SqlParameter("@gstno", gSTInfo.GSTNo),
                    new SqlParameter("@pincode", gSTInfo.RegisteredPincode),
                    new SqlParameter("@regcityid", gSTInfo.RegisteredCityId),
                    new SqlParameter("@regstateid", gSTInfo.RegisteredStateId),
                    new SqlParameter("@regcountryid", gSTInfo.RegisteredCountryId),
                    new SqlParameter("@searchtext", gSTInfo.searchText),
                    new SqlParameter("@status", gSTInfo.Status),
                    new SqlParameter("@statename", gSTInfo.StateName),
                    new SqlParameter("@cityname", gSTInfo.CityName),
                    new SqlParameter("@countryname", gSTInfo.CountryName),
                    new SqlParameter("@isDeleted", gSTInfo.IsDeleted),
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
