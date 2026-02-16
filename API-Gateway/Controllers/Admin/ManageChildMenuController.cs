using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageChildMenuController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ManageChildMenu> baseResponse = new BaseResponse<ManageChildMenu>();
        private ApiHelper helper;
        public ManageChildMenuController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create([FromForm] ManageChildMenuDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Name=" + model.Name + "&getChild=" + true + "&parentId=" + model.ParentId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageChildMenu> tempList = (List<ManageChildMenu>)baseResponse.Data;

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ManageChildMenu manageChildMenu = new ManageChildMenu();
                manageChildMenu.MenuType = model.MenuType;
                manageChildMenu.HeaderId = model.HeaderId;
                manageChildMenu.ParentId = model.ParentId;
                manageChildMenu.Name = model.Name;
                manageChildMenu.Image = UploadDoc(model.Name, model.ImageFile);
                manageChildMenu.ImageAlt = model.ImageAlt;
                manageChildMenu.HasLink = model.HasLink;
                manageChildMenu.RedirectTo = model.RedirectTo;
                manageChildMenu.LendingPageId = model.LendingPageId;
                manageChildMenu.CategoryId = model.CategoryId;
                manageChildMenu.StaticPageId = model.StaticPageId;
                manageChildMenu.CollectionId = model.CollectionId;
                manageChildMenu.CustomLink = model.CustomLink;
                manageChildMenu.Sizes = model.Sizes;
                manageChildMenu.Colors = model.Colors;
                manageChildMenu.Specifications = model.Specifications;
                manageChildMenu.Brands = model.Brands;
                manageChildMenu.Sequence = model.Sequence;
                manageChildMenu.CreatedAt = DateTime.Now;
                manageChildMenu.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageSubMenu, "POST", manageChildMenu);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update([FromForm] ManageChildMenuDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Name=" + model.Name + "&getChild=" + true + "&parentId=" + model.ParentId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageChildMenu> tempList = (List<ManageChildMenu>)baseResponse.Data;

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Id=" + model.Id + "&getChild=" + true, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageChildMenu record = (ManageChildMenu)baseResponse.Data;
                record.MenuType = model.MenuType;
                record.HeaderId = model.HeaderId;
                record.ParentId = model.ParentId;
                record.Name = model.Name;
                if (model.IsImageAvailable)
                {
                    record.Image = UpdateDocFile(record.Image, model.Name, model.ImageFile);
                }
                else
                {
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    imageUpload.RemoveDocFile(record.Image, "ManageChildMenu");
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
                record.Sizes = model.Sizes;
                record.Colors = model.Colors;
                record.Specifications = model.Specifications;
                record.Brands = model.Brands;
                record.Sequence = model.Sequence;
                record.ModifiedAt = DateTime.Now;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageSubMenu, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Id=" + id + "&getChild=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageChildMenu> tempList = (List<ManageChildMenu>)baseResponse.Data;
            if (tempList.Any())
            {
                var data = tempList.FirstOrDefault();
                if (!string.IsNullOrEmpty(data.Image))
                {
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    imageUpload.RemoveDocFile(data.Image, "ManageChildMenu");
                }

                var response = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Id=" + id + "&getChild=" + true, "DELETE", null);
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
            var response = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?PageIndex=" + pageindex + "&PageSize=" + pageSize + "&getChild=" + true, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Id=" + id + "&getChild=" + true, "GET" , null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchText, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Searchtext=" + HttpUtility.UrlEncode(searchText) + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&getChild=" + true, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [NonAction]
        public string UploadDoc(string Name, IFormFile FileName)
        {
            try
            {
                var file = FileName;

                var folderName = Path.Combine("Resources", "ManageChildMenu");
                if (file != null)
                {
                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + "_ChildMenu_" + a;
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
                    var fileName = Name + "_ChildMenu_" + a;
                    var folderName = Path.Combine("Resources", "ManageChildMenu");
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
