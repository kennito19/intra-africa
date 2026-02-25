using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageSubMenuController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string UserURL = string.Empty;

        BaseResponse<ManageSubMenu> baseResponse = new BaseResponse<ManageSubMenu>();
        private ApiHelper helper;

        public ManageSubMenuController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            UserURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create([FromForm] ManageSubMenuDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Name=" + model.Name + "&headerId=" + model.HeaderId + "&getParent=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageSubMenu> tempList = baseResponse.Data as List<ManageSubMenu> ?? new List<ManageSubMenu>();

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ManageSubMenu manageSubMenu = new ManageSubMenu();
                manageSubMenu.MenuType = model.MenuType;
                manageSubMenu.HeaderId = model.HeaderId;
                manageSubMenu.ParentId = null;
                manageSubMenu.Name = model.Name;
                manageSubMenu.Image = UploadDoc(model.Name, model.ImageFile);
                manageSubMenu.ImageAlt = model.ImageAlt;
                manageSubMenu.HasLink = model.HasLink;
                manageSubMenu.RedirectTo = model.RedirectTo;
                manageSubMenu.LendingPageId = model.LendingPageId;
                manageSubMenu.CategoryId = model.CategoryId;
                manageSubMenu.StaticPageId = model.StaticPageId;
                manageSubMenu.CollectionId = model.CollectionId;
                manageSubMenu.CustomLink = model.CustomLink;
                manageSubMenu.Sizes = model.Sizes;
                manageSubMenu.Colors = model.Colors;
                manageSubMenu.Specifications = model.Specifications;
                manageSubMenu.Brands = model.Brands;
                manageSubMenu.Sequence = model.Sequence;
                manageSubMenu.CreatedAt = DateTime.Now;
                manageSubMenu.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageSubMenu, "POST", manageSubMenu);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPost("categoryWiseSubMenu")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> categoryWiseSubMenu(CategoryWiseSubMenuCreateDTO model)
        {
            List<int> categoryIds = model.CategoryIds;
            List<int> brandIds = null;
            DeleteRecord(categoryIds, brandIds, model.HeaderId);

            ManageSubMenu manageSubMenu = new ManageSubMenu();
            manageSubMenu.MenuType = "CategoryWise";
            manageSubMenu.HeaderId = model.HeaderId;
            manageSubMenu.RedirectTo = "Product List";
            manageSubMenu.HasLink = true;
            manageSubMenu.CreatedAt = DateTime.Now;
            manageSubMenu.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

            for (int i = 0; i < categoryIds.Count; i++)
            {
                BaseResponse<CategoryLibrary> CatbaseResponse = new BaseResponse<CategoryLibrary>();

                var CategoryId = categoryIds[i];

                var category = helper.ApiCall(URL, EndPoints.Category + "?Id=" + CategoryId, "GET", null);
                var categorybaseResponse = CatbaseResponse.JsonParseRecord(category);
                CategoryLibrary categoryDetails = categorybaseResponse.Data as CategoryLibrary;
                manageSubMenu.ImageAlt = categoryDetails.Name;
                manageSubMenu.Image = categoryDetails.Image;
                manageSubMenu.Sequence = i + 1;
                manageSubMenu.CategoryId = CategoryId;
                manageSubMenu.Name = categoryDetails.Name;

                var response = helper.ApiCall(URL, EndPoints.ManageSubMenu, "POST", manageSubMenu);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPost("brandWiseSubMenu")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> brandWiseSubMenu(BrandWiseSubMenuCreateDTO model)
        {
            List<int> categoryIds = null;
            List<int> brandIds = model.BrandId;
            DeleteRecord(categoryIds, brandIds, model.HeaderId);

            ManageSubMenu manageSubMenu = new ManageSubMenu();
            manageSubMenu.MenuType = "BrandWise";
            manageSubMenu.HeaderId = model.HeaderId;
            manageSubMenu.RedirectTo = "Brand List";
            manageSubMenu.HasLink = true;
            manageSubMenu.CreatedAt = DateTime.Now;
            manageSubMenu.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

            for (int i = 0; i < brandIds.Count; i++)
            {
                BaseResponse<BrandLibrary> BrandbaseResponse = new BaseResponse<BrandLibrary>();

                var brandId = brandIds[i];

                var brand = helper.ApiCall(UserURL, EndPoints.Brand + "?Id=" + brandId, "GET", null);
                var brandbaseResponse = BrandbaseResponse.JsonParseRecord(brand);
                BrandLibrary brandDetails = brandbaseResponse.Data as BrandLibrary;
                manageSubMenu.Sequence = i + 1;
                manageSubMenu.Brands = Convert.ToString(brandId);
                manageSubMenu.Name = brandDetails.Name;

                var response = helper.ApiCall(URL, EndPoints.ManageSubMenu, "POST", manageSubMenu);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update([FromForm] ManageSubMenuDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Name=" + model.Name + "&headerId=" + model.HeaderId + "&getParent=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageSubMenu> tempList = baseResponse.Data as List<ManageSubMenu> ?? new List<ManageSubMenu>();

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageSubMenu record = baseResponse.Data as ManageSubMenu;
                record.MenuType = model.MenuType;
                record.HeaderId = model.HeaderId;
                record.ParentId = null;
                record.Name = model.Name;
                if (model.IsImageAvailable)
                {
                    record.Image = UpdateDocFile(record.Image, model.Name, model.ImageFile);
                }
                else
                {
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    imageUpload.RemoveDocFile(record.Image, "ManageSubMenu");
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
            var temp = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageSubMenu> tempList = baseResponse.Data as List<ManageSubMenu> ?? new List<ManageSubMenu>();
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?parentId=" + id + "&getChild=" + true, "GET", null);
                BaseResponse<ManageChildMenu> baseResponse1 = new BaseResponse<ManageChildMenu>();
                baseResponse1 = baseResponse1.JsonParseList(response);
                List<ManageChildMenu> tempList1 = baseResponse1.Data as List<ManageChildMenu> ?? new List<ManageChildMenu>();
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
                        imageUpload.RemoveDocFile(data.Image, "ManageSubMenu");
                    }

                    response = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Id=" + id, "DELETE", null);
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
        public ActionResult<ApiHelper> GetById(int id, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Id=" + id + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&getParent=" + true, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("byHeaderId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByHeaderId(int headerId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?headerId=" + headerId + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize , "GET", null);
            //return Ok(baseResponse.JsonParseList(response));
            baseResponse = baseResponse.JsonParseList(response);
            List<ManageSubMenu> subMenus = baseResponse.Data as List<ManageSubMenu> ?? new List<ManageSubMenu>();

            var result = subMenus.Where(x => x.ParentId == null).Select(x => new
            {
                Id = x.Id,
                MenuType = x.MenuType,
                HeaderId = x.HeaderId,
                ParentId = x.ParentId,
                Name = x.Name,
                Image = x.Image,
                ImageAlt = x.ImageAlt,
                HasLink = x.HasLink,
                RedirectTo = x.RedirectTo,
                LendingPageId = x.LendingPageId,
                CategoryId = x.CategoryId,
                StaticPageId = x.StaticPageId,
                CollectionId = x.CollectionId,
                CustomLink = x.CustomLink,
                Sizes = x.Sizes,
                Colors = x.Colors,
                Specifications = x.Specifications,
                Brands = x.Brands,
                Sequence = x.Sequence,
                CreatedBy = x.CreatedBy,
                CreatedAt = x.CreatedAt,
                ModifiedAt = x.ModifiedAt,
                ModifiedBy = x.ModifiedBy,
                ChildMenu = subMenus.Where(detail => detail.ParentId == x.Id)
                .Select(y => new
                {
                    Id = y.Id,
                    MenuType = y.MenuType,
                    HeaderId = y.HeaderId,
                    ParentId = y.ParentId,
                    Name = y.Name,
                    Image = y.Image,
                    ImageAlt = y.ImageAlt,
                    HasLink = y.HasLink,
                    RedirectTo = y.RedirectTo,
                    LendingPageId = y.LendingPageId,
                    CategoryId = y.CategoryId,
                    StaticPageId = y.StaticPageId,
                    CollectionId = y.CollectionId,
                    CustomLink = y.CustomLink,
                    Sizes = y.Sizes,
                    Colors = y.Colors,
                    Specifications = y.Specifications,
                    Brands = y.Brands,
                    Sequence = y.Sequence,
                    CreatedBy = y.CreatedBy,
                    CreatedAt = y.CreatedAt,
                    ModifiedAt = y.ModifiedAt,
                    ModifiedBy = y.ModifiedBy
                }).OrderBy(x => x.Sequence)
                .ToList(),
            });
            baseResponse.Data = result.OrderBy(x => x.Sequence);
            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchText, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Searchtext=" + HttpUtility.UrlEncode(searchText) + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&getParent=" + true, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [NonAction]
        public string UploadDoc(string Name, IFormFile FileName)
        {
            try
            {
                var file = FileName;

                var folderName = Path.Combine("Resources", "ManageSubMenu");
                if (file != null)
                {
                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + "_SubMenu_" + a;
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
                    var fileName = Name + "_SubMenu_" + a;
                    var folderName = Path.Combine("Resources", "ManageSubMenu");
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

        [NonAction]
        public IActionResult DeleteRecord(List<int> categoryIds, List<int> brandIds, int headerId)
        {
            if (categoryIds != null)
            {
                foreach (int categoryId in categoryIds)
                {
                    var temp = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?CategoryId=" + categoryId + "&headerId=" + headerId, "GET", null);
                    baseResponse = baseResponse.JsonParseList(temp);
                    List<ManageSubMenu> tempList = baseResponse.Data as List<ManageSubMenu> ?? new List<ManageSubMenu>();
                    if (tempList.Any())
                    {
                        var response = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Id=" + tempList[0].Id, "DELETE", null);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                    else
                    {
                        baseResponse = baseResponse.NotExist();
                    }
                }
            }
            else if (brandIds != null)
            {
                foreach (int brandId in brandIds)
                {
                    var temp = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Brands=" + brandId + "&headerId=" + headerId, "GET", null);
                    baseResponse = baseResponse.JsonParseList(temp);
                    List<ManageSubMenu> tempList = baseResponse.Data as List<ManageSubMenu> ?? new List<ManageSubMenu>();
                    if (tempList.Any())
                    {
                        var response = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Id=" + tempList[0].Id, "DELETE", null);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                    else
                    {
                        baseResponse = baseResponse.NotExist();
                    }
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpPut("ChangeSequence")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> UpdateSequence(List<ChangeSequenceDTO> models)
        {
            foreach (var model in models)
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Id=" + model.Id, "GET", null);
                var parentBaseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageSubMenu parentSequence = parentBaseResponse.Data as ManageSubMenu;
                parentSequence.Sequence = model.Sequence;
                parentSequence.ParentId = null;
                parentSequence.ModifiedAt = DateTime.Now;
                parentSequence.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageSubMenu, "PUT", parentSequence);
                baseResponse = baseResponse.JsonParseInputResponse(response);

                List<ChangeChildSequenceDTO> changeChildSequences = model.ChildSequence;

                for (int i = 0; i < changeChildSequences.Count; i++)
                {
                    var Id = changeChildSequences[i];

                    recordCall = helper.ApiCall(URL, EndPoints.ManageSubMenu + "?Id=" + model.ChildSequence[i].Id, "GET", null);
                    baseResponse = baseResponse.JsonParseRecord(recordCall);
                    ManageSubMenu childSequence = baseResponse.Data as ManageSubMenu;
                    childSequence.ParentId = model.ChildSequence[i].ParentId;
                    childSequence.Sequence = model.ChildSequence[i].Sequence;
                    childSequence.ModifiedAt = DateTime.Now;
                    childSequence.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                    response = helper.ApiCall(URL, EndPoints.ManageSubMenu, "PUT", childSequence);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            return Ok(baseResponse);
        }
    }
}
