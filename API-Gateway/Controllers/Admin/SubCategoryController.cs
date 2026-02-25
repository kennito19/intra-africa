using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        private readonly IConfiguration _configuration;
        public static string URL = string.Empty;
        Response res = new Response(); // This line is mendatory for the non-action methods and functions do not remove this. (Sahil)
        BaseResponse<CategoryLibrary> baseResponse = new BaseResponse<CategoryLibrary>();

        public SubCategoryController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create([FromForm] SubCategoryDTO model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Name) || model.ParentId <= 0)
                {
                    return Ok(baseResponse.InvalidInput("Sub category name and parent category are required."));
                }

                var temp = helper.ApiCall(URL, EndPoints.Category + "?Name=" + HttpUtility.UrlEncode(model.Name.Replace("'", "''")) + "&ParentID=" + model.ParentId, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<CategoryLibrary> categories = baseResponse.Data as List<CategoryLibrary> ?? new List<CategoryLibrary>();
                if (categories.Any())
                {
                    baseResponse = baseResponse.AlreadyExists();
                }
                else
                {
                    bool AllowColorInCategory = Convert.ToBoolean(_configuration.GetValue<string>("Allow_Color_In_Category"));

                    CategoryLibrary cat = new CategoryLibrary();
                    cat.Name = model.Name;
                    cat.CurrentLevel = model.CurrentLevel;
                    cat.Image = UploadDoc(model.Name, model.FileName);
                    cat.MetaTitles = model.MetaTitles;
                    cat.MetaDescription = model.MetaDescription;
                    cat.MetaKeywords = model.MetaKeywords;
                    cat.Status = string.IsNullOrWhiteSpace(model.Status) ? "Active" : model.Status;
                    if (AllowColorInCategory == true)
                    {
                        cat.Color = model.Color;
                    }
                    cat.Title = model.Title;
                    cat.SubTitle = model.SubTitle;
                    cat.Description = model.Description;
                    cat.ParentId = model.ParentId;
                    cat.CreatedBy = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type.Equals("UserID"))?.Value ?? "system";

                    var response = helper.ApiCall(URL, EndPoints.Category, "POST", cat);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                    int insertedId = 0;
                    try
                    {
                        insertedId = Convert.ToInt32(baseResponse.Data);
                    }
                    catch
                    {
                        insertedId = 0;
                    }

                    if (insertedId != 0)
                    {
                        UpdatePathId(insertedId);
                    }
                }
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                return Ok(baseResponse.InvalidInput(ex.Message));
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update([FromForm] SubCategoryDTO model)
        {
            try
            {
                if (model == null || model.Id <= 0 || string.IsNullOrWhiteSpace(model.Name) || model.ParentId <= 0)
                {
                    return Ok(baseResponse.InvalidInput("Sub category id, name and parent category are required."));
                }

                var temp = helper.ApiCall(URL, EndPoints.Category + "?Name=" + HttpUtility.UrlEncode(model.Name.Replace("'", "''")) + "&ParentId=" + model.ParentId, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<CategoryLibrary> templist = baseResponse.Data as List<CategoryLibrary> ?? new List<CategoryLibrary>();
                if (templist.Where(x => x.Id != model.Id).Any())
                {
                    baseResponse = baseResponse.AlreadyExists();
                }
                else
                {
                    bool AllowColorInCategory = Convert.ToBoolean(_configuration.GetValue<string>("Allow_Color_In_Category"));

                    var imagedoc = helper.ApiCall(URL, EndPoints.Category + "?Id=" + model.Id, "GET", null);
                    baseResponse = baseResponse.JsonParseRecord(imagedoc);

                    CategoryLibrary cat = baseResponse.Data as CategoryLibrary;
                    if (cat == null)
                    {
                        return Ok(baseResponse.NotExist());
                    }
                    string OldName = cat.Image;
                    cat.Name = model.Name;
                    cat.CurrentLevel = model.CurrentLevel;
                    cat.ParentId = model.ParentId;
                    cat.MetaTitles = model.MetaTitles;
                    cat.MetaDescription = model.MetaDescription;
                    cat.MetaKeywords = model.MetaKeywords;
                    cat.Status = string.IsNullOrWhiteSpace(model.Status) ? "Active" : model.Status;
                    if (AllowColorInCategory == true)
                    {
                        cat.Color = model.Color;
                    }
                    cat.Title = model.Title;
                    cat.SubTitle = model.SubTitle;
                    cat.Description = model.Description;
                    cat.ModifiedBy = HttpContext.User?.Claims?.FirstOrDefault(x => x.Type.Equals("UserID"))?.Value ?? "system";
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

                    var response = helper.ApiCall(URL, EndPoints.Category, "PUT", cat);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                    UpdatePathId(cat.Id);
                }
                return Ok(baseResponse);
            }
            catch (Exception ex)
            {
                return Ok(baseResponse.InvalidInput(ex.Message));
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            try
            {
                var temp = helper.ApiCall(URL, EndPoints.Category + "?ParentID=" + id, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<CategoryLibrary> tmpList = baseResponse.Data as List<CategoryLibrary> ?? new List<CategoryLibrary>();
                if (!tmpList.Any())
                {
                    List<AssignSpecificationToCategoryLibrary> assignSpecToCat = new List<AssignSpecificationToCategoryLibrary>();
                    List<AssignSizeValueToCategory> assignSizeValToCat = new List<AssignSizeValueToCategory>();
                    List<AssignReturnPolicyToCatagoryLibrary> assignReturnPolicyToCat = new List<AssignReturnPolicyToCatagoryLibrary>();
                    List<Products> product = new List<Products>();
                    List<ProductSpecificationMapping> productspecMapp = new List<ProductSpecificationMapping>();

                    try
                    {
                        var tempAssignSpecToCat = helper.ApiCall(URL, EndPoints.AssignSpecToCat + "?CategoryID=" + id, "GET", null);
                        BaseResponse<AssignSpecificationToCategoryLibrary> baseAssignSpecToCat = new BaseResponse<AssignSpecificationToCategoryLibrary>();
                        var assignSpecResp = baseAssignSpecToCat.JsonParseList(tempAssignSpecToCat);
                        assignSpecToCat = assignSpecResp.Data as List<AssignSpecificationToCategoryLibrary> ?? new List<AssignSpecificationToCategoryLibrary>();
                    }
                    catch { }

                    try
                    {
                        var tempAssignSizeValToCat = helper.ApiCall(URL, EndPoints.AssignSizeValueToCategory + "?CategoryID=" + id, "GET", null);
                        BaseResponse<AssignSizeValueToCategory> baseAssignSizeValToCat = new BaseResponse<AssignSizeValueToCategory>();
                        var assignSizeResp = baseAssignSizeValToCat.JsonParseList(tempAssignSizeValToCat);
                        assignSizeValToCat = assignSizeResp.Data as List<AssignSizeValueToCategory> ?? new List<AssignSizeValueToCategory>();
                    }
                    catch { }

                    try
                    {
                        var tempReturnPolicyToCat = helper.ApiCall(URL, EndPoints.AssignReturnPolicyToCatagory + "?CategoryID=" + id, "GET", null);
                        BaseResponse<AssignReturnPolicyToCatagoryLibrary> baseReturnPolicyToCat = new BaseResponse<AssignReturnPolicyToCatagoryLibrary>();
                        var returnPolicyToCat = baseReturnPolicyToCat.JsonParseList(tempReturnPolicyToCat);
                        assignReturnPolicyToCat = returnPolicyToCat.Data as List<AssignReturnPolicyToCatagoryLibrary> ?? new List<AssignReturnPolicyToCatagoryLibrary>();
                    }
                    catch { }

                    try
                    {
                        var tempProduct = helper.ApiCall(URL, EndPoints.Product + "?CategoryId=" + id, "GET", null);
                        BaseResponse<Products> baseProducts = new BaseResponse<Products>();
                        var products = baseProducts.JsonParseList(tempProduct);
                        product = products.Data as List<Products> ?? new List<Products>();
                    }
                    catch { }

                    try
                    {
                        var tempProductSpecMapp = helper.ApiCall(URL, EndPoints.ProductSpecificationMapping + "?CatId=" + id, "GET", null);
                        BaseResponse<ProductSpecificationMapping> baseProductSpecMapp = new BaseResponse<ProductSpecificationMapping>();
                        var productSpecMapp = baseProductSpecMapp.JsonParseList(tempProductSpecMapp);
                        productspecMapp = productSpecMapp.Data as List<ProductSpecificationMapping> ?? new List<ProductSpecificationMapping>();
                    }
                    catch { }

                    if (assignSpecToCat.Any() || assignSizeValToCat.Any() || assignReturnPolicyToCat.Any() || product.Any() || productspecMapp.Any())
                    {
                        if (assignSpecToCat.Any())
                        {
                            baseResponse = baseResponse.ChildAlreadyExists("AssignSpecificationToCategory", "SubCategory");
                        }

                        if (assignSizeValToCat.Any())
                        {
                            baseResponse = baseResponse.ChildAlreadyExists("AssignSizeValesToCategory", "SubCategory");
                        }

                        if (assignReturnPolicyToCat.Any())
                        {
                            baseResponse = baseResponse.ChildAlreadyExists("AssignReturnPolicyToCategory", "SubCategory");
                        }

                        if (product.Any())
                        {
                            baseResponse = baseResponse.ChildAlreadyExists("Products", "SubCategory");
                        }

                        if (productspecMapp.Any())
                        {
                            baseResponse = baseResponse.ChildAlreadyExists("ProudctSpecificationMapping", "SubCategory");
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

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "?GetChild=" + true + "&Id=" + id + "&Isdeleted=false", "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("bindMainCategories")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> BindMainCategories()
        {
            int max_category_level = _configuration.GetValue<int>("category_level");

            var response = helper.ApiCall(URL, EndPoints.Category + "?PageIndex=0&PageSize=0&Isdeleted=false&Status=Active", "GET", null);
            var res = baseResponse.JsonParseList(response);
            var data = res.Data as IEnumerable<CategoryLibrary> ?? new List<CategoryLibrary>();
            var ListData = data.ToList();
            var RequiredData = ListData.Where(x => x.CurrentLevel < max_category_level).ToList();

            baseResponse.Data = RequiredData;

            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, int? pageindex = 1, int? pageSize = 10)
        {
            string url = string.Empty;

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }
            var response = helper.ApiCall(URL, EndPoints.Category + "?GetChild=" + true + "&Isdeleted =" + false + "&PageIndex=" + pageindex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
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


        #region NonAction Methods
        [NonAction]
        public Response JsonDeserialize(HttpResponseMessage httpresponse)
        {
            return JsonConvert.DeserializeObject<Response>(httpresponse.Content.ReadAsStringAsync().Result);
        }

        [NonAction]
        public ActionResult<Response<CategoryLibrary>> UpdatePathId(int id, bool isChildParent = false)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "?Id=" + id + "&IsChildParent=" + isChildParent, "GET", null);
            var tmp = new CategoryLibrary();
            if (response.IsSuccessStatusCode)
            {
                res = JsonDeserialize(response);
                var data = res?.data ?? new List<CategoryLibrary>();
                tmp = data.FirstOrDefault();
                if (tmp == null)
                {
                    return Ok(res);
                }
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
        public ActionResult<Response<CategoryLibrary>> UpdateSubPath(int id, bool isChildParent = false)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "?ParentID=" + id + "&IsChildParent=" + isChildParent, "GET", null);

            if (response.IsSuccessStatusCode)
            {
                res = JsonDeserialize(response);
                List<CategoryLibrary> spec = res?.data ?? new List<CategoryLibrary>();
                foreach (var item in spec)
                {
                    UpdatePathId(item.Id, isChildParent);
                    UpdateSubPath(item.Id);
                }
            }
            return null;
        }

        [NonAction]
        public string PathIdFunction(int Id)
        {
            var returnString = "";
            var response = helper.ApiCall(URL, EndPoints.Category + "?id=" + Id, "GET", null);
            if (response.IsSuccessStatusCode)
            {
                var resp = JsonDeserialize(response);
                var Current = resp.data.FirstOrDefault();
                var ParentId = Current.ParentId;
                if (ParentId != null)
                {
                    while (ParentId != null)
                    {
                        var getPrevious = helper.ApiCall(URL, EndPoints.Category + "?id=" + ParentId, "GET", null);
                        if (getPrevious.IsSuccessStatusCode)
                        {
                            var r1 = JsonDeserialize(getPrevious);
                            var c1 = r1.data.FirstOrDefault();
                            ParentId = c1.ParentId;
                            var currentid = c1.Id;
                            string PreviousString = returnString;
                            returnString = currentid.ToString() + "/" + PreviousString;
                        }
                    }
                    returnString = Current.Id.ToString() + returnString;
                    return returnString;
                }
                return returnString;
            }
            return "error";
        }

        #endregion
    }
}
