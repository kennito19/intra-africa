using Order.Domain.Entity;
using Order.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IRepositories
{
    public interface IIssueReasonRepository
    {
        Task<BaseResponse<long>> Create(IssueReason issueReason);

        Task<BaseResponse<long>> Update(IssueReason issueReason);

        Task<BaseResponse<long>> Delete(IssueReason issueReason);

        Task<BaseResponse<List<IssueReason>>> Get(IssueReason issueReason, int PageIndex, int PageSize, string Mode);


    }
}
