using Log.Application.IRepositories;
using Log.Domain;
using Log.Domain.Entity;
using Log.Infrastructure.Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Log.Infrastructure.Repository
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        public ActivityLogRepository(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public async Task<BaseResponse<long>> Create(ActivityLog activityLog)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                    new SqlParameter("@mode", "add"),

                    new SqlParameter("@userid", activityLog.UserId),
                    new SqlParameter("@usertype", activityLog.UserType),
                    new SqlParameter("@url", activityLog.URL),
                    new SqlParameter("@action", activityLog.Action),
                    new SqlParameter("@logtitle", activityLog.LogTitle),
                    new SqlParameter("@logDescription", activityLog.LogDescription),


                    new SqlParameter("@createdat", activityLog.CreatedAt),
            };

                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;

                SqlParameter newid = new SqlParameter();
                newid.ParameterName = "@newid";
                newid.Direction = ParameterDirection.Output;
                newid.SqlDbType = SqlDbType.BigInt;

                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("DBconnection"), Procedures.ActivityLog, output, newid, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<BaseResponse<List<ActivityLog>>> get(ActivityLog activityLog, int PageIndex, int PageSize, string Mode)
        {
            try
            {
                var sqlParams = new List<SqlParameter>() {
                new SqlParameter("@mode", Mode),

                new SqlParameter("@userid ", activityLog.UserId),
                new SqlParameter("@usertype ", activityLog.UserType),
                new SqlParameter("@action ", activityLog.Action),
                new SqlParameter("@searchtext", activityLog.Searchtext),

                new SqlParameter("@pageIndex", PageIndex),
                new SqlParameter("@PageSize", PageSize),
            };
                SqlParameter output = new SqlParameter();
                output.ParameterName = "@output";
                output.Direction = ParameterDirection.Output;
                output.SqlDbType = SqlDbType.Int;



                SqlParameter message = new SqlParameter();
                message.ParameterName = "@message";
                message.Direction = ParameterDirection.Output;
                message.SqlDbType = SqlDbType.NVarChar;
                message.Size = 50;

                return await _dataProviderHelper.ExecuteReaderAsync(_configuration.GetConnectionString("DBconnection"), Procedures.GetActivityLog, ActivityLogParserAsync, output, newid: null, message, sqlParams.ToArray());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<List<ActivityLog>> ActivityLogParserAsync(DbDataReader reader)
        {
            List<ActivityLog> lstactivitylog = new List<ActivityLog>();
            while (await reader.ReadAsync())
            {
                lstactivitylog.Add(new ActivityLog()
                {
                    RowNumber = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RowNumber"))),
                    PageCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("PageCount"))),
                    RecordCount = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("RecordCount"))),
                    Id = Convert.ToInt32(reader.GetValue(reader.GetOrdinal("Id"))),

                    UserId = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserId"))),
                    UserType = Convert.ToString(reader.GetValue(reader.GetOrdinal("UserType"))),
                    URL = Convert.ToString(reader.GetValue(reader.GetOrdinal("URL"))),
                    Action = Convert.ToString(reader.GetValue(reader.GetOrdinal("Action"))),
                    LogTitle = Convert.ToString(reader.GetValue(reader.GetOrdinal("LogTitle"))),
                    LogDescription = Convert.ToString(reader.GetValue(reader.GetOrdinal("LogDescription"))),

                    CreatedAt = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal("CreatedAt")))) ? null : Convert.ToDateTime(reader.GetValue(reader.GetOrdinal("CreatedAt"))),


                });
            }
            return lstactivitylog;
        }


    }
}
