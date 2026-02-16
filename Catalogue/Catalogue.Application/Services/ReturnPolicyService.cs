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
    public class ReturnPolicyService : IReturnPolicyService
    {
        private readonly IReturnPolicyRepository _returnPolicyRepository;

        public ReturnPolicyService(IReturnPolicyRepository returnPolicyRepository)
        {
            _returnPolicyRepository = returnPolicyRepository;
        }

        public Task<BaseResponse<long>> AddReturnPolicy(ReturnPolicy returnPolicy)
        {
            var responce = _returnPolicyRepository.AddReturnPolicy(returnPolicy);
            return responce;
        }

        public Task<BaseResponse<long>> DeleteReturnPolicy(ReturnPolicy returnPolicy)
        {
            var responce = _returnPolicyRepository.DeleteReturnPolicy(returnPolicy);
            return responce;
        }

        public Task<BaseResponse<List<ReturnPolicy>>> GetReturnPolicy(ReturnPolicy returnPolicy, int PageIndex, int PageSize, string Mode)
        {
            var responce = _returnPolicyRepository.GetReturnPolicy(returnPolicy, PageIndex, PageSize, Mode);
            return responce;
        }

        public Task<BaseResponse<long>> UpdateReturnPolicy(ReturnPolicy returnPolicy)
        {
            var responce = _returnPolicyRepository.UpdateReturnPolicy(returnPolicy);
            return responce;
        }
    }
}
