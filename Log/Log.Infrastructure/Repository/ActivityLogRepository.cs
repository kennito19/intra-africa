using Log.Application.IRepositories;
using Log.Domain;
using Log.Domain.Entity;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Log.Infrastructure.Repository
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly string _connectionString;

        public ActivityLogRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(ActivityLog activityLog)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO ActivityLog (UserId, UserType, URL, Action, LogTitle, LogDescription, CreatedAt)
VALUES (@userId, @userType, @url, @action, @logTitle, @logDescription, @createdAt);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@userId", (object?)activityLog.UserId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@userType", (object?)activityLog.UserType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@url", (object?)activityLog.URL ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@action", (object?)activityLog.Action ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@logTitle", (object?)activityLog.LogTitle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@logDescription", (object?)activityLog.LogDescription ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@createdAt", activityLog.CreatedAt ?? DateTime.Now);

                var newIdObj = await cmd.ExecuteScalarAsync();
                var newId = Convert.ToInt64(newIdObj ?? 0);

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

        public async Task<BaseResponse<List<ActivityLog>>> get(ActivityLog activityLog, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (!string.IsNullOrWhiteSpace(activityLog.UserId))
                {
                    where.Add("UserId = @userId");
                    cmd.Parameters.AddWithValue("@userId", activityLog.UserId);
                }
                if (!string.IsNullOrWhiteSpace(activityLog.UserType))
                {
                    where.Add("UserType = @userType");
                    cmd.Parameters.AddWithValue("@userType", activityLog.UserType);
                }
                if (!string.IsNullOrWhiteSpace(activityLog.Action))
                {
                    where.Add("Action = @action");
                    cmd.Parameters.AddWithValue("@action", activityLog.Action);
                }
                if (!string.IsNullOrWhiteSpace(activityLog.Searchtext))
                {
                    where.Add("(LogTitle LIKE @search OR LogDescription LIKE @search OR URL LIKE @search OR Action LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{activityLog.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $"SELECT COUNT(1) FROM ActivityLog{whereClause};";
                var totalObj = await cmd.ExecuteScalarAsync();
                var total = Convert.ToInt32(totalObj ?? 0);

                var safePageIndex = PageIndex < 1 ? 1 : PageIndex;
                var safePageSize = PageSize < 1 ? 10 : PageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);
                var offset = (safePageIndex - 1) * safePageSize;

                var items = new List<ActivityLog>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT Id, UserId, UserType, URL, Action, LogTitle, LogDescription, CreatedAt
FROM ActivityLog
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
                        items.Add(new ActivityLog
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? null : reader.GetInt32(reader.GetOrdinal("Id")),
                            UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? null : reader.GetString(reader.GetOrdinal("UserId")),
                            UserType = reader.IsDBNull(reader.GetOrdinal("UserType")) ? string.Empty : reader.GetString(reader.GetOrdinal("UserType")),
                            URL = reader.IsDBNull(reader.GetOrdinal("URL")) ? string.Empty : reader.GetString(reader.GetOrdinal("URL")),
                            Action = reader.IsDBNull(reader.GetOrdinal("Action")) ? string.Empty : reader.GetString(reader.GetOrdinal("Action")),
                            LogTitle = reader.IsDBNull(reader.GetOrdinal("LogTitle")) ? string.Empty : reader.GetString(reader.GetOrdinal("LogTitle")),
                            LogDescription = reader.IsDBNull(reader.GetOrdinal("LogDescription")) ? string.Empty : reader.GetString(reader.GetOrdinal("LogDescription")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                        });
                    }
                }

                return new BaseResponse<List<ActivityLog>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ActivityLog>>
                {
                    code = 400,
                    message = ex.Message,
                    data = new List<ActivityLog>()
                };
            }
        }
    }
}
