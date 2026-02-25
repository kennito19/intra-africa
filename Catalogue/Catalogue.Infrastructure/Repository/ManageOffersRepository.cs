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
    public class ManageOffersRepository : IManageOffersRepository
    {
        private readonly string _connectionString;

        public ManageOffersRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ManageOffersLibrary model)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO ManageOffers
(
    Name, Code, Terms, OfferCreateBy, OfferType, UsesType, UsesPerCustomer, Value,
    MinimumOrderValue, MaximumDiscountAmount, BuyQty, GetQty, ApplyOn, HasShippingFree,
    ShowToCustomer, OnlyForOnlinePayments, OnlyForNewCustomers, StartDate, EndDate, Status,
    CreatedBy, Createdat, IsDeleted
)
VALUES
(
    @name, @code, @terms, @offerCreateBy, @offerType, @usesType, @usesPerCustomer, @value,
    @minimumOrderValue, @maximumDiscountAmount, @buyQty, @getQty, @applyOn, @hasShippingFree,
    @showToCustomer, @onlyForOnlinePayments, @onlyForNewCustomers, @startDate, @endDate, @status,
    @createdBy, @createdAt, @isDeleted
);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", (object?)model.name ?? string.Empty);
                cmd.Parameters.AddWithValue("@code", (object?)model.code ?? string.Empty);
                cmd.Parameters.AddWithValue("@terms", (object?)model.terms ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@offerCreateBy", (object?)model.offerCreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@offerType", (object?)model.offerType ?? string.Empty);
                cmd.Parameters.AddWithValue("@usesType", (object?)model.usesType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@usesPerCustomer", (object?)model.usesPerCustomer ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@value", (object?)model.value ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@minimumOrderValue", (object?)model.minimumOrderValue ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@maximumDiscountAmount", (object?)model.maximumDiscountAmount ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@buyQty", (object?)model.buyQty ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@getQty", (object?)model.getQty ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@applyOn", (object?)model.applyOn ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@hasShippingFree", (object?)model.hasShippingFree ?? false);
                cmd.Parameters.AddWithValue("@showToCustomer", (object?)model.showToCustomer ?? false);
                cmd.Parameters.AddWithValue("@onlyForOnlinePayments", (object?)model.onlyForOnlinePayments ?? false);
                cmd.Parameters.AddWithValue("@onlyForNewCustomers", (object?)model.onlyForNewCustomers ?? false);
                cmd.Parameters.AddWithValue("@startDate", (object?)model.startDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@endDate", (object?)model.endDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", (object?)model.status ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdBy", (object?)model.CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdAt", model.CreatedAt ?? DateTime.Now);
                cmd.Parameters.AddWithValue("@isDeleted", model.IsDeleted);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(ManageOffersLibrary model)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE ManageOffers
SET Name = @name,
    Code = @code,
    Terms = @terms,
    OfferCreateBy = @offerCreateBy,
    OfferType = @offerType,
    UsesType = @usesType,
    UsesPerCustomer = @usesPerCustomer,
    Value = @value,
    MinimumOrderValue = @minimumOrderValue,
    MaximumDiscountAmount = @maximumDiscountAmount,
    BuyQty = @buyQty,
    GetQty = @getQty,
    ApplyOn = @applyOn,
    HasShippingFree = @hasShippingFree,
    ShowToCustomer = @showToCustomer,
    OnlyForOnlinePayments = @onlyForOnlinePayments,
    OnlyForNewCustomers = @onlyForNewCustomers,
    StartDate = @startDate,
    EndDate = @endDate,
    Status = @status,
    ModifiedBy = @modifiedBy,
    Modifiedat = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", model.id);
                cmd.Parameters.AddWithValue("@name", (object?)model.name ?? string.Empty);
                cmd.Parameters.AddWithValue("@code", (object?)model.code ?? string.Empty);
                cmd.Parameters.AddWithValue("@terms", (object?)model.terms ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@offerCreateBy", (object?)model.offerCreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@offerType", (object?)model.offerType ?? string.Empty);
                cmd.Parameters.AddWithValue("@usesType", (object?)model.usesType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@usesPerCustomer", (object?)model.usesPerCustomer ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@value", (object?)model.value ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@minimumOrderValue", (object?)model.minimumOrderValue ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@maximumDiscountAmount", (object?)model.maximumDiscountAmount ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@buyQty", (object?)model.buyQty ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@getQty", (object?)model.getQty ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@applyOn", (object?)model.applyOn ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@hasShippingFree", (object?)model.hasShippingFree ?? false);
                cmd.Parameters.AddWithValue("@showToCustomer", (object?)model.showToCustomer ?? false);
                cmd.Parameters.AddWithValue("@onlyForOnlinePayments", (object?)model.onlyForOnlinePayments ?? false);
                cmd.Parameters.AddWithValue("@onlyForNewCustomers", (object?)model.onlyForNewCustomers ?? false);
                cmd.Parameters.AddWithValue("@startDate", (object?)model.startDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@endDate", (object?)model.endDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", (object?)model.status ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)model.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", model.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = model.id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageOffersLibrary model)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE ManageOffers
