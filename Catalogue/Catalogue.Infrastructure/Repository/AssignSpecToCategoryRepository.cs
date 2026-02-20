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
    public class AssignSpecToCategoryRepository : IAssignSpecToCategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;
        public AssignSpecToCategoryRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(AssignSpecToCategory assignSpec)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@categoryid", assignSpec.CategoryID),
                new MySqlParameter("@isallowsize", assignSpec.IsAllowSize),
                new MySqlParameter("@isallowpricevariant", assignSpec.IsAllowPriceVariant),
                new MySqlParameter("@isallowspecifications", assignSpec.IsAllowSpecifications),
                new MySqlParameter("@isallowexpirydates", assignSpec.IsAllowExpiryDates),
                new MySqlParameter("@isallowcolors", assignSpec.IsAllowColors),
                new MySqlParameter("@isallowcolorsinfilter", assignSpec.IsAllowColorsInFilter),
                new MySqlParameter("@isallowcolorsinvariant", assignSpec.IsAllowColorsInVariant),
                new MySqlParameter("@isallowcolorsincomparision", assignSpec.IsAllowColorsInComparision),
                new MySqlParameter("@isallowcolorsintitle", assignSpec.IsAllowColorsInTitle),
                new MySqlParameter("@titlesequenceofcolor ", assignSpec.TitleSequenceOfColor),
                new MySqlParameter("@createdby", assignSpec.CreatedBy),
                new MySqlParameter("@createdat", assignSpec.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@guid", assignSpec.Guid),
                new MySqlParameter("@isallowsize", assignSpec.IsAllowSize),
                new MySqlParameter("@isallowpricevariant", assignSpec.IsAllowPriceVariant),
                new MySqlParameter("@isallowspecifications", assignSpec.IsAllowSpecifications),
                new MySqlParameter("@isallowexpirydates", assignSpec.IsAllowExpiryDates),
                new MySqlParameter("@isallowcolors", assignSpec.IsAllowColors),
                new MySqlParameter("@isallowcolorsinfilter", assignSpec.IsAllowColorsInFilter),
                new MySqlParameter("@isallowcolorsinvariant", assignSpec.IsAllowColorsInVariant),
                new MySqlParameter("@isallowcolorsincomparision", assignSpec.IsAllowColorsInComparision),
                new MySqlParameter("@isallowcolorsintitle", assignSpec.IsAllowColorsInTitle),
                new MySqlParameter("@titlesequenceofcolor", assignSpec.TitleSequenceOfColor),
                new MySqlParameter("@modifiedby", assignSpec.ModifiedBy),
                new MySqlParameter("@modifiedat", assignSpec.ModifiedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", assignSpec.Id),
                new MySqlParameter("@deletedby", assignSpec.DeletedBy),
                new MySqlParameter("@deletedat", assignSpec.DeletedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", assignSpec.Id),
                new MySqlParameter("@guid", assignSpec.Guid),
                new MySqlParameter("@catid", assignSpec.CategoryID),
                new MySqlParameter("@catname", assignSpec.CategoryName),
                new MySqlParameter("@isdeleted", assignSpec.IsDeleted),
                new MySqlParameter("@searchtext", assignSpec.Searchtext),
                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@pageSize", PageSize),

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
