using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain;
using User.Infrastructure.Helper;
using User.Application.IRepositories;
using User.Domain.Entity;

namespace User.Infrastructure.Repository
{
    public class KycCountRepository : IKycCountRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        SqlConnection con;

        public KycCountRepository(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new SqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<List<KycCounts>>> get(string? days)
        {
            try
            {
                var sqlParams = new List<SqlParameter>()
                {
                    new SqlParameter("@date", DateTime.Now.ToString()),
                    new SqlParameter("@days", days),
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

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetKycCount, LayoutParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<KycCounts>> LayoutParserAsync(DbDataReader reader)
        {
            List<KycCounts> lstLayouts = new List<KycCounts>();
            while (await reader.ReadAsync())
            {
                lstLayouts.Add(new KycCounts()
                {
                    Total = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("TotalKYC")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("TotalKYC"))),
                    Completed = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CompleteKYC")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CompleteKYC"))),
                    Pending = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("PendingKYC")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PendingKYC"))),
                    NotApproved = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("NotApprovedKYC")))) ? 0 : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("NotApprovedKYC"))),
                });
            }
            return lstLayouts;
        }
    }
}
