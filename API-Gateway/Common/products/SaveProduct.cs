using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Immutable;

namespace API_Gateway.Common.products
{
    public class SaveProduct
    {
        private readonly IConfiguration _configuration;
        public string CatelogueURL = string.Empty;
        public string UserURL = string.Empty;
        public string IDServerUrl = string.Empty;
        public string UserId = string.Empty;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        public SaveProduct(IConfiguration configuration, HttpContext httpContext, string Userid)
        {
            _configuration = configuration;
            UserId = Userid;
            _httpContext = httpContext;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            UserURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            helper = new ApiHelper(_httpContext);
        }


        public Products BindProduct(ProductDetails model)
        {
            Products product = new Products();
            product.IsMasterProduct = model.IsMasterProduct;
            if (model.IsMasterProduct)
            {
                product.ParentId = null;
            }
            else
            {
                product.ParentId = model.ParentId;
                product.ProductBreadth = model.ProductBreadth;
                product.ProductWeight = model.ProductWeight;
                product.ProductHeight = model.ProductHeight;
                product.ProductLength = model.ProductLength;
            }

            product.CategoryId = model.CategoryId;
            product.CompanySKUCode = model.CompanySKUCode;
            product.AssiCategoryId = model.AssiCategoryId;
            product.TaxValueId = model.TaxValueId;
            product.HSNCodeId = model.HSNCodeId;
            product.ProductName = model.ProductName;
            product.CustomeProductName = model.CustomeProductName;
            product.Description = model.Description;
            product.Highlights = model.Highlights;
            product.Keywords = model.Keywords;
            product.CreatedBy = UserId;
            product.CreatedAt = DateTime.Now;



            return product;
        }

        public SellerProduct BindSellerProducts(SellerProductDTO? item, int ProductId, int BrandId)
        {
            BaseResponse<KYCDetails> baseResponse = new BaseResponse<KYCDetails>();


            SellerProduct sellerProduct = new SellerProduct();
            sellerProduct.ProductID = ProductId;
            sellerProduct.SellerID = item.SellerID;
            sellerProduct.BrandID = BrandId;
            sellerProduct.SKUCode = item.SellerSKU;
            sellerProduct.IsSizeWisePriceVariant = item.IsSizeWisePriceVariant;
            sellerProduct.IsExistingProduct = item.IsExistingProduct;
            sellerProduct.PackingBreadth = item.PackingBreadth;
            sellerProduct.PackingHeight = item.PackingHeight;
            sellerProduct.PackingLength = item.PackingLength;
            sellerProduct.PackingWeight = item.PackingWeight;
            sellerProduct.WeightSlabId = item.WeightSlabId;
            sellerProduct.Live = item.Live;
            sellerProduct.Status = item.Status;
            sellerProduct.ManufacturedDate = null;
            sellerProduct.ExpiryDate = null;
            sellerProduct.MOQ = null;
            sellerProduct.CreatedAt = DateTime.Now;
            sellerProduct.CreatedBy = UserId;



            sellerKyc seller = new sellerKyc(_configuration, _httpContext);
            var sellerdetails = seller.BindSeller(item.SellerID);

            BaseResponse<AssignBrandToSeller> AssibaseResponse = new BaseResponse<AssignBrandToSeller>();
            AssignBrands AssignbrandDetails = new AssignBrands(UserURL, _httpContext, CatelogueURL, IDServerUrl, _configuration);
            AssibaseResponse = AssignbrandDetails.GetAssignBrandsByUserIDandBrandId(item.SellerID, BrandId, null);

            AssignBrandToSeller brandLibrary = new AssignBrandToSeller();
            brandLibrary = AssibaseResponse.Data as AssignBrandToSeller;

            string legalName = string.Empty;
            string tradeName = string.Empty;
            if (!sellerdetails.IsUserWithGST)
            {
                legalName = sellerdetails.DisplayName;
                tradeName = sellerdetails.DisplayName;
            }
            else
            {
                if (sellerdetails.gSTInfos.ToList().Count > 0)
                {
                    legalName = sellerdetails.gSTInfos.FirstOrDefault().LegalName;
                    tradeName = sellerdetails.gSTInfos.FirstOrDefault().TradeName;
                }
                else
                {
                    legalName = sellerdetails.DisplayName;
                    tradeName = sellerdetails.DisplayName;
                }
            }

            var SellerDetails = new { FullName = sellerdetails.FullName, Email = sellerdetails.UserName, PhoneNo = sellerdetails.PhoneNumber, Display = sellerdetails.DisplayName, LegalName = legalName, TradeName = tradeName, SellerId = item.SellerID, SellerSignature = sellerdetails.DigitalSign, ShipmentBy = sellerdetails.ShipmentBy, ShipmentChargesPaidBy = sellerdetails.ShipmentChargesPaidBy, ShipmentChargesPaidByName = sellerdetails.ShipmentChargesPaidByName, SellerStatus = sellerdetails.SellerStatus, KycStatus = sellerdetails.Status };
            var BrandDetails = new { Name = brandLibrary.BrandName, BrandId = brandLibrary.BrandId, AssignBrandId = brandLibrary.Id, AssignBrandStatus = brandLibrary.Status, BrandStatus = brandLibrary.BrandStatus, Logo = brandLibrary.Logo };

            var Extradetails = new
            {
                SellerDetails = SellerDetails,
                BrandDetails = BrandDetails
            };


            var jsonString = JsonConvert.SerializeObject(Extradetails);

            sellerProduct.ExtraDetails = jsonString;




            return sellerProduct;
        }

