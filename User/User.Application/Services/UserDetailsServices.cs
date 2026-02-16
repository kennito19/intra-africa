using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Application.IServices;
using User.Domain;
using User.Domain.DTO;
using User.Domain.Entity;

namespace User.Application.Services
{
    public class UserDetailsServices : IUserDetailsServices
    {
        private readonly IUserDetailsRepository _userDetailsRepository;

        public UserDetailsServices(IUserDetailsRepository userDetailsRepository)
        {
            _userDetailsRepository = userDetailsRepository;
        }

        public Task<BaseResponse<long>> Create(UserDetails userDetails)
        {
            var data = _userDetailsRepository.Create(userDetails);
            return data;
        }
        public Task<BaseResponse<long>> Update(UserDetails userDetails)
        {
            var data = _userDetailsRepository.Update(userDetails);
            return data;
        }

        public Task<BaseResponse<long>> Delete(UserDetails userDetails)
        {
            var data = _userDetailsRepository.Delete(userDetails);
            return data;
        }

        public Task<BaseResponse<List<UserDetailsDTO>>> GetUserDetails(UserDetailsDTO sellerDetails, string? KycStatus, bool? GetArchived, string? SearchText, int? PageIndex, int? PageSize, string? Mode)
        {
            var data = _userDetailsRepository.GetUserDetails(sellerDetails, KycStatus, GetArchived, SearchText, PageIndex, PageSize, Mode);
            return data;
        }

    }
}