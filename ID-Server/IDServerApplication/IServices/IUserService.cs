using IDServer.Domain.DTO;
using IDServer.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IDServerApplication.IServices
{
    public interface IUserService
    {
        string GenerateNoAuthToken(string deviceId);
        Task<BaseResponse<string>> ChangePasswordAsync(ChangePassword model, string UserID);
        Task<BaseResponse<string>> ResetPasswordAsync(ResetPassword model, string UserID);
        Task<BaseResponse<ResetModel>> GeneratePasswordChangeTokenAsync(ForgetPassword forgotPassword);
        IdentityResult CreateRole(string Name);
        Task<IdentityResult> CreateRoleBaseClaims(string RoleID, string ClaimType, string ClaimValue);
        Task<SignedInUserResponse> Create(Users users, string Password, string roleName, string? DeviceId = null);

        Task<IdentityResult> CreateSpecificUserClaim(string UserId, string ClaimType, string ClaimValue);
       
        Task<UserClaimResponse> GetUserRoleClaims(string UserId);

        Task<UserClaimResponse> GetUserSpecificClaims(string UserId);

        List<UserSessions> GetUserActiveSessions(string UserId);

        Task<SignedInUserResponse> SignIn(SignIn signIn);

        List<string> RolesList();

        Task SignOut();

        Task<BaseResponse<AdminListModel>> UpdateAdminDetails(AdminListModel model);
        Task<BaseResponse<SellerListModel>> UpdateSellerDetails(SellerListModel model);
        Task<BaseResponse<SellerListModel>> DeleteSellerDetails(Users model);
        Task<BaseResponse<AdminListModel>> GetAdmins(string? ID, string? searchString = null, string? status = null, int pageIndex = 0, int pageSize = 0);
        Task<BaseResponse<AdminListModel>> getNoSuperAdmin(int pageIndex = 0, int pageSize = 0, string? status = null);
        Task<BaseResponse<AdminListModel>> GetAdminById(string Id);
        Task<BaseResponse<AdminListModel>> GetAdminByEmail(string Email);

        Task<SignedInUserResponse> SellerSignIn(SignIn signIn);
        Task<BaseResponse<SellerListModel>> GetSellers(int pageIndex, int pageSize, string? searchString = null, string? fromDate = null, string? toDate = null, string? status = null, bool? IsArchive = null);
        Task<BaseResponse<SellerListModel>> GetSellerByID(string ID);
        Task<BaseResponse<SellerListModel>> GetSellerByEmail(string Email);
        Task<BaseResponse<CustomerListModel>> UpdateCustomerDetails(CustomerListModel model);
        Task<BaseResponse<CustomerListModel>> DeleteCustomerDetails(Users model);

        Task<SignedInUserResponse> CustomerSignIn(SignIn signIn);
        Task<BaseResponse<CustomerListModel>> GetCustomers(int pageIndex, int pageSize, string? searchString = null);
        Task<BaseResponse<CustomerListModel>> GetCustomerByID(string ID);
        Task<BaseResponse<CustomerListModel>> GetCustomerByEmail(string Email);


        Task<List<Claim>> loadClaims(Users user, string UserType ,IList<string> UserRoles);
        void setNewRefreshToken(string user, string refreshToken);

        Task<BaseResponse<string>> GenerateMobileOTP(GenerateMobileOtp model);
        Task<SignedInUserResponse> SignInViaOTP(SignInViaOtp signIn);
        Task<SignedInUserResponse> SignInViaEmailId(SignInViaEmailId signIn);
        BaseResponse<string> UserLogout(Logout model);
        BaseResponse<UserSessions> UserSession(UserSessiondto model);
        string GenerateRandomPassword();
        Task<SignedInUserResponse> Guestcheckout(GuestUser guest, string userId);
    }
}
