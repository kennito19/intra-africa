using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.IDServer;
using API_Gateway.Models.Entity.User;
using DocumentFormat.OpenXml.Wordprocessing;
using Irony;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;

namespace API_Gateway.Controllers.User
{
    [Route("api/Account")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        BaseResponse<SignInResult> baseResponse = new BaseResponse<SignInResult>();
        public static string IDServerUrl = string.Empty;
        public string URL = string.Empty;
        private ApiHelper api;

        public UserAccountController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            api = new ApiHelper(_httpContext);
            _configuration = configuration;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
        }


        [HttpGet("GenerateNoAuthToken")]
        public ActionResult<string> GenerateNoAuthToken(string deviceId)
        {
            var response = api.ApiCall(IDServerUrl, EndPoints.GenerateNoAuth + "?deviceId=" + deviceId, "GET");

            JObject obj = new JObject();
            obj["Token"] = response.Content.ReadAsStringAsync().Result.ToString();

            return obj.ToString();
        }

        [HttpPost("Customer/Login")]
        public async Task<IActionResult> Login(SignIn model)
        {

            if (ModelState.IsValid)
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.CustomerSignIn, "POST", model);
                var result = response.Content.ReadAsStringAsync().Result;
                var rsf = JsonConvert.DeserializeObject<SignedInUserResponse>(result);

