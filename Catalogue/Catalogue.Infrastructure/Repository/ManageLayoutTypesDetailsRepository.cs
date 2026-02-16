using Catalogue.Domain.Entity;
using Catalogue.Domain;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
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
        SqlConnection con;

        public ManageLayoutTypesDetailsRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(ManageLayoutTypesDetails typesDetails)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@layoutId", typesDetails.LayoutId),
                new SqlParameter("@layoutTypeId", typesDetails.LayoutTypeId),
                new SqlParameter("@name", typesDetails.Name),
                new SqlParameter("@sectionType", typesDetails.SectionType),
                new SqlParameter("@innerColumns", typesDetails.InnerColumns),
                new SqlParameter("@createdby", typesDetails.CreatedBy),
                new SqlParameter("@createdat", typesDetails.CreatedAt),
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", typesDetails.Id),
                new SqlParameter("@layoutId", typesDetails.LayoutId),
                new SqlParameter("@layoutTypeId", typesDetails.LayoutTypeId),
                new SqlParameter("@name", typesDetails.Name),
                new SqlParameter("@sectionType", typesDetails.SectionType),
                new SqlParameter("@innerColumns", typesDetails.InnerColumns),
                new SqlParameter("@modifiedby", typesDetails.ModifiedBy),
                new SqlParameter("@modifiedat", typesDetails.ModifiedAt),
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", typesDetails.Id),
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", typesDetails.Id),
                new SqlParameter("@layoutId", typesDetails.LayoutId),
                new SqlParameter("@layoutTypeId", typesDetails.LayoutTypeId),
                new SqlParameter("@name", typesDetails.Name),
                new SqlParameter("@searchtext", typesDetails.Searchtext),
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
