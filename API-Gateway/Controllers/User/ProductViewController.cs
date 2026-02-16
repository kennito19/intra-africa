using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.Order;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace API_Gateway.Controllers.User
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class ProductViewController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        public string CatalogueUrl = string.Empty;
        BaseResponse<ProductView> baseResponse = new BaseResponse<ProductView>();
        public ProductViewController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            //CatalogueUrl = "http://localhost:7246/";
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public ActionResult<ApiHelper> Save(ProductView model)
        {
            var temp = helper.ApiCall(CatalogueUrl, EndPoints.ProductView + "?ProductId=" + model.ProductId + "&SellerId" + model.SellerId + "&UserId=" + model.UserId + "&SellerProductId=" + model.SellerProductId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ProductView> tmp = (List<ProductView>)baseResponse.Data;
            if (!tmp.Any())
            {

                ProductView productView = new ProductView();
                productView.Id = model.Id;
                productView.ProductId = model.ProductId;
                productView.ProductGUID = model.ProductGUID;
                productView.SellerId = model.SellerId;
                productView.SellerProductId = model.SellerProductId;
                productView.UserId = model.UserId;
                productView.CreatedAt = DateTime.Now;


                var response = helper.ApiCall(CatalogueUrl, EndPoints.ProductView, "POST", productView);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Customer")]
        public ActionResult<ApiHelper> Get(int? ProductId = 0, string? SellerId = null, string? UserId = null, int? SellerProductId = 0, string? fromDate = null, string? toDate = null, int PageIndex = 1, int PageSize = 10)
        {
            string url = string.Empty;

            if (ProductId != null && ProductId != 0)
            {
                url += "&ProductId=" + ProductId;
            }
            if (SellerId != null && SellerId != "")
            {
                url += "&SellerId=" + SellerId;
            }
            if (UserId != null && UserId != "")
            {
                url += "&UserId=" + UserId;
            }
            if (SellerProductId != null && SellerProductId != 0)
            {
                url += "&SellerProductId=" + SellerProductId;
            }
            if (fromDate != null && fromDate != "")
            {
                url += "&fromDate=" + fromDate;
            }
            if (toDate != null && toDate != "")
            {
                url += "&toDate=" + toDate;
            }
            if (SellerId != null && SellerId != "")
            {
                url += "&SellerId=" + SellerId;
            }

            var temp = helper.ApiCall(CatalogueUrl, EndPoints.ProductView + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);            
            return Ok(baseResponse);
        }
    }
}
