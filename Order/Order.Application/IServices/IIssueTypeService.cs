using Order.Domain;
using Order.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.IServices
{
    public interface IIssueTypeService
    {
        Task<BaseResponse<long>> Create(IssueType issueType);

        Task<BaseResponse<long>> Update(IssueType issueType);

        Task<BaseResponse<long>> Delete(IssueType issueType);

        Task<BaseResponse<List<IssueType>>> Get(IssueType issueType, int PageIndex, int PageSize, string Mode);


    }
}
