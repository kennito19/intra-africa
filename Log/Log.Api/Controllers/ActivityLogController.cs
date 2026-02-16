using Log.Application.IServices;
using Log.Domain;
using Log.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace Log.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityLogController : ControllerBase
    {
        private readonly IActivityLogService _activityLogService;

        public ActivityLogController(IActivityLogService activityLogService)
        {
            _activityLogService = activityLogService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(ActivityLog activityLog)
        {
            activityLog.CreatedAt = DateTime.Now;

            var data = await _activityLogService.Create(activityLog);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<ActivityLog>>> get(string? UserId = null, string? UserType = null, string? Action = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get", string? Searchtext = null)
        {
            ActivityLog activitylog = new ActivityLog();
            activitylog.UserId = UserId;
            activitylog.UserType = UserType;
            activitylog.Action = Action;

            activitylog.Searchtext = HttpUtility.UrlDecode(Searchtext);

            var data = await _activityLogService.get(activitylog, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
