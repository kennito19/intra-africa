using Catalogue.Application.IRepositories;
using Catalogue.Domain;
using Catalogue.Domain.DTO;
using Catalogue.Domain.Entity;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Catalogue.Infrastructure.Repository
{
    public class ManageHomePageDetailsRepository : IManageHomePageDetailsRepository
    {
        private readonly string _connectionString;

        public ManageHomePageDetailsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ManageHomePageDetailsLibrary pageDetails)
        {
            try
            {
                if ((pageDetails.SectionId ?? 0) <= 0)
                {
                    return new BaseResponse<long> { code = 400, message = "Section is required.", data = 0 };
                }

                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                const string sql = @"
INSERT INTO managehomepagedetails
(SectionId, LayoutTypeDetailsId, OptionId, Image, ImageAlt, IsTitleVisible, Title, SubTitle, TitlePosition, Sequence, Columns,
 RedirectTo, CategoryId, BrandIds, SizeIds, SpecificationIds, ColorIds, CollectionId, ProductId, StaticPageId, LendingPageId,
 CustomLinks, TitleColor, SubTitleColor, TitleSize, SubTitleSize, ItalicSubTitle, ItalicTitle, Description, SliderType, VideoLinkType,
 VideoId, Name, AssignCity, AssignState, AssignCountry, Status, CreatedBy, CreatedAt)
VALUES
(@SectionId, @LayoutTypeDetailsId, @OptionId, @Image, @ImageAlt, @IsTitleVisible, @Title, @SubTitle, @TitlePosition, @Sequence, @Columns,
 @RedirectTo, @CategoryId, @BrandIds, @SizeIds, @SpecificationIds, @ColorIds, @CollectionId, @ProductId, @StaticPageId, @LendingPageId,
 @CustomLinks, @TitleColor, @SubTitleColor, @TitleSize, @SubTitleSize, @ItalicSubTitle, @ItalicTitle, @Description, @SliderType, @VideoLinkType,
 @VideoId, @Name, @AssignCity, @AssignState, @AssignCountry, @Status, @CreatedBy, @CreatedAt);
SELECT LAST_INSERT_ID();";
                await using var cmd = new MySqlCommand(sql, con);
                BindCommon(cmd, pageDetails, false);
                cmd.Parameters.AddWithValue("@CreatedBy", (object?)pageDetails.CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedAt", pageDetails.CreatedAt ?? DateTime.Now);
                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(ManageHomePageDetailsLibrary pageDetails)
        {
            try
            {
                if ((pageDetails.SectionId ?? 0) <= 0)
                {
                    return new BaseResponse<long> { code = 400, message = "Section is required.", data = 0 };
                }

                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                const string sql = @"
UPDATE managehomepagedetails SET
SectionId=@SectionId, LayoutTypeDetailsId=@LayoutTypeDetailsId, OptionId=@OptionId, Image=@Image, ImageAlt=@ImageAlt, IsTitleVisible=@IsTitleVisible,
Title=@Title, SubTitle=@SubTitle, TitlePosition=@TitlePosition, Sequence=@Sequence, Columns=@Columns, RedirectTo=@RedirectTo, CategoryId=@CategoryId,
BrandIds=@BrandIds, SizeIds=@SizeIds, SpecificationIds=@SpecificationIds, ColorIds=@ColorIds, CollectionId=@CollectionId, ProductId=@ProductId,
StaticPageId=@StaticPageId, LendingPageId=@LendingPageId, CustomLinks=@CustomLinks, TitleColor=@TitleColor, SubTitleColor=@SubTitleColor,
TitleSize=@TitleSize, SubTitleSize=@SubTitleSize, ItalicSubTitle=@ItalicSubTitle, ItalicTitle=@ItalicTitle, Description=@Description,
SliderType=@SliderType, VideoLinkType=@VideoLinkType, VideoId=@VideoId, Name=@Name, AssignCity=@AssignCity, AssignState=@AssignState,
AssignCountry=@AssignCountry, Status=@Status, ModifiedBy=@ModifiedBy, ModifiedAt=@ModifiedAt
WHERE Id=@Id;";
                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Id", pageDetails.Id);
                BindCommon(cmd, pageDetails, false);
                cmd.Parameters.AddWithValue("@ModifiedBy", (object?)pageDetails.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ModifiedAt", pageDetails.ModifiedAt ?? DateTime.Now);
                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = pageDetails.Id ?? 0
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageHomePageDetailsLibrary pageDetails)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                var deleteById = (pageDetails.Id ?? 0) > 0;
                var sql = deleteById ? "DELETE FROM managehomepagedetails WHERE Id=@Id;" : "DELETE FROM managehomepagedetails WHERE SectionId=@SectionId;";
                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue(deleteById ? "@Id" : "@SectionId", deleteById ? pageDetails.Id : pageDetails.SectionId);
                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = (pageDetails.Id ?? pageDetails.SectionId) ?? 0
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ManageHomePageDetailsLibrary>>> get(ManageHomePageDetailsLibrary pageDetails, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };
                var where = new List<string>();
                if ((pageDetails.Id ?? 0) > 0) { where.Add("d.Id=@Id"); cmd.Parameters.AddWithValue("@Id", pageDetails.Id); }
                if ((pageDetails.SectionId ?? 0) > 0) { where.Add("d.SectionId=@SectionId"); cmd.Parameters.AddWithValue("@SectionId", pageDetails.SectionId); }
                if ((pageDetails.LayoutTypeDetailsId ?? 0) > 0) { where.Add("d.LayoutTypeDetailsId=@LayoutTypeDetailsId"); cmd.Parameters.AddWithValue("@LayoutTypeDetailsId", pageDetails.LayoutTypeDetailsId); }
                if ((pageDetails.OptionId ?? 0) > 0) { where.Add("d.OptionId=@OptionId"); cmd.Parameters.AddWithValue("@OptionId", pageDetails.OptionId); }
                if (!string.IsNullOrWhiteSpace(pageDetails.SectionName)) { where.Add("s.Name LIKE @SectionName"); cmd.Parameters.AddWithValue("@SectionName", $"%{pageDetails.SectionName}%"); }
                if (!string.IsNullOrWhiteSpace(pageDetails.Status)) { where.Add("d.Status=@Status"); cmd.Parameters.AddWithValue("@Status", pageDetails.Status); }
                if (!string.IsNullOrWhiteSpace(pageDetails.SearchText)) { where.Add("(d.Title LIKE @Search OR d.SubTitle LIKE @Search OR s.Name LIKE @Search)"); cmd.Parameters.AddWithValue("@Search", $"%{pageDetails.SearchText}%"); }
                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $"SELECT COUNT(1) FROM managehomepagedetails d LEFT JOIN managehomepagesections s ON s.Id=d.SectionId{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);
                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var offset = (safePageIndex - 1) * safePageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);

                var items = new List<ManageHomePageDetailsLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT d.*, s.Name AS SectionName, o.Name AS OptionName
FROM managehomepagedetails d
LEFT JOIN managehomepagesections s ON s.Id=d.SectionId
LEFT JOIN layoutoptions o ON o.Id=d.OptionId
{whereClause}
ORDER BY d.SectionId ASC, d.Sequence ASC, d.Id DESC
LIMIT @offset, @pageSize;";
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);
                    await using var reader = await cmd.ExecuteReaderAsync();
                    var row = offset;
                    while (await reader.ReadAsync())
                    {
                        row++;
                        items.Add(new ManageHomePageDetailsLibrary
                        {
                            RowNumber = row,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            SectionId = reader.GetInt32(reader.GetOrdinal("SectionId")),
                            LayoutTypeDetailsId = GetInt(reader, "LayoutTypeDetailsId"),
                            OptionId = GetInt(reader, "OptionId"),
                            Image = GetString(reader, "Image"),
                            ImageAlt = GetString(reader, "ImageAlt"),
                            IsTitleVisible = GetBool(reader, "IsTitleVisible"),
                            Title = GetString(reader, "Title"),
                            SubTitle = GetString(reader, "SubTitle"),
                            TitlePosition = GetString(reader, "TitlePosition"),
                            Sequence = GetInt(reader, "Sequence") ?? 0,
                            Columns = GetInt(reader, "Columns"),
                            RedirectTo = GetString(reader, "RedirectTo"),
                            CategoryId = GetInt(reader, "CategoryId"),
                            BrandIds = GetString(reader, "BrandIds"),
                            SizeIds = GetString(reader, "SizeIds"),
                            SpecificationIds = GetString(reader, "SpecificationIds"),
                            ColorIds = GetString(reader, "ColorIds"),
                            CollectionId = GetInt(reader, "CollectionId"),
                            ProductId = GetString(reader, "ProductId"),
                            StaticPageId = GetInt(reader, "StaticPageId"),
                            LendingPageId = GetInt(reader, "LendingPageId"),
                            CustomLinks = GetString(reader, "CustomLinks"),
                            TitleColor = GetString(reader, "TitleColor"),
                            SubTitleColor = GetString(reader, "SubTitleColor"),
                            TitleSize = GetString(reader, "TitleSize"),
                            SubTitleSize = GetString(reader, "SubTitleSize"),
                            ItalicSubTitle = GetBool(reader, "ItalicSubTitle"),
                            ItalicTitle = GetBool(reader, "ItalicTitle"),
                            Description = GetString(reader, "Description"),
                            SliderType = GetString(reader, "SliderType"),
                            VideoLinkType = GetString(reader, "VideoLinkType"),
                            VideoId = GetString(reader, "VideoId"),
                            Name = GetString(reader, "Name"),
                            AssignCity = GetString(reader, "AssignCity"),
                            AssignState = GetString(reader, "AssignState"),
                            AssignCountry = GetString(reader, "AssignCountry"),
                            Status = GetString(reader, "Status") ?? string.Empty,
                            CreatedBy = GetString(reader, "CreatedBy"),
                            CreatedAt = GetDate(reader, "CreatedAt"),
                            ModifiedBy = GetString(reader, "ModifiedBy"),
                            ModifiedAt = GetDate(reader, "ModifiedAt"),
                            SectionName = GetString(reader, "SectionName"),
                            OptionName = GetString(reader, "OptionName")
                        });
                    }
                }

                return new BaseResponse<List<ManageHomePageDetailsLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageHomePageDetailsLibrary>> { code = 400, message = ex.Message, data = new List<ManageHomePageDetailsLibrary>() };
            }
        }

        public async Task<BaseResponse<List<FrontHomepageDetailsDto>>> GetFrontHomepageDetails(string Mode, string? Status = null)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };
                var where = string.Empty;
                if (!string.IsNullOrWhiteSpace(Status))
                {
                    where = "WHERE s.Status = @status";
                    cmd.Parameters.AddWithValue("@status", Status);
                }

                cmd.CommandText = $@"
