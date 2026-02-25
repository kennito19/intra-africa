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
    public class ManageSubMenuRepository : IManageSubMenuRepository
    {
        private readonly string _connectionString;

        public ManageSubMenuRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ManageSubMenu model)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO ManageSubMenu
(
    MenuType, HeaderId, ParentId, Name, Image, ImageAlt, HasLink, RedirectTo, LendingPageId,
    CategoryId, StaticPageId, CollectionId, CustomLink, Sizes, Colors, Specifications, Brands,
    Sequence, CreatedBy, Createdat
)
VALUES
(
    @menuType, @headerId, @parentId, @name, @image, @imageAlt, @hasLink, @redirectTo, @lendingPageId,
    @categoryId, @staticPageId, @collectionId, @customLink, @sizes, @colors, @specifications, @brands,
    @sequence, @createdBy, @createdAt
);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@menuType", (object?)model.MenuType ?? string.Empty);
                cmd.Parameters.AddWithValue("@headerId", model.HeaderId > 0 ? model.HeaderId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@parentId", model.ParentId.HasValue && model.ParentId.Value > 0 ? model.ParentId.Value : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@name", (object?)model.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@image", (object?)model.Image ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@imageAlt", (object?)model.ImageAlt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@hasLink", model.HasLink);
                cmd.Parameters.AddWithValue("@redirectTo", (object?)model.RedirectTo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lendingPageId", (object?)model.LendingPageId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@categoryId", (object?)model.CategoryId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@staticPageId", (object?)model.StaticPageId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@collectionId", (object?)model.CollectionId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@customLink", (object?)model.CustomLink ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sizes", (object?)model.Sizes ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@colors", (object?)model.Colors ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@specifications", (object?)model.Specifications ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@brands", (object?)model.Brands ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sequence", model.Sequence);
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

        public async Task<BaseResponse<long>> Update(ManageSubMenu model)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE ManageSubMenu
SET MenuType = @menuType,
    HeaderId = @headerId,
    ParentId = @parentId,
    Name = @name,
    Image = @image,
    ImageAlt = @imageAlt,
    HasLink = @hasLink,
    RedirectTo = @redirectTo,
    LendingPageId = @lendingPageId,
    CategoryId = @categoryId,
    StaticPageId = @staticPageId,
    CollectionId = @collectionId,
    CustomLink = @customLink,
    Sizes = @sizes,
    Colors = @colors,
    Specifications = @specifications,
    Brands = @brands,
    Sequence = @sequence,
    ModifiedBy = @modifiedBy,
    Modifiedat = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", model.Id);
                cmd.Parameters.AddWithValue("@menuType", (object?)model.MenuType ?? string.Empty);
                cmd.Parameters.AddWithValue("@headerId", model.HeaderId > 0 ? model.HeaderId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@parentId", model.ParentId.HasValue && model.ParentId.Value > 0 ? model.ParentId.Value : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@name", (object?)model.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@image", (object?)model.Image ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@imageAlt", (object?)model.ImageAlt ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@hasLink", model.HasLink);
                cmd.Parameters.AddWithValue("@redirectTo", (object?)model.RedirectTo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@lendingPageId", (object?)model.LendingPageId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@categoryId", (object?)model.CategoryId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@staticPageId", (object?)model.StaticPageId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@collectionId", (object?)model.CollectionId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@customLink", (object?)model.CustomLink ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sizes", (object?)model.Sizes ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@colors", (object?)model.Colors ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@specifications", (object?)model.Specifications ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@brands", (object?)model.Brands ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sequence", model.Sequence);
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

        public async Task<BaseResponse<long>> Delete(ManageSubMenu model)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = "DELETE FROM ManageSubMenu WHERE Id = @id;";
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

        public async Task<BaseResponse<List<ManageSubMenu>>> get(ManageSubMenu model, int PageIndex, int PageSize, string Mode, string HeaderName = null, string ParentName = null, bool getParent = false, bool getChild = false)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (model.Id > 0)
                {
                    where.Add("sm.Id = @id");
                    cmd.Parameters.AddWithValue("@id", model.Id);
                }
                if (model.ParentId.HasValue)
                {
                    where.Add("sm.ParentId = @parentId");
                    cmd.Parameters.AddWithValue("@parentId", model.ParentId.Value);
                }
                if (model.HeaderId > 0)
                {
                    where.Add("sm.HeaderId = @headerId");
                    cmd.Parameters.AddWithValue("@headerId", model.HeaderId);
                }
                if (model.CategoryId.HasValue && model.CategoryId.Value > 0)
                {
                    where.Add("sm.CategoryId = @categoryId");
                    cmd.Parameters.AddWithValue("@categoryId", model.CategoryId.Value);
                }
                if (!string.IsNullOrWhiteSpace(model.MenuType))
                {
                    where.Add("sm.MenuType = @menuType");
                    cmd.Parameters.AddWithValue("@menuType", model.MenuType);
                }
                if (!string.IsNullOrWhiteSpace(model.Name))
                {
                    where.Add("sm.Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{model.Name}%");
                }
                if (!string.IsNullOrWhiteSpace(HeaderName))
                {
                    where.Add("hm.Name LIKE @headerName");
                    cmd.Parameters.AddWithValue("@headerName", $"%{HeaderName}%");
                }
                if (!string.IsNullOrWhiteSpace(ParentName))
                {
                    where.Add("pm.Name LIKE @parentName");
                    cmd.Parameters.AddWithValue("@parentName", $"%{ParentName}%");
                }
                if (getParent)
                {
                    where.Add("sm.ParentId IS NULL");
                }
                if (getChild)
                {
                    where.Add("sm.ParentId IS NOT NULL");
                }
                if (!string.IsNullOrWhiteSpace(model.Searchtext))
                {
                    where.Add("(sm.Name LIKE @search OR hm.Name LIKE @search OR pm.Name LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{model.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT COUNT(1)
FROM ManageSubMenu sm
LEFT JOIN ManageHeaderMenu hm ON hm.Id = sm.HeaderId
LEFT JOIN ManageSubMenu pm ON pm.Id = sm.ParentId
{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var offset = (safePageIndex - 1) * safePageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);

                var items = new List<ManageSubMenu>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT sm.Id, sm.MenuType, sm.HeaderId, sm.ParentId, sm.Name, sm.Image, sm.ImageAlt, sm.HasLink, sm.RedirectTo,
       sm.LendingPageId, sm.CategoryId, sm.StaticPageId, sm.CollectionId, sm.CustomLink, sm.Sizes, sm.Colors,
       sm.Specifications, sm.Brands, sm.Sequence, sm.CreatedBy, sm.Createdat, sm.ModifiedBy, sm.Modifiedat
FROM ManageSubMenu sm
LEFT JOIN ManageHeaderMenu hm ON hm.Id = sm.HeaderId
LEFT JOIN ManageSubMenu pm ON pm.Id = sm.ParentId
{whereClause}
ORDER BY sm.Sequence ASC, sm.Id DESC
LIMIT @offset, @pageSize;";
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        items.Add(new ManageSubMenu
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            MenuType = reader.IsDBNull(reader.GetOrdinal("MenuType")) ? null : reader.GetString(reader.GetOrdinal("MenuType")),
                            HeaderId = reader.IsDBNull(reader.GetOrdinal("HeaderId")) ? 0 : reader.GetInt32(reader.GetOrdinal("HeaderId")),
                            ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? null : reader.GetInt32(reader.GetOrdinal("ParentId")),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                            Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                            ImageAlt = reader.IsDBNull(reader.GetOrdinal("ImageAlt")) ? null : reader.GetString(reader.GetOrdinal("ImageAlt")),
                            HasLink = !reader.IsDBNull(reader.GetOrdinal("HasLink")) && reader.GetBoolean(reader.GetOrdinal("HasLink")),
                            RedirectTo = reader.IsDBNull(reader.GetOrdinal("RedirectTo")) ? null : reader.GetString(reader.GetOrdinal("RedirectTo")),
                            LendingPageId = reader.IsDBNull(reader.GetOrdinal("LendingPageId")) ? null : reader.GetInt32(reader.GetOrdinal("LendingPageId")),
                            CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? null : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            StaticPageId = reader.IsDBNull(reader.GetOrdinal("StaticPageId")) ? null : reader.GetInt32(reader.GetOrdinal("StaticPageId")),
                            CollectionId = reader.IsDBNull(reader.GetOrdinal("CollectionId")) ? null : reader.GetInt32(reader.GetOrdinal("CollectionId")),
                            CustomLink = reader.IsDBNull(reader.GetOrdinal("CustomLink")) ? null : reader.GetString(reader.GetOrdinal("CustomLink")),
                            Sizes = reader.IsDBNull(reader.GetOrdinal("Sizes")) ? null : reader.GetString(reader.GetOrdinal("Sizes")),
                            Colors = reader.IsDBNull(reader.GetOrdinal("Colors")) ? null : reader.GetString(reader.GetOrdinal("Colors")),
                            Specifications = reader.IsDBNull(reader.GetOrdinal("Specifications")) ? null : reader.GetString(reader.GetOrdinal("Specifications")),
                            Brands = reader.IsDBNull(reader.GetOrdinal("Brands")) ? null : reader.GetString(reader.GetOrdinal("Brands")),
                            Sequence = reader.IsDBNull(reader.GetOrdinal("Sequence")) ? 0 : reader.GetInt32(reader.GetOrdinal("Sequence")),
                            HeaderColor = null,
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("Createdat")) ? null : reader.GetDateTime(reader.GetOrdinal("Createdat")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("Modifiedat")) ? null : reader.GetDateTime(reader.GetOrdinal("Modifiedat"))
                        });
                    }
                }

                return new BaseResponse<List<ManageSubMenu>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageSubMenu>> { code = 400, message = ex.Message, data = new List<ManageSubMenu>() };
            }
        }
    }
}
