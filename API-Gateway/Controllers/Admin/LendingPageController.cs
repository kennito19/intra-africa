using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class LendingPageController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ManageLendingPageLibrary> baseResponse = new BaseResponse<ManageLendingPageLibrary>();
        private ApiHelper helper;
        public LendingPageController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(ManageLendingPageLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLendingPage + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageLendingPageLibrary> tempList = (List<ManageLendingPageLibrary>)baseResponse.Data;

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ManageLendingPageLibrary manageLendingPage = new ManageLendingPageLibrary();
                manageLendingPage.Name = model.Name;
                manageLendingPage.Link = model.Link;
                manageLendingPage.Sequence = model.Sequence;
                manageLendingPage.Status = model.Status;
                manageLendingPage.CreatedAt = DateTime.Now;
                manageLendingPage.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageLendingPage, "POST", manageLendingPage);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ManageLendingPageLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLendingPage + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageLendingPageLibrary> tempList = (List<ManageLendingPageLibrary>)baseResponse.Data;

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageLendingPage + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageLendingPageLibrary manageLendingPage = (ManageLendingPageLibrary)baseResponse.Data;
                manageLendingPage.Id = model.Id;
                manageLendingPage.Name = model.Name;
                manageLendingPage.Link = model.Link;
                manageLendingPage.Sequence = model.Sequence;
                manageLendingPage.Status = model.Status;
                manageLendingPage.ModifiedAt = DateTime.Now;
                manageLendingPage.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageLendingPage, "PUT", manageLendingPage);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLendingPage + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageLendingPageLibrary> tempList = (List<ManageLendingPageLibrary>)baseResponse.Data;

            if (tempList.Any())
            {
                var _tempList = helper.ApiCall(URL, EndPoints.ManageLendingPageSection + "?LendingPageId=" + id, "GET", null);
                BaseResponse<LendingPageSections> lendingbaseResponse = new BaseResponse<LendingPageSections>();
                lendingbaseResponse = lendingbaseResponse.JsonParseList(_tempList);
                List<LendingPageSections> _lendingtempList = (List<LendingPageSections>)lendingbaseResponse.Data;
                if (_lendingtempList.Any())
                {
                    baseResponse = baseResponse.ChildExists();
                }
                else
                {
                    var response = helper.ApiCall(URL, EndPoints.ManageLendingPage + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLendingPage + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLendingPage + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }
            var response = helper.ApiCall(URL, EndPoints.ManageLendingPage + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