        public SellerProduct BindSellerProducts(ExistingProduct model)
        {
            SellerProduct sellerProduct = new SellerProduct();

            sellerProduct.ProductID = model.ProductId;
            sellerProduct.SellerID = model.SellerId;
            sellerProduct.BrandID = model.BrandId;
            sellerProduct.SKUCode = model.SellerSKU;
            sellerProduct.IsSizeWisePriceVariant = model.IsSizeWisePriceVariant;
            sellerProduct.IsExistingProduct = model.IsExistingProduct;
            sellerProduct.PackingBreadth = model.PackingBreadth;
            sellerProduct.PackingHeight = model.PackingHeight;
            sellerProduct.PackingLength = model.PackingLength;
            sellerProduct.PackingWeight = model.PackingWeight;
            sellerProduct.WeightSlabId = model.WeightSlabId;
            sellerProduct.Live = model.Live;
            sellerProduct.Status = model.Status;
            sellerProduct.ManufacturedDate = null;
            sellerProduct.ExpiryDate = null;
            sellerProduct.MOQ = null;
            sellerProduct.CreatedAt = DateTime.Now;
            sellerProduct.CreatedBy = UserId;

            sellerProduct.ModifiedBy = UserId;
            sellerKyc seller = new sellerKyc(_configuration, _httpContext);
            var sellerdetails = seller.BindSeller(model.SellerId);

            BaseResponse<AssignBrandToSeller> AssibaseResponse = new BaseResponse<AssignBrandToSeller>();
            AssignBrands AssignbrandDetails = new AssignBrands(UserURL, _httpContext, CatelogueURL, IDServerUrl, _configuration);
            AssibaseResponse = AssignbrandDetails.GetAssignBrandsByUserIDandBrandId(model.SellerId, model.BrandId, null);

            AssignBrandToSeller brandLibrary = new AssignBrandToSeller();
            brandLibrary = AssibaseResponse.Data as AssignBrandToSeller;

            string legalName = string.Empty;
            string tradeName = string.Empty;
            if (!sellerdetails.IsUserWithGST)
            {
                legalName = sellerdetails.DisplayName;
                tradeName = sellerdetails.DisplayName;
            }
            else
            {
                if (sellerdetails.gSTInfos.ToList().Count > 0)
                {
                    legalName = sellerdetails.gSTInfos.FirstOrDefault().LegalName;
                    tradeName = sellerdetails.gSTInfos.FirstOrDefault().TradeName;
                }
                else
                {
                    legalName = sellerdetails.DisplayName;
                    tradeName = sellerdetails.DisplayName;
                }
            }

            var SellerDetails = new { FullName = sellerdetails.FullName, Email = sellerdetails.UserName, PhoneNo = sellerdetails.PhoneNumber, Display = sellerdetails.DisplayName, LegalName = legalName, TradeName = tradeName, SellerId = model.SellerId, SellerSignature = sellerdetails.DigitalSign, ShipmentBy = sellerdetails.ShipmentBy, ShipmentChargesPaidBy = sellerdetails.ShipmentChargesPaidBy, ShipmentChargesPaidByName = sellerdetails.ShipmentChargesPaidByName, SellerStatus = sellerdetails.SellerStatus, KycStatus = sellerdetails.Status };
            var BrandDetails = new { Name = brandLibrary.BrandName, BrandId = brandLibrary.BrandId, AssignBrandId = brandLibrary.Id, AssignBrandStatus = brandLibrary.Status, BrandStatus = brandLibrary.BrandStatus, Logo = brandLibrary.Logo };

            var Extradetails = new
            {
                SellerDetails = SellerDetails,
                BrandDetails = BrandDetails
            };


            var jsonString = JsonConvert.SerializeObject(Extradetails);

            sellerProduct.ExtraDetails = jsonString;

            return sellerProduct;
        }

        public List<ProductPrice> BindProductPrices(IEnumerable<ProductPriceDTO>? ProductPrices, int SellerProductId)
        {
            List<ProductPrice> pp = new List<ProductPrice>();
            bool setMarginProductLevel = Convert.ToBoolean(_configuration.GetValue<string>("allow_set_margin_on_product_level"));
            foreach (var item in ProductPrices)
            {
                ProductPrice productPrice = new ProductPrice();
                productPrice.SellerProductID = SellerProductId;
                productPrice.MRP = item.MRP;
                productPrice.SellingPrice = item.SellingPrice;
                productPrice.Discount = item.Discount;
                productPrice.Quantity = item.Quantity;
                if (setMarginProductLevel)
                {
                    productPrice.MarginIn = item.MarginIn;
                    productPrice.MarginCost = item.MarginCost;
                    productPrice.MarginPercentage = item.MarginPercentage;
                }
                else
                {
                    productPrice.MarginIn = null;
                    productPrice.MarginCost = 0;
                    productPrice.MarginPercentage = 0;
                }
                productPrice.SizeID = item.SizeID;
                productPrice.CreatedAt = DateTime.Now;
                productPrice.CreatedBy = UserId;
                pp.Add(productPrice);
            }
            return pp;
        }

