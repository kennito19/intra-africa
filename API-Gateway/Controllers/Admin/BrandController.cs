using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private Brands brands;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string catalougeURL = string.Empty;
        BaseResponse<BrandLibrary> baseResponse = new BaseResponse<BrandLibrary>();

        public BrandController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;

            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            catalougeURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            brands = new Brands(URL, _configuration, _httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Save([FromForm] BrandDTO model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = brands.SaveBrand(model, userID, true);
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update([FromForm] BrandDTO model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = brands.UpdateBrand(model, userID);
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int? id = 0)
        {
            baseResponse = brands.DeleteBrand(id);
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Seller,Customer")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            baseResponse = brands.GetBrand(pageIndex, pageSize);
            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin,Customer")]
        public ActionResult<ApiHelper> BySearch(string? searchtext = null, string? status=null, int? PageIndex = 1, int? PageSize = 10)
        {
            baseResponse = brands.Search(searchtext, status, PageIndex, PageSize);
            return Ok(baseResponse);
        }

    }
}
