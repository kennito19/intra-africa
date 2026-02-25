using Log.Application.IRepositories;
using Log.Domain;
using Log.Domain.Entity;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Log.Infrastructure.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly string _connectionString;

        public NotificationRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBconnection");
        }

        public async Task<BaseResponse<long>> Create(Notification notification)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                const string sql = @"
INSERT INTO Notification
(SenderId, ReceiverId, UserType, NotificationTitle, NotificationDescription, Url, ImageUrl, NotificationsOf, IsRead, CreatedAt, UpdatedAt)
VALUES
(@senderId, @receiverId, @userType, @notificationTitle, @notificationDescription, @url, @imageUrl, @notificationsOf, @isRead, @createdAt, @updatedAt);
SELECT LAST_INSERT_ID();";

                await using var cmd = new MySqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@senderId", (object?)notification.SenderId ?? string.Empty);
                cmd.Parameters.AddWithValue("@receiverId", (object?)notification.ReceiverId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@userType", (object?)notification.UserType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@notificationTitle", (object?)notification.NotificationTitle ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@notificationDescription", (object?)notification.NotificationDescription ?? string.Empty);
                cmd.Parameters.AddWithValue("@url", (object?)notification.Url ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@imageUrl", (object?)notification.ImageUrl ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@notificationsOf", (object?)notification.NotificationsOf ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isRead", notification.IsRead ?? false);
                cmd.Parameters.AddWithValue("@createdAt", notification.CreatedAt ?? DateTime.Now);
                cmd.Parameters.AddWithValue("@updatedAt", notification.UpdatedAt ?? DateTime.Now);

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

        public async Task<BaseResponse<long>> Update(Notification notification)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();

                await using var cmd = new MySqlCommand { Connection = con };

                if (notification.Id.HasValue && notification.Id.Value > 0)
                {
                    cmd.CommandText = @"
UPDATE Notification
SET IsRead = 1,
    UpdatedAt = @updatedAt
WHERE Id = @id;";
                    cmd.Parameters.AddWithValue("@id", notification.Id.Value);
                    cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now);
                }
                else
                {
                    cmd.CommandText = @"
UPDATE Notification
SET IsRead = 1,
    UpdatedAt = @updatedAt
WHERE (@receiverId IS NULL OR ReceiverId = @receiverId)
  AND (@notificationsOf IS NULL OR NotificationsOf = @notificationsOf)
  AND IFNULL(IsRead, 0) = 0;";
                    cmd.Parameters.AddWithValue("@receiverId", (object?)notification.ReceiverId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@notificationsOf", (object?)notification.NotificationsOf ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@updatedAt", DateTime.Now);
                }

                var affected = await cmd.ExecuteNonQueryAsync();
                return new BaseResponse<long>
                {
                    code = 200,
                    message = "Record updated successfully.",
                    data = affected
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

        public async Task<BaseResponse<List<Notification>>> get(Notification notification, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                await using var con = new MySqlConnection(_connectionString);
                await con.OpenAsync();
                await using var cmd = new MySqlCommand { Connection = con };

                var where = new List<string>();
                if (notification.Id.HasValue && notification.Id.Value > 0)
                {
                    where.Add("Id = @id");
                    cmd.Parameters.AddWithValue("@id", notification.Id.Value);
                }
                if (!string.IsNullOrWhiteSpace(notification.SenderId))
                {
                    where.Add("SenderId = @senderId");
                    cmd.Parameters.AddWithValue("@senderId", notification.SenderId);
                }
                if (!string.IsNullOrWhiteSpace(notification.ReceiverId))
                {
                    where.Add("ReceiverId = @receiverId");
                    cmd.Parameters.AddWithValue("@receiverId", notification.ReceiverId);
                }
                if (notification.IsRead.HasValue)
                {
                    where.Add("IFNULL(IsRead, 0) = @isRead");
                    cmd.Parameters.AddWithValue("@isRead", notification.IsRead.Value);
                }
                if (!string.IsNullOrWhiteSpace(notification.NotificationsOf))
                {
                    where.Add("NotificationsOf = @notificationsOf");
                    cmd.Parameters.AddWithValue("@notificationsOf", notification.NotificationsOf);
                }
                if (!string.IsNullOrWhiteSpace(notification.Searchtext))
                {
                    where.Add("(NotificationTitle LIKE @search OR NotificationDescription LIKE @search)");
                    cmd.Parameters.AddWithValue("@search", $"%{notification.Searchtext}%");
                }

                var whereClause = where.Count > 0 ? $" WHERE {string.Join(" AND ", where)}" : string.Empty;

                cmd.CommandText = $"SELECT COUNT(1) FROM Notification{whereClause};";
                var totalObj = await cmd.ExecuteScalarAsync();
                var total = Convert.ToInt32(totalObj ?? 0);

                // Keep unread count available for the UI in each row.
                cmd.CommandText = @"SELECT COUNT(1) FROM Notification
WHERE (@receiverForUnread IS NULL OR ReceiverId = @receiverForUnread)
  AND IFNULL(IsRead, 0) = 0;";
                cmd.Parameters.AddWithValue("@receiverForUnread", (object?)notification.ReceiverId ?? DBNull.Value);
                var unreadObj = await cmd.ExecuteScalarAsync();
                var unreadCount = Convert.ToInt32(unreadObj ?? 0);

                var safePageIndex = PageIndex < 1 ? 1 : PageIndex;
                var safePageSize = PageSize < 1 ? 10 : PageSize;
                var pageCount = total == 0 ? 0 : (int)Math.Ceiling(total / (double)safePageSize);
                var offset = (safePageIndex - 1) * safePageSize;

                var items = new List<Notification>();
                if (total > 0)
                {
                    cmd.CommandText = $@"
SELECT Id, SenderId, ReceiverId, UserType, NotificationTitle, NotificationDescription, Url, ImageUrl, IsRead, NotificationsOf, CreatedAt, UpdatedAt
FROM Notification
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
                        items.Add(new Notification
                        {
                            RowNumber = rowNumber,
                            PageCount = pageCount,
                            RecordCount = total,
                            UnreadCount = unreadCount,
                            Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? null : reader.GetInt32(reader.GetOrdinal("Id")),
                            SenderId = reader.IsDBNull(reader.GetOrdinal("SenderId")) ? null : reader.GetString(reader.GetOrdinal("SenderId")),
                            ReceiverId = reader.IsDBNull(reader.GetOrdinal("ReceiverId")) ? null : reader.GetString(reader.GetOrdinal("ReceiverId")),
                            UserType = reader.IsDBNull(reader.GetOrdinal("UserType")) ? null : reader.GetString(reader.GetOrdinal("UserType")),
                            NotificationTitle = reader.IsDBNull(reader.GetOrdinal("NotificationTitle")) ? null : reader.GetString(reader.GetOrdinal("NotificationTitle")),
                            NotificationDescription = reader.IsDBNull(reader.GetOrdinal("NotificationDescription")) ? null : reader.GetString(reader.GetOrdinal("NotificationDescription")),
                            Url = reader.IsDBNull(reader.GetOrdinal("Url")) ? null : reader.GetString(reader.GetOrdinal("Url")),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("ImageUrl")) ? null : reader.GetString(reader.GetOrdinal("ImageUrl")),
                            IsRead = reader.IsDBNull(reader.GetOrdinal("IsRead")) ? null : reader.GetBoolean(reader.GetOrdinal("IsRead")),
                            NotificationsOf = reader.IsDBNull(reader.GetOrdinal("NotificationsOf")) ? null : reader.GetString(reader.GetOrdinal("NotificationsOf")),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                        });
                    }
                }

                return new BaseResponse<List<Notification>>
                {
                    code = items.Count > 0 ? 200 : 204,
                    message = items.Count > 0 ? "Record bind successfully." : "Record does not Exist.",
                    data = items
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Notification>>
                {
                    code = 400,
                    message = ex.Message,
                    data = new List<Notification>()
                };
            }
        }
    }
}
