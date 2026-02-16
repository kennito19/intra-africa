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
    public class AssignSpecificationToCategoryController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public AssignSpecificationToCategoryController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {

            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }
        
        BaseResponse<AssignSpecificationToCategoryLibrary> baseResponse = new BaseResponse<AssignSpecificationToCategoryLibrary>();

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Save(AssignSpecificationToCategoryLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.AssignSpecToCat + "?CategoryID=" + model.CategoryID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignSpecificationToCategoryLibrary> assignSpecifications = (List<AssignSpecificationToCategoryLibrary>)baseResponse.Data;
            if (assignSpecifications.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                AssignSpecificationToCategoryLibrary asptcat = new AssignSpecificationToCategoryLibrary();
                asptcat.CategoryID = model.CategoryID;
                asptcat.IsAllowSize = model.IsAllowSize;
                asptcat.IsAllowSpecifications = model.IsAllowSpecifications;
                asptcat.IsAllowPriceVariant = model.IsAllowPriceVariant;
                asptcat.IsAllowColors = model.IsAllowColors;
                asptcat.IsAllowColorsInFilter = model.IsAllowColorsInFilter;
                asptcat.IsAllowColorsInVariant = false;
                asptcat.IsAllowColorsInComparision = model.IsAllowColorsInComparision;
                asptcat.IsAllowColorsInTitle = model.IsAllowColorsInTitle;
                asptcat.IsAllowExpiryDates = false;
                asptcat.TitleSequenceOfColor = model.TitleSequenceOfColor;
                asptcat.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.AssignSpecToCat, "POST", asptcat);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(AssignSpecificationToCategoryLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.AssignSpecToCat + "?CategoryID=" + model.CategoryID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignSpecificationToCategoryLibrary> templist = (List<AssignSpecificationToCategoryLibrary>)baseResponse.Data;
            if (templist.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                AssignSpecificationToCategoryLibrary asptcat = new AssignSpecificationToCategoryLibrary();
                asptcat.Id = model.Id;
                asptcat.Guid = model.Guid;
                asptcat.CategoryID = model.CategoryID;
                asptcat.IsAllowSize = model.IsAllowSize;
                asptcat.IsAllowSpecifications = model.IsAllowSpecifications;
                asptcat.IsAllowPriceVariant = model.IsAllowPriceVariant;
                asptcat.IsAllowColors = model.IsAllowColors;
                asptcat.IsAllowColorsInFilter = model.IsAllowColorsInFilter;
                asptcat.IsAllowColorsInVariant = false;
                asptcat.IsAllowColorsInComparision = model.IsAllowColorsInComparision;
                asptcat.IsAllowColorsInTitle = model.IsAllowColorsInTitle;
                asptcat.IsAllowExpiryDates = false;
                asptcat.TitleSequenceOfColor = model.TitleSequenceOfColor;
                asptcat.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.AssignSpecToCat, "PUT", asptcat);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.AssignSpecToCat + "?id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignSpecificationToCategoryLibrary> templist = (List<AssignSpecificationToCategoryLibrary>)baseResponse.Data;

            if (templist.Any())
            {
                var tempAssignSizeToCat = helper.ApiCall(URL, EndPoints.AssignSizeValueToCategory + "?AssignSpecId=" + id, "Get", null);
                BaseResponse<AssignSizeValueToCategory> baseResSizeToCat = new BaseResponse<AssignSizeValueToCategory>();
                var AssignSizeToCat = baseResSizeToCat.JsonParseList(tempAssignSizeToCat);
                List<AssignSizeValueToCategory> assignSizeToCat = (List<AssignSizeValueToCategory>)AssignSizeToCat.Data;

                var tempAssignSpecValuesToCat = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?AssignSpecID=" + id, "Get", null);
                BaseResponse<AssignSpecValuesToCategoryLibrary> baseSpecValuseToCat = new BaseResponse<AssignSpecValuesToCategoryLibrary>();
                var AssignSpecValuesToCat = baseSpecValuseToCat.JsonParseList(tempAssignSpecValuesToCat);
                List<AssignSpecValuesToCategoryLibrary> assignSpecValToCat = (List<AssignSpecValuesToCategoryLibrary>)AssignSpecValuesToCat.Data;

                var tempProduct = helper.ApiCall(URL, EndPoints.Product + "?AssiCategoryId=" + id, "Get", null);
                BaseResponse<Products> baseProudcts = new BaseResponse<Products>();
                var Products = baseProudcts.JsonParseList(tempProduct);
                List<Products> assignCatInProduct = (List<Products>)Products.Data;

                if (assignSpecValToCat.Any())
                {
                    baseResponse = baseResponse.ChildAlreadyExists("AssignSpecValuesToCategory", "AssignSpecificationToCategory");
                }
                else if (assignSizeToCat.Any())
                {
                    baseResponse = baseResponse.ChildAlreadyExists("AssignSizeValueToCategory", "AssignSpecificationToCategory");
                }
                else if (assignCatInProduct.Any())
                {
                    baseResponse = baseResponse.ChildAlreadyExists("Products", "AssignSpecificationToCategory");
                }
                else
                {
                    var response = helper.ApiCall(URL, EndPoints.AssignSpecToCat + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.AssignSpecToCat + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("getById")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            BaseResponse<checkAssignSpecToCategory> AssignSpecbaseResponse = new BaseResponse<checkAssignSpecToCategory>();
            var responseData = helper.ApiCall(URL, EndPoints.CheckAssignSpecsToCategory + "/checkAssignSpecToCat" + "?assignSpecId=" + id + "&multiSeller=" + Convert.ToBoolean(_configuration.GetValue<string>("is_one_product_multiseller")), "GET", null);
            AssignSpecbaseResponse = AssignSpecbaseResponse.JsonParseRecord(responseData);


            var response = helper.ApiCall(URL, EndPoints.AssignSpecToCat + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            if (baseResponse.code == 200)
            {
                AssignSpecificationToCategoryLibrary _assignSpecificationToCategoryLibrary = (AssignSpecificationToCategoryLibrary)baseResponse.Data;
                if (AssignSpecbaseResponse.code == 200)
                {
                    checkAssignSpecToCategory checkAssignSpecToCategory = (checkAssignSpecToCategory)AssignSpecbaseResponse.Data;

                    _assignSpecificationToCategoryLibrary.SizeEnabled = Convert.ToBoolean(checkAssignSpecToCategory.AllowSize);
                    _assignSpecificationToCategoryLibrary.ColorEnabled = Convert.ToBoolean(checkAssignSpecToCategory.AllowColor);
                    _assignSpecificationToCategoryLibrary.PriceVariantEnabled = Convert.ToBoolean(checkAssignSpecToCategory.AllowPriceVariant);
                    _assignSpecificationToCategoryLibrary.SpecificationsEnabled = Convert.ToBoolean(checkAssignSpecToCategory.AllowSpecifications);

                }
                baseResponse.Data = _assignSpecificationToCategoryLibrary;
            }

            return Ok(baseResponse);
        }

        [HttpGet("getListByCatId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> getListByCatId(int catId)
        {
            var response = helper.ApiCall(URL, EndPoints.AssignSpecToCat + "?CategoryID=" + catId, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("getByCatId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByCatID(int catId)
        {
            var response = helper.ApiCall(URL, EndPoints.AssignSpecToCat + "?CategoryID=" + catId, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(int? catId = 0, string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (catId != null && catId > 0)
            {
                url += "&CatId=" + catId;
            }

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url += "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(URL, EndPoints.AssignSpecToCat + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }
    }
}
