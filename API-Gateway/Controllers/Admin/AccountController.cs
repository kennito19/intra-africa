using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.IDServer;
using API_Gateway.Models.Entity.Order;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using SignedInUserResponse = API_Gateway.Models.Dto.SignedInUserResponse;
using UserSignInResponseModel = API_Gateway.Models.Dto.UserSignInResponseModel;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper api;
        BaseResponse<SignInResult> baseResponse = new BaseResponse<SignInResult>();
        public static string IDServerUrl = string.Empty;

        public AccountController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpContext = _httpContextAccessor.HttpContext;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            api = new ApiHelper(_httpContext);
        }

        [HttpPost("Admin/Login")]
        public async Task<IActionResult> Login(SignIn model)
        {
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.AdminSignIn, "POST", model);
                var result = await response.Content.ReadAsStringAsync();
                var rsf = JsonConvert.DeserializeObject<SignedInUserResponse>(result);
                return Ok(rsf);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpPost("Admin/ChangePassword")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePassword(ChangePassword model)
        {
            if (ModelState.IsValid)
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.AdminChangePassword, "POST", model);
                return Ok(baseResponse.JsonParseInputResponse(response));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Admin/ForgotPassword")]
        public ActionResult<ApiHelper> ForgotPassword(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                BaseResponse<ResetModel> baseResponse = new BaseResponse<ResetModel>();
                var response = api.ApiCall(IDServerUrl, EndPoints.ForgotPassword, "POST", model);
                baseResponse = baseResponse.JsonParseList(response);

                List<ResetModel> Resetdata = (List<ResetModel>)baseResponse.Data;

                if (Resetdata.Count > 0)
                {
                    #region send mail
                    MailSendSES objses = new MailSendSES(_configuration);

                    string subject = "Forgot password request";
                    string htmlBody = "";
                    List<string> ReceiverEmail = new List<string>();

                    ReceiverEmail.Add(model.Email);

                    StreamReader reader = new StreamReader("Resources" + "\\EmailTemplate" + "\\admin" + "\\forgot_password.html");
                    string readFile = reader.ReadToEnd();

                    var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";


                    readFile = readFile.Replace("{{admin_name}}", Resetdata[0].UserName);
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

        [HttpPost("Admin/ResetPassword")]
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

        [HttpPost("Admin/SignUp")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SignUp([FromForm] AdminSignUpForm model)
        {
            if (ModelState.IsValid)
            {
                AdminSignUp newModel = new AdminSignUp();
                newModel.EmailID = model.EmailID;
                newModel.LastName = model.LastName;
                newModel.FirstName = model.FirstName;
                newModel.UserTypeId = model.UserTypeId;
                newModel.Password = model.Password;
                newModel.MobileNo = model.MobileNo;
                newModel.ProfileImage = UploadProfile(model.FirstName + " " + model.LastName, model.FileName);
                newModel.ReceiveNotifications = model.ReceiveNotifications;
                newModel.DeviceId = null;


                var response = api.ApiCall(IDServerUrl, EndPoints.AdminSignUp, "POST", newModel);
                var result = await response.Content.ReadAsStringAsync();
                var rsf = JsonConvert.DeserializeObject<SignedInUserResponse>(result);
                return Ok(rsf);

                //var datares = JsonConvert.DeserializeObject<BaseResponse<string>>(response.Content.ReadAsStringAsync().Result);
                //BaseResponse<UserSignInResponseModel> baseResponse = new BaseResponse<UserSignInResponseModel>();
                //if (response.IsSuccessStatusCode)
                //{
                //    if (datares.code == 200)
                //    {
                //        baseResponse = baseResponse.ParseSignUpResponse(response);
                //    }
                //    else
                //    {
                //        baseResponse.code = datares.code;
                //        baseResponse.Message = datares.Message;
                //        baseResponse.pagination = null;
                //        baseResponse.Data = null;

                //    }
                //}
                //else
                //{
                //    baseResponse.code = 400;
                //    baseResponse.Message = "Error in Creating new Admin";
                //}
                //return Ok(baseResponse);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("Token/GetNewTokens")]
        public async Task<IActionResult> GetNewTokens(RequestNewTokenModel model)
        {
            var response = api.ApiCall(IDServerUrl, EndPoints.GetNewAccessToken, "POST", model);
            var result = response.Content.ReadAsStringAsync().Result;
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("User/AccessInfo")]
        public IActionResult GetUserRoleClaims(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.GetUserRoleClaims + "?UserId=" + id, "GET");
                var baseResponse = new BaseResponse<UserClaimResponse>();
                var claimResponse = baseResponse.Parser(response);

                return Ok(claimResponse);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("User/UserAllowedPages")]
        public IActionResult GetUserSpecificClaims(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.GetUserSpecificClaims + "?userId=" + id, "GET");
                var baseResponse = new BaseResponse<UserClaimResponse>();
                var claimResponse = baseResponse.Parser(response);

                return Ok(claimResponse);
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Admin/Update")]
        public async Task<IActionResult> Update([FromForm] AdminModel model)
        {
            AdminListModel admin = new AdminListModel();
            admin.Id = model.Id;
            admin.UserName = model.UserName;
            admin.FirstName = model.FirstName;
            admin.LastName = model.LastName;
            admin.MobileNo = model.MobileNo;
            admin.ProfileImage = UpdateProfile(model.OldProfileImage, model.FirstName + " " + model.LastName, model.FileName);
            admin.Status = model.Status;
            admin.UserTypeId = model.UserTypeId;
            admin.UserType = model.UserType;
            admin.ReceiveNotifications = model.ReceiveNotifications;

            BaseResponse<AdminListModel> baseResponse = new BaseResponse<AdminListModel>();
            var response = api.ApiCall(IDServerUrl, EndPoints.AdminList, "PUT", admin);
            baseResponse = baseResponse.JsonParseInputResponse(response);

            if (baseResponse.code == 200)
            {
                baseResponse.code = 200;
                baseResponse.Message = "Profile updated successfully";
                baseResponse.Data = admin;
            }
            return Ok(baseResponse);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/List")]
        public IActionResult GetAdmins(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(IDServerUrl, EndPoints.AdminList + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
            BaseResponse<AdminListModel> baseResponse = new BaseResponse<AdminListModel>();

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

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/NoSuperAdminList")]
        public IActionResult GetNoSuperAdminList(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(IDServerUrl, EndPoints.AdminList + "/BygetNoSuperAdmin?pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
            BaseResponse<AdminListModel> baseResponse = new BaseResponse<AdminListModel>();

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

        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/Search")]
        public IActionResult GetAdminsSearch(string? searchtext = null, string? status = null, int pageIndex = 1, int pageSize = 10)
        {   // here i add Search additionalyy due to avoiding unwanted Endpoints.

            string url = string.Empty;

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchString=" + HttpUtility.UrlEncode(searchtext);
            }

            if (!string.IsNullOrEmpty(status))
            {
                url += "&status=" + status;
            }

            var response = api.ApiCall(IDServerUrl, EndPoints.AdminList + "/search" + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize + url, "GET", null);
            BaseResponse<AdminListModel> baseResponse = new BaseResponse<AdminListModel>();

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


        [Authorize(Roles = "Admin")]
        [HttpGet("Admin/ById")]
        public IActionResult GetAdminById(string Id)
        {
            var response = api.ApiCall(IDServerUrl, EndPoints.AdminList + "/ById?ID=" + Id, "GET", null);
            BaseResponse<AdminListModel> baseResponse = new BaseResponse<AdminListModel>();

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

        [Authorize(Roles = "Admin")]
        [HttpPost("Admin/logout")]
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
                var file = FileName;


                var folderName = Path.Combine("Resources", "UserProfile");

                if (file != null)
                {
                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + a;
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    fileName = imageUpload.UploadImageAndDocs(fileName, folderName, FileName);

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
                    var fileName = Name + a;
                    var folderName = Path.Combine("Resources", "UserProfile");
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    fileName = imageUpload.UpdateUploadImageAndDocs(OldName, fileName, folderName, FileName);

                    return fileName;
                }
                else
                {
                    string fileName = null;
                    if (!string.IsNullOrEmpty(OldName))
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
