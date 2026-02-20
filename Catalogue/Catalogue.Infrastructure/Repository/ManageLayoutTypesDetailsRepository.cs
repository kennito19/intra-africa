using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using MySqlConnector;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogue.Application.IRepositories;

namespace Catalogue.Infrastructure.Repository
{
    public class ManageLayoutTypesDetailsRepository : IManageLayoutTypesDetailsRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;

        public ManageLayoutTypesDetailsRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(ManageLayoutTypesDetails typesDetails)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@layoutId", typesDetails.LayoutId),
                new MySqlParameter("@layoutTypeId", typesDetails.LayoutTypeId),
                new MySqlParameter("@name", typesDetails.Name),
                new MySqlParameter("@sectionType", typesDetails.SectionType),
                new MySqlParameter("@innerColumns", typesDetails.InnerColumns),
                new MySqlParameter("@createdby", typesDetails.CreatedBy),
                new MySqlParameter("@createdat", typesDetails.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageLayoutTypesDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(ManageLayoutTypesDetails typesDetails)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", typesDetails.Id),
                new MySqlParameter("@layoutId", typesDetails.LayoutId),
                new MySqlParameter("@layoutTypeId", typesDetails.LayoutTypeId),
                new MySqlParameter("@name", typesDetails.Name),
                new MySqlParameter("@sectionType", typesDetails.SectionType),
                new MySqlParameter("@innerColumns", typesDetails.InnerColumns),
                new MySqlParameter("@modifiedby", typesDetails.ModifiedBy),
                new MySqlParameter("@modifiedat", typesDetails.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageLayoutTypesDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageLayoutTypesDetails typesDetails)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", typesDetails.Id),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ManageLayoutTypesDetails, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ManageLayoutTypesDetails>>> get(ManageLayoutTypesDetails typesDetails, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", typesDetails.Id),
                new MySqlParameter("@layoutId", typesDetails.LayoutId),
                new MySqlParameter("@layoutTypeId", typesDetails.LayoutTypeId),
                new MySqlParameter("@name", typesDetails.Name),
                new MySqlParameter("@searchtext", typesDetails.Searchtext),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetManageLayoutTypesDetails, LayoutsDetailsParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ManageLayoutTypesDetails>> LayoutsDetailsParserAsync(DbDataReader reader)
        {
            List<ManageLayoutTypesDetails> lstLayoutsDetails = new List<ManageLayoutTypesDetails>();
            while (await reader.ReadAsync())
            {
                lstLayoutsDetails.Add(new ManageLayoutTypesDetails()
                {
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    LayoutId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LayoutId"))),
                    LayoutTypeId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("LayoutTypeId"))),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    SectionType = Convert.ToString(reader.GetValue(reader.GetOrdinal("SectionType"))),
                    InnerColumns = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("InnerColumns")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("InnerColumns"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    LayoutName = Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutName"))),
                    LayoutTypeName = Convert.ToString(reader.GetValue(reader.GetOrdinal("LayoutTypeName"))),
                });
            }
            return lstLayoutsDetails;
        }
    }
}