SET IsDeleted = 1,
    DeletedBy = @deletedBy,
    Deletedat = @deletedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", model.id);
                cmd.Parameters.AddWithValue("@deletedBy", (object?)model.DeletedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deletedAt", model.DeletedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = model.id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ManageOffersLibrary>>> get(ManageOffersLibrary model, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (model.id > 0)
                {
                    where.Add("Id = @id");
                    cmd.Parameters.AddWithValue("@id", model.id);
                }
                if (!string.IsNullOrWhiteSpace(model.name))
                {
                    where.Add("Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{model.name}%");
                }
                if (!string.IsNullOrWhiteSpace(model.offerType))
                {
                    where.Add("OfferType = @offerType");
                    cmd.Parameters.AddWithValue("@offerType", model.offerType);
                }
                if (!string.IsNullOrWhiteSpace(model.code))
                {
                    where.Add("Code LIKE @code");
                    cmd.Parameters.AddWithValue("@code", $"%{model.code}%");
                }
                if (!string.IsNullOrWhiteSpace(model.status))
                {
                    where.Add("Status = @status");
                    cmd.Parameters.AddWithValue("@status", model.status);
                }
                where.Add("IFNULL(IsDeleted, 0) = @isDeleted");
                cmd.Parameters.AddWithValue("@isDeleted", model.IsDeleted);

                if (model.hasShippingFree.HasValue)
                {
                    where.Add("IFNULL(HasShippingFree, 0) = @hasShippingFree");
                    cmd.Parameters.AddWithValue("@hasShippingFree", model.hasShippingFree.Value);
                }
                if (model.showToCustomer.HasValue)
                {
                    where.Add("IFNULL(ShowToCustomer, 0) = @showToCustomer");
                    cmd.Parameters.AddWithValue("@showToCustomer", model.showToCustomer.Value);
                }
                if (!string.IsNullOrWhiteSpace(model.offerCreatedBy))
                {
                    where.Add("OfferCreateBy = @offerCreatedBy");
                    cmd.Parameters.AddWithValue("@offerCreatedBy", model.offerCreatedBy);
                }
                if (!string.IsNullOrWhiteSpace(model.CreatedBy))
                {
                    where.Add("CreatedBy = @createdBy");
                    cmd.Parameters.AddWithValue("@createdBy", model.CreatedBy);
                }
                if (!string.IsNullOrWhiteSpace(model.offerIds))
                {
                    where.Add("FIND_IN_SET(CAST(Id AS CHAR), @offerIds)");
                    cmd.Parameters.AddWithValue("@offerIds", model.offerIds);
                }
                if (!string.IsNullOrWhiteSpace(model.Searchtext))
                {
                    where.Add("(Name LIKE @search OR Code LIKE @search OR OfferType LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{model.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;
                cmd.CommandText = $"SELECT COUNT(1) FROM ManageOffers{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var offset = (safePageIndex - 1) * safePageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);

                var items = new List<ManageOffersLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT Id, Name, Code, Terms, OfferCreateBy, OfferType, UsesType, UsesPerCustomer, Value,
       MinimumOrderValue, MaximumDiscountAmount, BuyQty, GetQty, ApplyOn, HasShippingFree,
       ShowToCustomer, OnlyForOnlinePayments, OnlyForNewCustomers, StartDate, EndDate, Status,
       CreatedBy, Createdat, ModifiedBy, Modifiedat, DeletedBy, Deletedat, IsDeleted
FROM ManageOffers
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
                        var status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status"));
                        items.Add(new ManageOffersLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            id = reader.GetInt32(reader.GetOrdinal("Id")),
                            name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                            code = reader.IsDBNull(reader.GetOrdinal("Code")) ? null : reader.GetString(reader.GetOrdinal("Code")),
                            terms = reader.IsDBNull(reader.GetOrdinal("Terms")) ? null : reader.GetString(reader.GetOrdinal("Terms")),
                            offerCreatedBy = reader.IsDBNull(reader.GetOrdinal("OfferCreateBy")) ? null : reader.GetString(reader.GetOrdinal("OfferCreateBy")),
                            offerType = reader.IsDBNull(reader.GetOrdinal("OfferType")) ? null : reader.GetString(reader.GetOrdinal("OfferType")),
                            usesType = reader.IsDBNull(reader.GetOrdinal("UsesType")) ? null : reader.GetString(reader.GetOrdinal("UsesType")),
                            usesPerCustomer = reader.IsDBNull(reader.GetOrdinal("UsesPerCustomer")) ? null : reader.GetString(reader.GetOrdinal("UsesPerCustomer")),
                            value = reader.IsDBNull(reader.GetOrdinal("Value")) ? null : reader.GetDecimal(reader.GetOrdinal("Value")),
                            minimumOrderValue = reader.IsDBNull(reader.GetOrdinal("MinimumOrderValue")) ? null : reader.GetDecimal(reader.GetOrdinal("MinimumOrderValue")),
                            maximumDiscountAmount = reader.IsDBNull(reader.GetOrdinal("MaximumDiscountAmount")) ? null : reader.GetDecimal(reader.GetOrdinal("MaximumDiscountAmount")),
                            buyQty = reader.IsDBNull(reader.GetOrdinal("BuyQty")) ? null : reader.GetInt32(reader.GetOrdinal("BuyQty")),
                            getQty = reader.IsDBNull(reader.GetOrdinal("GetQty")) ? null : reader.GetInt32(reader.GetOrdinal("GetQty")),
                            applyOn = reader.IsDBNull(reader.GetOrdinal("ApplyOn")) ? null : reader.GetString(reader.GetOrdinal("ApplyOn")),
                            hasShippingFree = reader.IsDBNull(reader.GetOrdinal("HasShippingFree")) ? null : reader.GetBoolean(reader.GetOrdinal("HasShippingFree")),
                            showToCustomer = reader.IsDBNull(reader.GetOrdinal("ShowToCustomer")) ? null : reader.GetBoolean(reader.GetOrdinal("ShowToCustomer")),
                            onlyForOnlinePayments = reader.IsDBNull(reader.GetOrdinal("OnlyForOnlinePayments")) ? null : reader.GetBoolean(reader.GetOrdinal("OnlyForOnlinePayments")),
                            onlyForNewCustomers = reader.IsDBNull(reader.GetOrdinal("OnlyForNewCustomers")) ? null : reader.GetBoolean(reader.GetOrdinal("OnlyForNewCustomers")),
                            startDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? null : reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            endDate = reader.IsDBNull(reader.GetOrdinal("EndDate")) ? null : reader.GetDateTime(reader.GetOrdinal("EndDate")),
                            status = status,
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("Createdat")) ? null : reader.GetDateTime(reader.GetOrdinal("Createdat")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("Modifiedat")) ? null : reader.GetDateTime(reader.GetOrdinal("Modifiedat")),
                            DeletedBy = reader.IsDBNull(reader.GetOrdinal("DeletedBy")) ? null : reader.GetString(reader.GetOrdinal("DeletedBy")),
                            DeletedAt = reader.IsDBNull(reader.GetOrdinal("Deletedat")) ? null : reader.GetDateTime(reader.GetOrdinal("Deletedat")),
                            IsDeleted = !reader.IsDBNull(reader.GetOrdinal("IsDeleted")) && reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                            OfferStatus = !string.IsNullOrWhiteSpace(status) && status.Equals("Active", StringComparison.OrdinalIgnoreCase)
                        });
                    }
                }

                return new BaseResponse<List<ManageOffersLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageOffersLibrary>> { code = 400, message = ex.Message, data = new List<ManageOffersLibrary>() };
            }
        }
    }
}
