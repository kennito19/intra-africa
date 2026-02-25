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
    public class ManageLayoutTypesController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        private readonly HttpContext _httpContext;
        BaseResponse<ManageLayoutTypesLibrary> baseResponse = new BaseResponse<ManageLayoutTypesLibrary>();
        private ApiHelper helper;
        public ManageLayoutTypesController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create([FromForm] ManageLayoutTypesDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLayoutTypes + "?Name=" + model.Name + "&LayoutId=" + model.LayoutId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageLayoutTypesLibrary> tempList = baseResponse.Data as List<ManageLayoutTypesLibrary> ?? new List<ManageLayoutTypesLibrary>();

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                ManageLayoutTypesLibrary manageLayoutTypes = new ManageLayoutTypesLibrary();
                manageLayoutTypes.Name = model.Name;
                manageLayoutTypes.LayoutId = model.LayoutId;
                manageLayoutTypes.ImageUrl = UploadDoc(model.Name, model.Image);
                manageLayoutTypes.Options = model.Options;
                manageLayoutTypes.ClassName = model.ClassName;
                manageLayoutTypes.HasInnerColumns = model.HasInnerColumns;
                manageLayoutTypes.Columns = model.Columns;
                manageLayoutTypes.MinImage = model.MinImage;
                manageLayoutTypes.MaxImage = model.MaxImage;
                manageLayoutTypes.CreatedAt = DateTime.Now;
                manageLayoutTypes.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypes, "POST", manageLayoutTypes);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update([FromForm] ManageLayoutTypesDTO model)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLayoutTypes + "?Name=" + model.Name + "&LayoutId=" + model.LayoutId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageLayoutTypesLibrary> tempList = baseResponse.Data as List<ManageLayoutTypesLibrary> ?? new List<ManageLayoutTypesLibrary>();

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = helper.ApiCall(URL, EndPoints.ManageLayoutTypes + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                ManageLayoutTypesLibrary record = baseResponse.Data as ManageLayoutTypesLibrary;
                record.LayoutId = model.LayoutId;
                record.Name = model.Name;
                record.ImageUrl = UpdateDocFile(record.ImageUrl, model.Name, model.Image);
                record.Options = model.Options;
                record.ClassName = model.ClassName;
                record.HasInnerColumns = model.HasInnerColumns;
                record.Columns = model.Columns;
                record.MinImage = model.MinImage;
                record.MaxImage = model.MaxImage;
                record.ModifiedAt = DateTime.Now;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypes, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLayoutTypes + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageLayoutTypesLibrary> tempList = baseResponse.Data as List<ManageLayoutTypesLibrary> ?? new List<ManageLayoutTypesLibrary>();
            if (tempList.Any())
            {
                var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypes + "?Id=" + id, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet("bindlayouts")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> bindlayouts()
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypes + "?PageIndex=0&PageSize=0", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ManageLayoutTypesLibrary> layoutlst = new List<ManageLayoutTypesLibrary>();
            BaseResponse<string> Responsedata = new BaseResponse<string>();
            if (baseResponse.code == 200)
            {
                layoutlst = baseResponse.Data as List<ManageLayoutTypesLibrary> ?? new List<ManageLayoutTypesLibrary>();

                if (layoutlst.Count > 0)
                {
                    List<layoutdata> layoutDataList = new List<layoutdata>();

                    var parentData = layoutlst.Select(x => new { x.LayoutId, x.LayoutName }).Distinct().ToList();
                    foreach (var item in parentData)
                    {
                        var childData = layoutlst.Where(x => x.LayoutId == item.LayoutId).OrderBy(p => p.Columns).ToList();

                        layoutdata data = new layoutdata();
                        data.Id = item.LayoutId;
                        data.Name = item.LayoutName;
                        data.layoutTypes = childData.Select(child => new layoutTypedata
                        {
                            Id = child.Id,
                            Name = child.Name,
                            Image = child.ImageUrl
                        }).ToList();
                        layoutDataList.Add(data);
                    }

                    BaseResponse<ManageLayoutsLibrary> Layoutresponsedata = new BaseResponse<ManageLayoutsLibrary>();
                    var Layoutresponse = helper.ApiCall(URL, EndPoints.ManageLayouts + "?PageIndex=0&PageSize=0", "GET", null);
                    Layoutresponsedata = Layoutresponsedata.JsonParseList(Layoutresponse);
                    List<ManageLayoutsLibrary> _layoutlst = new List<ManageLayoutsLibrary>();

                    if (Layoutresponsedata.code == 200)
                    {
                        _layoutlst = Layoutresponsedata.Data as List<ManageLayoutsLibrary> ?? new List<ManageLayoutsLibrary>();
                        if (_layoutlst.Count > 0)
                        {
                            _layoutlst = _layoutlst.Where(p => parentData.All(item => item.LayoutId != p.Id)).ToList();
                            if (_layoutlst.Count > 0)
                            {
                                foreach (var item in _layoutlst)
                                {
                                    layoutdata data = new layoutdata();
                                    data.Id = item.Id;
                                    data.Name = item.Name;
                                    layoutDataList.Add(data);
                                }
                            }
                        }
                    }

                    Responsedata.code = 200;
                    Responsedata.Message = "Record bind successfully.";
                    Responsedata.Data = layoutDataList;
                }
                else
                {
                    Responsedata.code = 200;
                    Responsedata.Message = "Record does not exist.";
                    Responsedata.Data = null;
                }

            }
            return Ok(Responsedata);
            //return Ok(baseResponse.JsonParseList(response));
        }


        [HttpGet("bindInnercolumns")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> BindInnercolumns()
        {
            var temp = helper.ApiCall(URL, EndPoints.ManageLayoutTypes + "?PageIndex=" + 0 + "&PageSize=" + 0, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageLayoutTypesLibrary> tempList = baseResponse.Data as List<ManageLayoutTypesLibrary> ?? new List<ManageLayoutTypesLibrary>();
            List<ManageLayoutTypesLibrary> manageLayoutTypesLibrary = new List<ManageLayoutTypesLibrary>();

            if (tempList.Count > 0)
            {
                manageLayoutTypesLibrary = tempList.Where(x => x.HasInnerColumns == true).ToList();
            }
            baseResponse.Data = manageLayoutTypesLibrary;

            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypes + "?Id=" + id, "GET", null);
            return Ok(baseResponse.JsonParseRecord(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller,Customer")]
        public ActionResult<ApiHelper> Search(string? searchText, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.ManageLayoutTypes + "?Searchtext=" + HttpUtility.UrlEncode(searchText) + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }


        [NonAction]
        public string UploadDoc(string Name, IFormFile FileName)
        {
            try
            {
                var file = FileName;

                var folderName = Path.Combine("Resources", "ManageLayoutTypes");
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
                    var folderName = Path.Combine("Resources", "ManageLayoutTypes");
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

        public class layoutdata
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<layoutTypedata> layoutTypes { get; set; }
        }

        public class layoutTypedata
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
        }

    }
}
