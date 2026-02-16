using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.IServices;
using User.Application.Services;
using User.Domain;
using User.Domain.Entity;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KycCountController : ControllerBase
    {
        private readonly IKycCountService _kycCountService;

        public KycCountController(IKycCountService kycCountService)
        {
            _kycCountService = kycCountService;
        }

        [HttpGet]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<List<KycCounts>>> Get(string? days = null)
        {
            var data = await _kycCountService.get(days);
            return data;
        }
    }
}
