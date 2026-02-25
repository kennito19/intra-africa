using Catalogue.Application.IRepositories;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class CommissionChargesRepository : ICommissionChargesRepository
    {
        private readonly string _connectionString;

        public CommissionChargesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(CommissionChargesLibrary commissionCharges)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO commissionchargeslibrary
(CatID, SellerID, BrandID, ChargesOn, ChargesIn, AmountValue, IsCompulsary, CreatedBy, CreatedAt, IsDeleted)
VALUES
(@catId, @sellerId, @brandId, @chargesOn, @chargesIn, @amountValue, @isCompulsary, @createdBy, @createdAt, 0);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@catId", (object?)commissionCharges.CatID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sellerId", (object?)commissionCharges.SellerID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@brandId", (object?)commissionCharges.BrandID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@chargesOn", (object?)commissionCharges.ChargesOn ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@chargesIn", (object?)commissionCharges.ChargesIn ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@amountValue", (object?)(commissionCharges.AmountValue?.ToString(CultureInfo.InvariantCulture)) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isCompulsary", commissionCharges.IsCompulsary ?? false);
                cmd.Parameters.AddWithValue("@createdBy", (object?)commissionCharges.CreatedBy ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdAt", commissionCharges.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(CommissionChargesLibrary commissionCharges)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE commissionchargeslibrary
SET CatID = @catId,
    SellerID = @sellerId,
    BrandID = @brandId,
    ChargesOn = @chargesOn,
    ChargesIn = @chargesIn,
    AmountValue = @amountValue,
    IsCompulsary = @isCompulsary,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE ID = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", commissionCharges.ID);
                cmd.Parameters.AddWithValue("@catId", (object?)commissionCharges.CatID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sellerId", (object?)commissionCharges.SellerID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@brandId", (object?)commissionCharges.BrandID ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@chargesOn", (object?)commissionCharges.ChargesOn ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@chargesIn", (object?)commissionCharges.ChargesIn ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@amountValue", (object?)(commissionCharges.AmountValue?.ToString(CultureInfo.InvariantCulture)) ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isCompulsary", commissionCharges.IsCompulsary ?? false);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)commissionCharges.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", (object?)commissionCharges.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = commissionCharges.ID
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(CommissionChargesLibrary commissionCharges)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE commissionchargeslibrary
SET IsDeleted = 1,
    DeletedBy = @deletedBy,
    DeletedAt = @deletedAt
WHERE ID = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", commissionCharges.ID);
                cmd.Parameters.AddWithValue("@deletedBy", (object?)commissionCharges.DeletedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deletedAt", (object?)commissionCharges.DeletedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = commissionCharges.ID
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<CommissionChargesLibrary>>> get(CommissionChargesLibrary commissionCharges, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (commissionCharges.ID > 0)
                {
                    where.Add("c.ID = @id");
                    cmd.Parameters.AddWithValue("@id", commissionCharges.ID);
                }
                if (commissionCharges.CatID.HasValue)
                {
                    where.Add("c.CatID = @catId");
                    cmd.Parameters.AddWithValue("@catId", commissionCharges.CatID.Value);
                }
                if (!string.IsNullOrWhiteSpace(commissionCharges.SellerID))
                {
                    where.Add("c.SellerID = @sellerId");
                    cmd.Parameters.AddWithValue("@sellerId", commissionCharges.SellerID);
                }
                if (commissionCharges.BrandID.HasValue)
                {
                    where.Add("c.BrandID = @brandId");
                    cmd.Parameters.AddWithValue("@brandId", commissionCharges.BrandID.Value);
                }
                if (!string.IsNullOrWhiteSpace(commissionCharges.ChargesOn))
                {
                    where.Add("c.ChargesOn = @chargesOn");
                    cmd.Parameters.AddWithValue("@chargesOn", commissionCharges.ChargesOn);
                }
                if (commissionCharges.IsCompulsary.HasValue)
                {
                    where.Add("c.IsCompulsary = @isCompulsary");
                    cmd.Parameters.AddWithValue("@isCompulsary", commissionCharges.IsCompulsary.Value);
                }

                where.Add("c.IsDeleted = @isDeleted");
                cmd.Parameters.AddWithValue("@isDeleted", commissionCharges.IsDeleted);

                if (!string.IsNullOrWhiteSpace(commissionCharges.Searchtext))
                {
                    where.Add("(c.ChargesOn LIKE @search OR c.SellerID LIKE @search OR b.Name LIKE @search OR cl.Name LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{commissionCharges.Searchtext}%");
                }

                if (commissionCharges.OnlyCategories == true)
                {
                    where.Add("c.CatID IS NOT NULL AND c.SellerID IS NULL AND c.BrandID IS NULL");
                }
                if (commissionCharges.OnlySellers == true)
                {
                    where.Add("c.SellerID IS NOT NULL");
                }
                if (commissionCharges.OnlyBrands == true)
                {
                    where.Add("c.BrandID IS NOT NULL");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT COUNT(1)
FROM commissionchargeslibrary c
LEFT JOIN categorylibrary cl ON cl.Id = c.CatID
LEFT JOIN iskrwaod_user.brand b ON b.ID = c.BrandID
{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);
                var offset = (safePageIndex - 1) * safePageSize;

                var items = new List<CommissionChargesLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT
  c.ID, c.CatID, c.SellerID, c.BrandID, c.ChargesOn, c.ChargesIn, c.AmountValue, c.IsCompulsary,
  c.CreatedBy, c.CreatedAt, c.ModifiedBy, c.ModifiedAt, c.DeletedBy, c.DeletedAt, c.IsDeleted,
  cl.Name AS CategoryName,
  b.Name AS BrandName
FROM commissionchargeslibrary c
LEFT JOIN categorylibrary cl ON cl.Id = c.CatID
LEFT JOIN iskrwaod_user.brand b ON b.ID = c.BrandID
{whereClause}
ORDER BY c.ID DESC
LIMIT @offset, @pageSize;";

                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        var amountText = reader.IsDBNull(reader.GetOrdinal("AmountValue")) ? null : reader.GetString(reader.GetOrdinal("AmountValue"));
                        decimal? amount = null;
                        if (!string.IsNullOrWhiteSpace(amountText) && decimal.TryParse(amountText, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsed))
                        {
                            amount = parsed;
                        }

                        items.Add(new CommissionChargesLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            ID = reader.GetInt32(reader.GetOrdinal("ID")),
                            CatID = reader.IsDBNull(reader.GetOrdinal("CatID")) ? null : reader.GetInt32(reader.GetOrdinal("CatID")),
                            SellerID = reader.IsDBNull(reader.GetOrdinal("SellerID")) ? null : reader.GetString(reader.GetOrdinal("SellerID")),
                            BrandID = reader.IsDBNull(reader.GetOrdinal("BrandID")) ? null : reader.GetInt32(reader.GetOrdinal("BrandID")),
                            ChargesOn = reader.IsDBNull(reader.GetOrdinal("ChargesOn")) ? null : reader.GetString(reader.GetOrdinal("ChargesOn")),
                            ChargesIn = reader.IsDBNull(reader.GetOrdinal("ChargesIn")) ? null : reader.GetString(reader.GetOrdinal("ChargesIn")),
                            AmountValue = amount,
                            IsCompulsary = reader.IsDBNull(reader.GetOrdinal("IsCompulsary")) ? null : reader.GetBoolean(reader.GetOrdinal("IsCompulsary")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            DeletedBy = reader.IsDBNull(reader.GetOrdinal("DeletedBy")) ? null : reader.GetString(reader.GetOrdinal("DeletedBy")),
                            DeletedAt = reader.IsDBNull(reader.GetOrdinal("DeletedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("DeletedAt")),
                            IsDeleted = !reader.IsDBNull(reader.GetOrdinal("IsDeleted")) && reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                            CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? null : reader.GetString(reader.GetOrdinal("CategoryName")),
                            BrandName = reader.IsDBNull(reader.GetOrdinal("BrandName")) ? null : reader.GetString(reader.GetOrdinal("BrandName")),
                            SellerName = reader.IsDBNull(reader.GetOrdinal("SellerID")) ? null : reader.GetString(reader.GetOrdinal("SellerID"))
                        });
                    }
                }

                return new BaseResponse<List<CommissionChargesLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<CommissionChargesLibrary>> { code = 400, message = ex.Message, data = new List<CommissionChargesLibrary>() };
            }
        }

        public async Task<BaseResponse<List<CommissionChargesLibrary>>> getCategoryWiseCommission(CommissionChargesLibrary commissionCharges)
        {
            // Reuse the same list query but return first matching records per existing method contract.
            return await get(commissionCharges, 1, 50, "get");
        }
    }
}
