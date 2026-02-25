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
    public class ManageConfigValueRepository : IManageConfigValueRepository
    {
        private readonly string _connectionString;

        public ManageConfigValueRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ManageConfigValueLibrary configValue)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO manageconfigvalues (KeyId, Value, CreatedAt, CreatedBy)
VALUES (@keyId, @value, @createdAt, @createdBy);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@keyId", configValue.KeyId);
                cmd.Parameters.AddWithValue("@value", (object?)configValue.Value ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdAt", configValue.CreatedAt ?? DateTime.Now);
                cmd.Parameters.AddWithValue("@createdBy", (object?)configValue.CreatedBy ?? DBNull.Value);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(ManageConfigValueLibrary configValue)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE manageconfigvalues
SET KeyId = @keyId,
    Value = @value,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", configValue.Id);
                cmd.Parameters.AddWithValue("@keyId", configValue.KeyId);
                cmd.Parameters.AddWithValue("@value", (object?)configValue.Value ?? string.Empty);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)configValue.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", configValue.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = configValue.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageConfigValueLibrary configValue)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = "DELETE FROM manageconfigvalues WHERE Id = @id;";
                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", configValue.Id);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = configValue.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ManageConfigValueLibrary>>> get(ManageConfigValueLibrary configValue, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (configValue.Id > 0)
                {
                    where.Add("v.Id = @id");
                    cmd.Parameters.AddWithValue("@id", configValue.Id);
                }
                if (configValue.KeyId > 0)
                {
                    where.Add("v.KeyId = @keyId");
                    cmd.Parameters.AddWithValue("@keyId", configValue.KeyId);
                }
                if (!string.IsNullOrWhiteSpace(configValue.KeyName))
                {
                    where.Add("k.Name LIKE @keyName");
                    cmd.Parameters.AddWithValue("@keyName", $"%{configValue.KeyName}%");
                }
                if (!string.IsNullOrWhiteSpace(configValue.Value))
                {
                    where.Add("v.Value LIKE @value");
                    cmd.Parameters.AddWithValue("@value", $"%{configValue.Value}%");
                }
                if (!string.IsNullOrWhiteSpace(configValue.SearchText))
                {
                    where.Add("(k.Name LIKE @search OR v.Value LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{configValue.SearchText}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT COUNT(1)
FROM manageconfigvalues v
INNER JOIN manageconfigkey k ON k.Id = v.KeyId
{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var offset = (safePageIndex - 1) * safePageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);

                var items = new List<ManageConfigValueLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT
  v.Id, v.KeyId, v.Value, v.CreatedBy, v.CreatedAt, v.ModifiedBy, v.ModifiedAt,
  k.Name AS KeyName
FROM manageconfigvalues v
INNER JOIN manageconfigkey k ON k.Id = v.KeyId
{whereClause}
ORDER BY v.Id DESC
LIMIT @offset, @pageSize;";
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        items.Add(new ManageConfigValueLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            KeyId = reader.GetInt32(reader.GetOrdinal("KeyId")),
                            Value = reader.IsDBNull(reader.GetOrdinal("Value")) ? null : reader.GetString(reader.GetOrdinal("Value")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            KeyName = reader.IsDBNull(reader.GetOrdinal("KeyName")) ? null : reader.GetString(reader.GetOrdinal("KeyName"))
                        });
                    }
                }

                return new BaseResponse<List<ManageConfigValueLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageConfigValueLibrary>> { code = 400, message = ex.Message, data = new List<ManageConfigValueLibrary>() };
            }
        }
    }
}
