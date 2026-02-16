using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;

namespace API_Gateway.Common.products
{
    public class DeleteProduct
    {
        public string CatelogueURL = string.Empty;
        private readonly HttpContext _httpContext;
        public string UserId = string.Empty;
        private ApiHelper helper;
        public DeleteProduct(HttpContext httpContext, string Userid, string URL)
        {
            UserId = Userid;
            _httpContext = httpContext;
            CatelogueURL = URL;
            helper = new ApiHelper(_httpContext);
        }

        public string DeleteData(ProductDelete model)
        {
            int ProductId = model.productId;
            int SellerProductId = model.SellerProductId;

            BaseResponse<ProductWareHouse> WarehousebaseResponse = new BaseResponse<ProductWareHouse>();
            var response1 = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse + "?SellerProductId=" + SellerProductId, "GET", null);
            WarehousebaseResponse = WarehousebaseResponse.JsonParseList(response1);
            List<ProductWareHouse> productWarehouses = (List<ProductWareHouse>)WarehousebaseResponse.Data;
            List<int> PWIdList = productWarehouses.Select(x => x.Id).ToList();

            BaseResponse<ProductPrice> baseResponse = new BaseResponse<ProductPrice>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductId=" + SellerProductId, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ProductPrice> productPrices = (List<ProductPrice>)baseResponse.Data;
            List<int> priceIdList = productPrices.Select(x => x.Id).ToList();

            foreach (var i in PWIdList)
            {
                var pw = ProductWarehouse(i);
            }

            foreach (var i in priceIdList)
            {
                var price = ProductPrice(i);
            }

            var sellerProduct = SellerProduct(SellerProductId);

            var product = Product(ProductId);

            return "Product Deleted Successfully";
        }

        public string DeleteExistingData(ProductDelete model)
        {
            int ProductId = model.productId;
            int SellerProductId = model.SellerProductId;

            BaseResponse<ProductWareHouse> WarehousebaseResponse = new BaseResponse<ProductWareHouse>();
            var response1 = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse + "?SellerProductId=" + SellerProductId, "GET", null);
            WarehousebaseResponse = WarehousebaseResponse.JsonParseList(response1);
            List<ProductWareHouse> productWarehouses = (List<ProductWareHouse>)WarehousebaseResponse.Data;
            List<int> PWIdList = productWarehouses.Select(x => x.Id).ToList();

            BaseResponse<ProductPrice> baseResponse = new BaseResponse<ProductPrice>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductId=" + SellerProductId, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ProductPrice> productPrices = (List<ProductPrice>)baseResponse.Data;
            List<int> priceIdList = productPrices.Select(x => x.Id).ToList();

            foreach (var i in PWIdList)
            {
                var pw = ProductWarehouse(i);
            }

            foreach (var i in priceIdList)
            {
                var price = ProductPrice(i);
            }

            var sellerProduct = SellerProduct(SellerProductId);

            var product = Product(ProductId);

            return "Product Deleted Successfully";
        }

        public BaseResponse<Products> Product(int ProductId)
        {
            BaseResponse<SellerProduct> baseResponse2 = new BaseResponse<SellerProduct>();
            BaseResponse<SellerProduct> baseResponse1 = new BaseResponse<SellerProduct>();
            BaseResponse<Products> baseResponse = new BaseResponse<Products>();
            BaseResponse<Products> baseResponseMaster = new BaseResponse<Products>();
            BaseResponse<Products> baseResponseMaster1 = new BaseResponse<Products>();
            var response1 = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?productId=" + ProductId + "&isDeleted=" + false, "GET", null);
            baseResponse1 = baseResponse1.JsonParseList(response1);
            if (baseResponse1.code == 204)
            {
                List<SellerProduct> sellerProductData = (List<SellerProduct>)baseResponse1.Data;
                try
                {
                    if (sellerProductData.Count == 0)
                    {
                        var response = helper.ApiCall(CatelogueURL, EndPoints.Product + "?ID=" + ProductId, "DELETE", null);
                        baseResponse = baseResponse.JsonParseInputResponse(response);

                        if (baseResponse.code == 200)
                        {
                            var responsemaster = helper.ApiCall(CatelogueURL, EndPoints.Product + "?ID=" + ProductId + "&Isdeleted=" + true, "GET", null);
                            baseResponseMaster = baseResponseMaster.JsonParseRecord(responsemaster);
                            if (baseResponseMaster.code == 200)
                            {
                                Products ProductData = (Products)baseResponseMaster.Data;

                                var responsemaster1 = helper.ApiCall(CatelogueURL, EndPoints.Product + "?ParentId=" + ProductData.ParentId, "GET", null);
                                baseResponseMaster1 = baseResponseMaster1.JsonParseList(responsemaster1);
                                if (baseResponseMaster1.code == 204)
                                {
                                    List<Products> ProductDatalst = (List<Products>)baseResponseMaster1.Data;

                                    if (ProductDatalst.Count == 0)
                                    {
                                        var responsefinal = helper.ApiCall(CatelogueURL, EndPoints.Product + "?ID=" + ProductData.ParentId, "DELETE", null);
                                        baseResponse = baseResponse.JsonParseInputResponse(responsefinal);
                                    }
                                }
                            }
                        }

                    }
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

        public BaseResponse<SellerProduct> SellerProduct(int SellerProductId)
        {
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?id=" + SellerProductId, "DELETE", null);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return baseResponse;
        }

        public BaseResponse<ProductPrice> ProductPrice(int PriceId)
        {
            BaseResponse<ProductPrice> baseResponse = new BaseResponse<ProductPrice>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?Id=" + PriceId, "DELETE", null);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return baseResponse;
        }

        public BaseResponse<ProductWareHouse> ProductWarehouse(int ProductWarehouseId)
        {
            BaseResponse<ProductWareHouse> baseResponse = new BaseResponse<ProductWareHouse>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse + "?Id=" + ProductWarehouseId, "DELETE", null);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return baseResponse;
        }

        public BaseResponse<ProductImages> ProductImages(int ProductId)
        {
            BaseResponse<ProductImages> baseResponse = new BaseResponse<ProductImages>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductsImage + "?ProductId=" + ProductId, "DELETE", null);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return baseResponse;
        }

        //public BaseResponse<ProductVideoLink> ProductVideos(int ProductId)
        //{
        //    BaseResponse<ProductVideoLink> baseResponse = new BaseResponse<ProductVideoLink>();
        //    var response = helper.ApiCall(CatelogueURL, EndPoints.ProductsVideoLinks + "?ProductId=" + ProductId, "DELETE", null);
        //    baseResponse = baseResponse.JsonParseInputResponse(response);
        //    return baseResponse;
        //}

        public BaseResponse<ProductColorMapp> ProductColor(int ProductId)
        {
            BaseResponse<ProductColorMapp> baseResponse = new BaseResponse<ProductColorMapp>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductColorMapping + "?ProductId=" + ProductId, "DELETE", null);
            baseResponse = baseResponse.JsonParseInputResponse(response);
            return baseResponse;
        }
    }
}
