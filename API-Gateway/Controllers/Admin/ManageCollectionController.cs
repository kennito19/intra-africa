using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;
using System.Web;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageCollectionController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<ManageCollectionLibrary> baseResponse = new BaseResponse<ManageCollectionLibrary>();
        private ApiHelper helper;
        public ManageCollectionController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create([FromForm] ManageCollectionDTO model)
        {
            var temp1 = helper.ApiCall(URL, EndPoints.ManageCollection + "?Type=" + model.Type + "&date=" + true + "&PageIndex=0&PageSize=0", "GET", null);
            baseResponse = baseResponse.JsonParseList(temp1);
            List<ManageCollectionLibrary> tempList1 = (List<ManageCollectionLibrary>)baseResponse.Data;

            //List<ManageCollectionLibrary> tempList = tempList1.Where(x => x.Name == HttpUtility.UrlEncode(model.Name)).ToList();

            //var temp = helper.ApiCall(URL, EndPoints.ManageCollection + "?Name=" + model.Name + "&Type=" + model.Type, "GET", null);
            //baseResponse = baseResponse.JsonParseList(temp);
            //List<ManageCollectionLibrary> tempList = (List<ManageCollectionLibrary>)baseResponse.Data;
            ManageCollectionLibrary manageCollection = new ManageCollectionLibrary();

            //if (tempList1.Any())
            //{
            //    baseResponse = baseResponse.AlreadyExists();
            //}
            //else
            //{
            if (model.Type.ToLower() == "flashsale" && model.StartDate != null && model.EndDate != null)
            {
                if (tempList1.Count > 0)
                {
                    DateTime sdate = DateTime.ParseExact(Convert.ToDateTime(model.StartDate).ToString("yyyy-MM-dd HH:mm:ss.fff"), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                    DateTime edate = DateTime.ParseExact(Convert.ToDateTime(model.EndDate).ToString("yyyy-MM-dd HH:mm:ss.fff"), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                    List<ManageCollectionLibrary> filteredList = tempList1
                    .Where(item => (sdate <= item.EndDate) && (edate >= item.StartDate))
                    //(sdate < item.EndDate && edate > item.StartDate) ||
                    //(item.StartDate >= sdate && item.StartDate <= edate) ||
                    //(item.StartDate <= sdate && item.EndDate >= edate) ||
                    //(item.StartDate == sdate && item.EndDate == edate) ||
                    //(item.EndDate == sdate && item.EndDate == edate))
                    .ToList();
                    if (filteredList.Count <= 0)
                    {
                        manageCollection.Name = model.Name;
                        manageCollection.Type = model.Type;
                        manageCollection.Image = UploadDoc(model.Name, model.ImageFile);
                        manageCollection.Title = model.Title;
                        manageCollection.Description = model.Description;
                        manageCollection.SubTitle = model.SubTitle;
                        manageCollection.StartDate = model.StartDate;
                        manageCollection.EndDate = model.EndDate;
                        manageCollection.Status = model.Status;
                        manageCollection.Sequence = model.Sequence;
                        manageCollection.CreatedAt = DateTime.Now;
                        manageCollection.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                        var response = helper.ApiCall(URL, EndPoints.ManageCollection, "POST", manageCollection);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                    else
                    {
                        baseResponse.code = 204;
                        baseResponse.Message = "Cannot create flash sale; an ongoing sale exists between these dates.";
                        baseResponse.Data = null;
                    }
                }
                else
                {
                    manageCollection.Name = model.Name;
                    manageCollection.Type = model.Type;
                    manageCollection.Image = UploadDoc(model.Name, model.ImageFile);
                    manageCollection.Title = model.Title;
                    manageCollection.Description = model.Description;
                    manageCollection.SubTitle = model.SubTitle;
                    manageCollection.StartDate = model.StartDate;
                    manageCollection.EndDate = model.EndDate;
                    manageCollection.Status = model.Status;
                    manageCollection.Sequence = model.Sequence;
                    manageCollection.CreatedAt = DateTime.Now;
                    manageCollection.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    var response = helper.ApiCall(URL, EndPoints.ManageCollection, "POST", manageCollection);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
                //}
                //else
                //{
                //    manageCollection.Name = model.Name;
                //    manageCollection.Type = model.Type;
                //    manageCollection.Image = UploadDoc(model.Name, model.ImageFile);
                //    manageCollection.Title = model.Title;
                //    manageCollection.Description = model.Description;
                //    manageCollection.SubTitle = model.SubTitle;
                //    manageCollection.StartDate = model.StartDate;
                //    manageCollection.EndDate = model.EndDate;
                //    manageCollection.Status = model.Status;
                //    manageCollection.Sequence = model.Sequence;
                //    manageCollection.CreatedAt = DateTime.Now;
                //    manageCollection.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                //    var response = helper.ApiCall(URL, EndPoints.ManageCollection, "POST", manageCollection);
                //    baseResponse = baseResponse.JsonParseInputResponse(response);
                //}
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update([FromForm] ManageCollectionDTO model)
        {
            var temp1 = helper.ApiCall(URL, EndPoints.ManageCollection + "?Type=" + model.Type + "&date=" + true + "&PageIndex=0&PageSize=0", "GET", null);
            baseResponse = baseResponse.JsonParseList(temp1);
            List<ManageCollectionLibrary> tempList1 = (List<ManageCollectionLibrary>)baseResponse.Data;

            //List<ManageCollectionLibrary> tempList = tempList1.Where(x => x.Name == HttpUtility.UrlEncode(model.Name)).ToList();

            //var temp = helper.ApiCall(URL, EndPoints.ManageCollection + "?Name=" + HttpUtility.UrlEncode(model.Name) + "&Type=" + model.Type, "GET", null);
            //baseResponse = baseResponse.JsonParseList(temp);
            //List<ManageCollectionLibrary> tempList = (List<ManageCollectionLibrary>)baseResponse.Data;
            ManageCollectionLibrary manageCollection = new ManageCollectionLibrary();

            var recordCall = helper.ApiCall(URL, EndPoints.ManageCollection + "?Id=" + model.Id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(recordCall);
            ManageCollectionLibrary record = (ManageCollectionLibrary)baseResponse.Data;


            //if (tempList1.Where(x => x.Id != model.Id).Any())
            //{
            //    baseResponse = baseResponse.AlreadyExists();
            //}
            //else
            //{
            if (model.Type.ToLower() == "flashsale" && model.StartDate != null && model.EndDate != null)
            {
                DateTime sdate = DateTime.ParseExact(Convert.ToDateTime(model.StartDate).ToString("yyyy-MM-dd HH:mm:ss.fff"), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                DateTime edate = DateTime.ParseExact(Convert.ToDateTime(model.EndDate).ToString("yyyy-MM-dd HH:mm:ss.fff"), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                List<ManageCollectionLibrary> filteredList = tempList1
                    .Where(item => item.Id != model.Id && (sdate <= item.EndDate) && (edate >= item.StartDate))
                    //((sdate >= item.StartDate && sdate <= item.EndDate) ||
                    //(edate >= item.StartDate && edate <= item.EndDate) ||
                    //(item.StartDate >= sdate && item.StartDate <= edate) ||
                    //(item.EndDate >= sdate && item.EndDate <= edate) ||
                    //(item.EndDate >= sdate && item.EndDate <= edate)))
                    .ToList();


                //List<ManageCollectionLibrary> filteredList = tempList1
                //    .Where(item =>
                //        item.Id != model.Id &&
                //        !(model.StartDate >= item.StartDate && model.StartDate <= item.EndDate) &&
                //        !(model.EndDate >= item.StartDate && model.EndDate <= item.EndDate))
                //    .ToList();
                if (filteredList.Count <= 0)
                {
                    record.Name = model.Name;
                    record.Type = model.Type;
                    record.Image = UpdateDocFile(record.Image, model.Name, model.ImageFile);
                    record.Title = model.Title;
                    record.Description = model.Description;
                    record.SubTitle = model.SubTitle;
                    record.StartDate = model.StartDate;
                    record.EndDate = model.EndDate;
                    record.Status = model.Status;
                    record.Sequence = model.Sequence;
                    record.ModifiedAt = DateTime.Now;
                    record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    var response = helper.ApiCall(URL, EndPoints.ManageCollection, "PUT", record);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
                else
                {
                    baseResponse.code = 204;
                    baseResponse.Message = "Cannot create flash sale; an ongoing sale exists between these dates.";
                    baseResponse.Data = null;
                }
            }
            else
            {
                record.Name = model.Name;
                record.Type = model.Type;
                record.Image = UpdateDocFile(record.Image, model.Name, model.ImageFile);
                record.Title = model.Title;
                record.Description = model.Description;
                record.SubTitle = model.SubTitle;
                record.StartDate = model.StartDate;
                record.EndDate = model.EndDate;
                record.Status = model.Status;
                record.Sequence = model.Sequence;
                record.ModifiedAt = DateTime.Now;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageCollection, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            //}

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageCollection + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageCollectionLibrary> tempList = (List<ManageCollectionLibrary>)baseResponse.Data;
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ManageCollectionMapping + "?CollectionId=" + id, "GET", null);
                BaseResponse<ManageCollectionMappingLibrary> baseResponse1 = new BaseResponse<ManageCollectionMappingLibrary>();
                baseResponse1 = baseResponse1.JsonParseList(response);
                List<ManageCollectionMappingLibrary> tempList1 = (List<ManageCollectionMappingLibrary>)baseResponse1.Data;
                if (tempList1.Any())
                {
                    baseResponse = baseResponse.ChildExists();
                }
                else
                {
                    response = helper.ApiCall(URL, EndPoints.ManageCollection + "?Id=" + id, "DELETE", null);
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
        [Authorize]
        public ActionResult<ApiHelper> Get(int? pageindex = 1, int? pageSize = 10, bool? IsLive = null)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageCollection + "?PageIndex=" + pageindex + "&PageSize=" + pageSize, "GET", null);

            baseResponse = baseResponse.JsonParseList(response);
            List<ManageCollectionLibrary> ManageCollectionLibrary = (List<ManageCollectionLibrary>)baseResponse.Data;
            if (IsLive != null && IsLive == true)
            {
                baseResponse.Data = ManageCollectionLibrary.Where(p => p.Type.ToString().ToLower() == "product collection" || (p.EndDate >= DateTime.Today && p.Type.ToString().ToLower() == "flashsale")).ToList();
            }
            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageCollection + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("byType")]
        [Authorize]
        public ActionResult<ApiHelper> GetByType(string type, string? status = null, int? pageindex = 1, int? pageSize = 10, bool? IsLive = null)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(status))
            {
                url += "&Status=" + status;
            }
            var response = helper.ApiCall(URL, EndPoints.ManageCollection + "?Type=" + type + "&PageIndex=" + pageindex + "&PageSize=" + pageSize + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ManageCollectionLibrary> ManageCollectionLibrary = (List<ManageCollectionLibrary>)baseResponse.Data;

            if (IsLive != null && IsLive == true)
            {
                baseResponse.Data = ManageCollectionLibrary.Where(p => p.Type.ToString().ToLower() == "product collection" || (p.EndDate >= DateTime.Today && p.Type.ToString().ToLower() == "flashsale")).ToList();

            }
            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize]
        public ActionResult<ApiHelper> Search(string type, string? searchtext = null, string? status = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }

            if (!string.IsNullOrEmpty(type))
            {
                url += "&Type=" + type;
            }
            if (!string.IsNullOrEmpty(status))
            {
                url += "&Status=" + status;
            }
            var response = helper.ApiCall(URL, EndPoints.ManageCollection + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [NonAction]
        public string UploadDoc(string Name, IFormFile FileName)
        {
            try
            {
                var file = FileName;


                var folderName = Path.Combine("Resources", "ManageCollection");
                if (file != null)
                {
                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + "_Collection_" + a;
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
                    var fileName = Name + "_Collection_" + a;
                    var folderName = Path.Combine("Resources", "ManageCollection");
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
