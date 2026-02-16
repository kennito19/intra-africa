using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;

namespace API_Gateway.Common.products
{
    public class ArchiveProduct
    {
        public string CatelogueURL = string.Empty;
        public string UserId = string.Empty;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;

        public ArchiveProduct(HttpContext httpContext, string Userid, string URL)
        {
            UserId = Userid;
            _httpContext = httpContext;
            CatelogueURL = URL;
            helper = new ApiHelper(_httpContext);
        }

        public BaseResponse<string> ArchiveProductData(ProductDelete model)
        {
            BaseResponse<string> baseResponse = new BaseResponse<string>();
            int ProductId = model.productId;
            int SellerProductId = model.SellerProductId;
            if (!string.IsNullOrEmpty(model.SellerId))
            {
                var sellerProduct = SellerProductBySellerId(model.SellerId);
                if (sellerProduct.code == 200)
                {
                    baseResponse.code = 200;
                    baseResponse.Message = "Product archived successfully.";
                }
            }
            else
            {
                var sellerProduct = SellerProduct(SellerProductId);
                if (sellerProduct.code == 200)
                {
                    baseResponse.code = 200;
                    baseResponse.Message = "Product archived successfully.";
                }
                else
                {
                    baseResponse.code = sellerProduct.code;
                    baseResponse.Message = sellerProduct.Message;
                    baseResponse.Data = sellerProduct.Data;
                }
            }


            return baseResponse;
        }

        public string ArchiveExistingData(ProductDelete model)
        {
            int ProductId = model.productId;
            int SellerProductId = model.SellerProductId;


            var sellerProduct = SellerProduct(SellerProductId);

            return "Seller product archived successfully.";
        }


        public BaseResponse<SellerProduct> SellerProduct(int SellerProductId)
        {
            BaseResponse<SellerProduct> baseResponse1 = new BaseResponse<SellerProduct>();
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            var response1 = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?id=" + SellerProductId, "GET", null);
            baseResponse1 = baseResponse1.JsonParseRecord(response1);
            if (baseResponse1.code == 200)
            {
                SellerProduct sellerProductData = (SellerProduct)baseResponse1.Data;
                try
                {
                    sellerProductData.Status = "Archived";
                    var response = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct, "PUT", sellerProductData);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                    //if (baseResponse.code != 200)
                    //{
                    //    throw new Exception("Error: Response code is not 200");
                    //}
                }
                catch (Exception ex)
                {
                    // Log the exception or handle it in some other way
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }

            return baseResponse;
        }

        public BaseResponse<SellerProduct> SellerProductBySellerId(string SellerId)
        {
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();

            try
            {
                var response = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "/Archived?SellerId=" + SellerId, "PUT", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);

            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                Console.WriteLine(ex.Message);
                throw;
            }

            return baseResponse;
        }


    }
}
