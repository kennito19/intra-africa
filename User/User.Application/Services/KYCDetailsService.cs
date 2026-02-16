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
    public class KYCDetailsService : IKYCDetailsService
    {
        private readonly IKYCDetailsRepository _kYCDetailsRepository;

        public KYCDetailsService(IKYCDetailsRepository kYCDetailsRepository)
        {
            _kYCDetailsRepository = kYCDetailsRepository;
        }

        public Task<BaseResponse<long>> Create(KYCDetails kYCDetails)
        {
            var data = _kYCDetailsRepository.Create(kYCDetails);
            return data;
        }
        public Task<BaseResponse<long>> Update(KYCDetails kYCDetails)
        {
            var data = _kYCDetailsRepository.Update(kYCDetails);
            return data;
        }
        public Task<BaseResponse<long>> Delete(KYCDetails kYCDetails)
        {
            var data = _kYCDetailsRepository.Delete(kYCDetails);
            return data;
        }

        public Task<BaseResponse<List<KYCDetails>>> Get(KYCDetails kYCDetails, int PageIndex, int PageSize, string Mode)
        {
            var data = _kYCDetailsRepository.Get(kYCDetails, PageIndex, PageSize, Mode);
            return data;
        }
        public Task<BaseResponse<List<BasicKycDetails>>> GetBasicKycDetails(BasicKycDetails kYCDetails, bool IsDeleted, int PageIndex, int PageSize, string Mode)
        {
            var data = _kYCDetailsRepository.GetBasicKycDetails(kYCDetails, IsDeleted, PageIndex, PageSize, Mode);
            return data;
        }

        
    }
}
