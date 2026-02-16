using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Infrastructure.Helper
{
    public class Procedures
    {
        public static string ActivityLog { get; set; } = "sp_ActivityLog";
        public static string GetActivityLog { get; set; } = "sp_GetActivityLog";
        public static string Notification { get; set; } = "sp_Notification";
        public static string GetNotification { get; set; } = "sp_GetNotification";

    }
}
