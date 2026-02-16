using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Gateway.Controllers.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class UserKYCController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string CatelogueURL = string.Empty;
        private UserKYCDetails UserKYCdetails;
        BaseResponse<KYCDetails> baseResponse = new BaseResponse<KYCDetails>();

        public UserKYCController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            UserKYCdetails = new UserKYCDetails(_httpContext, URL, CatelogueURL, _configuration);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Customer")]
        public ActionResult<ApiHelper> Save(KYCDetails model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            bool AllowAadharcard = Convert.ToBoolean(_configuration.GetValue<string>("allow_aadharcard"));
            baseResponse = UserKYCdetails.SaveKYC(model, userID, AllowAadharcard);

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Customer")]
        public ActionResult<ApiHelper> Update(KYCDetails model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            bool AllowAadharcard = Convert.ToBoolean(_configuration.GetValue<string>("allow_aadharcard"));
            baseResponse = UserKYCdetails.UpdateKYC(model, userID, AllowAadharcard);

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Customer")]
        public ActionResult<ApiHelper> Delete(int? id = 0, string? userId = "")
        {
            baseResponse = UserKYCdetails.DeleteKYC(id, userId);
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Customer")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            baseResponse = UserKYCdetails.GetKyc(pageIndex, pageSize);
            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin,Customer")]
        public ActionResult<ApiHelper> ById(int id)
        {
            baseResponse = UserKYCdetails.GetKycById(id);
            return Ok(baseResponse);
        }

        [HttpGet("byUserId")]
        [Authorize(Roles = "Admin,Customer")]
        public ActionResult<ApiHelper> ByUserID(string userId)
        {
            baseResponse = UserKYCdetails.GetKycByUserID(userId);
            return Ok(baseResponse);
        }
    }
}
