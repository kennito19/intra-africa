using IDServer.Domain.DTO;
using IDServer.Domain.Entity;
using IDServerApplication.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace JWTTokenProvider.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }


        #region Admin

        [HttpPost("signup")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminSignUp(SignUp signUp)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    Users users = new Users();
                    users.FirstName = signUp.FirstName;
                    users.LastName = signUp.LastName;
                    users.Email = signUp.EmailID;
                    users.PhoneNumber = signUp.MobileNo;
                    users.UserName = signUp.EmailID;
                    users.UserTypeId = signUp.UserTypeId;
                    users.ProfileImage = signUp.ProfileImage;
                    users.ReceiveNotifications = signUp.ReceiveNotifications;
                    users.Status = "Active";
                    users.CreatedAt = DateTime.UtcNow;
                    users.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    var dataresponse = await _userService.Create(users, signUp.Password, "Admin",null);
                    return Ok(dataresponse);
                }
                else
                {
                    BaseResponse<string> InvalidInput = new BaseResponse<string>(200, "Invalid Input");
                    var errorList = new List<string>();
                    foreach (var value in ModelState.Values)
                    {
                        foreach (var error in value.Errors)
                        {
                            errorList.Add(error.ErrorMessage);
                        }
                    }


                    InvalidInput.Data = errorList;

                    return Ok(InvalidInput);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("signin")]
        [AllowAnonymous]
        public async Task<IActionResult> AdminSignIn(SignIn signIn)
        {
            try
            {
                var dataresponse = await _userService.SignIn(signIn);
                return Ok(dataresponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("signout")]
        [Authorize(Roles = "Admin")]
        public string AdminSignOut()
        {
            _userService.SignOut();
            return "User Signout";
        }

        [HttpPost("changepassword")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {

            if (ModelState.IsValid)
            {
                //var UserID = User.Claims.FirstOrDefault().Value;
                var UserID = User.Claims.ToList().Where(p => p.Type == "Name").FirstOrDefault().Value;
                var result = await _userService.ChangePasswordAsync(model, UserID);
                return Ok(result);
            }
            return BadRequest();
        }

        #endregion

        [HttpPut]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Update(AdminListModel model)
        {
            var res = await _userService.UpdateAdminDetails(model);
            return Ok(res);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> get(int pageIndex = 1, int pageSize = 10, string? status = null)
        {
            var response = await _userService.GetAdmins(null, null, status, pageIndex, pageSize);
           return Ok(response);
        }

        [HttpGet("BygetNoSuperAdmin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getNoSuperAdmin(int pageIndex = 1, int pageSize = 10, string? status = null)
        {
            var response = await _userService.getNoSuperAdmin(pageIndex, pageSize, status);
            return Ok(response);
        }

        [HttpGet("ById")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getAdminId(string ID)
        {
            var response =await _userService.GetAdminById(ID);
            return Ok(response);
        }

        [HttpGet("ByEmail")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getAdminByEmail(string Email)
        {
            var response =await _userService.GetAdminByEmail(Email);
            return Ok(response);
        }


        [HttpGet("search")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> search(string? searchString = null, string? status = null, int pageIndex = 1, int pageSize = 10)
        {
            var response =await _userService.GetAdmins(null, searchString, status, pageIndex, pageSize);
            return Ok(response);
        }
    }
}
