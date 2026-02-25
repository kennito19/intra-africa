using Catalogue.Application.IRepositories;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using Catalogue.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using MySqlConnector;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Infrastructure.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        MySqlConnection con;
        public CategoryRepository(IConfiguration configuration)
        {

            string connectionString = configuration.GetConnectionString("DBconnection");
            con = new MySqlConnection(connectionString);

            _configuration = configuration;
        }
        public async Task<BaseResponse<long>> Create(CategoryLibrary category)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "add"),
                new MySqlParameter("@name", category.Name),
                new MySqlParameter("@parentId", category.ParentId),
                new MySqlParameter("@image", category.Image),
                new MySqlParameter("@pathids", category.PathIds),
                new MySqlParameter("@pathnames", category.PathNames),
                new MySqlParameter("@currentlevel", category.CurrentLevel),
                new MySqlParameter("@MetaTitles", category.MetaTitles),
                new MySqlParameter("@metaDescription", category.MetaDescription),
                new MySqlParameter("@metakeywords", category.MetaKeywords),
                new MySqlParameter("@status", category.Status),
                new MySqlParameter("@color", category.Color),
                new MySqlParameter("@title", category.Title),
                new MySqlParameter("@subtitle", category.SubTitle),
                new MySqlParameter("@description", category.Description),
                new MySqlParameter("@createdby", category.CreatedBy),
                new MySqlParameter("@createdat", category.CreatedAt),
            };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter newid = new MySqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Category, output, newid, message, sqlParams.ToArray());
            }
            catch(Exception ex)
            {
                if (!IsMissingProcedureException(ex))
                {
                    throw new Exception(ex.Message);
                }

                return await CreateWithoutStoredProcedure(category);
            }
        }

        public async Task<BaseResponse<long>> Update(CategoryLibrary category)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "update"),
                new MySqlParameter("@guid", category.Guid),
                new MySqlParameter("@name", category.Name),
                new MySqlParameter("@currentlevel", category.CurrentLevel),
                new MySqlParameter("@parentId", category.ParentId),
                new MySqlParameter("@image", category.Image),
                new MySqlParameter("@pathids", category.PathIds),
                new MySqlParameter("@pathnames", category.PathNames),
                new MySqlParameter("@MetaTitles", category.MetaTitles),
                new MySqlParameter("@metaDescription", category.MetaDescription),
                new MySqlParameter("@metakeywords", category.MetaKeywords),
                new MySqlParameter("@status", category.Status),
                new MySqlParameter("@color", category.Color),
                new MySqlParameter("@title", category.Title),
                new MySqlParameter("@subtitle", category.SubTitle),
                new MySqlParameter("@description", category.Description),
                new MySqlParameter("@modifiedby", category.ModifiedBy),
                new MySqlParameter("@modifiedat", category.ModifiedAt),
            };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter newid = new MySqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Category, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                if (!IsMissingProcedureException(ex))
                {
                    throw new Exception(ex.Message);
                }

                return await UpdateWithoutStoredProcedure(category);
            }
        }

        public async Task<BaseResponse<long>> Delete(CategoryLibrary category)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", "delete"),
                new MySqlParameter("@id", category.Id),
                new MySqlParameter("@deletedby", category.DeletedBy),
                new MySqlParameter("@deletedat", category.DeletedAt),
            };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter newid = new MySqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Category, output, newid , message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                if (!IsMissingProcedureException(ex))
                {
                    throw new Exception(ex.Message);
                }

                return await DeleteWithoutStoredProcedure(category);
            }
        }

        public async Task<BaseResponse<List<CategoryLibrary>>> get(CategoryLibrary category, bool Getparent = false, bool Getchild = false, int PageIndex = 1 , int PageSize = 10, string? Mode = "get")
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@mode", Mode),
                new MySqlParameter("@id", category.Id),
                new MySqlParameter("@guid", category.Guid),
                new MySqlParameter("@name", category.Name),
                new MySqlParameter("@pathids", category.PathIds),
                new MySqlParameter("@status", category.Status),
                new MySqlParameter("@isdeleted", category.IsDeleted),
                new MySqlParameter("@parentId", category.ParentId),
                new MySqlParameter("@parentname", category.ParentName),
                new MySqlParameter("@getparent", Getparent),
                new MySqlParameter("@getchild", Getchild),
                new MySqlParameter("@searchtext", category.Searchtext),
                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@PageSize", PageSize),

            };
                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                //MySqlParameter newid = new MySqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetCategory, categoryParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                if (!IsMissingProcedureException(ex))
                {
                    throw new Exception(ex.Message);
                }

                return await GetWithoutStoredProcedure(category, Getparent, Getchild, PageIndex, PageSize);
            }
        }

        public async Task<BaseResponse<List<CategoryLibrary>>> GetCategoryWithParent(int Categoryid)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                //new MySqlParameter("@mode", "get"),
                new MySqlParameter("@categoryid", Categoryid),
            };
                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                //MySqlParameter newid = new MySqlParameter();
                //newid.ParameterName = "@newid";
                //newid.Direction = ParameterDirection.Output;
                //newid.MySqlDbType = MySqlDbType.Int64;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetCategoryWithParent, categoryParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                if (!IsMissingProcedureException(ex))
                {
                    throw new Exception(ex.Message);
                }

                return await GetCategoryWithParentWithoutStoredProcedure(Categoryid);
            }
        }

        private async Task<List<CategoryLibrary>> categoryParserAsync(DbDataReader reader)
        {
            List<CategoryLibrary> lstcategory = new List<CategoryLibrary>();
            while (await reader.ReadAsync())
            {
                lstcategory.Add(new CategoryLibrary()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    Guid = Convert.ToString(reader.GetValue(reader.GetOrdinal("Guid"))),
                    Name = Convert.ToString(reader.GetValue(reader.GetOrdinal("Name"))),
                    CurrentLevel = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("CurrentLevel"))),
                    ParentId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentId")))) ? null : Convert.ToInt32(reader.GetValue(reader.GetOrdinal("ParentId"))),
                    Image = Convert.ToString(reader.GetValue(reader.GetOrdinal("Image"))),
                    PathIds = Convert.ToString(reader.GetValue(reader.GetOrdinal("PathIds"))),
                    PathNames = Convert.ToString(reader.GetValue(reader.GetOrdinal("PathNames"))),
                    MetaTitles = Convert.ToString(reader.GetValue(reader.GetOrdinal("MetaTitles"))),
                    MetaDescription = Convert.ToString(reader.GetValue(reader.GetOrdinal("MetaDescription"))),
                    MetaKeywords = Convert.ToString(reader.GetValue(reader.GetOrdinal("MetaKeywords"))),
                    Status = Convert.ToString(reader.GetValue(reader.GetOrdinal("Status"))),
                    Color = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Color")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Color"))),
                    Title = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Title")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Title"))),
                    SubTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("SubTitle"))),
                    Description = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Description")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Description"))),
                    CreatedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedBy"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    ModifiedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedBy"))),
                    ModifiedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ModifiedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("ModifiedAt"))),
                    DeletedBy = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedBy"))),
                    DeletedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("DeletedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("DeletedAt"))),
                    IsDeleted = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsDeleted"))),
                    ParentName = Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentName"))),
                    ParentPathNames = Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentPathNames"))),
                    ParentPathIds = Convert.ToString(reader.GetValue(reader.GetOrdinal("ParentPathIds")))
                });
            }
            return lstcategory;
        }

        private static bool IsMissingProcedureException(Exception ex)
        {
            return ex.Message.Contains("does not exist", StringComparison.OrdinalIgnoreCase)
                   || ex.Message.Contains("PROCEDURE", StringComparison.OrdinalIgnoreCase);
        }

        private async Task<BaseResponse<long>> CreateWithoutStoredProcedure(CategoryLibrary category)
        {
            try
            {
                await using var connection = new MySqlConnection(_configuration.GetConnectionString("DBconnection"));
                await connection.OpenAsync();

                const string sql = @"
INSERT INTO categorylibrary
(`Guid`, `Name`, `CurrentLevel`, `ParentId`, `Image`, `PathIds`, `PathNames`, `MetaTitles`, `MetaDescription`, `MetaKeywords`, `Status`, `Color`, `Title`, `SubTitle`, `Description`, `CreatedBy`, `CreatedAt`, `IsDeleted`)
VALUES
(@guid, @name, @currentLevel, @parentId, @image, @pathIds, @pathNames, @metaTitles, @metaDescription, @metaKeywords, @status, @color, @title, @subTitle, @description, @createdBy, @createdAt, 0);
SELECT LAST_INSERT_ID();";

                await using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@guid", string.IsNullOrWhiteSpace(category.Guid) ? Guid.NewGuid().ToString() : category.Guid);
                command.Parameters.AddWithValue("@name", (object?)category.Name ?? DBNull.Value);
                command.Parameters.AddWithValue("@currentLevel", category.CurrentLevel);
                command.Parameters.AddWithValue("@parentId", category.ParentId.HasValue && category.ParentId.Value > 0 ? category.ParentId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@image", (object?)category.Image ?? DBNull.Value);
                command.Parameters.AddWithValue("@pathIds", (object?)category.PathIds ?? DBNull.Value);
                command.Parameters.AddWithValue("@pathNames", (object?)category.PathNames ?? DBNull.Value);
                command.Parameters.AddWithValue("@metaTitles", (object?)category.MetaTitles ?? DBNull.Value);
                command.Parameters.AddWithValue("@metaDescription", (object?)category.MetaDescription ?? DBNull.Value);
                command.Parameters.AddWithValue("@metaKeywords", (object?)category.MetaKeywords ?? DBNull.Value);
                command.Parameters.AddWithValue("@status", (object?)category.Status ?? DBNull.Value);
                command.Parameters.AddWithValue("@color", (object?)category.Color ?? DBNull.Value);
                command.Parameters.AddWithValue("@title", (object?)category.Title ?? DBNull.Value);
                command.Parameters.AddWithValue("@subTitle", (object?)category.SubTitle ?? DBNull.Value);
                command.Parameters.AddWithValue("@description", (object?)category.Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@createdBy", (object?)category.CreatedBy ?? DBNull.Value);
                command.Parameters.AddWithValue("@createdAt", category.CreatedAt ?? DateTime.Now);

                var insertedId = Convert.ToInt64(await command.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = insertedId };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 500, message = ex.Message, data = 0 };
            }
        }

        private async Task<BaseResponse<long>> UpdateWithoutStoredProcedure(CategoryLibrary category)
        {
            try
            {
                await using var connection = new MySqlConnection(_configuration.GetConnectionString("DBconnection"));
                await connection.OpenAsync();

                const string sql = @"
UPDATE categorylibrary
SET `Name` = @name,
    `CurrentLevel` = @currentLevel,
    `ParentId` = @parentId,
    `Image` = @image,
    `PathIds` = @pathIds,
    `PathNames` = @pathNames,
    `MetaTitles` = @metaTitles,
    `MetaDescription` = @metaDescription,
    `MetaKeywords` = @metaKeywords,
    `Status` = @status,
    `Color` = @color,
    `Title` = @title,
    `SubTitle` = @subTitle,
    `Description` = @description,
    `ModifiedBy` = @modifiedBy,
    `ModifiedAt` = @modifiedAt
WHERE (`Id` = @id OR `Guid` = @guid) AND `IsDeleted` = 0;";

                await using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", category.Id);
                command.Parameters.AddWithValue("@guid", (object?)category.Guid ?? DBNull.Value);
                command.Parameters.AddWithValue("@name", (object?)category.Name ?? DBNull.Value);
                command.Parameters.AddWithValue("@currentLevel", category.CurrentLevel);
                command.Parameters.AddWithValue("@parentId", category.ParentId.HasValue && category.ParentId.Value > 0 ? category.ParentId.Value : DBNull.Value);
                command.Parameters.AddWithValue("@image", (object?)category.Image ?? DBNull.Value);
                command.Parameters.AddWithValue("@pathIds", (object?)category.PathIds ?? DBNull.Value);
                command.Parameters.AddWithValue("@pathNames", (object?)category.PathNames ?? DBNull.Value);
                command.Parameters.AddWithValue("@metaTitles", (object?)category.MetaTitles ?? DBNull.Value);
                command.Parameters.AddWithValue("@metaDescription", (object?)category.MetaDescription ?? DBNull.Value);
                command.Parameters.AddWithValue("@metaKeywords", (object?)category.MetaKeywords ?? DBNull.Value);
                command.Parameters.AddWithValue("@status", (object?)category.Status ?? DBNull.Value);
                command.Parameters.AddWithValue("@color", (object?)category.Color ?? DBNull.Value);
                command.Parameters.AddWithValue("@title", (object?)category.Title ?? DBNull.Value);
                command.Parameters.AddWithValue("@subTitle", (object?)category.SubTitle ?? DBNull.Value);
                command.Parameters.AddWithValue("@description", (object?)category.Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@modifiedBy", (object?)category.ModifiedBy ?? DBNull.Value);
                command.Parameters.AddWithValue("@modifiedAt", category.ModifiedAt ?? DateTime.Now);

                var affected = await command.ExecuteNonQueryAsync();
                if (affected <= 0)
                {
                    return new BaseResponse<long> { code = 204, message = "Record does not Exist.", data = 0 };
                }

                var updatedId = category.Id;
                if (updatedId <= 0)
                {
                    var lookupSql = "SELECT `Id` FROM categorylibrary WHERE `Guid` = @guid LIMIT 1;";
                    await using var lookup = new MySqlCommand(lookupSql, connection);
                    lookup.Parameters.AddWithValue("@guid", (object?)category.Guid ?? DBNull.Value);
                    updatedId = Convert.ToInt32(await lookup.ExecuteScalarAsync() ?? 0);
                }

                return new BaseResponse<long> { code = 200, message = "Record updated successfully.", data = updatedId };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 500, message = ex.Message, data = 0 };
            }
        }

        private async Task<BaseResponse<long>> DeleteWithoutStoredProcedure(CategoryLibrary category)
        {
            try
            {
                await using var connection = new MySqlConnection(_configuration.GetConnectionString("DBconnection"));
                await connection.OpenAsync();

                const string sql = @"
UPDATE categorylibrary
SET `IsDeleted` = 1,
    `DeletedBy` = @deletedBy,
    `DeletedAt` = @deletedAt
WHERE `Id` = @id AND `IsDeleted` = 0;";

                await using var command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", category.Id);
                command.Parameters.AddWithValue("@deletedBy", (object?)category.DeletedBy ?? DBNull.Value);
                command.Parameters.AddWithValue("@deletedAt", category.DeletedAt ?? DateTime.Now);

                var affected = await command.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = category.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 500, message = ex.Message, data = 0 };
            }
        }

        private async Task<BaseResponse<List<CategoryLibrary>>> GetWithoutStoredProcedure(CategoryLibrary category, bool getParent, bool getChild, int pageIndex, int pageSize)
        {
            try
            {
                await using var connection = new MySqlConnection(_configuration.GetConnectionString("DBconnection"));
                await connection.OpenAsync();
                await using var command = new MySqlCommand { Connection = connection };

                var filters = new List<string>();
                if (category.Id > 0)
                {
                    filters.Add("c.`Id` = @id");
                    command.Parameters.AddWithValue("@id", category.Id);
                }
                if (!string.IsNullOrWhiteSpace(category.Guid))
                {
                    filters.Add("c.`Guid` = @guid");
                    command.Parameters.AddWithValue("@guid", category.Guid);
                }
                if (!string.IsNullOrWhiteSpace(category.Name))
                {
                    filters.Add("c.`Name` LIKE @name");
                    command.Parameters.AddWithValue("@name", $"%{category.Name}%");
                }
                if (!string.IsNullOrWhiteSpace(category.PathIds))
                {
                    filters.Add("c.`PathIds` LIKE @pathIds");
                    command.Parameters.AddWithValue("@pathIds", $"%{category.PathIds}%");
                }
                if (!string.IsNullOrWhiteSpace(category.Status))
                {
                    filters.Add("c.`Status` = @status");
                    command.Parameters.AddWithValue("@status", category.Status);
                }
                if (category.ParentId.HasValue && category.ParentId.Value > 0)
                {
                    filters.Add("c.`ParentId` = @parentId");
                    command.Parameters.AddWithValue("@parentId", category.ParentId.Value);
                }
                if (!string.IsNullOrWhiteSpace(category.ParentName))
                {
                    filters.Add("p.`Name` LIKE @parentName");
                    command.Parameters.AddWithValue("@parentName", $"%{category.ParentName}%");
                }
                if (!string.IsNullOrWhiteSpace(category.Searchtext))
                {
                    filters.Add("(c.`Name` LIKE @search OR p.`Name` LIKE @search)");
                    command.Parameters.AddWithValue("@search", $"%{category.Searchtext}%");
                }

                filters.Add(category.IsDeleted.GetValueOrDefault(false) ? "c.`IsDeleted` = 1" : "c.`IsDeleted` = 0");

                if (getParent)
                {
                    filters.Add("c.`ParentId` IS NULL");
                }
                if (getChild)
                {
                    filters.Add("c.`ParentId` IS NOT NULL");
                }

                var whereClause = filters.Count > 0 ? $"WHERE {string.Join(" AND ", filters)}" : string.Empty;
                command.CommandText = $@"
SELECT COUNT(1)
FROM categorylibrary c
LEFT JOIN categorylibrary p ON p.`Id` = c.`ParentId`
{whereClause};";

                var total = Convert.ToInt32(await command.ExecuteScalarAsync() ?? 0);
                var safePageIndex = pageIndex <= 0 ? 1 : pageIndex;
                var safePageSize = pageSize <= 0 ? (total > 0 ? total : 10) : pageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);
                var offset = safePageSize > 0 ? (safePageIndex - 1) * safePageSize : 0;

                var items = new List<CategoryLibrary>();
                if (total > 0)
                {
                    var limitClause = (pageIndex == 0 || pageSize == 0)
                        ? string.Empty
                        : "LIMIT @offset, @pageSize";

                    command.CommandText = $@"
SELECT
  c.`Id`, c.`Guid`, c.`Name`, c.`CurrentLevel`, c.`ParentId`, c.`Image`, c.`PathIds`, c.`PathNames`,
  c.`MetaTitles`, c.`MetaDescription`, c.`MetaKeywords`, c.`Status`, c.`Color`, c.`Title`, c.`SubTitle`, c.`Description`,
  c.`CreatedBy`, c.`CreatedAt`, c.`ModifiedBy`, c.`ModifiedAt`, c.`DeletedBy`, c.`DeletedAt`, c.`IsDeleted`,
  p.`Name` AS `ParentName`, p.`PathIds` AS `ParentPathIds`, p.`PathNames` AS `ParentPathNames`
FROM categorylibrary c
LEFT JOIN categorylibrary p ON p.`Id` = c.`ParentId`
{whereClause}
ORDER BY c.`Id` DESC
{limitClause};";

                    if (!string.IsNullOrEmpty(limitClause))
                    {
                        command.Parameters.AddWithValue("@offset", offset);
                        command.Parameters.AddWithValue("@pageSize", safePageSize);
                    }

                    await using var reader = await command.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        items.Add(new CategoryLibrary
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Guid = reader.IsDBNull(reader.GetOrdinal("Guid")) ? null : reader.GetString(reader.GetOrdinal("Guid")),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                            CurrentLevel = reader.IsDBNull(reader.GetOrdinal("CurrentLevel")) ? 0 : reader.GetInt32(reader.GetOrdinal("CurrentLevel")),
                            ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId")) ? null : reader.GetInt32(reader.GetOrdinal("ParentId")),
                            Image = reader.IsDBNull(reader.GetOrdinal("Image")) ? null : reader.GetString(reader.GetOrdinal("Image")),
                            PathIds = reader.IsDBNull(reader.GetOrdinal("PathIds")) ? null : reader.GetString(reader.GetOrdinal("PathIds")),
                            PathNames = reader.IsDBNull(reader.GetOrdinal("PathNames")) ? null : reader.GetString(reader.GetOrdinal("PathNames")),
                            MetaTitles = reader.IsDBNull(reader.GetOrdinal("MetaTitles")) ? null : reader.GetString(reader.GetOrdinal("MetaTitles")),
                            MetaDescription = reader.IsDBNull(reader.GetOrdinal("MetaDescription")) ? null : reader.GetString(reader.GetOrdinal("MetaDescription")),
                            MetaKeywords = reader.IsDBNull(reader.GetOrdinal("MetaKeywords")) ? null : reader.GetString(reader.GetOrdinal("MetaKeywords")),
                            Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                            Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? null : reader.GetString(reader.GetOrdinal("Color")),
                            Title = reader.IsDBNull(reader.GetOrdinal("Title")) ? null : reader.GetString(reader.GetOrdinal("Title")),
                            SubTitle = reader.IsDBNull(reader.GetOrdinal("SubTitle")) ? null : reader.GetString(reader.GetOrdinal("SubTitle")),
                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            DeletedBy = reader.IsDBNull(reader.GetOrdinal("DeletedBy")) ? null : reader.GetString(reader.GetOrdinal("DeletedBy")),
                            DeletedAt = reader.IsDBNull(reader.GetOrdinal("DeletedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("DeletedAt")),
                            IsDeleted = !reader.IsDBNull(reader.GetOrdinal("IsDeleted")) && reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                            ParentName = reader.IsDBNull(reader.GetOrdinal("ParentName")) ? null : reader.GetString(reader.GetOrdinal("ParentName")),
                            ParentPathIds = reader.IsDBNull(reader.GetOrdinal("ParentPathIds")) ? null : reader.GetString(reader.GetOrdinal("ParentPathIds")),
                            ParentPathNames = reader.IsDBNull(reader.GetOrdinal("ParentPathNames")) ? null : reader.GetString(reader.GetOrdinal("ParentPathNames"))
                        });
                    }
                }

                return new BaseResponse<List<CategoryLibrary>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<CategoryLibrary>> { code = 500, message = ex.Message, data = new List<CategoryLibrary>() };
            }
        }

        private async Task<BaseResponse<List<CategoryLibrary>>> GetCategoryWithParentWithoutStoredProcedure(int categoryId)
        {
            var baseCategory = new CategoryLibrary { Id = categoryId, IsDeleted = false };
            return await GetWithoutStoredProcedure(baseCategory, false, false, 0, 0);
        }
    }
}
