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
    public class ManageAppConfigRepository : IManageAppConfigRepository
    {
        private readonly string _connectionString;

        public ManageAppConfigRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ManageAppConfig config)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var tx = await con.BeginTransactionAsync();

                const string insertKeySql = @"
INSERT INTO manageconfigkey (Name, CreatedAt, CreatedBy)
VALUES (@name, @createdAt, @createdBy);
SELECT LAST_INSERT_ID();";

                await using var insertKey = new MySqlCommand(insertKeySql, con, (MySqlTransaction)tx);
                insertKey.Parameters.AddWithValue("@name", (object?)config.Name ?? string.Empty);
                insertKey.Parameters.AddWithValue("@createdAt", config.CreatedAt ?? DateTime.Now);
                insertKey.Parameters.AddWithValue("@createdBy", (object?)config.CreatedBy ?? DBNull.Value);

                var keyId = Convert.ToInt64(await insertKey.ExecuteScalarAsync() ?? 0);

                const string insertValueSql = @"
INSERT INTO manageconfigvalues (KeyId, Value, CreatedAt, CreatedBy)
VALUES (@keyId, @value, @createdAt, @createdBy);";

                await using var insertValue = new MySqlCommand(insertValueSql, con, (MySqlTransaction)tx);
                insertValue.Parameters.AddWithValue("@keyId", keyId);
                insertValue.Parameters.AddWithValue("@value", (object?)config.Value ?? string.Empty);
                insertValue.Parameters.AddWithValue("@createdAt", config.CreatedAt ?? DateTime.Now);
                insertValue.Parameters.AddWithValue("@createdBy", (object?)config.CreatedBy ?? DBNull.Value);
                await insertValue.ExecuteNonQueryAsync();

                await tx.CommitAsync();
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = keyId };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(ManageAppConfig config)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var tx = await con.BeginTransactionAsync();

                const string updateKeySql = @"
UPDATE manageconfigkey
SET Name = @name,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var updateKey = new MySqlCommand(updateKeySql, con, (MySqlTransaction)tx);
                updateKey.Parameters.AddWithValue("@id", config.Id);
                updateKey.Parameters.AddWithValue("@name", (object?)config.Name ?? string.Empty);
                updateKey.Parameters.AddWithValue("@modifiedBy", (object?)config.ModifiedBy ?? DBNull.Value);
                updateKey.Parameters.AddWithValue("@modifiedAt", config.ModifiedAt ?? DateTime.Now);
                var affected = await updateKey.ExecuteNonQueryAsync();

                if (!string.IsNullOrWhiteSpace(config.Value))
                {
                    const string updateValueSql = @"
UPDATE manageconfigvalues
SET Value = @value,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE KeyId = @keyId
ORDER BY Id DESC
LIMIT 1;";

                    await using var updateValue = new MySqlCommand(updateValueSql, con, (MySqlTransaction)tx);
                    updateValue.Parameters.AddWithValue("@keyId", config.Id);
                    updateValue.Parameters.AddWithValue("@value", config.Value);
                    updateValue.Parameters.AddWithValue("@modifiedBy", (object?)config.ModifiedBy ?? DBNull.Value);
                    updateValue.Parameters.AddWithValue("@modifiedAt", config.ModifiedAt ?? DateTime.Now);

                    var valueAffected = await updateValue.ExecuteNonQueryAsync();
                    if (valueAffected == 0)
                    {
                        const string insertValueSql = @"
INSERT INTO manageconfigvalues (KeyId, Value, CreatedAt, CreatedBy)
VALUES (@keyId, @value, @createdAt, @createdBy);";

                        await using var insertValue = new MySqlCommand(insertValueSql, con, (MySqlTransaction)tx);
                        insertValue.Parameters.AddWithValue("@keyId", config.Id);
                        insertValue.Parameters.AddWithValue("@value", config.Value);
                        insertValue.Parameters.AddWithValue("@createdAt", config.ModifiedAt ?? DateTime.Now);
                        insertValue.Parameters.AddWithValue("@createdBy", (object?)config.ModifiedBy ?? DBNull.Value);
                        await insertValue.ExecuteNonQueryAsync();
                    }
                }

                await tx.CommitAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = config.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageAppConfig config)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var tx = await con.BeginTransactionAsync();

                const string deleteValuesSql = "DELETE FROM manageconfigvalues WHERE KeyId = @id;";
                await using (var deleteValues = new MySqlCommand(deleteValuesSql, con, (MySqlTransaction)tx))
                {
                    deleteValues.Parameters.AddWithValue("@id", config.Id);
                    await deleteValues.ExecuteNonQueryAsync();
                }

                const string deleteKeySql = "DELETE FROM manageconfigkey WHERE Id = @id;";
                await using var deleteKey = new MySqlCommand(deleteKeySql, con, (MySqlTransaction)tx);
                deleteKey.Parameters.AddWithValue("@id", config.Id);
                var affected = await deleteKey.ExecuteNonQueryAsync();

                await tx.CommitAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = config.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ManageAppConfig>>> get(ManageAppConfig config, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (config.Id > 0)
                {
                    where.Add("k.Id = @id");
                    cmd.Parameters.AddWithValue("@id", config.Id);
                }
                if (!string.IsNullOrWhiteSpace(config.Name))
                {
                    where.Add("k.Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{config.Name}%");
                }
                if (!string.IsNullOrWhiteSpace(config.Searchtext))
                {
                    where.Add("(k.Name LIKE @search OR IFNULL((SELECT mv.Value FROM manageconfigvalues mv WHERE mv.KeyId = k.Id ORDER BY mv.Id DESC LIMIT 1),'') LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{config.Searchtext}%");
                }
                if (!string.IsNullOrWhiteSpace(config.Status) && !config.Status.Equals("active", StringComparison.OrdinalIgnoreCase))
                {
                    return new BaseResponse<List<ManageAppConfig>> { code = 204, message = "Record does not Exist.", data = new List<ManageAppConfig>() };
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT COUNT(1)
FROM manageconfigkey k
{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var offset = (safePageIndex - 1) * safePageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);

                var items = new List<ManageAppConfig>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT
  k.Id,
  k.Name,
  (SELECT mv.Value FROM manageconfigvalues mv WHERE mv.KeyId = k.Id ORDER BY mv.Id DESC LIMIT 1) AS Value,
  'Active' AS Status,
  k.CreatedBy,
  k.CreatedAt,
  k.ModifiedBy,
  k.ModifiedAt
FROM manageconfigkey k
{whereClause}
ORDER BY k.Id DESC
LIMIT @offset, @pageSize;";
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        items.Add(new ManageAppConfig
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                            Value = reader.IsDBNull(reader.GetOrdinal("Value")) ? null : reader.GetString(reader.GetOrdinal("Value")),
                            Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "Active" : reader.GetString(reader.GetOrdinal("Status")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt"))
                        });
                    }
                }

                return new BaseResponse<List<ManageAppConfig>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageAppConfig>> { code = 400, message = ex.Message, data = new List<ManageAppConfig>() };
            }
        }
    }
}
