using Catalogue.Application.IRepositories;
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@returnpolicydetailid", assignReturnPolicyToCatagory.ReturnPolicyDetailID),
                new MySqlParameter("@catid", assignReturnPolicyToCatagory.CategoryID),
                new MySqlParameter("@createdby", assignReturnPolicyToCatagory.CreatedBy),
                new MySqlParameter("@createdat", assignReturnPolicyToCatagory.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", assignReturnPolicyToCatagory.Id),
                new MySqlParameter("@returnpolicydetailid", assignReturnPolicyToCatagory.ReturnPolicyDetailID),
                new MySqlParameter("@catid", assignReturnPolicyToCatagory.CategoryID),
                new MySqlParameter("@modifiedby", assignReturnPolicyToCatagory.ModifiedBy),
                new MySqlParameter("@modifiedat", assignReturnPolicyToCatagory.ModifiedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", assignReturnPolicyToCatagory.Id),
                new MySqlParameter("@deletedby", assignReturnPolicyToCatagory.DeletedBy),
                new MySqlParameter("@deletedat", assignReturnPolicyToCatagory.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", assignReturnPolicyToCatagory.Id),
                new MySqlParameter("@catid", assignReturnPolicyToCatagory.CategoryID),
                new MySqlParameter("@returnpolicydetailid", assignReturnPolicyToCatagory.ReturnPolicyDetailID),
                new MySqlParameter("@returnpolicyid", assignReturnPolicyToCatagory.ReturnPolicyID),
                new MySqlParameter("@policyname", assignReturnPolicyToCatagory.ReturnPolicy),
                new MySqlParameter("@isdeleted", assignReturnPolicyToCatagory.IsDeleted),
                new MySqlParameter("@searchtext", assignReturnPolicyToCatagory.Searchtext),
                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@PageSize", PageSize)

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
