using IDServer.Domain.DTO;
using IDServer.Domain.Entity;
using IDServer.Infrastructure.Auth;
using IDServerApplication.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWTTokenProvider.API.Controllers
{

    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly TokenProvider<Users> _sixDigitToken;
        private readonly UserManager<Users> _userManager;

        public AccountController(IUserService userService, TokenProvider<Users> sixDigitToken, UserManager<Users> userManager)
        {
            _userService = userService;
            _sixDigitToken = sixDigitToken;
            _userManager = userManager;
        }


        #region Basic Setup



        [HttpGet("/api/[controller]/roles")]
        [Authorize(Roles = "Admin")]
        public List<string> GetRoles()
        {
            try
            {
                return _userService.RolesList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        [HttpPost("/api/[controller]/createrole")]
        [Authorize(Roles = "Admin")]
        public IdentityResult CreateRole(string Name)
        {
            try
            {
                var dataresponse = _userService.CreateRole(Name);
                return dataresponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("/api/[controller]/createclaims")]
        [Authorize(Roles = "Admin")]
        public Task<IdentityResult> CreateRoleBaseClaims(string RoleID, string ClaimType, string ClaimValue)
        {
            try
            {

                var dataresponse = _userService.CreateRoleBaseClaims(RoleID, ClaimType, ClaimValue);
                return dataresponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("/api/[controller]/GetUserSpecificClaims")]
        [Authorize(Roles = "Admin")]
        public Task<UserClaimResponse> GetUserSpecificClaims(string userId)
        {
            try
            {

                var dataresponse = _userService.GetUserSpecificClaims(userId);
                return dataresponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Account features

        [HttpPost("/api/[controller]/forgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgetPassword forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null)
            {
                user = await _userManager.Users.Where(p => p.PhoneNumber == forgotPassword.Email).FirstOrDefaultAsync();
            }
            if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                if (!string.IsNullOrEmpty(forgotPassword.Email))
                {
                    var token = await _userService.GeneratePasswordChangeTokenAsync(forgotPassword);
                    return Ok(token);
                }
                return BadRequest();
            }
            else
            {
                BaseResponse<ResetModel> baseResponse = new BaseResponse<ResetModel>(204, "Invalid email address", null);
                return Ok(baseResponse);
            }
        }
        [HttpPost("/api/[controller]/sellerforgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> SellerForgotPassword(ForgetPassword forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null)
            {
                user = await _userManager.Users.Where(p => p.PhoneNumber == forgotPassword.Email).FirstOrDefaultAsync();
            }
            if (user != null && await _userManager.IsInRoleAsync(user, "Seller"))
            {
                if (!string.IsNullOrEmpty(forgotPassword.Email))
                {
                    var token = await _userService.GeneratePasswordChangeTokenAsync(forgotPassword);
                    return Ok(token);
                }
                return BadRequest();
            }
            else
            {
                BaseResponse<ResetModel> baseResponse = new BaseResponse<ResetModel>(204, "Invalid email address", null);
                return Ok(baseResponse);
            }


        }
        [HttpPost("/api/[controller]/customerforgotpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> CustomerForgotPassword(ForgetPassword forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if (user == null)
            {
                user = await _userManager.Users.Where(p => p.PhoneNumber == forgotPassword.Email).FirstOrDefaultAsync();
            }
            if (user != null && await _userManager.IsInRoleAsync(user, "Customer"))
            {
                if (!string.IsNullOrEmpty(forgotPassword.Email))
                {
                    var token = await _userService.GeneratePasswordChangeTokenAsync(forgotPassword);
                    return Ok(token);
                }
                return BadRequest();
            }
            else
            {
                BaseResponse<ResetModel> baseResponse = new BaseResponse<ResetModel>(204, "Invalid email address", null);
                return Ok(baseResponse);
            }


        }

        //Reset Password

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetPassword)
        {
            if (resetPassword != null)
            {
                if (!string.IsNullOrEmpty(resetPassword.uid) && !string.IsNullOrEmpty(resetPassword.token))
                {
                    var result = await _userService.ResetPasswordAsync(resetPassword, resetPassword.uid);
                    //if (result.Succeeded)
                    //{
                    //    return Ok("Success");
                    //}
                    return Ok(result);
                }

            }
            return BadRequest();
        }

        [HttpPost("/api/[controller]/userSession")]
        public async Task<IActionResult> UserSession(UserSessiondto model)
        {
            var response = _userService.UserSession(model);
            return Ok(response);
        }

        [HttpPost("/api/[controller]/logout")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public async Task<IActionResult> UserLogout(Logout model)
        {
            var response = _userService.UserLogout(model);
            return Ok(response);
        }

        #endregion


    }
}