        public List<ProductWareHouse> BindProductWarehouses(IEnumerable<ProductWarehouseDTO> ProductWarehouses, int ProductId, int SellerPriceId, int SellerProductId)
        {

            List<ProductWareHouse> productWareHouses = new List<ProductWareHouse>();
            foreach (var item in ProductWarehouses)
            {
                ProductWareHouse productWareHouse = new ProductWareHouse();
                productWareHouse.ProductID = ProductId;
                productWareHouse.WarehouseID = item.WarehouseId;
                productWareHouse.WarehouseName = item.WarehouseName;
                productWareHouse.ProductQuantity = item.Quantity;
                productWareHouse.SellerWiseProductPriceMasterID = SellerPriceId;
                productWareHouse.SellerProductID = SellerProductId;
                productWareHouse.CreatedAt = DateTime.Now;
                productWareHouse.CreatedBy = UserId;

                productWareHouses.Add(productWareHouse);
            }

            return productWareHouses;
        }

        public List<ProductImages> BindProductImage(ProductDetails model, int productId)
        {
            List<ProductImages> images = new List<ProductImages>();

            foreach (var p in model.ProductImage)
            {
                if (!string.IsNullOrEmpty(p.Url))
                {
                    ProductImages image = new ProductImages();
                    image.Id = p.Id;
                    image.Url = p.Url;
                    image.Type = p.Type;
                    image.Sequence = p.Sequence;
                    image.ProductID = productId;
                    image.CreatedBy = UserId;
                    image.CreatedAt = DateTime.Now;
                    images.Add(image);
                }
            }
            return images;
        }

        //public List<ProductVideoLink> BindProductVideoLinks(ProductDetails model, int productId)
        //{
        //    List<ProductVideoLink> videos = new List<ProductVideoLink>();

        //    foreach (var p in model.ProductVideoLinks)
        //    {
        //        if (!string.IsNullOrEmpty(p.Link))
        //        {
        //            ProductVideoLink videoLink = new ProductVideoLink();
        //            videoLink.Id = p.Id;
        //            videoLink.ProductID = productId;
        //            videoLink.Link = p.Link;
        //            videoLink.CreatedBy = UserId;
        //            videoLink.CreatedAt = DateTime.Now;
        //            videos.Add(videoLink);
        //        }
        //    }
        //    return videos;
        //}

        public List<ProductColorMapp> BindProductColorMapping(ProductDetails model, int productId)
        {
            List<ProductColorMapp> colors = new List<ProductColorMapp>();

            foreach (var p in model.ProductColorMapping)
            {
                if (p.ColorId != null && p.ColorId >= 0)
                {
                    ProductColorMapp colorMapp = new ProductColorMapp();
                    colorMapp.Id = p.Id;
                    colorMapp.ProductID = productId;
                    colorMapp.ColorID = p.ColorId;
                    colorMapp.CreatedBy = UserId;
                    colorMapp.CreatedAt = DateTime.Now;
                    colors.Add(colorMapp);
                }
            }
            return colors;
        }


        public BaseResponse<string> SaveData(ProductDetails model, bool isBulkupload = false)
        {


            BaseResponse<string> baseR1 = CheckProductExist(model);
            if (baseR1.code != 200)
            {
                return baseR1;
            }

            var productTableEntry = ProductTableEntry(model);

            int ProductId = Convert.ToInt32(productTableEntry.Data);
            int ParentId = Convert.ToInt32(model.ParentId);

            try
            {

                if (ProductId != null || ProductId != 0)
                {

                    var SellerProducts = SellerProductsEntries(model.SellerProducts, ProductId, model.BrandID);

                    int SellerProductId = Convert.ToInt32(SellerProducts.Data);

                    var ProductPrices = ProductPricesEntries(model.SellerProducts.ProductPrices, SellerProductId, ProductId);


                    //var ProductPriceIds = ProductPrices.Select(x => Convert.ToInt32(x.Data)).ToList();

                    //for (int j = 0; j < ProductPrices.Count; j++)
                    //{
                    //    var ProductWarehouses = ProductWarehousesEntries(model.SellerProducts.ProductPrices.ElementAt(j).ProductWarehouses, ProductId, ProductPriceIds.ElementAt(j), SellerProductId);
                    //}


                    // ProductSpecificationEntries (Multiple)
                    var productSpecTableEntries = ProductSpecEntries(model, ProductId);
 

                    
                    // ProductImagesTableEntries (Multiple)

                    var productImagesTableEntries = ProductImageEntries(model, ProductId);
                    List<ProductImageDTO> productsImage = new List<ProductImageDTO>();

                    productsImage = model.ProductImage.ToList();
                    productsImage = productsImage.Where(p => p.Type.ToLower() == "image").ToList();

                    foreach (var item in productsImage)
                    {
                        if (!string.IsNullOrEmpty(item.Url))
                        {
                            ImageUpload imageUpload = new ImageUpload(_configuration);
                            bool IsUploaded = imageUpload.UploadDocs("ProductImage", item.Url);
                        }
                    }
                    // ProductVideoLinksEntries (Multiple)
                    //var productVideoEntries = ProductVideoLinkEntries(model, ProductId);

                    var productColorEntries = ProductColorEntries(model, ProductId);

                    var dataresmessage = "Id:" + ProductId + " , MasterId:" + ParentId;

                    BaseResponse<string> baseR = new BaseResponse<string>();
                    baseR.code = 200;
                    baseR.Message = "Product Data Inserted Successfully";
                    baseR.Data = dataresmessage;
                    return baseR;
                }
                else
                {
                    BaseResponse<string> baseR = new BaseResponse<string>();
                    baseR.code = 201;
                    baseR.Message = "Error";

                    return baseR;
                }

            }
            catch (Exception ex)
            {
                // If any API call fails, roll back by calling the rollbackAPI
                //await RollbackAPI(data);
                var rollbackResponse = helper.ApiCall(CatelogueURL, EndPoints.ProductRollback + "?ProductId=" + ProductId, "GET", null);

                BaseResponse<string> baseR = new BaseResponse<string>();
                baseR = baseR.JsonParseInputResponse(rollbackResponse);
                return baseR;
            }

        }


