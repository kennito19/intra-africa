using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.IServices;
using User.Domain.Entity;
using User.Domain;
using Microsoft.AspNetCore.Authorization;
using User.Domain.DTO;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class KYCDetailsController : ControllerBase
    {
        private readonly IKYCDetailsService _kYCDetailsService;

        public KYCDetailsController(IKYCDetailsService kYCDetailsService)
        {
            _kYCDetailsService = kYCDetailsService;
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Create(KYCDetails kYCDetails)
        {
            kYCDetails.CreatedAt = DateTime.Now;
            var data = await _kYCDetailsService.Create(kYCDetails);
            return data;
        }

        [HttpPut]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Update(KYCDetails kYCDetails)
        {
            kYCDetails.ModifiedAt = DateTime.Now;
            var data = await _kYCDetailsService.Update(kYCDetails);
            return data;
        }

        [HttpDelete]
        [Route("")]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<long>> Delete(int? id, string? userID)
        {
            string DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            KYCDetails kYCDetails = new KYCDetails();
            kYCDetails.Id = id;
            kYCDetails.UserID = userID;
            kYCDetails.DeletedBy = DeletedBy;
            kYCDetails.DeletedAt = DateTime.Now;
            var data = await _kYCDetailsService.Delete(kYCDetails);

            return data;
        }

        [HttpGet]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<KYCDetails>>> Get(int? Id = null, string? UserID = null, string? Status = null, bool? IsDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            KYCDetails kYCDetails = new KYCDetails();
            if (Id != null)
            {
                kYCDetails.Id = Convert.ToInt32(Id);
            }
            
            kYCDetails.UserID = UserID;
            kYCDetails.Status = Status;
            kYCDetails.IsDeleted = Convert.ToBoolean(IsDeleted);
            var data = await _kYCDetailsService.Get(kYCDetails, PageIndex, PageSize, Mode);
            return data;
        }

        [HttpGet("getBasicKyc")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<BasicKycDetails>>> GetBasicKyc(int? Id = null, string? UserID = null, string? Kycfor = null, string? Status = null, bool? IsDeleted = false, int PageIndex = 1, int PageSize = 10, string? Mode = "get")
        {
            BasicKycDetails kYCDetails = new BasicKycDetails();
            if (Id != null)
            {
                kYCDetails.Id = Convert.ToInt32(Id);
            }
            kYCDetails.UserID = UserID;
            kYCDetails.Status = Status;
            kYCDetails.KYCFor = Kycfor;
            var data = await _kYCDetailsService.GetBasicKycDetails(kYCDetails, Convert.ToBoolean(IsDeleted), PageIndex, PageSize, Mode);
            return data;
        }
    }
}
