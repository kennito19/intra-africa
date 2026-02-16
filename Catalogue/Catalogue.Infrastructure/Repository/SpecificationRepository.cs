using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
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
    public class SpecificationRepository : ISpecificationRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public SpecificationRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(SpecificationLibrary specification)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@name", specification.Name),
                new SqlParameter("@fieldType", specification.FieldType),
                new SqlParameter("@parentid", specification.ParentId),
                new SqlParameter("@pathids", specification.PathIds),
                new SqlParameter("@ischildparent", specification.IsChildParent),
                new SqlParameter("@pathname", specification.PathName),
                new SqlParameter("@createdby", specification.CreatedBy),
                new SqlParameter("@createdat", specification.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Specification, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(SpecificationLibrary specification)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@guid", specification.Guid),
                new SqlParameter("@name", specification.Name),
                new SqlParameter("@fieldtype", specification.FieldType),
                new SqlParameter("@parentid", specification.ParentId),
                new SqlParameter("@pathids", specification.PathIds),
                new SqlParameter("@pathname", specification.PathName),
                new SqlParameter("@modifiedby", specification.ModifiedBy),
                new SqlParameter("@modifiedat", specification.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Specification, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(SpecificationLibrary specification)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", specification.ID),
                new SqlParameter("@deletedby", specification.DeletedBy),
                new SqlParameter("@deletedat", specification.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Specification, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<SpecificationLibrary>>> get(SpecificationLibrary specification, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", specification.ID),
                new SqlParameter("@guid", specification.Guid),
                new SqlParameter("@name", specification.Name),
                new SqlParameter("@fieldtype", specification.FieldType),
                new SqlParameter("@parentid", specification.ParentId),
                new SqlParameter("@pathids", specification.PathIds),
                new SqlParameter("@isdeleted", specification.isDeleted),
                new SqlParameter("@ischildparent", specification.IsChildParent),
                new SqlParameter("@searchtext", specification.Searchtext),
                new SqlParameter("@getparent", Getparent),
                new SqlParameter("@getchild", Getchild),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@PageSize", PageSize),

            };
                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                //SqlParameter newid = new SqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetSpecification, specificationParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<SpecificationLibrary>> specificationParserAsync(DbDataReader reader)
        {
            List<SpecificationLibrary> lstspecification = new List<SpecificationLibrary>();
            while (await reader.ReadAsync())
            {
                lstspecification.Add(new SpecificationLibrary()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    ID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ID"))),
                    Guid = reader.GetValue(reader.GetOrdinal("Guid")).ToString(),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    FieldType = Convert.ToString(reader.GetValue(reader.GetOrdinal("FieldType"))),
                    ParentId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentId"))),
                    PathIds = Convert.ToString(reader.GetValue(reader.GetOrdinal("PathIds"))),
                    PathName = Convert.ToString(reader.GetValue(reader.GetOrdinal("PathName"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    isDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("isDeleted"))),
                    IsChildParent = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsChildParent"))),
                    ParentName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentName"))),
                    ParentPathNames = Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentPathNames"))),
                    ParentPathIds = Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentPathIds"))),
                });
            }
            return lstspecification;
        }

    }
}