        public BaseResponse<string> SaveExistingData(ExistingProduct model)
        {
            BaseResponse<string> baseR1 = CheckExistingProducts(model);
            if (baseR1.code != 200)
            {
                return baseR1;
            }

            var SellerProducts = SellerProductsEntries(model);

            int SellerProductId = Convert.ToInt32(SellerProducts.Data);


            var ProductPrices = ProductPricesEntries(model.ProductPrices, SellerProductId, model.ProductId);


            //var ProductPriceIds = ProductPrices.Select(x => Convert.ToInt32(x.Data)).ToList();

            //for (int j = 0; j < ProductPrices.Count; j++)
            //{
            //    var ProductWarehouses = ProductWarehousesEntries(model.ProductPrices.ElementAt(j).ProductWarehouses.Where(x => x.Id == 0).ToList(), model.ProductId, ProductPriceIds.ElementAt(j), SellerProductId);
            //}


            BaseResponse<string> baseR = new BaseResponse<string>();
            baseR.code = 200;
            baseR.Message = "Product Data Inserted Successfully";
            baseR.Data = SellerProductId;
            return baseR;

        }


        public BaseResponse<Products> ProductTableEntry(ProductDetails model)
        {
            BaseResponse<Products> baseResponse = new BaseResponse<Products>();
            try
            {
                //var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?CategoryID=" + model.CategoryId + "&Getparent=" + true, "GET", null);
                //baseResponse = baseResponse.JsonParseList(GetResponse);
                //List<Products> tempList = baseResponse.Data as List<Products>;
                //if (tempList.Where(x => x.CompanySKUCode == model.CompanySKUCode).Any())
                //{
                //    baseResponse = baseResponse.AlreadyExists();
                //}
                //else
                //{
                //    if (model.IsMasterProduct)
                //    {
                //        Products product = BindProduct(model);
                //        var PostResponse = helper.ApiCall(CatelogueURL, EndPoints.Product, "POST", product);
                //        baseResponse = baseResponse.JsonParseInputResponse(PostResponse);
                //        model.ParentId = Convert.ToInt32(baseResponse.Data);
                //    }
                //    model.IsMasterProduct = false;
                //    Products childProduct = BindProduct(model);

                //    var Response = helper.ApiCall(CatelogueURL, EndPoints.Product, "POST", childProduct);
                //    baseResponse = baseResponse.JsonParseInputResponse(Response);
                //}

                if (model.IsMasterProduct)
                {
                    Products product = BindProduct(model);
                    var PostResponse = helper.ApiCall(CatelogueURL, EndPoints.Product, "POST", product);
                    baseResponse = baseResponse.JsonParseInputResponse(PostResponse);
                    model.ParentId = Convert.ToInt32(baseResponse.Data);
                }
                model.IsMasterProduct = false;
                Products childProduct = BindProduct(model);

                var Response = helper.ApiCall(CatelogueURL, EndPoints.Product, "POST", childProduct);
                baseResponse = baseResponse.JsonParseInputResponse(Response);

                if (baseResponse.code != 200)
                {
                    throw new Exception("Error occurred");
                }
            }
            catch (Exception ex)
            {
                // Log the error message
                Console.WriteLine(ex.Message);
                // Set the error response
                throw new Exception("Error occurred");
            }
            return baseResponse;
        }



        //Used for Inserting Existing Product Record 
        public BaseResponse<SellerProduct> SellerProductsEntries(ExistingProduct model)
        {
            try
            {
                SellerProduct sp = BindSellerProducts(model);
                BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
                var response = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct, "POST", sp);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                if (baseResponse.code != 200)
                {
                    throw new Exception("API call failed with error code: " + baseResponse.code);
                }
                return baseResponse;
            }
            catch (Exception ex)
            {
                // Handle the exception as per your application's requirements
                // For example, log the error or return an appropriate error response to the caller
                // Here, we're rethrowing the exception to propagate it to the calling function
                Console.WriteLine(ex.Message);
                // Set the error response
                throw new Exception("Error occurred");
            }
        }


