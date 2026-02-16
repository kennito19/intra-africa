using API_Gateway.Helper;
using API_Gateway.Models;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using API_Gateway.Common;
using System.Web;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]

    public class MainCategoryController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public static string URL = string.Empty;
        Response res = new Response(); // This line is mendatory for the non-action methods and functions do not remove this. (Sahil)
        BaseResponse<CategoryLibrary> baseResponse = new BaseResponse<CategoryLibrary>();
        private ApiHelper helper;
        public MainCategoryController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create([FromForm] MainCategoryDTO model)
        {
             
            var temp = helper.ApiCall(URL, EndPoints.Category + "?Name=" + HttpUtility.UrlEncode(model.Name.Replace("'", "''")) + "&GetParent=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<CategoryLibrary> categories = (List<CategoryLibrary>)baseResponse.Data;
            if (categories.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                bool AllowColorInCategory = Convert.ToBoolean(_configuration.GetValue<string>("Allow_Color_In_Category"));

                CategoryLibrary cat = new CategoryLibrary();
                cat.Name = model.Name;
                cat.CurrentLevel = 1;
                cat.Image = UploadDoc(model.Name, model.FileName);
                cat.MetaTitles = model.MetaTitles;
                cat.MetaDescription = model.MetaDescription;
                cat.MetaKeywords = model.MetaKeywords;
                cat.Status = model.Status;
                if (AllowColorInCategory == true)
                {
                    cat.Color = model.Color;
                }
                cat.Title = model.Title;
                cat.SubTitle = model.SubTitle;
                cat.Description = model.Description;
                cat.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.Category, "POST", cat);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                int i = (int)baseResponse.Data;
                if (i != 0)
                {
                    UpdatePathId(i);
                }
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update([FromForm] MainCategoryDTO model)
        {
             
            var temp = helper.ApiCall(URL, EndPoints.Category + "?Name=" + HttpUtility.UrlEncode(model.Name.Replace("'", "''")) + "&Getparent=" + true, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<CategoryLibrary> templist = (List<CategoryLibrary>)baseResponse.Data;
            if (templist.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                bool AllowColorInCategory = Convert.ToBoolean(_configuration.GetValue<string>("Allow_Color_In_Category"));

                var imagedoc = helper.ApiCall(URL, EndPoints.Category + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(imagedoc);
                CategoryLibrary cat = (CategoryLibrary)baseResponse.Data;
                string OldName = cat.Image;
                string? PathIds = model.Id.ToString();
                cat.Name = model.Name;
                cat.CurrentLevel = 1;
                cat.MetaTitles = model.MetaTitles;
                cat.MetaDescription = model.MetaDescription;
                cat.MetaKeywords = model.MetaKeywords;
                cat.Status = model.Status;
                if (AllowColorInCategory == true)
                {
                    cat.Color = model.Color;
                }
                cat.Title = model.Title;
                cat.SubTitle = model.SubTitle;
                cat.Description = model.Description;
                cat.PathIds = PathIds;
                cat.PathNames = model.Name;
                if (model.IsImageAvailable)
                {
                    cat.Image = UpdateDocFile(OldName, model.Name, model.FileName);
                }
                else
                {
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    imageUpload.RemoveDocFile(OldName, "CategoryImage");
                    cat.Image = null;
                }
                cat.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.Category, "PUT", cat);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                UpdateSubPath(cat.Id);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            try
            {
                var temp = helper.ApiCall(URL, EndPoints.Category + "?ParentID=" + id, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<CategoryLibrary> tmpList = (List<CategoryLibrary>)baseResponse.Data;
                if (!tmpList.Any())
                {

                    var tempAssignSpecToCat = helper.ApiCall(URL, EndPoints.AssignSpecToCat + "?CategoryID=" + id, "GET", null);
                    BaseResponse<AssignSpecificationToCategoryLibrary> baseAssignSpecToCat = new BaseResponse<AssignSpecificationToCategoryLibrary>();
                    var AssignSpecToCat = baseAssignSpecToCat.JsonParseList(tempAssignSpecToCat);
                    List<AssignSpecificationToCategoryLibrary> assignSpecToCat = (List<AssignSpecificationToCategoryLibrary>)AssignSpecToCat.Data;

                    var tempAssignSizeValToCat = helper.ApiCall(URL, EndPoints.AssignSizeValueToCategory + "?CategoryID=" + id, "GET", null);
                    BaseResponse<AssignSizeValueToCategory> baseAssignSizeValToCat = new BaseResponse<AssignSizeValueToCategory>();
                    var AssignSizeValToCat = baseAssignSizeValToCat.JsonParseList(tempAssignSizeValToCat);
                    List<AssignSizeValueToCategory> assignSizeValToCat = (List<AssignSizeValueToCategory>)AssignSizeValToCat.Data;

                    var tempReturnPolicyToCat = helper.ApiCall(URL, EndPoints.AssignReturnPolicyToCatagory + "?CategoryID=" + id, "GET", null);
                    BaseResponse<AssignReturnPolicyToCatagoryLibrary> baseReturnPolicyToCat = new BaseResponse<AssignReturnPolicyToCatagoryLibrary>();
                    var returnPolicyToCat = baseReturnPolicyToCat.JsonParseList(tempReturnPolicyToCat);
                    List<AssignReturnPolicyToCatagoryLibrary> assignReturnPolicyToCat = (List<AssignReturnPolicyToCatagoryLibrary>)returnPolicyToCat.Data;

                    var tempProduct = helper.ApiCall(URL, EndPoints.Product + "?CategoryId=" + id, "GET", null);
                    BaseResponse<Products> baseProducts = new BaseResponse<Products>();
                    var products = baseProducts.JsonParseList(tempProduct);
                    List<Products> product = (List<Products>)products.Data;

                    if (assignSpecToCat.Any() || assignSizeValToCat.Any() || assignReturnPolicyToCat.Any() || product.Any())
                    {
                        if (assignSpecToCat.Any())
                        {
                            baseResponse = baseResponse.ChildAlreadyExists("AssignSpecificationToCategory", "MainCategory");
                        }

                        if (assignSizeValToCat.Any())
                        {
                            baseResponse = baseResponse.ChildAlreadyExists("AssignSizeValesToCategory", "MainCategory");
                        }

                        if (assignReturnPolicyToCat.Any())
                        {
                            baseResponse = baseResponse.ChildAlreadyExists("AssignReturnPolicyToCategory", "MainCategory");
                        }

                        if (product.Any())
                        {
                            baseResponse = baseResponse.ChildAlreadyExists("Products", "MainCategory");
                        }
                    }
                    else
                    {
                        var response = helper.ApiCall(URL, EndPoints.Category + "?Id=" + id, "DELETE", null);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                }
                else
                {
                    baseResponse = baseResponse.ChildExists();
                }
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occured: " + ex.Message);
            }
        }

        [HttpGet("getAllCategory")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetAllCategory(int? pageindex = 1, int? PageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "?&Status=" + "Active" + "&PageIndex=" + pageindex + "&PageSize=" + PageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("getEndCategory")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetEndCategory(string? status = null)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "?PageIndex=" + 0 + "&PageSize=" + 0, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            var categories = (List<CategoryLibrary>)baseResponse.Data;
            var bindcategories = (List<CategoryLibrary>)baseResponse.Data;

            var result = categories
                .Where(c => c.IsDeleted == false)
                .Where(c => !bindcategories.Any(tt => tt.ParentId == c.Id))
                .Where(c => status == null || c.Status == status)
                .ToList();

            if (result.Any())
            {
                var json = JsonConvert.SerializeObject(result);
                var jsoncategory = JsonConvert.DeserializeObject<List<EndCategoryDTO>>(json);

                baseResponse.code = 200;
                baseResponse.Data = jsoncategory;
                baseResponse.Message = "Category Bind Successfully";
                baseResponse.pagination.RecordCount = result.Count;
                return Ok(baseResponse);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, string? status = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }

            if (!string.IsNullOrEmpty(status))
            {
                url += "&Status=" + status;
            }
            var response = helper.ApiCall(URL, EndPoints.Category + "?GetParent=" + true + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        #region NonAction Methods
        [NonAction]
        public ActionResult<Response<CategoryLibrary>> UpdatePathId(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "?Id=" + id, "GET", null);
            var tmp = new CategoryLibrary();
            if (response.IsSuccessStatusCode)
            {
                res = JsonDeserialize(response);
                tmp = res.data.FirstOrDefault();
                if (tmp.ParentId == null)
                {
                    tmp.PathIds = tmp.Id.ToString();
                    tmp.PathNames = tmp.Name;
                }
                else
                {

                    tmp.PathIds = tmp.ParentPathIds + ">" + tmp.Id;
                    tmp.PathNames = tmp.ParentPathNames + ">" + tmp.Name;
                }

                var update = helper.ApiCall(URL, EndPoints.Category, "PUT", tmp);
                if (update.IsSuccessStatusCode)
                {
                    return Ok(update.Content.ReadAsStringAsync().Result);
                }
            }
            return Ok(res);
        }

        [NonAction]
        public string UploadDoc(string Name, IFormFile FileName)
        {
            try
            {
                var file = FileName;

                var folderName = Path.Combine("Resources", "Images");
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
                    var folderName = Path.Combine("Resources", "Images");
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    fileName = imageUpload.UpdateUploadImageAndDocs(OldName, fileName, folderName, FileName);

                    return fileName;
                }
                else
                {
                    string fileName = null;
                    if (OldName != string.Empty)
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
        public Response JsonDeserialize(HttpResponseMessage httpresponse)
        {
            return JsonConvert.DeserializeObject<Response>(httpresponse.Content.ReadAsStringAsync().Result);
        }

        [NonAction]
        public ActionResult<Response> UpdateSubPath(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "?ParentID=" + id, "GET");

            if (response.IsSuccessStatusCode)
            {
                res = JsonDeserialize(response);
                List<CategoryLibrary> spec = new List<CategoryLibrary>();
                spec = res.data;
                foreach (var item in spec)
                {
                    UpdatePathId(item.Id);
                    UpdateSubPath(item.Id);
                }
            }
            return null;
        }
        public class Response
        {
            public int code { get; set; }
            public List<CategoryLibrary> data { get; set; }
            public string message { get; set; }
        }

        public class SaveResponse
        {
            public int code { get; set; }
            public int data { get; set; }
            public string message { get; set; }
        }

        #endregion
    }
}
