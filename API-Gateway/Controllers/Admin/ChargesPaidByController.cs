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
    public class ChargesPaidByController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ChargesPaidByLibrary> baseResponse = new BaseResponse<ChargesPaidByLibrary>();
        public ChargesPaidByController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Save(ChargesPaidByLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ChargesPaidBy + "?Name=" + model.Name, "GET", null);

            baseResponse = baseResponse.JsonParseList(temp);
            List<ChargesPaidByLibrary> tempList = (List<ChargesPaidByLibrary>)baseResponse.Data;
            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ChargesPaidByLibrary charge = new ChargesPaidByLibrary();
                charge.Name = model.Name;
                charge.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ChargesPaidBy, "POST", charge);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ChargesPaidByLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ChargesPaidBy + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ChargesPaidByLibrary> chargesPaidBy = (List<ChargesPaidByLibrary>)baseResponse.Data;
            if (chargesPaidBy.Where(X => X.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ChargesPaidByLibrary charge = new ChargesPaidByLibrary();
                charge.Id = model.Id;
                charge.Name = model.Name;
                charge.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ChargesPaidBy, "PUT", charge);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ChargesPaidBy + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ChargesPaidByLibrary> chargesPaidBy = (List<ChargesPaidByLibrary>)baseResponse.Data;

            if (chargesPaidBy.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ChargesPaidBy + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet("Search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }
            var response = helper.ApiCall(URL, EndPoints.ChargesPaidBy + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }
    }
}
