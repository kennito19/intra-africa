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
    public class AssignReturnPolicyToCatagoryService : IAssignReturnPolicyToCatagoryService
    {
        private readonly IAssignReturnPolicyToCatagoryRepository _assignReturnPolicyToCatagoryRepository;
        public AssignReturnPolicyToCatagoryService(IAssignReturnPolicyToCatagoryRepository assignReturnPolicyToCatagoryRepository)
        {
            _assignReturnPolicyToCatagoryRepository = assignReturnPolicyToCatagoryRepository;
        }
        public Task<BaseResponse<long>> Create(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory)
        {
            var response = _assignReturnPolicyToCatagoryRepository.Create(assignReturnPolicyToCatagory);
            return response;

        }
        public Task<BaseResponse<long>> Update(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory)
        {
            var response = _assignReturnPolicyToCatagoryRepository.Update(assignReturnPolicyToCatagory);
            return response;
        }
        public Task<BaseResponse<long>> Delete(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory)
        {
            var response = _assignReturnPolicyToCatagoryRepository.Delete(assignReturnPolicyToCatagory);
            return response;
        }

        public Task<BaseResponse<List<AssignReturnPolicyToCatagory>>> get(AssignReturnPolicyToCatagory assignReturnPolicyToCatagory, int PageIndex, int PageSize, string Mode)
        {
            var response = _assignReturnPolicyToCatagoryRepository.get(assignReturnPolicyToCatagory, PageIndex, PageSize, Mode);
            return response;
        }

        
    }
}
