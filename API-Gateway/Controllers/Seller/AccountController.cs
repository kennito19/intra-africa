using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;
using API_Gateway.Models.Entity.IDServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SignedInUserResponse = API_Gateway.Models.Dto.SignedInUserResponse;
using UserSignInResponseModel = API_Gateway.Models.Dto.UserSignInResponseModel;
using API_Gateway.Common;

namespace API_Gateway.Controllers.Seller
{
    [Route("api/Account")]
    [ApiController]
    public class SellerAccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        public static string IDServerUrl = string.Empty;
        BaseResponse<SignInResult> baseResponse = new BaseResponse<SignInResult>();
        private ApiHelper api;
        public string URL = string.Empty;
        public SellerAccountController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            api = new ApiHelper(_httpContext);
        }

        [HttpPost("Seller/Login")]
        public async Task<IActionResult> Login(SignIn model)
        {
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.SellerSignIn, "POST", model);
                var result = response.Content.ReadAsStringAsync().Result;
                var rsf = JsonConvert.DeserializeObject<SignedInUserResponse>(result);
                if (rsf != null && rsf.Code == 200)
                {
                    BaseResponse<UserDetailsDTO> baseResponse1 = new BaseResponse<UserDetailsDTO>();
                    var response1 = api.ApiCall(URL, EndPoints.UserDetails + "?UserId=" + rsf.CurrentUser.UserId, "Get", null);
                    baseResponse1 = baseResponse1.JsonParseRecord(response1);

                    if (baseResponse1.code == 200)
                    {
                        UserDetailsDTO userDetails = baseResponse1.Data as UserDetailsDTO;
                        if (userDetails == null || userDetails.UserId == null)
                        {
                            #region UserDetail Table Entry
                            string[] fullname = rsf.CurrentUser.FullName.Split(" ");
                            UserDetails ud = new UserDetails();
                            ud.FirstName = fullname[0];
                            ud.LastName = fullname[fullname.Count() - 1];
                            ud.Status = rsf.CurrentUser.Status;
                            ud.ProfileImage = rsf.CurrentUser.ProfileImage;
                            ud.Email = rsf.CurrentUser.UserName;
                            ud.UserId = rsf.CurrentUser.UserId;
                            ud.Phone = rsf.CurrentUser.Phone;
                            ud.Gender = rsf.CurrentUser.Gender;
                            ud.IsEmailConfirmed = rsf.CurrentUser.IsEmailConfirmed == true;
                            ud.IsPhoneConfirmed = rsf.CurrentUser.IsPhoneConfirmed == true;
                            ud.UserType = "seller";
                            ud.CreatedAt = DateTime.Now;
                            ud.CreatedBy = null;
                            var resp = api.ApiCall(URL, EndPoints.UserDetails, "POST", ud);
                            #endregion
                        }
                    }
                }
                return Ok(rsf);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost("Seller/SignUp")]
        public async Task<IActionResult> SignUp(SignUp model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.SellerSignUp, "POST", model);
                var result = response.Content == null ? string.Empty : await response.Content.ReadAsStringAsync();

                if (response == null)
                {
                    BaseResponse<string> nullResponse = new BaseResponse<string>();
                    nullResponse.code = 500;
                    nullResponse.Message = "No response from identity service";
                    return Ok(nullResponse);
                }

                if (!response.IsSuccessStatusCode)
                {
                    if (string.IsNullOrWhiteSpace(result))
                    {
                        BaseResponse<string> failedResponse = new BaseResponse<string>();
                        failedResponse.code = (int)response.StatusCode;
                        failedResponse.Message = "Seller signup failed";
                        return StatusCode((int)response.StatusCode, failedResponse);
                    }

                    return StatusCode((int)response.StatusCode, result);
                }

                SignedInUserResponse? rsf = null;
                try
                {
                    rsf = JsonConvert.DeserializeObject<SignedInUserResponse>(result);
                }
                catch
                {
                    BaseResponse<string> invalidJson = new BaseResponse<string>();
                    invalidJson.code = 500;
                    invalidJson.Message = "Invalid signup response format from identity service";
                    return Ok(invalidJson);
                }

                if (rsf == null)
                {
                    BaseResponse<string> invalidResponse = new BaseResponse<string>();
                    invalidResponse.code = 500;
                    invalidResponse.Message = "Invalid signup response from identity service";
                    return Ok(invalidResponse);
                }

                if (string.Equals(rsf.Message, "account created successfully", StringComparison.OrdinalIgnoreCase))
                {
                    #region UserDetail Table Entry
                    UserDetails ud = new UserDetails();
                    ud.FirstName = model.FirstName;
                    ud.LastName = model.LastName;
                    ud.Status = "Pending";
                    ud.ProfileImage = null;
                    ud.Email = model.EmailID;
                    ud.UserId = rsf.CurrentUser?.UserId;
                    ud.Phone = rsf.CurrentUser?.Phone;
                    ud.Gender = rsf.CurrentUser?.Gender;
                    ud.IsPhoneConfirmed = rsf.CurrentUser?.IsPhoneConfirmed != null ? Convert.ToBoolean(rsf.CurrentUser.IsPhoneConfirmed) : false;
                    ud.IsEmailConfirmed = rsf.CurrentUser?.IsEmailConfirmed != null ? Convert.ToBoolean(rsf.CurrentUser.IsEmailConfirmed) : false;
                    ud.UserType = "seller";
                    ud.CreatedAt = DateTime.Now;
                    ud.CreatedBy = null;

                    // If CurrentUser is not returned by identity service, keep signup response stable and skip profile creation.
                    if (!string.IsNullOrWhiteSpace(ud.UserId))
                    {
                        try
                        {
                            var resp = api.ApiCall(URL, EndPoints.UserDetails, "POST", ud);
                        }
                        catch
                        {
                            // Keep seller signup successful even if UserDetails service is unavailable.
                        }
                    }
                    #endregion

                    #region send mail
                    try
                    {
                        string templatePath = Path.Combine("Resources", "EmailTemplate", "seller", "welcome.html");
                        if (System.IO.File.Exists(templatePath))
                        {
                            MailSendSES objses = new MailSendSES(_configuration);
                            string subject = "Welcome TO Hashkart";
                            string htmlBody = "";
                            List<string> ReceiverEmail = new List<string>();

                            ReceiverEmail.Add(model.EmailID);

                            StreamReader reader = new StreamReader(templatePath);
                            string readFile = reader.ReadToEnd();

                            var request = _httpContextAccessor?.HttpContext?.Request;
                            var baseUrl = request == null
                                ? string.Empty
                                : $"{request.Scheme}://{request.Host}/";

                            readFile = readFile.Replace("{{image_server_path}}", baseUrl);
                            readFile = readFile.Replace("{{seller_name}}", model.FirstName + " " + model.LastName);

                            htmlBody = readFile;
                            objses.sendMail(subject, htmlBody, ReceiverEmail);
                        }
                    }
                    catch
                    {
                        // Keep seller signup successful even if email template/config is missing.
                    }
                    #endregion


                }
                return Ok(rsf);
            }
            catch (Exception ex)
            {
                BaseResponse<string> errorResponse = new BaseResponse<string>();
                errorResponse.code = 500;
                errorResponse.Message = $"Seller signup failed: {ex.Message}";
                return Ok(errorResponse);
            }
        }

        [Authorize(Roles = "Admin,Seller")]
        [HttpPut("Seller/UpdateProfile")]
        public async Task<IActionResult> Update(SellerListModel model)
        {
            var response = api.ApiCall(IDServerUrl, EndPoints.SellerList, "PUT", model);
            BaseResponse<SellerListModel> baseResponse = new BaseResponse<SellerListModel>();

            if (response.IsSuccessStatusCode)
            {
                baseResponse.code = 200;
                baseResponse.Message = "Succeeded. Seller Updated Successfully";
            }
            else
            {
                baseResponse = baseResponse.APICallFailed(response);
            }
            return Ok(baseResponse);
        }


        [HttpPost("Seller/ForgotPassword")]
        public ActionResult<ApiHelper> ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                BaseResponse<ResetModel> baseResponse = new BaseResponse<ResetModel>();
                var response = api.ApiCall(IDServerUrl, EndPoints.SellerForgotPassword, "POST", model);
                baseResponse = baseResponse.JsonParseList(response);

                List<ResetModel> Resetdata = baseResponse.Data as List<ResetModel> ?? new List<ResetModel>();

                if (Resetdata.Count > 0)
                {
                    #region send mail
                    try
                    {
                        string templatePath = Path.Combine("Resources", "EmailTemplate", "seller", "forgot_password.html");
                        if (System.IO.File.Exists(templatePath))
                        {
                            MailSendSES objses = new MailSendSES(_configuration);
                            string subject = "Forgot password request";
                            string htmlBody = "";
                            List<string> ReceiverEmail = new List<string>();

                            ReceiverEmail.Add(model.Email);

                            StreamReader reader = new StreamReader(templatePath);
                            string readFile = reader.ReadToEnd();

                            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";

                            readFile = readFile.Replace("{{seller_name}}", Resetdata[0].UserName);
                            readFile = readFile.Replace("{{image_server_path}}", baseUrl);
                            readFile = readFile.Replace("{{reset_link}}", Resetdata[0].ResetLink);

                            htmlBody = readFile;
                            objses.sendMail(subject, htmlBody, ReceiverEmail);
                        }
                    }
                    catch
                    {
                        // Keep forgot-password response stable even if email template/config is missing.
                    }

                    #endregion

                    baseResponse.code = 200;
                    baseResponse.Message = "Reset password link sent to your register mail address";
                    baseResponse.Data = null;
                }
                else
                {
                    baseResponse.code = 201;
                    baseResponse.Message = "User valid Email";
                }

                return Ok(baseResponse);
                //return Ok(JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Seller/ChangePassword")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.SellerChangePassword, "POST", model);
                return Ok(baseResponse.JsonParseInputResponse(response));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Seller/ResetPassword")]
        public ActionResult<ApiHelper> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                BaseResponse<string> baseResponse1 = new BaseResponse<string>();
                var response = api.ApiCall(IDServerUrl, EndPoints.ResetPassword, "POST", model);
                return Ok(baseResponse1.JsonParseInputResponse(response));
                //return Ok(JsonConvert.DeserializeObject<string>(response.Content.ReadAsStringAsync().Result));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "Admin, Seller")]
        [HttpGet("Seller/List")]
        public IActionResult Getseller(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(IDServerUrl, EndPoints.SellerList + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
            BaseResponse<SellerListModel> baseResponse = new BaseResponse<SellerListModel>();

            if (response.IsSuccessStatusCode)
            {
                baseResponse = baseResponse.JsonParseList(response);
            }
            else
            {
                baseResponse = baseResponse.APICallFailed(response);
            }
            return Ok(baseResponse);
        }

        [HttpPost("Seller/logout")]
        [Authorize(Roles = "Seller")]
        public ActionResult<ApiHelper> logout(Logout model)
        {
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.logout, "POST", model);
                var result = response.Content.ReadAsStringAsync().Result;
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [NonAction]
        public string UploadProfile(string Name, IFormFile FileName)
        {

            try
            {
                //var file = Request.Form.Files[0];

                var file = FileName;
                if (!System.IO.Directory.Exists("Resources" + "\\SellerProfile"))
                {
                    System.IO.Directory.CreateDirectory("Resources" + "\\SellerProfile");
                }

                var folderName = Path.Combine("Resources", "SellerProfile");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file != null)
                {
                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + a + Path.GetExtension(file.FileName);
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
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
        public string UpdateProfile(string OldName, string Name, IFormFile? FileName)
        {

            try
            {
                if (FileName != null)
                {
                    var file = FileName;

                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + a + Path.GetExtension(file.FileName);
                    var folderName = Path.Combine("Resources", "SellerProfile");
                    var Isdelete = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    var fullPath = Path.Combine(Isdelete, OldName);
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }
                    var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    fileName = Name + a + Path.GetExtension(file.FileName);
                    fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    FileStream fs;
                    using (fs = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(fs);
                    }
                    fs.Close();
                    return fileName;
                }
                else
                {
                    string fileName = null;
                    if (OldName != string.Empty)
                    {
                        string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                        fileName = Name + a + Path.GetExtension(OldName);
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
