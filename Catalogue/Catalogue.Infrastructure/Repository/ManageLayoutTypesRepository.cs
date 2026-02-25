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
    public class ManageLayoutTypesRepository : IManageLayoutTypesRepository
    {
        private readonly string _connectionString;

        public ManageLayoutTypesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ManageLayoutTypesLibrary layoutTypes)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO managelayouttypes
(LayoutId, Name, ImageUrl, Options, ClassName, HasInnerColumns, Columns, MinImage, MaxImage, CreatedBy, CreatedAt)
VALUES
(@layoutId, @name, @imageUrl, @options, @className, @hasInnerColumns, @columns, @minImage, @maxImage, @createdBy, @createdAt);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@layoutId", layoutTypes.LayoutId > 0 ? layoutTypes.LayoutId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@name", (object?)layoutTypes.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@imageUrl", (object?)layoutTypes.ImageUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@options", (object?)layoutTypes.Options ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@className", (object?)layoutTypes.ClassName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@hasInnerColumns", (object?)layoutTypes.HasInnerColumns ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@columns", (object?)layoutTypes.Columns ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@minImage", (object?)layoutTypes.MinImage ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@maxImage", (object?)layoutTypes.MaxImage ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdBy", (object?)layoutTypes.CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdAt", layoutTypes.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(ManageLayoutTypesLibrary layoutTypes)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE managelayouttypes
SET LayoutId = @layoutId,
    Name = @name,
    ImageUrl = @imageUrl,
    Options = @options,
    ClassName = @className,
    HasInnerColumns = @hasInnerColumns,
    Columns = @columns,
    MinImage = @minImage,
    MaxImage = @maxImage,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", layoutTypes.Id);
                cmd.Parameters.AddWithValue("@layoutId", layoutTypes.LayoutId > 0 ? layoutTypes.LayoutId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@name", (object?)layoutTypes.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@imageUrl", (object?)layoutTypes.ImageUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@options", (object?)layoutTypes.Options ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@className", (object?)layoutTypes.ClassName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@hasInnerColumns", (object?)layoutTypes.HasInnerColumns ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@columns", (object?)layoutTypes.Columns ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@minImage", (object?)layoutTypes.MinImage ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@maxImage", (object?)layoutTypes.MaxImage ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)layoutTypes.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", layoutTypes.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = layoutTypes.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageLayoutTypesLibrary layoutTypes)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = "DELETE FROM managelayouttypes WHERE Id = @id;";
                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", layoutTypes.Id);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = layoutTypes.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ManageLayoutTypesLibrary>>> get(ManageLayoutTypesLibrary layoutTypes, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (layoutTypes.Id > 0)
                {
                    where.Add("t.Id = @id");
                    cmd.Parameters.AddWithValue("@id", layoutTypes.Id);
                }
                if (layoutTypes.LayoutId > 0)
                {
                    where.Add("t.LayoutId = @layoutId");
                    cmd.Parameters.AddWithValue("@layoutId", layoutTypes.LayoutId);
                }
                if (!string.IsNullOrWhiteSpace(layoutTypes.Name))
                {
                    where.Add("t.Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{layoutTypes.Name}%");
                }
                if (!string.IsNullOrWhiteSpace(layoutTypes.Searchtext))
                {
                    where.Add("(t.Name LIKE @search OR l.Name LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{layoutTypes.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT COUNT(1)
FROM managelayouttypes t
LEFT JOIN managelayouts l ON l.Id = t.LayoutId
{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var offset = (safePageIndex - 1) * safePageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);

                var items = new List<ManageLayoutTypesLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT
  t.Id, t.LayoutId, t.Name, t.ImageUrl, t.Options, t.ClassName, t.HasInnerColumns, t.Columns, t.MinImage, t.MaxImage,
  t.CreatedBy, t.CreatedAt, t.ModifiedBy, t.ModifiedAt,
  l.Name AS LayoutName
FROM managelayouttypes t
LEFT JOIN managelayouts l ON l.Id = t.LayoutId
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
                        items.Add(new ManageLayoutTypesLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            LayoutId = reader.GetInt32(reader.GetOrdinal("LayoutId")),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                            Options = reader.IsDBNull(reader.GetOrdinal("Options")) ? null : reader.GetString(reader.GetOrdinal("Options")),
                            ClassName = reader.IsDBNull(reader.GetOrdinal("ClassName")) ? null : reader.GetString(reader.GetOrdinal("ClassName")),
                            HasInnerColumns = reader.IsDBNull(reader.GetOrdinal("HasInnerColumns")) ? null : reader.GetBoolean(reader.GetOrdinal("HasInnerColumns")),
                            Columns = reader.IsDBNull(reader.GetOrdinal("Columns")) ? null : reader.GetInt32(reader.GetOrdinal("Columns")),
                            MinImage = reader.IsDBNull(reader.GetOrdinal("MinImage")) ? null : reader.GetString(reader.GetOrdinal("MinImage")),
                            MaxImage = reader.IsDBNull(reader.GetOrdinal("MaxImage")) ? null : reader.GetString(reader.GetOrdinal("MaxImage")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            LayoutName = reader.IsDBNull(reader.GetOrdinal("LayoutName")) ? null : reader.GetString(reader.GetOrdinal("LayoutName"))
                        });
                    }
                }

                return new BaseResponse<List<ManageLayoutTypesLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageLayoutTypesLibrary>> { code = 400, message = ex.Message, data = new List<ManageLayoutTypesLibrary>() };
            }
        }
    }
}
