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
    public class CustomerController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public CustomerController(IUserService userService)
        {
            _userService = userService;
        }


        #region Customer

        [HttpPost("changepassword")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {

            if (ModelState.IsValid)
            {
                //var UserID = User.Claims.FirstOrDefault().Value;
                var UserID = User.Claims.ToList().Where(p => p.Type == "Name").FirstOrDefault().Value;
                var result = await _userService.ChangePasswordAsync(model, UserID);
                
                    return Ok(result);
                
            }
            return BadRequest(ModelState);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> CustomerSignUp(SignUp signUp)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    string uid = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault()!=null? HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value : null;
                    Users users = new Users();
                    users.FirstName = signUp.FirstName;
                    users.LastName = signUp.LastName;
                    users.Email = signUp.EmailID;
                    users.PhoneNumber = signUp.MobileNo;
                    users.UserName = signUp.EmailID;
                    users.Status = "Active";
                    users.Gender = signUp.Gender;
                    users.CreatedAt = DateTime.UtcNow;
                    users.CreatedBy = uid;
                    var dataresponse = await _userService.Create(users, signUp.Password, "Customer", signUp.DeviceId);
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
        public async Task<IActionResult> CustomerSignIn(SignIn signIn)
        {
            try
            {
                var dataresponse = await _userService.CustomerSignIn(signIn);
                return Ok(dataresponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("signout")]
        public string CustomerSignOut()
        {
            _userService.SignOut();
            return "User Signout";
        }

        [HttpPut("Update")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Update(CustomerListModel model)
        {
            var res = await _userService.UpdateCustomerDetails(model);
            return Ok(res);
        }

        [HttpPut("GenerateMobileOtp")]
        public async Task<IActionResult> GenerateMobileOTP(GenerateMobileOtp model)
        {
            var res = await _userService.GenerateMobileOTP(model);
            return Ok(res);
        }

        [HttpPost("signinViaOtp")]
        public async Task<IActionResult> signinViaOtp(SignInViaOtp signIn)
        {
            try
            {
                var dataresponse = await _userService.SignInViaOTP(signIn);
                return Ok(dataresponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("signInViaEmailId")]
        public async Task<IActionResult> SignInViaEmailId(SignInViaEmailId signIn)
        {
            try
            {
                var dataresponse = await _userService.SignInViaEmailId(signIn);
                return Ok(dataresponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(CustomerListModel model)
        {
            try
            {
                Users users = new Users();
                users.Id = model.Id;
                users.DeletedAt = DateTime.UtcNow;
                users.DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                users.IsDeleted = true;
                var dataresponse = await _userService.DeleteSellerDetails(users);
                return Ok(dataresponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        //[Authorize(Roles = "Admin, Seller, Customer")]
        [Authorize(Policy = "General")]
        public async Task<IActionResult> get(int pageIndex = 1, int pageSize = 10)
        {
            var listAdmins =await _userService.GetCustomers(pageIndex,pageSize);
            return Ok(listAdmins);
        }

        [HttpGet("ById")]
        //[Authorize(Roles = "Admin, Seller, Customer")]
        [Authorize(Policy = "General")]
        public async Task<IActionResult> getCustomerById(string ID)
        {
            var response = await _userService.GetCustomerByID(ID);
            return Ok(response);
        }

        [HttpGet("ByEmail")]
        [Authorize(Policy = "General")]
        public async Task<IActionResult> getCustomerByEmail(string Email)
        {
            var response =await _userService.GetCustomerByEmail(Email);
            return Ok(response);
        }


        [HttpGet("search")]
        //[Authorize(Roles = "Admin, Seller, Customer")]
        [Authorize(Policy = "General")]
        public async Task<IActionResult> search(int pageIndex = 1, int pageSize = 10, string? search = null)
        {
            var response =await _userService.GetCustomers(pageIndex, pageSize, search);
            return Ok(response);
        }

        [HttpPost("GuestUser")]
        public async Task<IActionResult> GuestUser(GuestUser guest)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string uid = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault()!=null? HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value : null;
                    var dataresponse = await _userService.Guestcheckout(guest, uid);
                    return Ok(dataresponse);
                }
                else
                {
                    BaseResponse<string> InvalidInput = new BaseResponse<string>(204, "Invalid Input");
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

        #endregion
    }
}
