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
    public class ManageLayoutOptionRepository : IManageLayoutOptionRepository
    {
        private readonly string _connectionString;

        public ManageLayoutOptionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ManageLayoutOption layoutOptions)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO layoutoptions (Name, Image, CreatedBy, CreatedAt)
VALUES (@name, @image, @createdBy, @createdAt);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", (object?)layoutOptions.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@image", (object?)layoutOptions.Image ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdBy", (object?)layoutOptions.CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdAt", layoutOptions.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(ManageLayoutOption layoutOptions)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE layoutoptions
SET Name = @name,
    Image = @image,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", layoutOptions.Id);
                cmd.Parameters.AddWithValue("@name", (object?)layoutOptions.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@image", (object?)layoutOptions.Image ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)layoutOptions.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", layoutOptions.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = layoutOptions.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageLayoutOption layoutOptions)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = "DELETE FROM layoutoptions WHERE Id = @id;";
                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", layoutOptions.Id);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = layoutOptions.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ManageLayoutOption>>> get(ManageLayoutOption layoutOptions, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (layoutOptions.Id > 0)
                {
                    where.Add("Id = @id");
                    cmd.Parameters.AddWithValue("@id", layoutOptions.Id);
                }
                if (!string.IsNullOrWhiteSpace(layoutOptions.Name))
                {
                    where.Add("Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{layoutOptions.Name}%");
                }
                if (!string.IsNullOrWhiteSpace(layoutOptions.Searchtext))
                {
                    where.Add("Name LIKE @search");
                    cmd.Parameters.AddWithValue("@search", $"%{layoutOptions.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $"SELECT COUNT(1) FROM layoutoptions{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var offset = (safePageIndex - 1) * safePageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);

                var items = new List<ManageLayoutOption>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT Id, Name, Image, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt
FROM layoutoptions
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
                        items.Add(new ManageLayoutOption
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                            Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt"))
                        });
                    }
                }

                return new BaseResponse<List<ManageLayoutOption>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageLayoutOption>> { code = 400, message = ex.Message, data = new List<ManageLayoutOption>() };
            }
        }
    }
}
