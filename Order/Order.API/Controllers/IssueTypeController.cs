using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Order.Application.IServices;
using Order.Domain;
using Order.Domain.Entity;
using System;

namespace Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "Accessable")]
    public class IssueTypeController : ControllerBase
    {
        private readonly IIssueTypeService _issueTypeService;

        public IssueTypeController(IIssueTypeService issueTypeService)
        {
            _issueTypeService = issueTypeService;
        }

        [HttpPost]
        [Route("")]
        public async Task<BaseResponse<long>> Create(IssueType issueType)
        {
            issueType.CreatedAt = DateTime.Now;
            var data = await _issueTypeService.Create(issueType);
            return data;
        }

        [HttpPut]
        [Route("")]
        public async Task<BaseResponse<long>> Update(IssueType issueType)
        {
            issueType.ModifiedAt = DateTime.Now;
            var data = await _issueTypeService.Update(issueType);
            return data;
        }

        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse<long>> Delete(int Id)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            IssueType issueType = new IssueType();
            issueType.Id = Id;
            issueType.DeletedBy = DeletedBy;
            issueType.DeletedAt = DateTime.Now;
            var data = await _issueTypeService.Delete(issueType);
            return data;
        }

        [HttpGet]
        public async Task<BaseResponse<List<IssueType>>> Get(int Id = 0, int actionId = 0, string? Issue = null, string? actionName = null,string? searchtext = null, bool Isdeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            IssueType issueType = new IssueType();
            issueType.Id = Id;
            issueType.Issue = Issue;
            issueType.IsDeleted = Isdeleted;
            issueType.ActionId = actionId;
            issueType.ActionName = actionName;
            issueType.SearchText = searchtext;
            var data = await _issueTypeService.Get(issueType, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
