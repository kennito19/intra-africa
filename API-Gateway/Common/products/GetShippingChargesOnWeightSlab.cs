using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;

namespace API_Gateway.Common.products
{
    public class GetShippingChargesOnWeightSlab
    {
        private readonly HttpContext _httpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        BaseResponse<WeightSlabLibrary> baseResponse = new BaseResponse<WeightSlabLibrary>();

        public GetShippingChargesOnWeightSlab(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
        }

        public WeightSlabLibrary GetShippingCharges(int weightslabId, string URL)
        {
            WeightSlabLibrary Shippingcharges = new WeightSlabLibrary();
            ApiHelper helper = new ApiHelper(_httpContext);
            var response = helper.ApiCall(URL, EndPoints.WeightSlab + "?Id=" + weightslabId, "GET", null);
            if (response != null)
            {
                baseResponse = new BaseResponse<WeightSlabLibrary>();
                baseResponse = baseResponse.JsonParseRecord(response);
                Shippingcharges = (WeightSlabLibrary)baseResponse.Data;
            }
            else
            {
                Shippingcharges = new WeightSlabLibrary(); ;
            }
            return Shippingcharges;
        }
    }
}
