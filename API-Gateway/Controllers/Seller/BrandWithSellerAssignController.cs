using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;

namespace API_Gateway.Controllers.Seller
{
    [Route("api/seller/[controller]")]
    [ApiController]
    public class BrandWithSellerAssignController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        BaseResponse<BrandLibrary> baseResponse = new BaseResponse<BrandLibrary>();
        BaseResponse<AssignBrandToSeller> AssignBrandbaseResponse = new BaseResponse<AssignBrandToSeller>();
        private Brands brands;
        private AssignBrands assignBrands;
        public string URL = string.Empty;
        public string catalougeURL = string.Empty;
        public static string IDServerUrl = string.Empty;

        public BrandWithSellerAssignController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;

            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            brands = new Brands(URL, _configuration, _httpContext);
            catalougeURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            assignBrands = new AssignBrands(URL, _httpContext, catalougeURL, IDServerUrl, _configuration);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> AddBrand([FromForm] BrandWithSellerAssignDTO model)
        {
            bool AllowBrandCerti = Convert.ToBoolean(_configuration.GetValue<string>("allow_brand_certificate"));
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            BrandDTO brand = new BrandDTO();
            brand.Name = model.Name;
            brand.Description = model.Description;
            brand.Status = model.Status;
            brand.Logo = model.Logo;
            brand.FileName = model.LogoFile;

            baseResponse = brands.SaveBrand(brand, userID, true);
            if (baseResponse.code == 200)
            {
                var brandId = Convert.ToInt32(baseResponse.Data);

                AssignBrandToSellerDTO abts = new AssignBrandToSellerDTO();
                abts.SellerID = model.SellerId;
                abts.BrandId = brandId;
                abts.Status = model.Status;
                abts.BrandCertificate = null;
                abts.FileName = null;
                var response = assignBrands.SaveAssignBrands(abts, userID, AllowBrandCerti, true);

                baseResponse.code = 200;
                baseResponse.Message = "Record added successfully";
            }
            else
            {
                baseResponse.code = 201;
                baseResponse.Message = "Brand already exists";
            }
            return Ok(baseResponse);
        }
        
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> UpdateBrand([FromForm] BrandWithSellerAssignDTO model)
        {
            bool AllowBrandCerti = Convert.ToBoolean(_configuration.GetValue<string>("allow_brand_certificate"));
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            BrandDTO brand = new BrandDTO();
            brand.ID = model.Id;
            brand.Name = model.Name;
            brand.Description = model.Description;
            brand.Status = model.Status;
            brand.Logo = model.Logo;
            brand.FileName = model.LogoFile;

            baseResponse = brands.UpdateBrand(brand, userID);
            if (baseResponse.code == 200)
            {
                var brandId = Convert.ToInt32(baseResponse.Data);

                AssignBrandToSellerDTO abts = new AssignBrandToSellerDTO();

                abts.Id = Convert.ToInt32(model.AssignBrandToSeller);
                abts.SellerID = model.SellerId;
                abts.BrandId = brandId;
                abts.Status = model.Status;
                abts.BrandCertificate = null;
                abts.FileName = null;
                var response = assignBrands.SaveAssignBrands(abts, userID, AllowBrandCerti, true);

                baseResponse.code = 200;
                baseResponse.Message = "Record added successfully";
            }
            else
            {
                baseResponse.code = 201;
                baseResponse.Message = "Brand already exists";
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int? assignbrandid = 0,int? brandid = 0)
        {

            AssignBrandbaseResponse = assignBrands.DeleteAssignBrands(assignbrandid);
            if (AssignBrandbaseResponse.code == 200)
            {
                baseResponse = brands.DeleteBrand(brandid);
                return Ok(baseResponse);
            }
            else
            {
                return Ok(AssignBrandbaseResponse);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Customer")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            AssignBrandbaseResponse = assignBrands.GetAssignBrands(pageIndex, pageSize);
            return Ok(AssignBrandbaseResponse);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ById(int id)
        {
            AssignBrandbaseResponse = assignBrands.GetAssignBrandsById(id);
            return Ok(AssignBrandbaseResponse);
        }

        [HttpGet("byStatus")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> ByStatus(string Status, int? pageIndex = 1, int? pageSize = 10)
        {
            AssignBrandbaseResponse = assignBrands.GetAssignBrandsByStatus(Status, pageIndex, pageSize);
            return Ok(AssignBrandbaseResponse);
        }

        [HttpGet("bySeller&BrandId")]
        [Authorize(Roles = "Admin, Customer, Seller")]
        public ActionResult<ApiHelper> ByUserID(string sellerId = null, int brandId = 0, string? status = null, int? pageIndex = 1, int? pageSize = 10)
        {
            AssignBrandbaseResponse = assignBrands.GetAssignBrandsByUserID(sellerId, brandId, status, pageIndex,pageSize);
            return Ok(AssignBrandbaseResponse);
        }

        [HttpGet("byBrandId")]
        [Authorize(Roles = "Admin, Customer, Seller")]
        public ActionResult<ApiHelper> byBrandId(int brandId, string? status = null)
        {
            AssignBrandbaseResponse = assignBrands.GetAssignBrandsByBrandId(brandId, status);
            return Ok(AssignBrandbaseResponse);
        }

        [HttpGet("Search")]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult<ApiHelper> Search(string? searchtext, string? sellerId, string? status, int? PageIndex = 1, int? PageSize = 10)
        {
            AssignBrandbaseResponse = assignBrands.Search(searchtext, sellerId, status, PageIndex, PageSize);
            return Ok(AssignBrandbaseResponse);
        }

    }
}
