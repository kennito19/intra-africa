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
    public class ManageStaticPagesController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ManageStaticPagesLibrary> baseResponse = new BaseResponse<ManageStaticPagesLibrary>();
        private ApiHelper helper;
        public ManageStaticPagesController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Save(ManageStaticPagesLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.Staticpages + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageStaticPagesLibrary> tempList = (List<ManageStaticPagesLibrary>)baseResponse.Data;
            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ManageStaticPagesLibrary pages = new ManageStaticPagesLibrary();
                pages.Name = model.Name;
                pages.Link = model.Link;
                pages.PageContent = model.PageContent;
                pages.Status = model.Status;
                pages.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.Staticpages, "POST", pages);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ManageStaticPagesLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.Staticpages + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageStaticPagesLibrary> tempList = (List<ManageStaticPagesLibrary>)baseResponse.Data;

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.Staticpages + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageStaticPagesLibrary record = (ManageStaticPagesLibrary)baseResponse.Data;
                record.Name = model.Name;
                record.Link = model.Link;
                record.PageContent = model.PageContent;
                record.Status = model.Status;
                record.ModifiedAt = DateTime.Now;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.Staticpages, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.Staticpages + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageStaticPagesLibrary> tempList = (List<ManageStaticPagesLibrary>)baseResponse.Data;

            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.Staticpages + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.Staticpages + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.Staticpages + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("search")]
        [Authorize]
        public ActionResult<ApiHelper> Search(string? searchtext, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }
            var response = helper.ApiCall(URL, EndPoints.Staticpages + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
