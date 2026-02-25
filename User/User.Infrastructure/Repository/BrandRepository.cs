using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Domain;
using User.Domain.Entity;

namespace User.Infrastructure.Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly string _connectionString;

        public BrandRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(Brand brand)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO Brand (GUID, Name, Description, Status, Logo, CreatedBy, CreatedAt, IsDeleted)
VALUES (UUID(), @name, @description, @status, @logo, @createdBy, @createdAt, 0);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", brand.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@description", (object?)brand.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", brand.Status ?? "Active");
                cmd.Parameters.AddWithValue("@logo", brand.Logo ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdBy", brand.CreatedBy ?? string.Empty);
                cmd.Parameters.AddWithValue("@createdAt", brand.CreatedAt == default ? DateTime.Now : brand.CreatedAt);

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

        public async Task<BaseResponse<long>> Update(Brand brand)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE Brand
SET Name = @name,
    Description = @description,
    Status = @status,
    Logo = @logo,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt,
    IsDeleted = @isDeleted,
    DeletedBy = @deletedBy,
    DeletedAt = @deletedAt
WHERE ID = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", brand.ID);
                cmd.Parameters.AddWithValue("@name", brand.Name ?? string.Empty);
                cmd.Parameters.AddWithValue("@description", (object?)brand.Description ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", brand.Status ?? "Active");
                cmd.Parameters.AddWithValue("@logo", brand.Logo ?? string.Empty);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)brand.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", (object?)brand.ModifiedAt ?? DateTime.Now);
                cmd.Parameters.AddWithValue("@isDeleted", brand.IsDeleted);
                cmd.Parameters.AddWithValue("@deletedBy", (object?)brand.DeletedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deletedAt", (object?)brand.DeletedAt ?? DBNull.Value);

                var affected = await cmd.ExecuteNonQueryAsync();
                if (affected == 0)
                {
                    return new BaseResponse<long>
                    {
                        code = 204,
                        message = "Record does not Exist.",
                        data = 0
                    };
                }

                return new BaseResponse<long>
                {
                    code = 200,
                    message = "Record updated successfully.",
                    data = brand.ID
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

        public async Task<BaseResponse<long>> Delete(Brand brand)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
UPDATE Brand
SET IsDeleted = 1,
    DeletedBy = @deletedBy,
    DeletedAt = @deletedAt
WHERE ID = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", brand.ID);
                cmd.Parameters.AddWithValue("@deletedBy", (object?)brand.DeletedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@deletedAt", (object?)brand.DeletedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                if (affected == 0)
                {
                    return new BaseResponse<long>
                    {
                        code = 204,
                        message = "Record does not Exist.",
                        data = 0
                    };
                }

                return new BaseResponse<long>
                {
                    code = 200,
                    message = "Record deleted successfully.",
                    data = brand.ID
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

        public async Task<BaseResponse<List<Brand>>> Get(Brand brand, int pageIndex, int pageSize, string mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                var where = new List<string>();
                await using var cmd = new MySqlCommand();
                cmd.Connection = con;

                if (brand.ID > 0)
                {
                    where.Add("ID = @id");
                    cmd.Parameters.AddWithValue("@id", brand.ID);
                }

                if (!string.IsNullOrWhiteSpace(brand.BrandIds))
                {
                    where.Add("FIND_IN_SET(CAST(ID AS CHAR), @ids)");
                    cmd.Parameters.AddWithValue("@ids", brand.BrandIds);
                }

                if (!string.IsNullOrWhiteSpace(brand.Name))
                {
                    where.Add("Name = @name");
                    cmd.Parameters.AddWithValue("@name", brand.Name);
                }

                if (!string.IsNullOrWhiteSpace(brand.Status))
                {
                    where.Add("Status = @status");
                    cmd.Parameters.AddWithValue("@status", brand.Status);
                }

                if (!string.IsNullOrWhiteSpace(brand.searchText))
                {
                    where.Add("Name LIKE @search");
                    cmd.Parameters.AddWithValue("@search", $"%{brand.searchText}%");
                }

                where.Add("IsDeleted = @isDeleted");
                cmd.Parameters.AddWithValue("@isDeleted", brand.IsDeleted);

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $"SELECT COUNT(1) FROM Brand{whereClause};";
                var totalObj = await cmd.ExecuteScalarAsync();
                var total = Convert.ToInt32(totalObj ?? 0);

                var items = new List<Brand>();
                if (total > 0)
                {
                    var fetchAll = pageIndex <= 0 || pageSize <= 0;
                    var safePageIndex = pageIndex <= 0 ? 1 : pageIndex;
                    var safePageSize = pageSize <= 0 ? total : pageSize;
                    var offset = (safePageIndex - 1) * safePageSize;

                    cmd.CommandText = $@"
SELECT ID, GUID, Name, Description, Status, Logo, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt, DeletedBy, DeletedAt, IsDeleted
FROM Brand
{whereClause}
ORDER BY ID DESC
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
                        items.Add(new Brand
                        {
                            RowNumber = rowNo,
                            PageCount = pageCount,
                            RecordCount = total,
                            ID = reader.GetInt32("ID"),
                            GUID = reader.IsDBNull("GUID") ? null : reader.GetString("GUID"),
                            Name = reader.IsDBNull("Name") ? string.Empty : reader.GetString("Name"),
                            Description = reader.IsDBNull("Description") ? null : reader.GetString("Description"),
                            Status = reader.IsDBNull("Status") ? string.Empty : reader.GetString("Status"),
                            Logo = reader.IsDBNull("Logo") ? string.Empty : reader.GetString("Logo"),
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

                return new BaseResponse<List<Brand>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Brand>>
                {
                    code = 400,
                    message = ex.Message,
                    data = new List<Brand>()
                };
            }
        }
    }
}

