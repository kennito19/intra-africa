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
    public class TaxMappingRespository : ITaxMappingRespository
    {
        private readonly string _connectionString;

        public TaxMappingRespository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> AddTaxMapping(TaxMapping taxmap)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO taxmapping (TaxId, TaxTypeId, TaxMapBy, SpecificState, SpecificTaxTypeId, CreatedBy, CreatedAt, IsDeleted)
VALUES (@taxId, @taxTypeId, @taxMapBy, @specificState, @specificTaxTypeId, @createdBy, @createdAt, 0);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@taxId", taxmap.TaxId);
                cmd.Parameters.AddWithValue("@taxTypeId", taxmap.TaxTypeId);
                cmd.Parameters.AddWithValue("@taxMapBy", (object?)taxmap.TaxMapBy ?? string.Empty);
                cmd.Parameters.AddWithValue("@specificState", (object?)taxmap.SpecificState ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@specificTaxTypeId", (object?)taxmap.SpecificTaxTypeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdBy", (object?)taxmap.CreatedBy ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdAt", taxmap.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> UpdateTaxMapping(TaxMapping taxmap)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE taxmapping
SET TaxId = @taxId,
    TaxTypeId = @taxTypeId,
    TaxMapBy = @taxMapBy,
    SpecificState = @specificState,
    SpecificTaxTypeId = @specificTaxTypeId,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", taxmap.Id);
                cmd.Parameters.AddWithValue("@taxId", taxmap.TaxId);
                cmd.Parameters.AddWithValue("@taxTypeId", taxmap.TaxTypeId);
                cmd.Parameters.AddWithValue("@taxMapBy", (object?)taxmap.TaxMapBy ?? string.Empty);
                cmd.Parameters.AddWithValue("@specificState", (object?)taxmap.SpecificState ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@specificTaxTypeId", (object?)taxmap.SpecificTaxTypeId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)taxmap.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", (object?)taxmap.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = taxmap.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> DeleteTaxMapping(TaxMapping taxmap)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE taxmapping
SET IsDeleted = 1,
    DeletedBy = @deletedBy,
    DeletedAt = @deletedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", taxmap.Id);
                cmd.Parameters.AddWithValue("@deletedBy", (object?)taxmap.DeletedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deletedAt", (object?)taxmap.DeletedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = taxmap.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<TaxMapping>>> GetTaxMapping(TaxMapping taxmap, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (taxmap.Id > 0)
                {
                    where.Add("m.Id = @id");
                    cmd.Parameters.AddWithValue("@id", taxmap.Id);
                }
                if (taxmap.TaxId > 0)
                {
                    where.Add("m.TaxId = @taxId");
                    cmd.Parameters.AddWithValue("@taxId", taxmap.TaxId);
                }
                if (taxmap.TaxTypeId > 0)
                {
                    where.Add("m.TaxTypeId = @taxTypeId");
                    cmd.Parameters.AddWithValue("@taxTypeId", taxmap.TaxTypeId);
                }
                if (!string.IsNullOrWhiteSpace(taxmap.TaxMapBy))
                {
                    where.Add("m.TaxMapBy = @taxMapBy");
                    cmd.Parameters.AddWithValue("@taxMapBy", taxmap.TaxMapBy);
                }

                where.Add("m.IsDeleted = @isDeleted");
                cmd.Parameters.AddWithValue("@isDeleted", taxmap.IsDeleted);

                if (!string.IsNullOrWhiteSpace(taxmap.Searchtext))
                {
                    where.Add("(tv.Name LIKE @search OR tt.TaxType LIKE @search OR st.TaxType LIKE @search OR m.SpecificState LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{taxmap.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT COUNT(1)
FROM taxmapping m
LEFT JOIN taxtypevaluelibrary tv ON tv.Id = m.TaxId
LEFT JOIN taxtypelibrary tt ON tt.Id = m.TaxTypeId
LEFT JOIN taxtypelibrary st ON st.Id = m.SpecificTaxTypeId
{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);
                var offset = (safePageIndex - 1) * safePageSize;

                var items = new List<TaxMapping>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT
  m.Id, m.TaxId, m.TaxTypeId, m.TaxMapBy, m.SpecificState, m.SpecificTaxTypeId,
  m.CreatedBy, m.CreatedAt, m.ModifiedBy, m.ModifiedAt, m.DeletedBy, m.DeletedAt, m.IsDeleted,
  tv.Name AS Tax,
  tt.TaxType,
  st.TaxType AS SpecificTaxType
FROM taxmapping m
LEFT JOIN taxtypevaluelibrary tv ON tv.Id = m.TaxId
LEFT JOIN taxtypelibrary tt ON tt.Id = m.TaxTypeId
LEFT JOIN taxtypelibrary st ON st.Id = m.SpecificTaxTypeId
{whereClause}
ORDER BY m.Id DESC
LIMIT @offset, @pageSize;";
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        items.Add(new TaxMapping
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            TaxId = reader.GetInt32(reader.GetOrdinal("TaxId")),
                            TaxTypeId = reader.GetInt32(reader.GetOrdinal("TaxTypeId")),
                            TaxMapBy = reader.IsDBNull(reader.GetOrdinal("TaxMapBy")) ? string.Empty : reader.GetString(reader.GetOrdinal("TaxMapBy")),
                            SpecificState = reader.IsDBNull(reader.GetOrdinal("SpecificState")) ? null : reader.GetString(reader.GetOrdinal("SpecificState")),
                            SpecificTaxTypeId = reader.IsDBNull(reader.GetOrdinal("SpecificTaxTypeId")) ? null : reader.GetInt32(reader.GetOrdinal("SpecificTaxTypeId")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            DeletedBy = reader.IsDBNull(reader.GetOrdinal("DeletedBy")) ? null : reader.GetString(reader.GetOrdinal("DeletedBy")),
                            DeletedAt = reader.IsDBNull(reader.GetOrdinal("DeletedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("DeletedAt")),
                            IsDeleted = !reader.IsDBNull(reader.GetOrdinal("IsDeleted")) && reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                            TaxValueId = reader.IsDBNull(reader.GetOrdinal("TaxId")) ? null : reader.GetInt32(reader.GetOrdinal("TaxId")),
                            Tax = reader.IsDBNull(reader.GetOrdinal("Tax")) ? null : reader.GetString(reader.GetOrdinal("Tax")),
                            TaxType = reader.IsDBNull(reader.GetOrdinal("TaxType")) ? null : reader.GetString(reader.GetOrdinal("TaxType")),
                            SpecificTaxType = reader.IsDBNull(reader.GetOrdinal("SpecificTaxType")) ? null : reader.GetString(reader.GetOrdinal("SpecificTaxType"))
                        });
                    }
                }

                return new BaseResponse<List<TaxMapping>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<TaxMapping>> { code = 400, message = ex.Message, data = new List<TaxMapping>() };
            }
        }
    }
}
