using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class AssignSpecValuesToCategoryController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        private ApiHelper helper;
        BaseResponse<AssignSpecValuesToCategoryLibrary> baseResponse = new BaseResponse<AssignSpecValuesToCategoryLibrary>();
        public AssignSpecValuesToCategoryController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Save(AssignSpecValuesToCat model)
        {
            var response = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?AssignSpecID=" + model.AssignSpecID + "&SpecID=" + model.SpecID + "&SpecTypeID=" + model.SpecTypeID, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<AssignSpecValuesToCategoryLibrary> assignSpecValues = (List<AssignSpecValuesToCategoryLibrary>)baseResponse.Data;
            if (assignSpecValues.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                baseResponse = SaveAssiSpecs(model);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(AssignSpecValuesToCat model)
        {
            var response = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?AssignSpecID=" + model.AssignSpecID + "&SpecID=" + model.SpecID + "&SpecTypeID=" + model.SpecTypeID, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<AssignSpecValuesToCategoryLibrary> assignSpecValues = (List<AssignSpecValuesToCategoryLibrary>)baseResponse.Data;
            if (assignSpecValues.Any())
            {
                response = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?AssignSpecID=" + model.AssignSpecID + "&SpecID=" + model.SpecID + "&SpecTypeID=" + model.SpecTypeID, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            baseResponse = SaveAssiSpecs(model);

            return Ok(baseResponse);
        }

        [NonAction]
        public BaseResponse<AssignSpecValuesToCategoryLibrary> SaveAssiSpecs(AssignSpecValuesToCat model)
        {
            var temp = new HttpResponseMessage();

            AssignSpecValuesToCategoryLibrary assignSpec = new AssignSpecValuesToCategoryLibrary();
            assignSpec.AssignSpecID = model.AssignSpecID;
            assignSpec.SpecID = model.SpecID;
            assignSpec.SpecTypeID = model.SpecTypeID;
            assignSpec.IsAllowSpecInFilter = model.IsAllowSpecInFilter;
            assignSpec.IsAllowSpecInVariant = model.IsAllowSpecInVariant;
            assignSpec.IsAllowSpecInComparision = model.IsAllowSpecInComparision;
            assignSpec.IsAllowSpecInTitle = model.IsAllowSpecInTitle;
            assignSpec.IsAllowMultipleSelection = model.IsAllowMultipleSelection;
            assignSpec.TitleSequenceOfSpecification = model.TitleSequenceOfSpecification;
            assignSpec.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

            if (model.SpecTypeValueID != null)
            {
                foreach (var item in model.SpecTypeValueID)
                {
                    assignSpec.SpecTypeValueID = item;
                    temp = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory, "POST", assignSpec);
                }
            }
            else
            {
                temp = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory, "POST", assignSpec);
            }

            baseResponse = baseResponse.JsonParseInputResponse(temp);
            return baseResponse;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int assignSpecId, int specId, int specTypeId)
        {
            var temp = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?AssignSpecID=" + assignSpecId + "&SpecID=" + specId + "&SpecTypeID=" + specTypeId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignSpecValuesToCategoryLibrary> templist = (List<AssignSpecValuesToCategoryLibrary>)baseResponse.Data;
            if (templist.Any())
            {
                //var tempProducts = helper.ApiCall(URL, EndPoints.Product + "?AssiCategoryId=" + templist[0].AssignSpecID, "GET", null);
                //baseResponse = baseResponse.JsonParseList(tempProducts);
                //List<Products> productRecords = (List<Products>)baseResponse.Data;
                BaseResponse<CheckSpecType> AssignSpecbaseResponse = new BaseResponse<CheckSpecType>();
                var responseData = helper.ApiCall(URL, EndPoints.CheckAssignSpecsToCategory + "/checkSpecType" + "?assignSpecId=" + assignSpecId + "&specTypeId=" + specTypeId , "GET", null);
                AssignSpecbaseResponse = AssignSpecbaseResponse.JsonParseRecord(responseData);
                if (AssignSpecbaseResponse.code==200)
                {
                    CheckSpecType checkSpecType = new CheckSpecType();
                    checkSpecType = (CheckSpecType)AssignSpecbaseResponse.Data;
                    if (!Convert.ToBoolean(checkSpecType.IsAllowDeleteSpecType))
                    {
                        baseResponse = baseResponse.ChildAlreadyExists("AssignSpecValuesToCategory", "Products");
                    }
                    else
                    {
                        var response = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?AssignSpecID=" + assignSpecId + "&SpecID=" + specId + "&SpecTypeID=" + specTypeId, "DELETE", null);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                }
                else
                {
                    var response = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?AssignSpecID=" + assignSpecId + "&SpecID=" + specId + "&SpecTypeID=" + specTypeId, "DELETE", null);
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
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byAssignSpecID")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByAssignSpecID(int assignSpecId, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&AssignSpecID=" + assignSpecId, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("bySearch")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetBySearch(int? assignSpecId, int? specId, int? specTypeId, string? searchText, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?AssignSpecID=" + assignSpecId + "&SpecID=" + specId + "&SpecTypeID=" + specTypeId + "&Searchtext=" + HttpUtility.UrlEncode(searchText) + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("GetSpecsList")]
        public ActionResult<ApiHelper> GetSpecsList(int? AssignSpecId)
        {
            var response = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?AssignSpecID=" + AssignSpecId + "&PageIndex=0&PageSize=0&Mode=get", "GET", null);
            List<AssignSpecValuesToCategoryLibrary> props = baseResponse.JsonParseList(response).Data as List<AssignSpecValuesToCategoryLibrary>;

            var result = new JObject(
                            new JProperty("ProductSpecs", new JArray(
                                from spec in props
                                group spec by spec.SpecificationName into g
                                select new JObject(
                                    new JProperty("Name", g.Key),
                                    new JProperty("SpecId", g.First().SpecID),
                                    new JProperty("Types",
                                        new JArray(
                                            from type in g
                                            group type by type.SpecificationTypeName into g2
                                            select new JObject(
                                                new JProperty("Name", g2.Key),
                                                new JProperty("specTypeId", g2.First().SpecTypeID),
                                                new JProperty("FieldType", g2.First().FieldType),
                                                new JProperty("values",
                                                    g2.Where(x => x.SpecTypeValueID != 0)
                                                    .Select(x => new JObject(
                                                        new JProperty("specValueId", x.SpecTypeValueID),
                                                        new JProperty("Name", x.SpecificationTypeValueName),
                                                        new JProperty("IsAllowSpecInTitle", x.IsAllowSpecInTitle),
                                                        new JProperty("TitleSequenceOfSpecification", x.TitleSequenceOfSpecification),
                                                        new JProperty("IsAllowSpecInComparision", x.IsAllowSpecInComparision),
                                                        new JProperty("IsAllowSpecInFilter", x.IsAllowSpecInFilter),
                                                        new JProperty("IsAllowMultipleSelection", x.IsAllowMultipleSelection)
                                                    ))
                                                    .ToList()
                                                )
                                            )
                                        )
                                    )
                                )
                            ))
                        );

            string parseString = result.ToString();
            // Parse the original JSON using JObject
            JObject originalObject = JObject.Parse(parseString);

            // Extract the child objects from the ProductSpecs property
            JArray childArray = (JArray)originalObject["ProductSpecs"];

            // Serialize the child objects directly as a JSON array
            string newJson = childArray.ToString();


            var iip = JsonConvert.DeserializeObject<List<ProductSpec>>(newJson);

            BaseResponse<ProductSpec> baseRes = new BaseResponse<ProductSpec>()
            {
                code = 200,
                Message = "Record Bind Successfully",
                Data = iip
            };

            return Ok(baseRes);

        }

        [HttpGet("bySpecificationTypeValues")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> bySpecificationTypeValues(int? assignSpecId, int? specId, int? specTypeId, int? pageIndex = 1, int? pageSize = 10)
        {
            BaseResponse<CheckAssignSpecValuestoCategory> AssignSpecbaseResponse = new BaseResponse<CheckAssignSpecValuestoCategory>();
            var responseData = helper.ApiCall(URL, EndPoints.CheckAssignSpecsToCategory + "/checkAssignSpecvaluesToCat" + "?assignSpecId=" + assignSpecId + "&specTypeId=" + specTypeId + "&multiSeller=" + Convert.ToBoolean(_configuration.GetValue<string>("is_one_product_multiseller")), "GET", null);
            AssignSpecbaseResponse = AssignSpecbaseResponse.JsonParseRecord(responseData);


            var response = helper.ApiCall(URL, EndPoints.AssignSpecValuesToCategory + "?AssignSpecID=" + assignSpecId + "&SpecID=" + specId + "&SpecTypeID=" + specTypeId + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);

            if (baseResponse.code == 200)
            {
                List<AssignSpecValuesToCategoryLibrary> lstassignSpecValues = (List<AssignSpecValuesToCategoryLibrary>)baseResponse.Data;
                if (AssignSpecbaseResponse.code == 200)
                {
                    CheckAssignSpecValuestoCategory checkAssignSpecValuestocat = (CheckAssignSpecValuestoCategory)AssignSpecbaseResponse.Data;
                    var result = lstassignSpecValues.Select(x => new
                    {
                        RowNumber = x.RowNumber,
                        PageCount = x.PageCount,
                        RecordCount = x.RecordCount,
                        Id = x.Id,
                        AssignSpecID = x.AssignSpecID,
                        SpecID = x.SpecID,
                        SpecTypeID = x.SpecTypeID,
                        SpecTypeValueID = x.SpecTypeValueID,
                        IsAllowSpecInFilter = x.IsAllowSpecInFilter,
                        IsAllowSpecInComparision = x.IsAllowSpecInComparision,
                        IsAllowSpecInTitle = x.IsAllowSpecInTitle,
                        IsAllowMultipleSelection = x.IsAllowMultipleSelection,
                        TitleSequenceOfSpecification = x.TitleSequenceOfSpecification,
                        CreatedBy = x.CreatedBy,
                        CreatedAt = x.CreatedAt,
                        ModifiedBy = x.ModifiedBy,
                        ModifiedAt = x.ModifiedAt,
                        FieldType = x.FieldType,
                        SpecificationName = x.SpecificationName,
                        SpecificationTypeName = x.SpecificationTypeName,
                        SpecificationTypeValueName = x.SpecificationTypeValueName,
                        ValueEnabled = string.IsNullOrEmpty(checkAssignSpecValuestocat.SpecValueIds) ? true : Convert.ToBoolean(checkAssignSpecValuestocat.SpecValueIds.Split(',').Contains(x.SpecTypeValueID.ToString())) ? false : true,
                    }).ToList();

                    baseResponse.Data = result;
                }
            }

            return Ok(baseResponse);
            //return Ok(baseResponse.JsonParseList(response));
        }


    }



    public class ProductSpec
    {
        public string Name { get; set; }
        public int SpecId { get; set; }
        public List<SpecificationType>? Types { get; set; }
    }

    public class SpecificationType
    {
        public string Name { get; set; }
        public int SpecTypeId { get; set; }
        public string FieldType { get; set; }
        public List<SpecificationTypeValue>? Values { get; set; }
    }

    public class SpecificationTypeValue
    {
        public int? SpecValueId { get; set; }
        public string? Name { get; set; }
        public bool IsAllowSpecInFilter { get; set; } = false;
        public bool IsAllowSpecInComparision { get; set; } = false;
        public bool IsAllowSpecInTitle { get; set; } = false;
        public bool IsAllowMultipleSelection { get; set; } = false;
        public int? TitleSequenceOfSpecification { get; set; }
    }



    public class AssignSpecValuesToCat
    {
        public int Id { get; set; }
        public int? AssignSpecID { get; set; }
        public int? SpecID { get; set; }
        public int? SpecTypeID { get; set; }
        public int[]? SpecTypeValueID { get; set; }
        public bool IsAllowSpecInFilter { get; set; } = false;
        public bool IsAllowSpecInVariant { get; set; } = false;
        public bool IsAllowSpecInComparision { get; set; } = false;
        public bool IsAllowSpecInTitle { get; set; } = false;
        public bool IsAllowMultipleSelection { get; set; } = false;
        public int? TitleSequenceOfSpecification { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
