using IDServer.Domain.DTO;
using IDServer.Domain.Entity;
using IDServerApplication.IRepositories;
using IDServerApplication.IServices;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IDServerApplication.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public IdentityResult CreateRole(string Name)
        {
            var user = _userRepository.CreateRole(Name);
            return user;
        }

        public Task<IdentityResult> CreateRoleBaseClaims(string RoleID, string ClaimType, string ClaimValue)
        {
            var user = _userRepository.CreateRoleBaseClaims(RoleID, ClaimType, ClaimValue);
            return user;
        }

        public async Task<SignedInUserResponse> Create(Users users, string Password, string roleName, string? DeviceId = null)
        {
            var user = await _userRepository.Create(users, Password, roleName, DeviceId);
            return user;
        }

        public async Task<SignedInUserResponse> SignIn(SignIn signIn)
        {
            var user = await _userRepository.SignIn(signIn);
            return user;
        }

        public Task SignOut()
        {
            var user = _userRepository.SignOut();
            return user;
        }

        public async Task<BaseResponse<AdminListModel>> UpdateAdminDetails(AdminListModel model)
        {
            var response = await _userRepository.UpdateAdminDetails(model);
            return response;
        }

        public async Task<BaseResponse<SellerListModel>> UpdateSellerDetails(SellerListModel model)
        {
            var response = await _userRepository.UpdateSellerDetails(model);
            return response;
        }

        public async Task<BaseResponse<SellerListModel>> DeleteSellerDetails(Users model)
        {
            var response = await _userRepository.DeleteSellerDetails(model);
            return response;
        }

        public async Task<BaseResponse<AdminListModel>> GetAdmins(string? ID, string? searchString = null, string? status = null, int pageIndex = 0, int pageSize = 0)
        {
            var userList = await _userRepository.GetAdmins(ID, searchString, status, pageIndex, pageSize);
            return userList;
        }
        public async Task<BaseResponse<AdminListModel>> getNoSuperAdmin(int pageIndex = 0, int pageSize = 0, string? status = null)
        {
            var userList = await _userRepository.getNoSuperAdmin(pageIndex, pageSize, status);
            return userList;
        }
        
        public async Task<BaseResponse<AdminListModel>> GetAdminById(string Id)
        {
            var userList = await _userRepository.GetAdminByID(Id);
            return userList;
        }

        public async Task<BaseResponse<AdminListModel>> GetAdminByEmail(string Email)
        {
            var userList = await _userRepository.GetAdminByEmail(Email);
            return userList;
        }

        public async Task<SignedInUserResponse> SellerSignIn(SignIn signIn)
        {
            var user = await _userRepository.SellerSignIn(signIn);
            return user;
        }

        public async Task<BaseResponse<SellerListModel>> GetSellers(int pageIndex, int pageSize, string? searchString = null, string? fromDate = null, string? toDate = null, string? status = null, bool? IsArchive = null)
        {
            var userList = await _userRepository.GetSellers(pageIndex, pageSize, searchString, fromDate, toDate, status, IsArchive);
            return userList;
        }

        public async Task<BaseResponse<SellerListModel>> GetSellerByID(string ID)
        {
            var userList = await _userRepository.GetSellerByID(ID);
            return userList;
        }

        public async Task<BaseResponse<SellerListModel>> GetSellerByEmail(string Email)
        {
            var userList = await _userRepository.GetSellerByEmail(Email);
            return userList;
        }

        public async Task<SignedInUserResponse> CustomerSignIn(SignIn signIn)
        {
            var user = await _userRepository.CustomerSignIn(signIn);
            return user;
        }

        public async Task<BaseResponse<CustomerListModel>> GetCustomers(int pageIndex, int pageSize, string? searchString = null)
        {
            var userList = await _userRepository.GetCustomers(pageIndex, pageSize, searchString);
            return userList;
        }

        public async Task<BaseResponse<CustomerListModel>> GetCustomerByID(string ID)
        {
            var userList = await _userRepository.GetCustomerByID(ID);
            return userList;
        }

        public async Task<BaseResponse<CustomerListModel>> GetCustomerByEmail(string Email)
        {
            var userList = await _userRepository.GetCustomerByEmail(Email);
            return userList;
        }

        public List<Users> List()
        {
            var user = _userRepository.List().Where(X => X.IsDeleted == false).ToList();
            return user;
        }
        public async Task<List<Claim>> loadClaims(Users user, string UserType, IList<string> UserRoles)
        {
            List<Claim> claims = await _userRepository.loadClaims(user, UserType, UserRoles);
            return claims;
        }
        public void setNewRefreshToken(string user, string refreshToken)
        {
            _userRepository.setNewRefreshToken(user, refreshToken);
        }

        public async Task<UserClaimResponse> GetUserRoleClaims(string UserId)
        {
            var response = await _userRepository.GetUserRoleClaims(UserId);
            return response;
        }

        public async Task<UserClaimResponse> GetUserSpecificClaims(string UserId)
        {
            var response = await _userRepository.GetUserSpecificClaims(UserId);
            return response;
        }

        public Task<IdentityResult> CreateSpecificUserClaim(string UserId, string ClaimType, string ClaimValue)
        {
            var response = _userRepository.CreateSpecificUserClaim(UserId, ClaimType, ClaimValue);
            return response;
        }

        public List<string> RolesList()
        {
            var list = _userRepository.RolesList();
            return list;
        }

        public List<UserSessions> GetUserActiveSessions(string UserId)
        {
            var list = _userRepository.GetUserActiveSessions(UserId);
            return list;
        }

        public async Task<BaseResponse<string>> ChangePasswordAsync(ChangePassword model, string UserID)
        {
            var dataresponse = await _userRepository.ChangePasswordAsync(model, UserID);
            return dataresponse;
        }

        public async Task<BaseResponse<string>> ResetPasswordAsync(ResetPassword model, string UserID)
        {
            var dataresponse = await _userRepository.ResetPasswordAsync(model, UserID);
            return dataresponse;
        }

        public async Task<BaseResponse<ResetModel>> GeneratePasswordChangeTokenAsync(ForgetPassword forgotPassword)
        {
            var res = await _userRepository.GeneratePasswordChangeTokenAsync(forgotPassword);
            return res;
        }

        public async Task<BaseResponse<CustomerListModel>> UpdateCustomerDetails(CustomerListModel model)
        {
            var response = await _userRepository.UpdateCustomerDetails(model);
            return response;
        }

        public async Task<BaseResponse<CustomerListModel>> DeleteCustomerDetails(Users model)
        {
            var response = await _userRepository.DeleteCustomerDetails(model);
            return response;
        }

        public string GenerateNoAuthToken(string deviceId)
        {
            string token = _userRepository.GenerateNoAuthToken(deviceId);
            return token;
        }

        public async Task<BaseResponse<string>> GenerateMobileOTP(GenerateMobileOtp model)
        {
            var response = await _userRepository.GenerateMobileOTP(model);
            return response;
        }

        public async Task<SignedInUserResponse> SignInViaOTP(SignInViaOtp signIn)
        {
            var response = await _userRepository.SignInViaOTP(signIn);
            return response;
        }

        public async Task<SignedInUserResponse> SignInViaEmailId(SignInViaEmailId signIn)
        {
            var response = await _userRepository.SignInViaEmailId(signIn);
            return response;
        }

        public BaseResponse<string> UserLogout(Logout model)
        {
            var response = _userRepository.UserLogout(model);
            return response;
        }

        public BaseResponse<UserSessions> UserSession(UserSessiondto model)
        {
            var response = _userRepository.UserSession(model);
            return response;
        }

        public string GenerateRandomPassword()
        {
            var response = _userRepository.GenerateRandomPassword();
            return response;
        }
        public Task<SignedInUserResponse> Guestcheckout(GuestUser guest, string userId)
        {
            var response = _userRepository.Guestcheckout(guest, userId);
            return response;
        }
    }
}
