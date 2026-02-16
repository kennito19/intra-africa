using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class ReturnPolicyDetailService : IReturnPolicyDetailService
    {
        private readonly IReturnPolicyDetailRepository _returnPolicyDetailRepository;

        public ReturnPolicyDetailService(IReturnPolicyDetailRepository returnPolicyDetailRepository)
        {
            _returnPolicyDetailRepository = returnPolicyDetailRepository;
        }

        public Task<BaseResponse<long>> AddReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail)
        {
            var responce = _returnPolicyDetailRepository.AddReturnPolicyDetail(returnPolicyDetail);
            return responce;
        }

        public Task<BaseResponse<long>> DeleteReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail)
        {
            var responce = _returnPolicyDetailRepository.DeleteReturnPolicyDetail(returnPolicyDetail);
            return responce;
        }

        public Task<BaseResponse<List<ReturnPolicyDetail>>> GetReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail, int PageIndex, int PageSize, string Mode)
        {
            var responce = _returnPolicyDetailRepository.GetReturnPolicyDetail(returnPolicyDetail, PageIndex, PageSize, Mode);
            return responce;
        }

        public Task<BaseResponse<long>> UpdateReturnPolicyDetail(ReturnPolicyDetail returnPolicyDetail)
        {
            var responce = _returnPolicyDetailRepository.UpdateReturnPolicyDetail(returnPolicyDetail);
            return responce;
        }
    }
}
