using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Domain.Entity;
using User.Domain;
using User.Infrastructure.Helper;

namespace User.Infrastructure.Repository
{
    public class BrandCountRepository : IBrandCountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;
        public BrandCountRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }

        public async Task<BaseResponse<List<BrandCounts>>> get(string? sellerId, string? days)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@date", DateTime.Now.ToString()),
                    new SqlParameter("@days", days),
                    new SqlParameter("@sellerid", sellerId),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetBrandCount, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<BrandCounts>> LayoutParserAsync(DbDataReader reader)
        {
            List<BrandCounts> lstLayouts = new List<BrandCounts>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new BrandCounts()
                {
                    Total = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalBrands")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalBrands"))),
                    Active = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ActiveBrands")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ActiveBrands"))),
                    Inactive = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("InactiveBrands")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("InactiveBrands"))),
                    InRequest = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("RequestedBrands")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RequestedBrands"))),
                });
            }
            return lstLayouts;
        }
    }
}
