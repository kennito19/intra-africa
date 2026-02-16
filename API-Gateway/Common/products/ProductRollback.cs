using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;


namespace API_Gateway.Common.products
{
    public class ProductRollback
    {
        public string CatelogueURL = string.Empty;
        public string Token = string.Empty;
        public string UserId = string.Empty;
        private readonly HttpContext _httpContext;
        public ProductRollback(string URL, string UserID, HttpContext httpContext)
        {
            UserId = UserID;
            CatelogueURL = URL;
            _httpContext = httpContext;
        }

        public void CheckAndRollback(BaseResponse<Products> productRes,
                             BaseResponse<SellerProduct> sellerProductRes,
                             BaseResponse<ProductPrice> productPriceRes,
                             List<BaseResponse<ProductWareHouse>> productWarehouseResList,
                             //List<BaseResponse<ProductVideoLink>> videoResList,
                             List<BaseResponse<ProductImages>> imagesResList,
                             List<BaseResponse<ProductColorMapp>> colorResList)
        {
            //bool isRollbackNeed = IsRollbackNeeded(productRes, sellerProductRes, productPriceRes, productWarehouseResList, videoResList, imagesResList, colorResList);
            bool isRollbackNeed = IsRollbackNeeded(productRes, sellerProductRes, productPriceRes, productWarehouseResList,  imagesResList, colorResList);

            if (isRollbackNeed)
            {
                int productId = convertResponse(productRes.Data);
                int SellerProductId = convertResponse(sellerProductRes.Data);
                int priceMasterId = convertResponse(productPriceRes.Data);
                List<int> productWarehouseId = new List<int>();
                foreach (var pw in productWarehouseResList)
                {
                    int i = Convert.ToInt32(pw.Data);
                    productWarehouseId.Add(i);
                }

                //Here deletion occurs
                foreach (int i in productWarehouseId)
                {
                    ProductWarehouseRollback(i);
                }


                ProductPriceRollback(priceMasterId);
                SellerProductRollback(SellerProductId);
                ProductMasterRollback(productId);

            }
        }


        public bool CheckRecord<T>(BaseResponse<T> baseResponse)
        {
            if (baseResponse.code == 200 && baseResponse.Data is int)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int convertResponse(object i)
        {
            int res = Convert.ToInt32(i);
            return res;
        }

        public bool IsRollbackNeeded(BaseResponse<Products> productRes,
                             BaseResponse<SellerProduct> sellerProductRes,
                             BaseResponse<ProductPrice> productPriceRes,
                             List<BaseResponse<ProductWareHouse>> productWarehouseResList,
                             //List<BaseResponse<ProductVideoLink>> videoResList,
                             List<BaseResponse<ProductImages>> imagesResList,
                             List<BaseResponse<ProductColorMapp>> colorResList)
        {

            bool isPartialExist = true;
            if (CheckRecord<Products>(productRes) &&
                CheckRecord<SellerProduct>(sellerProductRes) &&
                CheckRecord<ProductPrice>(productPriceRes))
            {

                if (!productWarehouseResList.Where(x => CheckRecord<ProductWareHouse>(x) == false).Any())
                {
                    isPartialExist = false;
                }

                //if (!videoResList.Where(x => CheckRecord<ProductVideoLink>(x) == false).Any())
                //{
                //    isPartialExist = false;
                //}

                if (!imagesResList.Where(x => CheckRecord<ProductImages>(x) == false).Any())
                {
                    isPartialExist = false;
                }
                if (!colorResList.Where(x => CheckRecord<ProductColorMapp>(x) == false).Any())
                {
                    isPartialExist = false;
                }
            }

            return isPartialExist;
        }

        public void ProductMasterRollback(int productId)
        {
            DeleteProduct deleteProduct = new DeleteProduct(_httpContext, UserId, CatelogueURL);
            deleteProduct.Product(productId);
        }

        public void SellerProductRollback(int SellerProductId)
        {
            DeleteProduct deleteProduct = new DeleteProduct(_httpContext, UserId, CatelogueURL);
            deleteProduct.SellerProduct(SellerProductId);
        }

        public void ProductPriceRollback(int PriceMasterid)
        {
            DeleteProduct deleteProduct = new DeleteProduct(_httpContext, UserId, CatelogueURL);
            deleteProduct.ProductPrice(PriceMasterid);
        }

        public void ProductWarehouseRollback(int ProductWarehouseId)
        {
            DeleteProduct deleteProduct = new DeleteProduct(_httpContext, UserId, CatelogueURL);
            deleteProduct.ProductWarehouse(ProductWarehouseId);
        }




    }
}
