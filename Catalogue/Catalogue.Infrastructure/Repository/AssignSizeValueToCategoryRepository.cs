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
    public class AssignSizeValueToCategoryRepository : IAssignSizeValueToCategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;
        public AssignSizeValueToCategoryRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(AssignSizeValueToCategory assignSize)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@assignspecid", assignSize.AssignSpecID),
                new SqlParameter("@sizetypeid", assignSize.SizeTypeID),
                new SqlParameter("@sizeid", assignSize.SizeId),
                new SqlParameter("@isallowsizeinfilter", assignSize.IsAllowSizeInFilter),
                new SqlParameter("@isallowsizeinvariant", assignSize.IsAllowSizeInVariant),
                new SqlParameter("@isallowsizeincomparision", assignSize.IsAllowSizeInComparision),
                new SqlParameter("@isallowsizeintitle", assignSize.IsAllowSizeInTitle),
                new SqlParameter("@titlesequenceofsize", assignSize.TitleSequenceOfSize),
                new SqlParameter("@createdby", assignSize.CreatedBy),
                new SqlParameter("@createdat", assignSize.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignSizeValueToCategory, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(AssignSizeValueToCategory assignSize)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@assiid", assignSize.AssignSpecID),
               new SqlParameter("@sizetypeid", assignSize.SizeTypeID),
               new SqlParameter("@sizeid", assignSize.SizeId),
                new SqlParameter("@isallowsizeinfilter", assignSize.IsAllowSizeInFilter),
                new SqlParameter("@isallowsizeinvariant", assignSize.IsAllowSizeInVariant),
                new SqlParameter("@isallowsizeincomparision", assignSize.IsAllowSizeInComparision),
                new SqlParameter("@isallowsizeintitle", assignSize.IsAllowSizeInTitle),
                new SqlParameter("@titlesequenceofsize", assignSize.TitleSequenceOfSize),
                new SqlParameter("@modifiedby", assignSize.ModifiedBy),
                new SqlParameter("@modifiedat", assignSize.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignSizeValueToCategory, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(AssignSizeValueToCategory assignSize)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@assignspecid", assignSize.AssignSpecID),
                new SqlParameter("@sizetypeid", assignSize.SizeTypeID)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignSizeValueToCategory, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<AssignSizeValueToCategory>>> get(AssignSizeValueToCategory assignSize, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", assignSize.Id),
                new SqlParameter("@assiid", assignSize.AssignSpecID),
                new SqlParameter("@sizeid", assignSize.SizeId),
                new SqlParameter("@sizetypeid", assignSize.SizeTypeID),
                new SqlParameter("@sizename", assignSize.SizeName),
                new SqlParameter("@sizetypename", assignSize.SizeTypeName),
                new SqlParameter("@categoryId", assignSize.CategoryId),
                new SqlParameter("@isAllowSizeInFilter", assignSize.IsAllowSizeInFilter),
                new SqlParameter("@isDeleted", assignSize.IsDeleted),
                new SqlParameter("@searchtext", assignSize.Searchtext),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetAssignSizeValueToCategory, assignSizeValueToCategoryParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<AssignSizeValueToCategory>> assignSizeValueToCategoryParserAsync(DbDataReader reader)
        {
            List<AssignSizeValueToCategory> lstassignSize = new List<AssignSizeValueToCategory>();
            while (await reader.ReadAsync())
            {
                lstassignSize.Add(new AssignSizeValueToCategory()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    AssignSpecID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("AssignSpecID"))),
                    SizeId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeId"))),
                    SizeTypeID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SizeTypeID"))),
                    IsAllowSizeInFilter = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsAllowSizeInFilter")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSizeInFilter"))),
                    IsAllowSizeInVariant = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSizeInVariant"))),
                    IsAllowSizeInComparision = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSizeInComparision"))),
                    IsAllowSizeInTitle = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSizeInTitle"))),
                    TitleSequenceOfSize = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleSequenceOfSize")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TitleSequenceOfSize"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    SizeName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeName"))),
                    SizeTypeName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeTypeName"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    AllowSize = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AllowSize")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("AllowSize"))),
                    isPriceVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("isPriceVariant")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("isPriceVariant"))),
                });
            }
            return lstassignSize;
        }
    }
}
