using Catalogue.Application.IRepositories;
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
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "add"),
                    new MySqlParameter("@layoutId", layouts.LayoutId),
                    new MySqlParameter("@name", layouts.Name),
                    new MySqlParameter("@image", layouts.ImageUrl),
                    new MySqlParameter("@options", layouts.Options),
                    new MySqlParameter("@className", layouts.ClassName),
                    new MySqlParameter("@hasInnerColumns", layouts.HasInnerColumns),
                    new MySqlParameter("@columns", layouts.Columns),
                    new MySqlParameter("@minImage", layouts.MinImage),
                    new MySqlParameter("@maxImage", layouts.MaxImage),
                    new MySqlParameter("@createdby", layouts.CreatedBy),
                new MySqlParameter("@createdat", layouts.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", layouts.Id),
                new MySqlParameter("@layoutId", layouts.LayoutId),
                new MySqlParameter("@name", layouts.Name),
                new MySqlParameter("@image", layouts.ImageUrl),
                new MySqlParameter("@options", layouts.Options),
                new MySqlParameter("@className", layouts.ClassName),
                new MySqlParameter("@hasInnerColumns", layouts.HasInnerColumns),
                new MySqlParameter("@columns", layouts.Columns),
                new MySqlParameter("@minImage", layouts.MinImage),
                new MySqlParameter("@maxImage", layouts.MaxImage),
                new MySqlParameter("@modifiedby", layouts.ModifiedBy),
                new MySqlParameter("@modifiedat", layouts.ModifiedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", layouts.Id),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", layouts.Id),
                new MySqlParameter("@layoutId", layouts.LayoutId),
                new MySqlParameter("@name", layouts.Name),
                new MySqlParameter("@layoutName", layouts.LayoutName),
                new MySqlParameter("@searchtext", layouts.Searchtext),
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
