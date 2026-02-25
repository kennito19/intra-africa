using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_Gateway.Common.products
{
    public class GetProduct
    {
        private readonly IConfiguration _configuration;
        public string CatelogueURL = string.Empty;
        public string IdServerURL = string.Empty;
        public string UserURL = string.Empty;
        public string UserId = string.Empty;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        public GetProduct(IConfiguration configuration, HttpContext httpContext)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IdServerURL = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            UserURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        public BaseResponse<GetProductDTO> Get(bool? isDeleted = null)
        {
            BaseResponse<GetProductDTO> baseResponse = new BaseResponse<GetProductDTO>();
            var Products = BindProductDetailsLst(isDeleted);
            return Products;
        }

        public BaseResponse<GetProductDTO> GetExisting(bool? isDeleted = null, int? pageIndex = 0, int? pageSize = 0)
        {
            var ProductRes = BindProductDetailsLst(isDeleted, pageIndex, pageSize);
            BaseResponse<GetProductDTO> baseResponse = new BaseResponse<GetProductDTO>();
            baseResponse.code = ProductRes.code;
            baseResponse.pagination = ProductRes.pagination;
            baseResponse.Message = ProductRes.Message;
            baseResponse.pagination = ProductRes.pagination;
            var ProductList = ProductRes.Data as List<GetProductDTO>;

            foreach (var item in ProductList)
            {
                int pid = (int)item.productId;
                item.ProductImage = BindImages(pid);
                //item.ProductVideoLinks = BindVideos(pid);
                item.ProductColorMapping = BindColors(pid);
                item.ProductSpecificationsMapp = BindSpecs(pid);
                //item.SellerProducts= BindCurrentSellerProduct(pid);

            }

            baseResponse.Data = ProductList;

            return baseResponse;
        }

        public BaseResponse<GetProductDTO> GetProductlist(int? pageIndex = 0, int? pageSize = 0, int id = 0, int parentid = 0, int? categoryID = 0, string? guid = null, string? companySKUCode = null, bool isMasterProduct = false, bool getparent = false, bool getchild = true, string? searchText = null, bool? isDeleted = null)
        {
            var ProductRes = BindProductList(pageIndex, pageSize, id, parentid, categoryID, guid, companySKUCode, isMasterProduct, getparent, getchild, searchText, isDeleted);
            BaseResponse<GetProductDTO> baseResponse = new BaseResponse<GetProductDTO>();
            baseResponse.code = ProductRes.code;
            baseResponse.pagination = ProductRes.pagination;
            baseResponse.Message = ProductRes.Message;
            baseResponse.pagination = ProductRes.pagination;
            var ProductList = ProductRes.Data as List<GetProductDTO>;

            foreach (var item in ProductList)
            {
                int pid = (int)item.productId;
                item.ProductImage = BindImages(pid);
                //item.ProductVideoLinks = BindVideos(pid);
                item.ProductColorMapping = BindColors(pid);
                item.SellerProducts = BindCurrentSellerProduct(pid);
                item.ProductSpecificationsMapp = BindSpecs(pid);
            }

            baseResponse.Data = ProductList;

            return baseResponse;
        }

        public BaseResponse<AddInExistingProductList> getAddInExistingProductList(string SellerId, int? CategoryId = 0, int? BrandId = 0, string? CompanySkuCode = null, string? searchText = null, int? PageIndex = 0, int? PageSize = 0)
        {
            var ProductRes = BindExistingProductList(SellerId, CategoryId, BrandId, CompanySkuCode, searchText, PageIndex, PageSize);
            return ProductRes;
        }

        public BaseResponse<GetProductDTO> Get(int ProductId, bool? isDeleted = null, bool? isArchive = null)
        {
            var Product = BindProductDetails(ProductId, 0, 0, null, null, null, null, isDeleted, isArchive);
            return Product;
        }

        public BaseResponse<ProductDetails> GetAllProducts(int? ProductId = 0, string? sellerId = null, bool? isDeleted = null, bool? isArchive = null, int? pageIndex = 0, int? pageSize = 0)
        {
            string url = string.Empty;
            if (ProductId != null && ProductId != 0)
            {
                url += "&productId=" + ProductId;
            }
            if (!string.IsNullOrEmpty(sellerId) && sellerId != "")
            {
                url += "&sellerId=" + sellerId;
            }
            if (isDeleted != null)
            {
                url += "&isDeleted=" + isDeleted;
            }
            if (isArchive != null)
            {
                url += "&isArchive=" + isArchive;
            }

            List<GetProductDTO> productDetails = new List<GetProductDTO>();
            BaseResponse<ProductDetails> productRes = new BaseResponse<ProductDetails>();
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<SellerProduct> sellerProducts = baseResponse.Data as List<SellerProduct>;
            //if (isArchive != null && isArchive != false && sellerProducts.Count>0)
            //{
            //    BaseResponse<SellerListModel> sellerBaseResponse = new BaseResponse<SellerListModel>();
            //    var Seller_response = helper.ApiCall(IdServerURL, EndPoints.SellerList + "?pageIndex=0&pageSize=0&status=active", "GET", null);
            //    sellerBaseResponse = sellerBaseResponse.JsonParseList(Seller_response);
            //    List<SellerKycList> lstSellers = new List<SellerKycList>();
            //    if (sellerBaseResponse.code == 200)
            //    {
            //        List<SellerListModel> sellerLists = sellerBaseResponse.Data as List<SellerListModel>;
            //        sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            //        if (seller != null)
            //        {
            //            lstSellers = seller.bindSellerDetails(sellerLists);
            //            //if (isArchive != null && isArchive != false)
            //            //{
            //            //    sellerProducts = sellerProducts.Where(product => !lstSellers.Any(item => item.UserID == product.SellerID)).ToList();
            //            //}
            //        }
            //    }

            //}

            foreach (var item in sellerProducts)
            {
                var pp = BindProductDetails(item.ProductID, item.Id, 0, sellerId, null, null, null, isDeleted, isArchive).Data as GetProductDTO;
                productDetails.Add(pp);
            }

            productRes.code = baseResponse.code;
            productRes.Message = baseResponse.Message;
            productRes.pagination = baseResponse.pagination;
            productRes.Data = productDetails;

            return productRes;
        }

        public BaseResponse<ProductDetails> GetSellerProducts(string? SellerId = null, int? pageIndex = 0, int? pageSize = 0, bool? isDeleted = null, bool? isArchive = null)
        {
            List<GetProductDTO> productDetails = new List<GetProductDTO>();
            BaseResponse<ProductDetails> productRes = new BaseResponse<ProductDetails>();
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?sellerId=" + SellerId + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<SellerProduct> sellerProducts = baseResponse.Data as List<SellerProduct>;
            foreach (var item in sellerProducts)
            {
                var pp = BindProductDetails(item.ProductID, item.Id, 0, SellerId, null, null, null, isDeleted, isArchive).Data as GetProductDTO;
                productDetails.Add(pp);
            }
            productRes.code = baseResponse.code;
            productRes.Message = baseResponse.Message;
            productRes.pagination = baseResponse.pagination;
            productRes.Data = productDetails;

            return productRes;
        }


        //public BaseResponse<GetProductDTO> GetCustomerProductDetails(string ProductGUID, int? sizeId = 0, string? sellerId = null, string? status = null, bool? isProductExist = null, bool? isDeleted = null, bool? isArchive = null)
        public BaseResponse<GetProductDTO> GetCustomerProductDetails(string ProductGUID, string userId)
        {
            //var Product = BindCustomerProductDetails(ProductGUID);
            var Product = BindUserProductDetails(ProductGUID, userId);
            return Product;
        }


        public BaseResponse<GetProductDTO> GetProductDetails(int ProductId, int? sellerProductId = 0, int? sizeId = 0, string? sellerId = null, bool? live = null, bool? isProductExist = null, string? status = null, bool? isDeleted = null, bool? isArchive = null)
        {
            var Product = BindProductDetails(ProductId, sellerProductId, sizeId, sellerId, live, isProductExist, status, isDeleted, isArchive);
            return Product;
        }

        public BaseResponse<ProductDetails> GetProductDetailsWithSellerId(int ProductId, string sellerId, int? sellerProductId = 0, int? sizeId = 0, bool? live = null, bool? isProductExist = null, string? status = null, bool? isDeleted = null, bool? isArchive = null)
        {
            var Product = BindProductDetailsWithSeller(ProductId, sellerId, sellerProductId, sizeId, live, isProductExist, status, isDeleted, isArchive);
            return Product;
        }

        public BaseResponse<GetProductDTO> GetExistingProductDetails(int ProductId, bool? isDeleted = null, bool? isArchive = null)
        {
            var ProductRes = BindProductDetails(ProductId, 0, 0, null, null, false, null, isDeleted, isArchive);
            var Product = ProductRes.Data as GetProductDTO;

            //Product.SellerProducts = new List<SellerProductDTO>();

            ProductRes.Data = Product;

            return ProductRes;
        }

        public JObject GetProductComparision(string SellerProductId)
        {
            BaseResponse<ProductComparision> baseResponse = new BaseResponse<ProductComparision>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "/getProductCompare?SellerProductId=" + SellerProductId, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            List<ProductComparision> productComparisions = baseResponse.Data as List<ProductComparision>;
            List<KeyValueItem> scs = new List<KeyValueItem>();

            JObject ProductSummary = new JObject();
            JObject ProductSummaryData = new JObject();
            JObject ProductSpecificationData = new JObject();
            JArray ProductSpecification = new JArray();

            int CatCount = productComparisions.GroupBy(p => p.CategoryId).Select(group => group.Key).ToList().Count();

            if (productComparisions.Count > 0 && CatCount == 1)
            {

                #region Static Containt
                scs = productComparisions.Select(obj => new KeyValueItem { SellerProductID = obj.SellerProductID, ProductGuid = obj.ProductGuid, Value = obj.ProductImage }).ToList();
                ProductSummaryData["image"] = product(scs, "image");

                scs.Clear();
                scs = productComparisions.Select(obj => new KeyValueItem { SellerProductID = obj.SellerProductID, ProductGuid = obj.ProductGuid, Value = obj.ProductName }).ToList();
                ProductSummaryData["productName"] = product(scs, "productName");

                scs.Clear();
                scs = productComparisions.Select(obj => new KeyValueItem { SellerProductID = obj.SellerProductID, Value = obj.MRP }).ToList();
                ProductSummaryData["mrp"] = product(scs, "mrp");

                scs.Clear();
                scs = productComparisions.Select(obj => new KeyValueItem { SellerProductID = obj.SellerProductID, Value = obj.SellingPrice }).ToList();
                ProductSummaryData["sellingPrice"] = product(scs, "sellingPrice");

                scs.Clear();
                scs = productComparisions.Select(obj => new KeyValueItem { SellerProductID = obj.SellerProductID, Value = obj.Discount }).ToList();
                ProductSummaryData["discount"] = product(scs, "discount");

                scs.Clear();
                scs = productComparisions.Select(obj => new KeyValueItem { SellerProductID = obj.SellerProductID, Value = obj.Highlights }).ToList();
                ProductSummaryData["highlights"] = product(scs, "highlights");

                scs.Clear();
                scs = productComparisions.Select(obj => new KeyValueItem { SellerProductID = obj.SellerProductID, Value = obj.Description }).ToList();
                ProductSummaryData["description"] = product(scs, "description");

                scs.Clear();
                scs = productComparisions.Select(obj => new KeyValueItem { SellerProductID = obj.SellerProductID, Value = obj.Color }).ToList();
                ProductSummaryData["color"] = product(scs, "color");

                scs.Clear();
                scs = productComparisions.Select(obj => new KeyValueItem { SellerProductID = obj.SellerProductID, Value = obj.ProductID }).ToList();
                ProductSummaryData["productId"] = product(scs, "productId");


                List<string> sizeArray = productComparisions.Select(obj => obj.ProductSize).ToList();

                List<ProductSize> PSize = new List<ProductSize>();
                for (int i = 0; i < sizeArray.Count; i++)
                {
                    if (!string.IsNullOrEmpty(sizeArray[i]))
                    {
                        List<ProductSize> deserializedList = JsonConvert.DeserializeObject<List<ProductSize>>(sizeArray[i].ToString());
                        PSize.AddRange(deserializedList);
                    }
                }

                scs.Clear();
                foreach (var item in productComparisions)
                {
                    scs.Add(new KeyValueItem { SellerProductID = item.SellerProductID, Value = string.Join(", ", PSize.Where(p => p.SellerProductID == item.SellerProductID).Select(a => a.Value + " " + a.SizeType).ToList()) });

                }
                ProductSummaryData["size"] = product(scs, "size");

                ProductSummary["productSummary"] = ProductSummaryData;

                #endregion

                ProductSummaryData = new JObject();

                ProductSummaryData["categoryId"] = productComparisions.Select(obj => obj.CategoryId).FirstOrDefault();

                ProductSummary["productCategory"] = ProductSummaryData;


                #region Dynamic Containt

                List<string> stringArray = productComparisions.Select(obj => obj.ProductSpec).ToList();


                List<ProductSpec> PSpec = new List<ProductSpec>();
                for (int i = 0; i < stringArray.Count; i++)
                {
                    if (stringArray[i] != null)
                    {

                        List<ProductSpec> deserializedList = JsonConvert.DeserializeObject<List<ProductSpec>>(stringArray[i].ToString());
                        PSpec.AddRange(deserializedList);
                    }
                }

                JObject ProductSpec = new JObject();
                JArray spec = new JArray();
                JArray aa = new JArray();

                List<string> specId = PSpec.GroupBy(p => p.SpecID).Select(group => group.Key).ToList();

                foreach (var item in specId)
                {
                    ProductSpecificationData["featureName"] = PSpec.Where(p => p.SpecID == item).Select(p => p.Name).FirstOrDefault().ToString();

                    List<string> specTypeId = PSpec.Where(p => p.SpecID == item).GroupBy(p => p.SpecTypeId).Select(group => group.Key).ToList();
                    foreach (var item1 in specTypeId)
                    {

                        scs.Clear();
                        spec = new JArray();
                        ProductSpec = new JObject();
                        foreach (var item2 in productComparisions)
                        {

                            string val = "-";
                            if (PSpec.Where(a => a.SpecID == item && a.SpecTypeId == item1 && a.SellerProductID == item2.SellerProductID).ToList().Count() > 0)
                            {
                                val = string.Join(", ", PSpec.Where(a => a.SpecID == item && a.SpecTypeId == item1 && a.SellerProductID == item2.SellerProductID).Select(p => p.Value).ToList());
                            }
                            scs.Add(new KeyValueItem { SellerProductID = item2.SellerProductID, Value = val });

                        }
                        spec = productSpecs(scs, PSpec.Where(p => p.SpecID == item).Select(p => p.Name).FirstOrDefault().ToString());
                        ProductSpec["specificationName"] = PSpec.Where(p => p.SpecID == item && p.SpecTypeId == item1).Select(p => p.SpecType).FirstOrDefault().ToString();
                        ProductSpec["value"] = spec;
                        aa.Add(ProductSpec);
                    }
                    ProductSpecificationData["values"] = aa;
                    ProductSpecification.Add(ProductSpecificationData);

                    spec = new JArray();
                    ProductSpec = new JObject();
                    aa = new JArray();
                    ProductSpecificationData = new JObject();
                }

                ProductSummary["productSpecification"] = ProductSpecification;

                #endregion
            }

            return ProductSummary;



            //return productComparisions;
        }

        public BaseResponse<ProductCompareBrand> BindProductCompareBrand(int CategoryId)
        {

            BaseResponse<ProductCompareBrand> baseResponse = new BaseResponse<ProductCompareBrand>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "/getProductCompareBrand?CategoryId=" + CategoryId, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);


            List<ProductCompareBrand> details = baseResponse.Data as List<ProductCompareBrand>;



            BaseResponse<ProductCompareBrand> productbase = new BaseResponse<ProductCompareBrand>();

            productbase.code = baseResponse.code;
            productbase.pagination = baseResponse.pagination;
            productbase.Message = baseResponse.Message;
            productbase.Data = details;
            return productbase;
        }

        public BaseResponse<ProductCompareBrandProduct> BindProductCompareBrandProduct(int CategoryId, int BrandId)
        {

            BaseResponse<ProductCompareBrandProduct> baseResponse = new BaseResponse<ProductCompareBrandProduct>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "/getProductCompareBrandProduct?CategoryId=" + CategoryId + "&BrandId=" + BrandId, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);


            List<ProductCompareBrandProduct> details = baseResponse.Data as List<ProductCompareBrandProduct>;



            BaseResponse<ProductCompareBrandProduct> productbase = new BaseResponse<ProductCompareBrandProduct>();

            productbase.code = baseResponse.code;
            productbase.pagination = baseResponse.pagination;
            productbase.Message = baseResponse.Message;
            productbase.Data = details;
            return productbase;
        }


        public UserProductListWithFilterDTO GetCustomerProductListsOld(int? CategoryId = 0, string? SellerIds = null, string? BrandIds = null, string? searchTexts = null, string? SizeIds = null, string? ColorIds = null, string? productCollectionId = null, string? MinPrice = null, string? MaxPrice = null, string? MinDiscount = null, bool? available = false, int? PriceSort = 0, int? pageIndex = 1, int? pageSize = 30)
        {
            BaseResponse<UserProductListWithFilterDTO> baseResponse = new BaseResponse<UserProductListWithFilterDTO>();
            BaseResponse<UserProductList> wholeResponse = new BaseResponse<UserProductList>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?CategoryId=" + CategoryId + "&SellerIds=" + SellerIds + "&BrandIds=" + BrandIds + "&searchTexts=" + HttpUtility.UrlEncode(searchTexts) + "&SizeIds=" + SizeIds + "&ColorIds=" + ColorIds + "&productCollectionId=" + productCollectionId + "&MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&MinDiscount=" + MinDiscount + "&availableProduct=" + available + "&PriceSort=" + PriceSort + "&Mode=get&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
            wholeResponse = wholeResponse.JsonParseList(response);

            baseResponse.code = wholeResponse.code;
            baseResponse.Message = wholeResponse.Message;
            baseResponse.pagination = wholeResponse.pagination;

            char p = 'p';
            char f = 'f';
            var wlist = wholeResponse.Data as List<UserProductList>;
            List<UserProductsDTO> usrProducts = wlist.Where(x => x.flag == p).Select(x => new UserProductsDTO
            {
                Id = x.Id,
                Guid = x.Guid,
                IsMasterProduct = x.IsMasterProduct,
                ParentId = x.ParentId,
                CategoryId = x.CategoryId,
                AssiCategoryId = x.AssiCategoryId,
                ProductName = x.ProductName,
                CustomeProductName = x.CustomeProductName,
                CompanySKUCode = x.CompanySKUCode,
                Image1 = x.Image1,
                MRP = x.MRP,
                SellingPrice = x.SellingPrice,
                Discount = x.Discount,
                Quantity = x.Quantity,
                CreatedAt = x.CreatedAt,
                ModifiedAt = x.ModifiedAt,
                CategoryName = x.CategoryName,
                CategoryPathIds = x.CategoryPathIds,
                CategoryPathNames = x.CategoryPathNames,
                SellerProductId = x.SellerProductId,
                SellerId = x.SellerId,
                SellerName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                BrandId = x.BrandId,
                BrandName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? x.ExtraDetails : null,
                TotalQty = x.TotalQty,
                Status = x.Status,
                Live = x.Live,
                TotalVariant = x.TotalVariant
            }).ToList();


            var flist = wlist.Where(x => x.flag == f).ToList();

            int? totalProducts = flist.Where(x => x.F_ProductCount != null).Any() ? wlist.Where(x => x.flag == f && x.F_ProductCount != null).Select(x => x.F_ProductCount).FirstOrDefault() : 0;

            decimal? MinSellingPrice = flist.Where(x => x.MinSellingPrice != null).Select(x => x.MinSellingPrice).FirstOrDefault();
            decimal? MaxSellingPrice = flist.Where(x => x.MaxSellingPrice != null).Select(x => x.MaxSellingPrice).FirstOrDefault();


            List<CategoryFilterDTO> catfilter = flist.Where(x => x.flag == f && x.F_CategoryId != null).Distinct().Select(x => new CategoryFilterDTO
            {
                CategoryId = x.F_CategoryId,
                CategoryName = x.F_CategoryName,
            }).ToList();

            List<SizeFilterDTO> sizefilter = flist.Where(x => x.flag == f && x.F_SizeID != null).Distinct().Select(x => new SizeFilterDTO
            {
                SizeID = x.F_SizeID,
                Size = x.F_Size,
                Quantity = x.F_Quantity,
            }).ToList();

            List<BrandFilterDTO> brandfilter = flist.Where(x => x.F_BrandId != null).Distinct().Select(x => new BrandFilterDTO
            {
                BrandId = x.F_BrandId,
                BrandName = x.BrandName,//getBrand((int)x.F_BrandId).Name
            }).ToList();

            List<ColorFilterDTO> colorfilter = flist.Where(x => x.F_ColorID != null).Distinct().Select(x => new ColorFilterDTO
            {
                ColorCode = x.F_ColorCode,
                ColorId = x.F_ColorID,
                ColorName = x.F_ColorName
            }).ToList();


            List<FilterTypeDTO> filterTypes = flist.Where(x => x.FilterTypeId != null && x.FilterValueId != null)
                .GroupBy(x => new { x.FilterTypeId, x.FilterTypeName })
                    .Select(x => new FilterTypeDTO
                    {
                        FilterTypeId = x.Key.FilterTypeId,
                        FilterTypeName = x.Key.FilterTypeName,
                        FilterValues = x.Select(r => new FilterValueDTO
                        {
                            FilterValueId = r.FilterValueId,
                            FilterValueName = r.FilterValueName
                        }).ToList()
                    }).ToList();






            UserProductFilterDTO filter = new UserProductFilterDTO()
            {
                product_count = totalProducts,
                MinSellingPrice = MinSellingPrice,
                MaxSellingPrice = MaxSellingPrice,
                category_filter = catfilter,
                color_filter = colorfilter,
                size_filter = sizefilter,
                brand_filter = brandfilter,
                filter_types = filterTypes
            };

            UserProductListWithFilterDTO usrprops = new UserProductListWithFilterDTO()
            {
                Products = usrProducts,
                filterList = filter
            };


            return usrprops;
        }

        public BaseResponse<UserProductListWithFilterDTO> GetCustomerProductLists(int? CategoryId = 0, string? SellerIds = null, string? BrandIds = null, string? searchTexts = null, string? SizeIds = null, string? ColorIds = null, string? productCollectionId = null, string? MinPrice = null, string? MaxPrice = null, string? MinDiscount = null, bool? available = false, int? PriceSort = 0, string? SpecTypeValueIds = null, int? pageIndex = 1, int? pageSize = 30, string? userId = null)
        {
            BaseResponse<UserProductListWithFilterDTO> baseResponse = new BaseResponse<UserProductListWithFilterDTO>();

            string SpecTypeIds = string.Empty;
            if (string.IsNullOrWhiteSpace(SpecTypeValueIds) == false)
            {
                List<string> lstSpecIds = SpecTypeValueIds.Split('|').ToList();
                int totalFilterRegion = lstSpecIds.Count;
                for (int i = 0; i < lstSpecIds.Count; i++)
                {
                    string currIds = lstSpecIds[i];
                    SpecTypeIds += "stvm.SpecValueId in (" + currIds + ")";

                    if ((i + 1) != totalFilterRegion)
                    {
                        SpecTypeIds += " OR ";
                    }
                }

                SpecTypeIds += " group by stvm.ProductID having count(stvm.ProductID) >= " + totalFilterRegion;
            }

            char p = 'p';
            char f = 'f';

            #region get all filter

            string filterBrandIds = "";
            string filterSize = "";
            string filterColor = "";

            BaseResponse<UserProductList> baseResponseFilterProduct = new BaseResponse<UserProductList>();
            var responseFilterProduct = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?CategoryId=" + CategoryId + "&searchTexts=" + HttpUtility.UrlEncode(searchTexts) + "&MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&availableProduct=" + available + "&PriceSort=" + PriceSort + "&Mode=get&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
            baseResponseFilterProduct = baseResponseFilterProduct.JsonParseList(responseFilterProduct);

            List<UserProductList> listFilterProduct = baseResponseFilterProduct.Data as List<UserProductList>;

            //var flist = listFilterProduct.Where(x => x.flag == f).ToList();

            List<CategoryFilterDTO> catfilter = listFilterProduct.Where(x => x.flag == f && x.F_CategoryId != null).Distinct().Select(x => new CategoryFilterDTO
            {
                CategoryId = x.F_CategoryId,
                CategoryName = x.F_CategoryName,
            }).ToList();

            List<BrandFilterDTO> brandfilter = listFilterProduct.Where(x => x.flag == f && x.F_BrandId != null).Distinct().Select(x => new BrandFilterDTO
            {
                BrandId = x.F_BrandId,
                BrandName = x.F_BrandName,//getBrand((int)x.F_BrandId).Name
            }).ToList();

            List<SizeFilterDTO> sizefilter = listFilterProduct.Where(x => x.flag == f && x.F_SizeID != null).Distinct().Select(x => new SizeFilterDTO
            {
                SizeID = x.F_SizeID,
                Size = x.F_Size,
                Quantity = x.F_Quantity,
            }).ToList();

            List<ColorFilterDTO> colorfilter = listFilterProduct.Where(x => x.flag == f && x.F_ColorID != null).Distinct().Select(x => new ColorFilterDTO
            {
                ColorCode = x.F_ColorCode,
                ColorId = x.F_ColorID,
                ColorName = x.F_ColorName
            }).ToList();

            List<FilterTypeDTO> filterTypes = listFilterProduct.Where(x => x.flag == f && x.FilterTypeId != null && x.FilterValueId != null)
                .GroupBy(x => new { x.FilterTypeId, x.FilterTypeName })
                    .Select(x => new FilterTypeDTO
                    {
                        FilterTypeId = x.Key.FilterTypeId,
                        FilterTypeName = x.Key.FilterTypeName,
                        FilterValues = x.Select(r => new FilterValueDTO
                        {
                            FilterValueId = r.FilterValueId,
                            FilterValueName = r.FilterValueName
                        }).ToList()
                    }).ToList();

            #endregion

            #region update filter based on Brand

            if (!string.IsNullOrEmpty(BrandIds))
            {
                responseFilterProduct = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?CategoryId=" + CategoryId + "&BrandIds=" + BrandIds + "&searchTexts=" + HttpUtility.UrlEncode(searchTexts) + "&MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&availableProduct=" + available + "&PriceSort=" + PriceSort + "&Mode=get&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
                baseResponseFilterProduct = baseResponseFilterProduct.JsonParseList(responseFilterProduct);

                listFilterProduct = baseResponseFilterProduct.Data as List<UserProductList>;

                sizefilter = listFilterProduct.Where(x => x.flag == f && x.F_SizeID != null).Distinct().Select(x => new SizeFilterDTO
                {
                    SizeID = x.F_SizeID,
                    Size = x.F_Size,
                    Quantity = x.F_Quantity,
                }).ToList();

                colorfilter = listFilterProduct.Where(x => x.flag == f && x.F_ColorID != null).Distinct().Select(x => new ColorFilterDTO
                {
                    ColorCode = x.F_ColorCode,
                    ColorId = x.F_ColorID,
                    ColorName = x.F_ColorName
                }).ToList();

                filterTypes = listFilterProduct.Where(x => x.flag == f && x.FilterTypeId != null && x.FilterValueId != null)
                .GroupBy(x => new { x.FilterTypeId, x.FilterTypeName })
                    .Select(x => new FilterTypeDTO
                    {
                        FilterTypeId = x.Key.FilterTypeId,
                        FilterTypeName = x.Key.FilterTypeName,
                        FilterValues = x.Select(r => new FilterValueDTO
                        {
                            FilterValueId = r.FilterValueId,
                            FilterValueName = r.FilterValueName
                        }).ToList()
                    }).ToList();
            }

            #endregion

            #region update filter based on Color

            if (!string.IsNullOrEmpty(ColorIds))
            {
                if (!string.IsNullOrEmpty(BrandIds))
                {
                    filterBrandIds = BrandIds;
                }
                responseFilterProduct = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?CategoryId=" + CategoryId + "&BrandIds=" + filterBrandIds + "&ColorIds=" + ColorIds + "&searchTexts=" + HttpUtility.UrlEncode(searchTexts) + "&MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&availableProduct=" + available + "&PriceSort=" + PriceSort + "&Mode=get&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
                baseResponseFilterProduct = baseResponseFilterProduct.JsonParseList(responseFilterProduct);

                listFilterProduct = baseResponseFilterProduct.Data as List<UserProductList>;

                sizefilter = listFilterProduct.Where(x => x.flag == f && x.F_SizeID != null).Distinct().Select(x => new SizeFilterDTO
                {
                    SizeID = x.F_SizeID,
                    Size = x.F_Size,
                    Quantity = x.F_Quantity,
                }).ToList();

                filterTypes = listFilterProduct.Where(x => x.flag == f && x.FilterTypeId != null && x.FilterValueId != null)
                .GroupBy(x => new { x.FilterTypeId, x.FilterTypeName })
                    .Select(x => new FilterTypeDTO
                    {
                        FilterTypeId = x.Key.FilterTypeId,
                        FilterTypeName = x.Key.FilterTypeName,
                        FilterValues = x.Select(r => new FilterValueDTO
                        {
                            FilterValueId = r.FilterValueId,
                            FilterValueName = r.FilterValueName
                        }).ToList()
                    }).ToList();
            }

            #endregion

            #region update filter based on Size

            if (!string.IsNullOrEmpty(SizeIds))
            {
                if (!string.IsNullOrEmpty(BrandIds))
                {
                    filterBrandIds = BrandIds;
                }
                responseFilterProduct = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?CategoryId=" + CategoryId + "&BrandIds=" + filterBrandIds + "&SizeIds=" + SizeIds + "&searchTexts=" + HttpUtility.UrlEncode(searchTexts) + "&MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&availableProduct=" + available + "&PriceSort=" + PriceSort + "&Mode=get&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
                baseResponseFilterProduct = baseResponseFilterProduct.JsonParseList(responseFilterProduct);

                listFilterProduct = baseResponseFilterProduct.Data as List<UserProductList>;

                colorfilter = listFilterProduct.Where(x => x.flag == f && x.F_ColorID != null).Distinct().Select(x => new ColorFilterDTO
                {
                    ColorCode = x.F_ColorCode,
                    ColorId = x.F_ColorID,
                    ColorName = x.F_ColorName
                }).ToList();

                filterTypes = listFilterProduct.Where(x => x.flag == f && x.FilterTypeId != null && x.FilterValueId != null)
                .GroupBy(x => new { x.FilterTypeId, x.FilterTypeName })
                    .Select(x => new FilterTypeDTO
                    {
                        FilterTypeId = x.Key.FilterTypeId,
                        FilterTypeName = x.Key.FilterTypeName,
                        FilterValues = x.Select(r => new FilterValueDTO
                        {
                            FilterValueId = r.FilterValueId,
                            FilterValueName = r.FilterValueName
                        }).ToList()
                    }).ToList();
            }

            #endregion


            #region Rebinding filters

            List<int> brandIdsList = BrandIds.Split(',').Select(int.Parse).ToList();
            brandIdsList = brandIdsList.Where(id => brandfilter.Any(brand => brand.BrandId == id)).ToList();
            BrandIds = string.Join(",", brandIdsList);

            List<int> colorIdsList = ColorIds.Split(',').Select(int.Parse).ToList();
            colorIdsList = colorIdsList.Where(id => colorfilter.Any(color => color.ColorId == id)).ToList();
            ColorIds = string.Join(",", colorIdsList);

            List<int> sizeIdsList = SizeIds.Split(',').Select(int.Parse).ToList();
            sizeIdsList = sizeIdsList.Where(id => sizefilter.Any(size => size.SizeID == id)).ToList();
            SizeIds = string.Join(",", sizeIdsList);

            #endregion

            #region Wishlist

            BaseResponse<Wishlist> baseResponseWishlist = new BaseResponse<Wishlist>();
            var responseWish = helper.ApiCall(UserURL, EndPoints.Wishlist + "?UserID=" + userId + "&pageIndex=0&pageSize=0", "GET", null);
            baseResponseWishlist = baseResponseWishlist.JsonParseList(responseWish);
            List<Wishlist> listWishlist = baseResponseWishlist.Data as List<Wishlist>;

            #endregion

            #region get products

            BaseResponse<UserProductList> wholeResponse = new BaseResponse<UserProductList>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?CategoryId=" + CategoryId + "&SellerIds=" + SellerIds + "&BrandIds=" + BrandIds + "&searchTexts=" + HttpUtility.UrlEncode(searchTexts) + "&SizeIds=" + SizeIds + "&ColorIds=" + ColorIds + "&productCollectionId=" + productCollectionId + "&MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&MinDiscount=" + MinDiscount + "&availableProduct=" + available + "&PriceSort=" + PriceSort + "&SpecTypeIds=" + SpecTypeIds + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
            wholeResponse = wholeResponse.JsonParseList(response);

            baseResponse.code = wholeResponse.code;
            baseResponse.Message = wholeResponse.Message;
            baseResponse.pagination = wholeResponse.pagination;

            var listResult = wholeResponse.Data as List<UserProductList>;

            List<UserProductsDTO> usrProducts = listResult.Where(x => x.flag == p).Select(x => new UserProductsDTO
            {
                Id = x.Id,
                Guid = x.Guid,
                IsMasterProduct = x.IsMasterProduct,
                ParentId = x.ParentId,
                CategoryId = x.CategoryId,
                AssiCategoryId = x.AssiCategoryId,
                ProductName = x.ProductName,
                CustomeProductName = x.CustomeProductName,
                CompanySKUCode = x.CompanySKUCode,
                Image1 = x.Image1,
                MRP = x.MRP,
                SellingPrice = x.SellingPrice,
                Discount = x.Discount,
                Quantity = x.Quantity,
                CreatedAt = x.CreatedAt,
                ModifiedAt = x.ModifiedAt,
                CategoryName = x.CategoryName,
                CategoryPathIds = x.CategoryPathIds,
                CategoryPathNames = x.CategoryPathNames,
                SellerProductId = x.SellerProductId,
                SellerId = x.SellerId,
                SellerName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                BrandId = x.BrandId,
                BrandName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? x.ExtraDetails : null,
                TotalQty = x.TotalQty,
                Status = x.Status,
                Live = x.Live,
                IsWishlistProduct = listWishlist.Where(q => q.ProductId == x.Guid).ToList().Count > 0 ? true : false,
                TotalVariant = x.TotalVariant
            }).ToList();

            #endregion

            int? totalProducts = listResult.Where(x => x.F_ProductCount != null).Any() ? listResult.Where(x => x.flag == f && x.F_ProductCount != null).Select(x => x.F_ProductCount).FirstOrDefault() : 0;

            decimal? MinSellingPrice = listResult.Where(x => x.flag == f && x.MinSellingPrice != null).Select(x => x.MinSellingPrice).FirstOrDefault();
            decimal? MaxSellingPrice = listResult.Where(x => x.flag == f && x.MaxSellingPrice != null).Select(x => x.MaxSellingPrice).FirstOrDefault();

            decimal? MinDisc = listResult.Where(x => x.flag == f && x.Discount > 0).Distinct().OrderBy(d => d.Discount).Select(x => x.Discount).ToList().FirstOrDefault();
            decimal? MaxDisc = listResult.Where(x => x.flag == f && x.Discount > 0).Distinct().OrderByDescending(d => d.Discount).Select(x => x.Discount).ToList().FirstOrDefault();

            UserProductFilterDTO filter = new UserProductFilterDTO()
            {
                product_count = totalProducts,
                MinSellingPrice = MinSellingPrice,
                MaxSellingPrice = MaxSellingPrice,
                MinDiscount = MinDisc,
                MaxDiscount = MaxDisc,
                category_filter = catfilter,
                color_filter = colorfilter,
                size_filter = sizefilter,
                brand_filter = brandfilter,
                filter_types = filterTypes
            };

            UserProductListWithFilterDTO usrprops = new UserProductListWithFilterDTO()
            {
                Products = usrProducts,
                filterList = filter
            };

            baseResponse.Data = usrprops;

            return baseResponse;
        }


        public BaseResponse<GetProductDTO> BindProductDetails(int ProductId, int? sellerProductId = 0, int? sizeId = 0, string? sellerId = null, bool? live = null, bool? isProductExist = null, string? status = null, bool? isDeleted = null, bool? isArchive = null)
        {
            BaseResponse<Products> baseResponse = new BaseResponse<Products>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?Id=" + ProductId + "&isProductExist=" + isProductExist, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(GetResponse);

            GetProductDTO product = new GetProductDTO();
            Products details = baseResponse.Data as Products;

            product.productId = details.Id;
            product.productGuid = details.Guid;
            product.CategoryId = details.CategoryId ?? 0;
            product.CategoryName = details.CategoryName;
            product.CategoryPathName = details.CategoryPathNames;

            product.ParentId = details.ParentId;
            product.AssiCategoryId = details.AssiCategoryId ?? 0;
            product.HSNCodeId = details.HSNCodeId ?? 0;
            product.CompanySKUCode = details.CompanySKUCode;
            product.ProductName = details.ProductName;
            product.CustomeProductName = details.CustomeProductName;
            product.TaxValueId = details.TaxValueId ?? 0;

            product.Description = details.Description;
            product.Highlights = details.Highlights;
            product.Keywords = details.Keywords;
            product.ProductLength = details.ProductLength;
            product.ProductBreadth = details.ProductBreadth;
            product.ProductHeight = details.ProductHeight;
            product.ProductWeight = details.ProductWeight ?? 0;
            List<SellerProductDTO> SellerData = BindSellerProduct(ProductId, sellerProductId, sizeId, sellerId, live, isProductExist, status, isDeleted, isArchive);
            List<SellerProductDTO> _SellerData = new List<SellerProductDTO>();
            if (SellerData.Count() > 0)
            {
                product.SellerProducts = SellerData;
                product.BrandID = product.SellerProducts.FirstOrDefault().BrandID;
                product.BrandName = product.SellerProducts.FirstOrDefault().BrandName;
                product.WeightSlab = product.SellerProducts.FirstOrDefault().WeightSlabName.ToString();

            }
            else
            {
                product.SellerProducts = _SellerData;
                product.BrandID = 0;
                product.WeightSlab = null;
            }
            //product.BrandName = product.SellerProducts.FirstOrDefault().;
            product.ProductColorMapping = BindColors(ProductId);
            product.ProductImage = BindImages(ProductId);
            //product.ProductVideoLinks = BindVideos(ProductId);
            product.ProductSpecificationsMapp = BindSpecs(ProductId);
            product.ReturnPolicy = BindReturnPolicy(product.CategoryId);
            product.HSNCode = details.HSNCode;
            JObject taxRateObject = JObject.Parse(details.TaxValue);
            product.TaxRate = (string)taxRateObject["IGST"];
            product.TaxValue = details.TaxValue;
            product.WeightSlab = product.SellerProducts.FirstOrDefault().WeightSlabName.ToString();
            var sellerdata = getsellerDetails(product.SellerProducts.FirstOrDefault().SellerID);
            //product.ShipmentBy = sellerdata.ShipmentBy;
            //product.ShipmentPaidBy = sellerdata.ShipmentChargesPaidByName;

            //var taxdata = getTax(details.HSNCodeId, details.TaxValueId);


            BaseResponse<GetProductDTO> productbase = new BaseResponse<GetProductDTO>();
            productbase.code = baseResponse.code;
            productbase.pagination = baseResponse.pagination;
            productbase.Message = baseResponse.Message;
            productbase.Data = product;
            return productbase;
        }

        public BaseResponse<GetProductDTO> BindAllProductDetails(int? ProductId = 0, string? sellerId = null, int? sellerProductId = 0, int? sizeId = 0, bool? live = null, bool? isProductExist = null, string? status = null, bool? isDeleted = null, bool? isArchive = null)
        {
            string url = string.Empty;
            string para = "?";
            if (ProductId != null && ProductId != 0)
            {
                para = string.IsNullOrEmpty(url) ? "?" : "&";
                url += para + "Id=" + ProductId;
            }
            if (isProductExist != null)
            {
                para = string.IsNullOrEmpty(url) ? "?" : "&";
                url += para + "isProductExist=" + isProductExist;
            }
            //if (isDeleted != null)
            //{
            //    para = string.IsNullOrEmpty(url) ? "?" : "&";
            //    url += para + "IsDeleted=" + isDeleted;
            //}

            BaseResponse<Products> baseResponse = new BaseResponse<Products>();
            BaseResponse<GetProductDTO> baseResponse1 = new BaseResponse<GetProductDTO>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            if (baseResponse?.code != 200 || baseResponse.Data == null)
            {
                // handle the error here
                baseResponse1.Data = new List<GetProductDTO>();
                return baseResponse1;
            }

            var products = baseResponse.Data as List<Products>;
            if (products == null)
            {
                // handle the error here
                baseResponse1.Data = new List<GetProductDTO>();
                return baseResponse1;
            }
            baseResponse1.code = baseResponse.code;
            baseResponse1.Message = baseResponse.Message;
            baseResponse1.pagination = baseResponse.pagination;
            baseResponse1.Data = products.Select(details => new GetProductDTO
            {
                productId = details.Id,
                productGuid = details.Guid,
                CategoryId = details.CategoryId ?? 0,
                ParentId = details.ParentId,
                AssiCategoryId = details.AssiCategoryId ?? 0,
                HSNCodeId = details.HSNCodeId ?? 0,
                CompanySKUCode = details.CompanySKUCode,
                ProductName = details.ProductName,
                CustomeProductName = details.CustomeProductName,
                TaxValueId = details.TaxValueId ?? 0,
                Description = details.Description,
                Highlights = details.Highlights,
                Keywords = details.Keywords,
                ProductLength = details.ProductLength,
                ProductBreadth = details.ProductBreadth,
                ProductHeight = details.ProductHeight,
                ProductWeight = details.ProductWeight ?? 0,
                CategoryName = details.CategoryName,
                CategoryPathName = details.CategoryPathNames,
                SellerProducts = BindSellerProduct(details.Id, sellerProductId, sizeId, sellerId, live, isProductExist, status, isDeleted, isArchive),
                ProductImage = BindImages(details.Id),
                ProductColorMapping = BindColors(details.Id),
                //ProductVideoLinks = BindVideos(details.Id),
                ProductSpecificationsMapp = BindSpecs(details.Id),
                ReturnPolicy = BindReturnPolicy(details.CategoryId ?? 0)
            }).ToList();

            return baseResponse1;

        }

        public BaseResponse<ProductDetails> BindProductDetailsWithSeller(int ProductId, string sellerId, int? sellerProductId = 0, int? sizeId = 0, bool? live = null, bool? isProductExist = null, string? status = null, bool? isDeleted = null, bool? isArchive = null)
        {
            BaseResponse<Products> baseResponse = new BaseResponse<Products>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?Id=" + ProductId + "&isProductExist=" + isProductExist, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(GetResponse);

            ProductDetails product = new ProductDetails();
            Products details = baseResponse.Data as Products;

            product.productId = details.Id;
            product.productGuid = details.Guid;
            product.CategoryId = details.CategoryId ?? 0;
            product.CategoryName = details.CategoryName;
            product.CategoryPathName = details.CategoryPathNames;

            product.ParentId = details.ParentId;
            product.AssiCategoryId = details.AssiCategoryId ?? 0;
            product.HSNCodeId = details.HSNCodeId ?? 0;
            product.CompanySKUCode = details.CompanySKUCode;
            product.ProductName = details.ProductName;
            product.CustomeProductName = details.CustomeProductName;
            product.TaxValueId = details.TaxValueId ?? 0;

            product.Description = details.Description;
            product.Highlights = details.Highlights;
            product.Keywords = details.Keywords;
            product.ProductLength = details.ProductLength;
            product.ProductBreadth = details.ProductBreadth;
            product.ProductHeight = details.ProductHeight;
            product.ProductWeight = details.ProductWeight ?? 0;
            product.SellerProducts = BindSingleSellerProduct(ProductId, sellerId, sellerProductId, sizeId, live, isProductExist, status, isDeleted, isArchive);
            product.BrandID = product.SellerProducts != null ? product.SellerProducts.BrandID : 0;
            product.BrandName = product.SellerProducts != null ? !string.IsNullOrEmpty(product.SellerProducts.ExtraDetails) ? JObject.Parse(product.SellerProducts.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? product.SellerProducts.ExtraDetails : null : null;
            product.ProductColorMapping = BindColors(ProductId);
            product.ProductImage = BindImages(ProductId);
            //product.ProductVideoLinks = BindVideos(ProductId);
            product.ReturnPolicy = BindReturnPolicy(product.CategoryId);
            product.ProductSpecificationsMapp = BindSpecs(ProductId);
            BaseResponse<ProductDetails> productbase = new BaseResponse<ProductDetails>();
            productbase.code = baseResponse.code;
            productbase.pagination = baseResponse.pagination;
            productbase.Message = baseResponse.Message;
            productbase.Data = product;
            return productbase;
        }


        public BaseResponse<GetProductDTO> BindCustomerProductDetails(string ProductGUID, int? sizeId = 0, string? sellerId = null, string? status = null, bool? isProductExist = null, bool? isDeleted = null, bool? isArchive = null)
        {
            BaseResponse<Products> baseResponse = new BaseResponse<Products>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?guid=" + ProductGUID, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(GetResponse);

            GetProductDTO product = new GetProductDTO();
            Products details = baseResponse.Data as Products;
            //product.SellerProducts = BindCustomerSideSellerProduct(details.Id, sizeId, sellerId, status, isProductExist,isDeleted, isArchive);

            List<SellerProductDTO> SellerData = BindCustomerSideSellerProduct(details.Id, sizeId, sellerId, status, isProductExist, isDeleted, isArchive);
            List<SellerProductDTO> _SellerData = new List<SellerProductDTO>();
            if (SellerData.Count() > 0)
            {

                product.SellerProducts = SellerData;
                product.BrandID = product.SellerProducts.FirstOrDefault().BrandID;
                product.BrandName = product.SellerProducts.FirstOrDefault().BrandName;
                product.WeightSlab = product.SellerProducts.FirstOrDefault().WeightSlabName.ToString();
            }
            else
            {
                product.SellerProducts = _SellerData;
                product.BrandID = 0;
                product.WeightSlab = null;
            }

            product.productId = details.Id;
            product.productGuid = details.Guid;
            product.CategoryId = details.CategoryId ?? 0;
            product.ParentId = details.ParentId;
            product.AssiCategoryId = details.AssiCategoryId ?? 0;
            product.HSNCodeId = details.HSNCodeId ?? 0;
            product.CompanySKUCode = details.CompanySKUCode;
            product.ProductName = details.ProductName;
            product.CustomeProductName = details.CustomeProductName;
            product.TaxValueId = details.TaxValueId ?? 0;
            //product.BrandID = BindCustomerSideSellerProduct(details.Id, sizeId, sellerId, status, isProductExist,isDeleted, isArchive).FirstOrDefault().BrandID;
            //product.BrandID = product.SellerProducts.FirstOrDefault().BrandID;
            product.Description = details.Description;
            product.Highlights = details.Highlights;
            product.Keywords = details.Keywords;
            product.ProductLength = details.ProductLength;
            product.ProductBreadth = details.ProductBreadth;
            product.ProductHeight = details.ProductHeight;
            product.ProductWeight = details.ProductWeight ?? 0;
            product.CategoryName = details.CategoryName;
            product.CategoryPathName = details.CategoryPathNames;
            product.ProductSpecificationsMapp = BindSpecs(details.Id);
            product.ProductColorMapping = BindColors(details.Id);
            product.ProductImage = BindImages(details.Id);
            //product.ProductVideoLinks = BindVideos(details.Id);

            BaseResponse<GetProductDTO> productbase = new BaseResponse<GetProductDTO>();
            productbase.code = baseResponse.code;
            productbase.pagination = baseResponse.pagination;
            productbase.Message = baseResponse.Message;
            productbase.Data = product;
            return productbase;
        }


        public BaseResponse<GetProductDTO> BindProductDetailsLst(bool? isDeleted = null, int? pageIndex = 0, int? pageSize = 0)
        {
            BaseResponse<Products> baseResponse = new BaseResponse<Products>();
            BaseResponse<GetProductDTO> baseResponse1 = new BaseResponse<GetProductDTO>();
            string url = string.Empty;
            if (isDeleted != null)
            {
                url += "&IsDeleted=" + isDeleted;
            }
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize + "&getChild=true" + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            if (baseResponse?.code != 200 || baseResponse.Data == null)
            {
                // handle the error here
                baseResponse1.Data = new List<GetProductDTO>();
                return baseResponse1;
            }

            var products = baseResponse.Data as List<Products>;
            if (products == null)
            {
                // handle the error here
                baseResponse1.Data = new List<GetProductDTO>();
                return baseResponse1;
            }
            baseResponse1.code = baseResponse.code;
            baseResponse1.Message = baseResponse.Message;
            baseResponse1.pagination = baseResponse.pagination;
            baseResponse1.Data = products.Select(details => new GetProductDTO
            {
                productId = details.Id,
                productGuid = details.Guid,
                CategoryId = details.CategoryId ?? 0,
                ParentId = details.ParentId,
                AssiCategoryId = details.AssiCategoryId ?? 0,
                HSNCodeId = details.HSNCodeId ?? 0,
                CompanySKUCode = details.CompanySKUCode,
                ProductName = details.ProductName,
                CustomeProductName = details.CustomeProductName,
                TaxValueId = details.TaxValueId ?? 0,
                Description = details.Description,
                Highlights = details.Highlights,
                Keywords = details.Keywords,
                ProductLength = details.ProductLength,
                ProductBreadth = details.ProductBreadth,
                ProductHeight = details.ProductHeight,
                ProductWeight = details.ProductWeight ?? 0,
                CategoryName = details.CategoryName,
                CategoryPathName = details.CategoryPathNames,
                //SellerProducts = BindCurrentSellerProduct(details.Id),
                //ProductImage = BindImages(details.Id),
                //ProductColorMapping = BindColors(details.Id),
                //ProductVideoLinks = BindVideos(details.Id),
            })
                .ToList();

            return baseResponse1;
        }

        public BaseResponse<GetProductDTO> BindProductList(int? pageIndex = 0, int? pageSize = 0, int id = 0, int parentid = 0, int? categoryID = 0, string? guid = null, string? companySKUCode = null, bool isMasterProduct = false, bool getparent = false, bool getchild = true, string? searchText = null, bool? isDeleted = null)
        {
            string url = string.Empty;
            if (id > 0)
            {
                url += "&ID=" + id;
            }
            if (parentid > 0)
            {
                url += "&ParentId=" + parentid;
            }
            if (categoryID > 0)
            {
                url += "&CategoryID=" + categoryID;
            }
            if (!string.IsNullOrEmpty(guid))
            {
                url += "&Guid=" + guid;
            }
            if (!string.IsNullOrEmpty(companySKUCode))
            {
                url += "&CompanySKUCode=" + companySKUCode;
            }
            if (isMasterProduct)
            {
                url += "&IsMasterProduct=" + isMasterProduct;
            }
            if (getparent)
            {
                url += "&Getparent=" + getparent;
            }
            if (getchild)
            {
                url += "&Getchild=" + getchild;
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                url += "&searchText=" + HttpUtility.UrlEncode(searchText);
            }
            if (isDeleted != null)
            {
                url += "&IsDeleted=" + isDeleted;
            }
            BaseResponse<Products> baseResponse = new BaseResponse<Products>();
            BaseResponse<GetProductDTO> baseResponse1 = new BaseResponse<GetProductDTO>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?pageIndex=" + pageIndex + "&pageSize=" + pageSize + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            if (baseResponse?.code != 200 || baseResponse.Data == null)
            {
                // handle the error here
                baseResponse1.Data = new List<GetProductDTO>();
                return baseResponse1;
            }

            var products = baseResponse.Data as List<Products>;
            if (products == null)
            {
                // handle the error here
                baseResponse1.Data = new List<GetProductDTO>();
                return baseResponse1;
            }
            baseResponse1.code = baseResponse.code;
            baseResponse1.Message = baseResponse.Message;
            baseResponse1.pagination = baseResponse.pagination;
            baseResponse1.Data = products.Select(details => new GetProductDTO
            {
                productId = details.Id,
                productGuid = details.Guid,
                CategoryId = details.CategoryId ?? 0,
                ParentId = details.ParentId,
                AssiCategoryId = details.AssiCategoryId ?? 0,
                HSNCodeId = details.HSNCodeId ?? 0,
                CompanySKUCode = details.CompanySKUCode,
                ProductName = details.ProductName,
                CustomeProductName = details.CustomeProductName,
                TaxValueId = details.TaxValueId ?? 0,
                Description = details.Description,
                Highlights = details.Highlights,
                Keywords = details.Keywords,
                ProductLength = details.ProductLength,
                ProductBreadth = details.ProductBreadth,
                ProductHeight = details.ProductHeight,
                ProductWeight = details.ProductWeight ?? 0,
                CategoryName = details.CategoryName,
                CategoryPathName = details.CategoryPathNames,
                //SellerProducts = BindCurrentSellerProduct(details.Id),
                //ProductImage = BindImages(details.Id),
                //ProductColorMapping = BindColors(details.Id),
                //ProductVideoLinks = BindVideos(details.Id),
            })
                .ToList();

            return baseResponse1;
        }

        public List<SellerProductDTO> BindSellerProduct(int ProductId, int? sellerProductId = 0, int? sizeId = 0, string? sellerId = null, bool? live = null, bool? isProductExist = null, string? status = null, bool? isDeleted = null, bool? isArchive = null)
        {
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            var GetResponse = new HttpResponseMessage();
            string url = live != null ? "&live=" + live : "";
            if (isDeleted != null)
            {
                url += "&isDeleted=" + isDeleted;
            }
            if (isArchive != null)
            {
                url += "&isArchive=" + isArchive;
            }
            string isexist = isProductExist != null ? "&isProductExist=" + isProductExist : "";
            string pstaus = status != null ? "&Status=" + status : "";
            if ((sellerProductId == null || sellerProductId == 0) && (sellerId == null || sellerId == ""))
            {
                url = url + isexist + pstaus;
                GetResponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?productId=" + ProductId + url, "GET", null);
                baseResponse = baseResponse.JsonParseList(GetResponse);
            }
            else if ((sellerProductId != null || sellerProductId != 0) && (sellerId == null || sellerId == ""))
            {
                url = url + pstaus;
                GetResponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?id=" + sellerProductId + url, "GET", null);
                baseResponse = baseResponse.JsonParseList(GetResponse);
            }
            else
            {
                url = url + isexist + pstaus;
                GetResponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?SellerID=" + sellerId + "&productId=" + ProductId + url, "GET", null);
                baseResponse = baseResponse.JsonParseList(GetResponse);
            }
            List<SellerProduct> Sps = baseResponse.Data as List<SellerProduct>;

            //BaseResponse<SellerListModel> sellerBaseResponse = new BaseResponse<SellerListModel>();
            //var Seller_response = helper.ApiCall(IdServerURL, EndPoints.SellerList + "?pageIndex=0&pageSize=0&status=active", "GET", null);
            //sellerBaseResponse = sellerBaseResponse.JsonParseList(Seller_response);
            //List<SellerKycList> lstSellers = new List<SellerKycList>();
            //if (sellerBaseResponse.code == 200)
            //{
            //    List<SellerListModel> sellerLists = sellerBaseResponse.Data as List<SellerListModel>;
            //    sellerKycListDetail seller = new sellerKycListDetail(_configuration,_httpContext);
            //    if (seller != null)
            //    {
            //        lstSellers = seller.bindSellerDetails(sellerLists);
            //    }
            //}

            BaseResponse<KYCDetails> KycbaseResponse = new BaseResponse<KYCDetails>();
            List<KYCDetails> kYCDetails = new List<KYCDetails>();
            string kycurl = string.Empty;
            if (!string.IsNullOrEmpty(sellerId))
            {
                kycurl = "&UserID=" + sellerId;
            }

            var Kycresponse = helper.ApiCall(UserURL, EndPoints.KYCDetails + "?pageIndex=0&pageSize=0" + kycurl, "GET", null);
            KycbaseResponse = KycbaseResponse.JsonParseList(Kycresponse);
            if (KycbaseResponse.code == 200)
            {
                kYCDetails = KycbaseResponse.Data as List<KYCDetails>;
            }

            var DTOList = Sps.Select(x => new SellerProductDTO
            {
                Id = x.Id,
                ProductID = x.ProductID,
                SellerID = x.SellerID,
                SellerName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                BrandID = x.BrandID,
                BrandName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? x.ExtraDetails : null,
                SellerSKU = x.SKUCode,
                Live = x.Live,
                Status = x.Status,
                WeightSlabId = x.WeightSlabId,
                PackingWeight = x.PackingWeight,
                PackingLength = x.PackingLength,
                PackingBreadth = x.PackingBreadth,
                PackingHeight = x.PackingHeight,
                IsExistingProduct = x.IsExistingProduct,
                IsSizeWisePriceVariant = x.IsSizeWisePriceVariant,
                MOQ = x.MOQ,
                WeightSlabName = x.WeightSlabName,
                //ShipmentBy = lstSellers.Count() > 0 ? lstSellers.Where(p => p.UserID == x.SellerID).ToList().Count()>0 ? lstSellers.Where(p => p.UserID == x.SellerID).FirstOrDefault().ShipmentBy : null : null,
                //ShipmentPaidBy = lstSellers.Count() > 0 ? lstSellers.Where(p => p.UserID == x.SellerID).ToList().Count() > 0 ? lstSellers.Where(p => p.UserID == x.SellerID).FirstOrDefault().ShipmentChargesPaidByName : null : null,
                ShipmentBy = kYCDetails.Count() > 0 ? kYCDetails.Where(p => p.UserID == x.SellerID).ToList().Count() > 0 ? kYCDetails.Where(p => p.UserID == x.SellerID).FirstOrDefault().ShipmentBy : null : null,
                ShipmentPaidBy = kYCDetails.Count() > 0 ? kYCDetails.Where(p => p.UserID == x.SellerID).ToList().Count() > 0 ? kYCDetails.Where(p => p.UserID == x.SellerID).FirstOrDefault().ShipmentChargesPaidByName : null : null,

                ProductPrices = BindProductPriceAndWarehouses(x.Id, sizeId)
            }).ToList();

            return DTOList;
        }

        public SellerProductDTO BindSingleSellerProduct(int ProductId, string? sellerId = null, int? sellerProductId = 0, int? sizeId = 0, bool? live = null, bool? isProductExist = null, string? status = null, bool? isDeleted = null, bool? isArchive = null)
        {
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            var GetResponse = new HttpResponseMessage();
            string url = live != null ? "&live=" + live : "";
            if (isDeleted != null)
            {
                url += "&isDeleted=" + isDeleted;
            }
            if (isArchive != null)
            {
                url += "&isArchive=" + isArchive;
            }
            if (sellerId != null)
            {
                url += "&SellerID=" + sellerId;
            }
            string isexist = isProductExist != null ? "&isProductExist=" + isProductExist : "";
            string pstatus = status != null ? "&Status=" + status : "";
            //if ((sellerProductId == null || sellerProductId == 0) && (sellerId == null || sellerId == ""))
            //{
            //    url = url + isexist + pstaus;
            //    GetResponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?productId=" + ProductId + url, "GET", null);
            //    baseResponse = baseResponse.JsonParseList(GetResponse);
            //}
            if ((sellerProductId != null || sellerProductId != 0) && (sellerId == null || sellerId == ""))
            {
                url = url + pstatus;
                GetResponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?id=" + sellerProductId + url, "GET", null);
                baseResponse = baseResponse.JsonParseList(GetResponse);
            }
            else
            {
                url = url + isexist + pstatus;
                GetResponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?productId=" + ProductId + url, "GET", null);
                baseResponse = baseResponse.JsonParseList(GetResponse);
            }
            List<SellerProduct> Sps = baseResponse.Data as List<SellerProduct>;

            //BaseResponse<SellerListModel> sellerBaseResponse = new BaseResponse<SellerListModel>();
            //var Seller_response = helper.ApiCall(IdServerURL, EndPoints.SellerList + "?pageIndex=0&pageSize=0&status=active", "GET", null);
            //sellerBaseResponse = sellerBaseResponse.JsonParseList(Seller_response);
            List<UserDetailsDTO> lstSellers = new List<UserDetailsDTO>();
            //if (sellerBaseResponse.code == 200)
            //{
            //    List<SellerListModel> sellerLists = sellerBaseResponse.Data as List<SellerListModel>;
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            //if (seller != null)
            //{
            lstSellers = seller.bindSellerDetails(false, "Active", null, null, null, 0, 0);
            //}
            //}

            var DTO = Sps.Select(x => new SellerProductDTO
            {
                Id = x.Id,
                ProductID = x.ProductID,
                SellerID = x.SellerID,
                //SellerName = x.ExtraDetails,
                SellerName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                BrandID = x.BrandID,
                BrandName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? x.ExtraDetails : null,
                SellerSKU = x.SKUCode,
                Live = x.Live,
                Status = x.Status,
                WeightSlabId = x.WeightSlabId,
                PackingWeight = x.PackingWeight,
                PackingLength = x.PackingLength,
                PackingBreadth = x.PackingBreadth,
                PackingHeight = x.PackingHeight,
                IsExistingProduct = x.IsExistingProduct,
                IsSizeWisePriceVariant = x.IsSizeWisePriceVariant,
                MOQ = x.MOQ,
                WeightSlabName = x.WeightSlabName,
                ShipmentBy = lstSellers.Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).ToList().Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).FirstOrDefault().ShipmentBy : null : null,
                ShipmentPaidBy = lstSellers.Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).ToList().Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).FirstOrDefault().ShipmentChargesPaidByName : null : null,


                ProductPrices = BindProductPriceAndWarehouses(x.Id, sizeId)
            }).FirstOrDefault();




            return DTO;
        }

        public List<SellerProductDTO> BindCustomerSideSellerProduct(int ProductId, int? sizeId = 0, string? sellerId = null, string? status = null, bool? isProductExist = null, bool? isDeleted = null, bool? isArchive = null)
        {
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            var GetResponse = new HttpResponseMessage();
            string pstaus = status != null ? "&Status=" + status : "";
            //&isProductExist=true
            string url = "&live=true";
            if (isDeleted != null)
            {
                url += "&isDeleted=" + isDeleted;
            }
            if (isArchive != null)
            {
                url += "&isArchive=" + isArchive;
            }
            if (isProductExist != null)
            {
                url += "&isProductExist=" + isProductExist;
            }

            GetResponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?productId=" + ProductId + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);


            List<SellerProduct> Sps = baseResponse.Data as List<SellerProduct>;

            //BaseResponse<SellerListModel> sellerBaseResponse = new BaseResponse<SellerListModel>();
            //var Seller_response = helper.ApiCall(IdServerURL, EndPoints.SellerList + "?pageIndex=0&pageSize=0&status=active", "GET", null);
            //sellerBaseResponse = sellerBaseResponse.JsonParseList(Seller_response);
            //List<SellerKycList> lstSellers = new List<SellerKycList>();
            List<UserDetailsDTO> lstSellers = new List<UserDetailsDTO>();

            //if (sellerBaseResponse.code == 200)
            //{
            //    List<SellerListModel> sellerLists = sellerBaseResponse.Data as List<SellerListModel>;
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            //if (seller != null)
            //{
            lstSellers = seller.bindSellerDetails(false, "Active", null, null, null, 0, 0);

            //    }
            //}


            List<SellerProductDTO> DTOList = new List<SellerProductDTO>();


            if (sellerId != null)
            {
                bool SellerCheck = Sps.Where(x => x.SellerID.Equals(sellerId)).Any();
                DTOList = Sps.Where(x => x.SellerID.Equals(sellerId)).Select(x => new SellerProductDTO
                {
                    Id = x.Id,
                    ProductID = x.ProductID,
                    SellerID = x.SellerID,
                    SellerName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                    BrandName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? x.ExtraDetails : null,
                    SellerSKU = x.SKUCode,
                    Live = x.Live,
                    Status = x.Status,
                    WeightSlabId = x.WeightSlabId,
                    PackingWeight = x.PackingWeight,
                    PackingLength = x.PackingLength,
                    PackingBreadth = x.PackingBreadth,
                    PackingHeight = x.PackingHeight,
                    IsExistingProduct = x.IsExistingProduct,
                    IsSizeWisePriceVariant = x.IsSizeWisePriceVariant,
                    MOQ = x.MOQ,
                    WeightSlabName = x.WeightSlabName,
                    ShipmentBy = lstSellers.Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).ToList().Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).FirstOrDefault().ShipmentBy : null : null,
                    ShipmentPaidBy = lstSellers.Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).ToList().Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).FirstOrDefault().ShipmentChargesPaidByName : null : null,

                    ProductPrices = BindProductPriceAndWarehouses(x.Id, sizeId)
                }).ToList();
            }
            else
            {
                var SellerProductIds = Sps.Select(x => x.Id).ToList();
                var ProductPriceList = new List<ProductPriceDTO>();
                foreach (var spi in SellerProductIds)
                {
                    ProductPriceList = BindCustomerProductPriceAndWarehouses(spi, sizeId);
                }

                var lowestPriceSellerPrice = ProductPriceList.OrderBy(x => x.SellingPrice).ToList();

                DTOList = Sps.Where(x => x.Id == lowestPriceSellerPrice.First().SellerProductId).Select(x => new SellerProductDTO
                {
                    Id = x.Id,
                    ProductID = x.ProductID,
                    SellerID = x.SellerID,
                    SellerName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                    BrandName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? x.ExtraDetails : null,
                    SellerSKU = x.SKUCode,
                    Live = x.Live,
                    Status = x.Status,
                    WeightSlabId = x.WeightSlabId,
                    PackingWeight = x.PackingWeight,
                    PackingLength = x.PackingLength,
                    PackingBreadth = x.PackingBreadth,
                    PackingHeight = x.PackingHeight,
                    MOQ = x.MOQ,
                    WeightSlabName = x.WeightSlabName,
                    ShipmentBy = lstSellers.Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).ToList().Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).FirstOrDefault().ShipmentBy : null : null,
                    ShipmentPaidBy = lstSellers.Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).ToList().Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).FirstOrDefault().ShipmentChargesPaidByName : null : null,


                    IsExistingProduct = x.IsExistingProduct,
                    IsSizeWisePriceVariant = x.IsSizeWisePriceVariant,
                    ProductPrices = lowestPriceSellerPrice
                }).ToList();
            }






            return DTOList;
        }


        public List<SellerProductDTO> BindCurrentSellerProduct(int ProductId, bool? isDeleted = null, bool? isArchive = null)
        {
            string url = string.Empty;
            if (isDeleted != null)
            {
                url += "&isDeleted=" + isDeleted;
            }
            if (isArchive != null)
            {
                url += "&isArchive=" + isArchive;
            }
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?productId=" + ProductId + "&isProductExist=false&live=true" + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            List<SellerProduct> Sps = baseResponse.Data as List<SellerProduct>;

            //BaseResponse<SellerListModel> sellerBaseResponse = new BaseResponse<SellerListModel>();
            //var Seller_response = helper.ApiCall(IdServerURL, EndPoints.SellerList + "?pageIndex=0&pageSize=0&status=active", "GET", null);
            //sellerBaseResponse = sellerBaseResponse.JsonParseList(Seller_response);
            //List<SellerKycList> lstSellers = new List<SellerKycList>();
            List<UserDetailsDTO> lstSellers = new List<UserDetailsDTO>();

            //if (sellerBaseResponse.code == 200)
            //{
            //    List<SellerListModel> sellerLists = sellerBaseResponse.Data as List<SellerListModel>;
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            //if (seller != null)
            //{
            lstSellers = seller.bindSellerDetails(false, "Active", null, null, null, 0, 0);

            //    }
            //}

            var DTOList = Sps.Select(x => new SellerProductDTO
            {
                Id = x.Id,
                ProductID = x.ProductID,
                BrandID = x.BrandID,
                SellerID = x.SellerID,
                SellerName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                BrandName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? x.ExtraDetails : null,
                SellerSKU = x.SKUCode,
                Live = x.Live,
                Status = x.Status,
                WeightSlabId = x.WeightSlabId,
                PackingWeight = x.PackingWeight,
                PackingLength = x.PackingLength,
                PackingBreadth = x.PackingBreadth,
                PackingHeight = x.PackingHeight,
                IsExistingProduct = x.IsExistingProduct,
                IsSizeWisePriceVariant = x.IsSizeWisePriceVariant,
                MOQ = x.MOQ,
                WeightSlabName = x.WeightSlabName,
                ShipmentBy = lstSellers.Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).ToList().Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).FirstOrDefault().ShipmentBy : null : null,
                ShipmentPaidBy = lstSellers.Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).ToList().Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).FirstOrDefault().ShipmentChargesPaidByName : null : null,

                ProductPrices = BindProductPriceAndWarehouses(x.Id)
            }).ToList();


            return DTOList;
        }

        public List<ProductPriceDTO> BindCustomerProductPriceAndWarehouses(int SellerProductId, int? sizeId = 0)
        {
            BaseResponse<ProductPrice> baseResponse = new BaseResponse<ProductPrice>();
            var GetResponse = new HttpResponseMessage();
            if (sizeId != 0)
            {
                GetResponse = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductID=" + SellerProductId + "&SizeID=" + sizeId, "GET", null);
                baseResponse = baseResponse.JsonParseList(GetResponse);
            }
            else
            {
                GetResponse = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductID=" + SellerProductId, "GET", null);
                baseResponse = baseResponse.JsonParseList(GetResponse);
            }

            baseResponse = baseResponse.JsonParseList(GetResponse);

            List<ProductPrice> productPrices = baseResponse.Data as List<ProductPrice>;
            var DTOList = productPrices.Select(x => new ProductPriceDTO
            {
                Id = x.Id,
                SellerProductId = (int)x.SellerProductID,
                MRP = x.MRP,
                SellingPrice = x.SellingPrice,
                Discount = x.Discount,
                Quantity = x.Quantity,
                SizeID = x.SizeID,
                SizeName = x.SizeName,
                ProductWarehouses = BindProductWarehouse(x.Id),
            }).OrderBy(x => x.SellingPrice).ToList();

            if (sizeId != 0)
            {
                //DTOList = DTOList.Where(x => x.SizeID == sizeId || sizeId == null).Take(1).ToList();
                DTOList = DTOList.Where(x => x.SizeID == sizeId || sizeId == null).ToList();
            }
            else
            {
                //DTOList = DTOList.Take(1).ToList();
                DTOList = DTOList.ToList();
            }


            return DTOList;
        }


        public List<ProductPriceDTO> BindProductPriceAndWarehouses(int SellerProductId, int? sizeId = 0)
        {
            BaseResponse<ProductPrice> baseResponse = new BaseResponse<ProductPrice>();
            var GetResponse = new HttpResponseMessage();
            if (sizeId != 0)
            {
                GetResponse = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductID=" + SellerProductId + "&SizeID=" + sizeId, "GET", null);
                baseResponse = baseResponse.JsonParseList(GetResponse);
            }
            else
            {
                GetResponse = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductID=" + SellerProductId, "GET", null);
                baseResponse = baseResponse.JsonParseList(GetResponse);
            }

            baseResponse = baseResponse.JsonParseList(GetResponse);

            List<ProductPrice> productPrices = baseResponse.Data as List<ProductPrice> ?? new List<ProductPrice>();

            var DTOList = productPrices.Select(x => new ProductPriceDTO
            {
                Id = x.Id,
                SellerProductId = (int)x.SellerProductID,
                MRP = x.MRP,
                SellingPrice = x.SellingPrice,
                Discount = x.Discount,
                Quantity = x.Quantity,
                MarginIn = x.MarginIn,
                MarginCost = x.MarginCost,
                MarginPercentage = x.MarginPercentage,
                SizeID = x.SizeID,
                SizeName = x.SizeName,
                SizeTypeName = x.SizeTypeName,
                ProductWarehouses = BindProductWarehouse(x.Id),
            }).ToList();



            return DTOList;
        }

        public List<ProductWarehouseDTO> BindProductWarehouse(int PriceMasterId)
        {
            BaseResponse<ProductWareHouse> baseResponse = new BaseResponse<ProductWareHouse>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse + "?sellerwiseproductpricemasterid=" + PriceMasterId + "&PageIndex=0&PageSize=0", "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            List<ProductWareHouse> productWarehouses = baseResponse.Data as List<ProductWareHouse> ?? new List<ProductWareHouse>();
            var DTOList = productWarehouses.Select(x => new ProductWarehouseDTO
            {
                Id = x.Id,
                WarehouseId = (int)x.WarehouseID,
                Quantity = x.ProductQuantity,
                WarehouseName = x.WarehouseName,
            }).ToList();

            return DTOList;
        }

        public List<ProductColorDTO> BindColors(int productId)
        {

            BaseResponse<ProductColorMapp> baseResponse = new BaseResponse<ProductColorMapp>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductColorMapping + "?ProductID=" + productId, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ProductColorMapp> tempList = baseResponse.Data as List<ProductColorMapp> ?? new List<ProductColorMapp>();
            var DTOList = tempList.Select(x => new ProductColorDTO
            {
                Id = x.Id,
                ColorId = (int)x.ColorID,
                ColorCode = x.ColorCode,
                ColorName = x.ColorName,
            }).ToList();

            return DTOList;
        }

        public List<ProductImageDTO> BindImages(int productId)
        {
            BaseResponse<ProductImages> baseResponse = new BaseResponse<ProductImages>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductsImage + "?ProductID=" + productId, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ProductImages> tempList = baseResponse.Data as List<ProductImages> ?? new List<ProductImages>();

            var DTOList = tempList.Select(x => new ProductImageDTO
            {
                Id = x.Id,
                Sequence = x.Sequence,
                Url = x.Url,
                Type = x.Type,
            }).OrderBy(p => p.Sequence).ToList();

            return DTOList;
        }


        public List<ProductSpecificationMappingDto> BindSpecs(int productId)
        {
            BaseResponse<ProductSpecificationMapping> baseResponse = new BaseResponse<ProductSpecificationMapping>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductSpecificationMapping + "?ProductID=" + productId + "&PageIndex=0&PageSize=0", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ProductSpecificationMapping> tempList = baseResponse.Data as List<ProductSpecificationMapping>;

            var DTOList = tempList.Select(x => new ProductSpecificationMappingDto
            {
                Id = x.Id,
                SpecId = x.SpecId,
                SpecTypeId = x.SpecTypeId,
                SpecValueId = x.SpecValueId,
                Value = x.Value,
                SpecificationName = x.SpecificationName,
                SpecificationTypeName = x.SpecificationTypeName,

            }).ToList();

            return DTOList;
        }

        //public List<ProductVideoLinkDTO> BindVideos(int productId)
        //{
        //    BaseResponse<ProductVideoLink> baseResponse = new BaseResponse<ProductVideoLink>();
        //    var response = helper.ApiCall(CatelogueURL, EndPoints.ProductsVideoLinks + "?ProductID=" + productId, "GET", null);
        //    baseResponse = baseResponse.JsonParseList(response);
        //    List<ProductVideoLink> tempList = baseResponse.Data as List<ProductVideoLink>;

        //    var res = tempList.Select(x => new ProductVideoLinkDTO
        //    {
        //        Id = x.Id,
        //        Link = x.Link
        //    });

        //    return res.ToList();
        //}


        //public SellerKycList getsellerKyc(string sellerId)
        //{
        //    BaseResponse<KYCDetails> baseResponse = new BaseResponse<KYCDetails>();
        //    BaseResponse<GSTInfo> baseResponse1 = new BaseResponse<GSTInfo>();
        //    BaseResponse<SellerListModel> baseResponse2 = new BaseResponse<SellerListModel>();

        //    var response2 = helper.ApiCall(IdServerURL, EndPoints.SellerById + "?ID=" + sellerId, "GET", null);
        //    baseResponse2 = baseResponse2.JsonParseRecord(response2);
        //    SellerListModel seller = baseResponse2.Data as SellerListModel;


        //    var response = helper.ApiCall(UserURL, EndPoints.KYCDetails + "?UserID=" + sellerId, "GET", null);
        //    baseResponse = baseResponse.JsonParseRecord(response);
        //    KYCDetails kycDetails = baseResponse.Data as KYCDetails;

        //    var response1 = helper.ApiCall(UserURL, EndPoints.GSTInfo + "?UserID=" + sellerId, "GET", null);
        //    baseResponse1 = baseResponse1.JsonParseRecord(response1);
        //    GSTInfo gstInfo = baseResponse1.Data as GSTInfo;


        //    // Create a new instance of SellerKycList
        //    SellerKycList sellerKycList = new SellerKycList();

        //    // Set Seller Details
        //    sellerKycList.UserID = seller.Id;
        //    sellerKycList.FullName = $"{seller.FirstName} {seller.LastName}";
        //    sellerKycList.EmailID = seller.UserName;
        //    sellerKycList.PhoneNumber = kycDetails.ContactPersonMobileNo;
        //    sellerKycList.CreatedAt = kycDetails.CreatedAt;

        //    // Set KYC Details
        //    sellerKycList.KycFor = kycDetails.KYCFor;
        //    sellerKycList.DisplayName = kycDetails.DisplayName;
        //    sellerKycList.OwnerName = kycDetails.OwnerName;
        //    sellerKycList.ContactPersonName = kycDetails.ContactPersonName;
        //    sellerKycList.ContactPersonMobilNo = kycDetails.ContactPersonMobileNo;
        //    sellerKycList.Logo = kycDetails.Logo;
        //    sellerKycList.Status = kycDetails.Status;
        //    sellerKycList.ModifiedDate = kycDetails.ModifiedAt;

        //    // Set GST Details
        //    sellerKycList.LegalName = gstInfo.LegalName;
        //    sellerKycList.TradeName = gstInfo.TradeName;

        //    return sellerKycList;
        //}

        //public BrandLibrary getBrand(int brandId)
        //{
        //    BaseResponse<BrandLibrary> brandResponse = new BaseResponse<BrandLibrary>();
        //    var response = helper.ApiCall(UserURL, EndPoints.Brand + "?Id=" + brandId, "GET", null);
        //    brandResponse = brandResponse.JsonParseRecord(response);

        //    BrandLibrary brand = new BrandLibrary();
        //    brand = brandResponse.Data as BrandLibrary;

        //    return brand;
        //}

        public List<AssignSpecValuesToCategoryLibrary> getAssignSpecValues(int assispecId)
        {
            BaseResponse<AssignSpecValuesToCategoryLibrary> assignSpecValuesBaseResponse = new BaseResponse<AssignSpecValuesToCategoryLibrary>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.AssignSpecValuesToCategory + "?PageIndex=0&PageSize=0&AssignSpecID=" + assispecId, "GET", null);
            assignSpecValuesBaseResponse = assignSpecValuesBaseResponse.JsonParseList(response);

            List<AssignSpecValuesToCategoryLibrary> lstassignSpecValues = new List<AssignSpecValuesToCategoryLibrary>();
            lstassignSpecValues = assignSpecValuesBaseResponse.Data as List<AssignSpecValuesToCategoryLibrary>;

            return lstassignSpecValues;
        }

        public ReturnPolicyDTO BindReturnPolicy(int Categoryid)
        {
            List<CategoryLibrary> categoryLists = GetCategoryWithParent(Categoryid, CatelogueURL);
            ReturnPolicyDTO returnPolicy = new ReturnPolicyDTO();
            BaseResponse<AssignReturnPolicyToCatagoryLibrary> baseResponse = new BaseResponse<AssignReturnPolicyToCatagoryLibrary>();

            if (categoryLists != null && categoryLists.Count > 0)
            {
                categoryLists = categoryLists.OrderByDescending(p => p.Id).ToList();
                foreach (var item in categoryLists)
                {
                    var response = helper.ApiCall(CatelogueURL, EndPoints.AssignReturnPolicyToCatagory + "?CategoryID=" + item.Id, "GET", null);
                    baseResponse = baseResponse.JsonParseRecord(response);
                    AssignReturnPolicyToCatagoryLibrary policy = new AssignReturnPolicyToCatagoryLibrary();
                    if (baseResponse.code == 200)
                    {
                        policy = baseResponse.Data as AssignReturnPolicyToCatagoryLibrary;
                        returnPolicy.Id = policy.Id;
                        returnPolicy.ReturnPolicyID = Convert.ToInt32(policy.ReturnPolicyID);
                        returnPolicy.ValidityDays = Convert.ToInt32(policy.ValidityDays);
                        returnPolicy.Title = policy.Title;
                        returnPolicy.Covers = policy.Covers;
                        returnPolicy.Description = policy.Description;
                        returnPolicy.ReturnPolicyName = policy.ReturnPolicy;
                        break;
                    }
                    else
                    {
                        returnPolicy = new ReturnPolicyDTO();
                    }
                }
            }

            return returnPolicy;
        }
        
        public SellerKycDetails getsellerDetails(string sellerId)
        {
            sellerKyc seller = new sellerKyc(_configuration, _httpContext);
            var sellerdetails = seller.BindSeller(sellerId);

            return sellerdetails;
        }

        public BrandLibrary getBrandDetails(int BrandId)
        {
            Brands brandDetails = new Brands(UserURL, _configuration, _httpContext);
            var brandLibrary = brandDetails.GetBrandById(BrandId).Data as BrandLibrary;

            return brandLibrary;
        }

        public AssignTaxRateToHSNCode getTax(int hsnCodeId, int TaxTypeValueID)
        {
            BaseResponse<AssignTaxRateToHSNCode> TaxbaseResponse = new BaseResponse<AssignTaxRateToHSNCode>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.AssignTaxRateToHSNCode + "?HsnCodeId=" + hsnCodeId + "&TaxValueId=" + TaxTypeValueID, "GET", null);
            TaxbaseResponse = TaxbaseResponse.JsonParseRecord(response);
            AssignTaxRateToHSNCode taxdata = new AssignTaxRateToHSNCode();
            taxdata = TaxbaseResponse.Data as AssignTaxRateToHSNCode;
            return taxdata;
        }

        public BaseResponse<MasterProductDTO> MasterProductDetails(int id)
        {
            BaseResponse<Products> baseResponse = new BaseResponse<Products>();
            BaseResponse<MasterProductDTO> baseResponse1 = new BaseResponse<MasterProductDTO>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?ID=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(GetResponse);

            if (baseResponse?.code != 200 || baseResponse.Data == null)
            {
                // handle the error here
                baseResponse1.Data = new MasterProductDTO();
                return baseResponse1;
            }

            var products = baseResponse.Data as Products;

            BaseResponse<Products> baseResponseChildProduct = new BaseResponse<Products>();
            var GetchildResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?ParentId=" + products.Id, "GET", null);
            baseResponseChildProduct = baseResponseChildProduct.JsonParseList(GetchildResponse);

            if (baseResponseChildProduct?.code != 200 || baseResponseChildProduct.Data == null)
            {
                // handle the error here
                baseResponse1.Data = new MasterProductDTO();
                return baseResponse1;
            }
            var childproductsList = baseResponseChildProduct.Data as List<Products>;
            SellerProductDTO SellerProduct = new SellerProductDTO();
            foreach (var item in childproductsList)
            {
                SellerProduct = BindSellerProduct(item.Id).FirstOrDefault();
                if (SellerProduct != null && SellerProduct.Id != null && SellerProduct.Id != 0)
                {

                    break;

                }
            }
            //var childproducts = baseResponseChildProduct.Data as Products;

            //SellerProductDTO SellerProduct = BindSellerProduct(childproducts.Id).FirstOrDefault();

            MasterProductDTO masterProduct = new MasterProductDTO();
            masterProduct.ProductId = id;
            masterProduct.ProductGuid = products.Guid;
            masterProduct.AssiSpecid = products.AssiCategoryId;
            masterProduct.CategoryId = products.CategoryId;
            masterProduct.CategoryName = products.CategoryName;
            masterProduct.CategoryPathName = products.CategoryPathNames;
            masterProduct.BrandId = SellerProduct.BrandID;
            masterProduct.BrandName = SellerProduct.BrandName;

            baseResponse1.code = baseResponse.code;
            baseResponse1.Message = baseResponse.Message;
            baseResponse1.pagination = baseResponse.pagination;
            baseResponse1.Data = masterProduct;

            return baseResponse1;
        }
        public CategoryLibrary getCategory(int categoryId)
        {
            var ResCat = helper.ApiCall(CatelogueURL, EndPoints.Category + "?Id=" + categoryId + "&Isdeleted=false", "GET", null);
            BaseResponse<CategoryLibrary> baseCat = new BaseResponse<CategoryLibrary>();
            baseCat = baseCat.JsonParseRecord(ResCat);
            CategoryLibrary category = baseCat.Data as CategoryLibrary;

            return category;
        }

        public BaseResponse<ArchiveProductList> GetArchiveProducts(int? ProductId = 0, int? ProductMasterid = 0, int? CategoryId = 0, int? AssiCategoryId = 0, string? CompanySkuCode = null, string? SellerSkuCode = null, string? SellerId = null, int? BrandId = 0, string? Guid = null, int PageIndex = 1, int PageSize = 10, string? searchText = null)
        {
            string url = string.Empty;

            if (ProductId != 0 && ProductId != null) { url += "&ProductId=" + ProductId; }
            if (ProductMasterid != 0 && ProductMasterid != null) { url += "&ProductMasterid=" + ProductMasterid; }
            if (CategoryId != 0 && CategoryId != null) { url += "&CategoryId=" + CategoryId; }
            if (AssiCategoryId != 0 && AssiCategoryId != null) { url += "&AssiCategoryId=" + AssiCategoryId; }
            if (BrandId != 0 && BrandId != null) { url += "&BrandId=" + BrandId; }
            if (!string.IsNullOrEmpty(CompanySkuCode)) { url += "&CompanySkuCode=" + CompanySkuCode; }
            if (!string.IsNullOrEmpty(SellerSkuCode)) { url += "&SellerSkuCode=" + SellerSkuCode; }
            if (!string.IsNullOrEmpty(SellerId)) { url += "&SellerId=" + SellerId; }
            if (!string.IsNullOrEmpty(Guid)) { url += "&Guid=" + Guid; }
            if (!string.IsNullOrEmpty(searchText)) { url += "&searchText=" + HttpUtility.UrlEncode(searchText); }
            BaseResponse<ArchiveProductList> baseResponse = new BaseResponse<ArchiveProductList>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "/getArchiveProductList" + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            List<ArchiveProductList> Sps = baseResponse.Data as List<ArchiveProductList> ?? new List<ArchiveProductList>();
            string? GetExtraDetailsValue(string? extraDetails, string section, string field, string? fallback = null)
            {
                if (string.IsNullOrWhiteSpace(extraDetails))
                {
                    return fallback;
                }

                try
                {
                    var parsed = JObject.Parse(extraDetails);
                    var value = parsed[section]?[field]?.ToString();
                    return string.IsNullOrWhiteSpace(value) ? fallback : value;
                }
                catch
                {
                    return fallback;
                }
            }

            var DTOList = Sps.Select(x =>
            {
                string? brandName = GetExtraDetailsValue(x.ExtraDetails, "BrandDetails", "Name", x.ExtraDetails);
                string? sellerName = GetExtraDetailsValue(x.ExtraDetails, "SellerDetails", "Display")
                    ?? GetExtraDetailsValue(x.ExtraDetails, "SellerDetails", "FullName")
                    ?? "Unknown Seller";

                return new ArchiveProductList
                {
                RowNumber = !string.IsNullOrEmpty(x.RowNumber.ToString()) ? x.RowNumber : null,
                PageCount = !string.IsNullOrEmpty(x.PageCount.ToString()) ? x.PageCount : null,
                RecordCount = !string.IsNullOrEmpty(x.RecordCount.ToString()) ? x.RecordCount : null,
                ProductId = !string.IsNullOrEmpty(x.ProductId.ToString()) ? x.ProductId : null,
                Guid = !string.IsNullOrEmpty(x.Guid) ? x.Guid : null,
                ProductMasterId = !string.IsNullOrEmpty(x.ProductMasterId.ToString()) ? x.ProductMasterId : null,
                CategoryId = !string.IsNullOrEmpty(x.CategoryId.ToString()) ? x.CategoryId : null,
                AssiCategoryId = !string.IsNullOrEmpty(x.AssiCategoryId.ToString()) ? x.AssiCategoryId : null,
                TaxValueId = !string.IsNullOrEmpty(x.TaxValueId.ToString()) ? x.TaxValueId : null,
                HSNCodeId = !string.IsNullOrEmpty(x.HSNCodeId.ToString()) ? x.HSNCodeId : null,
                ProductName = !string.IsNullOrEmpty(x.ProductName) ? x.ProductName : null,
                CustomeProductName = !string.IsNullOrEmpty(x.CustomeProductName) ? x.CustomeProductName : null,
                CompanySKUCode = !string.IsNullOrEmpty(x.CompanySKUCode) ? x.CompanySKUCode : null,
                SellerSKUCode = !string.IsNullOrEmpty(x.SellerSKUCode) ? x.SellerSKUCode : null,
                SellerProductId = !string.IsNullOrEmpty(x.SellerProductId.ToString()) ? x.SellerProductId : null,
                BrandID = !string.IsNullOrEmpty(x.BrandID.ToString()) ? x.BrandID : null,
                SellerID = !string.IsNullOrEmpty(x.SellerID) ? x.SellerID : null,
                Status = !string.IsNullOrEmpty(x.Status) ? x.Status : null,
                ExtraDetails = !string.IsNullOrEmpty(x.ExtraDetails) ? x.ExtraDetails : null,
                WeightSlabId = !string.IsNullOrEmpty(x.WeightSlabId.ToString()) ? x.WeightSlabId : null,
                PriceMasterId = !string.IsNullOrEmpty(x.PriceMasterId.ToString()) ? x.PriceMasterId : null,
                MRP = !string.IsNullOrEmpty(x.MRP) ? x.MRP : null,
                SellingPrice = !string.IsNullOrEmpty(x.SellingPrice) ? x.SellingPrice : null,
                Discount = !string.IsNullOrEmpty(x.Discount) ? x.Discount : null,
                Quantity = !string.IsNullOrEmpty(x.Quantity) ? x.Quantity : null,
                ProductImage = !string.IsNullOrEmpty(x.ProductImage) ? x.ProductImage : null,
                CategoryName = !string.IsNullOrEmpty(x.CategoryName) ? x.CategoryName : null,
                CategoryPathIds = !string.IsNullOrEmpty(x.CategoryPathIds) ? x.CategoryPathIds : null,
                CategoryPathNames = !string.IsNullOrEmpty(x.CategoryPathNames) ? x.CategoryPathNames : null,
                BrandName = !string.IsNullOrEmpty(x.ExtraDetails) ? brandName : null,
                SellerName = !string.IsNullOrEmpty(x.ExtraDetails) ? sellerName : null,
                };
            }).ToList();
            baseResponse.Data = DTOList;

            return baseResponse;
        }

        public BaseResponse<UserProductListWithFilterDTO> GetCustomerProductLists1(int? CategoryId = 0, string? SellerIds = null, string? BrandIds = null, string? searchTexts = null, string? SizeIds = null, string? ColorIds = null, string? productCollectionId = null, string? MinPrice = null, string? MaxPrice = null, string? MinDiscount = null, bool? available = false, int? PriceSort = 0, string? SpecTypeValueIds = null, int? pageIndex = 1, int? pageSize = 30, string? userId = null, string? fby = null)
        {
            BaseResponse<UserProductListWithFilterDTO> baseResponse = new BaseResponse<UserProductListWithFilterDTO>();

            string SpecTypeIds = string.Empty;
            if (string.IsNullOrWhiteSpace(SpecTypeValueIds) == false)
            {
                List<string> lstSpecIds = SpecTypeValueIds.Split('|').ToList();
                int totalFilterRegion = lstSpecIds.Count;
                for (int i = 0; i < lstSpecIds.Count; i++)
                {
                    string currIds = lstSpecIds[i];
                    SpecTypeIds += "stvm.SpecValueId in (" + currIds + ")";

                    if ((i + 1) != totalFilterRegion)
                    {
                        SpecTypeIds += " OR ";
                    }
                }

                SpecTypeIds += " group by stvm.ProductID having count(stvm.ProductID) >= " + totalFilterRegion;
            }

            char p = 'p';
            char f = 'f';

            #region Wishlist
            List<Wishlist> listWishlist = new List<Wishlist>();
            if (userId != null)
            {
                BaseResponse<Wishlist> baseResponseWishlist = new BaseResponse<Wishlist>();
                var responseWish = helper.ApiCall(UserURL, EndPoints.Wishlist + "?UserID=" + userId + "&pageIndex=0&pageSize=0", "GET", null);
                baseResponseWishlist = baseResponseWishlist.JsonParseList(responseWish);
                listWishlist = baseResponseWishlist.Data as List<Wishlist>;
            }
            #endregion

            #region get products

            BaseResponse<UserProductList> wholeResponse = new BaseResponse<UserProductList>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?CategoryId=" + CategoryId + "&SellerIds=" + SellerIds + "&BrandIds=" + BrandIds + "&searchTexts=" + HttpUtility.UrlEncode(searchTexts) + "&SizeIds=" + SizeIds + "&ColorIds=" + ColorIds + "&productCollectionId=" + productCollectionId + "&MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&MinDiscount=" + MinDiscount + "&availableProduct=" + available + "&PriceSort=" + PriceSort + "&SpecTypeIds=" + SpecTypeIds + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
            wholeResponse = wholeResponse.JsonParseList(response);

            baseResponse.code = wholeResponse.code;
            baseResponse.Message = wholeResponse.Message;
            baseResponse.pagination = wholeResponse.pagination;

            var listResult = wholeResponse.Data as List<UserProductList>;

            List<UserProductsDTO> usrProducts = listResult.Where(x => x.flag == p).Select(x => new UserProductsDTO
            {
                Id = x.Id,
                Guid = x.Guid,
                IsMasterProduct = x.IsMasterProduct,
                ParentId = x.ParentId,
                CategoryId = x.CategoryId,
                AssiCategoryId = x.AssiCategoryId,
                ProductName = x.ProductName,
                CustomeProductName = x.CustomeProductName,
                CompanySKUCode = x.CompanySKUCode,
                Image1 = x.Image1,
                MRP = x.MRP,
                SellingPrice = x.SellingPrice,
                Discount = x.Discount,
                Quantity = x.Quantity,
                CreatedAt = x.CreatedAt,
                ModifiedAt = x.ModifiedAt,
                CategoryName = x.CategoryName,
                CategoryPathIds = x.CategoryPathIds,
                CategoryPathNames = x.CategoryPathNames,
                SellerProductId = x.SellerProductId,
                SellerId = x.SellerId,
                SellerName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                BrandId = x.BrandId,
                BrandName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? x.ExtraDetails : null,
                TotalQty = x.TotalQty,
                Status = x.Status,
                Live = x.Live,
                IsWishlistProduct = listWishlist.Where(q => q.ProductId == x.Guid).ToList().Count > 0 ? true : false,
                TotalVariant = x.TotalVariant,
                Highlights = x.Highlights
            }).ToList();

            #endregion

            int? totalProducts = listResult.Where(x => x.RecordCount != null).Any() ? listResult.Where(x => x.RecordCount != null).Select(x => x.RecordCount).FirstOrDefault() : 0;

            decimal? MinSellingPrice = listResult.Where(x => x.flag == f && x.MinSellingPrice != null).Select(x => x.MinSellingPrice).FirstOrDefault();
            decimal? MaxSellingPrice = listResult.Where(x => x.flag == f && x.MaxSellingPrice != null).Select(x => x.MaxSellingPrice).FirstOrDefault();

            decimal? MinDisc = listResult.Where(x => x.flag == f && x.Discount > 0).Distinct().OrderBy(d => d.Discount).Select(x => x.Discount).ToList().FirstOrDefault();
            decimal? MaxDisc = listResult.Where(x => x.flag == f && x.Discount > 0).Distinct().OrderByDescending(d => d.Discount).Select(x => x.Discount).ToList().FirstOrDefault();

            #region get all filter

            //BaseResponse<UserProductList> baseResponseFilterProduct = new BaseResponse<UserProductList>();
            //var responseFilterProduct = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?CategoryId=" + CategoryId + "&searchTexts=" + searchTexts + "&MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&availableProduct=" + available + "&PriceSort=" + PriceSort + "&Mode=get&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
            //baseResponseFilterProduct = baseResponseFilterProduct.JsonParseList(responseFilterProduct);

            //List<UserProductList> listFilterProduct = baseResponseFilterProduct.Data as List<UserProductList>;

            List<CategoryFilterDTO> catfilter = listResult.Where(x => x.flag == f && x.F_CategoryId != null).Distinct().Select(x => new CategoryFilterDTO
            {
                CategoryId = x.F_CategoryId,
                CategoryName = x.F_CategoryName,
            }).ToList();

            List<BrandFilterDTO> brandfilter = listResult.Where(x => x.flag == f && x.F_BrandId != null).Distinct().Select(x => new BrandFilterDTO
            {
                BrandId = x.F_BrandId,
                BrandName = x.F_BrandName,//getBrand((int)x.F_BrandId).Name
            }).ToList();

            List<SizeFilterDTO> sizefilter = listResult.Where(x => x.flag == f && x.F_SizeID != null).Distinct().Select(x => new SizeFilterDTO
            {
                SizeID = x.F_SizeID,
                Size = x.F_Size,
                Quantity = x.F_Quantity,
            }).ToList();

            List<ColorFilterDTO> colorfilter = listResult.Where(x => x.flag == f && x.F_ColorID != null).Distinct().Select(x => new ColorFilterDTO
            {
                ColorCode = x.F_ColorCode,
                ColorId = x.F_ColorID,
                ColorName = x.F_ColorName
            }).ToList();

            List<FilterTypeDTO> filterTypes = listResult.Where(x => x.flag == f && x.FilterTypeId != null && x.FilterValueId != null)
                .GroupBy(x => new { x.FilterTypeId, x.FilterTypeName })
                    .Select(x => new FilterTypeDTO
                    {
                        FilterTypeId = x.Key.FilterTypeId,
                        FilterTypeName = x.Key.FilterTypeName,
                        FilterValues = x.Select(r => new FilterValueDTO
                        {
                            FilterValueId = r.FilterValueId,
                            FilterValueName = r.FilterValueName
                        }).ToList()
                    }).ToList();

            #endregion

            string filterBrandIds = BrandIds;
            string filterSize = SizeIds;
            string filterColor = ColorIds;
            string filterSpecs = SpecTypeIds;

            if (!string.IsNullOrEmpty(fby) && fby.ToLower() == "brand")
            {
                filterBrandIds = "";
                response = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?CategoryId=" + CategoryId + "&SellerIds=" + SellerIds + "&BrandIds=" + filterBrandIds + "&searchTexts=" + HttpUtility.UrlEncode(searchTexts) + "&SizeIds=" + filterSize + "&ColorIds=" + filterColor + "&productCollectionId=" + productCollectionId + "&MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&MinDiscount=" + MinDiscount + "&availableProduct=" + available + "&PriceSort=" + PriceSort + "&SpecTypeIds=" + filterSpecs + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
                wholeResponse = wholeResponse.JsonParseList(response);
                listResult = wholeResponse.Data as List<UserProductList>;

                brandfilter = listResult.Where(x => x.flag == f && x.F_BrandId != null).Distinct().Select(x => new BrandFilterDTO
                {
                    BrandId = x.F_BrandId,
                    BrandName = x.F_BrandName,
                }).ToList();
            }

            if (!string.IsNullOrEmpty(fby) && fby.ToLower() == "size")
            {
                filterSize = "";
                response = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?CategoryId=" + CategoryId + "&SellerIds=" + SellerIds + "&BrandIds=" + filterBrandIds + "&searchTexts=" + HttpUtility.UrlEncode(searchTexts) + "&SizeIds=" + filterSize + "&ColorIds=" + filterColor + "&productCollectionId=" + productCollectionId + "&MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&MinDiscount=" + MinDiscount + "&availableProduct=" + available + "&PriceSort=" + PriceSort + "&SpecTypeIds=" + filterSpecs + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
                wholeResponse = wholeResponse.JsonParseList(response);
                listResult = wholeResponse.Data as List<UserProductList>;

                sizefilter = listResult.Where(x => x.flag == f && x.F_SizeID != null).Distinct().Select(x => new SizeFilterDTO
                {
                    SizeID = x.F_SizeID,
                    Size = x.F_Size,
                    Quantity = x.F_Quantity,
                }).ToList();
            }

            if (!string.IsNullOrEmpty(fby) && fby.ToLower() == "color")
            {
                filterColor = "";
                response = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?CategoryId=" + CategoryId + "&SellerIds=" + SellerIds + "&BrandIds=" + filterBrandIds + "&searchTexts=" + HttpUtility.UrlEncode(searchTexts) + "&SizeIds=" + filterSize + "&ColorIds=" + filterColor + "&productCollectionId=" + productCollectionId + "&MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&MinDiscount=" + MinDiscount + "&availableProduct=" + available + "&PriceSort=" + PriceSort + "&SpecTypeIds=" + filterSpecs + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
                wholeResponse = wholeResponse.JsonParseList(response);
                listResult = wholeResponse.Data as List<UserProductList>;

                colorfilter = listResult.Where(x => x.flag == f && x.F_ColorID != null).Distinct().Select(x => new ColorFilterDTO
                {
                    ColorCode = x.F_ColorCode,
                    ColorId = x.F_ColorID,
                    ColorName = x.F_ColorName
                }).ToList();
            }

            if (!string.IsNullOrEmpty(fby) && fby.ToLower() == "spec")
            {
                filterSpecs = "";
                response = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?CategoryId=" + CategoryId + "&SellerIds=" + SellerIds + "&BrandIds=" + filterBrandIds + "&searchTexts=" + HttpUtility.UrlEncode(searchTexts) + "&SizeIds=" + filterSize + "&ColorIds=" + filterColor + "&productCollectionId=" + productCollectionId + "&MinPrice=" + MinPrice + "&MaxPrice=" + MaxPrice + "&MinDiscount=" + MinDiscount + "&availableProduct=" + available + "&PriceSort=" + PriceSort + "&SpecTypeIds=" + filterSpecs + "&pageIndex=" + pageIndex + "&pageSize=" + pageSize, "GET", null);
                wholeResponse = wholeResponse.JsonParseList(response);
                listResult = wholeResponse.Data as List<UserProductList>;

                filterTypes = listResult.Where(x => x.flag == f && x.FilterTypeId != null && x.FilterValueId != null)
                .GroupBy(x => new { x.FilterTypeId, x.FilterTypeName })
                    .Select(x => new FilterTypeDTO
                    {
                        FilterTypeId = x.Key.FilterTypeId,
                        FilterTypeName = x.Key.FilterTypeName,
                        FilterValues = x.Select(r => new FilterValueDTO
                        {
                            FilterValueId = r.FilterValueId,
                            FilterValueName = r.FilterValueName
                        }).ToList()
                    }).ToList();
            }

            UserProductFilterDTO filter = new UserProductFilterDTO()
            {
                product_count = totalProducts,
                MinSellingPrice = MinSellingPrice,
                MaxSellingPrice = MaxSellingPrice,
                MinDiscount = MinDisc,
                MaxDiscount = MaxDisc,
                category_filter = catfilter,
                color_filter = colorfilter,
                size_filter = sizefilter,
                brand_filter = brandfilter,
                filter_types = filterTypes
            };

            UserProductListWithFilterDTO usrprops = new UserProductListWithFilterDTO()
            {
                Products = usrProducts,
                filterList = filter
            };

            baseResponse.Data = usrprops;

            return baseResponse;
        }


        public BaseResponse<GetProductDTO> BindUserProductDetails(string ProductGUID, string userId)
        {
            BaseResponse<UserProductDetailsDto> baseResponse = new BaseResponse<UserProductDetailsDto>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "/getUserProductDetails" + "?ProductGuid=" + ProductGUID, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);

            #region Wishlist
            List<Wishlist> listWishlist = new List<Wishlist>();
            if (userId != null)
            {
                BaseResponse<Wishlist> baseResponseWishlist = new BaseResponse<Wishlist>();
                var responseWish = helper.ApiCall(UserURL, EndPoints.Wishlist + "?UserID=" + userId + "&pageIndex=0&pageSize=0", "GET", null);
                baseResponseWishlist = baseResponseWishlist.JsonParseList(responseWish);
                listWishlist = baseResponseWishlist.Data as List<Wishlist>;
            }
            #endregion

            GetProductDTO product = new GetProductDTO();
            if (baseResponse.code == 200)
            {
                List<UserProductDetailsDto> details = baseResponse.Data as List<UserProductDetailsDto>;

                var ProductDetails = details.FirstOrDefault();
                List<SellerProductDTO> sellerProductlst = new List<SellerProductDTO>();
                List<ProductPriceDTO> priceDto = new List<ProductPriceDTO>();

                //BaseResponse<SellerListModel> sellerBaseResponse = new BaseResponse<SellerListModel>();
                //var Seller_response = helper.ApiCall(IdServerURL, EndPoints.SellerList + "?pageIndex=0&pageSize=0&status=active", "GET", null);
                //sellerBaseResponse = sellerBaseResponse.JsonParseList(Seller_response);
                //List<SellerKycList> lstSellers = new List<SellerKycList>();
                List<UserDetailsDTO> lstSellers = new List<UserDetailsDTO>();

                //if (sellerBaseResponse.code == 200)
                //{
                //    List<SellerListModel> sellerLists = sellerBaseResponse.Data as List<SellerListModel>;
                sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
                //if (seller != null)
                //{
                lstSellers = seller.bindSellerDetails(false, "Active", null, null, null, 0, 0);

                //    }
                //}


                priceDto = details.Select(x => new ProductPriceDTO
                {
                    Id = x.PriceMasterId != null ? Convert.ToInt32(x.PriceMasterId) : 0,
                    SellerProductId = x.SellerProductId != null ? Convert.ToInt32(x.SellerProductId) : 0,
                    MRP = x.MRP != null ? Convert.ToDecimal(x.MRP) : 0,
                    SellingPrice = x.SellingPrice != null ? Convert.ToDecimal(x.SellingPrice) : 0,
                    Discount = x.Discount != null ? Convert.ToDecimal(x.Discount) : 0,
                    Quantity = x.Quantity != null ? Convert.ToInt32(x.Quantity) : 0,
                    MarginIn = x.MarginIn != null ? Convert.ToString(x.MarginIn) : null,
                    MarginCost = x.MarginCost != null ? Convert.ToDecimal(x.MarginCost) : 0,
                    MarginPercentage = x.MarginPercentage != null ? Convert.ToDecimal(x.MarginPercentage) : 0,
                    SizeID = x.SizeId != null ? Convert.ToInt32(x.SizeId) : 0,
                    SizeName = x.SizeName != null ? Convert.ToString(x.SizeName) : null,
                    SizeTypeName = x.SizeTypeName != null ? Convert.ToString(x.SizeTypeName) : null

                }).DistinctBy(x => Convert.ToInt32(x.Id)).ToList();


                sellerProductlst = details.Select(x => new SellerProductDTO
                {
                    Id = x.SellerProductId != null ? Convert.ToInt32(x.SellerProductId) : 0,
                    ProductID = x.Id,
                    SellerID = x.SellerID != null ? Convert.ToString(x.SellerID) : null,
                    BrandID = x.BrandID != null ? Convert.ToInt32(x.BrandID) : 0,
                    SellerSKU = x.SKUCode != null ? Convert.ToString(x.SKUCode) : null,
                    IsSizeWisePriceVariant = x.IsSizeWisePriceVariant != null ? Convert.ToBoolean(x.IsSizeWisePriceVariant) : false,
                    IsExistingProduct = x.IsExistingProduct != null ? Convert.ToBoolean(x.IsExistingProduct) : false,
                    Live = x.Live != null ? Convert.ToBoolean(x.Live) : false,
                    Status = x.Status != null ? Convert.ToString(x.Status) : null,
                    ManufacturedDate = x.ManufacturedDate != null ? Convert.ToDateTime(x.ManufacturedDate) : null,
                    ExpiryDate = x.ExpiryDate != null ? Convert.ToDateTime(x.ExpiryDate) : null,
                    ExtraDetails = x.ExtraDetails != null ? Convert.ToString(x.ExtraDetails) : null,
                    MOQ = x.MOQ != null ? Convert.ToInt32(x.MOQ) : null,
                    SellerName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(x.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                    BrandName = !string.IsNullOrEmpty(x.ExtraDetails) ? JObject.Parse(x.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? x.ExtraDetails : null,
                    ShipmentBy = lstSellers.Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).ToList().Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).FirstOrDefault().ShipmentBy : null : null,
                    ShipmentPaidBy = lstSellers.Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).ToList().Count() > 0 ? lstSellers.Where(p => p.UserId == x.SellerID).FirstOrDefault().ShipmentChargesPaidByName : null : null,

                    ProductPrices = priceDto.Where(p => p.SellerProductId == Convert.ToInt32(x.SellerProductId)).ToList()

                }).DistinctBy(x => Convert.ToInt32(x.Id)).ToList();

                product.productId = ProductDetails.Id;
                product.productGuid = ProductDetails.Guid;
                product.CategoryId = ProductDetails.CategoryId ?? 0;
                product.ParentId = ProductDetails.MasterProductId;
                product.AssiCategoryId = ProductDetails.AssiCategoryId ?? 0;
                product.HSNCodeId = ProductDetails.HSNCodeId ?? 0;
                product.CompanySKUCode = ProductDetails.CompanySKUCode;
                product.ProductName = ProductDetails.ProductName;
                product.CustomeProductName = ProductDetails.CustomeProductName;
                product.TaxValueId = ProductDetails.TaxValueId ?? 0;
                product.Description = ProductDetails.Description;
                product.Highlights = ProductDetails.Highlights;
                product.CategoryName = ProductDetails.CategoryName;
                product.CategoryPathName = ProductDetails.CategoryPathNames;
                product.CategoryPathIds = ProductDetails.CategoryPathIds;
                product.SellerProducts = sellerProductlst;
                product.ProductColorMapping = BindColors(ProductDetails.Id);
                product.ProductImage = BindImages(ProductDetails.Id);
                //product.ProductVideoLinks = BindVideos(ProductDetails.Id);
                product.ProductSpecificationsMapp = BindSpecs(ProductDetails.Id);


                product.IsWishlistProduct = listWishlist.Where(q => q.ProductId == ProductDetails.Guid).ToList().Count > 0 ? true : false;
                product.MetaDescription = ProductDetails.MetaDescription;
                product.MetaTitle = ProductDetails.MetaTitle;

            }



            //List<SellerProductDTO> _SellerData = new List<SellerProductDTO>();
            //if (SellerData.Count() > 0)
            //{

            //    product.SellerProducts = SellerData;
            //    product.BrandID = product.SellerProducts.FirstOrDefault().BrandID;
            //    product.BrandName = product.SellerProducts.FirstOrDefault().BrandName;
            //    product.WeightSlab = product.SellerProducts.FirstOrDefault().WeightSlabName.ToString();
            //}
            //else
            //{
            //    product.SellerProducts = _SellerData;
            //    product.BrandID = 0;
            //    product.WeightSlab = null;
            //}

            //product.productId = details.Id;
            //product.productGuid = details.Guid;
            //product.CategoryId = (int)details.CategoryId;
            //product.ParentId = details.ParentId;
            //product.AssiCategoryId = (int)details.AssiCategoryId;
            //product.HSNCodeId = (int)details.HSNCodeId;
            //product.CompanySKUCode = details.CompanySKUCode;
            //product.ProductName = details.ProductName;
            //product.CustomeProductName = details.CustomeProductName;
            //product.TaxValueId = (int)details.TaxValueId;
            ////product.BrandID = BindCustomerSideSellerProduct(details.Id, sizeId, sellerId, status, isProductExist,isDeleted, isArchive).FirstOrDefault().BrandID;
            ////product.BrandID = product.SellerProducts.FirstOrDefault().BrandID;
            //product.Description = details.Description;
            //product.Highlights = details.Highlights;
            //product.Keywords = details.Keywords;
            //product.ProductLength = details.ProductLength;
            //product.ProductBreadth = details.ProductBreadth;
            //product.ProductHeight = details.ProductHeight;
            //product.ProductWeight = (decimal)details.ProductWeight;
            //product.CategoryName = details.CategoryName;
            //product.CategoryPathName = details.CategoryPathNames;
            //product.ProductColorMapping = BindColors(details.Id);
            //product.ProductImage = BindImages(details.Id);
            //product.ProductVideoLinks = BindVideos(details.Id);

            BaseResponse<GetProductDTO> productbase = new BaseResponse<GetProductDTO>();
            productbase.code = baseResponse.code;
            productbase.pagination = baseResponse.pagination;
            productbase.Message = baseResponse.Message;
            productbase.Data = product;
            return productbase;
        }

        public BaseResponse<AddInExistingProductList> BindExistingProductList(string SellerId, int? CategoryId = 0, int? BrandId = 0, string? CompanySkuCode = null, string? searchText = null, int? PageIndex = 0, int? PageSize = 0)
        {
            string url = string.Empty;
            if (BrandId > 0)
            {
                url += "&BrandId=" + BrandId;
            }
            if (CategoryId > 0)
            {
                url += "&CategoryId=" + CategoryId;
            }
            if (!string.IsNullOrEmpty(SellerId))
            {
                url += "&SellerId=" + SellerId;
            }
            if (!string.IsNullOrEmpty(CompanySkuCode))
            {
                url += "&CompanySkuCode=" + CompanySkuCode;
            }
            if (!string.IsNullOrEmpty(searchText))
            {
                url += "&searchText=" + searchText;
            }
            BaseResponse<AddInExistingProductList> baseResponse = new BaseResponse<AddInExistingProductList>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "/getExistingProductList" + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(GetResponse);
            return baseResponse;
        }


        public JObject product(List<KeyValueItem> objValue, string objName)
        {
            JObject _image = new JObject();
            JObject _PimageValueArray = new JObject();
            JArray _PimageValue = new JArray();



            foreach (var item in objValue)
            {
                JObject _Pimage = new JObject();
                JObject _PValue = new JObject();

                _Pimage["value"] = item.Value;
                if (item.ProductGuid != null)
                {
                    _Pimage["ProductGuid"] = item.ProductGuid;

                }

                _PValue[item.SellerProductID] = _Pimage;

                _PimageValue.Add(_PValue);
            }

            _PimageValueArray["featureName"] = objName;

            _PimageValueArray["value"] = _PimageValue;

            //_image["image"] = _PimageValueArray;

            return _PimageValueArray;

        }

        public JArray productSpecs(List<KeyValueItem> objValue, string objName)
        {
            JObject _image = new JObject();
            JObject _PimageValueArray = new JObject();
            JArray _PimageValue = new JArray();



            foreach (var item in objValue)
            {
                JObject _Pimage = new JObject();
                JObject _PValue = new JObject();

                _Pimage["value"] = item.Value;
                _PValue[item.SellerProductID] = _Pimage;
                _PimageValue.Add(_PValue);
            }

            //_PimageValueArray["values"] = _PimageValue;

            //_image["image"] = _PimageValueArray;

            return _PimageValue;

        }

        public BaseResponse<SearchSuggestion> searchsuggestion(string searchText)
        {


            BaseResponse<SearchSuggestion> baseResponse = new BaseResponse<SearchSuggestion>();
            BaseResponse<BrandLibrary> baseResponseBrand = new BaseResponse<BrandLibrary>();
            BaseResponse<UserProductList> wholeResponse = new BaseResponse<UserProductList>();
            BaseResponse<CategoryLibrary> baseResponseCat = new BaseResponse<CategoryLibrary>();

            var responseBrand = helper.ApiCall(UserURL, EndPoints.Brand + "?PageIndex=1&PageSize=6&searchText=" + searchText + "&status=Active", "GET", null);
            baseResponseBrand = baseResponseBrand.JsonParseList(responseBrand);
            List<BrandLibrary> lstBrand = baseResponseBrand.Data as List<BrandLibrary>;

            var response = helper.ApiCall(CatelogueURL, EndPoints.UserProductList + "?&searchTexts=" + HttpUtility.UrlEncode(searchText) + "&pageIndex=1&pageSize=6", "GET", null);
            wholeResponse = wholeResponse.JsonParseList(response);
            List<UserProductList> UserProductList = wholeResponse.Data as List<UserProductList>;
            UserProductList = UserProductList.Where(p => p.flag.ToString().ToLower() == "p").ToList();

            var responseCat = helper.ApiCall(CatelogueURL, EndPoints.Category + "?pageIndex=1&pageSize=6&Status=Active&Isdeleted=" + false + "&Searchtext=" + searchText, "GET", null);
            baseResponseCat = baseResponseCat.JsonParseList(responseCat);
            List<CategoryLibrary> lstCategory = baseResponseCat.Data as List<CategoryLibrary>;


            SearchSuggestion searchSuggestion = new SearchSuggestion();
            searchSuggestion.Brands = lstBrand.ToList();
            searchSuggestion.Categories = lstCategory.ToList();
            searchSuggestion.Products = UserProductList.ToList();

            baseResponse.code = 200;
            baseResponse.Data = searchSuggestion;
            return baseResponse;
        }

        public List<CategoryLibrary> GetCategoryWithParent(int Categoryid, string URL)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "/GetCategoryWithParent?Categoryid=" + Categoryid, "GET", null);
            List<CategoryLibrary> lstCategoryLibrary = new List<CategoryLibrary>();
            if (response != null)
            {
                BaseResponse<CategoryLibrary> baseResponse = new BaseResponse<CategoryLibrary>();
                baseResponse = baseResponse.JsonParseList(response);
                lstCategoryLibrary = baseResponse.Data as List<CategoryLibrary>;
            }
            return lstCategoryLibrary;

        }

    }


}
public class KeyValueItem
{
    public string SellerProductID { get; set; }
    public string? ProductGuid { get; set; } = null;
    public string Value { get; set; }
}
public class ProductSize
{
    public string SellerProductID { get; set; }
    public string SizeType { get; set; }
    public string Value { get; set; }
}

public class ProductSpec
{
    public string SellerProductID { get; set; }
    public string CatID { get; set; }
    public string SpecID { get; set; }
    public string SpecTypeId { get; set; }
    public string SpecValueId { get; set; }
    public string Name { get; set; }
    public string SpecType { get; set; }
    public string Value { get; set; }
}
