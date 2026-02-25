using API_Gateway.Helper;
using API_Gateway.Models;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Catalogue
{

    [Route("api/[controller]")]
    [ApiController]
    public class SpecificationController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        Response<SpecificationLibrary> res = new Response<SpecificationLibrary>();
        BaseResponse<SpecificationLibrary> baseResponse = new BaseResponse<SpecificationLibrary>();
        private ApiHelper helper;
        public SpecificationController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        #region Parent Specification

        [HttpPost("save")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> SaveParent(SpecificationLibrary model)
        {

            var temp = helper.ApiCall(URL, EndPoints.Specification + "?Name=" + model.Name.Replace("'", "''") + "&Getparent=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<SpecificationLibrary> tempList = baseResponse.Data as List<SpecificationLibrary> ?? new List<SpecificationLibrary>();

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                SpecificationLibrary spec = new SpecificationLibrary();
                spec.Name = model.Name;
                spec.ParentId = null;
                spec.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                var response = helper.ApiCall(URL, EndPoints.Specification, "POST", spec);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                int i = Convert.ToInt32(baseResponse.Data);
                UpdatePathId(i);
            }

            return Ok(baseResponse);
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> UpdateParent(SpecificationLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.Specification + "?Name=" + model.Name.Replace("'", "''") + "&Getparent=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<SpecificationLibrary> tempList = baseResponse.Data as List<SpecificationLibrary> ?? new List<SpecificationLibrary>();

            if (tempList.Where(x => x.ID != model.ID).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(URL, EndPoints.Specification + "?Id=" + model.ID, "GET", null);
                var tempResponse = baseResponse.JsonParseRecord(response);

                SpecificationLibrary spec = tempResponse.Data as SpecificationLibrary;
                spec.Name = model.Name;
                spec.PathName = model.Name;
                spec.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                response = helper.ApiCall(URL, EndPoints.Specification, "PUT", spec);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                UpdateSubPath(model.ID, true);
            }

            return Ok(baseResponse);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> DeleteParent(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.Specification + "?ParentID=" + id + "&IsChildParent=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<SpecificationLibrary> tempList = baseResponse.Data as List<SpecificationLibrary> ?? new List<SpecificationLibrary>();
            if (tempList.Any())
            {
                baseResponse = baseResponse.ChildExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.Specification + "?Id=" + id + "&Getparent=" + true, "GET", null);
                var tempResponse = baseResponse.JsonParseRecord(recordCall);
                if (tempResponse.Data != null)
                {
                    var response = helper.ApiCall(URL, EndPoints.Specification + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
                else
                {
                    baseResponse = baseResponse.NotExist();
                }
            }

            return Ok(baseResponse);
        }

        [HttpGet("get")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetParent(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.Specification + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Getparent=" + true, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext=null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(URL, EndPoints.Specification + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Getparent=" + true + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        #endregion

        [NonAction]
        public ActionResult<Response<SpecificationLibrary>> UpdatePathId(int id, bool isChildParent = false)
        {
            var response = helper.ApiCall(URL, EndPoints.Specification + "?ID=" + id + "&IsChildParent=" + isChildParent, "GET", null);
            var tmp = new SpecificationLibrary();
            if (response.IsSuccessStatusCode)
            {
                res = JsonParse(response);
                tmp = res.data.FirstOrDefault();
                if (tmp.ParentId == null)
                {
                    tmp.PathIds = tmp.ID.ToString();
                    tmp.PathName = tmp.Name;
                }
                else
                {
                    tmp.PathIds = tmp.ParentPathIds + ">" + tmp.ID;
                    tmp.PathName = tmp.ParentPathNames + ">" + tmp.Name;
                }

                var update = helper.ApiCall(URL, EndPoints.Specification, "PUT", tmp);
                if (update.IsSuccessStatusCode)
                {
                    return Ok(update.Content.ReadAsStringAsync().Result);
                }
            }
            return Ok(res);
        }

        [NonAction]
        public ActionResult<Response<SpecificationLibrary>> UpdateSubPath(int id, bool isChildParent = false)
        {
            var response = helper.ApiCall(URL, EndPoints.Specification + "?ParentID=" + id + "&IsChildParent=" + isChildParent, "GET", null);

            if (response.IsSuccessStatusCode)
            {
                res = JsonParse(response);
                List<SpecificationLibrary> spec = new List<SpecificationLibrary>();
                spec = res.data;
                foreach (var item in spec)
                {
                    UpdatePathId(item.ID, isChildParent);
                    UpdateSubPath(item.ID);
                }
            }
            return null;
        }

        [NonAction]
        public Response<SpecificationLibrary> JsonParse(HttpResponseMessage message)
        {
            return JsonConvert.DeserializeObject<Response<SpecificationLibrary>>(message.Content.ReadAsStringAsync().Result);
        }

    }
}
