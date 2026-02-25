using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using MySqlConnector;
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
        private readonly string _connectionString;
        public BrandCountRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<List<BrandCounts>>> get(string? sellerId, string? days)
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
                    dateFilter = " AND b.CreatedAt >= DATE_SUB(NOW(), INTERVAL @days DAY) ";
                    cmd.Parameters.AddWithValue("@days", dayCount);
                }

                var sellerJoin = string.Empty;
                if (!string.IsNullOrWhiteSpace(sellerId))
                {
                    sellerJoin = " INNER JOIN AssignBrandToSeller abs ON abs.BrandId = b.ID AND abs.IsDeleted = 0 AND abs.SellerID = @sellerId ";
                    cmd.Parameters.AddWithValue("@sellerId", sellerId);
                }

                cmd.CommandText = $@"
SELECT
    COUNT(DISTINCT b.ID) AS TotalBrands,
    COUNT(DISTINCT CASE WHEN LOWER(COALESCE(b.Status,'')) = 'active' THEN b.ID END) AS ActiveBrands,
    COUNT(DISTINCT CASE WHEN LOWER(COALESCE(b.Status,'')) = 'inactive' THEN b.ID END) AS InactiveBrands,
    COUNT(DISTINCT CASE WHEN LOWER(COALESCE(b.Status,'')) IN ('request for approval','requested','in request') THEN b.ID END) AS RequestedBrands
FROM Brand b
{sellerJoin}
WHERE b.IsDeleted = 0
{dateFilter};";

                await using var reader = await cmd.ExecuteReaderAsync();
                var result = new List<BrandCounts>();
                if (await reader.ReadAsync())
                {
                    result.Add(new BrandCounts
                    {
                        Total = reader.IsDBNull("TotalBrands") ? 0 : reader.GetInt32("TotalBrands"),
                        Active = reader.IsDBNull("ActiveBrands") ? 0 : reader.GetInt32("ActiveBrands"),
                        Inactive = reader.IsDBNull("InactiveBrands") ? 0 : reader.GetInt32("InactiveBrands"),
                        InRequest = reader.IsDBNull("RequestedBrands") ? 0 : reader.GetInt32("RequestedBrands")
                    });
                }

                return new BaseResponse<List<BrandCounts>>
                {
                    code = 200,
                    message = "Record bind successfully.",
                    data = result
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<BrandCounts>>
                {
                    code = 400,
                    message = ex.Message,
                    data = new List<BrandCounts>()
                };
            }
        }
    }
}
