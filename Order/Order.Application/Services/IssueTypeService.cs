using Order.Application.IRepositories;
using Order.Application.IServices;
using Order.Domain;
using Order.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public class IssueTypeService : IIssueTypeService
    {
        private readonly IIssueTypeRepository _issueTypeRepository;

        public IssueTypeService(IIssueTypeRepository issueTypeRepository)
        {
            _issueTypeRepository = issueTypeRepository;
        }

        Task<BaseResponse<long>> IIssueTypeService.Create(IssueType issueType)
        {
            var response = _issueTypeRepository.Create(issueType);
            return response;
        }

        Task<BaseResponse<long>> IIssueTypeService.Update(IssueType issueType)
        {
            var response = _issueTypeRepository.Update(issueType);
            return response;
        }

        Task<BaseResponse<long>> IIssueTypeService.Delete(IssueType issueType)
        {
            var response = _issueTypeRepository.Delete(issueType);
            return response;
        }

        Task<BaseResponse<List<IssueType>>> IIssueTypeService.Get(IssueType issueType, int PageIndex, int PageSize, string Mode)
        {
            var response = _issueTypeRepository.Get(issueType, PageIndex, PageSize, Mode);
            return response;
        }

       
    }
}
