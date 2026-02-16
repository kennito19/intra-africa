using IDServer.Domain.Entity;
using IDServerApplication.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTTokenProvider.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PageController : ControllerBase
    {
        private readonly IPageRolesServices _pageRolesServices;

        public PageController(IPageRolesServices pageRolesServices)
        {
            _pageRolesServices = pageRolesServices;
        }

        [HttpPost("CreatePage")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateModule(PageRoleModule pageRole)
        {
            var dataresponse = _pageRolesServices.CreateModule(pageRole);
            return Ok(dataresponse);
        }

        [HttpPut("EditPage")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditModule(PageRoleModule pageRole)
        {
            var dataresponse = _pageRolesServices.EditModule(pageRole);
            return Ok(dataresponse);
        }

        [HttpDelete("DeletePage")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteModule(int Id)
        {
            var dataresponse = _pageRolesServices.DeleteModule(Id);
            return Ok(dataresponse);
        }

        [HttpPost("CreateRoleType")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUserType(RoleType model)
        {
            var dataresponse = _pageRolesServices.CreateRoleType(model);
            return Ok(dataresponse);
        }

        [HttpPut("EditRoleType")]
        [Authorize(Roles = "Admin")]
        public IActionResult EditUserType(RoleType model)
        {
            var dataresponse = _pageRolesServices.EditRoleType(model);
            return Ok(dataresponse);
        }


        [HttpDelete("DeleteRoleType")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUserType(int Id)
        {
            var dataresponse = _pageRolesServices.DeleteRoleType(Id);
            return Ok(dataresponse);
        }

        [HttpPost("AssignPageRoles")]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignPageRoles(AssignPageRole model)
        {
            var dataresponse = _pageRolesServices.AssignPage(model);
            return Ok(dataresponse);
        }

        [HttpGet("GetAssignedPagesByRoleType")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAssignedPagesByRoleType(int Id)
        {
            var dataresponse = _pageRolesServices.GetAssignedPagesByRoleType(Id);
            return Ok(dataresponse);
        }

        [HttpGet("GetAssignedPagesByUserId")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAssignedPagesByUserId(string Id)
        {
            var dataresponse =await _pageRolesServices.GetAssignedPagesByUserId(Id);
            return Ok(dataresponse);
        }

        [HttpGet("/api/Page/GetAssignedPagesByUserIdandRoleTypeId")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAssignedPagesByUserIdandRoleTypeId(string UserId, int RoletypeId)
        {
            var dataresponse = await _pageRolesServices.GetAssignedPagesByUserIdandRoleTypeId(UserId,RoletypeId);
            return Ok(dataresponse);
        }


        [HttpGet("GetAllRoleTypes")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUserType(int pageIndex = 1, int pageSize = 10)
        {
            var dataresponse = _pageRolesServices.GetRoleTypes(pageIndex,pageSize);
            return Ok(dataresponse);
        }

        [HttpGet("GetRoleTypeById")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetRoleTypeById(int Id)
        {
            var dataresponse = _pageRolesServices.GetRoleTypeById(Id);
            return Ok(dataresponse);
        }

        [HttpGet("GetPageAccessByRoleType")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPageAccessByRoleType(int Id)
        {
            var dataresponse = _pageRolesServices.GetUserTypeWithPageAccess(Id);
            return Ok(dataresponse);
        }

        [HttpGet("GetAllPages")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllPages(int pageIndex = 1, int pageSize = 10, string? searchString = null)
        {
            var dataresponse = _pageRolesServices.GetPages(pageIndex, pageSize, searchString);
            return Ok(dataresponse);
        }

        [HttpGet("GetPageById")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetPageModuleById(int Id)
        {
            var dataresponse = _pageRolesServices.GetPageModuleById(Id);
            return Ok(dataresponse);
        }


    }
}
