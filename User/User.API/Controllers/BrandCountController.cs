using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.IServices;
using User.Domain;
using User.Domain.Entity;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandCountController : ControllerBase
    {
        private readonly IBrandCountService _brandCountService;

        public BrandCountController(IBrandCountService brandCountService)
        {
            _brandCountService = brandCountService;
        }

        [HttpGet]
        [Authorize(Policy = "Accessable")]
        public async Task<BaseResponse<List<BrandCounts>>> Get(string? sellerId = null, string? days = null)
        {
            var data = await _brandCountService.get(sellerId, days);
            return data;
        }
    }
}
