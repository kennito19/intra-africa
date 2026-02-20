using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;
using User.Infrastructure.Helper;
using User.Application.IRepositories;
using System.Data.Common;

namespace User.Infrastructure.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly MySqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public BrandRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }


        public async Task<BaseResponse<long>> Create(Brand brand)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode","add"),
                    new MySqlParameter("@id",brand.ID),
                    new MySqlParameter("@name",brand.Name),
                    new MySqlParameter("@description",brand.Description),
                    new MySqlParameter("@status",brand.Status),
                    new MySqlParameter("@logo",brand.Logo),
                    new MySqlParameter("@createdBy", brand.CreatedBy),
                    new MySqlParameter("@createdAt", brand.CreatedAt),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Brand, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BaseResponse<long>> Update(Brand brand)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@id",brand.ID),
                    new MySqlParameter("@name",brand.Name),
                    new MySqlParameter("@description",brand.Description),
                    new MySqlParameter("@status",brand.Status),
                    new MySqlParameter("@logo",brand.Logo),
                    new MySqlParameter("@createdBy", brand.CreatedBy),
                    new MySqlParameter("@createdAt", brand.CreatedAt),
                    new MySqlParameter("@modifiedBy", brand.ModifiedBy),
                    new MySqlParameter("@modifiedAt", brand.ModifiedAt),
                    new MySqlParameter("@isDeleted", brand.IsDeleted),
                    new MySqlParameter("@deletedby", brand.DeletedBy),
                    new MySqlParameter("@deletedat", brand.DeletedAt)

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Brand, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> Delete(Brand brand)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "delete"),
                    new MySqlParameter("@id", brand.ID),
                    new MySqlParameter("@deletedBy", brand.DeletedBy),
                    new MySqlParameter("@deletedAt", brand.DeletedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Brand, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<List<Brand>>> Get(Brand brand, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", Mode),
                    new MySqlParameter("@id", brand.ID),
                    new MySqlParameter("@ids", brand.BrandIds),
                    new MySqlParameter("@name", brand.Name),
                    new MySqlParameter("@status", brand.Status),
                    new MySqlParameter("@searchtext", brand.searchText),
                    new MySqlParameter("@isDeleted", brand.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetBrand, BrandParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<Brand>> BrandParserAsync(DbDataReader reader)
        {
            List<Brand> lstBrand = new List<Brand>();
            while (await reader.ReadAsync())
            {
                lstBrand.Add(new Brand()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    ID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),

                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    Description = Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),
                    Logo = Convert.ToString(reader.GetValue(reader.GetOrdinal("Logo"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),

                    CreatedBy = Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("ModifiedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedBy")).ToString()) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(reader.GetValue(reader.GetOrdinal("DeletedAt")).ToString()) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),


                });
            }
            return lstBrand;
        }

    }
}
