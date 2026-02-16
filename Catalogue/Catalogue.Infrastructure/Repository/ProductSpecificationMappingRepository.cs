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
    public class ProductSpecificationMappingRepository : IProductSpecificationMappingRepository
    {
        
        private readonly SqlConnection con;
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public ProductSpecificationMappingRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(ProductSpecificationMapping productSpecificationMapping)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode","add"),
                     
                    new SqlParameter("@CatID", productSpecificationMapping.CatId),
                    new SqlParameter("@ProductID", productSpecificationMapping.ProductID),
                    new SqlParameter("@SpecID", productSpecificationMapping.SpecId),
                    new SqlParameter("@SpecTypeId", productSpecificationMapping.SpecTypeId),
                    new SqlParameter("@SpecValueId", productSpecificationMapping.SpecValueId),
                    new SqlParameter("@Value", productSpecificationMapping.Value),
                    new SqlParameter("@fileName", productSpecificationMapping.FileName),

                    new SqlParameter("@createdBy", productSpecificationMapping.CreatedBy),
                    new SqlParameter("@createdAt", productSpecificationMapping.CreatedAt),
                    new SqlParameter("@modifiedBy", productSpecificationMapping.ModifiedBy),
                    new SqlParameter("@modifiedAt", productSpecificationMapping.ModifiedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductSpecificationMapping, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<BaseResponse<long>> Delete(ProductSpecificationMapping productSpecificationMapping)
        {

            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "delete"),
                new SqlParameter("@ProductID", productSpecificationMapping.ProductID),
                 
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductSpecificationMapping, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<long>> Update(ProductSpecificationMapping productSpecificationMapping)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", "update"),
                new SqlParameter("@id", productSpecificationMapping.Id),
                new SqlParameter("@CatID", productSpecificationMapping.CatId),
                new SqlParameter("@ProductID", productSpecificationMapping.ProductID),
                new SqlParameter("@SpeID", productSpecificationMapping.SpecId),
                new SqlParameter("@SpecTypeId", productSpecificationMapping.SpecTypeId),
                new SqlParameter("@SpecValueId", productSpecificationMapping.SpecValueId),
                new SqlParameter("@Value", productSpecificationMapping.Value),
                new SqlParameter("@fileName", productSpecificationMapping.FileName),
                new SqlParameter("@modifiedBy", productSpecificationMapping.ModifiedBy),
                new SqlParameter("@modifiedAt", productSpecificationMapping.ModifiedAt)
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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ProductSpecificationMapping, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<ProductSpecificationMapping>>> get(ProductSpecificationMapping productSpecificationMapping, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                 new SqlParameter("@mode", Mode),
               
                new SqlParameter("@id", productSpecificationMapping.Id),
                new SqlParameter("@CatId", productSpecificationMapping.CatId),
                new SqlParameter("@ProductId", productSpecificationMapping.ProductID),
                new SqlParameter("@SpecId", productSpecificationMapping.SpecId),
                new SqlParameter("@SpecTypeId", productSpecificationMapping.SpecTypeId),
                new SqlParameter("@SpecValueId", productSpecificationMapping.SpecValueId),
                new SqlParameter("@Value", productSpecificationMapping.Value),

                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@PageSize", PageSize),

            };
                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int; 

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetProductSpecificationMapping, productSpecificationMappingParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ProductSpecificationMapping>> productSpecificationMappingParserAsync(DbDataReader reader)
        {
            List<ProductSpecificationMapping> lstSpec = new List<ProductSpecificationMapping>();
            while (await reader.ReadAsync())
            {
                lstSpec.Add(new ProductSpecificationMapping()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),

                    CatId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CatId"))),
                    ProductID = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ProductId"))),
                    SpecId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SpecId"))),
                    SpecTypeId = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SpecTypeId"))),
                    SpecValueId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecValueId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("SpecValueId"))),
                    Value = Convert.ToString(reader.GetValue(reader.GetOrdinal("Value"))),
                    FileName = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("FileName")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("FileName"))),
                    SpecificationName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificationName"))),
                    SpecificationTypeName = Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecificationTypeName"))),
                    
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    
                });
            }
            return lstSpec;
        }

    }
}
