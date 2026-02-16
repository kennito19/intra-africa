using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Gateway.Controllers.Seller
{
    [Route("api/Seller/[controller]")]
    [ApiController]
    public class ManageOffersController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        private ManageOffers manageOffers;
        BaseResponse<ManageOffersLibrary> baseResponse = new BaseResponse<ManageOffersLibrary>();

        public ManageOffersController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;

            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            manageOffers = new ManageOffers(_configuration, URL,_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Create(ManageOffersDTO model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = manageOffers.SaveOffer(model, userID, false);

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Update(ManageOffersDTO model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = manageOffers.UpdateOffer(model, userID, false);

            return Ok(baseResponse);
        }

        [HttpPut("updateStatus")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> UpdateStatus(int offerId, string status)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = manageOffers.UpdateOfferStatus(offerId, status, userID);

            return Ok(baseResponse);
        }

        [HttpPut("optinOffer")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> optinOffer(int offerId, bool optIn, string SellerId)
        {
            BaseResponse<ManageOffersMapping> baseResponse = new BaseResponse<ManageOffersMapping>();
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = manageOffers.optinOffer(offerId, optIn, SellerId, userID);

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = manageOffers.DeleteOffer(id);

            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Get(int? pageindex = 1, int? pageSize = 10)
        {
            baseResponse = manageOffers.GetOffer(pageindex, pageSize);
            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = manageOffers.GetOfferById(id, userID);
            return Ok(baseResponse);
        }

        [HttpGet("byName")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByName(string name)
        {
            baseResponse = manageOffers.GetOfferByName(name);
            return Ok(baseResponse);
        }

        [HttpGet("sellerOffer")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetsellerOffer(string sellerId, string? searchtext = null, string? offerType = null, string? status = null, int? pageindex = 1, int? pageSize = 10)
        {
            baseResponse = manageOffers.GetSellerOffer(sellerId, searchtext , offerType, status, pageindex, pageSize);
            return Ok(baseResponse);
        }

        [HttpGet("sellerOptinOffer")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> sellerOptinOffer(string sellerId, string? searchtext = null, string? offerType = null, string? status = null, int pageindex = 1, int pageSize = 10)
        {
            baseResponse = manageOffers.GetOptInSellerOffer(sellerId, searchtext, offerType, status, pageindex, pageSize);
            return Ok(baseResponse);
        }

        [HttpGet("AdminOffer")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetAdminOffer(string sellerId, string? searchtext = null, string? offerType = null, string? status = null, int pageindex = 1, int pageSize = 10)
        {
            baseResponse = manageOffers.GetAdminOffer(sellerId, searchtext, offerType, status, pageindex, pageSize);
            return Ok(baseResponse);
        }

        [HttpGet("byOfferType")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByOfferType(string offerType)
        {
            baseResponse = manageOffers.GetOfferByOfferType(offerType);
            return Ok(baseResponse);
        }

        [HttpGet("byCode")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByCode(string code)
        {
            baseResponse = manageOffers.GetOfferByCode(code);
            return Ok(baseResponse);
        }

        [HttpGet("byStatus")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetByStatus(string status)
        {
            baseResponse = manageOffers.GetOfferByStatus(status);
            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, bool? showToCustomer = null, string? offerType = null, string? status = null, int? pageindex = 1, int? pageSize = 10)
        {
            baseResponse = manageOffers.Search(searchtext, showToCustomer, offerType, status, pageindex, pageSize);
            return Ok(baseResponse);
        }


    }
}
