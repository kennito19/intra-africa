using Catalogue.Application.IRepositories;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class WeightSlabRepository : IWeightSlabRepository
    {
        private readonly string _connectionString;

        public WeightSlabRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(WeightSlabLibrary weightSlab)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO weightslablibrary (WeightSlab, LocalCharges, ZonalCharges, NationalCharges, CreatedBy, CreatedAt, IsDeleted)
VALUES (@weightSlab, @localCharges, @zonalCharges, @nationalCharges, @createdBy, @createdAt, 0);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@weightSlab", (object?)weightSlab.WeightSlab ?? string.Empty);
                cmd.Parameters.AddWithValue("@localCharges", (object?)weightSlab.LocalCharges ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@zonalCharges", (object?)weightSlab.ZonalCharges ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nationalCharges", (object?)weightSlab.NationalCharges ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdBy", (object?)weightSlab.CreatedBy ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdAt", weightSlab.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(WeightSlabLibrary weightSlab)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE weightslablibrary
SET WeightSlab = @weightSlab,
    LocalCharges = @localCharges,
    ZonalCharges = @zonalCharges,
    NationalCharges = @nationalCharges,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", weightSlab.Id);
                cmd.Parameters.AddWithValue("@weightSlab", (object?)weightSlab.WeightSlab ?? string.Empty);
                cmd.Parameters.AddWithValue("@localCharges", (object?)weightSlab.LocalCharges ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@zonalCharges", (object?)weightSlab.ZonalCharges ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@nationalCharges", (object?)weightSlab.NationalCharges ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)weightSlab.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", (object?)weightSlab.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = weightSlab.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(WeightSlabLibrary weightSlab)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE weightslablibrary
SET IsDeleted = 1,
    DeletedBy = @deletedBy,
    DeletedAt = @deletedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", weightSlab.Id);
                cmd.Parameters.AddWithValue("@deletedBy", (object?)weightSlab.DeletedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deletedAt", (object?)weightSlab.DeletedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = weightSlab.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<WeightSlabLibrary>>> get(WeightSlabLibrary weightSlab, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (weightSlab.Id > 0)
                {
                    where.Add("Id = @id");
                    cmd.Parameters.AddWithValue("@id", weightSlab.Id);
                }
                if (!string.IsNullOrWhiteSpace(weightSlab.WeightSlab))
                {
                    where.Add("WeightSlab LIKE @weightSlab");
                    cmd.Parameters.AddWithValue("@weightSlab", $"%{weightSlab.WeightSlab}%");
                }
                where.Add("IsDeleted = @isDeleted");
                cmd.Parameters.AddWithValue("@isDeleted", weightSlab.IsDeleted);
                if (!string.IsNullOrWhiteSpace(weightSlab.Searchtext))
                {
                    where.Add("WeightSlab LIKE @search");
                    cmd.Parameters.AddWithValue("@search", $"%{weightSlab.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $"SELECT COUNT(1) FROM weightslablibrary{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);
                var offset = (safePageIndex - 1) * safePageSize;

                var items = new List<WeightSlabLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT Id, WeightSlab, LocalCharges, ZonalCharges, NationalCharges, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt, DeletedBy, DeletedAt, IsDeleted
FROM weightslablibrary
{whereClause}
ORDER BY Id DESC
LIMIT @offset, @pageSize;";
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        items.Add(new WeightSlabLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            WeightSlab = reader.IsDBNull(reader.GetOrdinal("WeightSlab")) ? string.Empty : reader.GetString(reader.GetOrdinal("WeightSlab")),
                            LocalCharges = reader.IsDBNull(reader.GetOrdinal("LocalCharges")) ? null : reader.GetDecimal(reader.GetOrdinal("LocalCharges")),
                            ZonalCharges = reader.IsDBNull(reader.GetOrdinal("ZonalCharges")) ? null : reader.GetDecimal(reader.GetOrdinal("ZonalCharges")),
                            NationalCharges = reader.IsDBNull(reader.GetOrdinal("NationalCharges")) ? null : reader.GetDecimal(reader.GetOrdinal("NationalCharges")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            DeletedBy = reader.IsDBNull(reader.GetOrdinal("DeletedBy")) ? null : reader.GetString(reader.GetOrdinal("DeletedBy")),
                            DeletedAt = reader.IsDBNull(reader.GetOrdinal("DeletedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("DeletedAt")),
                            IsDeleted = !reader.IsDBNull(reader.GetOrdinal("IsDeleted")) && reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                        });
                    }
                }

                return new BaseResponse<List<WeightSlabLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<WeightSlabLibrary>> { code = 400, message = ex.Message, data = new List<WeightSlabLibrary>() };
            }
        }
    }
}
