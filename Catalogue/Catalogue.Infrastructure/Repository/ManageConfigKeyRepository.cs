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
    public class ManageConfigKeyRepository : IManageConfigKeyRepository
    {
        private readonly string _connectionString;

        public ManageConfigKeyRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ManageConfigKeyLibrary configKey)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO manageconfigkey (Name, CreatedAt, CreatedBy)
VALUES (@name, @createdAt, @createdBy);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", (object?)configKey.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdAt", configKey.CreatedAt ?? DateTime.Now);
                cmd.Parameters.AddWithValue("@createdBy", (object?)configKey.CreatedBy ?? DBNull.Value);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(ManageConfigKeyLibrary configKey)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE manageconfigkey
SET Name = @name,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", configKey.Id);
                cmd.Parameters.AddWithValue("@name", (object?)configKey.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)configKey.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", configKey.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = configKey.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageConfigKeyLibrary configKey)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var tx = await con.BeginTransactionAsync();

                const string deleteValuesSql = "DELETE FROM manageconfigvalues WHERE KeyId = @id;";
                await using (var deleteValues = new MySqlCommand(deleteValuesSql, con, (MySqlTransaction)tx))
                {
                    deleteValues.Parameters.AddWithValue("@id", configKey.Id);
                    await deleteValues.ExecuteNonQueryAsync();
                }

                const string deleteKeySql = "DELETE FROM manageconfigkey WHERE Id = @id;";
                await using var deleteKey = new MySqlCommand(deleteKeySql, con, (MySqlTransaction)tx);
                deleteKey.Parameters.AddWithValue("@id", configKey.Id);
                var affected = await deleteKey.ExecuteNonQueryAsync();

                await tx.CommitAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = configKey.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ManageConfigKeyLibrary>>> get(ManageConfigKeyLibrary configKey, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (configKey.Id > 0)
                {
                    where.Add("Id = @id");
                    cmd.Parameters.AddWithValue("@id", configKey.Id);
                }
                if (!string.IsNullOrWhiteSpace(configKey.Name))
                {
                    where.Add("Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{configKey.Name}%");
                }
                if (!string.IsNullOrWhiteSpace(configKey.Searchtext))
                {
                    where.Add("Name LIKE @search");
                    cmd.Parameters.AddWithValue("@search", $"%{configKey.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $"SELECT COUNT(1) FROM manageconfigkey{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var offset = (safePageIndex - 1) * safePageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);

                var items = new List<ManageConfigKeyLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT Id, Name, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt
FROM manageconfigkey
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
                        items.Add(new ManageConfigKeyLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt"))
                        });
                    }
                }

                return new BaseResponse<List<ManageConfigKeyLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageConfigKeyLibrary>> { code = 400, message = ex.Message, data = new List<ManageConfigKeyLibrary>() };
            }
        }
    }
}
