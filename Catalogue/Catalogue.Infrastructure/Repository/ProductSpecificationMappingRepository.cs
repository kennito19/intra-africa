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
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode","add"),
                     
                    new MySqlParameter("@CatID", productSpecificationMapping.CatId),
                    new MySqlParameter("@ProductID", productSpecificationMapping.ProductID),
                    new MySqlParameter("@SpecID", productSpecificationMapping.SpecId),
                    new MySqlParameter("@SpecTypeId", productSpecificationMapping.SpecTypeId),
                    new MySqlParameter("@SpecValueId", productSpecificationMapping.SpecValueId),
                    new MySqlParameter("@Value", productSpecificationMapping.Value),
                    new MySqlParameter("@fileName", productSpecificationMapping.FileName),

                    new MySqlParameter("@createdBy", productSpecificationMapping.CreatedBy),
                    new MySqlParameter("@createdAt", productSpecificationMapping.CreatedAt),
                    new MySqlParameter("@modifiedBy", productSpecificationMapping.ModifiedBy),
                    new MySqlParameter("@modifiedAt", productSpecificationMapping.ModifiedAt)
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@ProductID", productSpecificationMapping.ProductID),
                 
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
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@id", productSpecificationMapping.Id),
                new MySqlParameter("@CatID", productSpecificationMapping.CatId),
                new MySqlParameter("@ProductID", productSpecificationMapping.ProductID),
                new MySqlParameter("@SpeID", productSpecificationMapping.SpecId),
                new MySqlParameter("@SpecTypeId", productSpecificationMapping.SpecTypeId),
                new MySqlParameter("@SpecValueId", productSpecificationMapping.SpecValueId),
                new MySqlParameter("@Value", productSpecificationMapping.Value),
                new MySqlParameter("@fileName", productSpecificationMapping.FileName),
                new MySqlParameter("@modifiedBy", productSpecificationMapping.ModifiedBy),
                new MySqlParameter("@modifiedAt", productSpecificationMapping.ModifiedAt)
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
                var sqlParams = new List<MySqlParameter>() {
                 new MySqlParameter("@mode", Mode),
               
                new MySqlParameter("@id", productSpecificationMapping.Id),
                new MySqlParameter("@CatId", productSpecificationMapping.CatId),
                new MySqlParameter("@ProductId", productSpecificationMapping.ProductID),
                new MySqlParameter("@SpecId", productSpecificationMapping.SpecId),
                new MySqlParameter("@SpecTypeId", productSpecificationMapping.SpecTypeId),
                new MySqlParameter("@SpecValueId", productSpecificationMapping.SpecValueId),
                new MySqlParameter("@Value", productSpecificationMapping.Value),

                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@PageSize", PageSize),

            };
                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32; 

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
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