        //Used for Inserting Non-Existing Product Record 
        public BaseResponse<SellerProduct> SellerProductsEntries(SellerProductDTO sellerProduct, int ProductId, int BrandId)
        {
            SellerProduct sp = BindSellerProducts(sellerProduct, ProductId, BrandId);

            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            try
            {
                var response = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct, "POST", sp);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                if (baseResponse.code != 200)
                {
                    throw new Exception("Failed to add seller product.");
                }

            }
            catch (Exception ex)
            {
                // Handle the exception as appropriate for your application
                // For example, log the error or return a custom error message
                Console.WriteLine(ex.Message);
                // Set the error response
                throw new Exception("Error occurred");
            }
            return baseResponse;
        }




        public List<BaseResponse<ProductPrice>> ProductPricesEntries(IEnumerable<ProductPriceDTO> Productprices, int SellerProductId, int productId)
        {
            List<BaseResponse<ProductPrice>> bases = new List<BaseResponse<ProductPrice>>();
            List<ProductPrice> pps = BindProductPrices(Productprices, SellerProductId);
            foreach (var pp in pps)
            {

                BaseResponse<ProductPrice> PbaseResponse = new BaseResponse<ProductPrice>();
                PbaseResponse = SaveProductPrice(pp, Productprices, SellerProductId, productId);
                if (PbaseResponse.code == 200)
                {
                    bases.Add(PbaseResponse);
                }

                //try
                //{
                //    BaseResponse<ProductPrice> PbaseResponse = new BaseResponse<ProductPrice>();
                //    string url = string.Empty;
                //    if (pp.SizeID != null)
                //    {
                //        url = "&SizeID=" + pp.SizeID;
                //    }
                //    var presponse = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductID=" + SellerProductId + url, "get", null);
                //    PbaseResponse = PbaseResponse.JsonParseList(presponse);
                //    List<ProductPrice> DataProductPrices = PbaseResponse.Data as List<ProductPrice>;
                //    if (DataProductPrices.Count == 0)
                //    {

                //        BaseResponse<ProductPrice> baseResponse = new BaseResponse<ProductPrice>();
                //        var response = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster, "POST", pp);
                //        baseResponse = baseResponse.JsonParseInputResponse(response);
                //        if (baseResponse.code != 200)
                //        {
                //            throw new Exception($"API call failed with status code: {baseResponse.code}");
                //        }
                //        bases.Add(baseResponse);
                //        bool AllowWarehouseManagement = Convert.ToBoolean(_configuration.GetValue<string>("Allow_warehouse_management"));

                //        if (!string.IsNullOrEmpty(baseResponse.Data.ToString()))
                //        {
                //            int Priceid = Convert.ToInt32(baseResponse.Data.ToString());
                //            var data = Productprices.Where(x => x.SizeID == pp.SizeID).ToList();
                //            presponse = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductID=" + SellerProductId + url, "get", null);
                //            PbaseResponse = PbaseResponse.JsonParseList(presponse);
                //            List<ProductPrice> Size = PbaseResponse.Data as List<ProductPrice>;
                //            if (Size.Count > 0)
                //            {
                //                if (AllowWarehouseManagement)
                //                {
                //                    List<ProductWarehouseDTO> warehouseDto = data[0].ProductWarehouses.ToList();
                //                    var ProductWarehouses = ProductWarehousesEntries(warehouseDto, productId, Priceid, SellerProductId);
                //                }
                //            }
                //        }

                //    }
                //}
                //catch (Exception ex)
                //{
                //    // Log the exception
                //    Console.WriteLine($"An error occurred while making API call: {ex.Message}");
                //    // Rethrow the exception to propagate it to the calling function
                //    throw;
                //}
            }

            return bases;
        }

        public BaseResponse<ProductPrice> SaveProductPrice(ProductPrice price, IEnumerable<ProductPriceDTO> Productprices, int SellerProductId, int productId)
        {

            try
            {
                BaseResponse<ProductPrice> PbaseResponse = new BaseResponse<ProductPrice>();
                BaseResponse<ProductPrice> baseResponse = new BaseResponse<ProductPrice>();
                string url = string.Empty;
                if (price.SizeID != null)
                {
                    url = "&SizeID=" + price.SizeID;
                }
                var presponse = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductID=" + SellerProductId + url, "get", null);
                PbaseResponse = PbaseResponse.JsonParseList(presponse);
                List<ProductPrice> DataProductPrices = PbaseResponse.Data as List<ProductPrice>;
                if (DataProductPrices.Count == 0)
                {

                    var response = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster, "POST", price);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                    if (baseResponse.code != 200)
                    {
                        throw new Exception($"API call failed with status code: {baseResponse.code}");
                    }
                    //bases.Add(baseResponse);
                    bool AllowWarehouseManagement = Convert.ToBoolean(_configuration.GetValue<string>("Allow_warehouse_management"));

                    if (!string.IsNullOrEmpty(baseResponse.Data.ToString()))
                    {
                        int Priceid = Convert.ToInt32(baseResponse.Data.ToString());
                        var data = Productprices.Where(x => x.SizeID == price.SizeID).ToList();
                        //presponse = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductID=" + SellerProductId + url, "get", null);
                        //PbaseResponse = PbaseResponse.JsonParseList(presponse);
                        //List<ProductPrice> Size = PbaseResponse.Data as List<ProductPrice>;
                        //if (Size.Count > 0)
                        //{
                        if (AllowWarehouseManagement)
                        {
                            List<ProductWarehouseDTO> warehouseDto = data[0].ProductWarehouses.ToList();
                            var ProductWarehouses = ProductWarehousesEntries(warehouseDto, productId, Priceid, SellerProductId);
                        }
                        //}
                    }

                }
                return baseResponse;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while making API call: {ex.Message}");
                // Rethrow the exception to propagate it to the calling function
                throw;
            }

        }


        public List<BaseResponse<ProductWareHouse>> ProductWarehousesEntries(IEnumerable<ProductWarehouseDTO> productWarehouses, int ProductId, int ProductPriceId, int SellerProductId)
        {
            List<BaseResponse<ProductWareHouse>> bases = new List<BaseResponse<ProductWareHouse>>();
            List<ProductWareHouse> pws = BindProductWarehouses(productWarehouses, ProductId, ProductPriceId, SellerProductId);
            foreach (var pw in pws)
            {
                BaseResponse<ProductWareHouse> baseResponse = new BaseResponse<ProductWareHouse>();
                try
                {
                    BaseResponse<ProductWareHouse> PbaseResponse = new BaseResponse<ProductWareHouse>();
                    var presponse = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse + "?sellerwiseproductpricemasterid=" + ProductPriceId + "&productId=" + ProductId + "&warehouseid=" + pw.WarehouseID, "get", null);
                    PbaseResponse = PbaseResponse.JsonParseList(presponse);
                    List<ProductWareHouse> DataproductWarehouses = PbaseResponse.Data as List<ProductWareHouse>;
                    if (DataproductWarehouses.Count == 0)
                    {
                        var response = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse, "POST", pw);
                        baseResponse = baseResponse.JsonParseInputResponse(response);

                        if (baseResponse.code != 200)
                        {
                            throw new Exception("The API call returned an error: " + baseResponse.Message);
                        }
                        bases.Add(baseResponse);
                    }


                }
                catch (Exception ex)
                {
                    // Handle the exception, log it, or re-throw it if necessary
                    // For example:
                    Console.WriteLine("An error occurred while processing a ProductWarehouse entry: " + ex.Message);
                    throw;
                }
            }

            return bases;
        }



        public List<BaseResponse<ProductImages>> ProductImageEntries(ProductDetails model, int productId)
        {
            List<BaseResponse<ProductImages>> bases = new List<BaseResponse<ProductImages>>();
            BaseResponse<ProductImages> baseResponse = new BaseResponse<ProductImages>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductsImage + "?ProductID=" + productId, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ProductImages> tempList = baseResponse.Data as List<ProductImages>;

            if (tempList.Any())
            {
                response = helper.ApiCall(CatelogueURL, EndPoints.ProductsImage + "?ProductID=" + productId, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            List<ProductImages> productImages = BindProductImage(model, productId);
            foreach (var image in productImages)
            {
                try
                {
                    response = helper.ApiCall(CatelogueURL, EndPoints.ProductsImage, "POST", image);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                    if (baseResponse.code != 200)
                    {
                        throw new Exception("API call failed with error code: " + baseResponse.code);
                    }
                    bases.Add(baseResponse);
                }
                catch (Exception ex)
                {
                    // Handle exception here
                    // You can log the error message or re-throw the exception
                    throw ex;
                }
            }

            return bases;
        }

        //public List<BaseResponse<ProductVideoLink>> ProductVideoLinkEntries(ProductDetails model, int ProductId)
        //{
        //    List<BaseResponse<ProductVideoLink>> bases = new List<BaseResponse<ProductVideoLink>>();
        //    BaseResponse<ProductVideoLink> baseResponse = new BaseResponse<ProductVideoLink>();
        //    try
        //    {
        //        var temp = helper.ApiCall(CatelogueURL, EndPoints.ProductsVideoLinks + "?productID=" + ProductId, "GET", null);
        //        baseResponse = baseResponse.JsonParseList(temp);
        //        List<ProductVideoLink> tempList = baseResponse.Data as List<ProductVideoLink>;

        //        if (tempList.Any())
        //        {
        //            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductsVideoLinks + "?productID=" + ProductId, "DELETE", null);
        //            baseResponse = baseResponse.JsonParseInputResponse(response);
        //        }
        //        List<ProductVideoLink> productVideoLinks = BindProductVideoLinks(model, ProductId);
        //        foreach (var link in productVideoLinks)
        //        {

        //            temp = helper.ApiCall(CatelogueURL, EndPoints.ProductsVideoLinks, "POST", link);
        //            baseResponse = baseResponse.JsonParseInputResponse(temp);
        //            if (baseResponse.code != 200)
        //            {
        //                throw new Exception("Api call failed with status code " + baseResponse.code);
        //            }
        //            bases.Add(baseResponse);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle the exception as per your requirements.
        //        // You could log the error, rethrow the exception, or return an error response to the caller.
        //        Console.WriteLine("An error occurred while processing a ProductVideos entry: " + ex.Message);
        //        throw;
        //    }
        //    return bases;
        //}


        public List<BaseResponse<ProductColorMapp>> ProductColorEntries(ProductDetails model, int ProductId)
        {
            List<BaseResponse<ProductColorMapp>> bases = new List<BaseResponse<ProductColorMapp>>();
            BaseResponse<ProductColorMapp> baseResponse = new BaseResponse<ProductColorMapp>();
            var temp = helper.ApiCall(CatelogueURL, EndPoints.ProductColorMapping + "?productID=" + ProductId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ProductColorMapp> tempList = baseResponse.Data as List<ProductColorMapp>;

            try
            {
                if (tempList.Any())
                {
                    var response = helper.ApiCall(CatelogueURL, EndPoints.ProductColorMapping + "?productID=" + ProductId, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);

                    if (baseResponse.code != 200)
                    {
                        throw new Exception("Failed to delete color mapping");
                    }
                }

                List<ProductColorMapp> colors = BindProductColorMapping(model, ProductId);

                foreach (var link in colors)
                {
                    temp = helper.ApiCall(CatelogueURL, EndPoints.ProductColorMapping, "POST", link);
                    baseResponse = baseResponse.JsonParseInputResponse(temp);

                    if (baseResponse.code != 200)
                    {
                        throw new Exception("Failed to add color mapping");
                    }

                    bases.Add(baseResponse);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception as needed, e.g. log it, return an error response, etc.
                Console.WriteLine("An error occurred while processing a ProductVideos entry: " + ex.Message);
                throw;
            }

            return bases;
        }

        public List<ProductSpecificationMapping> BindProductSpec(ProductDetails model, int productId)
        {
            List<ProductSpecificationMapping> specs = new List<ProductSpecificationMapping>();
            if (model.ProductSpecificationsMapp != null)
            {
                foreach (var p in model.ProductSpecificationsMapp)
                {
                    if (!string.IsNullOrEmpty(p.Value))
                    {
                        ProductSpecificationMapping specificationMapping = new ProductSpecificationMapping();
                        specificationMapping.Id = p.Id;
                        specificationMapping.SpecId = p.SpecId;
                        specificationMapping.SpecTypeId = p.SpecTypeId;
                        specificationMapping.SpecValueId = p.SpecValueId;
                        specificationMapping.Value = p.Value;
                        specificationMapping.FileName = p.FileName;
                        specificationMapping.CatId = model.CategoryId;
                        specificationMapping.ProductID = productId;
                        specificationMapping.CreatedBy = UserId;
                        specificationMapping.CreatedAt = DateTime.UtcNow;
                        specs.Add(specificationMapping);
                    }
                }
            }
            return specs;
        }
        public List<BaseResponse<ProductSpecificationMapping>> ProductSpecEntries(ProductDetails model, int productId)
        {
            List<BaseResponse<ProductSpecificationMapping>> bases = new List<BaseResponse<ProductSpecificationMapping>>();
            BaseResponse<ProductSpecificationMapping> baseResponse = new BaseResponse<ProductSpecificationMapping>();
            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductSpecificationMapping + "?ProductID=" + productId + "&PageIndex=0&PageSize=0", "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ProductSpecificationMapping> tempList = baseResponse.Data as List<ProductSpecificationMapping>;

            if (tempList.Any())
            {
                //response = helper.ApiCall(CatelogueURL, EndPoints.ProductSpecificationMapping + "?ProductID=" + productId, "DELETE", null, Token);
                //baseResponse = baseResponse.JsonParseInputResponse(response);
                baseResponse = baseResponse.AlreadyExists();
                bases.Add(baseResponse);
            }
            else
            {

                List<ProductSpecificationMapping> productSpecifications = BindProductSpec(model, productId);
                foreach (var specMap in productSpecifications)
                {
                    specMap.FileName = null;
                    response = helper.ApiCall(CatelogueURL, EndPoints.ProductSpecificationMapping, "POST", specMap);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                    bases.Add(baseResponse);
                }
            }
            return bases;
        }

        public BaseResponse<string> CheckProductExist(ProductDetails model)
        {
            BaseResponse<string> baseR = new BaseResponse<string>();
            baseR.code = 200;
            int count = 0;
            JObject obj = new JObject();
            JObject message = new JObject();
            JArray Amessage = new JArray();
            BaseResponse<Products> ProductbaseResponse = new BaseResponse<Products>();

            //var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?CategoryID=" + model.CategoryId + "&Getparent=" + true, "GET", null);
            //ProductbaseResponse = ProductbaseResponse.JsonParseList(GetResponse);
            //List<Products> tempList = ProductbaseResponse.Data as List<Products>;
            //if (tempList.Where(x => x.CompanySKUCode == model.CompanySKUCode).Any())
            //{
            //    count = 1 + count;
            //    baseR.code = 201;
            //    obj["message"] = "company sku code already exists";
            //    Amessage.Add(obj["message"]);
            //}


            BaseResponse<SellerProduct> SellerProductbaseResponse = new BaseResponse<SellerProduct>();

            var GetResponse1 = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?categoryId=" + model.CategoryId + "&brandId=" + model.SellerProducts.BrandID, "GET", null);
            SellerProductbaseResponse = SellerProductbaseResponse.JsonParseList(GetResponse1);
            List<SellerProduct> tempList1 = SellerProductbaseResponse.Data as List<SellerProduct>;

            if (tempList1.Where(x => x.CompanySKUCode == model.CompanySKUCode).Any())
            {
                count = 1 + count;
                baseR.code = 201;
                obj["message"] = "Company sku code already exists";
                Amessage.Add(obj["message"]);
            }

            if (tempList1.Where(x => x.SKUCode == model.SellerProducts.SellerSKU && x.SellerID == model.SellerProducts.SellerID).Any())
            {
                count = 1 + count;
                baseR.code = 201;
                obj["message"] = "Seller sku code already exists";
                Amessage.Add(obj["message"]);
            }
            message[""] = Amessage;
            baseR.Message = message.ToString();
            return baseR;
        }


        public BaseResponse<string> CheckExistingProducts(ExistingProduct model)
        {
            BaseResponse<string> baseR = new BaseResponse<string>();
            baseR.code = 200;
            int count = 0;
            JObject obj = new JObject();
            JObject message = new JObject();
            JArray Amessage = new JArray();

            BaseResponse<SellerProduct> SellerProductbaseResponse = new BaseResponse<SellerProduct>();

            var GetResponse1 = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?productId=" + model.ProductId + "&sellerId=" + model.SellerId, "GET", null);
            SellerProductbaseResponse = SellerProductbaseResponse.JsonParseList(GetResponse1);
            List<SellerProduct> tempList1 = SellerProductbaseResponse.Data as List<SellerProduct>;

            if (tempList1.Any())
            {
                count = 1 + count;
                baseR.code = 201;
                obj["message"] = "Product already exists";
                Amessage.Add(obj["message"]);
            }
            message[""] = Amessage;
            baseR.Message = message.ToString();
            return baseR;
        }


        public BaseResponse<string> CheckCompanySkuCode(CheckCompanySKUCodeDto model)
        {
            BaseResponse<string> baseR = new BaseResponse<string>();
            baseR.code = 200;
            int count = 0;
            JObject obj = new JObject();
            JObject message = new JObject();
            JArray Amessage = new JArray();

            if (model.CategoryId > 0 && model.BrandID > 0 && !string.IsNullOrEmpty(model.CompanySKUCode))
            {
                BaseResponse<SellerProduct> SellerProductbaseResponse = new BaseResponse<SellerProduct>();

                var GetResponse1 = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?categoryId=" + model.CategoryId + "&brandId=" + model.BrandID, "GET", null);
                SellerProductbaseResponse = SellerProductbaseResponse.JsonParseList(GetResponse1);
                List<SellerProduct> tempList1 = SellerProductbaseResponse.Data as List<SellerProduct>;
                tempList1 = tempList1.Where(x => x.CompanySKUCode.ToLower() == model.CompanySKUCode.ToLower()).ToList();

                if (model.ProductId != null && model.ProductId != 0)
                {
                    tempList1 = tempList1.Where(x => x.ProductID != model.ProductId).ToList();
                }

                if (tempList1.Any())
                {
                    baseR.code = 204;
                    baseR.Message = "Company sku code already exists";
                }
            }
            return baseR;
        }

        public BaseResponse<string> CheckSellerSkuCode(CheckSellerSKUCodeDto model)
        {
            BaseResponse<string> baseR = new BaseResponse<string>();
            baseR.code = 200;
            int count = 0;
            JObject obj = new JObject();
            JObject message = new JObject();
            JArray Amessage = new JArray();

            if (model.CategoryId > 0 && model.BrandID > 0 && !string.IsNullOrEmpty(model.SellerSKUCode))
            {
                BaseResponse<SellerProduct> SellerProductbaseResponse = new BaseResponse<SellerProduct>();

                var GetResponse1 = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?categoryId=" + model.CategoryId + "&brandId=" + model.BrandID, "GET", null);
                SellerProductbaseResponse = SellerProductbaseResponse.JsonParseList(GetResponse1);
                List<SellerProduct> tempList1 = SellerProductbaseResponse.Data as List<SellerProduct>;
                tempList1 = tempList1.Where(x => x.SKUCode.ToLower() == model.SellerSKUCode.ToLower() && x.SellerID == model.SellerID).ToList();

                if (model.SellerProductId != null && model.SellerProductId != 0)
                {
                    tempList1 = tempList1.Where(x => x.Id != model.SellerProductId).ToList();
                }

                if (tempList1.Any())
                {
                    baseR.code = 204;
                    baseR.Message = "Seller sku code already exists";
                }
            }
            return baseR;
        }

    }
}
