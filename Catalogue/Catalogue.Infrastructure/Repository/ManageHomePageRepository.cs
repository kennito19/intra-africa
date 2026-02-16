using Catalogue.Application.IRepositories;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class ManageHomePageRepository : IManageHomePageRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;
        public ManageHomePageRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);
            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(ManageHomePage manageHomePage)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                   new SqlParameter("@mode","add"),
                   new SqlParameter("@name",manageHomePage.Name),
                   new SqlParameter("@status",manageHomePage.Status),
                   new SqlParameter("@createdat", manageHomePage.CreatedAt),
                   new SqlParameter("@createdby", manageHomePage.CreatedBy)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageHomePage, output, newid, message, sqlParams.ToArray());


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(ManageHomePage manageHomePage)
        {

            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                   new SqlParameter("@mode","update"),
                   new SqlParameter("@id",manageHomePage.Id),
                   new SqlParameter("@name",manageHomePage.Name),
                   new SqlParameter("@status",manageHomePage.Status),
                   new SqlParameter("@modifiedat", manageHomePage.ModifiedAt),
                   new SqlParameter("@modifiedby", manageHomePage.ModifiedBy),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageHomePage, output, newid, message, sqlParams.ToArray());


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Delete(ManageHomePage manageHomePage)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", manageHomePage.Id)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageHomePage, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ManageHomePage>>> get(ManageHomePage manageHomePage, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", manageHomePage.Id),
                new SqlParameter("@name", manageHomePage.Name),
                new SqlParameter("@status", manageHomePage.Status)

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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageHomePage, ManageHomePageParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ManageHomePage>> ManageHomePageParserAsync(DbDataReader reader)
        {
            List<ManageHomePage> manageHomePages = new List<ManageHomePage>();
            while (await reader.ReadAsync())
            {
                manageHomePages.Add(new ManageHomePage()
                {
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Name = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Name")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    Status = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Status")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),

                });
            }
            return manageHomePages;
        }

    }
}
