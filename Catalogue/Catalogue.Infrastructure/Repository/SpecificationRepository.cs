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
using MySqlConnector;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class SpecificationRepository : ISpecificationRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;

        public SpecificationRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(SpecificationLibrary specification)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@name", specification.Name),
                new MySqlParameter("@fieldType", specification.FieldType),
                new MySqlParameter("@parentid", specification.ParentId),
                new MySqlParameter("@pathids", specification.PathIds),
                new MySqlParameter("@ischildparent", specification.IsChildParent),
                new MySqlParameter("@pathname", specification.PathName),
                new MySqlParameter("@createdby", specification.CreatedBy),
                new MySqlParameter("@createdat", specification.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@guid", specification.Guid),
                new MySqlParameter("@name", specification.Name),
                new MySqlParameter("@fieldtype", specification.FieldType),
                new MySqlParameter("@parentid", specification.ParentId),
                new MySqlParameter("@pathids", specification.PathIds),
                new MySqlParameter("@pathname", specification.PathName),
                new MySqlParameter("@modifiedby", specification.ModifiedBy),
                new MySqlParameter("@modifiedat", specification.ModifiedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", specification.ID),
                new MySqlParameter("@deletedby", specification.DeletedBy),
                new MySqlParameter("@deletedat", specification.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", specification.ID),
                new MySqlParameter("@guid", specification.Guid),
                new MySqlParameter("@name", specification.Name),
                new MySqlParameter("@fieldtype", specification.FieldType),
                new MySqlParameter("@parentid", specification.ParentId),
                new MySqlParameter("@pathids", specification.PathIds),
                new MySqlParameter("@isdeleted", specification.isDeleted),
                new MySqlParameter("@ischildparent", specification.IsChildParent),
                new MySqlParameter("@searchtext", specification.Searchtext),
                new MySqlParameter("@getparent", Getparent),
                new MySqlParameter("@getchild", Getchild),
                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@PageSize", PageSize),

            };
                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                //MySqlParameter newid = new MySqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
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
