using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entity;
using User.Domain;
using User.Domain.DTO;

namespace User.Application.IRepositories
{
    public interface IKYCDetailsRepository
    {
        Task<BaseResponse<long>> Create(KYCDetails kYCDetails);
        Task<BaseResponse<long>> Update(KYCDetails kYCDetails);
        Task<BaseResponse<long>> Delete(KYCDetails kYCDetails);
        Task<BaseResponse<List<KYCDetails>>> Get(KYCDetails kYCDetails, int PageIndex, int PageSize, string Mode);
        Task<BaseResponse<List<BasicKycDetails>>> GetBasicKycDetails(BasicKycDetails kYCDetails, bool IsDeleted, int PageIndex, int PageSize, string Mode);
    }
}
