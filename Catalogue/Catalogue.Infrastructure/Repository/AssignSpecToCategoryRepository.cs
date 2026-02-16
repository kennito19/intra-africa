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
    public class AssignSpecToCategoryRepository : IAssignSpecToCategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;
        public AssignSpecToCategoryRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(AssignSpecToCategory assignSpec)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@categoryid", assignSpec.CategoryID),
                new SqlParameter("@isallowsize", assignSpec.IsAllowSize),
                new SqlParameter("@isallowpricevariant", assignSpec.IsAllowPriceVariant),
                new SqlParameter("@isallowspecifications", assignSpec.IsAllowSpecifications),
                new SqlParameter("@isallowexpirydates", assignSpec.IsAllowExpiryDates),
                new SqlParameter("@isallowcolors", assignSpec.IsAllowColors),
                new SqlParameter("@isallowcolorsinfilter", assignSpec.IsAllowColorsInFilter),
                new SqlParameter("@isallowcolorsinvariant", assignSpec.IsAllowColorsInVariant),
                new SqlParameter("@isallowcolorsincomparision", assignSpec.IsAllowColorsInComparision),
                new SqlParameter("@isallowcolorsintitle", assignSpec.IsAllowColorsInTitle),
                new SqlParameter("@titlesequenceofcolor ", assignSpec.TitleSequenceOfColor),
                new SqlParameter("@createdby", assignSpec.CreatedBy),
                new SqlParameter("@createdat", assignSpec.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignSpecificationToCategory, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(AssignSpecToCategory assignSpec)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@guid", assignSpec.Guid),
                new SqlParameter("@isallowsize", assignSpec.IsAllowSize),
                new SqlParameter("@isallowpricevariant", assignSpec.IsAllowPriceVariant),
                new SqlParameter("@isallowspecifications", assignSpec.IsAllowSpecifications),
                new SqlParameter("@isallowexpirydates", assignSpec.IsAllowExpiryDates),
                new SqlParameter("@isallowcolors", assignSpec.IsAllowColors),
                new SqlParameter("@isallowcolorsinfilter", assignSpec.IsAllowColorsInFilter),
                new SqlParameter("@isallowcolorsinvariant", assignSpec.IsAllowColorsInVariant),
                new SqlParameter("@isallowcolorsincomparision", assignSpec.IsAllowColorsInComparision),
                new SqlParameter("@isallowcolorsintitle", assignSpec.IsAllowColorsInTitle),
                new SqlParameter("@titlesequenceofcolor", assignSpec.TitleSequenceOfColor),
                new SqlParameter("@modifiedby", assignSpec.ModifiedBy),
                new SqlParameter("@modifiedat", assignSpec.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignSpecificationToCategory, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(AssignSpecToCategory assignSpec)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@id", assignSpec.Id),
                new SqlParameter("@deletedby", assignSpec.DeletedBy),
                new SqlParameter("@deletedat", assignSpec.DeletedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignSpecificationToCategory, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<AssignSpecToCategory>>> get(AssignSpecToCategory assignSpec, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", assignSpec.Id),
                new SqlParameter("@guid", assignSpec.Guid),
                new SqlParameter("@catid", assignSpec.CategoryID),
                new SqlParameter("@catname", assignSpec.CategoryName),
                new SqlParameter("@isdeleted", assignSpec.IsDeleted),
                new SqlParameter("@searchtext", assignSpec.Searchtext),
                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@pageSize", PageSize),

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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetAssignSpecificationToCategory, assignSpecToCategoryParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<AssignSpecToCategory>> assignSpecToCategoryParserAsync(DbDataReader reader)
        {
            List<AssignSpecToCategory> lstassignSpec = new List<AssignSpecToCategory>();
            while (await reader.ReadAsync())
            {
                lstassignSpec.Add(new AssignSpecToCategory()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Guid = Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    CategoryID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryID"))),
                    IsAllowSize = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSize"))),
                    IsAllowPriceVariant = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowPriceVariant"))),
                    IsAllowSpecifications = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSpecifications"))),
                    IsAllowExpiryDates = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowExpiryDates"))),
                    IsAllowColors = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowColors"))),
                    IsAllowColorsInFilter = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowColorsInFilter"))),
                    IsAllowColorsInVariant = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowColorsInVariant"))),
                    IsAllowColorsInComparision = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowColorsInComparision"))),
                    IsAllowColorsInTitle = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowColorsInTitle"))),
                    TitleSequenceOfColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleSequenceOfColor")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TitleSequenceOfColor"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    CategoryName = Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryName"))),
                    CategoryPathNames = Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryPathNames"))),
                });
            }
            return lstassignSpec;
        }
    }
}
