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
    public class ManageLayoutsController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ManageLayoutsLibrary> baseResponse = new BaseResponse<ManageLayoutsLibrary>();
        private ApiHelper helper;
        public ManageLayoutsController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create([FromForm] ManageLayoutsDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLayouts + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageLayoutsLibrary> tempList = (List<ManageLayoutsLibrary>)baseResponse.Data;

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ManageLayoutsLibrary manageLayouts = new ManageLayoutsLibrary();
                manageLayouts.Name = model.Name;
                manageLayouts.ImageUrl = UploadDoc(model.Name, model.ImageFile);
                manageLayouts.CreatedAt = DateTime.Now;
                manageLayouts.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageLayouts, "POST", manageLayouts);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update([FromForm] ManageLayoutsDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLayouts + "?Name=" + model.Name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageLayoutsLibrary> tempList = (List<ManageLayoutsLibrary>)baseResponse.Data;

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageLayouts + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageLayoutsLibrary record = (ManageLayoutsLibrary)baseResponse.Data;
                string OldName = record.ImageUrl;
                record.Name = model.Name;
                record.ImageUrl = UpdateDocFile(OldName, model.Name, model.ImageFile);
                record.ModifiedAt = DateTime.Now;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageLayouts, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLayouts + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageLayoutsLibrary> tempList = (List<ManageLayoutsLibrary>)baseResponse.Data;
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypes + "?LayoutId=" + id, "GET", null);
                BaseResponse<ManageLayoutTypesLibrary> baseResponse1 = new BaseResponse<ManageLayoutTypesLibrary>();
                baseResponse1 = baseResponse1.JsonParseList(response);
                List<ManageLayoutTypesLibrary> tempList1 = (List<ManageLayoutTypesLibrary>)baseResponse1.Data;
                if (tempList1.Any())
                {
                    baseResponse = baseResponse.ChildExists();
                }
                else
                {
                    response = helper.ApiCall(URL, EndPoints.ManageLayouts + "?Id=" + id, "DELETE", null);
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
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> Get(int? pageindex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLayouts + "?PageIndex=" + pageindex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLayouts + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }

            var response = helper.ApiCall(URL, EndPoints.ManageLayouts + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }


        [NonAction]
        public string UploadDoc(string Name, IFormFile FileName)
        {
            try
            {
                var file = FileName;

                var folderName = Path.Combine("Resources", "ManageLayouts");
                if (file != null)
                {
                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + "_Layout" + a;
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
                    var fileName = Name + "_Layout" + a;
                    var folderName = Path.Combine("Resources", "ManageLayouts");
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
