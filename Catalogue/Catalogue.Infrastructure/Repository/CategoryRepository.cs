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
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;
        public CategoryRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(CategoryLibrary category)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@name", category.Name),
                new SqlParameter("@parentId", category.ParentId),
                new SqlParameter("@image", category.Image),
                new SqlParameter("@pathids", category.PathIds),
                new SqlParameter("@pathnames", category.PathNames),
                new SqlParameter("@currentlevel", category.CurrentLevel),
                new SqlParameter("@MetaTitles", category.MetaTitles),
                new SqlParameter("@metaDescription", category.MetaDescription),
                new SqlParameter("@metakeywords", category.MetaKeywords),
                new SqlParameter("@status", category.Status),
                new SqlParameter("@color", category.Color),
                new SqlParameter("@title", category.Title),
                new SqlParameter("@subtitle", category.SubTitle),
                new SqlParameter("@description", category.Description),
                new SqlParameter("@createdby", category.CreatedBy),
                new SqlParameter("@createdat", category.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Category, output, newid, message, sqlParams.ToArray());
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(CategoryLibrary category)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@guid", category.Guid),
                new SqlParameter("@name", category.Name),
                new SqlParameter("@currentlevel", category.CurrentLevel),
                new SqlParameter("@parentId", category.ParentId),
                new SqlParameter("@image", category.Image),
                new SqlParameter("@pathids", category.PathIds),
                new SqlParameter("@pathnames", category.PathNames),
                new SqlParameter("@MetaTitles", category.MetaTitles),
                new SqlParameter("@metaDescription", category.MetaDescription),
                new SqlParameter("@metakeywords", category.MetaKeywords),
                new SqlParameter("@status", category.Status),
                new SqlParameter("@color", category.Color),
                new SqlParameter("@title", category.Title),
                new SqlParameter("@subtitle", category.SubTitle),
                new SqlParameter("@description", category.Description),
                new SqlParameter("@modifiedby", category.ModifiedBy),
                new SqlParameter("@modifiedat", category.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Category, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(CategoryLibrary category)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", category.Id),
                new SqlParameter("@deletedby", category.DeletedBy),
                new SqlParameter("@deletedat", category.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Category, output, newid , message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<CategoryLibrary>>> get(CategoryLibrary category, bool Getparent = false, bool Getchild = false, int PageIndex = 1 , int PageSize = 10, string? Mode = "get")
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", category.Id),
                new SqlParameter("@guid", category.Guid),
                new SqlParameter("@name", category.Name),
                new SqlParameter("@pathids", category.PathIds),
                new SqlParameter("@status", category.Status),
                new SqlParameter("@isdeleted", category.IsDeleted),
                new SqlParameter("@parentId", category.ParentId),
                new SqlParameter("@parentname", category.ParentName),
                new SqlParameter("@getparent", Getparent),
                new SqlParameter("@getchild", Getchild),
                new SqlParameter("@searchtext", category.Searchtext),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetCategory, categoryParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<CategoryLibrary>>> GetCategoryWithParent(int Categoryid)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@categoryid", Categoryid),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetCategoryWithParent, categoryParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<CategoryLibrary>> categoryParserAsync(DbDataReader reader)
        {
            List<CategoryLibrary> lstcategory = new List<CategoryLibrary>();
            while (await reader.ReadAsync())
            {
                lstcategory.Add(new CategoryLibrary()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Guid = Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    CurrentLevel = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CurrentLevel"))),
                    ParentId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentId"))),
                    Image = Convert.ToString(reader.GetValue(reader.GetOrdinal("Image"))),
                    PathIds = Convert.ToString(reader.GetValue(reader.GetOrdinal("PathIds"))),
                    PathNames = Convert.ToString(reader.GetValue(reader.GetOrdinal("PathNames"))),
                    MetaTitles = Convert.ToString(reader.GetValue(reader.GetOrdinal("MetaTitles"))),
                    MetaDescription = Convert.ToString(reader.GetValue(reader.GetOrdinal("MetaDescription"))),
                    MetaKeywords = Convert.ToString(reader.GetValue(reader.GetOrdinal("MetaKeywords"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    Color = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Color")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Color"))),
                    Title = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Title")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Title"))),
                    SubTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitle"))),
                    Description = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Description")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    ParentName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentName"))),
                    ParentPathNames = Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentPathNames"))),
                    ParentPathIds = Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentPathIds")))
                });
            }
            return lstcategory;
        }
    }
}
