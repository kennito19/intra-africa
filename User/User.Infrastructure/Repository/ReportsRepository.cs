using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.DTO;
using User.Domain;
using User.Infrastructure.Helper;
using User.Application.IRepositories;

namespace User.Infrastructure.Repository
{
    public class ReportsRepository : IReportsRepository
    {
        private readonly MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public ReportsRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<List<WarehouseReport>>> GetWarehouseReport(string? SellerId = null, string? fromDate = null, string? toDate = null)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "Warehouse"),
                    new MySqlParameter("@sellerId", SellerId),
                    new MySqlParameter("@fromdate", fromDate),
                    new MySqlParameter("@todate",toDate),
                    
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Reports, GetWarehouseReportParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<WarehouseReport>> GetWarehouseReportParserAsync(DbDataReader reader)
        {
            List<WarehouseReport> lstKYCDetails = new List<WarehouseReport>();
            while (await reader.ReadAsync())
            {
                lstKYCDetails.Add(new WarehouseReport()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    UserID = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserID")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserID"))),
                    DisplayName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DisplayName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DisplayName"))),
                    OwnerName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("OwnerName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OwnerName"))),
                    TradeName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TradeName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TradeName"))),
                    LegalName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("LegalName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LegalName"))),
                    GSTNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTNo"))),
                    GSTType = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTType")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTType"))),
                    WareHouseName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("WareHouseName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("WareHouseName"))),
                    ContactPersonName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ContactPersonName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ContactPersonName"))),
                    ContactPersonMobileNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ContactPersonMobileNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ContactPersonMobileNo"))),
                    AddressLine1 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AddressLine1")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AddressLine1"))),
                    AddressLine2 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("AddressLine2")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("AddressLine2"))),
                    Landmark = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Landmark")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Landmark"))),
                    Pincode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Pincode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Pincode"))),
                    City = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("City")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("City"))),
                    State = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("State")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("State"))),
                    Country = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Country")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Country"))),
                    RegisteredAddressLine1 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine1")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine1"))),
                    RegisteredAddressLine2 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine2")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine2"))),
                    RegisteredLandmark = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredLandmark")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredLandmark"))),
                    RegisteredPincode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredPincode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredPincode"))),
                    RegisteredCity = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredCity")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredCity"))),
                    RegisteredState = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredState")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredState"))),
                    RegisteredCountry = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredCountry")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredCountry"))),
                    Status = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Status")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    CreatedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CreatedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    
                });
            }
            return lstKYCDetails;
        }

        public async Task<BaseResponse<List<GSTReport>>> GetGSTReport(string? SellerId = null, string? fromDate = null, string? toDate = null)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "GSTReport"),
                    new MySqlParameter("@sellerId", SellerId),
                    new MySqlParameter("@fromdate", fromDate),
                    new MySqlParameter("@todate",toDate),

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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Reports, GetGSTReportParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<GSTReport>> GetGSTReportParserAsync(DbDataReader reader)
        {
            List<GSTReport> lstKYCDetails = new List<GSTReport>();
            while (await reader.ReadAsync())
            {
                lstKYCDetails.Add(new GSTReport()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    UserID = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("UserID")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("UserID"))),
                    DisplayName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DisplayName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DisplayName"))),
                    OwnerName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("OwnerName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("OwnerName"))),
                    TradeName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("TradeName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("TradeName"))),
                    LegalName = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("LegalName")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("LegalName"))),
                    GSTNo = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTNo")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTNo"))),
                    GSTType = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTType")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTType"))),
                    City = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("City")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("City"))),
                    State = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("State")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("State"))),
                    Country = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("Country")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Country"))),
                    RegisteredAddressLine1 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine1")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine1"))),
                    RegisteredAddressLine2 = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine2")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredAddressLine2"))),
                    RegisteredLandmark = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredLandmark")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredLandmark"))),
                    RegisteredPincode = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("RegisteredPincode")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("RegisteredPincode"))),
                    GSTStatus = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("GSTStatus")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("GSTStatus"))),
                    CreatedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("CreatedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),

                });
            }
            return lstKYCDetails;
        }
    }
}
