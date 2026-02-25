using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ColorLibrary> baseResponse = new BaseResponse<ColorLibrary>();
        private ApiHelper helper;
        public ColorController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Save(ColorLibrary model)
        {
            var temp = HttpUtility.UrlEncode(model.Code);
            var tmp = helper.ApiCall(URL, EndPoints.Color + "?Code=" + temp + "&Name=" + model.Name + "&Mode=" + Mode.Check, "GET", null);

            baseResponse = baseResponse.JsonParseList(tmp);
            List<ColorLibrary> tempList = baseResponse.Data as List<ColorLibrary> ?? new List<ColorLibrary>();
            if (!tempList.Any())
            {
                ColorLibrary color = new ColorLibrary();
                color.Name = model.Name;
                color.Code = model.Code;
                color.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.Color, "POST", color);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ColorLibrary model)
        {
            var Code = HttpUtility.UrlEncode(model.Code);
            var temp = helper.ApiCall(URL, EndPoints.Color + "?Name=" + model.Name + "&Code=" + Code + "&Mode=" + Mode.Check, "GET", null);

            baseResponse = baseResponse.JsonParseList(temp);
            List<ColorLibrary> tempList = baseResponse.Data as List<ColorLibrary> ?? new List<ColorLibrary>();

            if (!tempList.Where(x => x.Id != model.Id).Any())
            {
                var recordResponse = helper.ApiCall(URL, EndPoints.Color + "?guid=" + model.Guid, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordResponse);
                ColorLibrary color = baseResponse.Data as ColorLibrary;
                color.Id = model.Id;
                color.Guid = model.Guid;
                color.Name = model.Name;
                color.Code = model.Code;
                color.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.Color, "PUT", color);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.AlreadyExists();
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.Color + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ColorLibrary> tempList = baseResponse.Data as List<ColorLibrary> ?? new List<ColorLibrary>();
            if (tempList.Any())
            {
                var tempProductColorMapp = helper.ApiCall(URL, EndPoints.ProductColorMapping + "?ColorID=" + id, "GET", null);
                BaseResponse<ProductColorMapp> baseProductColor = new BaseResponse<ProductColorMapp>();
                var ProductColorMapping = baseProductColor.JsonParseList(tempProductColorMapp);
                List<ProductColorMapp> productColorMapp = ProductColorMapping.Data as List<ProductColorMapp> ?? new List<ProductColorMapp>();
                if (productColorMapp.Any())
                {
                    baseResponse = baseResponse.ChildAlreadyExists("ProductColorMapping", "Color");
                }
                else
                {
                    var response = helper.ApiCall(URL, EndPoints.Color + "?Id=" + id, "DELETE", null);
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
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext=null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }
            var response = helper.ApiCall(URL, EndPoints.Color + "?Mode=" + Mode.Get + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }



    }
}
