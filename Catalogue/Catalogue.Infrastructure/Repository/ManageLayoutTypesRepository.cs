using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Catalogue.Infrastructure.Repository
{
    public class ManageLayoutTypesRepository : IManageLayoutTypesRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public ManageLayoutTypesRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(ManageLayoutTypesLibrary layouts)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "add"),
                    new SqlParameter("@layoutId", layouts.LayoutId),
                    new SqlParameter("@name", layouts.Name),
                    new SqlParameter("@image", layouts.ImageUrl),
                    new SqlParameter("@options", layouts.Options),
                    new SqlParameter("@className", layouts.ClassName),
                    new SqlParameter("@hasInnerColumns", layouts.HasInnerColumns),
                    new SqlParameter("@columns", layouts.Columns),
                    new SqlParameter("@minImage", layouts.MinImage),
                    new SqlParameter("@maxImage", layouts.MaxImage),
                    new SqlParameter("@createdby", layouts.CreatedBy),
                new SqlParameter("@createdat", layouts.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageLayoutTypes, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(ManageLayoutTypesLibrary layouts)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", layouts.Id),
                new SqlParameter("@layoutId", layouts.LayoutId),
                new SqlParameter("@name", layouts.Name),
                new SqlParameter("@image", layouts.ImageUrl),
                new SqlParameter("@options", layouts.Options),
                new SqlParameter("@className", layouts.ClassName),
                new SqlParameter("@hasInnerColumns", layouts.HasInnerColumns),
                new SqlParameter("@columns", layouts.Columns),
                new SqlParameter("@minImage", layouts.MinImage),
                new SqlParameter("@maxImage", layouts.MaxImage),
                new SqlParameter("@modifiedby", layouts.ModifiedBy),
                new SqlParameter("@modifiedat", layouts.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageLayoutTypes, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageLayoutTypesLibrary layouts)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", layouts.Id),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageLayoutTypes, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ManageLayoutTypesLibrary>>> get(ManageLayoutTypesLibrary layouts, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", layouts.Id),
                new SqlParameter("@layoutId", layouts.LayoutId),
                new SqlParameter("@name", layouts.Name),
                new SqlParameter("@layoutName", layouts.LayoutName),
                new SqlParameter("@searchtext", layouts.Searchtext),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageLayoutTypes, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ManageLayoutTypesLibrary>> LayoutParserAsync(DbDataReader reader)
        {
            List<ManageLayoutTypesLibrary> lstLayouts = new List<ManageLayoutTypesLibrary>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new ManageLayoutTypesLibrary()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    LayoutId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LayoutId"))),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    ImageUrl = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ImageUrl")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ImageUrl"))),
                    Options = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Options")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Options"))),
                    ClassName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ClassName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ClassName"))),
                    HasInnerColumns = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("HasInnerColumns")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("HasInnerColumns"))),
                    Columns = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Columns")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Columns"))),
                    MinImage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MinImage")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MinImage"))),
                    MaxImage = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("MaxImage")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("MaxImage"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    LayoutName = Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutName"))),
                });
            }
            return lstLayouts;
        }
    }
}
