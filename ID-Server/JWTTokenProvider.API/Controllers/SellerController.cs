using IDServer.Domain.DTO;
using IDServer.Domain.Entity;
using IDServerApplication.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWTTokenProvider.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        private readonly IUserService _userService;
        public SellerController(IUserService userService)
        {
            _userService = userService;
        }


        #region Seller

        [HttpPost("signup")]
        public async Task<IActionResult> SellerSignUp(SignUp signUp)
        {
            try
            {
                
                if (ModelState.IsValid)
                {
                    string uid = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault() != null ? HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value : null;
                    Users users = new Users();
                    users.FirstName = signUp.FirstName;
                    users.LastName = signUp.LastName;
                    users.Email = signUp.EmailID;
                    users.PhoneNumber = signUp.MobileNo;
                    users.UserName = signUp.EmailID;
                    users.Status = "Pending";
                    users.UserTypeId = signUp.UserTypeId;
                    users.CreatedAt = DateTime.UtcNow;
                    users.CreatedBy = uid;
                    var dataresponse = await _userService.Create(users, signUp.Password, "Seller", signUp.DeviceId);
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
        public async Task<IActionResult> SellerSignIn(SignIn signIn)
        {
            try
            {
                var dataresponse = await _userService.SellerSignIn(signIn);
                return Ok(dataresponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("signout")]
        public string SellerSignOut()
        {
            _userService.SignOut();
            return "User Signout";
        }


        [HttpPut]
        [Authorize(Roles = "Admin, Seller")]
        public async Task<IActionResult> Update(SellerListModel Seller)
        {
            try
            {
                //Users users = new Users();
                Seller.ModifiedAt = DateTime.UtcNow;
                Seller.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var dataresponse = await _userService.UpdateSellerDetails(Seller);
                return Ok(dataresponse);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPut("delete")]
        [Authorize(Roles = "Admin, Seller")]
        public async Task<IActionResult> Delete(SellerListModel Seller)
        {
            try
            {
                Users users = new Users();
                users.Id = Seller.Id;
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
        public async Task<IActionResult> get(int pageIndex = 1, int pageSize = 10, string? searchString = null, string? fromDate = null, string? toDate = null, string? status = null, bool? IsArchive = null)
        {
            var response = await _userService.GetSellers(pageIndex, pageSize, searchString, fromDate, toDate, status, IsArchive);
            return Ok(response);
        }
        
        [HttpGet("ById")]
        //[Authorize(Roles = "Admin, Seller, Customer")]
        [Authorize(Policy = "General")]
        public async Task<IActionResult> getSellerId(string ID)
        {
            var response = await _userService.GetSellerByID(ID);
            return Ok(response);
        }

        [HttpGet("ByEmail")]
        //[Authorize(Roles = "Admin, Seller, Customer")]
        [Authorize(Policy = "General")]
        public async Task<IActionResult> getSellerByEmail(string Email)
        {
            var response = await _userService.GetSellerByEmail(Email);
            return Ok(response);
        }


        [HttpGet("search")]
        //[Authorize(Roles = "Admin, Seller, Customer")]
        [Authorize(Policy = "General")]
        public async Task<IActionResult> search(int pageIndex = 1, int pageSize = 10, string? search = null)
        {
            var response = await _userService.GetSellers(pageIndex, pageSize, search);
            return Ok(response);
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                //var SellerId = User.Claims.FirstOrDefault().Value;
                var SellerId = User.Claims.ToList().Where(p => p.Type == "Name").FirstOrDefault().Value;
                var result = await _userService.ChangePasswordAsync(model, SellerId);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        #endregion
    }
}
