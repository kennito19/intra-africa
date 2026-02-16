using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.IServices;
using Order.Application.Services;
using Order.Domain.Entity;
using Order.Domain;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Accessable")]
    public class IssueReasonController : ControllerBase
    {
        private readonly IIssueReasonService _issueReasonService;

        public IssueReasonController(IIssueReasonService issueReasonService)
        {
            _issueReasonService = issueReasonService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(IssueReason issueReason)
        {
            issueReason.CreatedAt = DateTime.Now;
            var data = await _issueReasonService.Create(issueReason);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(IssueReason issueReason)
        {
            issueReason.ModifiedAt = DateTime.Now;
            var data = await _issueReasonService.Update(issueReason);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            IssueReason issueReason = new IssueReason();
            issueReason.Id = Id;
            issueReason.DeletedBy = DeletedBy;
            issueReason.DeletedAt = DateTime.Now;
            var data = await _issueReasonService.Delete(issueReason);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<IssueReason>>> Get(int Id = 0, int actionId = 0, int Issuetypeid = 0, string? Reasons = null, string? actionName = null,string? searchtext = null, string? Issuetype = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            IssueReason issueReason = new IssueReason();
            issueReason.Id = Id;
            issueReason.IssueTypeId = Issuetypeid;
            issueReason.Reasons = Reasons;
            issueReason.IssueType = Issuetype;
            issueReason.IsDeleted = Isdeleted;
            issueReason.ActionId = actionId;
            issueReason.ActionName = actionName;
            issueReason.SearchText = searchtext;
            var data = await _issueReasonService.Get(issueReason, PageIndex, PageSize, Mode);
            return data;
        }

    }
}
