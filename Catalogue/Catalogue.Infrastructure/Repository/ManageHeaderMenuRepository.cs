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
    public class ManageHeaderMenuRepository : IManageHeaderMenuRepository
    {
        private readonly string _connectionString;

        public ManageHeaderMenuRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ManageHeaderMenu model)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO ManageHeaderMenu
(
    Name, Image, ImageAlt, HasLink, RedirectTo, LendingPageId, CategoryId, StaticPageId,
    CollectionId, CustomLink, Sequence, Color, CreatedBy, Createdat
)
VALUES
(
    @name, @image, @imageAlt, @hasLink, @redirectTo, @lendingPageId, @categoryId, @staticPageId,
    @collectionId, @customLink, @sequence, @color, @createdBy, @createdAt
);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", (object?)model.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@image", (object?)model.Image ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@imageAlt", (object?)model.ImageAlt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@hasLink", model.HasLink);
                cmd.Parameters.AddWithValue("@redirectTo", (object?)model.RedirectTo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lendingPageId", model.LendingPageId);
                cmd.Parameters.AddWithValue("@categoryId", model.CategoryId);
                cmd.Parameters.AddWithValue("@staticPageId", model.StaticPageId);
                cmd.Parameters.AddWithValue("@collectionId", model.CollectionId);
                cmd.Parameters.AddWithValue("@customLink", (object?)model.CustomLink ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sequence", model.Sequence);
                cmd.Parameters.AddWithValue("@color", (object?)model.color ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdBy", (object?)model.CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdAt", model.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(ManageHeaderMenu model)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE ManageHeaderMenu
SET Name = @name,
    Image = @image,
    ImageAlt = @imageAlt,
    HasLink = @hasLink,
    RedirectTo = @redirectTo,
    LendingPageId = @lendingPageId,
    CategoryId = @categoryId,
    StaticPageId = @staticPageId,
    CollectionId = @collectionId,
    CustomLink = @customLink,
    Sequence = @sequence,
    Color = @color,
    ModifiedBy = @modifiedBy,
    Modifiedat = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", model.Id);
                cmd.Parameters.AddWithValue("@name", (object?)model.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@image", (object?)model.Image ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@imageAlt", (object?)model.ImageAlt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@hasLink", model.HasLink);
                cmd.Parameters.AddWithValue("@redirectTo", (object?)model.RedirectTo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lendingPageId", model.LendingPageId);
                cmd.Parameters.AddWithValue("@categoryId", model.CategoryId);
                cmd.Parameters.AddWithValue("@staticPageId", model.StaticPageId);
                cmd.Parameters.AddWithValue("@collectionId", model.CollectionId);
                cmd.Parameters.AddWithValue("@customLink", (object?)model.CustomLink ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sequence", model.Sequence);
                cmd.Parameters.AddWithValue("@color", (object?)model.color ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)model.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", model.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = model.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageHeaderMenu model)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = "DELETE FROM ManageHeaderMenu WHERE Id = @id;";
                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", model.Id);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = model.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ManageHeaderMenu>>> get(ManageHeaderMenu model, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (model.Id > 0)
                {
                    where.Add("Id = @id");
                    cmd.Parameters.AddWithValue("@id", model.Id);
                }
                if (!string.IsNullOrWhiteSpace(model.Name))
                {
                    where.Add("Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{model.Name}%");
                }
                if (!string.IsNullOrWhiteSpace(model.Searchtext))
                {
                    where.Add("(Name LIKE @search OR RedirectTo LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{model.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;
                cmd.CommandText = $"SELECT COUNT(1) FROM ManageHeaderMenu{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var offset = (safePageIndex - 1) * safePageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);

                var items = new List<ManageHeaderMenu>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT Id, Name, Image, ImageAlt, HasLink, RedirectTo, LendingPageId, CategoryId, StaticPageId,
       CollectionId, CustomLink, Sequence, Color, CreatedBy, Createdat, ModifiedBy, Modifiedat
FROM ManageHeaderMenu
{whereClause}
ORDER BY Sequence ASC, Id DESC
LIMIT @offset, @pageSize;";
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        items.Add(new ManageHeaderMenu
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                            Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                            ImageAlt = reader.IsDBNull(reader.GetOrdinal("ImageAlt")) ? null : reader.GetString(reader.GetOrdinal("ImageAlt")),
                            HasLink = !reader.IsDBNull(reader.GetOrdinal("HasLink")) && reader.GetBoolean(reader.GetOrdinal("HasLink")),
                            RedirectTo = reader.IsDBNull(reader.GetOrdinal("RedirectTo")) ? null : reader.GetString(reader.GetOrdinal("RedirectTo")),
                            LendingPageId = reader.IsDBNull(reader.GetOrdinal("LendingPageId")) ? 0 : reader.GetInt32(reader.GetOrdinal("LendingPageId")),
                            CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            StaticPageId = reader.IsDBNull(reader.GetOrdinal("StaticPageId")) ? 0 : reader.GetInt32(reader.GetOrdinal("StaticPageId")),
                            CollectionId = reader.IsDBNull(reader.GetOrdinal("CollectionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CollectionId")),
                            CustomLink = reader.IsDBNull(reader.GetOrdinal("CustomLink")) ? null : reader.GetString(reader.GetOrdinal("CustomLink")),
                            Sequence = reader.IsDBNull(reader.GetOrdinal("Sequence")) ? 0 : reader.GetInt32(reader.GetOrdinal("Sequence")),
                            color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString(reader.GetOrdinal("Color")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("Createdat")) ? null : reader.GetDateTime(reader.GetOrdinal("Createdat")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("Modifiedat")) ? null : reader.GetDateTime(reader.GetOrdinal("Modifiedat"))
                        });
                    }
                }

                return new BaseResponse<List<ManageHeaderMenu>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageHeaderMenu>> { code = 400, message = ex.Message, data = new List<ManageHeaderMenu>() };
            }
        }
    }
}
