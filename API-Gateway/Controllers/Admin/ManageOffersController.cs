using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/Admin/[controller]")]
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
            manageOffers = new ManageOffers(_configuration, URL, _httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(ManageOffersDTO model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = manageOffers.SaveOffer(model, userID, true);

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(ManageOffersDTO model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = manageOffers.UpdateOffer(model, userID, true);

            return Ok(baseResponse);
        }

        [HttpPut("updateStatus")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> UpdateStatus(int offerId, string status)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = manageOffers.UpdateOfferStatus(offerId, status, userID);

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            baseResponse = manageOffers.DeleteOffer(id);

            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> GetById(int id)
        {
            baseResponse = manageOffers.GetOfferById(id);
            return Ok(baseResponse);
        }

        [HttpGet("byOfferType")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> GetByOfferType(string offerType)
        {
            baseResponse = manageOffers.GetOfferByOfferType(offerType);
            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Search(string? searchtext=null, bool? showToCustomer = null, string? offerType=null, string? status = null, int? pageindex = 1, int? pageSize = 10)
        {
            baseResponse = manageOffers.Search(searchtext, showToCustomer, offerType, status, pageindex, pageSize);
            return Ok(baseResponse);
        }


    }
}
