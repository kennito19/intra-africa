using Log.Application.IServices;
using Log.Application.Services;
using Log.Domain.Entity;
using Log.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Components.Web;

namespace Log.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(Notification notification)
        {
            notification.IsRead = false;
            notification.CreatedAt = DateTime.Now;
            notification.UpdatedAt = DateTime.Now;

            var data = await _notificationService.Create(notification);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(Notification notification)
        {
            
            var data = await _notificationService.Update(notification);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<Notification>>> get(int? Id = 0,string? SenderId = null, string? ReceiverId = null, bool? IsRead = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? NotificationsOf=null, string? Searchtext = null)
        {
            Notification notification = new Notification();
            if (Id != 0)
            {
                notification.Id = Id;
            }
            notification.SenderId = SenderId;
            notification.NotificationsOf = NotificationsOf;
            notification.ReceiverId = ReceiverId;
            if (IsRead != null)
            {
                notification.IsRead = Convert.ToBoolean(IsRead);
            }
            notification.Searchtext = HttpUtility.UrlDecode(Searchtext);

            var data = await _notificationService.get(notification, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
