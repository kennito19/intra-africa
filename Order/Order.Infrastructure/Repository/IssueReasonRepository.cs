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
    public class IssueReasonRepository : IIssueReasonRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
      
        public IssueReasonRepository(IConfiguration configuration)
        {
            
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(IssueReason issueReason)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@issuetypeid", issueReason.IssueTypeId),
                new SqlParameter("@actionId", issueReason.ActionId),
                new SqlParameter("@reasons", issueReason.Reasons),
                new SqlParameter("@createdBy", issueReason.CreatedBy),
                new SqlParameter("@createdAt", issueReason.CreatedAt),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.IssueReason, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(IssueReason issueReason)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", issueReason.Id),
                new SqlParameter("@actionId", issueReason.ActionId),
                new SqlParameter("@issuetypeid", issueReason.IssueTypeId),
                new SqlParameter("@reasons", issueReason.Reasons),
                new SqlParameter("@createdBy", issueReason.CreatedBy),
                new SqlParameter("@createdAt", issueReason.CreatedAt),
                new SqlParameter("@modifiedby", issueReason.ModifiedBy),
                new SqlParameter("@modifiedat", issueReason.ModifiedAt),
                new SqlParameter("@isDeleted", issueReason.IsDeleted),
                new SqlParameter("@deletedby", issueReason.DeletedBy),
                new SqlParameter("@deletedat", issueReason.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.IssueReason, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(IssueReason issueReason)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", issueReason.Id),
                new SqlParameter("@deletedby", issueReason.DeletedBy),
                new SqlParameter("@deletedat", issueReason.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.IssueReason, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<IssueReason>>> Get(IssueReason issueReason, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", issueReason.Id),
                
                new SqlParameter("@issuetypeid", issueReason.IssueTypeId),
                new SqlParameter("@reasons", issueReason.Reasons),
                new SqlParameter("@issuetype", issueReason.IssueType),
                new SqlParameter("@actionId", issueReason.ActionId),
                new SqlParameter("@actionName", issueReason.ActionName),
                new SqlParameter("@searchtext", issueReason.SearchText),
                new SqlParameter("@isDeleted", issueReason.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetIssueReason, issueReasonParserAsync, output, newid: null, message, sqlParams.ToArray());

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<IssueReason>> issueReasonParserAsync(DbDataReader reader)
        {
            List<IssueReason> lstissueReason = new List<IssueReason>();
            while (await reader.ReadAsync())
            {
                lstissueReason.Add(new IssueReason()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    
                    IssueTypeId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("IssueTypeId"))),
                    ActionId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ActionId")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ActionId"))),
                    Reasons = Convert.ToString(reader.GetValue(reader.GetOrdinal("Reasons"))),
                    IssueType = Convert.ToString(reader.GetValue(reader.GetOrdinal("IssueType"))),
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
            return lstissueReason;
        }

    }
}
