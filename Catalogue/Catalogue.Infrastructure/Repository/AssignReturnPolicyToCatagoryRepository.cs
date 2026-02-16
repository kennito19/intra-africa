using Catalogue.Application.IRepositories;
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
    public class AssignReturnPolicyToCatagoryRepository : IAssignReturnPolicyToCatagoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;
        public AssignReturnPolicyToCatagoryRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@returnpolicydetailid", assignReturnPolicyToCatagory.ReturnPolicyDetailID),
                new SqlParameter("@catid", assignReturnPolicyToCatagory.CategoryID),
                new SqlParameter("@createdby", assignReturnPolicyToCatagory.CreatedBy),
                new SqlParameter("@createdat", assignReturnPolicyToCatagory.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignReturnPolicyToCategory, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Update(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", assignReturnPolicyToCatagory.Id),
                new SqlParameter("@returnpolicydetailid", assignReturnPolicyToCatagory.ReturnPolicyDetailID),
                new SqlParameter("@catid", assignReturnPolicyToCatagory.CategoryID),
                new SqlParameter("@modifiedby", assignReturnPolicyToCatagory.ModifiedBy),
                new SqlParameter("@modifiedat", assignReturnPolicyToCatagory.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignReturnPolicyToCategory, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<BaseResponse<long>> Delete(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", assignReturnPolicyToCatagory.Id),
                new SqlParameter("@deletedby", assignReturnPolicyToCatagory.DeletedBy),
                new SqlParameter("@deletedat", assignReturnPolicyToCatagory.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignReturnPolicyToCategory, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<AssignReturnPolicyToCatagory>>> get(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", assignReturnPolicyToCatagory.Id),
                new SqlParameter("@catid", assignReturnPolicyToCatagory.CategoryID),
                new SqlParameter("@returnpolicydetailid", assignReturnPolicyToCatagory.ReturnPolicyDetailID),
                new SqlParameter("@returnpolicyid", assignReturnPolicyToCatagory.ReturnPolicyID),
                new SqlParameter("@policyname", assignReturnPolicyToCatagory.ReturnPolicy),
                new SqlParameter("@isdeleted", assignReturnPolicyToCatagory.IsDeleted),
                new SqlParameter("@searchtext", assignReturnPolicyToCatagory.Searchtext),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@PageSize", PageSize)

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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetAssignReturnPolicyToCategory, assignReturnPolicyToCatagoryParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<AssignReturnPolicyToCatagory>> assignReturnPolicyToCatagoryParserAsync(DbDataReader reader)
        {
            List<AssignReturnPolicyToCatagory> lstassignReturnPolicyToCatagory = new List<AssignReturnPolicyToCatagory>();
            while (await reader.ReadAsync())
            {
                lstassignReturnPolicyToCatagory.Add(new AssignReturnPolicyToCatagory()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    ReturnPolicyDetailID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyDetailID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ReturnPolicyDetailID"))),
                    CategoryID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CatID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CatID"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    Category = Convert.ToString(reader.GetValue(reader.GetOrdinal("Category"))),
                    ParentCategoryID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentCategoryID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentCategoryID"))),
                    PathIds = Convert.ToString(reader.GetValue(reader.GetOrdinal("PathIds"))),
                    PathNames = Convert.ToString(reader.GetValue(reader.GetOrdinal("PathNames"))),
                    ReturnPolicyID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicyID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ReturnPolicyID"))),
                    ValidityDays = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ValidityDays")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ValidityDays"))),
                    Title = Convert.ToString(reader.GetValue(reader.GetOrdinal("Title"))),
                    Covers = Convert.ToString(reader.GetValue(reader.GetOrdinal("Covers"))),
                    Description = Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),
                    ReturnPolicy = Convert.ToString(reader.GetValue(reader.GetOrdinal("ReturnPolicy"))),
                });
            }
            return lstassignReturnPolicyToCatagory;
        }
    }
}
