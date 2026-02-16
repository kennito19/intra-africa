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
    public class LendingPageSectionDetailsController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<LendingPageSectionDetails> baseResponse = new BaseResponse<LendingPageSectionDetails>();
        private ApiHelper helper;
        public LendingPageSectionDetailsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create([FromForm] LendingPageSectionDetailDTO model)
        {
            LendingPageSectionDetails LendingPageSectionDetail = new LendingPageSectionDetails();
            LendingPageSectionDetail.LendingPageSectionId = model.LendingPageSectionId;
            LendingPageSectionDetail.LayoutTypeDetailsId = model.LayoutTypeDetailsId;
            LendingPageSectionDetail.OptionId = model.OptionId;
            LendingPageSectionDetail.Image = UploadDoc(model.LendingPageSectionName, model.ImageFile);// UploadDoc(model.Name, model.FileName);
            LendingPageSectionDetail.ImageAlt = model.ImageAlt;
            LendingPageSectionDetail.IsTitleVisible = model.IsTitleVisible;
            LendingPageSectionDetail.Title = model.Title;
            LendingPageSectionDetail.SubTitle = model.SubTitle;
            LendingPageSectionDetail.TitlePosition = model.TitlePosition;
            LendingPageSectionDetail.Sequence = model.Sequence;
            LendingPageSectionDetail.Columns = model.Columns;
            LendingPageSectionDetail.RedirectTo = model.RedirectTo;
            LendingPageSectionDetail.CategoryId = model.CategoryId;
            LendingPageSectionDetail.BrandIds = model.BrandIds;
            LendingPageSectionDetail.SizeIds = model.SizeIds;
            LendingPageSectionDetail.SpecificationIds = model.SpecificationIds;
            LendingPageSectionDetail.ColorIds = model.ColorIds;
            LendingPageSectionDetail.CollectionId = model.CollectionId;
            LendingPageSectionDetail.ProductId = model.ProductId;
            LendingPageSectionDetail.StaticPageId = model.StaticPageId;
            LendingPageSectionDetail.CustomLinks = model.CustomLinks;
            LendingPageSectionDetail.TitleColor = model.TitleColor;
            LendingPageSectionDetail.SubTitleColor = model.SubTitleColor;
            LendingPageSectionDetail.TitleSize = model.TitleSize;
            LendingPageSectionDetail.SubTitleSize = model.SubTitleSize;
            LendingPageSectionDetail.ItalicSubTitle = model.ItalicSubTitle;
            LendingPageSectionDetail.ItalicTitle = model.ItalicTitle;
            LendingPageSectionDetail.Description = model.Description;
            LendingPageSectionDetail.SliderType = model.SliderType;
            LendingPageSectionDetail.VideoLinkType = model.VideoLinkType;
            LendingPageSectionDetail.VideoId = model.VideoId;
            LendingPageSectionDetail.Name = model.Name;
            LendingPageSectionDetail.AssignCity = model.AssignCity;
            LendingPageSectionDetail.AssignState = model.AssignState;
            LendingPageSectionDetail.AssignCountry = model.AssignCountry;
            LendingPageSectionDetail.Status = model.Status;
            LendingPageSectionDetail.CreatedAt = DateTime.Now;
            LendingPageSectionDetail.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail, "POST", LendingPageSectionDetail);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return Ok(baseResponse);
        }
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update([FromForm] LendingPageSectionDetailDTO model)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail + "?Id=" + model.Id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            LendingPageSectionDetails abts = (LendingPageSectionDetails)baseResponse.Data;
            abts.LendingPageSectionId = model.LendingPageSectionId;
            abts.LayoutTypeDetailsId = model.LayoutTypeDetailsId;
            abts.OptionId = model.OptionId;
            abts.ImageAlt = model.ImageAlt;
            abts.IsTitleVisible = model.IsTitleVisible;
            abts.Title = model.Title;
            abts.SubTitle = model.SubTitle;
            abts.TitlePosition = model.TitlePosition;
            abts.Sequence = model.Sequence;
            abts.Columns = model.Columns;
            abts.RedirectTo = model.RedirectTo;
            abts.CategoryId = model.CategoryId;
            abts.BrandIds = model.BrandIds;
            abts.SizeIds = model.SizeIds;
            abts.SpecificationIds = model.SpecificationIds;
            abts.ColorIds = model.ColorIds;
            abts.CollectionId = model.CollectionId;
            abts.ProductId = model.ProductId;
            abts.StaticPageId = model.StaticPageId;
            abts.CustomLinks = model.CustomLinks;
            abts.TitleColor = model.TitleColor;
            abts.SubTitleColor = model.SubTitleColor;
            abts.TitleSize = model.TitleSize;
            abts.SubTitleSize = model.SubTitleSize;
            abts.ItalicSubTitle = model.ItalicSubTitle;
            abts.ItalicTitle = model.ItalicTitle;
            abts.Description = model.Description;
            abts.SliderType = model.SliderType;
            abts.VideoLinkType = model.VideoLinkType;
            abts.VideoId = model.VideoId;
            abts.Name = model.Name;
            abts.AssignCity = model.AssignCity;
            abts.AssignState = model.AssignState;
            abts.AssignCountry = model.AssignCountry;
            abts.Status = model.Status;

            abts.Image = UpdateDocFile(abts.Image, model.LendingPageSectionName, model.ImageFile);

            abts.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value; ;
            abts.ModifiedAt = DateTime.Now;

            response = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail, "PUT", abts);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<LendingPageSectionDetails> tempList = (List<LendingPageSectionDetails>)baseResponse.Data;

            if (tempList.Any())
            {
                var data = tempList.FirstOrDefault();
                if (!string.IsNullOrEmpty(data.Image))
                {
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    imageUpload.RemoveDocFile(data.Image, "LendingPageSections");
                }

                var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageindex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail + "?PageIndex=" + pageindex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("layoutTypeDetailsId&sectionId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByLayoutTypeDetailsId(int? layoutTypeDetailsId = 0, int? sectionId = 0)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLendingPageSectionsDetail + "?LayoutTypeDetailsId=" + layoutTypeDetailsId + "&LendingPageSectionId=" + sectionId, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [NonAction]
        public string UploadDoc(string Name, IFormFile FileName)
        {
            try
            {
                var file = FileName;

                var folderName = Path.Combine("Resources", "LendingPageSections");

                if (file != null)
                {
                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + a;
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
                    var fileName = Name + a;
                    var folderName = Path.Combine("Resources", "LendingPageSections");
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
