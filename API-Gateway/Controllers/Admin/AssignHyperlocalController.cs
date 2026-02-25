using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignHyperlocalController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;

        private ApiHelper api;
        BaseResponse<AssignSellerHyperlocal> baseResponse = new BaseResponse<AssignSellerHyperlocal>();
        public static string UserAPI = string.Empty;

        public AssignHyperlocalController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            api = new ApiHelper(_httpContext);
            _configuration = configuration;
            UserAPI = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
        }

        [HttpPost]
        public IActionResult Save(AssignSellerDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?sellerId=" + model.SellerId + "&isDeleted=false", "GET", "");
                baseResponse = baseResponse.JsonParseList(response);

                List<AssignSellerHyperlocal> assignSellerHyperlocals = baseResponse.Data as List<AssignSellerHyperlocal> ?? new List<AssignSellerHyperlocal>();
                if (!assignSellerHyperlocals.Where(x => x.BrandId == (model.BrandId) && x.CityID == model.CityID).Any())
                {
                    AssignSellerHyperlocal assignSeller = new AssignSellerHyperlocal();
                    assignSeller.SellerId = model.SellerId;
                    assignSeller.CountryID = model.CountryID;
                    assignSeller.StateID = model.StateID;
                    assignSeller.SellerId = model.SellerId;
                    assignSeller.CityID = model.CityID;
                    assignSeller.BrandId = model.BrandId;
                    assignSeller.Status = model.Status;
                    assignSeller.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    assignSeller.CreatedAt = DateTime.UtcNow;

                    var res = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal, "POST", assignSeller);
                    baseResponse = baseResponse.JsonParseInputResponse(res);
                }
                else
                {
                    baseResponse = baseResponse.AlreadyExists();
                }
                return Ok(baseResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        public IActionResult Update(AssignSellerDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?sellerId=" + model.SellerId + "&isDeleted=false", "GET", "");
                baseResponse = baseResponse.JsonParseList(response);

                List<AssignSellerHyperlocal> assignSellerHyperlocals = baseResponse.Data as List<AssignSellerHyperlocal> ?? new List<AssignSellerHyperlocal>();
                if (!assignSellerHyperlocals.Where(x => x.BrandId == (model.BrandId ?? 0) && x.CityID == model.CityID).Any())
                {

                    var temp = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?id=" + model.Id + "&isDeleted=false&Mode=check", "GET", null);
                    baseResponse = baseResponse.JsonParseRecord(response);
                    AssignSellerHyperlocal assignSeller = baseResponse.Data as AssignSellerHyperlocal;
                    assignSeller.SellerId = model.SellerId;
                    assignSeller.CountryID = model.CountryID;
                    assignSeller.StateID = model.StateID;
                    assignSeller.SellerId = model.SellerId;
                    assignSeller.CityID = model.CityID;
                    assignSeller.BrandId = model.BrandId;
                    assignSeller.Status = model.Status;
                    assignSeller.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    assignSeller.ModifiedAt = DateTime.UtcNow;
                    var res = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal, "PUT", assignSeller);
                    baseResponse = baseResponse.JsonParseInputResponse(res);
                }
                else
                {
                    baseResponse = baseResponse.AlreadyExists();
                }
                return Ok(baseResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var response = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?id=" + Id + "&isDeleted=false", "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            AssignSellerHyperlocal assignSeller = baseResponse.Data as AssignSellerHyperlocal;
            if (assignSeller != null)
            {
                var res = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?id=" + Id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(res);
            }
            return Ok(baseResponse);
        }

        [HttpGet]
        public IActionResult Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?isDeleted=false&PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Mode=get", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            return Ok(baseResponse);
        }

        [HttpGet("ById")]
        public IActionResult GetById(int Id)
        {
            var response = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?id=" + Id + "&isDeleted=false", "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            return Ok(baseResponse);
        }


        [HttpGet("SellerId")]
        public IActionResult GetBySellerId(string sellerId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?sellerId=" + sellerId + "&isDeleted=false&PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Mode=get", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            return Ok(baseResponse);
        }

        [HttpGet("BrandId")]
        public IActionResult GetByBrandId(int brandId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?brandId=" + brandId + "&isDeleted=false&PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Mode=get", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            return Ok(baseResponse);
        }

        [HttpGet("CountryId")]
        public IActionResult GetByCountryId(int countryId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?countryId=" + countryId + "&isDeleted=false&PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Mode=get", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            return Ok(baseResponse);
        }

        [HttpGet("StateId")]
        public IActionResult GetByStateId(int stateId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?stateId=" + stateId + "&isDeleted=false&PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Mode=get", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            return Ok(baseResponse);
        }

        [HttpGet("CityId")]
        public IActionResult GetByCityId(int cityId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?cityId=" + cityId + "&isDeleted=false&PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Mode=get", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            return Ok(baseResponse);
        }

        [HttpGet("Search")]
        public IActionResult Search(string searchString, int? pageIndex = 1, int? pageSize = 10)
        {
            searchString = HttpUtility.UrlEncode(searchString);
            var response = api.ApiCall(UserAPI, EndPoints.AssignSellerHyperlocal + "?countryname=" + searchString + "&statename=" + searchString + "&cityname=" + searchString + "&isDeleted=false&PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&Mode=check", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            return Ok(baseResponse);
        }
    }
}
