using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.IServices;
using User.Application.Services;
using User.Domain.Entity;
using User.Domain;
using User.Domain.DTO;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDetailsController : ControllerBase
    {
        private readonly IUserDetailsServices _detailsServices;

        public UserDetailsController(IUserDetailsServices detailsServices)
        {
            _detailsServices = detailsServices;
        }

        [HttpPost]
        public async Task<BaseResponse<long>> Create(UserDetails userDetails)
        {
            var data = await _detailsServices.Create(userDetails);
            return data;
        }

        [HttpPut]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(UserDetails userDetails)
        {
            var data = await _detailsServices.Update(userDetails);
            return data;
        }

        [HttpDelete]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(UserDetails userDetails)
        {
            var data = await _detailsServices.Delete(userDetails);
            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<UserDetailsDTO>>> GetSellerDetails(int? Id = null, string? UserId = null, string? UserStatus = null, string? KycStatus = null, bool? GetArchived = false, bool? IsDeleted = false, string? SearchText = null, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            UserDetailsDTO sellerDetails = new UserDetailsDTO();
            if (Id != null)
            {
                sellerDetails.Id = Convert.ToInt32(Id);
            }
            sellerDetails.UserId = UserId;
            sellerDetails.UserStatus = UserStatus;
            sellerDetails.IsDeleted = IsDeleted;

            var data = await _detailsServices.GetUserDetails(sellerDetails, KycStatus, GetArchived, SearchText, PageIndex, PageSize, Mode);
            return data;
        }
       
    }
}
