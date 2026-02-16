using IDServer.Domain.DTO;
using IDServer.Domain.Entity;
using IDServer.Infrastructure.Auth;
using IDServer.Infrastructure.Data;
using IDServer.Infrastructure.LoggerClass;
using IDServerApplication.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace IDServer.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<Users> _userManager;
        private readonly TokenProvider<Users> _tokenProvider;
        private readonly SignInManager<Users> _signInManager;
        private readonly AspNetIdentityDBContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenManagerRepository _tokenManager;
        private readonly IPageRoleRepository _pageRoleRepository;
        private static readonly Random Random = new Random();
        public UserRepository(UserManager<Users> userManager, TokenProvider<Users> tokenProvider, RoleManager<IdentityRole> roleManager, IConfiguration configuration, AspNetIdentityDBContext dbContext, SignInManager<Users> signInManager, ITokenManagerRepository tokenManager, IPageRoleRepository pageRoleRepository)
        {
            _userManager = userManager;
            _tokenProvider = tokenProvider;
            _roleManager = roleManager;
            _configuration = configuration;
            _dbContext = dbContext;
            _signInManager = signInManager;
            _tokenManager = tokenManager;
            _pageRoleRepository = pageRoleRepository;
        }

        public string GenerateNoAuthToken(string deviceId)
        {
            string token = _tokenManager.GenerateNoAuthToken();

            var sessions = _dbContext.UserDeviceSessions.Where(x => String.IsNullOrEmpty(x.UserId) && String.IsNullOrEmpty(x.RefreshToken) && x.RefreshTokenExpiryTime < DateTime.UtcNow).ToList();
            if (sessions.Any())
            {
                foreach (UserSessions item in sessions)
                {
                    _dbContext.UserDeviceSessions.Remove(item);
                    _dbContext.SaveChanges();
                }
            }


            UserSessions Usersessions = new UserSessions(null, deviceId, null, DateTime.UtcNow.AddDays(1), token, DateTime.UtcNow);
            _dbContext.Add(Usersessions);
            _dbContext.SaveChanges();
            return token;
        }
        public async Task<BaseResponse<string>> ChangePasswordAsync(ChangePassword model, string UserID)
        {
            var user = await _userManager.FindByEmailAsync(UserID);
            var res = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (res.Succeeded)
            {
                BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Password Changed Successfully");
                return baseResponse;
            }
            else
            {
                BaseResponse<string> baseResponse = new BaseResponse<string>(204, "Invalid Current Password.");
                return baseResponse;
            }

        }

        public async Task<BaseResponse<string>> ResetPasswordAsync(ResetPassword model, string UserID)
        {
            var user = await _userManager.FindByIdAsync(UserID);
            BaseResponse<string> baseResponse = new BaseResponse<string>(200, "");
            var res = new IdentityResult();
            var token = await _userManager.GetAuthenticationTokenAsync(user, "Default", "Reset");
            if (token == model.token)
            {
                res = await _tokenProvider.ResetPasswordAsync(user, model.token, model.NewPassword);
                if (res.Succeeded)
                {
                    await _userManager.RemoveAuthenticationTokenAsync(user, "Default", "Reset");
                    baseResponse = new BaseResponse<string>(200, "Password Reset successfully.");
                }
                else
                {
                    baseResponse = new BaseResponse<string>(400, "Password reset failed.");
                }
            }
            else
            {
                baseResponse = new BaseResponse<string>(400, "The reset password link has expired. Please generate a new link.");
            }

            return baseResponse;
        }

        public async Task<BaseResponse<ResetModel>> GeneratePasswordChangeTokenAsync(ForgetPassword forgotPassword)
        {
            var user = await _userManager.FindByNameAsync(forgotPassword.Email);
            List<ResetModel> _resetModel = new List<ResetModel>();
            ResetModel resetModel = new ResetModel();
            BaseResponse<ResetModel> baseResponse = new BaseResponse<ResetModel>(200, "", null, resetModel);

            var token = "";
            if (user != null)
            {
                token = await _tokenProvider.GeneratePasswordResetTokenAsync();
                var tokenRes = await _userManager.SetAuthenticationTokenAsync(user, "Default", "Reset", token);

                if (tokenRes.Succeeded)
                {
                    string appDomain = string.Empty;
                    string confirmationLink = string.Empty;

                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        appDomain = _configuration.GetSection("reset").GetSection("admin").GetSection("domain").Value;
                        confirmationLink = _configuration.GetSection("reset").GetSection("admin").GetSection("resetlink").Value;
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Seller"))
                    {
                        appDomain = _configuration.GetSection("reset").GetSection("seller").GetSection("domain").Value;
                        confirmationLink = _configuration.GetSection("reset").GetSection("seller").GetSection("resetlink").Value;
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Customer"))
                    {
                        appDomain = _configuration.GetSection("reset").GetSection("customer").GetSection("domain").Value;
                        confirmationLink = _configuration.GetSection("reset").GetSection("customer").GetSection("resetlink").Value;
                    }

                    string resetLink = confirmationLink.Replace("{token}", token).Replace("{userId}", user.Id.ToString());

                    string Link = string.Format("{0}/{1}", appDomain, resetLink);

                    resetModel.ResetLink = Link;
                    resetModel.UserName = user.FirstName + " " + user.LastName.ToString();

                    _resetModel.Add(resetModel);

                    baseResponse = new BaseResponse<ResetModel>(200, "Reset password link generated successfully", null, _resetModel);
                }
            }
            else
            {
                baseResponse = new BaseResponse<ResetModel>(204, "Invalid user", null, resetModel);
            }
            return baseResponse;
        }

        public IdentityResult CreateRole(string Name)
        {
            try
            {
                var returndata = new IdentityResult();
                if (!_roleManager.RoleExistsAsync(Name).GetAwaiter().GetResult())
                {
                    returndata = _roleManager.CreateAsync(new IdentityRole(Name)).GetAwaiter().GetResult();
                }
                return returndata;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IdentityResult> CreateRoleBaseClaims(string RoleID, string ClaimType, string ClaimValue)
        {
            try
            {

                IdentityRole roles = await _roleManager.FindByIdAsync(RoleID);
                if (roles != null)
                {
                    var claims = _dbContext.RoleClaims.Where(p => p.RoleId == RoleID && p.ClaimType == ClaimType && p.ClaimValue == ClaimValue).ToList();
                    var returndata = new IdentityResult();
                    if (!claims.Any())
                    {
                        returndata = await _roleManager.AddClaimAsync(roles, new Claim(ClaimType, ClaimValue));
                    }

                    return returndata;
                }
                else
                {
                    var identityResult = IdentityResult.Failed(new IdentityError[] {
                      new IdentityError{
                         Code = "201",
                         Description = "Record Not Found"
                      } });
                    return identityResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SignedInUserResponse> Create(Users user, string Password, string roleName, string? DeviceId = null)
        {
            try
            {
                SignedInUserResponse signedInUser = null;
                BaseResponse<string> baseResponse = new BaseResponse<string>(200, "Initial");
                UserSignInResponseModel signupResponse = null;
                if (!_userManager.Users.Where(x => x.UserName == user.UserName).Any())
                {
                    if (!_userManager.Users.Where(x => x.PhoneNumber == user.PhoneNumber).Any())
                    {
                        var role = _roleManager.FindByNameAsync(roleName).Result;
                        if (role != null)
                        {

                            var returndata = await _userManager.CreateAsync(user, Password);
                            if (returndata.Succeeded)
                            {
                                await _userManager.AddToRoleAsync(user, role.Name);
                                var userSign = await _userManager.FindByEmailAsync(user.Email);
                                var roles = await _userManager.GetRolesAsync(userSign);
                                string userType = _dbContext.UserTypes.Where(x => x.Id == userSign.UserTypeId).Select(x => x.Name).FirstOrDefault();
                                signupResponse = new UserSignInResponseModel(userSign, userType, roles);
                                baseResponse.Message = "Account Created Successfully";
                                if (roleName.ToLower() != "admin")
                                {

                                    var _user = await _userManager.FindByEmailAsync(user.Email);
                                    if (user != null && await _userManager.IsInRoleAsync(user, roleName))
                                    {
                                        string getUserType = "default";

                                        var Refreshdays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]);

                                        var refreshTk = GenerateRefreshToken();
                                        //var tokenExpiry = DateTime.Now.AddDays(Refreshdays);
                                        var tokenExpiry = DateTime.UtcNow.AddDays(Refreshdays);

                                        var userRoles = await _userManager.GetRolesAsync(user);

                                        var ClaimList = await loadClaims(user, getUserType, userRoles);

                                        string accesstoken = _tokenManager.GenerateToken(user, ClaimList);
                                        UserSessions sessions = new UserSessions(user.Id, DeviceId, refreshTk, tokenExpiry, accesstoken, DateTime.UtcNow);
                                        _dbContext.Add(sessions);
                                        _dbContext.SaveChanges();

                                        UserSignInResponseModel userSignInResponse = new UserSignInResponseModel(user, getUserType, userRoles);
                                        TokenModel tokenModel = new TokenModel(accesstoken, refreshTk, 200, "Token generated successfully.");
                                        signedInUser = new SignedInUserResponse(SignInResult.Success, userSignInResponse, tokenModel, null, "Account Created Successfully");
                                        //Logger log = new Logger(user.Id, user.roleType.Name, "www.hashkart.com/admin/signIn", "POST", user.FirstName + " " + user.LastName + " Signed-In", "User Signed-In at" + DateTime.Now.ToString());
                                        //log.apicall(log);
                                    }
                                }
                                else
                                {
                                    signedInUser = new SignedInUserResponse(SignInResult.Success, signupResponse, null, null, "Account Created Successfully");

                                    //baseResponse.Data = signupResponse;
                                }
                            }
                            else
                            {
                                var errorList = returndata.Errors.Select(e => e.Description).ToList();
                                var errorString = string.Join(", ", errorList);
                                signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, errorString, 204);
                                //baseResponse.Code = 204;
                                //baseResponse.Message = errorString;
                                //baseResponse.Data = null;

                            }
                        }
                        else
                        {
                            //baseResponse.Code = 204;
                            //baseResponse.Message = "Role Doesn't Exist";
                            signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Role Doesn't Exist", 204);
                        }
                    }
                    else
                    {
                        //baseResponse.Code = 204;
                        //baseResponse.Message = "Username already exist";
                        signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "User mobile number already exist", 204);
                    }
                }
                else if (_userManager.Users.Where(x => x.UserName == user.UserName && x.AccountType.ToLower() == "guest").Any())
                {
                    if (!_userManager.Users.Where(x => x.PhoneNumber == user.PhoneNumber && x.UserName != user.UserName).Any())
                    {
                        var userData = await _userManager.FindByEmailAsync(user.UserName);

                        if (userData != null && userData.IsDeleted == false && await _userManager.IsInRoleAsync(userData, "Customer"))
                        {
                            userData.FirstName = user.FirstName;
                            userData.LastName = user.LastName;
                            userData.PhoneNumber = user.PhoneNumber;
                            userData.ProfileImage = user.ProfileImage;
                            userData.Gender = user.Gender;
                            userData.AccountType = null;
                            var res = await _userManager.UpdateAsync(userData);

                            if (res.Succeeded)
                            {
                                var ResetToken = await _tokenProvider.GeneratePasswordResetTokenAsync();
                                var tokenRes = await _userManager.SetAuthenticationTokenAsync(userData, "Default", "Reset", ResetToken);

                                if (tokenRes.Succeeded)
                                {
                                    var _gettoken = await _userManager.GetAuthenticationTokenAsync(userData, "Default", "Reset");
                                    if (_gettoken == ResetToken)
                                    {
                                        res = await _tokenProvider.ResetPasswordAsync(userData, ResetToken, Password);
                                        if (res.Succeeded)
                                        {
                                            await _userManager.RemoveAuthenticationTokenAsync(userData, "Default", "Reset");
                                        }
                                    }

                                }
                                var roles = await _userManager.GetRolesAsync(userData);
                                string userType = _dbContext.UserTypes.Where(x => x.Id == userData.UserTypeId).Select(x => x.Name).FirstOrDefault();
                                signupResponse = new UserSignInResponseModel(userData, userType, roles);
                                signedInUser = new SignedInUserResponse(SignInResult.Success, signupResponse, null, null, "Account Created Successfully");

                            }
                            else
                            {
                                var errorList = res.Errors.Select(e => e.Description).ToList();
                                var errorString = string.Join(", ", errorList);
                                signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Invalid User", 204);
                            }

                        }
                        else
                        {
                            signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Invalid User", 204);
                        }
                    }
                    else
                    {
                        //baseResponse.Code = 204;
                        //baseResponse.Message = "Username already exist";
                        signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "User mobile number already exist", 204);
                    }
                }
                else
                {
                    //baseResponse.Code = 204;
                    //baseResponse.Message = "Username already exist";
                    signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Email id already exist", 204);
                }
                //return baseResponse;
                return signedInUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<IdentityResult> CreateSpecificUserClaim(string UserId, string ClaimType, string ClaimValue)
        {
            try
            {

                var user = await _userManager.FindByIdAsync(UserId);
                if (user != null)
                {
                    var claims = _dbContext.UserClaims.Where(p => p.UserId == UserId && p.ClaimType == ClaimType && p.ClaimValue == ClaimValue).ToList();
                    var returndata = new IdentityResult();
                    if (!claims.Any())
                    {
                        returndata = await _userManager.AddClaimAsync(user, new Claim(ClaimType, ClaimValue));
                    }

                    return returndata;
                }
                else
                {
                    var identityResult = IdentityResult.Failed(new IdentityError[] {
                      new IdentityError{
                         Code = "201",
                         Description = "Record Not Found"
                      } });
                    return identityResult;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<UserSessions> GetUserActiveSessions(string UserId)
        {
            try
            {
                var sessions = _dbContext.UserDeviceSessions.Where(x => x.UserId == UserId).ToList();
                List<UserSessions> userSessions = new List<UserSessions>();
                if (sessions.Any())
                {
                    userSessions = sessions.Where(p => p.RefreshTokenExpiryTime > DateTime.UtcNow).ToList();
                    var RemoveuserSessions = sessions.Where(p => p.RefreshTokenExpiryTime < DateTime.UtcNow).ToList();
                    if (RemoveuserSessions.Any())
                    {
                        foreach (UserSessions item in RemoveuserSessions)
                        {
                            _dbContext.UserDeviceSessions.Remove(item);
                            _dbContext.SaveChanges();
                        }
                    }
                }

                return userSessions;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SignedInUserResponse> SignIn(SignIn signIn)
        {
            try
            {
                SignedInUserResponse signedInUser = null;

                var user = await _userManager.FindByEmailAsync(signIn.UserName);
                if (user == null)
                {
                    user = await _userManager.Users.Where(p => p.PhoneNumber == signIn.UserName).FirstOrDefaultAsync();
                }
                if (user != null && await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    if (user.Status.ToLower() == "active")
                    {
                        var validUser = await _userManager.CheckPasswordAsync(user, signIn.Password);
                        if (validUser)
                        {
                            string getUserType;
                            List<PageModuleSignInResponse> pagesAssigned = new List<PageModuleSignInResponse>();
                            int UserTypeId = user.UserTypeId ?? 0;
                            if (UserTypeId != 0)
                            {
                                //var pageAccess = _pageRoleRepository.GetRoleTypeWithPageAccess(UserTypeId, true);
                                var pageAccess = _pageRoleRepository.GetRoleTypeWithPageAccesswithUserSpecific(user.Id, UserTypeId, true);

                                pagesAssigned = pageAccess.pagesAssigned.Select(x => new PageModuleSignInResponse
                                {
                                    PageId = x.PageRoleId,
                                    PageName = x.PageRoleName,
                                    Read = x.Read,
                                    Write = x.Add,
                                    Update = x.Update,
                                    Delete = x.Delete
                                }).ToList();
                                getUserType = pageAccess.UserTypeName;
                            }
                            else
                            {
                                getUserType = "default";
                            }



                            var signinResult = await _signInManager.PasswordSignInAsync(signIn.UserName, signIn.Password, signIn.IsRemember, false);
                            var Refreshdays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]);

                            var refreshTk = GenerateRefreshToken();
                            //var tokenExpiry = DateTime.Now.AddDays(Refreshdays);
                            var tokenExpiry = DateTime.UtcNow.AddDays(Refreshdays);




                            var userRoles = await _userManager.GetRolesAsync(user);



                            var ClaimList = await loadClaims(user, getUserType, userRoles);

                            string accesstoken = _tokenManager.GenerateToken(user, ClaimList);

                            List<UserSessions> lstsessions = _dbContext.UserDeviceSessions.Where(x => x.DeviceId == signIn.DeviceId).ToList();
                            if (lstsessions.Any())
                            {
                                foreach (UserSessions item in lstsessions)
                                {
                                    _dbContext.Remove(item);
                                    _dbContext.SaveChanges();
                                }
                            }

                            UserSessions sessions = new UserSessions(user.Id, signIn.DeviceId, refreshTk, tokenExpiry, accesstoken, DateTime.UtcNow);
                            _dbContext.Add(sessions);
                            _dbContext.SaveChanges();

                            UserSignInResponseModel userSignInResponse = new UserSignInResponseModel(user, getUserType, userRoles);
                            TokenModel tokenModel = new TokenModel(accesstoken, refreshTk, 200, "Token generated successfully.");
                            signedInUser = new SignedInUserResponse(signinResult, userSignInResponse, tokenModel, pagesAssigned, "LoggedIn Successfully");

                            //Logger log = new Logger(user.Id, user.roleType.Name, "www.hashkart.com/admin/signIn", "POST", user.FirstName + " " + user.LastName + " Signed-In", "User Signed-In at" + DateTime.Now.ToString());
                            //log.apicall(log);
                            return signedInUser;
                        }
                        else
                        {
                            signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Invalid User Name or Password!");
                        }
                    }
                    else
                    {
                        signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Your account is inactive. Please contact the support team to have it activated");
                    }
                }
                else
                {
                    signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Invalid Account!");
                }

                return signedInUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SignOut()
        {

            await _signInManager.SignOutAsync();
        }

        public List<string> RolesList()
        {
            var roles = _roleManager.Roles.Select(x => x.Name).ToList();
            return roles;
        }

        public async Task<TokenModel> generateNewAccessToken(RequestNewTokenModel model)
        {
            TokenModel newTokens = new TokenModel();
            var principal = await _tokenManager.GetPrincipalFromToken(model.AccessToken);
            if (principal.IsValid)
            {
                var UserName = principal.Claims.First(c => c.Key.Equals("Name")).Value.ToString();
                var userResponse = await _userManager.FindByNameAsync(UserName);
                var userRoles = await _userManager.GetRolesAsync(userResponse);
                var userType = "default";
                if (userResponse != null && await _userManager.IsInRoleAsync(userResponse, "Admin"))
                {
                    userType = _dbContext.UserTypes.Where(x => x.Id == userResponse.UserTypeId).Select(x => x.Name).FirstOrDefault();
                }
                var claimList = await loadClaims(userResponse, userType, userRoles);

                var Sessions = GetUserActiveSessions(userResponse.Id);
                var _deviceSession = Sessions.Where(x => x.DeviceId == model.DeviceId).ToList();
                if (_deviceSession.Count() > 0)
                {
                    var _refreshSession = _deviceSession.Where(x => x.RefreshToken == model.RefreshToken).ToList();
                    if (_refreshSession.Count > 0)
                    {
                        //var rts = Sessions.Where(x => x.DeviceId == model.DeviceId && x.RefreshToken == model.RefreshToken).Select(x => new { x.RefreshToken, x.RefreshTokenExpiryTime }).FirstOrDefault();
                        var rts = _refreshSession.Select(x => new { x.RefreshToken, x.RefreshTokenExpiryTime }).FirstOrDefault();

                        if (rts.RefreshTokenExpiryTime > DateTime.UtcNow && !String.IsNullOrEmpty(rts.RefreshToken))
                        {
                            newTokens = _tokenManager.RegenerateToken(userResponse, claimList, rts.RefreshToken);

                            List<UserSessions> lstsessions = _dbContext.UserDeviceSessions.Where(x => x.DeviceId == model.DeviceId && x.RefreshToken == model.RefreshToken).ToList();
                            if (lstsessions.Any())
                            {
                                foreach (UserSessions item in lstsessions)
                                {
                                    item.AccessToken = newTokens.AccessToken;
                                    _dbContext.Update(item);
                                    _dbContext.SaveChanges();
                                }

                            }

                            return newTokens;
                        }
                        else
                        {
                            foreach (UserSessions item in _refreshSession)
                            {
                                _dbContext.UserDeviceSessions.Remove(item);
                                _dbContext.SaveChanges();
                            }


                            newTokens.AccessToken = "";
                            newTokens.RefreshToken = "";
                            newTokens.Code = 204;
                            newTokens.Message = "Refresh token is expired.";
                        }
                    }
                    else
                    {
                        foreach (UserSessions item in _deviceSession)
                        {
                            _dbContext.UserDeviceSessions.Remove(item);
                            _dbContext.SaveChanges();
                        }

                        newTokens.AccessToken = "";
                        newTokens.RefreshToken = "";
                        newTokens.Code = 204;
                        newTokens.Message = "Invalid refresh token.";
                    }
                }
                else
                {
                    //foreach (UserSessions item in Sessions)
                    //{
                    //    _dbContext.UserDeviceSessions.Remove(item);
                    //    _dbContext.SaveChanges();
                    //}

                    newTokens.AccessToken = "";
                    newTokens.RefreshToken = "";
                    newTokens.Code = 204;
                    newTokens.Message = "Invalid device id.";
                }
            }
            else
            {
                var userResponse = await _userManager.FindByIdAsync(model.UserId);
                var userRoles = await _userManager.GetRolesAsync(userResponse);
                var userType = "default";
                if (userResponse != null && await _userManager.IsInRoleAsync(userResponse, "Admin"))
                {
                    userType = _dbContext.UserTypes.Where(x => x.Id == userResponse.UserTypeId).Select(x => x.Name).FirstOrDefault();
                }
                var claimList = await loadClaims(userResponse, userType, userRoles);

                var Sessions = GetUserActiveSessions(userResponse.Id);
                var _deviceSession = Sessions.Where(x => x.DeviceId == model.DeviceId).ToList();
                if (_deviceSession.Count() > 0)
                {
                    var _refreshSession = _deviceSession.Where(x => x.RefreshToken == model.RefreshToken).ToList();
                    if (_refreshSession.Count > 0)
                    {
                        //var rts = Sessions.Where(x => x.DeviceId == model.DeviceId && x.RefreshToken == model.RefreshToken).Select(x => new { x.RefreshToken, x.RefreshTokenExpiryTime }).FirstOrDefault();
                        var rts = _refreshSession.Select(x => new { x.RefreshToken, x.RefreshTokenExpiryTime }).FirstOrDefault();

                        if (rts.RefreshTokenExpiryTime > DateTime.UtcNow && !String.IsNullOrEmpty(rts.RefreshToken))
                        {
                            newTokens = _tokenManager.RegenerateToken(userResponse, claimList, rts.RefreshToken);

                            List<UserSessions> lstsessions = _dbContext.UserDeviceSessions.Where(x => x.DeviceId == model.DeviceId && x.RefreshToken == model.RefreshToken).ToList();
                            if (lstsessions.Any())
                            {
                                foreach (UserSessions item in lstsessions)
                                {
                                    item.AccessToken = newTokens.AccessToken;
                                    _dbContext.Update(item);
                                    _dbContext.SaveChanges();
                                }

                            }

                            return newTokens;
                        }
                        else
                        {
                            foreach (UserSessions item in _refreshSession)
                            {
                                _dbContext.UserDeviceSessions.Remove(item);
                                _dbContext.SaveChanges();
                            }


                            newTokens.AccessToken = "";
                            newTokens.RefreshToken = "";
                            newTokens.Code = 204;
                            newTokens.Message = "Refresh token is expired.";
                        }
                    }
                    else
                    {
                        foreach (UserSessions item in _deviceSession)
                        {
                            _dbContext.UserDeviceSessions.Remove(item);
                            _dbContext.SaveChanges();
                        }

                        newTokens.AccessToken = "";
                        newTokens.RefreshToken = "";
                        newTokens.Code = 204;
                        newTokens.Message = "Invalid refresh token.";
                    }
                }
                else
                {
                    //foreach (UserSessions item in Sessions)
                    //{
                    //    _dbContext.UserDeviceSessions.Remove(item);
                    //    _dbContext.SaveChanges();
                    //}

                    newTokens.AccessToken = "";
                    newTokens.RefreshToken = "";
                    newTokens.Code = 204;
                    newTokens.Message = "Invalid device id.";
                }

                //newTokens.AccessToken = "";
                //newTokens.RefreshToken = "";
                //newTokens.Code = 204;
                //newTokens.Message = "Invalid access & refresh token.";
            }
            return newTokens;
        }
        public async Task<UserClaimResponse> GetUserRoleClaims(string UserId)
        {

            var user = await _userManager.FindByIdAsync(UserId);
            var roleNames = await _userManager.GetRolesAsync(user);

            var claims = new List<UserAccessClaims>();

            foreach (var item in roleNames)
            {
                var getRole = await _roleManager.FindByNameAsync(item);
                var getclaims = await _roleManager.GetClaimsAsync(getRole);

                int ResourceCount = getclaims.DistinctBy(x => x.Type).Count();
                for (int i = 0; i < ResourceCount; i++)
                {
                    UserAccessClaims newClaim = new UserAccessClaims();
                    newClaim.Resource = getclaims.DistinctBy(x => x.Type).Select(x => x.Type).ElementAt(i);
                    newClaim.AccessType = getclaims.Where(x => x.Type == newClaim.Resource).Select(x => x.Value).ToList();
                    claims.Add(newClaim);
                }
            }

            UserClaimResponse claimResponse = new UserClaimResponse(user.FirstName + " " + user.LastName, user.Id, (List<string>)roleNames, claims);
            return claimResponse;
        }

        public async Task<UserClaimResponse> GetUserSpecificClaims(string UserId)
        {

            var user = await _userManager.FindByIdAsync(UserId);
            var claims = new List<UserAccessClaims>();


            var getclaims = await _userManager.GetClaimsAsync(user);

            int ResourceCount = getclaims.DistinctBy(x => x.Type).Count();
            for (int i = 0; i < ResourceCount; i++)
            {
                UserAccessClaims newClaim = new UserAccessClaims();
                newClaim.Resource = getclaims.DistinctBy(x => x.Type).Select(x => x.Type).ElementAt(i);
                newClaim.AccessType = getclaims.Where(x => x.Type == newClaim.Resource).Select(x => x.Value).ToList();
                claims.Add(newClaim);
            }

            UserClaimResponse claimResponse = new(user.FirstName + " " + user.LastName, user.Id, null, claims);
            return claimResponse;
        }


        #region Admin

        public async Task<BaseResponse<AdminListModel>> GetAdmins(string? ID, string? searchString = null, string? status = null, int pageIndex = 0, int pageSize = 0)
        {
            var adminlist = await _userManager.GetUsersInRoleAsync("Admin");
            List<Users> lstadmin = new List<Users>();
            List<AdminListModel> userlist;
            BaseResponse<AdminListModel> baseResponse = new BaseResponse<AdminListModel>(200, "Record Doesn't Exist");

            if (adminlist.Count > 0)
            {
                lstadmin = adminlist.Where(p => p.IsDeleted == false).OrderByDescending(X => X.CreatedAt).ToList();

                if (!string.IsNullOrEmpty(ID))
                {
                    lstadmin = lstadmin.Where(p => p.Id == ID).OrderByDescending(X => X.CreatedAt).ToList();
                }

                if (!string.IsNullOrEmpty(searchString))
                {
                    //lstadmin = lstadmin.Where(p => p.PhoneNumber.Contains(searchString.ToLower()) || p.FirstName.ToLower().Contains(searchString.ToLower()) || p.LastName.ToLower().Contains(searchString.ToLower()) || p.Email.ToLower().Contains(searchString.ToLower()) || (p.roleType != null && p.roleType.Name.ToLower().Contains(searchString.ToLower()))).OrderByDescending(X => X.CreatedAt).ToList();
                    //lstadmin = lstadmin.Where(p =>
                    //                            (p.PhoneNumber != null && p.PhoneNumber.Contains(searchString.ToLower())) ||
                    //                            (p.FirstName != null && p.FirstName.ToLower().Contains(searchString.ToLower())) ||
                    //                            (p.LastName != null && p.LastName.ToLower().Contains(searchString.ToLower())) ||
                    //                            (p.Email != null && p.Email.ToLower().Contains(searchString.ToLower())) ||
                    //                            (p.roleType != null && p.roleType.Name.ToLower().Contains(searchString.ToLower()))
                    //                        ).OrderByDescending(X => X.CreatedAt).ToList();
                    string[] searchTerms = searchString.Split(' ');

                    lstadmin = lstadmin.Where(p =>
                        (p.PhoneNumber != null && p.PhoneNumber.Contains(searchString.ToLower())) ||
                        (p.FirstName != null && searchTerms.All(term => p.FirstName.ToLower().Contains(term.ToLower()))) ||
                        (p.LastName != null && searchTerms.All(term => p.LastName.ToLower().Contains(term.ToLower()))) ||
                        (p.Email != null && p.Email.ToLower().Contains(searchString.ToLower())) ||
                        (p.roleType != null && p.roleType.Name.ToLower().Contains(searchString.ToLower()))
                    ).OrderByDescending(x => x.CreatedAt).ToList();
                }

                if (status != null)
                {
                    lstadmin = lstadmin.Where(x => x.Status.ToLower() == status.ToLower()).OrderByDescending(X => X.CreatedAt).ToList();
                }

                var totalRecordCount = lstadmin.Count();
                var userTypes = await _dbContext.UserTypes.ToListAsync();

                if (pageSize != 0 && pageIndex != 0)
                {
                    userlist = lstadmin
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new AdminListModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        UserName = x.UserName,
                        UserTypeId = (int)x.UserTypeId,
                        ProfileImage = x.ProfileImage,
                        UserType = x.roleType?.Name ?? "default",
                        MobileNo = x.PhoneNumber,
                        Status = x.Status,
                        ReceiveNotifications = x.ReceiveNotifications
                    })
                    .ToList();
                }
                else
                {
                    userlist = lstadmin
                    .Select(x => new AdminListModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        UserName = x.UserName,
                        UserTypeId = (int)x.UserTypeId,
                        ProfileImage = x.ProfileImage,
                        UserType = x.roleType?.Name ?? "default",
                        MobileNo = x.PhoneNumber,
                        Status = x.Status,
                        ReceiveNotifications = x.ReceiveNotifications
                    })
                    .ToList();
                }

                Pagination pagination = new Pagination(pageIndex, pageSize, totalRecordCount);
                if (userlist.Any())
                {
                    baseResponse = new BaseResponse<AdminListModel>(200, "Records Bind Successfully.", pagination, userlist);
                }
            }

            return baseResponse;
        }

        public async Task<BaseResponse<AdminListModel>> getNoSuperAdmin(int pageIndex = 0, int pageSize = 0, string? status = null)
        {
            var adminlist = await _userManager.GetUsersInRoleAsync("Admin");
            List<Users> lstadmin = new List<Users>();
            List<AdminListModel> userlist;
            BaseResponse<AdminListModel> baseResponse = new BaseResponse<AdminListModel>(200, "Record Doesn't Exist");

            if (adminlist.Count > 0)
            {
                lstadmin = adminlist.Where(p => p.IsDeleted == false).OrderByDescending(X => X.CreatedAt).ToList();

                if (status != null)
                {
                    lstadmin = lstadmin.Where(x => x.Status.ToLower() == status.ToLower()).OrderByDescending(X => X.CreatedAt).ToList();
                }

                var totalRecordCount = lstadmin.Count();
                var userTypes = await _dbContext.UserTypes.ToListAsync();
                lstadmin = lstadmin.Where(p => p.roleType.Name.ToLower() != "super admin").ToList();
                if (pageSize != 0 && pageIndex != 0)
                {
                    userlist = lstadmin
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => new AdminListModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        UserName = x.UserName,
                        UserTypeId = (int)x.UserTypeId,
                        ProfileImage = x.ProfileImage,
                        UserType = x.roleType?.Name ?? "default",
                        MobileNo = x.PhoneNumber,
                        Status = x.Status,
                        ReceiveNotifications = x.ReceiveNotifications
                    })
                    .ToList();
                }
                else
                {
                    userlist = lstadmin
                    .Select(x => new AdminListModel
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        UserName = x.UserName,
                        UserTypeId = (int)x.UserTypeId,
                        ProfileImage = x.ProfileImage,
                        UserType = x.roleType?.Name ?? "default",
                        MobileNo = x.PhoneNumber,
                        Status = x.Status,
                        ReceiveNotifications = x.ReceiveNotifications
                    })
                    .ToList();
                }

                Pagination pagination = new Pagination(pageIndex, pageSize, totalRecordCount);
                if (userlist.Any())
                {
                    baseResponse = new BaseResponse<AdminListModel>(200, "Records Bind Successfully.", pagination, userlist);
                }
            }

            return baseResponse;
        }

        public async Task<BaseResponse<AdminListModel>> GetAdminByID(string ID)
        {
            var userTypes = await _dbContext.UserTypes.ToListAsync();
            var def = new BaseResponse<AdminListModel>(200, "Record not found.");
            var user = await _userManager.FindByIdAsync(ID);
            if (user != null && user.IsDeleted == false && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var userlist = new List<AdminListModel>
        {
            new AdminListModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                UserTypeId = (int)user.UserTypeId,
                ProfileImage = user.ProfileImage,
                UserType = user.roleType?.Name ?? "default",
                MobileNo = user.PhoneNumber,
                Status = user.Status,
                ReceiveNotifications = user.ReceiveNotifications
            }
        };
                return new BaseResponse<AdminListModel>(200, "Record Bind Successfully.", null, userlist);
            }

            return def;
        }


        public async Task<BaseResponse<AdminListModel>> GetAdminByEmail(string email)
        {
            var def = new BaseResponse<AdminListModel>(200, "Record not found.");
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && user.IsDeleted == false && await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var userlist = new List<AdminListModel>
        {
            new AdminListModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                UserTypeId = (int)user.UserTypeId,
                ProfileImage = user.ProfileImage,
                UserType = user.roleType?.Name ?? "default",
                MobileNo = user.PhoneNumber,
                Status = user.Status,
                ReceiveNotifications = user.ReceiveNotifications
            }
        };
                return new BaseResponse<AdminListModel>(200, "Record Bind Successfully.", null, userlist);
            }

            return def;
        }

        public async Task<BaseResponse<AdminListModel>> UpdateAdminDetails(AdminListModel model)
        {
            var def = new BaseResponse<AdminListModel>(200, "Record not found.");

            var user = await _userManager.FindByIdAsync(model.Id);

            if (!_userManager.Users.Where(x => x.UserName == model.UserName && x.Id != model.Id).Any())
            {
                if (!_userManager.Users.Where(x => x.PhoneNumber == model.MobileNo && x.Id != model.Id).Any())
                {


                    if (user != null && user.IsDeleted == false && await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.PhoneNumber = model.MobileNo;
                        user.UserTypeId = model.UserTypeId;
                        user.ProfileImage = model.ProfileImage;
                        user.Status = model.Status;
                        user.ReceiveNotifications = model.ReceiveNotifications;
                        var res = await _userManager.UpdateAsync(user);

                        if (res.Succeeded)
                        {
                            def = new BaseResponse<AdminListModel>(200, "User Updated Successfully.");
                        }
                        else
                        {
                            var errorList = res.Errors.Select(e => e.Description).ToList();
                            var errorString = string.Join(", ", errorList);
                            def = new BaseResponse<AdminListModel>(204, "User Updation Failed." + errorString);
                        }
                    }
                    else
                    {
                        def = new BaseResponse<AdminListModel>(204, "Invalid User.");
                    }
                }
                else
                {
                    def = new BaseResponse<AdminListModel>(204, "Mobile number already exist.");
                }
            }
            else
            {
                def = new BaseResponse<AdminListModel>(204, "User Name already exist.");
            }

            return def;
        }

        #endregion

        #region Seller
        //public async Task<BaseResponse<SellerListModel>> GetSellers(int pageIndex, int pageSize, string? searchString = null)
        //{
        //    var sellerList = await _userManager.GetUsersInRoleAsync("Seller");
        //    var filteredList = sellerList.Where(x => x.IsDeleted == false 
        //    && (string.IsNullOrEmpty(searchString) 
        //    || x.FirstName.Contains(searchString) 
        //    || x.LastName.Contains(searchString) 
        //    || x.Email.Contains(searchString) 
        //    || (x.roleType != null && x.roleType.Name.Contains(searchString))))
        //        .OrderByDescending(X=>X.CreatedAt);
        //    var totalRecordCount = filteredList.Count();
        //    var userTypes = await _dbContext.UserTypes.ToListAsync();
        //    List<SellerListModel> userlist;

        //    if (pageSize != 0)
        //    {
        //        userlist = filteredList
        //            .Skip((pageIndex - 1) * pageSize)
        //            .Take(pageSize)
        //            .Select(user => new SellerListModel
        //            {
        //                Id = user.Id,
        //                FirstName = user.FirstName,
        //                LastName = user.LastName,
        //                UserName = user.UserName,
        //                MobileNo = user.PhoneNumber,
        //                Status = user.Status,
        //                CreatedAt = user.CreatedAt,
        //                CreatedBy = user.CreatedBy,
        //                ModifiedAt = user.ModifiedAt,
        //                ModifiedBy = user.ModifiedBy,
        //                IsPhoneConfirmed = user.PhoneNumberConfirmed,
        //                IsEmailConfirmed = user.EmailConfirmed
        //            })
        //            .ToList();
        //    }
        //    else
        //    {
        //        userlist = filteredList
        //            .Select(user => new SellerListModel
        //            {
        //                Id = user.Id,
        //                FirstName = user.FirstName,
        //                LastName = user.LastName,
        //                UserName = user.UserName,
        //                MobileNo = user.PhoneNumber,
        //                Status = user.Status,
        //                CreatedAt = user.CreatedAt,
        //                CreatedBy = user.CreatedBy,
        //                ModifiedAt = user.ModifiedAt,
        //                ModifiedBy = user.ModifiedBy,
        //                IsPhoneConfirmed = user.PhoneNumberConfirmed,
        //                IsEmailConfirmed = user.EmailConfirmed
        //            })
        //            .ToList();
        //    }

        //    Pagination pagination = new Pagination(pageIndex, pageSize, totalRecordCount);
        //    BaseResponse<SellerListModel> baseResponse = new BaseResponse<SellerListModel>(200, "Record Doesn't Exist");
        //    if (userlist.Any())
        //    {
        //        baseResponse = new BaseResponse<SellerListModel>(200, "Records Bind Successfully.", pagination, userlist);
        //    }
        //    return baseResponse;
        //}


        public async Task<SignedInUserResponse> SellerSignIn(SignIn signIn)
        {
            try
            {
                SignedInUserResponse signedInUser = null;

                var user = await _userManager.FindByEmailAsync(signIn.UserName);
                if (user == null)
                {
                    user = await _userManager.Users.Where(p => p.PhoneNumber == signIn.UserName).FirstOrDefaultAsync();
                }

                if (user != null && await _userManager.IsInRoleAsync(user, "Seller"))
                {
                    if (user.Status.ToLower() == "active" || user.Status.ToLower() == "pending" || user.Status.ToLower() == "in progress")
                    {
                        var validUser = await _userManager.CheckPasswordAsync(user, signIn.Password);
                        if (validUser)
                        {
                            string getUserType;
                            int UserTypeId = user.UserTypeId ?? 0;
                            getUserType = "default";

                            var signinResult = await _signInManager.PasswordSignInAsync(signIn.UserName, signIn.Password, signIn.IsRemember, false);
                            var Refreshdays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]);

                            var refreshTk = GenerateRefreshToken();
                            //var tokenExpiry = DateTime.Now.AddDays(Refreshdays);
                            var tokenExpiry = DateTime.UtcNow.AddDays(Refreshdays);

                            var userRoles = await _userManager.GetRolesAsync(user);

                            var ClaimList = await loadClaims(user, getUserType, userRoles);

                            string accesstoken = _tokenManager.GenerateToken(user, ClaimList);

                            List<UserSessions> lstsessions = _dbContext.UserDeviceSessions.Where(x => x.DeviceId == signIn.DeviceId).ToList();
                            if (lstsessions.Any())
                            {
                                foreach (UserSessions item in lstsessions)
                                {
                                    _dbContext.Remove(item);
                                    _dbContext.SaveChanges();
                                }
                            }

                            UserSessions sessions = new UserSessions(user.Id, signIn.DeviceId, refreshTk, tokenExpiry, accesstoken, DateTime.UtcNow);
                            _dbContext.Add(sessions);
                            _dbContext.SaveChanges();

                            UserSignInResponseModel userSignInResponse = new UserSignInResponseModel(user, getUserType, userRoles);
                            TokenModel tokenModel = new TokenModel(accesstoken, refreshTk, 200, "Token generated successfully.");
                            signedInUser = new SignedInUserResponse(signinResult, userSignInResponse, tokenModel, null, "LoggedIn Successfully");

                            //Logger log = new Logger(user.Id, user.roleType.Name, "www.hashkart.com/admin/signIn", "POST", user.FirstName + " " + user.LastName + " Signed-In", "User Signed-In at" + DateTime.Now.ToString());
                            //log.apicall(log);
                            return signedInUser;
                        }
                        else
                        {
                            signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Invalid User Name or Password!");
                        }
                    }
                    else
                    {
                        signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Your account is inactive. Please contact the support team to have it activated");
                    }
                }
                else
                {
                    signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Invalid Account!");
                }

                return signedInUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<SellerListModel>> GetSellers(int pageIndex, int pageSize, string? searchString = null, string? fromDate = null, string? toDate = null, string? status = null, bool? IsArchive = null)
        {
            var sellerList = await _userManager.GetUsersInRoleAsync("Seller");
            //var filteredList = sellerList.Where(x => x.IsDeleted == false &&
            //    (string.IsNullOrEmpty(searchString) ||
            //    x.FirstName.ToLower().Contains(searchString.ToLower()) ||
            //    x.LastName.ToLower().Contains(searchString.ToLower()) ||
            //    x.Email.ToLower().Contains(searchString.ToLower()) ||
            //    (x.roleType != null && x.roleType.Name.ToLower().Contains(searchString))) &&
            //    (fromDate == null || x.CreatedAt >= Convert.ToDateTime(fromDate)) &&
            //    (toDate == null || x.CreatedAt <= Convert.ToDateTime(toDate)))
            //    .OrderByDescending(x => x.CreatedAt).ToList();
            var filteredList = sellerList.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreatedAt).ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                string[] searchTerms = searchString.Split(' ');

                filteredList = sellerList.Where(p =>
                    (p.PhoneNumber != null && p.PhoneNumber.Contains(searchString.ToLower())) ||
                    (p.FirstName != null && searchTerms.All(term => p.FirstName.ToLower().Contains(term.ToLower()))) ||
                    (p.LastName != null && searchTerms.All(term => p.LastName.ToLower().Contains(term.ToLower()))) ||
                    (p.Email != null && p.Email.ToLower().Contains(searchString.ToLower())) ||
                    (p.roleType != null && p.roleType.Name.ToLower().Contains(searchString.ToLower()))
                ).OrderByDescending(x => x.CreatedAt).ToList();
            }

            if (!string.IsNullOrEmpty(fromDate))
            {
                filteredList = sellerList.Where(x => x.CreatedAt >= Convert.ToDateTime(fromDate)).OrderByDescending(x => x.CreatedAt).ToList();
            }

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                filteredList = sellerList.Where(x => (x.CreatedAt >= Convert.ToDateTime(fromDate))&&(x.CreatedAt <= Convert.ToDateTime(toDate))).OrderByDescending(x => x.CreatedAt).ToList();
            }

            if (status != null)
            {
                filteredList = filteredList.Where(x => x.Status.ToLower() == status.ToLower()).ToList();
            }
            if (IsArchive != null)
            {
                if (Convert.ToBoolean(IsArchive))
                {
                    filteredList = filteredList.Where(x => x.Status.ToLower() == "archived").ToList();
                }
                else
                {
                    filteredList = filteredList.Where(x => x.Status.ToLower() != "archived").ToList();
                }
            }

            var totalRecordCount = filteredList.Count();
            var userTypes = await _dbContext.UserTypes.ToListAsync();
            List<SellerListModel> userlist;

            if (pageSize != 0)
            {
                userlist = filteredList
                    .Skip((pageIndex - 1) * pageSize)
                    .Take(pageSize)
                    .Select(user => new SellerListModel
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        MobileNo = user.PhoneNumber,
                        Status = user.Status,
                        Gender = user.Gender,
                        CreatedAt = user.CreatedAt,
                        CreatedBy = user.CreatedBy,
                        ModifiedAt = user.ModifiedAt,
                        ModifiedBy = user.ModifiedBy,
                        IsPhoneConfirmed = user.PhoneNumberConfirmed,
                        IsEmailConfirmed = user.EmailConfirmed
                    })
                    .ToList();
            }
            else
            {
                userlist = filteredList
                    .Select(user => new SellerListModel
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        MobileNo = user.PhoneNumber,
                        Status = user.Status,
                        Gender = user.Gender,
                        CreatedAt = user.CreatedAt,
                        CreatedBy = user.CreatedBy,
                        ModifiedAt = user.ModifiedAt,
                        ModifiedBy = user.ModifiedBy,
                        IsPhoneConfirmed = user.PhoneNumberConfirmed,
                        IsEmailConfirmed = user.EmailConfirmed
                    })
                    .ToList();
            }

            Pagination pagination = new Pagination(pageIndex, pageSize, totalRecordCount);
            BaseResponse<SellerListModel> baseResponse = new BaseResponse<SellerListModel>(200, "Record Doesn't Exist");
            if (userlist.Any())
            {
                baseResponse = new BaseResponse<SellerListModel>(200, "Records Bind Successfully.", pagination, userlist);
            }
            return baseResponse;
        }

        public async Task<BaseResponse<SellerListModel>> UpdateSellerDetails(SellerListModel model)
        {
            var def = new BaseResponse<SellerListModel>(200, "Record not found.");
            if (!_userManager.Users.Where(x => x.UserName == model.UserName && x.Id != model.Id).Any())
            {
                if (!_userManager.Users.Where(x => x.PhoneNumber == model.MobileNo && x.Id != model.Id).Any())
                {

                    var user = await _userManager.FindByIdAsync(model.Id);

                    if (user != null && user.IsDeleted == false && await _userManager.IsInRoleAsync(user, "Seller"))
                    {
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.PhoneNumber = model.MobileNo;
                        user.Status = model.Status;
                        user.Gender = model.Gender;
                        user.ProfileImage = model.ProfileImage;
                        user.ModifiedBy = model.ModifiedBy;
                        user.ModifiedAt = model.ModifiedAt;
                        var res = await _userManager.UpdateAsync(user);

                        if (model.Status.ToLower() == "inactive" || model.Status.ToLower() == "archived")
                        {
                            List<UserSessions> lstsessions = _dbContext.UserDeviceSessions.Where(x => x.UserId == model.Id).ToList();
                            if (lstsessions.Any())
                            {
                                foreach (UserSessions item in lstsessions)
                                {
                                    _dbContext.Remove(item);
                                    _dbContext.SaveChanges();
                                }
                            }

                            var token = _dbContext.UserTokens.Where(x => x.UserId == model.Id).ToList();
                            if (token.Any())
                            {
                                foreach (var item in token)
                                {
                                    _dbContext.Remove(item);
                                    _dbContext.SaveChanges();
                                }
                            }
                        }

                        if (res.Succeeded)
                        {
                            def = new BaseResponse<SellerListModel>(200, "User Updated Successfully.");
                        }
                        else
                        {
                            var errorList = res.Errors.Select(e => e.Description).ToList();
                            var errorString = string.Join(", ", errorList);
                            def = new BaseResponse<SellerListModel>(204, "User Updation Failed." + errorString);
                        }
                    }
                    else
                    {
                        def = new BaseResponse<SellerListModel>(204, "User Role Might Be Not A Seller.");
                    }
                }
                else
                {
                    def = new BaseResponse<SellerListModel>(204, "Mobile number already exist.");
                }
            }
            else
            {
                def = new BaseResponse<SellerListModel>(204, "User Name already exist.");
            }


            return def;
        }

        public async Task<BaseResponse<SellerListModel>> DeleteSellerDetails(Users model)
        {
            var def = new BaseResponse<SellerListModel>(200, "Record not found.");

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user != null && await _userManager.IsInRoleAsync(user, "Seller"))
            {
                await _userManager.RemoveFromRoleAsync(user, "Seller");

                List<UserSessions> lstsessions = _dbContext.UserDeviceSessions.Where(x => x.UserId == model.Id).ToList();
                if (lstsessions.Any())
                {
                    foreach (UserSessions item in lstsessions)
                    {
                        _dbContext.Remove(item);
                        _dbContext.SaveChanges();
                    }
                }

                var token = _dbContext.UserTokens.Where(x => x.UserId == model.Id).ToList();
                if (token.Any())
                {
                    foreach (var item in token)
                    {
                        _dbContext.Remove(item);
                        _dbContext.SaveChanges();
                    }
                }

                var res = await _userManager.DeleteAsync(user);

                //user.DeletedAt = model.DeletedAt;
                //user.DeletedBy = model.DeletedBy;
                //user.IsDeleted = model.IsDeleted;
                //var res = await _userManager.UpdateAsync(user);

                if (res.Succeeded)
                {
                    return new BaseResponse<SellerListModel>(200, "User Deleted Successfully.");
                }
                else
                {
                    var errorList = res.Errors.Select(e => e.Description).ToList();
                    var errorString = string.Join(", ", errorList);
                    return new BaseResponse<SellerListModel>(200, "User Delete Failed." + errorString);
                }
            }
            else
            {
                return new BaseResponse<SellerListModel>(200, "User Role Might Be Not A Seller.");
            }

            return def;
        }

        public async Task<BaseResponse<SellerListModel>> GetSellerByID(string ID)
        {
            var def = new BaseResponse<SellerListModel>(200, "Record not found.");
            var user = await _userManager.FindByIdAsync(ID);
            if (user != null && user.IsDeleted == false && await _userManager.IsInRoleAsync(user, "Seller"))
            {
                var userlist = new List<SellerListModel>
        {
            new SellerListModel
            {
                 Id = user.Id,
                 FirstName = user.FirstName,
                 LastName = user.LastName,
                 UserName = user.UserName,
                 Email = user.Email,
                 MobileNo = user.PhoneNumber,
                 Status = user.Status,
                 Gender = user.Gender,
                 CreatedAt = user.CreatedAt,
                 CreatedBy = user.CreatedBy,
                 ModifiedAt = user.ModifiedAt,
                 ModifiedBy = user.ModifiedBy,
                 IsPhoneConfirmed = user.PhoneNumberConfirmed,
                 IsEmailConfirmed = user.EmailConfirmed,


            }
        };
                return new BaseResponse<SellerListModel>(200, "Record Bind Successfully.", null, userlist);
            }

            return def;
        }

        public async Task<BaseResponse<SellerListModel>> GetSellerByEmail(string email)
        {
            var def = new BaseResponse<SellerListModel>(200, "Record not found.");
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && user.IsDeleted == false && await _userManager.IsInRoleAsync(user, "Seller"))
            {
                var userlist = new List<SellerListModel>
                {
            new SellerListModel
            {
                 Id = user.Id,
                 FirstName = user.FirstName,
                 LastName = user.LastName,
                 UserName = user.UserName,
                 MobileNo = user.PhoneNumber,
                 Status = user.Status,
                 Gender = user.Gender,
                 CreatedAt = user.CreatedAt,
                 CreatedBy = user.CreatedBy,
                 ModifiedAt = user.ModifiedAt,
                 ModifiedBy = user.ModifiedBy,
                 IsPhoneConfirmed = user.PhoneNumberConfirmed,
                 IsEmailConfirmed = user.EmailConfirmed
            }
            };
                return new BaseResponse<SellerListModel>(200, "Record Bind Successfully.", null, userlist);
            }

            return def;
        }
        #endregion

        #region Customer

        public async Task<SignedInUserResponse> CustomerSignIn(SignIn signIn)
        {
            try
            {
                SignedInUserResponse signedInUser = null;

                var user = await _userManager.FindByEmailAsync(signIn.UserName);
                if (user == null)
                {
                    user = await _userManager.Users.Where(p => p.PhoneNumber == signIn.UserName).FirstOrDefaultAsync();
                }

                if (user != null && await _userManager.IsInRoleAsync(user, "Customer"))
                {
                    if (user.Status.ToLower() == "active")
                    {
                        var validUser = await _userManager.CheckPasswordAsync(user, signIn.Password);
                        if (validUser)
                        {
                            string getUserType;
                            int UserTypeId = user.UserTypeId ?? 0;
                            getUserType = "default";

                            var signinResult = await _signInManager.PasswordSignInAsync(user.UserName, signIn.Password, signIn.IsRemember, false);
                            var Refreshdays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]);

                            var refreshTk = GenerateRefreshToken();
                            //var tokenExpiry = DateTime.Now.AddDays(Refreshdays);
                            var tokenExpiry = DateTime.UtcNow.AddDays(Refreshdays);

                            var userRoles = await _userManager.GetRolesAsync(user);

                            var ClaimList = await loadClaims(user, getUserType, userRoles);

                            string accesstoken = _tokenManager.GenerateToken(user, ClaimList);

                            List<UserSessions> lstsessions = _dbContext.UserDeviceSessions.Where(x => x.DeviceId == signIn.DeviceId).ToList();
                            if (lstsessions.Any())
                            {
                                foreach (UserSessions item in lstsessions)
                                {
                                    _dbContext.Remove(item);
                                    _dbContext.SaveChanges();
                                }
                            }

                            UserSessions sessions = new UserSessions(user.Id, signIn.DeviceId, refreshTk, tokenExpiry, accesstoken, DateTime.UtcNow);
                            _dbContext.Add(sessions);
                            _dbContext.SaveChanges();

                            UserSignInResponseModel userSignInResponse = new UserSignInResponseModel(user, getUserType, userRoles);
                            TokenModel tokenModel = new TokenModel(accesstoken, refreshTk, 200, "Token generated successfully.");
                            signedInUser = new SignedInUserResponse(signinResult, userSignInResponse, tokenModel, null, "Logged In Successfully");

                            //Logger log = new Logger(user.Id, user.roleType.Name, "www.hashkart.com/admin/signIn", "POST", user.FirstName + " " + user.LastName + " Signed-In", "User Signed-In at" + DateTime.Now.ToString());
                            //log.apicall(log);
                            return signedInUser;
                        }
                        else
                        {
                            signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Invalid User Name or Password!");
                        }
                    }
                    else
                    {
                        signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Your account is inactive. Please contact the support team to have it activated");
                    }
                }
                else
                {
                    signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Invalid Account!");
                }

                return signedInUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<CustomerListModel>> GetCustomers(int pageIndex, int pageSize, string? searchString = null)
        {
            List<Users> lstadmin = new List<Users>();
            var adminlist = await _userManager.GetUsersInRoleAsync("Customer");
            BaseResponse<CustomerListModel> baseResponse = new BaseResponse<CustomerListModel>(200, "Record Doesn't Exist");
            List<CustomerListModel> userlist;

            if (adminlist.Count > 0)
            {
                lstadmin = adminlist.Where(p => p.IsDeleted == false).OrderByDescending(X => X.CreatedAt).ToList();

                if (!string.IsNullOrEmpty(searchString))
                {
                    //lstadmin = lstadmin.Where(p => p.PhoneNumber.Contains(searchString.ToLower()) || p.FirstName.ToLower().Contains(searchString.ToLower()) || p.LastName.ToLower().Contains(searchString.ToLower()) || p.Email.ToLower().Contains(searchString.ToLower()) || (p.roleType != null && p.roleType.Name.ToLower().Contains(searchString.ToLower()))).OrderByDescending(X => X.CreatedAt).ToList();
                    //lstadmin = lstadmin.Where(p =>
                    //                            (p.PhoneNumber != null && p.PhoneNumber.Contains(searchString.ToLower())) ||
                    //                            (p.FirstName != null && p.FirstName.ToLower().Contains(searchString.ToLower())) ||
                    //                            (p.LastName != null && p.LastName.ToLower().Contains(searchString.ToLower())) ||
                    //                            (p.Email != null && p.Email.ToLower().Contains(searchString.ToLower())) ||
                    //                            (p.roleType != null && p.roleType.Name.ToLower().Contains(searchString.ToLower()))
                    //                        ).OrderByDescending(X => X.CreatedAt).ToList();

                    string[] searchTerms = searchString.Split(' ');

                    lstadmin = lstadmin.Where(p =>
                        (p.PhoneNumber != null && p.PhoneNumber.Contains(searchString.ToLower())) ||
                        (p.FirstName != null && searchTerms.All(term => p.FirstName.ToLower().Contains(term.ToLower()))) ||
                        (p.LastName != null && searchTerms.All(term => p.LastName.ToLower().Contains(term.ToLower()))) ||
                        (p.Email != null && p.Email.ToLower().Contains(searchString.ToLower())) ||
                        (p.roleType != null && p.roleType.Name.ToLower().Contains(searchString.ToLower()))
                    ).OrderByDescending(x => x.CreatedAt).ToList();
                }

                var totalRecordCount = lstadmin.Count();
                var userTypes = await _dbContext.UserTypes.ToListAsync();

                if (pageSize != 0 && pageIndex != 0)
                {
                    userlist = lstadmin
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize)
                        .Select(x => new CustomerListModel
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            UserName = x.UserName,
                            MobileNo = x.PhoneNumber,
                            Gender = x.Gender,
                            UserType = x.roleType?.Name ?? "default"

                        })
                        .ToList();
                }
                else
                {
                    userlist = lstadmin
                        .Select(x => new CustomerListModel
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            UserName = x.UserName,
                            MobileNo = x.PhoneNumber,
                            Gender = x.Gender,
                            UserType = x.roleType?.Name ?? "default"
                        })
                        .ToList();
                }

                Pagination pagination = new Pagination(pageIndex, pageSize, totalRecordCount);
                if (userlist.Any())
                {
                    baseResponse = new BaseResponse<CustomerListModel>(200, "Records Bind Successfully.", pagination, userlist);
                }
            }

            return baseResponse;
        }

        public async Task<BaseResponse<CustomerListModel>> GetCustomerByID(string ID)
        {
            var def = new BaseResponse<CustomerListModel>(200, "Record not found.");
            var user = await _userManager.FindByIdAsync(ID);
            if (user != null && user.IsDeleted == false && await _userManager.IsInRoleAsync(user, "Customer"))
            {
                var userlist = new List<CustomerListModel>
        {
            new CustomerListModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                MobileNo = user.PhoneNumber,
                Gender = user.Gender,
                ProfileImage = user.ProfileImage,
                UserType = user.roleType?.Name ?? "default"
            }
        };
                return new BaseResponse<CustomerListModel>(200, "Record Bind Successfully.", null, userlist);
            }

            return def;
        }

        public async Task<BaseResponse<CustomerListModel>> GetCustomerByEmail(string email)
        {
            var def = new BaseResponse<CustomerListModel>(200, "Record not found.");
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null && user.IsDeleted == false && await _userManager.IsInRoleAsync(user, "Customer"))
            {
                var userlist = new List<CustomerListModel>
        {
            new CustomerListModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                MobileNo = user.PhoneNumber,
                Gender = user.Gender,
                UserType = user.roleType?.Name ?? "default"
            }
        };
                return new BaseResponse<CustomerListModel>(200, "Record Bind Successfully.", null, userlist);
            }

            return def;
        }

        #endregion

        public List<Users> List()
        {
            try
            {
                var user = _userManager.Users.Where(X => X.IsDeleted == false).ToList();
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Claim>> loadClaims(Users user, string UserType, IList<string> UserRoles)
        {
            var roleClaims = new List<Claim>();
            var authClaims = new List<Claim>
                {
                    new Claim("Name", user.UserName),
                    new Claim("UserType", UserType),
                    new Claim("UserID", user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

            foreach (var role in UserRoles)
            {
                var roleType = await _roleManager.FindByNameAsync(role);
                var temp = await _roleManager.GetClaimsAsync(roleType);
                roleClaims = temp.ToList();
            }

            foreach (var claim in roleClaims)
            {
                authClaims.Add(new Claim("Scope", claim.Value));
            }
            foreach (var userRole in UserRoles)
            {
                var ttir = new Claim(ClaimTypes.Role, userRole);
                authClaims.Add(ttir);
            }
            authClaims.Add(new Claim("Scope", "general"));
            return authClaims;
        }

        public void setNewRefreshToken(string user, string refreshToken)
        {
            var Refreshdays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]);
            var currentsession = _dbContext.UserDeviceSessions.Where(x => x.UserId.Equals(user) && x.RefreshToken.Equals(refreshToken)).FirstOrDefault();
            currentsession.RefreshToken = GenerateRefreshToken();
            currentsession.RefreshTokenExpiryTime = DateTime.Now.AddDays(Refreshdays);
            _dbContext.Update(user);
            _dbContext.SaveChanges();
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<BaseResponse<string>> GenerateMobileOTP(GenerateMobileOtp model)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>(204, "");
            var user = await _userManager.Users.Where(p => p.PhoneNumber == model.MobileNo.ToString()).FirstOrDefaultAsync();
            if (user != null && user.IsDeleted == false && await _userManager.IsInRoleAsync(user, "Customer"))
            {
                var res = await _userManager.UpdateAsync(user);

                var random = new Random();
                var randomNum = random.Next(100000, 999999).ToString();

                user.MobileOTP = Convert.ToInt32(randomNum);
                await _userManager.UpdateAsync(user);

                baseResponse.Code = 200;
                baseResponse.Message = "Successfully sent OTP on this mobile number " + model.MobileNo;
                baseResponse.Data = randomNum;
                baseResponse.Pagination = null;
            }
            else
            {
                baseResponse.Code = 204;
                baseResponse.Message = "Invalid User Mobile No";
                baseResponse.Data = null;
                baseResponse.Pagination = null;
            }
            return baseResponse;
        }

        public async Task<SignedInUserResponse> SignInViaOTP(SignInViaOtp signIn)
        {
            try
            {
                SignedInUserResponse signedInUser = null;

                var user = await _userManager.Users.Where(p => p.PhoneNumber == signIn.MobileNo).FirstOrDefaultAsync();
                if (user != null)
                {
                    if (user.Status.ToLower() == "active")
                    {
                        if (user.MobileOTP.ToString() == signIn.otp)
                        {
                            string getUserType;
                            List<PageModuleSignInResponse> pagesAssigned = new List<PageModuleSignInResponse>();
                            int UserTypeId = user.UserTypeId ?? 0;
                            if (UserTypeId != 0)
                            {
                                //var pageAccess = _pageRoleRepository.GetRoleTypeWithPageAccess(UserTypeId, true);
                                var pageAccess = _pageRoleRepository.GetRoleTypeWithPageAccesswithUserSpecific(user.Id, UserTypeId, true);

                                pagesAssigned = pageAccess.pagesAssigned.Select(x => new PageModuleSignInResponse
                                {
                                    PageId = x.PageRoleId,
                                    PageName = x.PageRoleName,
                                    Read = x.Read,
                                    Write = x.Add,
                                    Update = x.Update,
                                    Delete = x.Delete
                                }).ToList();
                                getUserType = pageAccess.UserTypeName;
                            }
                            else
                            {
                                getUserType = "default";
                            }



                            //var signinResult = await _signInManager.PasswordSignInAsync(signIn.UserName, signIn.Password, signIn.IsRemember, false);

                            var Refreshdays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]);

                            var refreshTk = GenerateRefreshToken();
                            //var tokenExpiry = DateTime.Now.AddDays(Refreshdays);
                            var tokenExpiry = DateTime.UtcNow.AddDays(Refreshdays);




                            var userRoles = await _userManager.GetRolesAsync(user);



                            var ClaimList = await loadClaims(user, getUserType, userRoles);

                            string accesstoken = _tokenManager.GenerateToken(user, ClaimList);

                            UserSessions sessions = new UserSessions(user.Id, signIn.DeviceId, refreshTk, tokenExpiry, accesstoken, DateTime.UtcNow);
                            _dbContext.Add(sessions);
                            _dbContext.SaveChanges();

                            UserSignInResponseModel userSignInResponse = new UserSignInResponseModel(user, getUserType, userRoles);
                            TokenModel tokenModel = new TokenModel(accesstoken, refreshTk, 200, "Token generated successfully.");
                            signedInUser = new SignedInUserResponse(SignInResult.Success, userSignInResponse, tokenModel, pagesAssigned, "LoggedIn Successfully");

                            //Logger log = new Logger(user.Id, "customer", "www.hashkart.com/admin/signIn", "POST", user.FirstName + " " + user.LastName + " Signed-In", "User Signed-In at" + DateTime.Now.ToString());
                            //log.apicall(log);
                            return signedInUser;
                        }
                        else
                        {
                            signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Invalid OTP.");
                        }
                    }
                    else
                    {
                        signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Your account is inactive. Please contact the support team to have it activated");
                    }
                }
                else
                {
                    signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Invalid Account!");
                }

                return signedInUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SignedInUserResponse> SignInViaEmailId(SignInViaEmailId signIn)
        {
            try
            {
                SignedInUserResponse signedInUser = null;

                var user = await _userManager.FindByEmailAsync(signIn.EmailId);

                if (user != null && await _userManager.IsInRoleAsync(user, "Customer"))
                {
                    if (user.Status.ToLower() == "active")
                    {
                        string getUserType;

                        List<PageModuleSignInResponse> pagesAssigned = new List<PageModuleSignInResponse>();

                        getUserType = "default";

                        var Refreshdays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]);

                        var refreshTk = GenerateRefreshToken();

                        var tokenExpiry = DateTime.UtcNow.AddDays(Refreshdays);

                        var userRoles = await _userManager.GetRolesAsync(user);

                        var ClaimList = await loadClaims(user, getUserType, userRoles);

                        string accesstoken = _tokenManager.GenerateToken(user, ClaimList);

                        UserSessions sessions = new UserSessions(user.Id, signIn.DeviceId, refreshTk, tokenExpiry, accesstoken, DateTime.UtcNow);

                        _dbContext.Add(sessions);
                        _dbContext.SaveChanges();

                        UserSignInResponseModel userSignInResponse = new UserSignInResponseModel(user, getUserType, userRoles);

                        TokenModel tokenModel = new TokenModel(accesstoken, refreshTk, 200, "Token generated successfully.");

                        signedInUser = new SignedInUserResponse(SignInResult.Success, userSignInResponse, tokenModel, pagesAssigned, "LoggedIn Successfully");

                        return signedInUser;

                    }
                    else
                    {
                        signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Your account is inactive. Please contact the support team to have it activated");
                    }
                }
                else
                {
                    signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Invalid Account!");
                }

                return signedInUser;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponse<CustomerListModel>> UpdateCustomerDetails(CustomerListModel model)
        {
            var def = new BaseResponse<CustomerListModel>(200, "Record not found.");
            if (!_userManager.Users.Where(x => x.UserName == model.UserName && x.Id != model.Id).Any())
            {
                if (!_userManager.Users.Where(x => x.PhoneNumber == model.MobileNo && x.Id != model.Id).Any())
                {
                    var user = await _userManager.FindByIdAsync(model.Id);

                    if (user != null && user.IsDeleted == false && await _userManager.IsInRoleAsync(user, "Customer"))
                    {
                        user.FirstName = model.FirstName;
                        user.LastName = model.LastName;
                        user.PhoneNumber = model.MobileNo;
                        user.ProfileImage = model.ProfileImage;
                        user.Gender = model.Gender;
                        var res = await _userManager.UpdateAsync(user);

                        if (res.Succeeded)
                        {
                            def = new BaseResponse<CustomerListModel>(200, "User Updated Successfully.");
                        }
                        else
                        {
                            var errorList = res.Errors.Select(e => e.Description).ToList();
                            var errorString = string.Join(", ", errorList);
                            def = new BaseResponse<CustomerListModel>(204, "User Updation Failed." + errorString);
                        }
                    }
                    else
                    {
                        def = new BaseResponse<CustomerListModel>(204, "Invalid User.");
                    }
                }
                else
                {
                    def = new BaseResponse<CustomerListModel>(204, "Mobile number already exist.");
                }
            }
            else
            {
                def = new BaseResponse<CustomerListModel>(204, "User Name already exist.");
            }

            return def;
        }

        public async Task<BaseResponse<CustomerListModel>> DeleteCustomerDetails(Users model)
        {
            var def = new BaseResponse<CustomerListModel>(200, "Record not found.");

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user != null && await _userManager.IsInRoleAsync(user, "Customer"))
            {
                user.DeletedAt = model.DeletedAt;
                user.DeletedBy = model.DeletedBy;
                user.IsDeleted = model.IsDeleted;
                var res = await _userManager.UpdateAsync(user);

                if (res.Succeeded)
                {
                    return new BaseResponse<CustomerListModel>(200, "User Deleted Successfully.");
                }
                else
                {
                    var errorList = res.Errors.Select(e => e.Description).ToList();
                    var errorString = string.Join(", ", errorList);
                    return new BaseResponse<CustomerListModel>(200, "User Delete Failed." + errorString);
                }
            }

            return def;
        }

        public BaseResponse<string> UserLogout(Logout model)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>(204, "");
            List<UserSessions> lstsessions = _dbContext.UserDeviceSessions.Where(x => x.DeviceId == model.Deviceid && x.RefreshToken == model.RefreshToken && x.UserId == model.UserId).ToList();
            if (lstsessions.Any())
            {
                foreach (UserSessions item in lstsessions)
                {
                    _dbContext.Remove(item);
                    _dbContext.SaveChanges();
                }
            }

            baseResponse.Code = 200;
            baseResponse.Message = "Signout Successfully";
            baseResponse.Data = null;
            baseResponse.Pagination = null;

            return baseResponse;
        }

        public BaseResponse<UserSessions> UserSession(UserSessiondto model)
        {
            BaseResponse<UserSessions> baseResponse = new BaseResponse<UserSessions>(204, "");
            List<UserSessions> lstsessions = _dbContext.UserDeviceSessions.Where(x => x.DeviceId == model.DeviceId && x.AccessToken == model.AccessToken).ToList();
            if (lstsessions.Count > 0)
            {
                baseResponse.Code = 200;
                baseResponse.Data = lstsessions;
                baseResponse.Message = "Record Bind Successfully";
            }
            else
            {

                baseResponse.Code = 204;
                baseResponse.Data = null;
                baseResponse.Message = "Record does not exist";
            }
            return baseResponse;
        }

        public string GenerateRandomPassword()
        {
            const string allChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#";
            const int passwordLength = 10;

            char[] password = new char[passwordLength];

            // Ensure at least one character from each category
            password[0] = GetRandomCharacter("abcdefghijklmnopqrstuvwxyz");
            password[1] = GetRandomCharacter("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            password[2] = GetRandomCharacter("0123456789");
            password[3] = GetRandomCharacter("!@#");

            // Fill the rest of the password with random characters
            for (int i = 4; i < passwordLength; i++)
            {
                password[i] = GetRandomCharacter(allChars);
            }

            // Shuffle the password characters
            Array.Sort(password, (a, b) => Random.Next(-1, 2));

            return new string(password);
        }

        private static char GetRandomCharacter(string characterSet)
        {
            return characterSet[Random.Next(characterSet.Length)];
        }

        public async Task<SignedInUserResponse> Guestcheckout(GuestUser guest, string userId)
        {
            SignedInUserResponse signedInUser = null;

            string Password = GenerateRandomPassword();
            Users users = new Users();
            users.FirstName = guest.FirstName;
            users.LastName = guest.LastName;
            users.Email = guest.EmailID;
            users.PhoneNumber = guest.MobileNo;
            users.UserName = guest.EmailID;
            users.Status = "Active";
            users.AccountType = "Guest";
            users.Gender = guest.Gender;
            users.CreatedAt = DateTime.UtcNow;
            users.CreatedBy = userId;

            //var userSign = await _userManager.FindByEmailAsync(guest.EmailID);
            if (!_userManager.Users.Where(x => x.UserName == guest.EmailID).Any())
            {
                if (_userManager.Users.Where(x => x.PhoneNumber == guest.MobileNo).Any())
                {
                    signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "User mobile number already exist", 204);
                }
                else
                {
                    signedInUser = await Create(users, Password, "Customer", guest.DeviceId);
                }
            }
            else
            {
                if (_userManager.Users.Where(x => x.PhoneNumber == guest.MobileNo).Any())
                {
                    if (_userManager.Users.Where(x => x.UserName == guest.EmailID && x.PhoneNumber == guest.MobileNo).Any())
                    {
                        var user = _userManager.Users.Where(x => x.UserName == guest.EmailID && x.PhoneNumber == guest.MobileNo).FirstOrDefault();

                        if (user != null && await _userManager.IsInRoleAsync(user, "Customer"))
                        {
                            if (user.Status.ToLower() == "active")
                            {
                                string getUserType;
                                int UserTypeId = user.UserTypeId ?? 0;
                                getUserType = "default";

                                var Refreshdays = int.Parse(_configuration["JWT:RefreshTokenValidityInDays"]);

                                var refreshTk = GenerateRefreshToken();
                                //var tokenExpiry = DateTime.Now.AddDays(Refreshdays);
                                var tokenExpiry = DateTime.UtcNow.AddDays(Refreshdays);

                                var userRoles = await _userManager.GetRolesAsync(user);

                                var ClaimList = await loadClaims(user, getUserType, userRoles);

                                string accesstoken = _tokenManager.GenerateToken(user, ClaimList);

                                List<UserSessions> lstsessions = _dbContext.UserDeviceSessions.Where(x => x.DeviceId == guest.DeviceId).ToList();
                                if (lstsessions.Any())
                                {
                                    foreach (UserSessions item in lstsessions)
                                    {
                                        _dbContext.Remove(item);
                                        _dbContext.SaveChanges();
                                    }
                                }

                                UserSessions sessions = new UserSessions(user.Id, guest.DeviceId, refreshTk, tokenExpiry, accesstoken, DateTime.UtcNow);
                                _dbContext.Add(sessions);
                                _dbContext.SaveChanges();

                                UserSignInResponseModel userSignInResponse = new UserSignInResponseModel(user, getUserType, userRoles);
                                TokenModel tokenModel = new TokenModel(accesstoken, refreshTk, 200, "Token generated successfully.");
                                signedInUser = new SignedInUserResponse(SignInResult.Success, userSignInResponse, tokenModel, null, "Logged In Successfully");

                            }
                            else
                            {
                                signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Your account is inactive. Please contact the support team to have it activated");
                            }
                        }
                        else
                        {
                            signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Invalid Account!");
                        }
                    }
                    else
                    {
                        signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Email id already exist", 204);
                    }
                }
                else
                {
                    signedInUser = new SignedInUserResponse(SignInResult.Failed, null, null, null, "Email id already exist", 204);
                }
            }

            return signedInUser;
        }
    }
}
