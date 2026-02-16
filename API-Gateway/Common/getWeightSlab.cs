using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;
using System.Text.RegularExpressions;

namespace API_Gateway.Common
{
    public class getWeightSlab
    {
        private readonly IConfiguration _configuration;
        public string CatelogueURL = string.Empty;
        public string IdServerURL = string.Empty;
        public string UserURL = string.Empty;
        public string UserId = string.Empty;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        public getWeightSlab(IConfiguration configuration, HttpContext httpContext)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        public WeightSlabLibrary Get(decimal weight)
        {
            BaseResponse<WeightSlabLibrary> baseResponse = new BaseResponse<WeightSlabLibrary>();
            WeightSlabLibrary weightSlab = new WeightSlabLibrary();

            var response = helper.ApiCall(CatelogueURL, EndPoints.WeightSlab + "?PageIndex=0&PageSize=0", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<WeightSlabLibrary> details = (List<WeightSlabLibrary>)baseResponse.Data;
            Regex regex = new Regex(@"(\d+(\.\d+)?)\s*[-|to]\s*(\d+(\.\d+)?)");
            decimal preValue = 0;
            foreach (var items in details)
            {
                decimal _weight = Convert.ToDecimal(Convert.ToDecimal(weight).ToString("N1"));
                Match match = regex.Match(items.WeightSlab);
                if (match.Groups.Count > 1)
                {
                    decimal slab1 = decimal.Parse(match.Groups[1].Value);
                    decimal slab2 = decimal.Parse(match.Groups[3].Value);
                    if (_weight >= slab1 && _weight <= slab2)
                    {
                        weightSlab = items;
                        break;
                    }
                }
                else
                {

                    decimal slab1 = Convert.ToDecimal(items.WeightSlab);
                    if (_weight <= slab1 && _weight > preValue)
                    {
                        weightSlab = items;
                        break;
                    }

                    preValue = slab1;
                }
            }

            return weightSlab;
        }
    }
}