SELECT s.Id AS HomePageSectionId, s.LayoutId, s.LayoutTypeId, s.Name AS HomePageSectionName, s.Sequence AS HomePageSectionSequence,
       s.SectionColumns, s.Title AS HomePageSectionTitle, s.SubTitle AS HomePageSectionSubTitle, s.Status AS HomePageSectionStatus,
       s.ListType, s.TopProducts, s.CategoryId, d.Id AS HomePageSectionDetailsId, d.LayoutTypeDetailsId, d.OptionId, d.Image,
       d.ImageAlt, d.Title AS HomePageSectionDetailsTitle, d.SubTitle AS HomePageSectionDetailsSubTitle, d.Sequence AS HomePageSectionDetailsSequence,
       d.RedirectTo, d.ProductId, d.Status AS HomePageSectionDetailsStatus, l.Name AS LayoutName, t.Name AS LayoutTypeName,
       td.Name AS LayoutTypeDetailsName, o.Name AS LayoutOptionName, p.ProductName, c.Name AS CategoryName, c.PathNames AS CategoryPathName
FROM managehomepagesections s
LEFT JOIN managehomepagedetails d ON d.SectionId = s.Id
LEFT JOIN managelayouts l ON l.Id = s.LayoutId
LEFT JOIN managelayouttypes t ON t.Id = s.LayoutTypeId
LEFT JOIN managelayouttypesdetails td ON td.Id = d.LayoutTypeDetailsId
LEFT JOIN layoutoptions o ON o.Id = d.OptionId
LEFT JOIN productmaster p ON p.Id = d.ProductId
LEFT JOIN categorylibrary c ON c.Id = COALESCE(d.CategoryId, s.CategoryId)
{where}
ORDER BY s.Sequence ASC, d.Sequence ASC, d.Id ASC;";

                var items = new List<FrontHomepageDetailsDto>();
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    items.Add(new FrontHomepageDetailsDto
                    {
                        HomePageSectionId = GetInt(reader, "HomePageSectionId"),
                        LayoutId = GetInt(reader, "LayoutId"),
                        LayoutTypeId = GetInt(reader, "LayoutTypeId"),
                        HomePageSectionName = GetString(reader, "HomePageSectionName"),
                        HomePageSectionSequence = GetInt(reader, "HomePageSectionSequence"),
                        SectionColumns = GetInt(reader, "SectionColumns"),
                        HomePageSectionTitle = GetString(reader, "HomePageSectionTitle"),
                        HomePageSectionSubTitle = GetString(reader, "HomePageSectionSubTitle"),
                        HomePageSectionStatus = GetString(reader, "HomePageSectionStatus"),
                        ListType = GetString(reader, "ListType"),
                        TopProducts = GetInt(reader, "TopProducts"),
                        CategoryId = GetInt(reader, "CategoryId"),
                        HomePageSectionDetailsId = GetInt(reader, "HomePageSectionDetailsId"),
                        LayoutTypeDetailsId = GetInt(reader, "LayoutTypeDetailsId"),
                        OptionId = GetInt(reader, "OptionId"),
                        Image = GetString(reader, "Image"),
                        ImageAlt = GetString(reader, "ImageAlt"),
                        HomePageSectionDetailsTitle = GetString(reader, "HomePageSectionDetailsTitle"),
                        HomePageSectionDetailsSubTitle = GetString(reader, "HomePageSectionDetailsSubTitle"),
                        HomePageSectionDetailsSequence = GetInt(reader, "HomePageSectionDetailsSequence"),
                        RedirectTo = GetString(reader, "RedirectTo"),
                        ProductId = GetString(reader, "ProductId"),
                        HomePageSectionDetailsStatus = GetString(reader, "HomePageSectionDetailsStatus"),
                        LayoutName = GetString(reader, "LayoutName"),
                        LayoutTypeName = GetString(reader, "LayoutTypeName"),
                        LayoutTypeDetailsName = GetString(reader, "LayoutTypeDetailsName"),
                        LayoutOptionName = GetString(reader, "LayoutOptionName"),
                        ProductName = GetString(reader, "ProductName"),
                        CategoryName = GetString(reader, "CategoryName"),
                        CategoryPathName = GetString(reader, "CategoryPathName")
                    });
                }

                return new BaseResponse<List<FrontHomepageDetailsDto>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<FrontHomepageDetailsDto>> { code = 400, message = ex.Message, data = new List<FrontHomepageDetailsDto>() };
            }
        }

        private static void BindCommon(MySqlCommand cmd, ManageHomePageDetailsLibrary p, bool includeId)
        {
            if (includeId) cmd.Parameters.AddWithValue("@Id", p.Id);
            cmd.Parameters.AddWithValue("@SectionId", p.SectionId.HasValue && p.SectionId.Value > 0 ? p.SectionId.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LayoutTypeDetailsId", p.LayoutTypeDetailsId.HasValue && p.LayoutTypeDetailsId.Value > 0 ? p.LayoutTypeDetailsId.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@OptionId", p.OptionId.HasValue && p.OptionId.Value > 0 ? p.OptionId.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Image", (object?)p.Image ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ImageAlt", (object?)p.ImageAlt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@IsTitleVisible", (object?)p.IsTitleVisible ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Title", (object?)p.Title ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SubTitle", (object?)p.SubTitle ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TitlePosition", (object?)p.TitlePosition ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Sequence", p.Sequence > 0 ? p.Sequence : 1);
            cmd.Parameters.AddWithValue("@Columns", (object?)p.Columns ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RedirectTo", (object?)p.RedirectTo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CategoryId", p.CategoryId.HasValue && p.CategoryId.Value > 0 ? p.CategoryId.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@BrandIds", (object?)p.BrandIds ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SizeIds", (object?)p.SizeIds ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SpecificationIds", (object?)p.SpecificationIds ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ColorIds", (object?)p.ColorIds ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CollectionId", p.CollectionId.HasValue && p.CollectionId.Value > 0 ? p.CollectionId.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@ProductId", (object?)p.ProductId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@StaticPageId", p.StaticPageId.HasValue && p.StaticPageId.Value > 0 ? p.StaticPageId.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@LendingPageId", p.LendingPageId.HasValue && p.LendingPageId.Value > 0 ? p.LendingPageId.Value : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@CustomLinks", (object?)p.CustomLinks ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TitleColor", (object?)p.TitleColor ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SubTitleColor", (object?)p.SubTitleColor ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@TitleSize", (object?)p.TitleSize ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SubTitleSize", (object?)p.SubTitleSize ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ItalicSubTitle", (object?)p.ItalicSubTitle ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ItalicTitle", (object?)p.ItalicTitle ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Description", (object?)p.Description ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SliderType", (object?)p.SliderType ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@VideoLinkType", (object?)p.VideoLinkType ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@VideoId", (object?)p.VideoId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Name", (object?)p.Name ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AssignCity", (object?)p.AssignCity ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AssignState", (object?)p.AssignState ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@AssignCountry", (object?)p.AssignCountry ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", string.IsNullOrWhiteSpace(p.Status) ? "Active" : p.Status);
        }

        private static string? GetString(MySqlDataReader r, string name) => r.IsDBNull(r.GetOrdinal(name)) ? null : r.GetString(r.GetOrdinal(name));
        private static int? GetInt(MySqlDataReader r, string name) => r.IsDBNull(r.GetOrdinal(name)) ? null : r.GetInt32(r.GetOrdinal(name));
        private static bool? GetBool(MySqlDataReader r, string name) => r.IsDBNull(r.GetOrdinal(name)) ? null : r.GetBoolean(r.GetOrdinal(name));
        private static DateTime? GetDate(MySqlDataReader r, string name) => r.IsDBNull(r.GetOrdinal(name)) ? null : r.GetDateTime(r.GetOrdinal(name));
    }
}
