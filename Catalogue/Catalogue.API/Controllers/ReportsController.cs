using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalogue.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService _reportsService;

        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }


        [HttpGet("getProductReport")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductReport>>> GetProductReport(string? sellerId = null, string? fromDate = null, string? toDate = null)
        {
            var data = await _reportsService.GetProductReport(sellerId, fromDate, toDate);
            return data;
        }

        [HttpGet("getProductDetailsReport")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<ProductDetailsReport>>> GetProductDetailsReport(string? productIds = null)
        {
            var data = await _reportsService.GetProductDetailsReport(productIds);
            return data;
        }

    }
}
