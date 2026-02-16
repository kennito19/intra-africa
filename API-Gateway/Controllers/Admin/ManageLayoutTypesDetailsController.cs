using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageLayoutTypesDetailsController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        public string URL = string.Empty;
        BaseResponse<ManageLayoutTypesDetails> baseResponse = new BaseResponse<ManageLayoutTypesDetails>();
        private ApiHelper helper;
        public ManageLayoutTypesDetailsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(ManageLayoutTypesDetailsDTO model)
        {
            List<ManageLayoutTypesDetails> tempList = new List<ManageLayoutTypesDetails>();
            var temp = helper.ApiCall(URL, EndPoints.ManageLayoutTypesDetails + "?Name=" + model.Name + "&LayoutId=" + model.LayoutId + "&LayoutTypeId=" + model.LayoutTypeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            if (baseResponse.code == 200)
            {
                tempList = (List<ManageLayoutTypesDetails>)baseResponse.Data;
            }
            if (tempList.Any())
            {
                var data = tempList.Where(x => x.SectionType == model.SectionType).ToList();
                if (data.Any())
                {
                    baseResponse = baseResponse.AlreadyExists();
                    return Ok(baseResponse);
                }
                else
                {
                    ManageLayoutTypesDetails manageLayoutTypesDetails = new ManageLayoutTypesDetails();
                    manageLayoutTypesDetails.LayoutId = model.LayoutId;
                    manageLayoutTypesDetails.LayoutTypeId = model.LayoutTypeId;
                    manageLayoutTypesDetails.Name = model.Name;
                    manageLayoutTypesDetails.SectionType = model.SectionType;
                    manageLayoutTypesDetails.InnerColumns = model.InnerColumns;
                    manageLayoutTypesDetails.CreatedAt = DateTime.Now;
                    manageLayoutTypesDetails.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypesDetails, "POST", manageLayoutTypesDetails);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                ManageLayoutTypesDetails manageLayoutTypesDetails = new ManageLayoutTypesDetails();
                manageLayoutTypesDetails.LayoutId = model.LayoutId;
                manageLayoutTypesDetails.LayoutTypeId = model.LayoutTypeId;
                manageLayoutTypesDetails.Name = model.Name;
                manageLayoutTypesDetails.SectionType = model.SectionType;
                manageLayoutTypesDetails.InnerColumns = model.InnerColumns;
                manageLayoutTypesDetails.CreatedAt = DateTime.Now;
                manageLayoutTypesDetails.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypesDetails, "POST", manageLayoutTypesDetails);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ManageLayoutTypesDetailsDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLayoutTypesDetails + "?Name=" + model.Name + "&LayoutId=" + model.LayoutId + "&LayoutTypeId=" + model.LayoutTypeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageLayoutTypesDetails> tempList = (List<ManageLayoutTypesDetails>)baseResponse.Data;

            if (tempList.Where(x => x.Id != model.Id && x.SectionType == model.SectionType).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageLayoutTypes + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageLayoutTypesDetails record = (ManageLayoutTypesDetails)baseResponse.Data;
                record.LayoutId = model.LayoutId;
                record.LayoutTypeId = model.LayoutTypeId;
                record.Name = model.Name;
                record.SectionType = model.SectionType;
                record.InnerColumns = model.InnerColumns;
                record.ModifiedAt = DateTime.Now;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypesDetails, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLayoutTypesDetails + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageLayoutTypesDetails> tempList = (List<ManageLayoutTypesDetails>)baseResponse.Data;
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypesDetails + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> Get(int? pageindex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypesDetails + "?PageIndex=" + pageindex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypesDetails + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypesDetails + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }
}
