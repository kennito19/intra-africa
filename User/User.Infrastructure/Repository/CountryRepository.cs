using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using MySqlConnector;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Domain.Entity;
using User.Domain;
using User.Infrastructure.Helper;

namespace User.Infrastructure.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly string _connectionString;

        public CountryRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(CountryLibrary countryLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO Country (Name, Status, CreatedBy, CreatedAt, IsDeleted)
VALUES (@name, @status, @createdBy, @createdAt, 0);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", countryLibrary.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@status", countryLibrary.Status ?? "Active");
                cmd.Parameters.AddWithValue("@createdBy", countryLibrary.CreatedBy ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdAt", countryLibrary.CreatedAt == default ? DateTime.Now : countryLibrary.CreatedAt);

                var idObj = await cmd.ExecuteScalarAsync();
                var newId = Convert.ToInt64(idObj ?? 0);

                return new BaseResponse<long>
                {
                    code = 200,
                    message = "Record added successfully.",
                    data = newId
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long>
                {
                    code = 400,
                    message = ex.Message,
                    data = 0
                };
            }
        }

        public async Task<BaseResponse<long>> Update(CountryLibrary countryLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE Country
SET Name = @name,
    Status = @status,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", countryLibrary.Id);
                cmd.Parameters.AddWithValue("@name", countryLibrary.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@status", countryLibrary.Status ?? "Active");
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)countryLibrary.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", (object?)countryLibrary.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                if (affected == 0)
                {
                    return new BaseResponse<long> { code = 204, message = "Record does not Exist.", data = 0 };
                }

                return new BaseResponse<long> { code = 200, message = "Record updated successfully.", data = countryLibrary.Id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long>
                {
                    code = 400,
                    message = ex.Message,
                    data = 0
                };
            }
        }

        public async Task<BaseResponse<long>> Delete(CountryLibrary countryLibrary)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE Country
SET IsDeleted = 1,
    DeletedBy = @deletedBy,
    DeletedAt = @deletedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", countryLibrary.Id);
                cmd.Parameters.AddWithValue("@deletedBy", (object?)countryLibrary.DeletedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deletedAt", (object?)countryLibrary.DeletedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                if (affected == 0)
                {
                    return new BaseResponse<long> { code = 204, message = "Record does not Exist.", data = 0 };
                }

                return new BaseResponse<long> { code = 200, message = "Record deleted successfully.", data = countryLibrary.Id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long>
                {
                    code = 400,
                    message = ex.Message,
                    data = 0
                };
            }
        }

        public async Task<BaseResponse<List<CountryLibrary>>> Get(CountryLibrary countryLibrary, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand();
                cmd.Connection = con;

                var where = new List<string>();
                if (countryLibrary.Id > 0)
                {
                    where.Add("Id = @id");
                    cmd.Parameters.AddWithValue("@id", countryLibrary.Id);
                }
                if (!string.IsNullOrWhiteSpace(countryLibrary.Name))
                {
                    where.Add("Name = @name");
                    cmd.Parameters.AddWithValue("@name", countryLibrary.Name);
                }
                if (!string.IsNullOrWhiteSpace(countryLibrary.Status))
                {
                    where.Add("Status = @status");
                    cmd.Parameters.AddWithValue("@status", countryLibrary.Status);
                }
                if (!string.IsNullOrWhiteSpace(countryLibrary.Searchtext))
                {
                    where.Add("Name LIKE @search");
                    cmd.Parameters.AddWithValue("@search", $"%{countryLibrary.Searchtext}%");
                }

                where.Add("IsDeleted = @isDeleted");
                cmd.Parameters.AddWithValue("@isDeleted", countryLibrary.IsDeleted);

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;
                cmd.CommandText = $"SELECT COUNT(1) FROM Country{whereClause};";
                var totalObj = await cmd.ExecuteScalarAsync();
                var total = Convert.ToInt32(totalObj ?? 0);

                var items = new List<CountryLibrary>();
                if (total > 0)
                {
                    var fetchAll = PageIndex <= 0 || PageSize <= 0;
                    var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                    var safePageSize = PageSize <= 0 ? total : PageSize;
                    var offset = (safePageIndex - 1) * safePageSize;

                    cmd.CommandText = $@"
SELECT Id, Name, Status, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt, DeletedBy, DeletedAt, IsDeleted
FROM Country
{whereClause}
ORDER BY Id DESC
{(fetchAll ? string.Empty : "LIMIT @offset, @pageSize")};";

                    if (!fetchAll)
                    {
                        cmd.Parameters.AddWithValue("@offset", offset);
                        cmd.Parameters.AddWithValue("@pageSize", safePageSize);
                    }

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNo = 0;
                    var pageCount = safePageSize == 0 ? 1 : (int)Math.Ceiling(total / (double)safePageSize);
                    while (await reader.ReadAsync())
                    {
                        rowNo++;
                        items.Add(new CountryLibrary
                        {
                            RowNumber = rowNo,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32("Id"),
                            Name = reader.IsDBNull("Name") ? string.Empty : reader.GetString("Name"),
                            Status = reader.IsDBNull("Status") ? string.Empty : reader.GetString("Status"),
                            CreatedBy = reader.IsDBNull("CreatedBy") ? string.Empty : reader.GetString("CreatedBy"),
                            CreatedAt = reader.IsDBNull("CreatedAt") ? DateTime.MinValue : reader.GetDateTime("CreatedAt"),
                            ModifiedBy = reader.IsDBNull("ModifiedBy") ? null : reader.GetString("ModifiedBy"),
                            ModifiedAt = reader.IsDBNull("ModifiedAt") ? null : reader.GetDateTime("ModifiedAt"),
                            DeletedBy = reader.IsDBNull("DeletedBy") ? null : reader.GetString("DeletedBy"),
                            DeletedAt = reader.IsDBNull("DeletedAt") ? null : reader.GetDateTime("DeletedAt"),
                            IsDeleted = !reader.IsDBNull("IsDeleted") && reader.GetBoolean("IsDeleted")
                        });
                    }
                }

                return new BaseResponse<List<CountryLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<CountryLibrary>>
                {
                    code = 400,
                    message = ex.Message,
                    data = new List<CountryLibrary>()
                };
            }
        }
    }
}
