using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Application.IServices;
using User.Application.Services;
using User.Domain.DTO;
using User.Domain;

namespace User.API.Controllers
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

        [HttpGet("getGSTReport")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<GSTReport>>> GetGSTReport(string? sellerId = null, string? fromDate = null, string? toDate = null)
        {
            var data = await _reportsService.GetGSTReport(sellerId, fromDate, toDate);
            return data;
        }

        [HttpGet("getWarehouseReport")]
        [Authorize(Policy = "General")]
        public async Task<BaseResponse<List<WarehouseReport>>> GetWarehouseReport(string? sellerId = null, string? fromDate = null, string? toDate = null)
        {
            var data = await _reportsService.GetWarehouseReport(sellerId, fromDate, toDate);
            return data;
        }
    }
}
