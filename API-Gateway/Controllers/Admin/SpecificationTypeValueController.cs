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
    public class SpecificationTypeValueController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        private ApiHelper helper;
        Response<SpecificationLibrary> res = new Response<SpecificationLibrary>();
        BaseResponse<SpecificationLibrary> baseResponse = new BaseResponse<SpecificationLibrary>();
        public SpecificationTypeValueController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        #region Child Specification

        [HttpPost("save")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> SaveChild(SpecificationLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.Specification + "?Name=" + model.Name.Replace("'", "''") + "&ParentID=" + model.ParentId + "&Getchild=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<SpecificationLibrary> tempList = (List<SpecificationLibrary>)baseResponse.Data;
            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                SpecificationLibrary spec = new SpecificationLibrary();
                spec.Name = model.Name;
                spec.ParentId = model.ParentId;
                spec.FieldType = model.FieldType;
                spec.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                var response = helper.ApiCall(URL, EndPoints.Specification, "POST", spec);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                int i = (int)baseResponse.Data;
                if (i != null || i != 0)
                {
                    UpdatePathId(i);
                }
            }

            return Ok(baseResponse);
        }

        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> UpdateChild(SpecificationLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.Specification + "?Name=" + model.Name.Replace("'", "''") + "&ParentID=" + model.ParentId + "&Getchild=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<SpecificationLibrary> tempList = (List<SpecificationLibrary>)baseResponse.Data;
            if (tempList.Where(x => x.ID != model.ID && x.Name == model.Name).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(URL, EndPoints.Specification + "?Id=" + model.ID + "&Getchild=" + true, "GET", null);
                var tempResponse = baseResponse.JsonParseRecord(response);

                SpecificationLibrary spec = (SpecificationLibrary)tempResponse.Data;
                spec.Name = model.Name;
                spec.ParentId = model.ParentId;
                spec.FieldType = model.FieldType;
                spec.PathIds = model.PathIds;
                spec.PathName = model.PathName;
                spec.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                response = helper.ApiCall(URL, EndPoints.Specification, "PUT", spec);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                UpdatePathId(model.ID);
            }

            return Ok(baseResponse);
        }

        [HttpDelete("delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> DeleteChild(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.Specification + "?Id=" + id + "&Getchild=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<SpecificationLibrary> tempRecord = (List<SpecificationLibrary>)baseResponse.Data;
            if (tempRecord.Any())
            {
                var tempAssignSpecValue = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?AssignSpecId=" + id, "Get", null);
                BaseResponse<AssignSpecValuesToCategoryLibrary> baseAssignSpecValues = new BaseResponse<AssignSpecValuesToCategoryLibrary>();
                var ResSpecValuesToCat = baseAssignSpecValues.JsonParseList(tempAssignSpecValue);
                List<AssignSpecValuesToCategoryLibrary> SpecValuesToCatValues = (List<AssignSpecValuesToCategoryLibrary>)ResSpecValuesToCat.Data;

                if (SpecValuesToCatValues.Any())
                {
                    baseResponse = baseResponse.ChildAlreadyExists("Specification", "AssignSpecValuesToCategory");
                }
                else
                {
                    var response = helper.ApiCall(URL, EndPoints.Specification + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet("get")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetChild(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.Specification + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Getchild=" + true, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("getByParentId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByParentId(int parentId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.Specification + "?ParentID=" + parentId + "&Getchild=" + true + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, int? SpecificationTypeID = 0, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url += "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }
            if (SpecificationTypeID != 0 && SpecificationTypeID != null)
            {
                url += "&ParentID=" + SpecificationTypeID;
            }
            var response = helper.ApiCall(URL, EndPoints.Specification + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Getchild=" + true + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        #endregion

        [NonAction]
        public ActionResult<Response<SpecificationLibrary>> UpdatePathId(int id, bool IsChildParent = false)
        {
            var response = helper.ApiCall(URL, EndPoints.Specification + "?ID=" + id + "&IsChildParent=" + IsChildParent, "GET", null);
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
        public ActionResult<Response<SpecificationLibrary>> UpdateSubPath(int id, bool IsChildParent = false)
        {
            var response = helper.ApiCall(URL, EndPoints.Specification + "?ParentID=" + id + "&IsChildParent=" + IsChildParent, "GET", null);

            if (response.IsSuccessStatusCode)
            {
                res = JsonParse(response);
                List<SpecificationLibrary> spec = new List<SpecificationLibrary>();
                spec = res.data;
                foreach (var item in spec)
                {
                    UpdatePathId(item.ID, IsChildParent);
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

        public class SaveResponse
        {
            public int code { get; set; }
            public int data { get; set; }
            public string message { get; set; }
        }
    }
}
