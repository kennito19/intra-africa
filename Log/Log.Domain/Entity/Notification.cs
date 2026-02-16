using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log.Domain.Entity
{
    public class Notification
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int UnreadCount { get; set; }
        public int? Id { get; set; }
        
        public string? SenderId { get; set; }
        public string? ReceiverId { get; set; }
        public string? UserType { get; set; }
        public string? NotificationTitle { get; set; }
        public string? NotificationDescription { get; set; }
        public string? Url { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsRead { get; set; } = null;
        public string? NotificationsOf { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string? Searchtext { get; set; }


    }
}
