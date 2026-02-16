using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API_Gateway.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageHomePageSectionController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ManageHomePageSectionsLibrary> baseResponse = new BaseResponse<ManageHomePageSectionsLibrary>();

        public ManageHomePageSectionController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        //[HttpGet("GetHomePageSection")]
        //[Authorize(Roles = "Admin, Seller")]
        //public ActionResult<ApiHelper> GetHomePageSection(int? pageindex = 1, int? pageSize = 10)
        //{
        //    var response = helper.ApiCall(URL, EndPoints.ManageHomePageSections + "?PageIndex=" + pageindex + "&PageSize=" + pageSize, "GET", null, token);
        //    BaseResponse<ManageHomePageSectionsLibrary> baseResponse = new BaseResponse<ManageHomePageSectionsLibrary>();
        //    baseResponse = baseResponse.JsonParseList(response);
        //    List<ManageHomePageSectionsLibrary> homepageSection = (List<ManageHomePageSectionsLibrary>)baseResponse.Data;
        //    homepageSection = homepageSection.OrderBy(x => x.Sequence).ToList();

        //    var responseDetail = helper.ApiCall(URL, EndPoints.ManageHomePageDetails, "GET", null, token);
        //    BaseResponse<ManageHomePageDetailsLibrary> baseResponseDetail = new BaseResponse<ManageHomePageDetailsLibrary>();
        //    baseResponseDetail = baseResponseDetail.JsonParseList(responseDetail);
        //    List<ManageHomePageDetailsLibrary> homePageDetail = (List<ManageHomePageDetailsLibrary>)baseResponseDetail.Data;
        //    homePageDetail = homePageDetail.OrderBy(x => x.Sequence).ToList();

        //    var result = homepageSection.Select(section => new
        //    {
        //        Id = section.Id,
        //        LayoutId = section.LayoutId,
        //        LayoutName = section.LayoutName,
        //        LayoutTypeName = section.LayoutTypeName,
        //        LayoutTypeId = section.LayoutTypeId,
        //        //Name = section.Name,
        //        Sequence = section.Sequence,
        //        Title = section.Title,
        //        SubTitle = section.SubTitle,
        //        Status = section.Status,
        //        homePageSectionDetail = homePageDetail.Where(detail => detail.SectionId == section.Id)
        //        .Select(homePageSectionDetail => new
        //        {
        //            Id = homePageSectionDetail.Id,
        //            SectionId = homePageSectionDetail.SectionId,
        //            Image = homePageSectionDetail.Image,
        //            ImageAlt = homePageSectionDetail.ImageAlt,
        //            Sequence = homePageSectionDetail.Sequence,
        //            RedirectTo = homePageSectionDetail.RedirectTo,
        //            CategoryId = homePageSectionDetail.CategoryId,
        //            BrandIds = homePageSectionDetail.BrandIds,
        //            SizeIds = homePageSectionDetail.SizeIds,
        //            SpecificationIds = homePageSectionDetail.SpecificationIds,
        //            CollectionIds = homePageSectionDetail.CollectionId,
        //            ProductIds = homePageSectionDetail.ProductId,
        //            OtherIds = homePageSectionDetail.StaticPageId,
        //            AssignCity = homePageSectionDetail.AssignCity,
        //            AssignState = homePageSectionDetail.AssignState,
        //            AssignCountry = homePageSectionDetail.AssignCountry,
        //            Status = homePageSectionDetail.Status,
        //            SectionName = homePageSectionDetail.SectionName,
        //        })
        //        .ToList(),
        //    }); ;
        //    baseResponse.Data = result;
        //    return Ok(baseResponse);

        //}
        [HttpGet("GetHomePageSection")]
        [Authorize]
        public ActionResult<ApiHelper> GetHomePageSection(string? status = null)
        {
            getHomePageSections getHomePage = new getHomePageSections(_configuration, _httpContext);
            JObject res = getHomePage.setSections(status);
            return Ok(res.ToString());
        }


        [HttpGet("GetMenu")]
        [Authorize]
        public ActionResult<ApiHelper> GetMenu()
        {
            getHomePageSections getHomePage = new getHomePageSections(_configuration, _httpContext);
            var res = getHomePage.getmenuList();
            BaseResponse<HomepageMenu> baseResponse = new BaseResponse<HomepageMenu>();
            baseResponse.Data = res;
            baseResponse.Message = "Records bind successfully";
            baseResponse.code = 200;

            return Ok(baseResponse);
        }
    }
}
