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
    public class ManageLayoutTypesDetailsRepository : IManageLayoutTypesDetailsRepository
    {
        private readonly string _connectionString;

        public ManageLayoutTypesDetailsRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ManageLayoutTypesDetails typesDetails)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO managelayouttypesdetails
(LayoutId, LayoutTypeId, Name, SectionType, InnerColumns, CreatedBy, CreatedAt)
VALUES
(@layoutId, @layoutTypeId, @name, @sectionType, @innerColumns, @createdBy, @createdAt);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@layoutId", typesDetails.LayoutId > 0 ? typesDetails.LayoutId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@layoutTypeId", typesDetails.LayoutTypeId > 0 ? typesDetails.LayoutTypeId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@name", (object?)typesDetails.Name ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sectionType", (object?)typesDetails.SectionType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@innerColumns", (object?)typesDetails.InnerColumns ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdBy", (object?)typesDetails.CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdAt", typesDetails.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(ManageLayoutTypesDetails typesDetails)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE managelayouttypesdetails
SET LayoutId = @layoutId,
    LayoutTypeId = @layoutTypeId,
    Name = @name,
    SectionType = @sectionType,
    InnerColumns = @innerColumns,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", typesDetails.Id);
                cmd.Parameters.AddWithValue("@layoutId", typesDetails.LayoutId > 0 ? typesDetails.LayoutId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@layoutTypeId", typesDetails.LayoutTypeId > 0 ? typesDetails.LayoutTypeId : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@name", (object?)typesDetails.Name ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sectionType", (object?)typesDetails.SectionType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@innerColumns", (object?)typesDetails.InnerColumns ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)typesDetails.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", typesDetails.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = typesDetails.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageLayoutTypesDetails typesDetails)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = "DELETE FROM managelayouttypesdetails WHERE Id = @id;";
                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", typesDetails.Id);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = typesDetails.Id
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ManageLayoutTypesDetails>>> get(ManageLayoutTypesDetails typesDetails, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (typesDetails.Id > 0)
                {
                    where.Add("d.Id = @id");
                    cmd.Parameters.AddWithValue("@id", typesDetails.Id);
                }
                if (typesDetails.LayoutId > 0)
                {
                    where.Add("d.LayoutId = @layoutId");
                    cmd.Parameters.AddWithValue("@layoutId", typesDetails.LayoutId);
                }
                if (typesDetails.LayoutTypeId > 0)
                {
                    where.Add("d.LayoutTypeId = @layoutTypeId");
                    cmd.Parameters.AddWithValue("@layoutTypeId", typesDetails.LayoutTypeId);
                }
                if (!string.IsNullOrWhiteSpace(typesDetails.Name))
                {
                    where.Add("d.Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{typesDetails.Name}%");
                }
                if (!string.IsNullOrWhiteSpace(typesDetails.Searchtext))
                {
                    where.Add("(d.Name LIKE @search OR l.Name LIKE @search OR t.Name LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{typesDetails.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT COUNT(1)
FROM managelayouttypesdetails d
LEFT JOIN managelayouts l ON l.Id = d.LayoutId
LEFT JOIN managelayouttypes t ON t.Id = d.LayoutTypeId
{whereClause};";
                var total = Convert.ToInt32(await cmd.ExecuteScalarAsync() ?? 0);

                var safePageIndex = PageIndex <= 0 ? 1 : PageIndex;
                var safePageSize = PageSize <= 0 ? 10 : PageSize;
                var offset = (safePageIndex - 1) * safePageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);

                var items = new List<ManageLayoutTypesDetails>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT
  d.Id, d.LayoutId, d.LayoutTypeId, d.Name, d.SectionType, d.InnerColumns,
  d.CreatedBy, d.CreatedAt, d.ModifiedBy, d.ModifiedAt,
  l.Name AS LayoutName,
  t.Name AS LayoutTypeName
FROM managelayouttypesdetails d
LEFT JOIN managelayouts l ON l.Id = d.LayoutId
LEFT JOIN managelayouttypes t ON t.Id = d.LayoutTypeId
{whereClause}
ORDER BY d.Id DESC
LIMIT @offset, @pageSize;";
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@pageSize", safePageSize);

                    await using var reader = await cmd.ExecuteReaderAsync();
                    var rowNumber = offset;
                    while (await reader.ReadAsync())
                    {
                        rowNumber++;
                        items.Add(new ManageLayoutTypesDetails
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            LayoutId = reader.IsDBNull(reader.GetOrdinal("LayoutId")) ? 0 : reader.GetInt32(reader.GetOrdinal("LayoutId")),
                            LayoutTypeId = reader.IsDBNull(reader.GetOrdinal("LayoutTypeId")) ? 0 : reader.GetInt32(reader.GetOrdinal("LayoutTypeId")),
                            Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                            SectionType = reader.IsDBNull(reader.GetOrdinal("SectionType")) ? null : reader.GetString(reader.GetOrdinal("SectionType")),
                            InnerColumns = reader.IsDBNull(reader.GetOrdinal("InnerColumns")) ? null : reader.GetString(reader.GetOrdinal("InnerColumns")),
                            CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                            ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt")),
                            LayoutName = reader.IsDBNull(reader.GetOrdinal("LayoutName")) ? null : reader.GetString(reader.GetOrdinal("LayoutName")),
                            LayoutTypeName = reader.IsDBNull(reader.GetOrdinal("LayoutTypeName")) ? null : reader.GetString(reader.GetOrdinal("LayoutTypeName"))
                        });
                    }
                }

                return new BaseResponse<List<ManageLayoutTypesDetails>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageLayoutTypesDetails>> { code = 400, message = ex.Message, data = new List<ManageLayoutTypesDetails>() };
            }
        }
    }
}
