using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.UserLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public static string IDServerUrl = string.Empty;
        BaseResponse<NotificationLibrary> baseResponse = new BaseResponse<NotificationLibrary>();
        private ApiHelper helper;
        public NotificationController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserLog").Value;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Seller,Customer")]
        public ActionResult<ApiHelper> Create(NotificationLibrary model)
        {
            if (!string.IsNullOrEmpty(model.ReceiverId))
            {
                NotificationLibrary notification = new NotificationLibrary();
                notification.SenderId = model.SenderId;
                notification.ReceiverId = model.ReceiverId;
                notification.UserType = model.UserType;
                notification.NotificationTitle = model.NotificationTitle;
                notification.NotificationDescription = model.NotificationDescription;
                notification.Url = model.Url;
                notification.ImageUrl = model.ImageUrl;
                notification.NotificationsOf = model.NotificationsOf;
                notification.IsRead = model.IsRead;
                notification.CreatedAt = DateTime.Now;
                var response = helper.ApiCall(URL, EndPoints.ManageNotification, "POST", notification);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            var Adminresponse = helper.ApiCall(IDServerUrl, EndPoints.AdminList + "?pageIndex=0&pageSize=0", "GET", null);
            BaseResponse<AdminListModel> AdminbaseResponse = new BaseResponse<AdminListModel>();
            List<AdminListModel> lstAdmin = new List<AdminListModel>();
            if (Adminresponse.IsSuccessStatusCode)
            {
                BaseResponse<NotificationLibrary> _baseResponse = new BaseResponse<NotificationLibrary>();
                AdminbaseResponse = AdminbaseResponse.JsonParseList(Adminresponse);
                lstAdmin = (List<AdminListModel>)AdminbaseResponse.Data;
                var lstadminData = lstAdmin.Where(p => p.ReceiveNotifications.Contains(model.NotificationsOf)).ToList();
                if (lstadminData.Count > 0)
                {
                    foreach (var item in lstadminData)
                    {
                        NotificationLibrary _notification = new NotificationLibrary();
                        _notification.SenderId = model.SenderId;
                        _notification.ReceiverId = item.Id;
                        _notification.UserType = item.UserType;
                        _notification.NotificationTitle = model.NotificationTitle;
                        _notification.NotificationDescription = model.NotificationDescription;
                        _notification.Url = model.Url;
                        _notification.ImageUrl = model.ImageUrl;
                        _notification.NotificationsOf = model.NotificationsOf;
                        _notification.IsRead = model.IsRead;
                        _notification.CreatedAt = DateTime.Now;
                        var Iresponse = helper.ApiCall(URL, EndPoints.ManageNotification, "POST", _notification);
                        baseResponse = _baseResponse.JsonParseInputResponse(Iresponse);
                    }
                }
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> Update(updateNotification model)
        {
            NotificationLibrary notification = new NotificationLibrary();
            notification.Id = model.Id;
            var response = helper.ApiCall(URL, EndPoints.ManageNotification, "PUT", notification);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return Ok(baseResponse);
        }

        [HttpPut("markAllRead")]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> markAllRead(MarkAllRead model)
        {
            NotificationLibrary notification = new NotificationLibrary();
            notification.ReceiverId = model.ReceiverId;
            notification.NotificationsOf = model.NotificationsOf;
            var response = helper.ApiCall(URL, EndPoints.ManageNotification, "PUT", notification);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageNotification + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byReceiverId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByReceiverId(string receiverId, bool? IsRead = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string _url = string.Empty;
            if (IsRead != null)
            {
                _url += "&IsRead=" + IsRead;
            }
            var response = helper.ApiCall(URL, EndPoints.ManageNotification + "?ReceiverId=" + receiverId + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize + _url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? receiverId, string? searchtext, bool? IsRead = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }
            if (!string.IsNullOrEmpty(receiverId) && receiverId != "")
            {
                url = "&ReceiverId=" + HttpUtility.UrlEncode(receiverId);
            }
            if (IsRead!=null)
            {
                url += "&IsRead=" + IsRead;
            }

            var response = helper.ApiCall(URL, EndPoints.ManageNotification + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        public class updateNotification
        {
            public int Id { get; set; }
        }

        public class MarkAllRead
        {
            public string ReceiverId { get; set; }
            public string? NotificationsOf { get; set; }
        }

    }
}
