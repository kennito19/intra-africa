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
    public class ManageHomePageSectionsRepository : IManageHomePageSectionsRepository
    {
        private readonly string _connectionString;

        public ManageHomePageSectionsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ManageHomePageSectionsLibrary homePageSection)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO ManageHomePageSections
(
    LayoutId, LayoutTypeId, Name, Sequence, SectionColumns, IsTitleVisible, Title, SubTitle, TitlePosition,
    LinkIn, LinkText, Link, LinkPosition, Status, ListType, TopProducts, CategoryId, TotalRowsInSection,
    IsCustomGrid, NumberOfImages, Column1, Column2, Column3, Column4, BackgroundColor, InContainer,
    TitleColor, TextColor, CreatedBy, CreatedAt
)
VALUES
(
    @layoutId, @layoutTypeId, @name, @sequence, @sectionColumns, @isTitleVisible, @title, @subTitle, @titlePosition,
    @linkIn, @linkText, @link, @linkPosition, @status, @listType, @topProducts, @categoryId, @totalRowsInSection,
    @isCustomGrid, @numberOfImages, @column1, @column2, @column3, @column4, @backgroundColor, @inContainer,
    @titleColor, @textColor, @createdBy, @createdAt
);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@layoutId", homePageSection.LayoutId > 0 ? homePageSection.LayoutId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@layoutTypeId", homePageSection.LayoutTypeId > 0 ? homePageSection.LayoutTypeId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@name", (object?)homePageSection.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@sequence", (object?)homePageSection.Sequence ?? 0);
                cmd.Parameters.AddWithValue("@sectionColumns", (object?)homePageSection.SectionColumns ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isTitleVisible", (object?)homePageSection.IsTitleVisible ?? false);
                cmd.Parameters.AddWithValue("@title", (object?)homePageSection.Title ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@subTitle", (object?)homePageSection.SubTitle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@titlePosition", (object?)homePageSection.TitlePosition ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@linkIn", (object?)homePageSection.LinkIn ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@linkText", (object?)homePageSection.LinkText ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@link", (object?)homePageSection.Link ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@linkPosition", (object?)homePageSection.LinkPosition ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", (object?)homePageSection.Status ?? "Active");
                cmd.Parameters.AddWithValue("@listType", (object?)homePageSection.ListType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@topProducts", (object?)homePageSection.TopProducts ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@categoryId", (object?)homePageSection.CategoryId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@totalRowsInSection", (object?)homePageSection.TotalRowsInSection ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isCustomGrid", (object?)homePageSection.IsCustomGrid ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@numberOfImages", (object?)homePageSection.NumberOfImages ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@column1", (object?)homePageSection.Column1 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@column2", (object?)homePageSection.Column2 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@column3", (object?)homePageSection.Column3 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@column4", (object?)homePageSection.Column4 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@backgroundColor", (object?)homePageSection.BackgroundColor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@inContainer", (object?)homePageSection.InContainer ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@titleColor", (object?)homePageSection.TitleColor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@textColor", (object?)homePageSection.TextColor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdBy", (object?)homePageSection.CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdAt", homePageSection.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(ManageHomePageSectionsLibrary homePageSection)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE ManageHomePageSections
SET LayoutId = @layoutId,
    LayoutTypeId = @layoutTypeId,
    Name = @name,
    Sequence = @sequence,
    SectionColumns = @sectionColumns,
    IsTitleVisible = @isTitleVisible,
    Title = @title,
    SubTitle = @subTitle,
    TitlePosition = @titlePosition,
    LinkIn = @linkIn,
    LinkText = @linkText,
    Link = @link,
    LinkPosition = @linkPosition,
    Status = @status,
    ListType = @listType,
    TopProducts = @topProducts,
    CategoryId = @categoryId,
    TotalRowsInSection = @totalRowsInSection,
    IsCustomGrid = @isCustomGrid,
    NumberOfImages = @numberOfImages,
    Column1 = @column1,
    Column2 = @column2,
    Column3 = @column3,
    Column4 = @column4,
    BackgroundColor = @backgroundColor,
    InContainer = @inContainer,
    TitleColor = @titleColor,
    TextColor = @textColor,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", homePageSection.Id);
                cmd.Parameters.AddWithValue("@layoutId", homePageSection.LayoutId > 0 ? homePageSection.LayoutId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@layoutTypeId", homePageSection.LayoutTypeId > 0 ? homePageSection.LayoutTypeId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@name", (object?)homePageSection.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@sequence", (object?)homePageSection.Sequence ?? 0);
                cmd.Parameters.AddWithValue("@sectionColumns", (object?)homePageSection.SectionColumns ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isTitleVisible", (object?)homePageSection.IsTitleVisible ?? false);
                cmd.Parameters.AddWithValue("@title", (object?)homePageSection.Title ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@subTitle", (object?)homePageSection.SubTitle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@titlePosition", (object?)homePageSection.TitlePosition ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@linkIn", (object?)homePageSection.LinkIn ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@linkText", (object?)homePageSection.LinkText ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@link", (object?)homePageSection.Link ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@linkPosition", (object?)homePageSection.LinkPosition ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", (object?)homePageSection.Status ?? "Active");
                cmd.Parameters.AddWithValue("@listType", (object?)homePageSection.ListType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@topProducts", (object?)homePageSection.TopProducts ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@categoryId", (object?)homePageSection.CategoryId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@totalRowsInSection", (object?)homePageSection.TotalRowsInSection ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isCustomGrid", (object?)homePageSection.IsCustomGrid ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@numberOfImages", (object?)homePageSection.NumberOfImages ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@column1", (object?)homePageSection.Column1 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@column2", (object?)homePageSection.Column2 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@column3", (object?)homePageSection.Column3 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@column4", (object?)homePageSection.Column4 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@backgroundColor", (object?)homePageSection.BackgroundColor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@inContainer", (object?)homePageSection.InContainer ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@titleColor", (object?)homePageSection.TitleColor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@textColor", (object?)homePageSection.TextColor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)homePageSection.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", homePageSection.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = homePageSection.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageHomePageSectionsLibrary homePageSection)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var tx = await con.BeginTransactionAsync();

                const string deleteDetailsSql = "DELETE FROM ManageHomePageDetails WHERE SectionId = @id;";
                await using (var deleteDetails = new MySqlCommand(deleteDetailsSql, con, (MySqlTransaction)tx))
                {
                    deleteDetails.Parameters.AddWithValue("@id", homePageSection.Id);
                    await deleteDetails.ExecuteNonQueryAsync();
                }

                const string deleteSectionSql = "DELETE FROM ManageHomePageSections WHERE Id = @id;";
                await using var deleteSection = new MySqlCommand(deleteSectionSql, con, (MySqlTransaction)tx);
                deleteSection.Parameters.AddWithValue("@id", homePageSection.Id);
                var affected = await deleteSection.ExecuteNonQueryAsync();
                await tx.CommitAsync();

                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = homePageSection.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ManageHomePageSectionsLibrary>>> get(ManageHomePageSectionsLibrary homePageSection, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (homePageSection.Id > 0)
                {
                    where.Add("s.Id = @id");
                    cmd.Parameters.AddWithValue("@id", homePageSection.Id);
                }
                if (homePageSection.LayoutId > 0)
                {
                    where.Add("s.LayoutId = @layoutId");
                    cmd.Parameters.AddWithValue("@layoutId", homePageSection.LayoutId);
                }
                if (homePageSection.LayoutTypeId > 0)
                {
                    where.Add("s.LayoutTypeId = @layoutTypeId");
                    cmd.Parameters.AddWithValue("@layoutTypeId", homePageSection.LayoutTypeId);
                }
                if (!string.IsNullOrWhiteSpace(homePageSection.Name))
                {
                    where.Add("s.Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{homePageSection.Name}%");
                }
                if (!string.IsNullOrWhiteSpace(homePageSection.LayoutName))
                {
                    where.Add("l.Name LIKE @layoutName");
                    cmd.Parameters.AddWithValue("@layoutName", $"%{homePageSection.LayoutName}%");
                }
                if (!string.IsNullOrWhiteSpace(homePageSection.LayoutTypeName))
                {
                    where.Add("lt.Name LIKE @layoutTypeName");
                    cmd.Parameters.AddWithValue("@layoutTypeName", $"%{homePageSection.LayoutTypeName}%");
                }
                if (!string.IsNullOrWhiteSpace(homePageSection.Status))
                {
                    where.Add("s.Status = @status");
                    cmd.Parameters.AddWithValue("@status", homePageSection.Status);
                }
                if (!string.IsNullOrWhiteSpace(homePageSection.SearchText))
                {
                    where.Add("(s.Name LIKE @search OR l.Name LIKE @search OR lt.Name LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{homePageSection.SearchText}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;
                cmd.CommandText = $@"
SELECT COUNT(1)
FROM ManageHomePageSections s
LEFT JOIN ManageLayouts l ON l.Id = s.LayoutId
LEFT JOIN ManageLayoutTypes lt ON lt.Id = s.LayoutTypeId
{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var offset = (safePageIndex - 1) * safePageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);

                var items = new List<ManageHomePageSectionsLibrary>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT s.Id, s.LayoutId, s.LayoutTypeId, s.Name, s.Sequence, s.SectionColumns, s.IsTitleVisible,
       s.Title, s.SubTitle, s.TitlePosition, s.LinkIn, s.LinkText, s.Link, s.LinkPosition, s.Status,
       s.ListType, s.TopProducts, s.TotalRowsInSection, s.IsCustomGrid, s.NumberOfImages, s.Column1,
       s.Column2, s.Column3, s.Column4, s.CategoryId, s.BackgroundColor, s.InContainer, s.TitleColor,
       s.TextColor, s.CreatedBy, s.CreatedAt, s.ModifiedBy, s.ModifiedAt,
       l.Name AS LayoutName, lt.Name AS LayoutTypeName
FROM ManageHomePageSections s
LEFT JOIN ManageLayouts l ON l.Id = s.LayoutId
LEFT JOIN ManageLayoutTypes lt ON lt.Id = s.LayoutTypeId
{whereClause}
ORDER BY s.Sequence ASC, s.Id DESC
LIMIT @offset, @pageSize;";
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        items.Add(new ManageHomePageSectionsLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            LayoutId = reader.IsDBNull(reader.GetOrdinal("LayoutId")) ? 0 : reader.GetInt32(reader.GetOrdinal("LayoutId")),
                            LayoutTypeId = reader.IsDBNull(reader.GetOrdinal("LayoutTypeId")) ? 0 : reader.GetInt32(reader.GetOrdinal("LayoutTypeId")),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                            Sequence = reader.IsDBNull(reader.GetOrdinal("Sequence")) ? null : reader.GetInt32(reader.GetOrdinal("Sequence")),
                            SectionColumns = reader.IsDBNull(reader.GetOrdinal("SectionColumns")) ? null : reader.GetInt32(reader.GetOrdinal("SectionColumns")),
                            IsTitleVisible = reader.IsDBNull(reader.GetOrdinal("IsTitleVisible")) ? false : reader.GetBoolean(reader.GetOrdinal("IsTitleVisible")),
                            Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
                            SubTitle = reader.IsDBNull(reader.GetOrdinal("SubTitle")) ? null : reader.GetString(reader.GetOrdinal("SubTitle")),
                            TitlePosition = reader.IsDBNull(reader.GetOrdinal("TitlePosition")) ? null : reader.GetString(reader.GetOrdinal("TitlePosition")),
                            LinkIn = reader.IsDBNull(reader.GetOrdinal("LinkIn")) ? null : reader.GetString(reader.GetOrdinal("LinkIn")),
                            LinkText = reader.IsDBNull(reader.GetOrdinal("LinkText")) ? null : reader.GetString(reader.GetOrdinal("LinkText")),
                            Link = reader.IsDBNull(reader.GetOrdinal("Link")) ? null : reader.GetString(reader.GetOrdinal("Link")),
                            LinkPosition = reader.IsDBNull(reader.GetOrdinal("LinkPosition")) ? null : reader.GetString(reader.GetOrdinal("LinkPosition")),
                            Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                            ListType = reader.IsDBNull(reader.GetOrdinal("ListType")) ? null : reader.GetString(reader.GetOrdinal("ListType")),
                            TopProducts = reader.IsDBNull(reader.GetOrdinal("TopProducts")) ? null : reader.GetInt32(reader.GetOrdinal("TopProducts")),
                            TotalRowsInSection = reader.IsDBNull(reader.GetOrdinal("TotalRowsInSection")) ? null : reader.GetInt32(reader.GetOrdinal("TotalRowsInSection")),
                            IsCustomGrid = reader.IsDBNull(reader.GetOrdinal("IsCustomGrid")) ? false : reader.GetBoolean(reader.GetOrdinal("IsCustomGrid")),
                            NumberOfImages = reader.IsDBNull(reader.GetOrdinal("NumberOfImages")) ? null : reader.GetInt32(reader.GetOrdinal("NumberOfImages")),
                            Column1 = reader.IsDBNull(reader.GetOrdinal("Column1")) ? null : reader.GetInt32(reader.GetOrdinal("Column1")),
                            Column2 = reader.IsDBNull(reader.GetOrdinal("Column2")) ? null : reader.GetInt32(reader.GetOrdinal("Column2")),
                            Column3 = reader.IsDBNull(reader.GetOrdinal("Column3")) ? null : reader.GetInt32(reader.GetOrdinal("Column3")),
                            Column4 = reader.IsDBNull(reader.GetOrdinal("Column4")) ? null : reader.GetInt32(reader.GetOrdinal("Column4")),
                            CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? null : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                            BackgroundColor = reader.IsDBNull(reader.GetOrdinal("BackgroundColor")) ? null : reader.GetString(reader.GetOrdinal("BackgroundColor")),
                            InContainer = reader.IsDBNull(reader.GetOrdinal("InContainer")) ? false : reader.GetBoolean(reader.GetOrdinal("InContainer")),
                            TitleColor = reader.IsDBNull(reader.GetOrdinal("TitleColor")) ? null : reader.GetString(reader.GetOrdinal("TitleColor")),
                            TextColor = reader.IsDBNull(reader.GetOrdinal("TextColor")) ? null : reader.GetString(reader.GetOrdinal("TextColor")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            LayoutName = reader.IsDBNull(reader.GetOrdinal("LayoutName")) ? null : reader.GetString(reader.GetOrdinal("LayoutName")),
                            LayoutTypeName = reader.IsDBNull(reader.GetOrdinal("LayoutTypeName")) ? null : reader.GetString(reader.GetOrdinal("LayoutTypeName"))
                        });
                    }
                }

                return new BaseResponse<List<ManageHomePageSectionsLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageHomePageSectionsLibrary>> { code = 400, message = ex.Message, data = new List<ManageHomePageSectionsLibrary>() };
            }
        }

        public async Task<BaseResponse<List<ProductHomePageSectionLibrary>>> getProudctHomePageSection(ProductHomePageSectionLibrary proudctHomePageSection, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string> { "IFNULL(pm.IsDeleted, 0) = 0" };
                if (proudctHomePageSection.categoryId > 0)
                {
                    where.Add("pm.CategoryId = @categoryId");
                    cmd.Parameters.AddWithValue("@categoryId", proudctHomePageSection.categoryId);
                }
                if (!string.IsNullOrWhiteSpace(proudctHomePageSection.productIds))
                {
                    where.Add("FIND_IN_SET(CAST(pm.Id AS CHAR), @productIds)");
                    cmd.Parameters.AddWithValue("@productIds", proudctHomePageSection.productIds);
                }

                var whereClause = $" WHERE {string.Join(" AND ", where)}";
                var top = proudctHomePageSection.topProduct.HasValue && proudctHomePageSection.topProduct.Value > 0
                    ? proudctHomePageSection.topProduct.Value
                    : 20;

                cmd.CommandText = $@"
SELECT pm.Id, pm.Guid, pm.IsMasterProduct, pm.ParentId, pm.CategoryId, pm.AssiCategoryId,
       pm.ProductName, pm.CustomeProductName, pm.CompanySKUCode, pm.CreatedAt, pm.ModifiedAt,
       (SELECT pi.Url FROM ProductImages pi WHERE pi.ProductID = pm.Id AND pi.Type = 'Image' ORDER BY pi.Sequence LIMIT 1) AS Image1,
       COALESCE(spm.MRP, 0) AS MRP,
       COALESCE(spm.SellingPrice, 0) AS SellingPrice,
       COALESCE(spm.Discount, 0) AS Discount,
       COALESCE(spm.Quantity, 0) AS Quantity,
       spm.Id AS SellerProductId,
       spm.SellerID AS SellerId,
       spm.BrandID AS BrandId,
       spm.ExtraDetails,
       spm.Status,
       spm.Live
FROM ProductMaster pm
LEFT JOIN SellerProductMaster spm ON spm.ProductID = pm.Id AND spm.IsDeleted = 0 AND spm.Status = 'Active'
{whereClause}
ORDER BY pm.Id DESC
LIMIT @top;";
                cmd.Parameters.AddWithValue("@top", top);

                var items = new List<ProductHomePageSectionLibrary>();
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    items.Add(new ProductHomePageSectionLibrary
                    {
                        id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Guid = reader.IsDBNull(reader.GetOrdinal("Guid")) ? null : reader.GetString(reader.GetOrdinal("Guid")),
                        isMasterProduct = !reader.IsDBNull(reader.GetOrdinal("IsMasterProduct")) && reader.GetBoolean(reader.GetOrdinal("IsMasterProduct")),
                        parentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ParentId")),
                        categoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CategoryId")),
                        assiCategoryId = reader.IsDBNull(reader.GetOrdinal("AssiCategoryId")) ? 0 : reader.GetInt32(reader.GetOrdinal("AssiCategoryId")),
                        productName = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? null : reader.GetString(reader.GetOrdinal("ProductName")),
                        customeProductName = reader.IsDBNull(reader.GetOrdinal("CustomeProductName")) ? null : reader.GetString(reader.GetOrdinal("CustomeProductName")),
                        companySkuCode = reader.IsDBNull(reader.GetOrdinal("CompanySKUCode")) ? null : reader.GetString(reader.GetOrdinal("CompanySKUCode")),
                        image1 = reader.IsDBNull(reader.GetOrdinal("Image1")) ? null : reader.GetString(reader.GetOrdinal("Image1")),
                        mrp = reader.IsDBNull(reader.GetOrdinal("MRP")) ? 0 : reader.GetDecimal(reader.GetOrdinal("MRP")),
                        sellingPrice = reader.IsDBNull(reader.GetOrdinal("SellingPrice")) ? 0 : reader.GetDecimal(reader.GetOrdinal("SellingPrice")),
                        discount = reader.IsDBNull(reader.GetOrdinal("Discount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Discount")),
                        Quantity = reader.IsDBNull(reader.GetOrdinal("Quantity")) ? 0 : reader.GetInt32(reader.GetOrdinal("Quantity")),
                        createdAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        modifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                        categoryName = null,
                        categoryPathIds = null,
                        categoryPathNames = null,
                        sellerProductId = reader.IsDBNull(reader.GetOrdinal("SellerProductId")) ? 0 : reader.GetInt32(reader.GetOrdinal("SellerProductId")),
                        sellerId = reader.IsDBNull(reader.GetOrdinal("SellerId")) ? null : reader.GetString(reader.GetOrdinal("SellerId")),
                        brandId = reader.IsDBNull(reader.GetOrdinal("BrandId")) ? 0 : reader.GetInt32(reader.GetOrdinal("BrandId")),
                        brandName = null,
                        status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "Active" : reader.GetString(reader.GetOrdinal("Status")),
                        live = !reader.IsDBNull(reader.GetOrdinal("Live")) && reader.GetBoolean(reader.GetOrdinal("Live")),
                        extraDetails = reader.IsDBNull(reader.GetOrdinal("ExtraDetails")) ? null : reader.GetString(reader.GetOrdinal("ExtraDetails")),
                        totalVariant = 0
                    });
                }

                return new BaseResponse<List<ProductHomePageSectionLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ProductHomePageSectionLibrary>> { code = 400, message = ex.Message, data = new List<ProductHomePageSectionLibrary>() };
            }
        }
    }
}
