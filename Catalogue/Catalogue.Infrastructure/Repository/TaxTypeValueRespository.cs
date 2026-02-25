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
    public class TaxTypeValueRespository : ITaxTypeValueRepository
    {
        private readonly string _connectionString;

        public TaxTypeValueRespository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> AddTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO taxtypevaluelibrary (TaxTypeID, Name, Value, CreatedBy, CreatedAt, IsDeleted)
VALUES (@taxTypeId, @name, @value, @createdBy, @createdAt, 0);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@taxTypeId", taxTypeValueLibrary.TaxTypeID);
                cmd.Parameters.AddWithValue("@name", (object?)taxTypeValueLibrary.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@value", (object?)taxTypeValueLibrary.Value ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdBy", (object?)taxTypeValueLibrary.CreatedBy ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdAt", taxTypeValueLibrary.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> UpdateTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE taxtypevaluelibrary
SET TaxTypeID = @taxTypeId,
    Name = @name,
    Value = @value,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", taxTypeValueLibrary.Id);
                cmd.Parameters.AddWithValue("@taxTypeId", taxTypeValueLibrary.TaxTypeID);
                cmd.Parameters.AddWithValue("@name", (object?)taxTypeValueLibrary.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@value", (object?)taxTypeValueLibrary.Value ?? string.Empty);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)taxTypeValueLibrary.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", (object?)taxTypeValueLibrary.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = taxTypeValueLibrary.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> DeleteTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE taxtypevaluelibrary
SET IsDeleted = 1,
    DeletedBy = @deletedBy,
    DeletedAt = @deletedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", taxTypeValueLibrary.Id);
                cmd.Parameters.AddWithValue("@deletedBy", (object?)taxTypeValueLibrary.DeletedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deletedAt", (object?)taxTypeValueLibrary.DeletedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = taxTypeValueLibrary.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<TaxTypeValueLibrary>>> GetTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (taxTypeValueLibrary.Id > 0)
                {
                    where.Add("v.Id = @id");
                    cmd.Parameters.AddWithValue("@id", taxTypeValueLibrary.Id);
                }
                if (taxTypeValueLibrary.TaxTypeID > 0)
                {
                    where.Add("v.TaxTypeID = @taxTypeId");
                    cmd.Parameters.AddWithValue("@taxTypeId", taxTypeValueLibrary.TaxTypeID);
                }
                if (!string.IsNullOrWhiteSpace(taxTypeValueLibrary.Name))
                {
                    where.Add("v.Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{taxTypeValueLibrary.Name}%");
                }

                where.Add("v.IsDeleted = @isDeleted");
                cmd.Parameters.AddWithValue("@isDeleted", taxTypeValueLibrary.IsDeleted);

                if (!string.IsNullOrWhiteSpace(taxTypeValueLibrary.Searchtext))
                {
                    where.Add("(v.Name LIKE @search OR v.Value LIKE @search OR t.TaxType LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{taxTypeValueLibrary.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT COUNT(1)
FROM taxtypevaluelibrary v
LEFT JOIN taxtypelibrary t ON t.Id = v.TaxTypeID
{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);
                var offset = (safePageIndex - 1) * safePageSize;

                var items = new List<TaxTypeValueLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT
  v.Id, v.TaxTypeID, v.Name, v.Value, v.CreatedBy, v.CreatedAt, v.ModifiedBy, v.ModifiedAt, v.DeletedBy, v.DeletedAt, v.IsDeleted,
  t.TaxType
FROM taxtypevaluelibrary v
LEFT JOIN taxtypelibrary t ON t.Id = v.TaxTypeID
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
                        items.Add(new TaxTypeValueLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            TaxTypeID = reader.GetInt32(reader.GetOrdinal("TaxTypeID")),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                            Value = reader.IsDBNull(reader.GetOrdinal("Value")) ? string.Empty : reader.GetString(reader.GetOrdinal("Value")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            DeletedBy = reader.IsDBNull(reader.GetOrdinal("DeletedBy")) ? null : reader.GetString(reader.GetOrdinal("DeletedBy")),
                            DeletedAt = reader.IsDBNull(reader.GetOrdinal("DeletedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("DeletedAt")),
                            IsDeleted = !reader.IsDBNull(reader.GetOrdinal("IsDeleted")) && reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                            TaxType = reader.IsDBNull(reader.GetOrdinal("TaxType")) ? null : reader.GetString(reader.GetOrdinal("TaxType"))
                        });
                    }
                }

                return new BaseResponse<List<TaxTypeValueLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<TaxTypeValueLibrary>> { code = 400, message = ex.Message, data = new List<TaxTypeValueLibrary>() };
            }
        }
    }
}
