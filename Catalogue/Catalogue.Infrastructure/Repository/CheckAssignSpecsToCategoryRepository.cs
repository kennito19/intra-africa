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
    public class CheckAssignSpecsToCategoryRepository : ICheckAssignSpecsToCategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;
            
        public CheckAssignSpecsToCategoryRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<List<CheckAssignSpecsToCategory>>> checkAssignSpecToCat(int assignSpecId, bool? multiSeller = false)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode", "1"),
                    new MySqlParameter("@assignSpecId", assignSpecId),
                    new MySqlParameter("@multiSeller", multiSeller),
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
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode", "2"),
                    new MySqlParameter("@assignSpecId", assignSpecId),
                    new MySqlParameter("@sizeTypeId", sizeTypeId),
                    new MySqlParameter("@multiSeller", multiSeller),
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
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode", "3"),
                    new MySqlParameter("@assignSpecId", assignSpecId),
                    new MySqlParameter("@specTypeId", specTypeId),
                    new MySqlParameter("@multiSeller", multiSeller),
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
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode", "4"),
                    new MySqlParameter("@assignSpecId", assignSpecId),
                    new MySqlParameter("@sizeTypeId", sizeTypeId),
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
                var sqlParams = new List<MySqlParameter>()
                {
                    new MySqlParameter("@mode", "5"),
                    new MySqlParameter("@assignSpecId", assignSpecId),
                    new MySqlParameter("@specTypeId", specTypeId),
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
