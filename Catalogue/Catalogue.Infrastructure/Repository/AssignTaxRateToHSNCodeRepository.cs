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
    public class AssignTaxRateToHSNCodeRepository : ITaxRateToHSNCodeRepository
    {
        private readonly string _connectionString;

        public AssignTaxRateToHSNCodeRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(AssignTaxRateToHSNCodeLibrary rateToHSNCode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO assigntaxratetohsncode (HSNCodeId, TaxValueId, CreatedBy, CreatedAt)
VALUES (@hsnCodeId, @taxValueId, @createdBy, @createdAt);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@hsnCodeId", rateToHSNCode.HsnCodeId);
                cmd.Parameters.AddWithValue("@taxValueId", rateToHSNCode.TaxValueId);
                cmd.Parameters.AddWithValue("@createdBy", (object?)rateToHSNCode.CreatedBy ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdAt", rateToHSNCode.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(AssignTaxRateToHSNCodeLibrary rateToHSNCode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE assigntaxratetohsncode
SET HSNCodeId = @hsnCodeId,
    TaxValueId = @taxValueId,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", rateToHSNCode.Id);
                cmd.Parameters.AddWithValue("@hsnCodeId", rateToHSNCode.HsnCodeId);
                cmd.Parameters.AddWithValue("@taxValueId", rateToHSNCode.TaxValueId);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)rateToHSNCode.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", (object?)rateToHSNCode.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = rateToHSNCode.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(AssignTaxRateToHSNCodeLibrary rateToHSNCode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = "DELETE FROM assigntaxratetohsncode WHERE Id = @id;";
                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", rateToHSNCode.Id);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = rateToHSNCode.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<AssignTaxRateToHSNCodeLibrary>>> get(AssignTaxRateToHSNCodeLibrary rateToHSNCode, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (rateToHSNCode.Id > 0)
                {
                    where.Add("a.Id = @id");
                    cmd.Parameters.AddWithValue("@id", rateToHSNCode.Id);
                }
                if (rateToHSNCode.HsnCodeId > 0)
                {
                    where.Add("a.HSNCodeId = @hsnCodeId");
                    cmd.Parameters.AddWithValue("@hsnCodeId", rateToHSNCode.HsnCodeId);
                }
                if (rateToHSNCode.TaxValueId > 0)
                {
                    where.Add("a.TaxValueId = @taxValueId");
                    cmd.Parameters.AddWithValue("@taxValueId", rateToHSNCode.TaxValueId);
                }
                if (!string.IsNullOrWhiteSpace(rateToHSNCode.HsnCode))
                {
                    where.Add("h.HSNCode LIKE @hsnCode");
                    cmd.Parameters.AddWithValue("@hsnCode", $"%{rateToHSNCode.HsnCode}%");
                }
                if (!string.IsNullOrWhiteSpace(rateToHSNCode.TaxName))
                {
                    where.Add("tv.Name LIKE @taxName");
                    cmd.Parameters.AddWithValue("@taxName", $"%{rateToHSNCode.TaxName}%");
                }
                if (!string.IsNullOrWhiteSpace(rateToHSNCode.SearchText))
                {
                    where.Add("(h.HSNCode LIKE @search OR tv.Name LIKE @search OR tt.TaxType LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{rateToHSNCode.SearchText}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT COUNT(1)
FROM assigntaxratetohsncode a
LEFT JOIN hsncodelibrary h ON h.Id = a.HSNCodeId
LEFT JOIN taxtypevaluelibrary tv ON tv.Id = a.TaxValueId
LEFT JOIN taxtypelibrary tt ON tt.Id = tv.TaxTypeID
{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);
                var offset = (safePageIndex - 1) * safePageSize;

                var items = new List<AssignTaxRateToHSNCodeLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT
  a.Id, a.HSNCodeId, a.TaxValueId, a.CreatedBy, a.CreatedAt, a.ModifiedBy, a.ModifiedAt,
  h.HSNCode,
  tt.TaxType,
  tv.Name AS TaxName,
  tv.Value AS TaxTypeValue
FROM assigntaxratetohsncode a
LEFT JOIN hsncodelibrary h ON h.Id = a.HSNCodeId
LEFT JOIN taxtypevaluelibrary tv ON tv.Id = a.TaxValueId
LEFT JOIN taxtypelibrary tt ON tt.Id = tv.TaxTypeID
{whereClause}
ORDER BY a.Id DESC
LIMIT @offset, @pageSize;";

                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        var taxType = reader.IsDBNull(reader.GetOrdinal("TaxType")) ? string.Empty : reader.GetString(reader.GetOrdinal("TaxType"));
                        var taxName = reader.IsDBNull(reader.GetOrdinal("TaxName")) ? string.Empty : reader.GetString(reader.GetOrdinal("TaxName"));
                        var taxValue = reader.IsDBNull(reader.GetOrdinal("TaxTypeValue")) ? string.Empty : reader.GetString(reader.GetOrdinal("TaxTypeValue"));

                        items.Add(new AssignTaxRateToHSNCodeLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            HsnCodeId = reader.GetInt32(reader.GetOrdinal("HSNCodeId")),
                            TaxValueId = reader.GetInt32(reader.GetOrdinal("TaxValueId")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            HsnCode = reader.IsDBNull(reader.GetOrdinal("HSNCode")) ? null : reader.GetString(reader.GetOrdinal("HSNCode")),
                            TaxType = taxType,
                            TaxName = taxName,
                            TaxTypeValue = taxValue,
                            DisplayName = string.IsNullOrWhiteSpace(taxType) && string.IsNullOrWhiteSpace(taxName)
                                ? taxValue
                                : $"{taxType} - {taxName} ({taxValue})"
                        });
                    }
                }

                return new BaseResponse<List<AssignTaxRateToHSNCodeLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AssignTaxRateToHSNCodeLibrary>> { code = 400, message = ex.Message, data = new List<AssignTaxRateToHSNCodeLibrary>() };
            }
        }
    }
}
