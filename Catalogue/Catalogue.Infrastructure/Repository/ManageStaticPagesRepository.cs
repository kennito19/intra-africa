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
    public class ManageStaticPagesRepository : IManageStaticPagesRepository
    {
        private readonly string _connectionString;

        public ManageStaticPagesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ManageStaticPagesLibrary staticPages)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO ManageStaticPages (Name, Link, PageContent, Status, CreatedBy, CreatedAt)
VALUES (@name, @link, @pageContent, @status, @createdBy, @createdAt);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", (object?)staticPages.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@link", (object?)staticPages.Link ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@pageContent", (object?)staticPages.PageContent ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", (object?)staticPages.Status ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdBy", (object?)staticPages.CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdAt", staticPages.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(ManageStaticPagesLibrary staticPages)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE ManageStaticPages
SET Name = @name,
    Link = @link,
    PageContent = @pageContent,
    Status = @status,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", staticPages.Id);
                cmd.Parameters.AddWithValue("@name", (object?)staticPages.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@link", (object?)staticPages.Link ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@pageContent", (object?)staticPages.PageContent ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", (object?)staticPages.Status ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)staticPages.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", staticPages.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = staticPages.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageStaticPagesLibrary staticPages)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = "DELETE FROM ManageStaticPages WHERE Id = @id;";
                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", staticPages.Id);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = staticPages.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ManageStaticPagesLibrary>>> get(ManageStaticPagesLibrary staticPages, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (staticPages.Id > 0)
                {
                    where.Add("Id = @id");
                    cmd.Parameters.AddWithValue("@id", staticPages.Id);
                }
                if (!string.IsNullOrWhiteSpace(staticPages.Name))
                {
                    where.Add("Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{staticPages.Name}%");
                }
                if (!string.IsNullOrWhiteSpace(staticPages.Status))
                {
                    where.Add("Status = @status");
                    cmd.Parameters.AddWithValue("@status", staticPages.Status);
                }
                if (!string.IsNullOrWhiteSpace(staticPages.Searchtext))
                {
                    where.Add("(Name LIKE @search OR Link LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{staticPages.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;
                cmd.CommandText = $"SELECT COUNT(1) FROM ManageStaticPages{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var offset = (safePageIndex - 1) * safePageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);

                var items = new List<ManageStaticPagesLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT Id, Name, Link, PageContent, Status, CreatedAt, CreatedBy, ModifiedAt, ModifiedBy
FROM ManageStaticPages
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
                        items.Add(new ManageStaticPagesLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                            Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                            PageContent = reader.IsDBNull(reader.GetOrdinal("PageContent")) ? null : reader.GetString(reader.GetOrdinal("PageContent")),
                            Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt"))
                        });
                    }
                }

                return new BaseResponse<List<ManageStaticPagesLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageStaticPagesLibrary>> { code = 400, message = ex.Message, data = new List<ManageStaticPagesLibrary>() };
            }
        }
    }
}
