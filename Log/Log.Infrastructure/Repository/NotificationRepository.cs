using Log.Application.IRepositories;
using Log.Domain;
using Log.Domain.Entity;
using Log.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using MySqlConnector;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace Log.Infrastructure.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();

        public NotificationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<BaseResponse<long>> Create(Notification notification)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    new MySqlParameter("@mode", "add"),

                    new MySqlParameter("@sender_id", notification.SenderId),
                    new MySqlParameter("@receiverId", notification.ReceiverId),
                    new MySqlParameter("@usertype", notification.UserType),
                    new MySqlParameter("@notification_title", notification.NotificationTitle),
                    new MySqlParameter("@notification_description", notification.NotificationDescription),
                    new MySqlParameter("@url", notification.Url),
                    new MySqlParameter("@imagr_url", notification.ImageUrl),
                    new MySqlParameter("@notificationsOf", notification.NotificationsOf),
                    new MySqlParameter("@is_read", notification.IsRead),
                    new MySqlParameter("@createdat", notification.CreatedAt),
                    new MySqlParameter("@updatedat", notification.UpdatedAt),

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

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Notification, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<BaseResponse<long>> Update(Notification notification)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                    //new MySqlParameter("@mode", "update"),
                    new MySqlParameter("@mode", "MarkAllRead"),
                    new MySqlParameter("@id", notification.Id),
                    new MySqlParameter("@receiverId", notification.ReceiverId),
                    new MySqlParameter("@notificationsOf", notification.NotificationsOf),

                    new MySqlParameter("@updatedat", DateTime.Now),
                };

                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;

                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.Notification, output, newid: null, message, sqlParams.ToArray());
 
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<BaseResponse<List<Notification>>> get(Notification notification, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<MySqlParameter>() {
                new MySqlParameter("@mode", Mode),


                new MySqlParameter("@id", notification.Id),
                new MySqlParameter("@sender_id", notification.SenderId),
                new MySqlParameter("@receiverId", notification.ReceiverId),
                new MySqlParameter("@is_read", notification.IsRead),
                new MySqlParameter("@notificationsOf", notification.NotificationsOf),
                new MySqlParameter("@searchtext", notification.Searchtext),

                new MySqlParameter("@pageIndex", PageIndex),
                new MySqlParameter("@PageSize", PageSize),
            };
                MySqlParameter output = new MySqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.MySqlDbType = MySqlDbType.Int32;



                MySqlParameter message = new MySqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.MySqlDbType = MySqlDbType.VarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetNotification, NotificationParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        private async Task<List<Notification>> NotificationParserAsync(DbDataReader reader)
        {
            List<Notification> lstnotification = new List<Notification>();

            while (await reader.ReadAsync())
            {
                lstnotification.Add(new Notification()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    UnreadCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("UnreadCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),
                    SenderId = Convert.ToString(reader.GetValue(reader.GetOrdinal("SenderId"))),
                    ReceiverId = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ReceiverId")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ReceiverId"))),
                    UserType = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserType"))),
                    NotificationTitle = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("NotificationTitle")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("NotificationTitle"))),
                    NotificationDescription = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("NotificationDescription")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("NotificationDescription"))),
                    Url = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("Url")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("Url"))),
                    ImageUrl = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("ImageUrl")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("ImageUrl"))),
                    NotificationsOf = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("NotificationsOf")))) ? null : Convert.ToString(reader.GetValue(reader.GetOrdinal("NotificationsOf"))),
                    IsRead = Convert.ToBoolean(reader.GetValue(reader.GetOrdinal("IsRead"))),
                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),
                    UpdatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("UpdatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("UpdatedAt"))),


                });
            }

            return lstnotification;
        }
    }
}
