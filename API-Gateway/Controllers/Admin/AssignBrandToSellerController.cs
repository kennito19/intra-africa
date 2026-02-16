using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignBrandToSellerController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private AssignBrands AssignBrands;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string CatelogueURL = string.Empty;
        BaseResponse<AssignBrandToSeller> baseResponse = new BaseResponse<AssignBrandToSeller>();
        public static string IDServerUrl = string.Empty;

        public AssignBrandToSellerController(IConfiguration configuration)
        {
            _httpContextAccessor = new HttpContextAccessor();
            _httpContext = _httpContextAccessor.HttpContext;

            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            AssignBrands = new AssignBrands(URL, _httpContext, CatelogueURL, IDServerUrl, _configuration);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Save([FromForm] AssignBrandToSellerDTO model)
        {
            bool AllowBrandCerti = Convert.ToBoolean(_configuration.GetValue<string>("allow_brand_certificate"));
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = AssignBrands.SaveAssignBrands(model, userID, AllowBrandCerti, true);
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update([FromForm] AssignBrandToSellerDTO model)
        {
            bool AllowBrandCerti = Convert.ToBoolean(_configuration.GetValue<string>("allow_brand_certificate"));
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = AssignBrands.UpdateAssignBrands(model, userID, AllowBrandCerti);
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int? id = 0)
        {
            baseResponse = AssignBrands.DeleteAssignBrands(id);
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Customer")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            baseResponse = AssignBrands.GetAssignBrands(pageIndex, pageSize);
            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ById(int id)
        {
            baseResponse = AssignBrands.GetAssignBrandsById(id);
            return Ok(baseResponse);
        }

        [HttpGet("bySeller&BrandId")]
        [Authorize(Roles = "Admin, Customer, Seller")]
        public ActionResult<ApiHelper> ByUserID(string sellerId = null, int brandId = 0, string? status = null, int? pageIndex = 1, int? pageSize = 10)
        {
            baseResponse = AssignBrands.GetAssignBrandsByUserID(sellerId, brandId, status, pageIndex, pageSize);
            return Ok(baseResponse);
        }

        [HttpGet("Search")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Search(string? searchtext, string? sellerId, string? status, int? PageIndex = 1, int? PageSize = 10)
        {
            baseResponse = AssignBrands.Search(searchtext, sellerId, status, PageIndex, PageSize);
            return Ok(baseResponse);
        }

    }
}
