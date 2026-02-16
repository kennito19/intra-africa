using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Gateway.Controllers.Seller
{
    [Route("api/seller/[controller]")]
    [ApiController]
    public class SellerBrandController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private Brands brands;
        private AssignBrands AssignBrands;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string CatelogueURL = string.Empty;
        public string IDServerUrl = string.Empty;
        BaseResponse<BrandLibrary> baseResponse = new BaseResponse<BrandLibrary>();

        public SellerBrandController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            brands = new Brands(URL, _configuration, _httpContext);
            AssignBrands = new AssignBrands(URL, _httpContext, CatelogueURL, IDServerUrl, _configuration);
        }

        [HttpPost("AddNewBrandBySeller")]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> AddNewBrand([FromForm] SellerBrand model)
        {
            bool AllowBrandCerti = Convert.ToBoolean(_configuration.GetValue<string>("allow_brand_certificate"));
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            BrandDTO brand = new BrandDTO();
            brand.Name = model.Name;
            brand.Description = model.Description;
            brand.Status = model.Status;
            brand.Logo = model.Logo;
            brand.FileName = model.LogoFile;

            baseResponse = brands.SaveBrand(brand, userID, false);
            if (baseResponse.code == 200)
            {
                var brandId = (int)baseResponse.Data;

                AssignBrandToSellerDTO abts = new AssignBrandToSellerDTO();
                abts.SellerID = userID;
                abts.BrandId = brandId;
                abts.Status = "Request For Approval";
                abts.BrandCertificate = model.BrandCertificate;
                abts.FileName = model.BrandCertificateFile;
                abts.BrandName = model.Name;
                abts.SellerName = model.SellerDisplayName;

                var response = AssignBrands.SaveAssignBrands(abts, userID, AllowBrandCerti, false);

                baseResponse.code = 200;
                baseResponse.Message = "Brand Request Sent for Approval";

            }
            else
            {
                baseResponse.code = 201;
                baseResponse.Message = "Brand already exists";
            }
            return Ok(baseResponse);
        }


        [HttpPost("RequestBySeller")]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> RequestBrand([FromForm] AssignBrandToSellerDTO model)
        {
            bool AllowBrandCerti = Convert.ToBoolean(_configuration.GetValue<string>("allow_brand_certificate"));
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

            model.SellerID = userID;
            model.Status = "Request For Approval";
            model.BrandCertificate = model.BrandCertificate;
            model.FileName = model.FileName;
            var response = AssignBrands.SaveAssignBrands(model, userID, AllowBrandCerti, false);

            if (response.code == 200)
            {
                baseResponse.code = 200;
                baseResponse.Message = "Brand Request Sent for Approval";
            }
            else
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            return Ok(baseResponse);
        }

        [HttpPut("UpdateStatus")]
        [Authorize(Roles = "Seller")]
        public ActionResult<ApiHelper> Update([FromForm] AssignBrandToSellerDTO model)
        {
            BaseResponse<AssignBrandToSeller> assignmentResponse = new BaseResponse<AssignBrandToSeller>();
            bool AllowBrandCerti = Convert.ToBoolean(_configuration.GetValue<string>("allow_brand_certificate"));
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            assignmentResponse = AssignBrands.UpdateAssignBrands(model, userID, AllowBrandCerti);
            return Ok(assignmentResponse);
        }

        [HttpGet("SellerBrands")]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> GetSellerBrands(string SellerID, int pageIndex = 1, int pageSize = 10, string? searchtext = null, string? status = null)
        {
            baseResponse = brands.GetBrand(0, 0);
            List<BrandLibrary> brandLists = (List<BrandLibrary>)baseResponse.Data;


            BaseResponse<AssignBrandToSeller> assignmentResponse = new BaseResponse<AssignBrandToSeller>();

            assignmentResponse = AssignBrands.GetAssignBrands(0, 0);
            List<AssignBrandToSeller> SellerBrandLists = (List<AssignBrandToSeller>)assignmentResponse.Data;

            var response = (from b in brandLists
                            join ab in SellerBrandLists on b.ID equals ab.BrandId
                            where ab.SellerID == SellerID && ab.Status.ToLower() != "rejected"
                            select new { ab.Id, ab.BrandId, b.Logo, b.Name, ab.Status }).ToList();

            if (!string.IsNullOrEmpty(searchtext))
            {
                response = response.Where(p => p.Name.ToLower().Contains(searchtext.ToLower())).ToList();
            }

            if (!string.IsNullOrEmpty(status))
            {
                response = response.Where(p => p.Status.ToLower() == status.ToLower()).ToList();
            }

            //baseResponse.Data = response;
            int totalCount = response.Count;
            int TotalPages = pageSize == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)pageSize);
            var items = pageIndex == 0 ? response.ToList() : response.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            if (items.Count > 0)
            {
                baseResponse.Message = "Data bind suceessfully.";
                baseResponse.Data = items;
                baseResponse.code = 200;
                baseResponse.pagination.PageCount = TotalPages;
                baseResponse.pagination.RecordCount = totalCount;
            }
            else
            {
                baseResponse.Message = "Record does not exists.";
                baseResponse.Data = null;
                baseResponse.code = 204;
                baseResponse.pagination = null;
            }
            return Ok(baseResponse);
        }

        [HttpGet("BrandRequestList")]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> GetBrandRequestList(string SellerID, int pageIndex = 1, int pageSize = 10, string? searchtext = null)
        {
            BaseResponse<AssignBrandToSeller> assignmentResponse = new BaseResponse<AssignBrandToSeller>();
            List<AssignBrandToSeller> _SellerBrandLists = new List<AssignBrandToSeller>();
            List<AssignBrandToSeller> _SellerBrandLists1 = new List<AssignBrandToSeller>();

            baseResponse = brands.GetBrand(0, 0);
            assignmentResponse = AssignBrands.GetAssignBrands(0, 0);

            List<BrandLibrary> brandLists = (List<BrandLibrary>)baseResponse.Data;
            brandLists = brandLists.OrderByDescending(p => p.CreatedAt).ToList();

            List<AssignBrandToSeller> SellerBrandLists = (List<AssignBrandToSeller>)assignmentResponse.Data;
            SellerBrandLists = SellerBrandLists.OrderByDescending(p => p.CreatedAt).ToList();

            _SellerBrandLists = SellerBrandLists.Where(p => p.SellerID == SellerID && p.Status.ToLower() != "rejected").ToList();

            var response = brandLists.Select(b => new { b.ID, b.Logo, b.Name, Status = "Request Pending" }).ToList();

            if (_SellerBrandLists.Any())
            {
                brandLists = brandLists.Where(p => _SellerBrandLists.All(item => item.BrandId != p.ID)).ToList();
                _SellerBrandLists1 = SellerBrandLists.Where(p => p.SellerID == SellerID && p.Status.ToLower() == "rejected").ToList();
                if (_SellerBrandLists1.Any())
                {
                    response = response
                    .Concat(from b in brandLists
                            join ab in _SellerBrandLists1 on b.ID equals ab.BrandId
                            select new { b.ID, b.Logo, b.Name, Status = ab.Status }).ToList();
                }
                else
                {
                    response = brandLists.Select(b => new { b.ID, b.Logo, b.Name, Status = "Request Pending" }).ToList();
                }
            }
            else
            {
                _SellerBrandLists1 = SellerBrandLists.Where(p => p.SellerID == SellerID && p.Status.ToLower() == "rejected").ToList();
                if (_SellerBrandLists1.Any())
                {
                    response = response
                        .Concat(from b in brandLists
                                join ab in _SellerBrandLists1 on b.ID equals ab.BrandId
                                select new { b.ID, b.Logo, b.Name, Status = ab.Status }).ToList();
                }
            }

            if (!string.IsNullOrEmpty(searchtext))
            {
                response = response.Where(p => p.Name.ToLower().Contains(searchtext.ToLower())).ToList();
            }

            if (response.Count > 0)
            {
                int totalCount = response.Count;
                int TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
                var items = response.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                baseResponse.Message = "Data bind suceessfully.";
                baseResponse.Data = items;
                baseResponse.code = 200;
                baseResponse.pagination.PageCount = TotalPages;
                baseResponse.pagination.RecordCount = totalCount;
            }
            else
            {
                baseResponse.Message = "Record does not exists.";
                baseResponse.Data = null;
                baseResponse.code = 204;
                baseResponse.pagination = null;
            }

            return Ok(baseResponse);
        }

        [HttpGet("ListofBrands")]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> Get(int? PageIndex = 1, int? PageSize = 10)
        {
            baseResponse = brands.GetBrand(PageIndex, PageSize);
            return Ok(baseResponse);
        }

        [HttpGet("ById")]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> ById(int id)
        {
            baseResponse = brands.GetBrandById(id);
            return Ok(baseResponse);
        }

        [HttpGet("ByName")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ByName(string Name = null)
        {
            baseResponse = brands.GetBrandByName(Name);
            return Ok(baseResponse);
        }

    }
}
