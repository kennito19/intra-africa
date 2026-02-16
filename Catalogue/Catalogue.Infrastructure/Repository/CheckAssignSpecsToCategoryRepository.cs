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
    public class CheckAssignSpecsToCategoryRepository : ICheckAssignSpecsToCategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;
            
        public CheckAssignSpecsToCategoryRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSpecToCat(int assignSpecId, bool? multiSeller = false)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode", "1"),
                    new SqlParameter("@assignSpecId", assignSpecId),
                    new SqlParameter("@multiSeller", multiSeller),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.CheckAssignSpecsToCategory, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSizeValuesToCat(int assignSpecId, int sizeTypeId, bool? multiSeller = false)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode", "2"),
                    new SqlParameter("@assignSpecId", assignSpecId),
                    new SqlParameter("@sizeTypeId", sizeTypeId),
                    new SqlParameter("@multiSeller", multiSeller),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.CheckAssignSpecsToCategory, sizeLayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSpecvaluesToCat(int assignSpecId, int specTypeId, bool? multiSeller = false)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode", "3"),
                    new SqlParameter("@assignSpecId", assignSpecId),
                    new SqlParameter("@specTypeId", specTypeId),
                    new SqlParameter("@multiSeller", multiSeller),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.CheckAssignSpecsToCategory, specLayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkSizeType(int assignSpecId, int sizeTypeId)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode", "4"),
                    new SqlParameter("@assignSpecId", assignSpecId),
                    new SqlParameter("@sizeTypeId", sizeTypeId),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.CheckAssignSpecsToCategory, sizeTypeLayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkSpecType(int assignSpecId, int specTypeId)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@mode", "5"),
                    new SqlParameter("@assignSpecId", assignSpecId),
                    new SqlParameter("@specTypeId", specTypeId),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.CheckAssignSpecsToCategory, specTypeLayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<CheckAssignSpecsToCategory>> LayoutParserAsync(DbDataReader reader)
        {
            List<CheckAssignSpecsToCategory> lstLayouts = new List<CheckAssignSpecsToCategory>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new CheckAssignSpecsToCategory()
                {
                    AllowSize = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsAllowSize")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSize"))),
                    AllowColor = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsAllowColor")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowColor"))),
                    AllowSpecifications = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsAllowSpecs")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSpecs"))),
                    AllowExpiryDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsAllowExpiryDate")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowExpiryDate"))),
                    AllowPriceVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsAllowPriceVariant")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowPriceVariant"))),
                    AllowColorVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsAllowColorVariant")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowColorVariant")))
                });
            }
            return lstLayouts;
        }

        private async Task<List<CheckAssignSpecsToCategory>> sizeLayoutParserAsync(DbDataReader reader)
        {
            List<CheckAssignSpecsToCategory> lstLayouts = new List<CheckAssignSpecsToCategory>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new CheckAssignSpecsToCategory()
                {
                    SizeIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeValues")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SizeValues"))),
                    AllowSizeVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsAllowSizeVariant")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSizeVariant")))
                });
            }
            return lstLayouts;
        }

        private async Task<List<CheckAssignSpecsToCategory>> specLayoutParserAsync(DbDataReader reader)
        {
            List<CheckAssignSpecsToCategory> lstLayouts = new List<CheckAssignSpecsToCategory>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new CheckAssignSpecsToCategory()
                {
                    SpecValueIds = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecValues")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecValues"))),
                    AllowSpecVariant = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("IsAllowSpecVariant")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsAllowSpecVariant")))
                });
            }
            return lstLayouts;
        }

        private async Task<List<CheckAssignSpecsToCategory>> sizeTypeLayoutParserAsync(DbDataReader reader)
        {
            List<CheckAssignSpecsToCategory> lstLayouts = new List<CheckAssignSpecsToCategory>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new CheckAssignSpecsToCategory()
                {
                    IsAllowDeleteSizeType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("sizeTypeDeleted")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("sizeTypeDeleted")))
                });
            }
            return lstLayouts;
        }

        private async Task<List<CheckAssignSpecsToCategory>> specTypeLayoutParserAsync(DbDataReader reader)
        {
            List<CheckAssignSpecsToCategory> lstLayouts = new List<CheckAssignSpecsToCategory>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new CheckAssignSpecsToCategory()
                {
                    IsAllowDeleteSpecType = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SpecTypeDeleted")))) ? null : Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("SpecTypeDeleted")))
                });
            }
            return lstLayouts;
        }

    }
}
