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
    public class ManageHomePageRepository : IManageHomePageRepository
    {
        private readonly string _connectionString;

        public ManageHomePageRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        private async Task EnsureTableAsync(MySqlConnection con)
        {
            const string sql = @"
CREATE TABLE IF NOT EXISTS ManageHomePage (
    Id INT AUTO_INCREMENT NOT NULL,
    Name VARCHAR(250) NULL,
    Status VARCHAR(50) NULL,
    CreatedBy VARCHAR(500) NULL,
    CreatedAt DATETIME NULL,
    ModifiedBy VARCHAR(500) NULL,
    ModifiedAt DATETIME NULL,
    PRIMARY KEY (Id)
);";
            await using var cmd = new MySqlCommand(sql, con);
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<BaseResponse<long>> Create(ManageHomePage manageHomePage)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await EnsureTableAsync(con);

                const string sql = @"
INSERT INTO ManageHomePage (Name, Status, CreatedBy, CreatedAt)
VALUES (@name, @status, @createdBy, @createdAt);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@name", (object?)manageHomePage.Name ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", (object?)manageHomePage.Status ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdBy", (object?)manageHomePage.CreatedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdAt", manageHomePage.CreatedAt ?? DateTime.Now);

                var id = Convert.ToInt64(await cmd.ExecuteScalarAsync() ?? 0);
                return new BaseResponse<long> { code = 200, message = "Record added successfully.", data = id };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Update(ManageHomePage manageHomePage)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await EnsureTableAsync(con);

                const string sql = @"
UPDATE ManageHomePage
SET Name = @name,
    Status = @status,
    ModifiedBy = @modifiedBy,
    ModifiedAt = @modifiedAt
WHERE Id = @id;";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", manageHomePage.Id);
                cmd.Parameters.AddWithValue("@name", (object?)manageHomePage.Name ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@status", (object?)manageHomePage.Status ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedBy", (object?)manageHomePage.ModifiedBy ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modifiedAt", manageHomePage.ModifiedAt ?? DateTime.Now);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record updated successfully." : "Record does not Exist.",
                    data = manageHomePage.Id ?? 0
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<long>> Delete(ManageHomePage manageHomePage)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await EnsureTableAsync(con);

                const string sql = "DELETE FROM ManageHomePage WHERE Id = @id;";
                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@id", manageHomePage.Id);

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = affected > 0 ? 200 : 204,
                    message = affected > 0 ? "Record deleted successfully." : "Record does not Exist.",
                    data = manageHomePage.Id ?? 0
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<long> { code = 400, message = ex.Message, data = 0 };
            }
        }

        public async Task<BaseResponse<List<ManageHomePage>>> get(ManageHomePage manageHomePage, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await EnsureTableAsync(con);
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (manageHomePage.Id.HasValue && manageHomePage.Id.Value > 0)
                {
                    where.Add("Id = @id");
                    cmd.Parameters.AddWithValue("@id", manageHomePage.Id.Value);
                }
                if (!string.IsNullOrWhiteSpace(manageHomePage.Name))
                {
                    where.Add("Name LIKE @name");
                    cmd.Parameters.AddWithValue("@name", $"%{manageHomePage.Name}%");
                }
                if (!string.IsNullOrWhiteSpace(manageHomePage.Status))
                {
                    where.Add("Status = @status");
                    cmd.Parameters.AddWithValue("@status", manageHomePage.Status);
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $@"
SELECT Id, Name, Status, CreatedBy, CreatedAt, ModifiedBy, ModifiedAt
FROM ManageHomePage
{whereClause}
ORDER BY Id DESC;";

                var items = new List<ManageHomePage>();
                await using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    items.Add(new ManageHomePage
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                        Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                        CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetString(reader.GetOrdinal("CreatedBy")),
                        CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                        ModifiedBy = reader.IsDBNull(reader.GetOrdinal("ModifiedBy")) ? null : reader.GetString(reader.GetOrdinal("ModifiedBy")),
                        ModifiedAt = reader.IsDBNull(reader.GetOrdinal("ModifiedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("ModifiedAt"))
                    });
                }

                return new BaseResponse<List<ManageHomePage>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ManageHomePage>> { code = 400, message = ex.Message, data = new List<ManageHomePage>() };
            }
        }
    }
}
