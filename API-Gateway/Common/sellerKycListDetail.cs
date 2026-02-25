using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using System.Linq;
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
            if (!string.IsNullOrWhiteSpace(UserStatus))
            {
                url += "&UserStatus=" + UserStatus;
            }
            if (GetArchived != null)
            {
                url += "&GetArchived=" + GetArchived;
            }

            if (!string.IsNullOrWhiteSpace(searchtext))
            {
                url += "&SearchText=" + searchtext;
            }
            if (!string.IsNullOrWhiteSpace(UserId))
            {
                url += "&UserId=" + UserId;
            }

            var response = helper.ApiCall(UserURL, EndPoints.UserDetails + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response) ?? new BaseResponse<UserDetailsDTO>();
            List<UserDetailsDTO> lstkyc = (baseResponse.Data as List<UserDetailsDTO> ?? new List<UserDetailsDTO>())
                .Where(s => s != null)
                .ToList();

            var response1_1 = helper.ApiCall(CatelogueURL, EndPoints.ChargesPaidBy + "?pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
            baseResponseCharges = baseResponseCharges.JsonParseList(response1_1) ?? new BaseResponse<ChargesPaidByLibrary>();
            if (baseResponseCharges.code == 200)
            {
                Charges = (baseResponseCharges.Data as List<ChargesPaidByLibrary> ?? new List<ChargesPaidByLibrary>())
                    .Where(c => c != null)
                    .ToList();

            }
            foreach (UserDetailsDTO seller in lstkyc)
            {
                if (seller?.ShipmentChargesPaidBy != null && seller.ShipmentChargesPaidBy != 0 && Charges.Count > 0)
                {
                    var charge = Charges.FirstOrDefault(p => p.Id == Convert.ToInt32(seller.ShipmentChargesPaidBy));
                    seller.ShipmentChargesPaidByName = charge?.Name;
                }
                else
                {
                    seller.ShipmentChargesPaidByName = null;
                }
            }

            return lstkyc;
        }
    }
}
