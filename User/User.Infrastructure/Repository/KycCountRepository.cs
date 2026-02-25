using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using MySqlConnector;
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
        private readonly string _connectionString;

        public KycCountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }
        public async Task<BaseResponse<List<KycCounts>>> get(string? days)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                await using var cmd = new MySqlCommand();
                cmd.Connection = con;

                var dateFilter = string.Empty;
                if (int.TryParse(days, out var dayCount) && dayCount > 0)
                {
                    dateFilter = " AND k.CreatedAt >= DATE_SUB(NOW(), INTERVAL @days DAY) ";
                    cmd.Parameters.AddWithValue("@days", dayCount);
                }

                cmd.CommandText = $@"
SELECT
    COUNT(1) AS TotalKYC,
    COUNT(CASE WHEN LOWER(COALESCE(k.Status,'')) IN ('approved','complete','completed') THEN 1 END) AS CompleteKYC,
    COUNT(CASE WHEN LOWER(COALESCE(k.Status,'')) IN ('pending','inprogress','in progress') THEN 1 END) AS PendingKYC,
    COUNT(CASE WHEN LOWER(COALESCE(k.Status,'')) IN ('not approved','rejected','inactive') THEN 1 END) AS NotApprovedKYC
FROM KYCDetails k
WHERE COALESCE(k.IsDeleted,0) = 0
{dateFilter};";

                await using var reader = await cmd.ExecuteReaderAsync();
                var result = new List<KycCounts>();
                if (await reader.ReadAsync())
                {
                    result.Add(new KycCounts
                    {
                        Total = reader.IsDBNull("TotalKYC") ? 0 : reader.GetInt32("TotalKYC"),
                        Completed = reader.IsDBNull("CompleteKYC") ? 0 : reader.GetInt32("CompleteKYC"),
                        Pending = reader.IsDBNull("PendingKYC") ? 0 : reader.GetInt32("PendingKYC"),
                        NotApproved = reader.IsDBNull("NotApprovedKYC") ? 0 : reader.GetInt32("NotApprovedKYC")
                    });
                }

                return new BaseResponse<List<KycCounts>>
                {
                    code = 200,
                    message = "Record bind successfully.",
                    data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<KycCounts>>
                {
                    code = 400,
                    message = ex.Message,
                    data = new List<KycCounts>()
                };
            }
        }
    }
}
