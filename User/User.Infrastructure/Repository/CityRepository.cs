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
    public class CityRepository : ICityRepository
    {
        private readonly MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public CityRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(CityLibrary cityLibrary)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode","add"),
                    new MySqlParameter("@countryid",cityLibrary.CountryID),
                    new MySqlParameter("@stateid",cityLibrary.StateID),
                    new MySqlParameter("@name",cityLibrary.Name),
                    new MySqlParameter("@status",cityLibrary.Status),
                    new MySqlParameter("@createdBy", cityLibrary.CreatedBy),
                    new MySqlParameter("@createdAt", cityLibrary.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.City, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> Update(CityLibrary cityLibrary)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id", cityLibrary.Id),
                    new MySqlParameter("@countryid",cityLibrary.CountryID),
                    new MySqlParameter("@stateid",cityLibrary.StateID),
                    new MySqlParameter("@name", cityLibrary.Name),
                    new MySqlParameter("@status", cityLibrary.Status),
                    new MySqlParameter("@modifiedBy", cityLibrary.ModifiedBy),
                    new MySqlParameter("@modifiedAt", cityLibrary.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.City, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> Delete(CityLibrary cityLibrary)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", cityLibrary.Id),
                    new MySqlParameter("@deletedBy", cityLibrary.DeletedBy),
                    new MySqlParameter("@deletedAt", cityLibrary.DeletedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.City, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<CityLibrary>>> Get(CityLibrary cityLibrary, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", Mode),
                    new MySqlParameter("@id", cityLibrary.Id),
                    new MySqlParameter("@countryid", cityLibrary.CountryID),
                    new MySqlParameter("@stateid", cityLibrary.StateID),
                    new MySqlParameter("@name", cityLibrary.Name),
                    new MySqlParameter("@searchtext", cityLibrary.searchText),
                    new MySqlParameter("@status", cityLibrary.Status),
                    new MySqlParameter("@isDeleted", cityLibrary.IsDeleted),
                    new MySqlParameter("@countryname", cityLibrary.CountryName),
                    new MySqlParameter("@statename", cityLibrary.StateName),
                    new MySqlParameter("@stateids", cityLibrary.stateIds),
                    new MySqlParameter("@countryids", cityLibrary.countryIds),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetCity, CityParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<CityLibrary>> CityParserAsync(DbDataReader reader)
        {
            List<CityLibrary> lstcityLibraries = new List<CityLibrary>();
            while (await reader.ReadAsync())
            {
                lstcityLibraries.Add(new CityLibrary()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    CountryID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CountryId"))),
                    StateID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("StateId"))),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    CountryName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CountryName"))),
                    StateName = Convert.ToString(reader.GetValue(reader.GetOrdinal("StateName"))),
                });
            }
            return lstcityLibraries;
        }
    }
}
