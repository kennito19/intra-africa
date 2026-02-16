using API_Gateway.Helper;
using API_Gateway.Models.Entity.IDServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API_Gateway.Controllers.Admin
{
    [Route("api")]
    [ApiController]
    public class RoleTypeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        public static string IDServerUrl = string.Empty;
        private ApiHelper api;
        public RoleTypeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            api = new ApiHelper(_httpContext);
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
        }


        [HttpPost("CreateRoleType")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUserType(RoleType model)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            BaseResponse<RoleType> RolebaseResponse = new BaseResponse<RoleType>();
            var getresponse = api.ApiCall(IDServerUrl, EndPoints.GetAllRoleTypes + "?pageIndex=0&pageSize=0", "GET", null);
            RolebaseResponse = RolebaseResponse.JsonParseList(getresponse);

            List<RoleType> roleTypes = new List<RoleType>();
            roleTypes = (List<RoleType>)RolebaseResponse.Data;
            var data = roleTypes.Where(x => x.Name.ToLower() == model.Name.ToLower()).ToList();
            if (data.Count > 0)
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.CreateRoleType, "POST", model);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut("UpdateRoleType")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditUserType(RoleType model)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            BaseResponse<RoleType> RolebaseResponse = new BaseResponse<RoleType>();
            var getresponse = api.ApiCall(IDServerUrl, EndPoints.GetAllRoleTypes + "?pageIndex=0&pageSize=0", "GET", null);
            RolebaseResponse = RolebaseResponse.JsonParseList(getresponse);

            List<RoleType> roleTypes = new List<RoleType>();
            roleTypes = (List<RoleType>)RolebaseResponse.Data;

            var data = roleTypes.Where(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id).ToList();
            if (data.Count > 0)
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.EditRoleType, "PUT", model);
                baseResponse = (baseResponse.JsonParseInputResponse(response));
            }

            return Ok(baseResponse);
        }

        [HttpDelete("DeleteRoleType")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUserType(int Id)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            var response = api.ApiCall(IDServerUrl, EndPoints.DeleteRoleType + "?Id=" + Id, "DELETE", null); ;
            return Ok(baseResponse.JsonParseInputResponse(response));
        }


        [HttpPost("AssignPageRoles")]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignPageRoles(AssignPageRole model)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            int Id = 0;
            string UserId = null;
            if (model.UserID != null)
            {
                BaseResponse<AssignPageRole> AssibaseResponse = new BaseResponse<AssignPageRole>();
                var Assiresponse = api.ApiCall(IDServerUrl, EndPoints.GetAssignedPageByUserId + "?Id=" + model.UserID, "GET", null);
                AssibaseResponse = AssibaseResponse.JsonParseList(Assiresponse);
                List<AssignPageRole> assignPageRoles = new List<AssignPageRole>();
                assignPageRoles = (List<AssignPageRole>)AssibaseResponse.Data;
                var data = assignPageRoles.Where(x => x.PageRoleId == model.PageRoleId && x.UserID == model.UserID).ToList();
                if (data.Count > 0)
                {
                    Id = data[0].Id;
                    UserId = data[0].UserID;
                }
            }
            else
            {
                BaseResponse<AssignPageRole> AssibaseResponse = new BaseResponse<AssignPageRole>();
                var Assiresponse = api.ApiCall(IDServerUrl, EndPoints.GetAssignedPageByRoleType + "?Id=" + model.RoleTypeId, "GET", null);
                AssibaseResponse = AssibaseResponse.JsonParseList(Assiresponse);
                List<AssignPageRole> assignPageRoles = new List<AssignPageRole>();
                assignPageRoles = (List<AssignPageRole>)AssibaseResponse.Data;
                var data = assignPageRoles.Where(x => x.PageRoleId == model.PageRoleId && x.RoleTypeId == model.RoleTypeId).ToList();
                if (data.Count > 0)
                {
                    Id = data[0].Id;
                }
            }
            if (Id > 0)
            {
                model.Id = Id;
                if (!string.IsNullOrEmpty(UserId))
                {
                    model.UserID = UserId;
                }
            }

            var response = api.ApiCall(IDServerUrl, EndPoints.AssignPageRoles, "POST", model);
            baseResponse = baseResponse.JsonParseInputResponse(response);


            return Ok(baseResponse);
        }

        [HttpGet("AssignPageRolesByRoleType")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAssignedPagesByRoleType(int Id)
        {
            BaseResponse<AssignPageRole> baseResponse = new BaseResponse<AssignPageRole>();
            var response = api.ApiCall(IDServerUrl, EndPoints.GetAssignedPageByRoleType + "?Id=" + Id, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("AssignPageRolesByUserId")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAssignedPageByUserId(string Id)
        {
            BaseResponse<AssignPageRole> baseResponse = new BaseResponse<AssignPageRole>();
            var response = api.ApiCall(IDServerUrl, EndPoints.GetAssignedPageByUserId + "?Id=" + Id, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("GetAllRoleTypes")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllRoleTypes(int pageIndex = 1, int pageSize = 10)
        {
            BaseResponse<RoleType> baseResponse = new BaseResponse<RoleType>();
            var response = api.ApiCall(IDServerUrl, EndPoints.GetAllRoleTypes + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        
    }
}
