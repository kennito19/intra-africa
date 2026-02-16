using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using System.Net.WebSockets;

namespace API_Gateway.Common
{
    public class sellerKycListDetail
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public string IdserverURL = string.Empty;
        public string UserURL = string.Empty;
        public string CatelogueURL = string.Empty;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        public sellerKycListDetail(IConfiguration configuration, HttpContext httpContext)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            IdserverURL = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            UserURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }



        public List<UserDetailsDTO> bindSellerDetails(bool? GetArchived = null, string? UserStatus = null, string? KycStatus = null, string? searchtext = null, string? UserId = null,int pageIndex = 1, int pageSize = 10 )
        {
            BaseResponse<UserDetailsDTO> baseResponse = new BaseResponse<UserDetailsDTO>();
            BaseResponse<ChargesPaidByLibrary> baseResponseCharges = new BaseResponse<ChargesPaidByLibrary>();
            List<ChargesPaidByLibrary> Charges = new List<ChargesPaidByLibrary>();

            string url = string.Empty;
            if (KycStatus != null && KycStatus.ToLower() == "approved")
            {
                url += "&KycStatus=" + KycStatus;
            }
            if (UserStatus != null)
            {
                url += "&UserStatus=" + UserStatus;
            }
            if (GetArchived != null)
            {
                url += "&GetArchived=" + GetArchived;
            }

            if (searchtext != null)
            {
                url += "&SearchText=" + searchtext;
            }
            if (UserId != null)
            {
                url += "&UserId=" + UserId;
            }

            var response = helper.ApiCall(UserURL, EndPoints.UserDetails + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<UserDetailsDTO> lstkyc = (List<UserDetailsDTO>)baseResponse.Data;

            var response1_1 = helper.ApiCall(CatelogueURL, EndPoints.ChargesPaidBy + "?pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
            baseResponseCharges = baseResponseCharges.JsonParseList(response1_1);
            if (baseResponseCharges.code == 200)
            {
                Charges = (List<ChargesPaidByLibrary>)baseResponseCharges.Data;

            }
            foreach (UserDetailsDTO seller in lstkyc)
            {
                seller.ShipmentChargesPaidByName = seller != null ? seller.ShipmentChargesPaidBy != null && seller.ShipmentChargesPaidBy != 0 ? Charges.Count > 0 ? Charges.Where(p => p.Id == Convert.ToInt32(seller.ShipmentChargesPaidBy)).FirstOrDefault().Name : null : null : null;
            }

            return lstkyc;
        }
    }
}
