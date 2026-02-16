using Microsoft.Extensions.Configuration;
using Order.Application.IRepositories;
using Order.Domain;
using Order.Domain.Entity;
using Order.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Repository
{
    public class IssueTypeRepository : IIssueTypeRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
         public IssueTypeRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(IssueType issueType)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@actionId", issueType.ActionId),
                new SqlParameter("@issue", issueType.Issue),
                new SqlParameter("@createdAt", issueType.CreatedAt),
                new SqlParameter("@createdBy", issueType.CreatedBy),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.IssueType, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(IssueType issueType)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", issueType.Id),
                new SqlParameter("@actionId", issueType.ActionId),
                new SqlParameter("@issue", issueType.Issue),
                new SqlParameter("@createdAt", issueType.CreatedAt),
                new SqlParameter("@createdBy", issueType.CreatedBy),
                new SqlParameter("@modifiedby", issueType.ModifiedBy),
                new SqlParameter("@modifiedat", issueType.ModifiedAt),
                new SqlParameter("@isDeleted", issueType.IsDeleted),
                new SqlParameter("@deletedby", issueType.DeletedBy),
                new SqlParameter("@deletedat", issueType.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.IssueType, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(IssueType issueType)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", issueType.Id),
                new SqlParameter("@deletedby", issueType.DeletedBy),
                new SqlParameter("@deletedat", issueType.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.IssueType, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<IssueType>>> Get(IssueType issueType, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", issueType.Id),
                new SqlParameter("@issue", issueType.Issue),
                new SqlParameter("@actionId", issueType.ActionId),
                new SqlParameter("@actionName", issueType.ActionName),
                new SqlParameter("@searchtext", issueType.SearchText),
                new SqlParameter("@isDeleted", issueType.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetIssueType, issueTypeParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private async Task<List<IssueType>> issueTypeParserAsync(DbDataReader reader)
        {
            List<IssueType> lstissueType = new List<IssueType>();
            while (await reader.ReadAsync())
            {
                lstissueType.Add(new IssueType()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    ActionId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ActionId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ActionId"))),
                    Issue = Convert.ToString(reader.GetValue(reader.GetOrdinal("Issue"))),
                    ActionName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ActionName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ActionName"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),

                });
            }
            return lstissueType;
        }

    }
}
