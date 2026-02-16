using API_Gateway.Common;
using API_Gateway.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace API_Gateway.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageLendingPageSectionController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;

        public ManageLendingPageSectionController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;

            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        [HttpGet("GetLendingPageSection")]
        [Authorize]
        public ActionResult<ApiHelper> GetLendingPageSection(int LendingPageId, string? status = null)
        {
            getLendingPageSections getHomePage = new getLendingPageSections(_configuration, _httpContext);
            JObject res = getHomePage.setSections(LendingPageId, status);
            return Ok(res.ToString());
        }
    }
}
