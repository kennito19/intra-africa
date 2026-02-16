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
    public class AssignSpecValuesToCategoryRepository : IAssignSpecValuesToCategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;
        public AssignSpecValuesToCategoryRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(AssignSpecValuesToCategory assignSpecValues)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "add"),
                new SqlParameter("@assignspecid", assignSpecValues.AssignSpecID),
                new SqlParameter("@specid", assignSpecValues.SpecID),
                new SqlParameter("@specTypeid", assignSpecValues.SpecTypeID),
                new SqlParameter("@spectypevalueid", assignSpecValues.SpecTypeValueID),
                new SqlParameter("@isallowspecinfilter", assignSpecValues.IsAllowSpecInFilter),
                new SqlParameter("@isallowspecinvariant", assignSpecValues.IsAllowSpecInVariant),
                new SqlParameter("@isallowspecincomparision", assignSpecValues.IsAllowSpecInComparision),
                new SqlParameter("@isallowspecintitle", assignSpecValues.IsAllowSpecInTitle),
                new SqlParameter("@isallowmultipleselection", assignSpecValues.IsAllowMultipleSelection),
                new SqlParameter("@titlesequenceofspec", assignSpecValues.TitleSequenceOfSpecification),
                new SqlParameter("@createdby", assignSpecValues.CreatedBy),
                new SqlParameter("@createdat", assignSpecValues.CreatedAt),
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@assiid", assignSpecValues.AssignSpecID),
                new SqlParameter("@specid", assignSpecValues.SpecID),
                new SqlParameter("@specTypeid", assignSpecValues.SpecTypeID),
                new SqlParameter("@isallowspecinfilter", assignSpecValues.IsAllowSpecInFilter),
                new SqlParameter("@isallowspecinvariant", assignSpecValues.IsAllowSpecInVariant),
                new SqlParameter("@isallowspecincomparision", assignSpecValues.IsAllowSpecInComparision),
                new SqlParameter("@isallowspecintitle", assignSpecValues.IsAllowSpecInTitle),
                new SqlParameter("@isallowmultipleselection", assignSpecValues.IsAllowMultipleSelection),
                new SqlParameter("@titlesequenceofspec", assignSpecValues.TitleSequenceOfSpecification),
                new SqlParameter("@modifiedby", assignSpecValues.ModifiedBy),
                new SqlParameter("@modifiedat", assignSpecValues.ModifiedAt),
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
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@assignspecid", assignSpecValues.AssignSpecID),
                new SqlParameter("@specid", assignSpecValues.SpecID),
                new SqlParameter("@specTypeid", assignSpecValues.SpecTypeID),
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
                var sqlParams = new List<SqlParameter>() {
                //new SqlParameter("@mode", "get"),
                new SqlParameter("@mode", Mode),
                new SqlParameter("@id", assignSpecValues.Id),
                new SqlParameter("@assiid", assignSpecValues.AssignSpecID),
                new SqlParameter("@specid", assignSpecValues.SpecID),
                new SqlParameter("@specTypeid", assignSpecValues.SpecTypeID),
                new SqlParameter("@spectypevalueid", assignSpecValues.SpecTypeValueID),
                new SqlParameter("@specname", assignSpecValues.SpecificationName),
                new SqlParameter("@spectypename", assignSpecValues.SpecificationTypeName),
                new SqlParameter("@spectypevaluename", assignSpecValues.SpecificationTypeValueName),
                new SqlParameter("@categoryId", assignSpecValues.CategoryId),
                new SqlParameter("@isAllowSpecInFilter", assignSpecValues.IsAllowSpecInFilter),
                new SqlParameter("@isDeleted", assignSpecValues.IsDeleted),
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
