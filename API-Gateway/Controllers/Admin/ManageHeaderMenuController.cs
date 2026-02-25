using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageHeaderMenuController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ManageHeaderMenu> baseResponse = new BaseResponse<ManageHeaderMenu>();
        private ApiHelper helper;
        public ManageHeaderMenuController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create([FromForm] ManageHeaderMenuDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageHeaderMenu + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageHeaderMenu> tempList = baseResponse.Data as List<ManageHeaderMenu> ?? new List<ManageHeaderMenu>();

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                bool AllowColor = Convert.ToBoolean(_configuration.GetValue<string>("allow_color"));

                ManageHeaderMenu manageHeader = new ManageHeaderMenu();
                manageHeader.Name = model.Name;
                manageHeader.Image = UploadDoc(model.Name, model.ImageFile);
                manageHeader.ImageAlt = model.ImageAlt;
                manageHeader.HasLink = model.HasLink;
                manageHeader.RedirectTo = model.RedirectTo;
                manageHeader.LendingPageId = model.LendingPageId;
                manageHeader.CategoryId = model.CategoryId;
                manageHeader.StaticPageId = model.StaticPageId;
                manageHeader.CollectionId = model.CollectionId;
                manageHeader.CustomLink = model.CustomLink;
                manageHeader.Sequence = model.Sequence;
                if (AllowColor == true)
                {
                    manageHeader.color = model.color;
                }
                manageHeader.CreatedAt = DateTime.Now;
                manageHeader.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageHeaderMenu, "POST", manageHeader);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update([FromForm] ManageHeaderMenuDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageHeaderMenu + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageHeaderMenu> tempList = baseResponse.Data as List<ManageHeaderMenu> ?? new List<ManageHeaderMenu>();

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                bool AllowColor = Convert.ToBoolean(_configuration.GetValue<string>("allow_color"));

                var recordCall = helper.ApiCall(URL, EndPoints.ManageHeaderMenu + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageHeaderMenu record = baseResponse.Data as ManageHeaderMenu;
                string OldName = record.Image;
                record.Name = model.Name;
                if (model.IsImageAvailable)
                {
                    record.Image = UpdateDocFile(OldName, model.Name, model.ImageFile);
                }
                else
                {
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    imageUpload.RemoveDocFile(OldName, "ManageHeaderMenu");
                    record.Image = null;
                }
                record.ImageAlt = model.ImageAlt;
                record.HasLink = model.HasLink;
                record.RedirectTo = model.RedirectTo;
                record.LendingPageId = model.LendingPageId;
                record.CategoryId = model.CategoryId;
                record.StaticPageId = model.StaticPageId;
                record.CollectionId = model.CollectionId;
                record.CustomLink = model.CustomLink;
                record.Sequence = model.Sequence;
                if (AllowColor == true)
                {
                    record.color = model.color;
                }
                record.ModifiedAt = DateTime.Now;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageHeaderMenu, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageHeaderMenu + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageHeaderMenu> tempList = baseResponse.Data as List<ManageHeaderMenu> ?? new List<ManageHeaderMenu>();
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?headerId=" + id + "&getParent=" + true, "GET", null);
                BaseResponse<ManageSubMenu> baseResponse1 = new BaseResponse<ManageSubMenu>();
                baseResponse1 = baseResponse1.JsonParseList(response);
                List<ManageSubMenu> tempList1 = baseResponse1.Data as List<ManageSubMenu> ?? new List<ManageSubMenu>();
                if (tempList1.Any())
                {
                    baseResponse = baseResponse.ChildExists();
                }
                else
                {
                    var data = tempList.FirstOrDefault();
                    if (!string.IsNullOrEmpty(data.Image))
                    {
                        ImageUpload imageUpload = new ImageUpload(_configuration);
                        imageUpload.RemoveDocFile(data.Image, "ManageHeaderMenu");
                    }

                    response = helper.ApiCall(URL, EndPoints.ManageHeaderMenu + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
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
            var response = helper.ApiCall(URL, EndPoints.ManageHeaderMenu + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(URL, EndPoints.ManageHeaderMenu + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [NonAction]
        public string UploadDoc(string Name, IFormFile FileName)
        {
            try
            {
                var file = FileName;

                var folderName = Path.Combine("Resources", "ManageHeaderMenu");
                if (file != null)
                {
                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + "_Header_" + a;
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
                    var fileName = Name + "_Header_" + a;
                    var folderName = Path.Combine("Resources", "ManageHeaderMenu");
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