                if (rsf != null && rsf.Code == 200)
                {
                    BaseResponse<UserDetailsDTO> baseResponse1 = new BaseResponse<UserDetailsDTO>();

                    _httpContext.Request.Headers["device_id"] = model.DeviceId;
                    _httpContext.Request.Headers["Authorization"] = "Bearer " + rsf.Tokens.AccessToken;
                    api = new ApiHelper(_httpContext);

                    var response1 = api.ApiCall(URL, EndPoints.UserDetails + "?UserId=" + rsf.CurrentUser.UserId, "Get", null);
                    baseResponse1 = baseResponse1.JsonParseRecord(response1);

                    if (baseResponse1.code != 200)
                    {
                        UserDetailsDTO userDetails = (UserDetailsDTO)baseResponse1.Data;
                        if (userDetails.UserId == null && userDetails == null)
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
                            ud.IsEmailConfirmed = (bool)rsf.CurrentUser.IsEmailConfirmed;
                            ud.IsPhoneConfirmed = (bool)rsf.CurrentUser.IsPhoneConfirmed;
                            ud.UserType = "customer";
                            ud.CreatedBy = null;
                            ud.CreatedAt = DateTime.Now;
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

        [HttpPost("Customer/GuestCheckout")]
        public async Task<IActionResult> GuestCheckout(GuestUser model)
        {

            if (ModelState.IsValid)
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.GuestUser, "POST", model);
                var result = response.Content.ReadAsStringAsync().Result;
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("Customer/GenerateMobileOtp")]

        public async Task<IActionResult> GenerateMobileOtp(GenerateMobileOtp model)
        {
            BaseResponse<UserSignInResponseModel> baseResponse = new BaseResponse<UserSignInResponseModel>();
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.GenerateMobileOtp, "PUT", model);
                var datares = JsonConvert.DeserializeObject<BaseResponse<string>>(response.Content.ReadAsStringAsync().Result);
                baseResponse.code = datares.code;
                baseResponse.Message = datares.Message;
                baseResponse.pagination = null;
                baseResponse.Data = datares.Data;
            }
            else
            {
                return BadRequest(ModelState);
            }
            return Ok(baseResponse);
        }

        [HttpPost("Customer/LoginViaOtp")]
        public async Task<IActionResult> LoginViaOtp(SignInViaOtp model)
        {
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.CustomerSignInViaOtp, "POST", model);
                var result = response.Content.ReadAsStringAsync().Result;
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Customer/LoginViaEmail")]
        public async Task<IActionResult> SignInViaEmailId(SignInViaEmailId model)
        {
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.CustomerSignInViaEmail, "POST", model);
                var result = response.Content.ReadAsStringAsync().Result;
                return Ok(result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [Authorize(Roles = "Customer")]
        [HttpGet("Customer/ById")]
        public IActionResult GetCustomerById(string Id)
        {
            var response = api.ApiCall(IDServerUrl, EndPoints.CustomerById + "?ID=" + Id, "GET", null);
            BaseResponse<CustomerListModel> baseResponse = new BaseResponse<CustomerListModel>();

            if (response.IsSuccessStatusCode)
            {
                baseResponse = baseResponse.JsonParseRecord(response);
            }
            else
            {
                baseResponse = baseResponse.APICallFailed(response);
            }
            return Ok(baseResponse);
        }

        [HttpGet("Customer/ByEmail")]
        public IActionResult GetCustomerByEmail(string EmailId)
        {
            var response = api.ApiCall(IDServerUrl, EndPoints.CustomerByEmail + "?Email=" + EmailId, "GET", null);
            BaseResponse<CustomerListModel> baseResponse = new BaseResponse<CustomerListModel>();

            if (response.IsSuccessStatusCode)
            {
                baseResponse = baseResponse.JsonParseRecord(response);
            }
            else
            {
                baseResponse = baseResponse.APICallFailed(response);
            }
            return Ok(baseResponse);
        }

        [HttpPost("Customer/signUp")]

        public async Task<IActionResult> SignUp(CustomerSignup model)
        {
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.CustomerSignUp, "POST", model);
                var result = await response.Content.ReadAsStringAsync();
                var rsf = JsonConvert.DeserializeObject<SignedInUserResponse>(result);

                if (rsf.Code == 200)
                {
                    #region UserDetail Table Entry
                    UserDetails ud = new UserDetails();
                    ud.FirstName = model.FirstName;
                    ud.LastName = model.LastName;
                    ud.Status = "Pending";
                    ud.ProfileImage = null;
                    ud.Email = model.EmailID;
                    ud.UserId = rsf.CurrentUser.UserId;
                    ud.Phone = rsf.CurrentUser.Phone;
                    ud.Gender = rsf.CurrentUser.Gender;
                    ud.IsPhoneConfirmed = rsf.CurrentUser.IsPhoneConfirmed != null ? Convert.ToBoolean(rsf.CurrentUser.IsPhoneConfirmed) : false;
                    ud.IsEmailConfirmed = rsf.CurrentUser.IsEmailConfirmed != null ? Convert.ToBoolean(rsf.CurrentUser.IsEmailConfirmed) : false;
                    ud.UserType = "customer";
                    ud.CreatedBy = null;
                    ud.CreatedAt = DateTime.Now;
                    var resp = api.ApiCall(URL, EndPoints.UserDetails, "POST", ud);
                    #endregion

                    #region send mail
                    MailSendSES objses = new MailSendSES(_configuration);

                    string subject = "Welcome on Hashkart";
                    string htmlBody = "";
                    List<string> ReceiverEmail = new List<string>();

                    ReceiverEmail.Add(model.EmailID);

                    StreamReader reader = new StreamReader("Resources" + "\\EmailTemplate" + "\\user" + "\\welcome.html");
                    string readFile = reader.ReadToEnd();

                    var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";

                    readFile = readFile.Replace("{{image_server_path}}", baseUrl);

                    readFile = readFile.Replace("{{user_name}}", model.FirstName + " " + model.LastName);

                    htmlBody = readFile;

                    objses.sendMail(subject, htmlBody, ReceiverEmail);

                    #endregion
                }
                

                return Ok(rsf);

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Customer/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                BaseResponse<ResetModel> baseResponse = new BaseResponse<ResetModel>();
                var response = api.ApiCall(IDServerUrl, EndPoints.CustomerForgotPassword, "POST", model);
                baseResponse = baseResponse.JsonParseList(response);

                List<ResetModel> Resetdata = (List<ResetModel>)baseResponse.Data;

                if (Resetdata.Count >0)
                {
                    #region send mail
                    MailSendSES objses = new MailSendSES(_configuration);

                    string subject = "Forgot password request";
                    string htmlBody = "";
                    List<string> ReceiverEmail = new List<string>();

                    ReceiverEmail.Add(model.Email);

                    StreamReader reader = new StreamReader("Resources" + "\\EmailTemplate" + "\\user" + "\\forgot_password.html");
                    string readFile = reader.ReadToEnd();

                    var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";


                    readFile = readFile.Replace("{{user_name}}", Resetdata[0].UserName);
                    readFile = readFile.Replace("{{image_server_path}}", baseUrl);
                    readFile = readFile.Replace("{{reset_link}}", Resetdata[0].ResetLink);

                    htmlBody = readFile;

                    objses.sendMail(subject, htmlBody, ReceiverEmail);

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

        [HttpPost("Customer/ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPassword model)
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


        [HttpPost("Customer/ChangePassword")]
        [Authorize(Roles = "Customer")]
        public IActionResult ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.CustomerChangePassword, "POST", model);
                return Ok(baseResponse.JsonParseInputResponse(response));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("Customer/Update")]
        [Authorize(Roles = "Customer")]
        public IActionResult Update([FromForm] CustomerModel model)
        {
            CustomerListModel admin = new CustomerListModel();
            admin.Id = model.Id;
            admin.FirstName = model.FirstName;
            admin.LastName = model.LastName;
            admin.MobileNo = model.MobileNo;
            admin.Gender = model.Gender;
            admin.UserName = model.UserName;
            admin.ProfileImage = UpdateProfile(model.OldProfileImage, model.FirstName + "_" + model.LastName, model.FileName);
            admin.UserType = "Default";
            admin.IsEmailConfirmed = model.IsEmailConfirmed;
            admin.IsPhoneConfirmed = model.IsPhoneConfirmed;
            BaseResponse<CustomerListModel> baseResponse = new BaseResponse<CustomerListModel>();
            var response = api.ApiCall(IDServerUrl, EndPoints.CustomerUpdate, "PUT", admin);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            if (baseResponse.code == 200)
            {
                baseResponse.code = 200;
                baseResponse.Message = "Succeeded. User Updated Successfully";

                #region UserDetail Table Entry

                var res = api.ApiCall(URL, EndPoints.UserDetails + "/getUserDetails" + "?UserId=" + model.Id, "GET", null);
                BaseResponse<UserDetails> userBaseResponse = new BaseResponse<UserDetails>();
                userBaseResponse = userBaseResponse.JsonParseRecord(res);

                if(userBaseResponse.Data != null)
                {
                    UserDetails ud = (UserDetails)userBaseResponse.Data;
                    ud.FirstName = model.FirstName;
                    ud.LastName = model.LastName;
                    ud.Email = model.UserName;
                    ud.Phone = model.MobileNo;
                    ud.ProfileImage = admin.ProfileImage;
                    ud.Gender = model.Gender;
                    ud.IsEmailConfirmed = admin.IsEmailConfirmed;
                    ud.IsPhoneConfirmed = admin.IsPhoneConfirmed;
                    ud.UserType = "customer";
                    ud.ModifiedAt = DateTime.Now;
                    ud.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    var resp = api.ApiCall(URL, EndPoints.UserDetails, "PUT", ud);
                }
                else
                {
                    UserDetails ud = new UserDetails();
                    ud.UserId = model.Id;
                    ud.FirstName = model.FirstName;
                    ud.LastName = model.LastName;
                    ud.Email = model.UserName;
                    ud.Phone = model.MobileNo;
                    ud.Status = "Active";
                    ud.ProfileImage = admin.ProfileImage;
                    ud.Gender = model.Gender;
                    ud.IsEmailConfirmed = admin.IsEmailConfirmed;
                    ud.IsPhoneConfirmed = admin.IsPhoneConfirmed;
                    ud.UserType = "customer";
                    ud.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value; ;
                    ud.CreatedAt = DateTime.Now;
                    var resp = api.ApiCall(URL, EndPoints.UserDetails, "POST", ud);
                }

                
                #endregion
                
            }
            return Ok(baseResponse);
        }

        [HttpPost("Customer/logout")]
        [Authorize(Roles = "Customer")]
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

        [HttpGet("Customer/GetByToken")]
        [Authorize(Roles = "Customer")]
        public IActionResult GetByToken()
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            if (!string.IsNullOrEmpty(userID))
            {
                return GetCustomerById(userID);
            }
            else
            {
                return BadRequest(ModelState);
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
                    var fileName = Name + a;
                    var folderName = Path.Combine("Resources", "UserProfile");
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

    }
}
