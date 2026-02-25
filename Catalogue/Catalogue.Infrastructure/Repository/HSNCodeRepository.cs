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
    public class HSNCodeRepository : IHSNCodeRepository
    {
        private readonly string _connectionString;

        public HSNCodeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> addHSNCode(HSNCodeLibrary hSNCodeLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO hsncodelibrary (HSNCode, Description, CreatedBy, CreatedAt, IsDeleted)
VALUES (@hsncode, @description, @createdBy, @createdAt, 0);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@hsncode", (object?)hSNCodeLibrary.HSNCode ?? string.Empty);
                cmd.Parameters.AddWithValue("@description", (object?)hSNCodeLibrary.Description ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdBy", (object?)hSNCodeLibrary.CreatedBy ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdAt", hSNCodeLibrary.CreatedAt == default ? DateTime.Now : hSNCodeLibrary.CreatedAt);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> updateHSNCode(HSNCodeLibrary hSNCodeLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE hsncodelibrary
SET HSNCode = @hsncode,
    Description = @description,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", hSNCodeLibrary.Id);
                cmd.Parameters.AddWithValue("@hsncode", (object?)hSNCodeLibrary.HSNCode ?? string.Empty);
                cmd.Parameters.AddWithValue("@description", (object?)hSNCodeLibrary.Description ?? string.Empty);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)hSNCodeLibrary.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", (object?)hSNCodeLibrary.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = hSNCodeLibrary.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> deleteHSNCode(HSNCodeLibrary hSNCodeLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE hsncodelibrary
SET IsDeleted = 1,
    DeletedBy = @deletedBy,
    DeletedAt = @deletedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", hSNCodeLibrary.Id);
                cmd.Parameters.AddWithValue("@deletedBy", (object?)hSNCodeLibrary.DeletedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deletedAt", (object?)hSNCodeLibrary.DeletedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = hSNCodeLibrary.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<HSNCodeLibrary>>> getHSNCode(HSNCodeLibrary hSNCodeLibrary, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (hSNCodeLibrary.Id > 0)
                {
                    where.Add("Id = @id");
                    cmd.Parameters.AddWithValue("@id", hSNCodeLibrary.Id);
                }
                if (!string.IsNullOrWhiteSpace(hSNCodeLibrary.HSNCode))
                {
                    where.Add("HSNCode LIKE @hsnCode");
                    cmd.Parameters.AddWithValue("@hsnCode", $"%{hSNCodeLibrary.HSNCode}%");
                }
                where.Add("IsDeleted = @isDeleted");
                cmd.Parameters.AddWithValue("@isDeleted", hSNCodeLibrary.IsDeleted);
                if (!string.IsNullOrWhiteSpace(hSNCodeLibrary.Searchtext))
                {
                    where.Add("(HSNCode LIKE @search OR Description LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{hSNCodeLibrary.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $"SELECT COUNT(1) FROM hsncodelibrary{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);
                var offset = (safePageIndex - 1) * safePageSize;

                var items = new List<HSNCodeLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT Id, HSNCode, Description, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt, DeletedBy, DeletedAt, IsDeleted
FROM hsncodelibrary
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
                        items.Add(new HSNCodeLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            HSNCode = reader.IsDBNull(reader.GetOrdinal("HSNCode")) ? string.Empty : reader.GetString(reader.GetOrdinal("HSNCode")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader.GetString(reader.GetOrdinal("Description")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            DeletedBy = reader.IsDBNull(reader.GetOrdinal("DeletedBy")) ? null : reader.GetString(reader.GetOrdinal("DeletedBy")),
                            DeletedAt = reader.IsDBNull(reader.GetOrdinal("DeletedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("DeletedAt")),
                            IsDeleted = !reader.IsDBNull(reader.GetOrdinal("IsDeleted")) && reader.GetBoolean(reader.GetOrdinal("IsDeleted"))
                        });
                    }
                }

                return new BaseResponse<List<HSNCodeLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<HSNCodeLibrary>> { code = 400, message = ex.Message, data = new List<HSNCodeLibrary>() };
            }
        }
    }
}
