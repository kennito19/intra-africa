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
    public class AssignSpecValuesToCategoryRepository : IAssignSpecValuesToCategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;
        public AssignSpecValuesToCategoryRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(AssignSpecValuesToCategory assignSpecValues)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@assignspecid", assignSpecValues.AssignSpecID),
                new MySqlParameter("@specid", assignSpecValues.SpecID),
                new MySqlParameter("@specTypeid", assignSpecValues.SpecTypeID),
                new MySqlParameter("@spectypevalueid", assignSpecValues.SpecTypeValueID),
                new MySqlParameter("@isallowspecinfilter", assignSpecValues.IsAllowSpecInFilter),
                new MySqlParameter("@isallowspecinvariant", assignSpecValues.IsAllowSpecInVariant),
                new MySqlParameter("@isallowspecincomparision", assignSpecValues.IsAllowSpecInComparision),
                new MySqlParameter("@isallowspecintitle", assignSpecValues.IsAllowSpecInTitle),
                new MySqlParameter("@isallowmultipleselection", assignSpecValues.IsAllowMultipleSelection),
                new MySqlParameter("@titlesequenceofspec", assignSpecValues.TitleSequenceOfSpecification),
                new MySqlParameter("@createdby", assignSpecValues.CreatedBy),
                new MySqlParameter("@createdat", assignSpecValues.CreatedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignSpecValuesToCategory, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(AssignSpecValuesToCategory assignSpecValues)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@assiid", assignSpecValues.AssignSpecID),
                new MySqlParameter("@specid", assignSpecValues.SpecID),
                new MySqlParameter("@specTypeid", assignSpecValues.SpecTypeID),
                new MySqlParameter("@isallowspecinfilter", assignSpecValues.IsAllowSpecInFilter),
                new MySqlParameter("@isallowspecinvariant", assignSpecValues.IsAllowSpecInVariant),
                new MySqlParameter("@isallowspecincomparision", assignSpecValues.IsAllowSpecInComparision),
                new MySqlParameter("@isallowspecintitle", assignSpecValues.IsAllowSpecInTitle),
                new MySqlParameter("@isallowmultipleselection", assignSpecValues.IsAllowMultipleSelection),
                new MySqlParameter("@titlesequenceofspec", assignSpecValues.TitleSequenceOfSpecification),
                new MySqlParameter("@modifiedby", assignSpecValues.ModifiedBy),
                new MySqlParameter("@modifiedat", assignSpecValues.ModifiedAt),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignSpecValuesToCategory, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Delete(AssignSpecValuesToCategory assignSpecValues)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@assignspecid", assignSpecValues.AssignSpecID),
                new MySqlParameter("@specid", assignSpecValues.SpecID),
                new MySqlParameter("@specTypeid", assignSpecValues.SpecTypeID),
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.AssignSpecValuesToCategory, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<AssignSpecValuesToCategory>>> get(AssignSpecValuesToCategory assignSpecValues, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", assignSpecValues.Id),
                new MySqlParameter("@assiid", assignSpecValues.AssignSpecID),
                new MySqlParameter("@specid", assignSpecValues.SpecID),
                new MySqlParameter("@specTypeid", assignSpecValues.SpecTypeID),
                new MySqlParameter("@spectypevalueid", assignSpecValues.SpecTypeValueID),
                new MySqlParameter("@specname", assignSpecValues.SpecificationName),
                new MySqlParameter("@spectypename", assignSpecValues.SpecificationTypeName),
                new MySqlParameter("@spectypevaluename", assignSpecValues.SpecificationTypeValueName),
                new MySqlParameter("@categoryId", assignSpecValues.CategoryId),
                new MySqlParameter("@isAllowSpecInFilter", assignSpecValues.IsAllowSpecInFilter),
                new MySqlParameter("@isDeleted", assignSpecValues.IsDeleted),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetAssignSpecValuesToCategory, assignSpecValuesToCategoryParserAsync, output:null, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<AssignSpecValuesToCategory>> assignSpecValuesToCategoryParserAsync(DbDataReader reader)
        {
            List<AssignSpecValuesToCategory> lstassignSpecValue = new List<AssignSpecValuesToCategory>();
            while (await reader.ReadAsync())
            {
                lstassignSpecValue.Add(new AssignSpecValuesToCategory()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    AssignSpecID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("AssignSpecID"))),
                    SpecID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SpecID"))),
                    SpecTypeID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SpecTypeID"))),
                    SpecTypeValueID = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecTypeValueID")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SpecTypeValueID"))),
                    IsAllowSpecInFilter = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSpecInFilter"))),
                    IsAllowSpecInVariant = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSpecInVariant"))),
                    IsAllowSpecInComparision = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSpecInComparision"))),
                    IsAllowSpecInTitle = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSpecInTitle"))),
                    IsAllowMultipleSelection = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowMultipleSelection"))),
                    TitleSequenceOfSpecification = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TitleSequenceOfSpecification")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TitleSequenceOfSpecification"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    SpecificationName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificationName"))),
                    SpecificationTypeName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificationTypeName"))),
                    SpecificationTypeValueName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificationTypeValueName"))),
                    FieldType =  Convert.ToString(reader.GetValue(reader.GetOrdinal("FieldType"))),
                    CategoryId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CategoryId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CategoryId"))),
                    AllowSpecifications = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("AllowSpecifications")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("AllowSpecifications"))),
                });
            }
            return lstassignSpecValue;
        }
    }
}
