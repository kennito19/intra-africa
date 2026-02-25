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
    public class TaxTypeRepository : ITaxTypeRespository
    {
        private readonly string _connectionString;

        public TaxTypeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> AddTaxType(TaxTypeLibrary taxTypeLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO taxtypelibrary (TaxType, ParentId, CreatedBy, CreatedAt, IsDeleted)
VALUES (@taxType, @parentId, @createdBy, @createdAt, 0);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@taxType", (object?)taxTypeLibrary.TaxType ?? string.Empty);
                var parentId = taxTypeLibrary.ParentId.HasValue && taxTypeLibrary.ParentId.Value > 0
                    ? taxTypeLibrary.ParentId.Value
                    : (object)DBNull.Value;
                cmd.Parameters.AddWithValue("@parentId", parentId);
                cmd.Parameters.AddWithValue("@createdBy", (object?)taxTypeLibrary.CreatedBy ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdAt", taxTypeLibrary.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> UpdateTaxType(TaxTypeLibrary taxTypeLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE taxtypelibrary
SET TaxType = @taxType,
    ParentId = @parentId,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", taxTypeLibrary.Id);
                cmd.Parameters.AddWithValue("@taxType", (object?)taxTypeLibrary.TaxType ?? string.Empty);
                var parentId = taxTypeLibrary.ParentId.HasValue && taxTypeLibrary.ParentId.Value > 0
                    ? taxTypeLibrary.ParentId.Value
                    : (object)DBNull.Value;
                cmd.Parameters.AddWithValue("@parentId", parentId);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)taxTypeLibrary.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", (object?)taxTypeLibrary.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = taxTypeLibrary.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> DeleteTaxType(TaxTypeLibrary taxTypeLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE taxtypelibrary
SET IsDeleted = 1,
    DeletedBy = @deletedBy,
    DeletedAt = @deletedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", taxTypeLibrary.Id);
                cmd.Parameters.AddWithValue("@deletedBy", (object?)taxTypeLibrary.DeletedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deletedAt", (object?)taxTypeLibrary.DeletedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = taxTypeLibrary.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<TaxTypeLibrary>>> GetTaxType(TaxTypeLibrary taxTypeLibrary, bool Getparent, bool Getchild, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (taxTypeLibrary.Id > 0)
                {
                    where.Add("t.Id = @id");
                    cmd.Parameters.AddWithValue("@id", taxTypeLibrary.Id);
                }
                if (!string.IsNullOrWhiteSpace(taxTypeLibrary.TaxType))
                {
                    where.Add("t.TaxType LIKE @taxType");
                    cmd.Parameters.AddWithValue("@taxType", $"%{taxTypeLibrary.TaxType}%");
                }
                if (taxTypeLibrary.ParentId.HasValue && taxTypeLibrary.ParentId.Value > 0)
                {
                    where.Add("t.ParentId = @parentId");
                    cmd.Parameters.AddWithValue("@parentId", taxTypeLibrary.ParentId.Value);
                }

                if (Getparent)
                {
                    where.Add("t.ParentId IS NULL");
                }
                if (Getchild)
                {
                    where.Add("t.ParentId IS NOT NULL");
                }

                where.Add("t.IsDeleted = @isDeleted");
                cmd.Parameters.AddWithValue("@isDeleted", taxTypeLibrary.IsDeleted);

                if (!string.IsNullOrWhiteSpace(taxTypeLibrary.Searchtext))
                {
                    where.Add("(t.TaxType LIKE @search OR p.TaxType LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{taxTypeLibrary.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT COUNT(1)
FROM taxtypelibrary t
LEFT JOIN taxtypelibrary p ON p.Id = t.ParentId
{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);
                var offset = (safePageIndex - 1) * safePageSize;

                var items = new List<TaxTypeLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT
  t.Id, t.TaxType, t.ParentId, t.CreatedBy, t.CreatedAt, t.ModifiedBy, t.ModifiedAt, t.DeletedBy, t.DeletedAt, t.IsDeleted,
  p.TaxType AS ParentName
FROM taxtypelibrary t
LEFT JOIN taxtypelibrary p ON p.Id = t.ParentId
{whereClause}
ORDER BY t.Id DESC
LIMIT @offset, @pageSize;";

                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        items.Add(new TaxTypeLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            TaxType = reader.IsDBNull(reader.GetOrdinal("TaxType")) ? null : reader.GetString(reader.GetOrdinal("TaxType")),
                            ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? null : reader.GetInt32(reader.GetOrdinal("ParentId")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            DeletedBy = reader.IsDBNull(reader.GetOrdinal("DeletedBy")) ? null : reader.GetString(reader.GetOrdinal("DeletedBy")),
                            DeletedAt = reader.IsDBNull(reader.GetOrdinal("DeletedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("DeletedAt")),
                            IsDeleted = !reader.IsDBNull(reader.GetOrdinal("IsDeleted")) && reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                            ParentName = reader.IsDBNull(reader.GetOrdinal("ParentName")) ? null : reader.GetString(reader.GetOrdinal("ParentName"))
                        });
                    }
                }

                return new BaseResponse<List<TaxTypeLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<TaxTypeLibrary>> { code = 400, message = ex.Message, data = new List<TaxTypeLibrary>() };
            }
        }
    }
}
