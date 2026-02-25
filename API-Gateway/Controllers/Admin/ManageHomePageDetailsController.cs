using API_Gateway.Common;
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
    public class ManageHomePageDetailsController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ManageHomePageDetailsLibrary> baseResponse = new BaseResponse<ManageHomePageDetailsLibrary>();
        private ApiHelper helper;
        public ManageHomePageDetailsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create([FromForm] ManageHomePageDetailsDTO model)
        {
            ManageHomePageDetailsLibrary mhpd = new ManageHomePageDetailsLibrary();
            mhpd.SectionId = model.SectionId;
            mhpd.LayoutTypeDetailsId = model.LayoutTypeDetailsId;
            mhpd.OptionId = model.OptionId;
            mhpd.Image = UploadDoc(model.SectionName, model.ImageFile);
            mhpd.ImageAlt = model.ImageAlt;
            mhpd.IsTitleVisible = model.IsTitleVisible;
            mhpd.Title = model.Title;
            mhpd.SubTitle = model.SubTitle;
            mhpd.TitlePosition = model.TitlePosition;
            mhpd.Sequence = model.Sequence;
            mhpd.Columns = model.Columns;
            mhpd.RedirectTo = model.RedirectTo;
            mhpd.CategoryId = model.CategoryId;
            mhpd.BrandIds = model.BrandIds;
            mhpd.SizeIds = model.SizeIds;
            mhpd.SpecificationIds = model.SpecificationIds;
            mhpd.ColorIds = model.ColorIds;
            mhpd.CollectionId = model.CollectionId;
            mhpd.ProductId = model.ProductId;
            mhpd.StaticPageId = model.StaticPageId;
            mhpd.LendingPageId = model.LendingPageId;
            mhpd.CustomLinks = model.CustomLinks;
            mhpd.TitleColor = model.TitleColor;
            mhpd.SubTitleColor = model.SubTitleColor;
            mhpd.TitleSize = model.TitleSize;
            mhpd.SubTitleSize = model.SubTitleSize;
            mhpd.ItalicSubTitle = model.ItalicSubTitle;
            mhpd.ItalicTitle = model.ItalicTitle;
            mhpd.Description = model.Description;
            mhpd.SliderType = model.SliderType;
            mhpd.VideoLinkType = model.VideoLinkType;
            mhpd.VideoId = model.VideoId;
            mhpd.Name = model.Name;
            mhpd.AssignCity = model.AssignCity;
            mhpd.AssignState = model.AssignState;
            mhpd.AssignCountry = model.AssignCountry;
            mhpd.Status = model.Status;
            mhpd.CreatedAt = DateTime.Now;
            mhpd.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            var response = helper.ApiCall(URL, EndPoints.ManageHomePageDetails, "POST", mhpd);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update([FromForm] ManageHomePageDetailsDTO model)
        {
            var recordCall = helper.ApiCall(URL, EndPoints.ManageHomePageDetails + "?Id=" + model.Id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(recordCall);
            ManageHomePageDetailsLibrary mhpd = baseResponse.Data as ManageHomePageDetailsLibrary;
            mhpd.SectionId = model.SectionId;
            mhpd.LayoutTypeDetailsId = model.LayoutTypeDetailsId;
            mhpd.OptionId = model.OptionId;
            mhpd.Image = UpdateDocFile(mhpd.Image, model.SectionName, model.ImageFile);
            mhpd.ImageAlt = model.ImageAlt;
            mhpd.IsTitleVisible = model.IsTitleVisible;
            mhpd.Title = model.Title;
            mhpd.SubTitle = model.SubTitle;
            mhpd.TitlePosition = model.TitlePosition;
            mhpd.Sequence = model.Sequence;
            mhpd.Columns = model.Columns;
            mhpd.RedirectTo = model.RedirectTo;
            mhpd.CategoryId = model.CategoryId;
            mhpd.BrandIds = model.BrandIds;
            mhpd.SizeIds = model.SizeIds;
            mhpd.ColorIds = model.ColorIds;
            mhpd.SpecificationIds = model.SpecificationIds;
            mhpd.CollectionId = model.CollectionId;
            mhpd.ProductId = model.ProductId;
            mhpd.StaticPageId = model.StaticPageId;
            mhpd.LendingPageId = model.LendingPageId;
            mhpd.CustomLinks = model.CustomLinks;
            mhpd.TitleColor = model.TitleColor;
            mhpd.SubTitleColor = model.SubTitleColor;
            mhpd.TitleSize = model.TitleSize;
            mhpd.SubTitleSize = model.SubTitleSize;
            mhpd.ItalicSubTitle = model.ItalicSubTitle;
            mhpd.ItalicTitle = model.ItalicTitle;
            mhpd.Description = model.Description;
            mhpd.SliderType = model.SliderType;
            mhpd.VideoLinkType = model.VideoLinkType;
            mhpd.VideoId = model.VideoId;
            mhpd.Name = model.Name;
            mhpd.AssignCity = model.AssignCity;
            mhpd.AssignState = model.AssignState;
            mhpd.AssignCountry = model.AssignCountry;
            mhpd.Status = model.Status;
            mhpd.ModifiedAt = DateTime.Now;
            mhpd.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            var response = helper.ApiCall(URL, EndPoints.ManageHomePageDetails, "PUT", mhpd);
            baseResponse = baseResponse.JsonParseInputResponse(response);

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageHomePageDetails + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageHomePageDetailsLibrary> tempList = baseResponse.Data as List<ManageHomePageDetailsLibrary> ?? new List<ManageHomePageDetailsLibrary>();
            if (tempList.Any())
            {
                var data = tempList.FirstOrDefault();
                if (!string.IsNullOrEmpty(data.Image))
                {
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    imageUpload.RemoveDocFile(data.Image, "HomePages");
                }

                var response = helper.ApiCall(URL, EndPoints.ManageHomePageDetails + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageHomePageDetails + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10, int? sectionId = null)
        {
            string url = "?PageIndex=" + pageIndex + "&PageSize=" + pageSize;
            if (sectionId.HasValue && sectionId.Value > 0)
            {
                url += "&SectionId=" + sectionId.Value;
            }

            var response = helper.ApiCall(URL, EndPoints.ManageHomePageDetails + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, string? status = null, int? pageIndex = 1, int? pageSize = 10, int? sectionId = null)
        {
            string url = "?PageIndex=" + pageIndex + "&PageSize=" + pageSize;
            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                url += "&SearchText=" + HttpUtility.UrlEncode(searchtext);
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                url += "&Status=" + HttpUtility.UrlEncode(status);
            }
            if (sectionId.HasValue && sectionId.Value > 0)
            {
                url += "&SectionId=" + sectionId.Value;
            }

            var response = helper.ApiCall(URL, EndPoints.ManageHomePageDetails + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("layoutTypeDetailsId&sectionId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByLayoutTypeDetailsId(int? layoutTypeDetailsId = 0, int? sectionId = 0)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageHomePageDetails + "?LayoutTypeDetailsId=" + layoutTypeDetailsId + "&sectionId=" + sectionId, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [NonAction]
        public string UploadDoc(string Name, IFormFile FileName)
        {
            try
            {
                var file = FileName;

                var folderName = Path.Combine("Resources", "HomePages");

                if (file != null)
                {
                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + "_" + a;
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    fileName = imageUpload.UploadImageAndDocs(fileName, folderName, FileName);
                    return fileName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NonAction]
        public string UpdateDocFile(string OldName, string Name, IFormFile? FileName)
        {
            try
            {
                if (FileName != null)
                {
                    var file = FileName;

                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + "_" + a;
                    var folderName = Path.Combine("Resources", "HomePages");
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    fileName = imageUpload.UpdateUploadImageAndDocs(OldName, fileName, folderName, FileName);

                    return fileName;
                }
                else
                {
                    string fileName = null;
                    if (OldName != null)
                    {
                        fileName = OldName;

                    }
                    return fileName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
