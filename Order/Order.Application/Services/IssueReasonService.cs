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
    public class IssueReasonService:IIssueReasonService
    {
        private readonly IIssueReasonRepository _issueReasonRepository;

        public IssueReasonService(IIssueReasonRepository issueReasonRepository)
        {
            _issueReasonRepository = issueReasonRepository;
        }

        Task<BaseResponse<long>> IIssueReasonService.Create(IssueReason issueReason)
        {
            var response = _issueReasonRepository.Create(issueReason);
            return response;
        }
        Task<BaseResponse<long>> IIssueReasonService.Update(IssueReason issueReason)
        {
            var response = _issueReasonRepository.Update(issueReason);
            return response;
        }

        Task<BaseResponse<long>> IIssueReasonService.Delete(IssueReason issueReason)
        {
            var response = _issueReasonRepository.Delete(issueReason);
            return response;
        }

        Task<BaseResponse<List<IssueReason>>> IIssueReasonService.Get(IssueReason issueReason, int PageIndex, int PageSize, string Mode)
        {
            var response = _issueReasonRepository.Get(issueReason, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
