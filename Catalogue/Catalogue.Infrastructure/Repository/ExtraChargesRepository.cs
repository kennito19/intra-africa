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
    public class ExtraChargesRepository : IExtraChargesRepository
    {
        private readonly string _connectionString;

        public ExtraChargesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> AddExtraCharges(ExtraChargesLibrary extraChargesLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO extrachargeslibrary
(CatID, ChargesPaidById, Name, ChargesOn, IsCompulsary, ChargesIn, PercentageValue, AmountValue, MaxAmountValue, CreatedBy, CreatedAt, IsDeleted)
VALUES
(@catId, @chargesPaidById, @name, @chargesOn, @isCompulsary, @chargesIn, @percentageValue, @amountValue, @maxAmountValue, @createdBy, @createdAt, 0);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@catId", (object?)extraChargesLibrary.CatID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@chargesPaidById", extraChargesLibrary.ChargesPaidByID);
                cmd.Parameters.AddWithValue("@name", (object?)extraChargesLibrary.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@chargesOn", (object?)extraChargesLibrary.ChargesOn ?? string.Empty);
                cmd.Parameters.AddWithValue("@isCompulsary", extraChargesLibrary.IsCompulsary ?? false);
                cmd.Parameters.AddWithValue("@chargesIn", (object?)extraChargesLibrary.ChargesIn ?? string.Empty);
                cmd.Parameters.AddWithValue("@percentageValue", extraChargesLibrary.PercentageValue);
                cmd.Parameters.AddWithValue("@amountValue", extraChargesLibrary.AmountValue);
                cmd.Parameters.AddWithValue("@maxAmountValue", (object?)extraChargesLibrary.MaxAmountValue ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdBy", (object?)extraChargesLibrary.CreatedBy ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdAt", extraChargesLibrary.CreatedAt == default ? DateTime.Now : extraChargesLibrary.CreatedAt);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> UpdateExtraCharges(ExtraChargesLibrary extraChargesLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE extrachargeslibrary
SET CatID = @catId,
    ChargesPaidById = @chargesPaidById,
    Name = @name,
    ChargesOn = @chargesOn,
    IsCompulsary = @isCompulsary,
    ChargesIn = @chargesIn,
    PercentageValue = @percentageValue,
    AmountValue = @amountValue,
    MaxAmountValue = @maxAmountValue,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", extraChargesLibrary.Id);
                cmd.Parameters.AddWithValue("@catId", (object?)extraChargesLibrary.CatID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@chargesPaidById", extraChargesLibrary.ChargesPaidByID);
                cmd.Parameters.AddWithValue("@name", (object?)extraChargesLibrary.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@chargesOn", (object?)extraChargesLibrary.ChargesOn ?? string.Empty);
                cmd.Parameters.AddWithValue("@isCompulsary", extraChargesLibrary.IsCompulsary ?? false);
                cmd.Parameters.AddWithValue("@chargesIn", (object?)extraChargesLibrary.ChargesIn ?? string.Empty);
                cmd.Parameters.AddWithValue("@percentageValue", extraChargesLibrary.PercentageValue);
                cmd.Parameters.AddWithValue("@amountValue", extraChargesLibrary.AmountValue);
                cmd.Parameters.AddWithValue("@maxAmountValue", (object?)extraChargesLibrary.MaxAmountValue ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)extraChargesLibrary.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", (object?)extraChargesLibrary.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = extraChargesLibrary.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> DeleteExtraCharges(ExtraChargesLibrary extraChargesLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE extrachargeslibrary
SET IsDeleted = 1,
    DeletedBy = @deletedBy,
    DeletedAt = @deletedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", extraChargesLibrary.Id);
                cmd.Parameters.AddWithValue("@deletedBy", (object?)extraChargesLibrary.DeletedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deletedAt", (object?)extraChargesLibrary.DeletedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = extraChargesLibrary.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ExtraChargesLibrary>>> GetExtraCharges(ExtraChargesLibrary extraChargesLibrary, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (extraChargesLibrary.Id > 0)
                {
                    where.Add("e.Id = @id");
                    cmd.Parameters.AddWithValue("@id", extraChargesLibrary.Id);
                }
                if (extraChargesLibrary.CatID.HasValue)
                {
                    where.Add("e.CatID = @catId");
                    cmd.Parameters.AddWithValue("@catId", extraChargesLibrary.CatID.Value);
                }
                if (!string.IsNullOrWhiteSpace(extraChargesLibrary.Name))
                {
                    where.Add("e.Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{extraChargesLibrary.Name}%");
                }
                if (extraChargesLibrary.IsCompulsary.HasValue)
                {
                    where.Add("e.IsCompulsary = @isCompulsary");
                    cmd.Parameters.AddWithValue("@isCompulsary", extraChargesLibrary.IsCompulsary.Value);
                }

                where.Add("e.IsDeleted = @isDeleted");
                cmd.Parameters.AddWithValue("@isDeleted", extraChargesLibrary.IsDeleted);

                if (!string.IsNullOrWhiteSpace(extraChargesLibrary.searchText))
                {
                    where.Add("(e.Name LIKE @search OR cl.Name LIKE @search OR cpb.Name LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{extraChargesLibrary.searchText}%");
                }
                if (!string.IsNullOrWhiteSpace(extraChargesLibrary.CategoryName))
                {
                    where.Add("cl.Name LIKE @categoryName");
                    cmd.Parameters.AddWithValue("@categoryName", $"%{extraChargesLibrary.CategoryName}%");
                }
                if (!string.IsNullOrWhiteSpace(extraChargesLibrary.ChargesPaidByName))
                {
                    where.Add("cpb.Name LIKE @chargesPaidByName");
                    cmd.Parameters.AddWithValue("@chargesPaidByName", $"%{extraChargesLibrary.ChargesPaidByName}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT COUNT(1)
FROM extrachargeslibrary e
LEFT JOIN categorylibrary cl ON cl.Id = e.CatID
LEFT JOIN chargespaidbylibrary cpb ON cpb.Id = e.ChargesPaidById
{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);
                var offset = (safePageIndex - 1) * safePageSize;

                var items = new List<ExtraChargesLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT
  e.Id, e.CatID, e.ChargesPaidById, e.Name, e.ChargesOn, e.IsCompulsary, e.ChargesIn, e.PercentageValue, e.AmountValue, e.MaxAmountValue,
  e.CreatedBy, e.CreatedAt, e.ModifiedBy, e.ModifiedAt, e.DeletedBy, e.DeletedAt, e.IsDeleted,
  cl.Name AS CategoryName,
  cpb.Name AS ChargesPaidByName
FROM extrachargeslibrary e
LEFT JOIN categorylibrary cl ON cl.Id = e.CatID
LEFT JOIN chargespaidbylibrary cpb ON cpb.Id = e.ChargesPaidById
{whereClause}
ORDER BY e.Id DESC
LIMIT @offset, @pageSize;";
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        items.Add(new ExtraChargesLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            CatID = reader.IsDBNull(reader.GetOrdinal("CatID")) ? null : reader.GetInt32(reader.GetOrdinal("CatID")),
                            ChargesPaidByID = reader.GetInt32(reader.GetOrdinal("ChargesPaidById")),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                            ChargesOn = reader.IsDBNull(reader.GetOrdinal("ChargesOn")) ? string.Empty : reader.GetString(reader.GetOrdinal("ChargesOn")),
                            IsCompulsary = reader.IsDBNull(reader.GetOrdinal("IsCompulsary")) ? null : reader.GetBoolean(reader.GetOrdinal("IsCompulsary")),
                            ChargesIn = reader.IsDBNull(reader.GetOrdinal("ChargesIn")) ? string.Empty : reader.GetString(reader.GetOrdinal("ChargesIn")),
                            PercentageValue = reader.IsDBNull(reader.GetOrdinal("PercentageValue")) ? 0 : reader.GetDecimal(reader.GetOrdinal("PercentageValue")),
                            AmountValue = reader.IsDBNull(reader.GetOrdinal("AmountValue")) ? 0 : reader.GetDecimal(reader.GetOrdinal("AmountValue")),
                            MaxAmountValue = reader.IsDBNull(reader.GetOrdinal("MaxAmountValue")) ? null : reader.GetDecimal(reader.GetOrdinal("MaxAmountValue")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            DeletedBy = reader.IsDBNull(reader.GetOrdinal("DeletedBy")) ? null : reader.GetString(reader.GetOrdinal("DeletedBy")),
                            DeletedAt = reader.IsDBNull(reader.GetOrdinal("DeletedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("DeletedAt")),
                            IsDeleted = !reader.IsDBNull(reader.GetOrdinal("IsDeleted")) && reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                            CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? null : reader.GetString(reader.GetOrdinal("CategoryName")),
                            ChargesPaidByName = reader.IsDBNull(reader.GetOrdinal("ChargesPaidByName")) ? null : reader.GetString(reader.GetOrdinal("ChargesPaidByName"))
                        });
                    }
                }

                return new BaseResponse<List<ExtraChargesLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ExtraChargesLibrary>> { code = 400, message = ex.Message, data = new List<ExtraChargesLibrary>() };
            }
        }

        public async Task<BaseResponse<List<ExtraChargesLibrary>>> GetCatExtraCharges(int CategoryId)
        {
            var probe = new ExtraChargesLibrary { CatID = CategoryId, IsDeleted = false };
            return await GetExtraCharges(probe, 1, 200, "get");
        }
    }
}
