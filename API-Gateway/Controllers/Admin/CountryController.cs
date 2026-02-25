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
    public class CountryController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        BaseResponse<CountryLibrary> baseResponse = new BaseResponse<CountryLibrary>();
        public static string UserApi = string.Empty;
        private ApiHelper api;
        public CountryController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            UserApi = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            api = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(CountryLibrary model)
        {
            var temp = api.ApiCall(UserApi, EndPoints.Country + "?name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<CountryLibrary> countries = baseResponse.Data as List<CountryLibrary> ?? new List<CountryLibrary>();

            if (countries.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                CountryLibrary country = new CountryLibrary();
                country.Name = model.Name;
                country.Status = model.Status;
                country.CreatedAt = DateTime.Now;
                country.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value; ;
                var response = api.ApiCall(UserApi, EndPoints.Country, "POST", country);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(CountryLibrary model)
        {
            var temp = api.ApiCall(UserApi, EndPoints.Country + "?name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<CountryLibrary> countries = baseResponse.Data as List<CountryLibrary> ?? new List<CountryLibrary>();
            if (countries.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = api.ApiCall(UserApi, EndPoints.Country + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                CountryLibrary record = baseResponse.Data as CountryLibrary;
                record.Id = model.Id;
                record.Name = model.Name;
                record.Status = model.Status;
                record.ModifiedAt = DateTime.Now;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value; ;
                var response = api.ApiCall(UserApi, EndPoints.Country, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var tempCountry = api.ApiCall(UserApi, EndPoints.Country + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(tempCountry);
            List<CountryLibrary> country = baseResponse.Data as List<CountryLibrary> ?? new List<CountryLibrary>();

            if (country.Any())
            {
                var tempCity = api.ApiCall(UserApi, EndPoints.City + "?countryId=" + id, "GET", null);
                BaseResponse<CityLibrary> baseResCity = new BaseResponse<CityLibrary>();
                var CityResponse = baseResCity.JsonParseList(tempCity);
                List<CityLibrary> City = CityResponse.Data as List<CityLibrary> ?? new List<CityLibrary>();

                if (City.Any())
                {
                    baseResponse = baseResponse.ChildExists();
                }

                var tempStates = api.ApiCall(UserApi, EndPoints.State + "?countryId=" + id, "GET", null);
                BaseResponse<StateLibrary> baseResState = new BaseResponse<StateLibrary>();
                var StateResponse = baseResState.JsonParseList(tempStates);
                List<StateLibrary> State = StateResponse.Data as List<StateLibrary> ?? new List<StateLibrary>();

                if (State.Any())
                {
                    baseResponse = baseResponse.ChildExists();
                }
                else
                {
                    var response = api.ApiCall(UserApi, EndPoints.Country + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize]
        public ActionResult<ApiHelper> Search(string? searchtext=null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }
            var response = api.ApiCall(UserApi, EndPoints.Country + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

    }

}
