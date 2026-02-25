using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.Order;
using API_Gateway.Models.Entity.User;
using Irony.Parsing;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Transactions;

namespace API_Gateway.Common.products
{
    public class UpdateProduct
    {
        public string CatelogueURL = string.Empty;
        public string OrderURL = string.Empty;
        private readonly IConfiguration _configuration;
        public string UserId = string.Empty;
        public string UserURL = string.Empty;
        public string IdserverURL = string.Empty;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        public UpdateProduct(IConfiguration configuration, HttpContext httpContext, string Userid)
        {
            UserId = Userid;
            _configuration = configuration;
            _httpContext = httpContext;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            OrderURL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            UserURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            IdserverURL = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        //Get Record an bind specific fields

        public Products BindProduct(ProductDetails model)
        {
            BaseResponse<Products> baseResponse = new BaseResponse<Products>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?Id=" + model.productId, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(GetResponse);

            Products product = baseResponse.Data as Products;

            product.CategoryId = model.CategoryId;
            product.CompanySKUCode = model.CompanySKUCode;
            product.AssiCategoryId = model.AssiCategoryId;
            product.TaxValueId = model.TaxValueId;
            product.HSNCodeId = model.HSNCodeId;
            product.ProductName = model.ProductName;
            product.CustomeProductName = model.CustomeProductName;
            product.Description = model.Description;
            product.Highlights = model.Highlights;
            product.MetaDescription = model.MetaDescription;
            product.MetaTitle = model.MetaTitle;
            product.Keywords = model.Keywords;
            //product.CreatedBy = UserId;
            //product.CreatedAt = DateTime.Now;
            product.ModifiedAt = DateTime.Now;
            product.ModifiedBy = UserId;

            return product;
        }

        public SellerProduct BindSellerProducts(SellerProductDTO? item, int ProductId, int BrandId, bool? IsExistingProduct = null, bool? isDeleted = false, bool? live = null)
        {
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            SellerProduct sellerProduct = new SellerProduct();

            string url = string.Empty;

            if (IsExistingProduct != null)
            {
                url += "&IsExistingProduct=" + IsExistingProduct;
            }

            if (isDeleted != null)
            {
                url += "&isDeleted=" + isDeleted;
            }

            if (live != null)
            {
                url += "&live=" + live;
            }

            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?id=" + item.Id + url, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(GetResponse);
            sellerProduct = baseResponse.Data as SellerProduct;

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
            sellerProduct.ModifiedAt = DateTime.Now;
            sellerProduct.ModifiedBy = UserId;


            sellerKyc seller = new sellerKyc(_configuration, _httpContext);
            var sellerdetails = seller.BindSeller(item.SellerID);

            BaseResponse<AssignBrandToSeller> AssibaseResponse = new BaseResponse<AssignBrandToSeller>();
            AssignBrands AssignbrandDetails = new AssignBrands(UserURL, _httpContext, CatelogueURL, IdserverURL, _configuration);
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
            sellerProduct.Id = (int)model.SellerProductId;
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



            sellerKyc seller = new sellerKyc(_configuration, _httpContext);
            var sellerdetails = seller.BindSeller(model.SellerId);

            BaseResponse<AssignBrandToSeller> AssibaseResponse = new BaseResponse<AssignBrandToSeller>();
            AssignBrands AssignbrandDetails = new AssignBrands(UserURL, _httpContext, CatelogueURL, IdserverURL, _configuration);
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

        public SellerProduct BindSingleSellerProduct(SellerProductDTO? sellerProducts, int ProductId, int BrandId)
        {
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?id=" + sellerProducts.Id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(GetResponse);

            SellerProduct sellerProduct = new SellerProduct();
            sellerProduct = baseResponse.Data as SellerProduct;

            sellerProduct.ProductID = ProductId;
            sellerProduct.SellerID = sellerProducts.SellerID;
            sellerProduct.BrandID = BrandId;
            sellerProduct.SKUCode = sellerProducts.SellerSKU;
            sellerProduct.IsSizeWisePriceVariant = sellerProducts.IsSizeWisePriceVariant;
            sellerProduct.IsExistingProduct = sellerProducts.IsExistingProduct;
            sellerProduct.Live = sellerProducts.Live;
            sellerProduct.Status = sellerProducts.Status;
            sellerProduct.ManufacturedDate = null;
            sellerProduct.ExpiryDate = null;
            sellerProduct.MOQ = null;
            sellerProduct.ModifiedAt = DateTime.Now;
            sellerProduct.ModifiedBy = UserId;

            sellerKyc seller = new sellerKyc(_configuration, _httpContext);
            var sellerdetails = seller.BindSeller(sellerProducts.SellerID);

            BaseResponse<AssignBrandToSeller> AssibaseResponse = new BaseResponse<AssignBrandToSeller>();
            AssignBrands AssignbrandDetails = new AssignBrands(UserURL, _httpContext, CatelogueURL, IdserverURL, _configuration);
            AssibaseResponse = AssignbrandDetails.GetAssignBrandsByUserIDandBrandId(sellerProducts.SellerID, BrandId, null);

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

            var SellerDetails = new { FullName = sellerdetails.FullName, Email = sellerdetails.UserName, PhoneNo = sellerdetails.PhoneNumber, Display = sellerdetails.DisplayName, LegalName = legalName, TradeName = tradeName, SellerId = sellerProducts.SellerID, SellerSignature = sellerdetails.DigitalSign, ShipmentBy = sellerdetails.ShipmentBy, ShipmentChargesPaidBy = sellerdetails.ShipmentChargesPaidBy, ShipmentChargesPaidByName = sellerdetails.ShipmentChargesPaidByName, SellerStatus = sellerdetails.SellerStatus, KycStatus = sellerdetails.Status };
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
            BaseResponse<ProductPrice> baseResponse = new BaseResponse<ProductPrice>();
            List<ProductPrice> pp = new List<ProductPrice>();
            bool setMarginProductLevel = Convert.ToBoolean(_configuration.GetValue<string>("allow_set_margin_on_product_level"));
            foreach (var item in ProductPrices)
            {
                ProductPrice productPrice = new ProductPrice();

                string url = string.Empty;
                
                if(item.SizeID != null && item.SizeID != 0)
                {
                    url += "&SizeID=" + item.SizeID;
                }

                if (item.Id != null && item.Id != 0)
                {
                    url += "&ID=" + item.Id;
                }

                var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductID=" + SellerProductId + url, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(GetResponse);

                if (baseResponse.code != 200)
                {
                    GetResponse = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductID=" + SellerProductId + "&Isdeleted=" + true + url, "GET", null);
                    baseResponse = baseResponse.JsonParseRecord(GetResponse);
                }
                if (baseResponse.code == 200)
                {
                    productPrice = baseResponse.Data as ProductPrice;


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
                    productPrice.ModifiedAt = DateTime.Now;
                    productPrice.ModifiedBy = UserId;
                    productPrice.DeletedAt = null;
                    productPrice.DeletedBy = null;
                    productPrice.IsDeleted = false;
                    pp.Add(productPrice);
                }
                else
                {
                    productPrice.SellerProductID = SellerProductId;
                    productPrice.MRP = item.MRP;
                    productPrice.SellingPrice = item.SellingPrice;
                    productPrice.Discount = item.Discount;
                    productPrice.Quantity = item.Quantity;
                    productPrice.SizeID = item.SizeID;
                    productPrice.CreatedAt = DateTime.Now;
                    productPrice.CreatedBy = UserId;
                }
            }
            return pp;
        }

        public List<ProductWareHouse> BindProductWarehouses(IEnumerable<ProductWarehouseDTO> ProductWarehouses, int ProductId, int SellerPriceId, int SellerProductId)
        {
            List<ProductWareHouse> productWareHouses = new List<ProductWareHouse>();
            foreach (var item in ProductWarehouses)
            {
                BaseResponse<ProductWareHouse> baseResponse = new BaseResponse<ProductWareHouse>();
                ProductWareHouse productWareHouse = new ProductWareHouse();

                var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse + "?sellerwiseproductpricemasterid=" + SellerPriceId + "&productId=" + ProductId + "&warehouseid=" + item.WarehouseId, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(GetResponse);

                if (baseResponse.code != 200)
                {
                    GetResponse = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse + "?Id=" + item.Id + "&isDeleted=" + true, "GET", null);
                    baseResponse = baseResponse.JsonParseRecord(GetResponse);
                }
                if (baseResponse.code == 200)
                {
                    productWareHouse = baseResponse.Data as ProductWareHouse;


                    productWareHouse.ProductID = ProductId;
                    productWareHouse.WarehouseID = item.WarehouseId;
                    productWareHouse.WarehouseName = item.WarehouseName;
                    productWareHouse.ProductQuantity = item.Quantity;
                    productWareHouse.SellerWiseProductPriceMasterID = SellerPriceId;
                    productWareHouse.SellerProductID = SellerProductId;
                    productWareHouse.ModifiedAt = DateTime.Now;
                    productWareHouse.ModifiedBy = UserId;
                    productWareHouse.DeletedAt = null;
                    productWareHouse.DeletedBy = null;
                    productWareHouse.IsDeleted = false;

                    productWareHouses.Add(productWareHouse);
                }
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


        public BaseResponse<string> SaveData(ProductDetails model)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    BaseResponse<string> baseR1 = CheckProductExist(model);
                    if (baseR1.code != 200)
                    {
                        return baseR1;
                    }
                    // ProductTable
                    var productTableEntry = ProductTableEntry(model);

                    int ProductId = (int)model.productId;
                    string sellerId = model.SellerProducts.SellerID;
                    int sellerProductId = model.SellerProducts.Id;

                    var SellerProducts = SellerProductsEntries(model.SellerProducts, ProductId, model.BrandID);


                    //var ProductPriceIds = model.SellerProducts.ProductPrices.Select(x => x.Id).ToList();
                    List<ProductPriceDTO> ProductPriceDTO = model.SellerProducts.ProductPrices.Where(x => x.Id == 0 || x.Id == null).ToList();
                    List<ProductPrice> pps = BindProductPrices(ProductPriceDTO, sellerProductId);
                    foreach (var pp in pps)
                    {
                        BaseResponse<ProductPrice> PbaseResponse = new BaseResponse<ProductPrice>();
                        
                        if (pp.Id != null && pp.Id != 0)
                        {
                            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster, "PUT", pp);
                        }
                        else
                        {
                            SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, UserId);
                            PbaseResponse = saveProduct.SaveProductPrice(pp, model.SellerProducts.ProductPrices, sellerProductId, ProductId);
                            if (PbaseResponse.code == 200)
                            {
                                var pricedata = model.SellerProducts.ProductPrices.Where(x => x.SizeID == pp.SizeID).FirstOrDefault();
                                pricedata.Id = PbaseResponse.Data != null ? Convert.ToInt32(PbaseResponse.Data) : 0;
                            }
                        }
                    }
                    //for (int j = 0; j < pps.Count; j++)
                    //{
                        
                    //    if (ProductPriceIds[j] == 0 || ProductPriceIds[j] == null)
                    //    {
                    //        SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, UserId);
                    //        var newPriceRes = saveProduct.ProductPricesEntries(model.SellerProducts.ProductPrices.Where(x => x.Id == 0 || x.Id == null).ToList(), sellerProductId, ProductId);

                    //        var newPrice = newPriceRes.FirstOrDefault();
                    //        if (newPrice != null)
                    //        {
                    //            int? ieg = newPrice.Data as int?;
                    //            ProductPriceIds[j] = (int)ieg;
                    //        }
                    //    }
                    //}

                    var ProductPrices = ProductPricesEntries(model.SellerProducts.ProductPrices, sellerProductId, ProductId);

                    //for (int j = 0; j < ProductPriceIds.Count; j++)
                    //{
                    //    var ProductWarehousesIds = model.SellerProducts.ProductPrices.ElementAt(j).ProductWarehouses.Select(x => x.Id).ToList();

                    //    for (int k = 0; k < ProductWarehousesIds.Count; k++)
                    //    {
                    //        if (ProductWarehousesIds[k] == 0)
                    //        {
                    //            SaveProduct saveProduct = new SaveProduct(_configuration, Token, UserId);
                    //            var newWarehouseRes = saveProduct.ProductWarehousesEntries(model.SellerProducts.ProductPrices.ElementAt(j).ProductWarehouses.Where(x => x.Id == 0).ToList(), ProductId, ProductPriceIds.ElementAt(j), sellerProductId);

                    //            var newWarehouse = newWarehouseRes.FirstOrDefault();
                    //            if (newWarehouse != null)
                    //            {
                    //                int? weg = newWarehouse.Data as int?;
                    //                ProductWarehousesIds[k] = (int)weg;
                    //            }
                    //        }
                    //    }
                    //    var ProductWarehouses = ProductWarehousesEntries(model.SellerProducts.ProductPrices.ElementAt(j).ProductWarehouses, ProductId, ProductPriceIds.ElementAt(j), sellerProductId);


                    //}



                    // ProductImagesTableEntries (Multiple)
                    var productImagesTableEntries = ProductImageEntries(model, ProductId);

                    List<ProductImageDTO> ProductImages = new List<ProductImageDTO>();
                    ProductImages = model.ProductImage.ToList();
                    ProductImages = ProductImages.Where(t => t.Type.ToLower() == "image").ToList();

                    foreach (var item in ProductImages)
                    {
                        if (item.Id == null || item.Id == 0)
                        {
                            if (!string.IsNullOrEmpty(item.Url))
                            {
                                ImageUpload imageUpload = new ImageUpload(_configuration);
                                bool IsUploaded = imageUpload.UploadDocs("ProductImage", item.Url);
                            }
                        }
                    }

                    // ProductSpecificationsEntries (Multiple)
                    var productSpecTableEntries = ProductSpecEntries(model, ProductId);

                    // ProductVideoLinksEntries (Multiple)
                    //var productVideoEntries = ProductVideoLinkEntries(model, ProductId);

                    var productColorEntries = ProductColorEntries(model, ProductId);


                    //productRollback.CheckAndRollback(productTableEntry, sellerProductTableEntry, PriceMasterTableEntry, ProductWarehouseTableEntries, productVideoEntries, productImagesTableEntries, productColorEntries);

                    // Commit the transaction
                    transaction.Complete();
                    BaseResponse<string> baseR = new BaseResponse<string>();
                    baseR.code = 200;
                    baseR.Message = "Product Data Updated Successfully";


                    return baseR;
                }
                catch (Exception ex)
                {
                    // If any API call fails, roll back by calling the rollbackAPI
                    //await RollbackAPI(data);
                    BaseResponse<string> baseR = new BaseResponse<string>();
                    baseR.code = 200;
                    baseR.Message = ex.Message;
                    return baseR;
                }
            }
        }

        public string SaveExistingData(ExistingProduct model)
        {
            var SellerProducts = SellerExistingProductsEntry(model);

            int SellerProductId = (int)model.SellerProductId;

            //var ProductPriceIds = model.ProductPrices.Select(x => x.Id).ToList();

            //for (int j = 0; j < ProductPriceIds.Count; j++)
            //{

            //    if (ProductPriceIds[j] == 0 || ProductPriceIds[j] == null)
            //    {
            //        SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, UserId);
            //        var newPriceRes = saveProduct.ProductPricesEntries(model.ProductPrices.Where(x => x.Id == 0 || x.Id == null).ToList(), SellerProductId, model.ProductId);

            //        var newPrice = newPriceRes.FirstOrDefault();
            //        int? ieg = newPrice.Data as int?;
            //        ProductPriceIds[j] = (int)ieg;
            //    }
            //    var ProductPrices = ProductPricesEntries(model.ProductPrices, SellerProductId, model.ProductId);

            //    //var ProductWarehousesIds = model.ProductPrices.ElementAt(j).ProductWarehouses.Select(x => x.Id).ToList();

            //    //for (int k = 0; k < ProductWarehousesIds.Count; k++)
            //    //{
            //    //    if (ProductWarehousesIds[k] == 0)
            //    //    {
            //    //        SaveProduct saveProduct = new SaveProduct(_configuration, Token, UserId);
            //    //        var newWarehouseRes = saveProduct.ProductWarehousesEntries(model.ProductPrices.ElementAt(j).ProductWarehouses.Where(x => x.Id == 0).ToList(), model.ProductId, ProductPriceIds.ElementAt(j), SellerProductId);

            //    //        var newWarehouse = newWarehouseRes.FirstOrDefault();
            //    //        int? weg = newWarehouse.Data as int?;
            //    //        ProductWarehousesIds[k] = (int)weg;
            //    //    }
            //    //    //var ProductWarehouses = ProductWarehousesEntries(model.ProductPrices.ElementAt(j).ProductWarehouses, model.ProductId, ProductPriceIds.ElementAt(j), SellerProductId);
            //    //}
            //}
            List<ProductPriceDTO> ProductPriceDTO = model.ProductPrices.Where(x => x.Id == 0 || x.Id == null).ToList();
            List<ProductPrice> pps = BindProductPrices(ProductPriceDTO, Convert.ToInt32(model.SellerProductId));
            foreach (var pp in pps)
            {
                BaseResponse<ProductPrice> PbaseResponse = new BaseResponse<ProductPrice>();
                if (pp.Id != null && pp.Id != 0)
                {
                    var response = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster, "PUT", pp);
                }
                else
                {
                    SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, UserId);
                    PbaseResponse = saveProduct.SaveProductPrice(pp, model.ProductPrices, Convert.ToInt32(model.SellerProductId), model.ProductId);
                    if (PbaseResponse.code == 200)
                    {
                        var pricedata = model.ProductPrices.Where(x => x.SizeID == pp.SizeID).FirstOrDefault();
                        pricedata.Id = PbaseResponse.Data != null ? Convert.ToInt32(PbaseResponse.Data) : 0;
                    }
                }
            }
            var ProductPrices = ProductPricesEntries(model.ProductPrices, Convert.ToInt32(model.SellerProductId), model.ProductId);



            return "Product Data Updated Successfully";
        }

        public string SaveData(ProductDetails model, string SellerId)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    BaseResponse<string> baseR = CheckProductExist(model);
                    if (baseR.code != 200)
                    {
                        return baseR.Message;
                    }
                    // ProductTable
                    var productTableEntry = ProductTableEntry(model);

                    int ProductId = Convert.ToInt32(productTableEntry.Data);

                    var SellerProducts = SingleSellerProductsEntry(model.SellerProducts, ProductId, model.BrandID);

                    int SellerProductId = Convert.ToInt32(SellerProducts.Data);


                    //var ProductPrices = ProductPricesEntries(model.SellerProducts.ProductPrices, SellerProductId);

                    //var ProductPriceIds = model.SellerProducts.ProductPrices.Select(x => x.Id).ToList();

                    //for (int j = 0; j < ProductPrices.Count; j++)
                    //{
                    //    var ProductWarehouses = ProductWarehousesEntries(model.SellerProducts.ProductPrices.ElementAt(j).ProductWarehouses, ProductId, ProductPriceIds.ElementAt(j), SellerProductId);
                    //}

                    List<ProductPriceDTO> ProductPriceDTO = model.SellerProducts.ProductPrices.Where(x => x.Id == 0 || x.Id == null).ToList();
                    List<ProductPrice> pps = BindProductPrices(ProductPriceDTO, SellerProductId);
                    foreach (var pp in pps)
                    {
                        BaseResponse<ProductPrice> PbaseResponse = new BaseResponse<ProductPrice>();

                        if (pp.Id != null && pp.Id != 0)
                        {
                            var response = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster, "PUT", pp);
                        }
                        else
                        {
                            SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, UserId);
                            PbaseResponse = saveProduct.SaveProductPrice(pp, model.SellerProducts.ProductPrices, SellerProductId, ProductId);
                            if (PbaseResponse.code == 200)
                            {
                                var pricedata = model.SellerProducts.ProductPrices.Where(x => x.SizeID == pp.SizeID).FirstOrDefault();
                                pricedata.Id = PbaseResponse.Data != null ? Convert.ToInt32(PbaseResponse.Data) : 0;
                            }
                        }
                    }
                    var ProductPrices = ProductPricesEntries(model.SellerProducts.ProductPrices, SellerProductId, ProductId);


                    //var ProductPriceIds = model.SellerProducts.ProductPrices.Select(x => x.Id).ToList();

                    //for (int j = 0; j < ProductPriceIds.Count; j++)
                    //{

                    //    if (ProductPriceIds[j] == 0 || ProductPriceIds[j] == null)
                    //    {
                    //        SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, UserId);
                    //        var newPriceRes = saveProduct.ProductPricesEntries(model.SellerProducts.ProductPrices.Where(x => x.Id == 0 || x.Id == null).ToList(), SellerProductId, ProductId);

                    //        var newPrice = newPriceRes.FirstOrDefault();
                    //        int? ieg = newPrice.Data as int?;
                    //        ProductPriceIds[j] = (int)ieg;
                    //    }
                    //}

                    //var ProductPrices = ProductPricesEntries(model.SellerProducts.ProductPrices, SellerProductId, ProductId);

                    //for (int j = 0; j < ProductPriceIds.Count; j++)
                    //{
                    //    var ProductWarehousesIds = model.SellerProducts.ProductPrices.ElementAt(j).ProductWarehouses.Select(x => x.Id).ToList();

                    //    for (int k = 0; k < ProductWarehousesIds.Count; k++)
                    //    {
                    //        if (ProductWarehousesIds[k] == 0)
                    //        {
                    //            SaveProduct saveProduct = new SaveProduct(_configuration, Token, UserId);
                    //            var newWarehouseRes = saveProduct.ProductWarehousesEntries(model.SellerProducts.ProductPrices.ElementAt(j).ProductWarehouses.Where(x => x.Id == 0).ToList(), ProductId, ProductPriceIds.ElementAt(j), SellerProductId);

                    //            var newWarehouse = newWarehouseRes.FirstOrDefault();
                    //            int? weg = newWarehouse.Data as int?;
                    //            ProductWarehousesIds[k] = (int)weg;
                    //        }
                    //    }
                    //    var ProductWarehouses = ProductWarehousesEntries(model.SellerProducts.ProductPrices.ElementAt(j).ProductWarehouses, ProductId, ProductPriceIds.ElementAt(j), SellerProductId);


                    //}

                    // ProductImagesTableEntries (Multiple)
                    var productImagesTableEntries = ProductImageEntries(model, ProductId);

                    // ProductSpecificationsEntries (Multiple)
                    var productSpecTableEntries = ProductSpecEntries(model, ProductId);


                    // ProductVideoLinksEntries (Multiple)
                    //var productVideoEntries = ProductVideoLinkEntries(model, ProductId);

                    var productColorEntries = ProductColorEntries(model, ProductId);



                    //productRollback.CheckAndRollback(productTableEntry, sellerProductTableEntry, PriceMasterTableEntry, ProductWarehouseTableEntries, productVideoEntries, productImagesTableEntries, productColorEntries);

                    // Commit the transaction
                    transaction.Complete();

                    return "Product Data Updated Successfully";
                }
                catch (Exception ex)
                {
                    // If any API call fails, roll back by calling the rollbackAPI
                    //await RollbackAPI(data);

                    return ex.Message;
                }
            }
        }

        public BaseResponse<Products> ProductTableEntry(ProductDetails model)
        {
            BaseResponse<Products> baseResponse = new BaseResponse<Products>();
            try
            {
                //var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?CategoryID=" + model.CategoryId + "&Getparent=" + true, "GET", null);
                //baseResponse = baseResponse.JsonParseList(GetResponse);
                //List<Products> tempList = baseResponse.Data as List<Products>;
                //if (tempList.Where(x => x.CompanySKUCode == model.CompanySKUCode && x.Id != model.ParentId).Any())
                //{
                //    baseResponse = baseResponse.AlreadyExists();
                //}
                //else
                //{

                //}

                Products product = BindProduct(model);
                var PostResponse = helper.ApiCall(CatelogueURL, EndPoints.Product, "PUT", product);
                baseResponse = baseResponse.JsonParseInputResponse(PostResponse);

                if (baseResponse.code != 200)
                {
                    throw new Exception("API call failed with response code: " + baseResponse.code);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here or re-throw it
                Console.WriteLine("An error occurred: " + ex.Message);
                throw;
            }

            return baseResponse;
        }


        public BaseResponse<SellerProduct> SellerProductsEntries(SellerProductDTO sellerProduct, int ProductId, int BrandId)
        {
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            var sp = BindSellerProducts(sellerProduct, ProductId, BrandId);

            try
            {
                var response = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct, "PUT", sp);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                if (baseResponse.code != 200)
                {
                    throw new Exception("Error: Response code is not 200");
                }

            }
            catch (Exception ex)
            {
                // Log the exception or handle it in some other way
                Console.WriteLine(ex.Message);
                throw;
            }
            return baseResponse;
        }


        public BaseResponse<SellerProduct> SellerExistingProductsEntry(ExistingProduct model)
        {
            SellerProduct sp = BindSellerProducts(model);
            BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
            try
            {
                var response = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct, "PUT", sp);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                if (baseResponse.code != 200)
                {
                    throw new Exception("Error: SellerExistingProductsEntry:Base response code is not 200");
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the error, display a message to the user, or take other appropriate actions
                Console.WriteLine(ex.Message);
                throw;
            }
            return baseResponse;
        }


        public BaseResponse<string> QuickUpdate(QuickProductUpdateDTO model)
        {
            try
            {
                int sellerProductId = model.SellerProductId;
                int ProductId = model.ProductId;

                BaseResponse<Products> baseResponse = new BaseResponse<Products>();
                BaseResponse<SellerProduct> SellerbaseResponse = new BaseResponse<SellerProduct>();
                SellerProduct sellerProduct = new SellerProduct();
                BaseResponse<string> baseR = new BaseResponse<string>();
                var _GetResponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?id=" + sellerProductId, "GET", null);
                SellerbaseResponse = SellerbaseResponse.JsonParseRecord(_GetResponse);
                if (SellerbaseResponse.code == 200)
                {
                    sellerProduct = SellerbaseResponse.Data as SellerProduct;
                    BaseResponse<SellerListModel> sellerdetailsbaseResponse = new BaseResponse<SellerListModel>();
                    var sellerresponse = helper.ApiCall(IdserverURL, EndPoints.SellerById + "?Id=" + sellerProduct.SellerID, "GET", null);
                    sellerdetailsbaseResponse = sellerdetailsbaseResponse.JsonParseRecord(sellerresponse);
                    SellerListModel sellerListModel = new SellerListModel();
                    if (sellerdetailsbaseResponse.code == 200)
                    {
                        sellerListModel = sellerdetailsbaseResponse.Data as SellerListModel;
                        if (sellerListModel.Status.ToLower() != "archived")
                        {
                            var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?Id=" + ProductId, "GET", null);
                            baseResponse = baseResponse.JsonParseRecord(GetResponse);

                            Products product = baseResponse.Data as Products;

                            product.ProductName = model.ProductName;
                            product.ModifiedAt = DateTime.Now;
                            product.ModifiedBy = UserId;

                            try
                            {
                                var PostResponse = helper.ApiCall(CatelogueURL, EndPoints.Product, "PUT", product);
                                baseResponse = baseResponse.JsonParseInputResponse(PostResponse);

                                if (baseResponse.code != 200)
                                {
                                    throw new Exception("API call failed with response code: " + baseResponse.Message);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Handle the exception here or re-throw it
                                Console.WriteLine("An error occurred: " + ex.Message);
                                throw;
                            }


                            sellerProduct.PackingBreadth = model.PackingBreadth;
                            sellerProduct.PackingHeight = model.PackingHeight;
                            sellerProduct.PackingLength = model.PackingLength;
                            sellerProduct.PackingWeight = model.PackingWeight;
                            sellerProduct.WeightSlabId = model.WeightSlabId;
                            sellerProduct.Status = model.Status;
                            sellerProduct.Live = model.Live;
                            sellerProduct.ModifiedAt = DateTime.Now;
                            sellerProduct.ModifiedBy = UserId;

                            try
                            {
                                var sPostResponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct, "PUT", sellerProduct);
                                SellerbaseResponse = SellerbaseResponse.JsonParseInputResponse(sPostResponse);

                                if (SellerbaseResponse.code != 200)
                                {
                                    throw new Exception("API call failed with response code: " + baseResponse.Message);
                                }
                            }
                            catch (Exception ex)
                            {
                                // Handle the exception here or re-throw it
                                Console.WriteLine("An error occurred: " + ex.Message);
                                throw;
                            }

                            //var ProductPriceIds = model.ProductPrice.Select(x => x.Id).ToList();

                            //for (int j = 0; j < ProductPriceIds.Count; j++)
                            //{

                            //    if (ProductPriceIds[j] == 0 || ProductPriceIds[j] == null)
                            //    {
                            //        SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, UserId);
                            //        var newPriceRes = saveProduct.ProductPricesEntries(model.ProductPrice.Where(x => x.Id == 0 || x.Id == null).ToList(), sellerProductId, ProductId);

                            //        var newPrice = newPriceRes.FirstOrDefault();
                            //        int? ieg = newPrice.Data as int?;
                            //        ProductPriceIds[j] = (int)ieg;
                            //    }
                            //}
                            //var ProductPrices = ProductPricesEntries(model.ProductPrice, sellerProductId, ProductId);


                            List<ProductPriceDTO> ProductPriceDTO = model.ProductPrice.Where(x => x.Id == 0 || x.Id == null).ToList();
                            List<ProductPrice> pps = BindProductPrices(ProductPriceDTO, Convert.ToInt32(model.SellerProductId));
                            foreach (var pp in pps)
                            {
                                BaseResponse<ProductPrice> PbaseResponse = new BaseResponse<ProductPrice>();

                                if (pp.Id != null && pp.Id != 0)
                                {
                                    var response = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster, "PUT", pp);
                                }
                                else
                                {
                                    SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, UserId);
                                    PbaseResponse = saveProduct.SaveProductPrice(pp, model.ProductPrice, Convert.ToInt32(model.SellerProductId), model.ProductId);
                                    if (PbaseResponse.code == 200)
                                    {
                                        var pricedata = model.ProductPrice.Where(x => x.SizeID == pp.SizeID).FirstOrDefault();
                                        pricedata.Id = PbaseResponse.Data != null ? Convert.ToInt32(PbaseResponse.Data) : 0;
                                    }
                                }
                            }
                            var ProductPrices = ProductPricesEntries(model.ProductPrice, Convert.ToInt32(model.SellerProductId), model.ProductId);



                            //for (int j = 0; j < ProductPriceIds.Count; j++)
                            //{
                            //    var ProductWarehousesIds = model.ProductPrice.ElementAt(j).ProductWarehouses.Select(x => x.Id).ToList();

                            //    for (int k = 0; k < ProductWarehousesIds.Count; k++)
                            //    {
                            //        if (ProductWarehousesIds[k] == 0)
                            //        {
                            //            SaveProduct saveProduct = new SaveProduct(_configuration, Token, UserId);
                            //            var newWarehouseRes = saveProduct.ProductWarehousesEntries(model.ProductPrice.ElementAt(j).ProductWarehouses.Where(x => x.Id == 0).ToList(), ProductId, ProductPriceIds.ElementAt(j), sellerProductId);

                            //            var newWarehouse = newWarehouseRes.FirstOrDefault();
                            //            int? weg = newWarehouse.Data as int?;
                            //            ProductWarehousesIds[k] = (int)weg;
                            //        }
                            //        var ProductWarehouses = ProductWarehousesEntries(model.ProductPrice.ElementAt(j).ProductWarehouses, ProductId, ProductPriceIds.ElementAt(j), sellerProductId);
                            //    }


                            //}

                            //var res = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductID=" + model.SellerProductID, "GET", null);
                            //BaseResponse<ProductPrice> baseResponse = new BaseResponse<ProductPrice>();
                            //baseResponse = baseResponse.JsonParseList(res);

                            //if (baseResponse.code != 200)
                            //{
                            //    throw new Exception("ProductPrice didn't bind successfully" + baseResponse.code);
                            //}

                            //List<ProductPrice> productPrices = baseResponse.Data as List<ProductPrice>;

                            //foreach (var item in model.ProductPrice)
                            //{
                            //    var price = productPrices.Where(x => x.Id == item.Id).FirstOrDefault();
                            //    price.SellingPrice = item.SellingPrice;
                            //    price.MRP = item.MRP;
                            //    price.Quantity = item.Quantity;
                            //    price.Discount = item.Discount;

                            //    res = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster, "PUT", price);

                            //    res = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse + "?SellerWiseProductPriceMasterID=" + price.Id, "GET", null);
                            //    BaseResponse<ProductWareHouse> baseResponse1 = new BaseResponse<ProductWareHouse>();
                            //    baseResponse1 = baseResponse1.JsonParseList(res);
                            //    if (baseResponse1.code != 200)
                            //    {
                            //        throw new Exception("ProductWarehouse List didn't bind successfully" + baseResponse.code);
                            //    }
                            //    List<ProductWareHouse> productWareHouses = baseResponse1.Data as List<ProductWareHouse>;

                            //    foreach (var data in item.ProductWarehouses)
                            //    {
                            //        var wareHouse = productWareHouses.Where(x => x.Id == data.Id).FirstOrDefault();
                            //        wareHouse.ProductQuantity = data.Quantity;

                            //        res = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse, "PUT", wareHouse);
                            //        baseResponse1 = baseResponse1.JsonParseInputResponse(res);
                            //        if (baseResponse1.code != 200)
                            //        {
                            //            throw new Exception("ProductWarehouse Update Unsuccessful" + baseResponse.code);
                            //        }
                            //    }
                            //}


                            baseR.code = 200;
                            baseR.Message = "Product Data Updated Successfully";
                        }
                        else
                        {
                            baseR.code = 204;
                            baseR.Message = "You can't change product status because the seller is archived.";
                        }
                    }
                    else
                    {
                        baseR.code = 204;
                        baseR.Message = "Seller not found.";
                    }
                }
                else
                {
                    baseR.code = 204;
                    baseR.Message = "Seller product not found.";
                }
                return baseR;
            }
            catch (Exception ex)
            {
                // Log the exception
                // ...

                BaseResponse<string> baseR = new BaseResponse<string>();
                baseR.code = 500;
                baseR.Message = "An error occurred while updating product data.";

                return baseR;
            }
        }


        public BaseResponse<SellerProduct> SingleSellerProductsEntry(SellerProductDTO? sellerProducts, int ProductId, int BrandId)
        {
            try
            {
                SellerProduct sp = BindSingleSellerProduct(sellerProducts, ProductId, BrandId);
                BaseResponse<SellerProduct> baseResponse = new BaseResponse<SellerProduct>();
                var response = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct, "PUT", sp);
                baseResponse = baseResponse.JsonParseInputResponse(response);

                if (baseResponse.code != 200)
                {
                    throw new Exception($"Unexpected status code {baseResponse.code} received from API");
                }

                return baseResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<BaseResponse<ProductPrice>> ProductPricesEntries(IEnumerable<ProductPriceDTO> Productprices, int SellerProductId, int productId)
        {
            List<BaseResponse<ProductPrice>> bases = new List<BaseResponse<ProductPrice>>();
            List<ProductPrice> pps = BindProductPrices(Productprices, SellerProductId);
            var UpdateRecordID = pps.Select(x => x.Id).ToList();

            BaseResponse<ProductPrice> productDeletable = new BaseResponse<ProductPrice>();
            var resp = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?SellerProductID=" + SellerProductId, "GET", null);
            productDeletable = productDeletable.JsonParseList(resp);
            List<ProductPrice> deleteProductPrice = productDeletable.Data as List<ProductPrice>;

            List<int> deletePrices = deleteProductPrice.Select(x => x.Id).ToList();

            var prds = deletePrices.Except(UpdateRecordID);

            foreach (int i in prds)
            {
                var resp42 = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster + "?Id=" + i, "DELETE", null);
            }

            foreach (var pp in pps)
            {
                BaseResponse<ProductPrice> baseResponse = new BaseResponse<ProductPrice>();
                try
                {

 
                    BaseResponse<OrderItems> OrderbaseResponse = new BaseResponse<OrderItems>();
                    var GetResponse = helper.ApiCall(OrderURL, EndPoints.OrderItems + "?SellerProductID=" + pp.SellerProductID + "&Status=Placed", "GET", null);
                    OrderbaseResponse = OrderbaseResponse.JsonParseList(GetResponse);
                    List<OrderItems> ordersdetail = OrderbaseResponse.Data as List<OrderItems>;
                    if (ordersdetail != null && ordersdetail.Count > 0)
                    {
                        int ss = ordersdetail.Select(p => p.Qty).Sum();
                        pp.Quantity = pp.Quantity - ss >= 0 ? pp.Quantity - ss : 0;
                    }
                    var response = helper.ApiCall(CatelogueURL, EndPoints.ProductPriceMaster, "PUT", pp);
                    baseResponse = baseResponse.JsonParseInputResponse(response);

                    if (baseResponse.code != 200)
                    {
                        throw new Exception($"Failed to update ProductPrice. Response code: {baseResponse.code}");
                    }

                    bool AllowWarehouseManagement = Convert.ToBoolean(_configuration.GetValue<string>("Allow_warehouse_management"));

                    if (baseResponse.code == 200)
                    {
                        var data = Productprices.Where(x => x.SellerProductId == pp.SellerProductID && x.SizeID == pp.SizeID).ToList();
                        if (data.Count > 0)
                        {
                            var priceid = deleteProductPrice.Where(x => x.SizeID == pp.SizeID).FirstOrDefault();
                            if (AllowWarehouseManagement)
                            {
                                List<ProductWarehouseDTO> warehouseDto = data[0].ProductWarehouses.ToList();
                                var ProductWarehousesIds = data[0].ProductWarehouses.Select(x => x.Id).ToList();
                                for (int k = 0; k < ProductWarehousesIds.Count; k++)
                                {
                                    if (ProductWarehousesIds[k] == 0 || ProductWarehousesIds[k] == null)
                                    {
                                        SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, UserId);
                                        var newWarehouseRes = saveProduct.ProductWarehousesEntries(data[0].ProductWarehouses.Where(x => x.Id == 0 || x.Id == null).ToList(), productId, priceid.Id, SellerProductId);

                                        var newWarehouse = newWarehouseRes.FirstOrDefault();
                                        if (newWarehouse != null)
                                        {
                                            int? weg = newWarehouse.Data as int?;
                                            ProductWarehousesIds[k] = (int)weg;
                                        }
                                    }
                                }
                                var ProductWarehouses = ProductWarehousesEntries(warehouseDto, productId, priceid.Id, SellerProductId);
                            }
                        }
                    }


                    bases.Add(baseResponse);
                }
                catch (Exception ex)
                {
                    // Handle the exception as needed
                    throw new Exception($"Filed to update ProductPrice. {baseResponse.Message}");
                }
            }

            return bases;
        }


        public List<BaseResponse<ProductWareHouse>> ProductWarehousesEntries(IEnumerable<ProductWarehouseDTO> productWarehouses, int ProductId, int ProductPriceId, int SellerProductId)
        {
            List<BaseResponse<ProductWareHouse>> bases = new List<BaseResponse<ProductWareHouse>>();
            List<ProductWareHouse> pws = BindProductWarehouses(productWarehouses, ProductId, ProductPriceId, SellerProductId);
            var UpdateRecordID = pws.Select(x => x.Id).ToList();


            BaseResponse<ProductWareHouse> productDeletable = new BaseResponse<ProductWareHouse>();
            var resp = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse + "?sellerwiseproductpricemasterid=" + ProductPriceId, "GET", null);
            productDeletable = productDeletable.JsonParseList(resp);
            List<ProductWareHouse> deleteProductWarehouse = productDeletable.Data as List<ProductWareHouse>;

            List<int> deleteProductWarehouses = deleteProductWarehouse.Select(x => x.Id).ToList();

            var prds = deleteProductWarehouses.Except(UpdateRecordID);

            foreach (int i in prds)
            {
                var resp42 = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse + "?Id=" + i, "DELETE", null);
            }

            try
            {
                foreach (var pw in pws)
                {
                    BaseResponse<ProductWareHouse> baseResponse = new BaseResponse<ProductWareHouse>();
                    var response = helper.ApiCall(CatelogueURL, EndPoints.ProductWarehouse, "PUT", pw);
                    baseResponse = baseResponse.JsonParseInputResponse(response);

                    if (baseResponse.code != 200)
                    {
                        throw new Exception("Failed to update product warehouse entry. Response code: " + baseResponse.code);
                    }

                    bases.Add(baseResponse);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception here
                // You can log the error message or re-throw the exception
                throw ex;
            }

            return bases;
        }


        public List<BaseResponse<ProductImages>> ProductImageEntries(ProductDetails model, int productId)
        {
            List<BaseResponse<ProductImages>> bases = new List<BaseResponse<ProductImages>>();
            BaseResponse<ProductImages> baseResponse = new BaseResponse<ProductImages>();
            try
            {
                var response = helper.ApiCall(CatelogueURL, EndPoints.ProductsImage + "?ProductID=" + productId, "GET", null);
                baseResponse = baseResponse.JsonParseList(response);
                List<ProductImages> tempList = baseResponse.Data as List<ProductImages>;
                List<ProductImages> productImages = BindProductImage(model, productId);

                if (tempList.Any())
                {
                    List<ProductImages> matchingItems = tempList.Where(tempItem => !productImages.Any(pi => pi.Url == tempItem.Url)).ToList();

                    foreach (var item in matchingItems)
                    {
                        ImageUpload imageUpload = new ImageUpload(_configuration);
                        imageUpload.RemoveDocFile(item.Url, "ProductImage");

                    }
                    response = helper.ApiCall(CatelogueURL, EndPoints.ProductsImage + "?ProductID=" + productId, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }

                foreach (var image in productImages)
                {
                    response = helper.ApiCall(CatelogueURL, EndPoints.ProductsImage, "POST", image);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                    if (baseResponse.code != 200)
                    {
                        throw new Exception("Unexpected response code: " + baseResponse.code);
                    }
                    bases.Add(baseResponse);
                }
            }
            catch (Exception ex)
            {
                // handle the exception as needed, e.g. log it or rethrow it
                // you could also add a custom error message to the exception if desired
                throw ex;
            }
            return bases;
        }


        //public List<BaseResponse<ProductVideoLink>> ProductVideoLinkEntries(ProductDetails model, int ProductId)
        //{
        //    List<BaseResponse<ProductVideoLink>> bases = new List<BaseResponse<ProductVideoLink>>();
        //    BaseResponse<ProductVideoLink> baseResponse = new BaseResponse<ProductVideoLink>();
        //    var temp = helper.ApiCall(CatelogueURL, EndPoints.ProductsVideoLinks + "?productID=" + ProductId, "GET", null);
        //    baseResponse = baseResponse.JsonParseList(temp);
        //    List<ProductVideoLink> tempList = baseResponse.Data as List<ProductVideoLink>;

        //    if (tempList.Any())
        //    {
        //        var response = helper.ApiCall(CatelogueURL, EndPoints.ProductsVideoLinks + "?productID=" + ProductId, "DELETE", null);
        //        baseResponse = baseResponse.JsonParseInputResponse(response);
        //    }
        //    List<ProductVideoLink> productVideoLinks = BindProductVideoLinks(model, ProductId);
        //    foreach (var link in productVideoLinks)
        //    {
        //        try
        //        {
        //            temp = helper.ApiCall(CatelogueURL, EndPoints.ProductsVideoLinks, "POST", link);
        //            baseResponse = baseResponse.JsonParseInputResponse(temp);
        //            if (baseResponse.code != 200)
        //            {
        //                throw new Exception("Failed to add product video link. Error code: " + baseResponse.code);
        //            }
        //            bases.Add(baseResponse);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw;
        //            Console.WriteLine(ex.Message);
        //        }
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
                        throw new Exception("Error deleting product color mapping. Response code: " + baseResponse.code);
                    }
                }
                List<ProductColorMapp> colors = BindProductColorMapping(model, ProductId);
                foreach (var link in colors)
                {
                    temp = helper.ApiCall(CatelogueURL, EndPoints.ProductColorMapping, "POST", link);
                    baseResponse = baseResponse.JsonParseInputResponse(temp);
                    if (baseResponse.code != 200)
                    {
                        throw new Exception("Error adding product color mapping. Response code: " + baseResponse.code);
                    }
                    bases.Add(baseResponse);
                }
            }
            catch (Exception ex)
            {
                // Log the exception here or re-throw it to be handled by the caller.
                throw ex;
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
            //BaseResponse<Products> ProductbaseResponse = new BaseResponse<Products>();

            //var GetResponse = helper.ApiCall(CatelogueURL, EndPoints.Product + "?CategoryID=" + model.CategoryId + "&Getparent=" + true, "GET", null);
            //ProductbaseResponse = ProductbaseResponse.JsonParseList(GetResponse);
            //List<Products> tempList = ProductbaseResponse.Data as List<Products>;
            //if (tempList.Where(x => x.CompanySKUCode == model.CompanySKUCode && x.Id != model.ParentId).Any())
            //{
            //    count = 1 + count;
            //    baseR.code = 201;
            //    obj["message"] = "company sku code already exists";
            //    Amessage.Add(obj["message"]);
            //}


            BaseResponse<SellerProduct> SellerProductbaseResponse = new BaseResponse<SellerProduct>();

            var GetResponse1 = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?categoryId=" + model.CategoryId + "&brandId=" + model.SellerProducts.BrandID + "&isProductExist=" + false, "GET", null);
            SellerProductbaseResponse = SellerProductbaseResponse.JsonParseList(GetResponse1);
            List<SellerProduct> tempList1 = SellerProductbaseResponse.Data as List<SellerProduct>;

            if (tempList1.Where(x => x.CompanySKUCode == model.CompanySKUCode && x.Id != model.SellerProducts.Id).Any())
            {
                count = 1 + count;
                baseR.code = 201;
                obj["message"] = "Company sku code already exists";
                Amessage.Add(obj["message"]);
            }

            if (tempList1.Where(x => x.SKUCode == model.SellerProducts.SellerSKU && x.SellerID == model.SellerProducts.SellerID && x.Id != model.SellerProducts.Id).Any())
            {
                count = 1 + count;
                baseR.code = 201;
                obj["message"] = "seller sku code already exists";
                Amessage.Add(obj["message"]);
            }
            message[""] = Amessage;
            baseR.Message = message.ToString();
            return baseR;
        }

        public List<ProductSpecificationMapping> BindProductSpec(ProductDetails model, int productId)
        {
            List<ProductSpecificationMapping> specs = new List<ProductSpecificationMapping>();

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
                response = helper.ApiCall(CatelogueURL, EndPoints.ProductSpecificationMapping + "?ProductID=" + productId, "DELETE", null);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                //baseResponse = baseResponse.AlreadyExists();
                //bases.Add(baseResponse);
            }


            List<ProductSpecificationMapping> productSpecifications = BindProductSpec(model, productId);
            foreach (var specMap in productSpecifications)
            {
                specMap.FileName = null;
                response = helper.ApiCall(CatelogueURL, EndPoints.ProductSpecificationMapping, "POST", specMap);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                bases.Add(baseResponse);
            }
            return bases;
        }
    }
}
