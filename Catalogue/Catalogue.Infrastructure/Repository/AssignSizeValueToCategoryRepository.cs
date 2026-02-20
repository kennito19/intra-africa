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
    public class AssignSizeValueToCategoryRepository : IAssignSizeValueToCategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;
        public AssignSizeValueToCategoryRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(AssignSizeValueToCategory assignSize)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@assignspecid", assignSize.AssignSpecID),
                new MySqlParameter("@sizetypeid", assignSize.SizeTypeID),
                new MySqlParameter("@sizeid", assignSize.SizeId),
                new MySqlParameter("@isallowsizeinfilter", assignSize.IsAllowSizeInFilter),
                new MySqlParameter("@isallowsizeinvariant", assignSize.IsAllowSizeInVariant),
                new MySqlParameter("@isallowsizeincomparision", assignSize.IsAllowSizeInComparision),
                new MySqlParameter("@isallowsizeintitle", assignSize.IsAllowSizeInTitle),
                new MySqlParameter("@titlesequenceofsize", assignSize.TitleSequenceOfSize),
                new MySqlParameter("@createdby", assignSize.CreatedBy),
                new MySqlParameter("@createdat", assignSize.CreatedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@assiid", assignSize.AssignSpecID),
               new MySqlParameter("@sizetypeid", assignSize.SizeTypeID),
               new MySqlParameter("@sizeid", assignSize.SizeId),
                new MySqlParameter("@isallowsizeinfilter", assignSize.IsAllowSizeInFilter),
                new MySqlParameter("@isallowsizeinvariant", assignSize.IsAllowSizeInVariant),
                new MySqlParameter("@isallowsizeincomparision", assignSize.IsAllowSizeInComparision),
                new MySqlParameter("@isallowsizeintitle", assignSize.IsAllowSizeInTitle),
                new MySqlParameter("@titlesequenceofsize", assignSize.TitleSequenceOfSize),
                new MySqlParameter("@modifiedby", assignSize.ModifiedBy),
                new MySqlParameter("@modifiedat", assignSize.ModifiedAt),
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@assignspecid", assignSize.AssignSpecID),
                new MySqlParameter("@sizetypeid", assignSize.SizeTypeID)
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
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", assignSize.Id),
                new MySqlParameter("@assiid", assignSize.AssignSpecID),
                new MySqlParameter("@sizeid", assignSize.SizeId),
                new MySqlParameter("@sizetypeid", assignSize.SizeTypeID),
                new MySqlParameter("@sizename", assignSize.SizeName),
                new MySqlParameter("@sizetypename", assignSize.SizeTypeName),
                new MySqlParameter("@categoryId", assignSize.CategoryId),
                new MySqlParameter("@isAllowSizeInFilter", assignSize.IsAllowSizeInFilter),
                new MySqlParameter("@isDeleted", assignSize.IsDeleted),
                new MySqlParameter("@searchtext", assignSize.Searchtext),
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
