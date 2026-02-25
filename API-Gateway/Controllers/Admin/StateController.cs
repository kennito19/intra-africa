using API_Gateway.Helper;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<StateLibrary> baseResponse = new BaseResponse<StateLibrary>();
        public StateController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Save(StateLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.State + "?name=" + model.Name + "&countryId=" + model.CountryID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<StateLibrary> tmp = baseResponse.Data as List<StateLibrary> ?? new List<StateLibrary>();
            if (tmp.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                StateLibrary state = new StateLibrary();
                state.Name = model.Name;
                state.CountryID = model.CountryID;
                state.Status = model.Status;
                state.CreatedAt = DateTime.Now;
                state.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.State, "POST", state);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(StateLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.State + "?name=" + model.Name + "&countryId=" + model.CountryID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<StateLibrary> tmp = baseResponse.Data as List<StateLibrary> ?? new List<StateLibrary>();
            if (tmp.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(URL, EndPoints.State + "?id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(response);
                StateLibrary state = baseResponse.Data as StateLibrary;
                state.Id = model.Id;
                state.CountryID = model.CountryID;
                state.Status = model.Status;
                state.Name = model.Name;
                state.ModifiedAt = DateTime.Now;
                state.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                response = helper.ApiCall(URL, EndPoints.State, "PUT", state);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.State + "?id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<StateLibrary> tmp = baseResponse.Data as List<StateLibrary> ?? new List<StateLibrary>();
            if (tmp.Any())
            {
                temp = helper.ApiCall(URL, EndPoints.City + "?stateId=" + id, "GET", null);
                BaseResponse<CityLibrary> baseResCity = new BaseResponse<CityLibrary>();
                var CityResponse = baseResCity.JsonParseList(temp);
                List<CityLibrary> City = CityResponse.Data as List<CityLibrary> ?? new List<CityLibrary>();
                if (City.Any())
                {
                    baseResponse = baseResponse.ChildExists();
                }
                else
                {
                    var response = helper.ApiCall(URL, EndPoints.State + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet("byCountryId")]
        [Authorize]
        public ActionResult<ApiHelper> ByCountryId(int id, int? PageIndex = 1, int? PageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.State + "?countryId=" + id + "&PageIndex=" + PageIndex + "&PageSize=" + PageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byCountryIds")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> GetbyCountryIds(string countryIds, int? PageIndex = 1, int? PageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.State + "?countryIds=" + countryIds + "&PageIndex=" + PageIndex + "&PageSize=" + PageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("search")]
        [Authorize]
        public ActionResult<ApiHelper> Search(string? searchtext=null, int? PageIndex = 1, int? PageSize = 10)
        {
            string url = string.Empty;

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(URL, EndPoints.State + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }
    }
}
