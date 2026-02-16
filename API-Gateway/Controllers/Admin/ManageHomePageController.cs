using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageHomePageController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ManageHomePage> baseResponse = new BaseResponse<ManageHomePage>();
        private ApiHelper helper;
        public ManageHomePageController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(ManageHomePage model)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageHomePage + "?Status=Active", "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            ManageHomePage mhp = (ManageHomePage)baseResponse.Data;

            if (mhp != null)
            {
                mhp.Status = "Inactive";
                var resp = helper.ApiCall(URL, EndPoints.ManageHomePage, "PUT", mhp);
            }

            model.Status = "Active";
            model.CreatedAt = DateTime.Now;
            model.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            var res = helper.ApiCall(URL, EndPoints.ManageHomePage, "POST", model);
            baseResponse = baseResponse.JsonParseInputResponse(res);
            return Ok(baseResponse);

        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ManageHomePage model)
        {
            // here i have fetched the active records and made it inactive
            var response = helper.ApiCall(URL, EndPoints.ManageHomePage + "?Status=Active", "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            ManageHomePage mhp = (ManageHomePage)baseResponse.Data;
            mhp.Status = "Inactive";
            var res = helper.ApiCall(URL, EndPoints.ManageHomePage, "PUT", mhp);

            // here i am updating the inactive home page to active home page 
            var resp = helper.ApiCall(URL, EndPoints.ManageHomePage + "?Id=" + model.Id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            ManageHomePage hp = (ManageHomePage)baseResponse.Data;
            hp.Name = model.Name;
            hp.Status = "Active";
            hp.ModifiedAt = DateTime.Now;
            hp.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            var respo = helper.ApiCall(URL, EndPoints.ManageHomePage, "PUT", hp);
            baseResponse = baseResponse.JsonParseInputResponse(respo);

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int Id)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageHomePage + "?Id=" + Id, "DELETE", null);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> get(int? Id = null, string? Name = null, string? Status = null)
        {
            string url = string.Empty;
            if (Id != null)
            {
                url += "&Id=" + Id;
            }
            if (Name != null)
            {
                url += "&Name=" + Name;
            }
            if (Status != null)
            {
                url += "&Status=" + Status;
            }
            var response = helper.ApiCall(URL, EndPoints.ManageHomePage + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            return Ok(baseResponse);
        }
    }


}
