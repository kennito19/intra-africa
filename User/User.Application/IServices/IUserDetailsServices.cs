using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;
using User.Domain.DTO;

namespace User.Application.IServices
{
    public interface IUserDetailsServices
    {
        Task<BaseResponse<long>> Create(UserDetails userDetails);
        Task<BaseResponse<long>> Update(UserDetails userDetails);
        Task<BaseResponse<long>> Delete(UserDetails userDetails);
        Task<BaseResponse<List<UserDetailsDTO>>> GetUserDetails(UserDetailsDTO sellerDetails, string? KycStatus, bool? GetArchived, string? SearchText, int? PageIndex, int? PageSize, string? Mode);
    }
}
