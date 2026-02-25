using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class AssignSizeValuesToCategoryController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper api;
        private readonly IConfiguration _configuration;
        BaseResponse<AssignSizeValueToCategory> baseResponse = new BaseResponse<AssignSizeValueToCategory>();
        public static string CatalogueUrl = string.Empty;
        public AssignSizeValuesToCategoryController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            api = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(AssignSizeValueToCate model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?AssignSpecID=" + model.AssignSpecID + "&SizeTypeID=" + model.SizeTypeID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignSizeValueToCategory> tempList = baseResponse.Data as List<AssignSizeValueToCategory> ?? new List<AssignSizeValueToCategory>();
            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                baseResponse = SaveSizeType(model);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(AssignSizeValueToCate model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?AssignSpecID=" + model.AssignSpecID + "&SizeTypeID=" + model.SizeTypeID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignSizeValueToCategory> tempList = baseResponse.Data as List<AssignSizeValueToCategory> ?? new List<AssignSizeValueToCategory>();
            if (tempList.Any())
            {
                temp = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?AssignSpecID=" + model.AssignSpecID + "&SizeTypeID=" + model.SizeTypeID, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(temp);
            }
            baseResponse = SaveSizeType(model);
            return Ok(baseResponse);
        }

        [NonAction]
        public BaseResponse<AssignSizeValueToCategory> SaveSizeType(AssignSizeValueToCate model)
        {
            var temp = new HttpResponseMessage();

            AssignSizeValueToCategory assignSize = new AssignSizeValueToCategory();
            assignSize.AssignSpecID = model.AssignSpecID;
            assignSize.SizeTypeID = model.SizeTypeID;
            assignSize.IsAllowSizeInComparision = model.IsAllowSizeInComparision;
            assignSize.IsAllowSizeInFilter = model.IsAllowSizeInFilter;
            assignSize.IsAllowSizeInTitle = model.IsAllowSizeInTitle;
            assignSize.IsAllowSizeInVariant = false;
            assignSize.TitleSequenceOfSize = model.TitleSequenceOfSize;
            assignSize.CreatedAt = DateTime.Now;
            assignSize.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            foreach (var item in model.SizeId.Distinct().ToArray())
            {
                assignSize.SizeId = item;
                temp = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory, "POST", assignSize);
            }
            baseResponse = baseResponse.JsonParseInputResponse(temp);
            return baseResponse;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int SizeTypeID, int AssignSpecID)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?SizeTypeID=" + SizeTypeID + "&AssignSpecID=" + AssignSpecID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignSizeValueToCategory> record = baseResponse.Data as List<AssignSizeValueToCategory> ?? new List<AssignSizeValueToCategory>();
            if (record.Any())
            {
                BaseResponse<CheckSizeType> AssignSpecbaseResponse = new BaseResponse<CheckSizeType>();
                var responseData = api.ApiCall(CatalogueUrl, EndPoints.CheckAssignSpecsToCategory + "/checkSizeType" + "?assignSpecId=" + AssignSpecID + "&sizeTypeId=" + SizeTypeID, "GET", null);
                AssignSpecbaseResponse = AssignSpecbaseResponse.JsonParseRecord(responseData);
                if (AssignSpecbaseResponse.code == 200)
                {
                    CheckSizeType checkSpecType = new CheckSizeType();
                    checkSpecType = AssignSpecbaseResponse.Data as CheckSizeType;
                    if (!Convert.ToBoolean(checkSpecType.IsAllowDeleteSizeType))
                    {
                        baseResponse = baseResponse.ChildAlreadyExists("Products", "AssignSizeValueToCategory");
                    }
                    else
                    {
                        var response = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?AssignSpecID=" + AssignSpecID + "&SizeTypeID=" + SizeTypeID, "DELETE", null);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                }
                else
                {
                    var response = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?AssignSpecID=" + AssignSpecID + "&SizeTypeID=" + SizeTypeID, "DELETE", null);
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
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Get(int? PageIndex = 1, int? PageSize = 10)
        {
            var response = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byAssignSpecId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> GetByAssignSpecID(int assignSpecId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?AssignSpecID=" + assignSpecId + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("bySizeTypeId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> GetBySizeTypeId(int sizeTypeId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?SizeTypeId=" + sizeTypeId + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byAssignSpecId&SizeTypeId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> GetByAssignSpecIdANDSizeTypeId(int assignSpecId, int sizeTypeId, int? pageIndex = 1, int? pageSize = 10)
        {
            BaseResponse<CheckAssignSizeValuestoCategory> AssignSpecbaseResponse = new BaseResponse<CheckAssignSizeValuestoCategory>();
            var responseData = api.ApiCall(CatalogueUrl, EndPoints.CheckAssignSpecsToCategory + "/checkAssignSizeValuesToCat" + "?assignSpecId=" + assignSpecId + "&sizeTypeId=" + sizeTypeId + "&multiSeller=" + Convert.ToBoolean(_configuration.GetValue<string>("is_one_product_multiseller")), "GET", null);
            AssignSpecbaseResponse = AssignSpecbaseResponse.JsonParseRecord(responseData);

            var response = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?SizeTypeId=" + sizeTypeId + "&AssignSpecID=" + assignSpecId + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            if (baseResponse.code == 200)
            {
                List<AssignSizeValueToCategory> lstassignSizeValues = baseResponse.Data as List<AssignSizeValueToCategory> ?? new List<AssignSizeValueToCategory>();
                if (AssignSpecbaseResponse.code == 200)
                {
                    CheckAssignSizeValuestoCategory checkAssignSizeValuestocat = AssignSpecbaseResponse.Data as CheckAssignSizeValuestoCategory;

                    var result = lstassignSizeValues.Select(x => new
                    {
                        RowNumber = x.RowNumber,
                        PageCount = x.PageCount,
                        RecordCount = x.RecordCount,
                        Id = x.Id,
                        AssignSpecID = x.AssignSpecID,
                        SizeTypeID = x.SizeTypeID,
                        SizeId = x.SizeId,
                        IsAllowSizeInFilter = x.IsAllowSizeInFilter,
                        IsAllowSizeInComparision = x.IsAllowSizeInComparision,
                        IsAllowSizeInTitle = x.IsAllowSizeInTitle,
                        TitleSequenceOfSize = x.TitleSequenceOfSize,
                        CreatedBy = x.CreatedBy,
                        CreatedAt = x.CreatedAt,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedAt = x.ModifiedAt,
                        SizeName = x.SizeName,
                        SizeTypeName = x.SizeTypeName,
                        ValueEnabled = string.IsNullOrEmpty(checkAssignSizeValuestocat.SizeIds) ? true : Convert.ToBoolean(checkAssignSizeValuestocat.SizeIds.Split(',').Contains(x.SizeId.ToString())) ? false : true,
                    }).ToList();

                    baseResponse.Data = result;
                }
            }


            return Ok(baseResponse);
            //return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("bySearch")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> GetBySearch(string? searchtext, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;

            if (!string.IsNullOrEmpty(searchtext))
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }
            var response = api.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }
    }
    public class AssignSizeValueToCate
    {
        public int Id { get; set; }
        public int? AssignSpecID { get; set; }
        public int? SizeTypeID { get; set; }
        public int[]? SizeId { get; set; }
        public bool IsAllowSizeInFilter { get; set; } = false;
        public bool IsAllowSizeInVariant { get; set; } = false;
        public bool IsAllowSizeInComparision { get; set; } = false;
        public bool IsAllowSizeInTitle { get; set; } = false;
        public int? TitleSequenceOfSize { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public byte[]? timestamp { get; set; }

    }
}
