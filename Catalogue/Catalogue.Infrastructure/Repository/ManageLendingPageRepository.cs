using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Catalogue.Infrastructure.Repository
{
    public class ManageLendingPageRepository : IManageLendingPageRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;

        public ManageLendingPageRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(LendingPageLibrary lendingPage)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "add"),
                    new MySqlParameter("@name", lendingPage.Name),
                    new MySqlParameter("@link", lendingPage.Link),
                    new MySqlParameter("@sequence", lendingPage.Sequence),
                    new MySqlParameter("@status", lendingPage.Status),
                    new MySqlParameter("@createdby", lendingPage.CreatedBy),
                new MySqlParameter("@createdat", lendingPage.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.LendingPage, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(LendingPageLibrary lendingPage)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", lendingPage.Id),
                new MySqlParameter("@name", lendingPage.Name),
                new MySqlParameter("@link", lendingPage.Link),
                new MySqlParameter("@sequence", lendingPage.Sequence),
                new MySqlParameter("@status", lendingPage.Status),
                new MySqlParameter("@modifiedby", lendingPage.ModifiedBy),
                new MySqlParameter("@modifiedat", lendingPage.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.LendingPage, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(LendingPageLibrary lendingPage)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", lendingPage.Id),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.LendingPage, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<LendingPageLibrary>>> get(LendingPageLibrary lendingPage, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", lendingPage.Id),
                new MySqlParameter("@name", lendingPage.Name),
                new MySqlParameter("@searchtext", lendingPage.SearchText),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetLendingPage, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<LendingPageLibrary>> LayoutParserAsync(DbDataReader reader)
        {
            List<LendingPageLibrary> lendingPage = new List<LendingPageLibrary>();
            while (await reader.ReadAsync())
            {
                lendingPage.Add(new LendingPageLibrary()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    Link = Convert.ToString(reader.GetValue(reader.GetOrdinal("Link"))),
                    Sequence = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Sequence"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                });
            }
            return lendingPage;
        }
    }
}
