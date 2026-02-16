using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Gateway.Controllers.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class ManageOffersController : ControllerBase
    {
        private ManageOffers manageOffers;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string URL = string.Empty;
        BaseResponse<ManageOffersDTO> baseResponse = new BaseResponse<ManageOffersDTO>();

        public ManageOffersController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            manageOffers = new ManageOffers(_configuration, URL, httpContextAccessor.HttpContext);
        }

        [HttpGet("search")]
        [Authorize]
        public ActionResult<ApiHelper> Search(string? userId = null, bool? showToCustomer = true, int? categoryId = 0, int? brandId = 0, int? productId = 0, string? sellerId = null, string? applyOn = null, string? Mode = "get")
        {
            baseResponse = manageOffers.GetOfferList(userId, showToCustomer, categoryId, productId, brandId, sellerId, applyOn, Mode);
            return Ok(baseResponse);
        }

    }
}
