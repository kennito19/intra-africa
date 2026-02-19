using Microsoft.Extensions.Configuration;
using Order.Application.IRepositories;
using Order.Domain;
using Order.Domain.Entity;
using Order.Infrastructure.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@issuetypeid", issueReason.IssueTypeId),
                new MySqlParameter("@actionId", issueReason.ActionId),
                new MySqlParameter("@reasons", issueReason.Reasons),
                new MySqlParameter("@createdBy", issueReason.CreatedBy),
                new MySqlParameter("@createdAt", issueReason.CreatedAt),

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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", issueReason.Id),
                new MySqlParameter("@actionId", issueReason.ActionId),
                new MySqlParameter("@issuetypeid", issueReason.IssueTypeId),
                new MySqlParameter("@reasons", issueReason.Reasons),
                new MySqlParameter("@createdBy", issueReason.CreatedBy),
                new MySqlParameter("@createdAt", issueReason.CreatedAt),
                new MySqlParameter("@modifiedby", issueReason.ModifiedBy),
                new MySqlParameter("@modifiedat", issueReason.ModifiedAt),
                new MySqlParameter("@isDeleted", issueReason.IsDeleted),
                new MySqlParameter("@deletedby", issueReason.DeletedBy),
                new MySqlParameter("@deletedat", issueReason.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", issueReason.Id),
                new MySqlParameter("@deletedby", issueReason.DeletedBy),
                new MySqlParameter("@deletedat", issueReason.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", issueReason.Id),
                
                new MySqlParameter("@issuetypeid", issueReason.IssueTypeId),
                new MySqlParameter("@reasons", issueReason.Reasons),
                new MySqlParameter("@issuetype", issueReason.IssueType),
                new MySqlParameter("@actionId", issueReason.ActionId),
                new MySqlParameter("@actionName", issueReason.ActionName),
                new MySqlParameter("@searchtext", issueReason.SearchText),
                new MySqlParameter("@isDeleted", issueReason.IsDeleted),
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
