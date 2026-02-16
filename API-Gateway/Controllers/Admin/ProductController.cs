using API_Gateway.Common;
using API_Gateway.Common.products;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using ClosedXML.Excel;

using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

using DocumentFormat.OpenXml.EMMA;

using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace API_Gateway.Controllers.Admin

{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public static string CatalogueUrl = string.Empty;
        public static string IdentityServerUrl = string.Empty;
        public static string userUrl = string.Empty;
        private PriceCalculation _priceCalculation;
        private ImageUpload imageUpload;
        BaseResponse<Products> baseResponse = new BaseResponse<Products>();
        private ApiHelper helper;
        public ProductController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, PriceCalculation priceCalculation)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;

            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IdentityServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            userUrl = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            helper = new ApiHelper(_httpContext);
            imageUpload = new ImageUpload(_configuration);
            _priceCalculation = priceCalculation;
            helper = new ApiHelper(_httpContext);

        }

        [HttpPost("Product")]
        [Authorize]
        public ActionResult<ApiHelper> SaveProduct(ProductDetails model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, userID);
            var res = saveProduct.SaveData(model);
            return Ok(res);
        }

        [HttpPut("Product")]
        [Authorize]
        public ActionResult<ApiHelper> UpdateProduct(ProductDetails model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            UpdateProduct updateProduct = new UpdateProduct(_configuration, _httpContext, userID);
            var res = updateProduct.SaveData(model);
            return Ok(res);
        }

        [HttpPut("QuickUpdate")]
        [Authorize]
        public ActionResult<ApiHelper> QuickUpdate(QuickProductUpdateDTO model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            UpdateProduct updateProduct = new UpdateProduct(_configuration, _httpContext, userID);
            var res = updateProduct.QuickUpdate(model);
            return Ok(res);
        }


        [HttpDelete("Product")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> DeleteProduct(int productId, int sellerProductId)
        {
            ProductDelete model = new ProductDelete();
            model.productId = productId;
            model.SellerProductId = sellerProductId;
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            DeleteProduct deleteProduct = new DeleteProduct(_httpContext, userID, CatalogueUrl);
            var res = deleteProduct.DeleteData(model);
            return Ok(res);
        }

        [HttpPut("ArchiveProduct")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> ArchiveProduct(int productId, int sellerProductId)
        {
            ProductDelete model = new ProductDelete();
            model.productId = productId;
            model.SellerProductId = sellerProductId;
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            ArchiveProduct archiveProduct = new ArchiveProduct(_httpContext, userID, CatalogueUrl);
            var res = archiveProduct.ArchiveProductData(model);
            return Ok(res);
        }


        [HttpGet("AllChildProducts")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> GetProductList(bool? isDeleted = null)
        {
            GetProduct getProduct = new GetProduct(_configuration, _httpContext);
            var res = getProduct.Get(isDeleted);
            return Ok(res);
        }

        //[HttpGet("GetSellerProuduct")]
        //[Authorize]
        //public ActionResult<ApiHelper> getOnlySellerProduct(string sellerId)
        //{
        //    string userId = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
        //    GetSellerProduct Product = new GetSellerProduct( _configuration, token);
        //    var sellerProduct = Product.Get(sellerId);
        //    return Ok(sellerProduct);
        //}


        [HttpGet("ById")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult GetById(int productId, bool? isDeleted = null, bool? isArchive = null)
        {
            GetProduct product = new GetProduct(_configuration, _httpContext);

            return Ok(product.Get(productId, isDeleted, isArchive));

        }

        [HttpGet("ByMasterid")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult GetByMasterid(int productId)
        {
            GetProduct product = new GetProduct(_configuration, _httpContext);

            return Ok(product.MasterProductDetails(productId));

        }

        [HttpGet("GetProductDetailsWithSellerId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult GetProductDetailsWithSellerId(int ProductId, string sellerId, int? sellerProductId = 0, int? sizeId = 0, bool? live = null, bool? isProductExist = null, string? status = null, bool? isDeleted = null, bool? isArchive = null)
        {
            GetProduct product = new GetProduct(_configuration, _httpContext);
            return Ok(product.GetProductDetailsWithSellerId(ProductId, sellerId, sellerProductId, sizeId, live, isProductExist, status, isDeleted, isArchive));
        }

        [HttpGet("ArchiveProducts")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult GetArchiveProducts(int? productId = 0, int? productMasterid = 0, int? categoryId = 0, int? assiCategoryId = 0, string? companySkuCode = null, string? sellerSkuCode = null, string? sellerId = null, int? brandId = 0, string? guid = null, int pageIndex = 1, int pageSize = 10, string? searchtext = null)
        {
            GetProduct product = new GetProduct(_configuration, _httpContext);
            var res = product.GetArchiveProducts(productId, productMasterid, categoryId, assiCategoryId, companySkuCode, sellerSkuCode, sellerId, brandId, guid, pageIndex, pageSize, searchtext);
            return Ok(res);
            //BaseResponse<ProductDetails> baseResponseProductDetails = new BaseResponse<ProductDetails>();

            //if (searchText != null)
            //{


            //    baseResponseProductDetails = product.GetAllProducts(0, sellerId, null, true, 0, 0);
            //    List<GetProductDTO> datalist = new List<GetProductDTO>();
            //    datalist = (List<GetProductDTO>)baseResponseProductDetails.Data;

            //    datalist = datalist.Where(x => x.CompanySKUCode.ToLower().Contains(searchText.ToLower()) || x.CategoryName.ToLower().Contains(searchText.ToLower()) || x.ProductName.ToLower().Contains(searchText.ToLower())).ToList();
            //    if (datalist.Count > 0)
            //    {
            //        int totalCount = datalist.Count;
            //        int TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            //        List<GetProductDTO> items = datalist.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            //        baseResponseProductDetails.Message = "Data bind suceessfully.";
            //        baseResponseProductDetails.Data = items;
            //        baseResponseProductDetails.code = 200;
            //        baseResponseProductDetails.pagination.PageCount = TotalPages;
            //        baseResponseProductDetails.pagination.RecordCount = totalCount;
            //    }
            //    else
            //    {
            //        baseResponseProductDetails = baseResponseProductDetails.NotExist();
            //    }
            //    return Ok(baseResponseProductDetails);
            //}
            //else
            //{
            //    baseResponseProductDetails = product.GetAllProducts(0, sellerId, null, true, pageIndex, pageSize);
            //    List<GetProductDTO> datalist = new List<GetProductDTO>();
            //    datalist = (List<GetProductDTO>)baseResponseProductDetails.Data;
            //    if (datalist.Count > 0)
            //    {
            //        int totalCount = datalist.Count;
            //        int TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            //        List<GetProductDTO> items = datalist.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            //        baseResponseProductDetails.Message = "Data bind suceessfully.";
            //        baseResponseProductDetails.Data = items;
            //        baseResponseProductDetails.code = 200;
            //        baseResponseProductDetails.pagination.PageCount = TotalPages;
            //        baseResponseProductDetails.pagination.RecordCount = totalCount;
            //    }
            //    else
            //    {
            //        baseResponseProductDetails = baseResponseProductDetails.NotExist();
            //    }
            //    return Ok(baseResponseProductDetails);
            //}

        }

        [HttpGet("GetCommission")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> GetCommission(int CategoryId, string? sellerId = null, int brandId = 0)
        {
            GetCommissionCharges commissionCharges = new GetCommissionCharges(_httpContextAccessor);
            var res = commissionCharges.GetCommission(CategoryId, sellerId, brandId, CatalogueUrl);
            return Ok(res);

        }

        [HttpGet("CountCommission")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<string> CountCommission(decimal sellingprice, string chargesIn, decimal percentageValue, decimal Amount)
        {
            PriceCalculation commissionCharges = new PriceCalculation();
            JObject res = commissionCharges.countCommissionCost(sellingprice, chargesIn, percentageValue, Amount);
            string val = res.ToString();
            return val;

        }


        [HttpPost("DisplayCalculation")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<string> displayCalculation(DisplayPriceCalculation priceCalculation)
        {
            bool shippingOnCategory = Convert.ToBoolean(_configuration.GetValue<string>("shipping_charges_on_Category"));
            JObject _netSellerEarn = _priceCalculation.displayCalculation(priceCalculation, _httpContextAccessor, CatalogueUrl, false, shippingOnCategory);
            string val = _netSellerEarn.ToString();
            return val;
        }

        [HttpPost("CalculatePackagingWeight")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<string> CalculatePackagingWeight(PackagingWeightDTO packagingWeightDTO)
        {
            string weight = _priceCalculation.CountPackagingWeight(packagingWeightDTO);
            JObject PackagingWeight = new JObject();
            PackagingWeight["packaging_weight"] = weight;

            getWeightSlab getWeightSlab = new getWeightSlab(_configuration, _httpContext);

            WeightSlabLibrary slabLibrary = getWeightSlab.Get(Convert.ToDecimal(weight));
            if (!string.IsNullOrEmpty(slabLibrary.WeightSlab))
            {
                PackagingWeight["weightSlabId"] = slabLibrary.Id;
                PackagingWeight["WeightSlab"] = slabLibrary.WeightSlab;
            }

            return PackagingWeight.ToString();
        }

        [HttpPost("ProductTempImage")]
        [Authorize]
        public ActionResult<ApiHelper> TempImage(IFormFile Image, string productName, int sequence)
        {
            var result = imageUpload.TempProductUploadMethod(Image, productName, sequence);
            return Ok(result);
        }

        [HttpPost("CheckCompanySkuCode")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult CheckCompanySkuCode(CheckCompanySKUCodeDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveProduct product = new SaveProduct(_configuration, _httpContext, userID);
            var res = product.CheckCompanySkuCode(model);
            return Ok(res);

        }

        [HttpPost("CheckSellerSkuCode")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult CheckSellerSkuCode(CheckSellerSKUCodeDto model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            SaveProduct product = new SaveProduct(_configuration, _httpContext, userID);
            var res = product.CheckSellerSkuCode(model);
            return Ok(res);

        }

        [NonAction]
        //[Route("OldBulkupload")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> OldBulkupload([FromForm] BulkProduct model)
        {
            BaseResponse<string> res = new BaseResponse<string>();

            try
            {
                #region Save File
                if (model.File == null)
                {
                    throw new Exception("File is Not Received...");
                }

                string FileName = Path.GetFileNameWithoutExtension(model.File.FileName);

                string extension = Path.GetExtension(model.File.FileName);
                string FullName = FileName + DateTime.Now.ToString("ddMMyyyyfffhhmsff") + extension;

                string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

                if (!allowedExtsnions.Contains(extension))
                {
                    throw new Exception("Sorry! This file is not allowed,make sure that file having extension as either.xls or.xlsx is uploaded.");

                }


                if (!Directory.Exists("Resources" + "\\Excel" + "\\Product"))
                {
                    Directory.CreateDirectory("Resources" + "\\Excel" + "\\Product");
                }
                var folderName = Path.Combine("Resources", "Excel", "Product");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);



                var fullPath = Path.Combine(pathToSave, FullName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }
                #endregion

                //var response = helper.ApiCall(CatalogueUrl, EndPoints.Product + "\\Bulkupload?fullPath=" + fullPath + "&sellerId" + sellerId + "&brandId" + brandId, "POST", null, token);
                //fullPath = "D:\\OfficeWork\\Hashkart\\Hashkart-API\\Resources\\Excel\\Product\\Product290620235930122159.xlsx";
                DataTable dtProduct = conExcelTODataTable(fullPath, "Product");
                DataTable dtProductData = conExcelTODataTable(fullPath, "Product_Data");

                List<string> errorSKULST = new List<string>();
                bool isSheetValid = true;

                if (Convert.ToInt32(dtProductData.Rows[1]["Category"].ToString()) != model.CategoryId)
                {
                    res.Message = "Error in Sheet.";
                    res.code = 201;
                    res.Data = null;
                    errorSKULST.Add("You are Uploading Products in Wrong Category.");
                    isSheetValid = false;
                    //return res;
                }
                else
                {
                    #region Category
                    /// Get Category data from database
                    GetProduct getProduct = new GetProduct(_configuration, _httpContext);
                    CategoryLibrary category = getProduct.getCategory(model.CategoryId);
                    #endregion

                    #region AssignSpecToCat
                    var ResAssignSpec = helper.ApiCall(CatalogueUrl, EndPoints.AssignSpecToCat + "?CategoryID=" + model.CategoryId, "GET", null);
                    BaseResponse<AssignSpecificationToCategoryLibrary> baseAssignSpec = new BaseResponse<AssignSpecificationToCategoryLibrary>();
                    baseAssignSpec = baseAssignSpec.JsonParseRecord(ResAssignSpec);
                    AssignSpecificationToCategoryLibrary assignSpec = (AssignSpecificationToCategoryLibrary)baseAssignSpec.Data;
                    #endregion

                    #region Seller Product
                    /// Get Seller Product data from database
                    var ResProduct = helper.ApiCall(CatalogueUrl, EndPoints.SellerProduct + "?categoryId=" + model.CategoryId + "&brandId=" + model.BrandId + "&PageIndex=0&PageSize=0", "GET", null);
                    BaseResponse<SellerProduct> baseProduct = new BaseResponse<SellerProduct>();
                    baseProduct = baseProduct.JsonParseList(ResProduct);
                    List<SellerProduct> LSTProduct = (List<SellerProduct>)baseProduct.Data;
                    #endregion

                    #region AssignTaxRateToHSNCode

                    /// Get Assign Tax Rate to HSN code
                    var ResAssiTax = helper.ApiCall(CatalogueUrl, EndPoints.AssignTaxRateToHSNCode + "?PageIndex=0&PageSize=0", "GET", null);
                    BaseResponse<AssignTaxRateToHSNCode> baseAssiTax = new BaseResponse<AssignTaxRateToHSNCode>();
                    baseAssiTax = baseAssiTax.JsonParseList(ResAssiTax);
                    List<AssignTaxRateToHSNCode> LSTAssiTax = (List<AssignTaxRateToHSNCode>)baseAssiTax.Data;
                    #endregion

                    #region WeightSlab

                    /// Get Weight Slab data from database
                    var ResWeight = helper.ApiCall(CatalogueUrl, EndPoints.WeightSlab + "?PageIndex=0&PageSize=0", "GET", null);
                    BaseResponse<WeightSlabLibrary> baseWeight = new BaseResponse<WeightSlabLibrary>();
                    baseWeight = baseWeight.JsonParseList(ResWeight);
                    List<WeightSlabLibrary> LSTWeight = (List<WeightSlabLibrary>)baseWeight.Data;
                    #endregion

                    #region AssignSizeValueToCategory 

                    var ResSize = helper.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?AssignSpecID=" + assignSpec.Id + "&PageIndex=0&PageSize=0", "GET", null);
                    BaseResponse<AssignSizeValueToCategory> baseSize = new BaseResponse<AssignSizeValueToCategory>();
                    baseSize = baseSize.JsonParseList(ResSize);
                    List<AssignSizeValueToCategory> LSTSize = (List<AssignSizeValueToCategory>)baseSize.Data;
                    #endregion

                    #region Color 

                    /// Get Color data from database
                    var Rescolor = helper.ApiCall(CatalogueUrl, EndPoints.Color + "?PageIndex=0&PageSize=0", "GET", null);
                    BaseResponse<ColorLibrary> baseColor = new BaseResponse<ColorLibrary>();
                    baseColor = baseColor.JsonParseList(Rescolor);
                    List<ColorLibrary> LSTColor = (List<ColorLibrary>)baseColor.Data;
                    #endregion


                    #region Spec data

                    var ResSpecification = helper.ApiCall(CatalogueUrl, EndPoints.AssignSpecValuesToCategory + "?AssignSpecId=" + assignSpec.Id + "&pageindex=0&PageSize=0", "GET", null);

                    BaseResponse<AssignSpecValuesToCategoryLibrary> baseAssignSpecValueToCat = new BaseResponse<AssignSpecValuesToCategoryLibrary>();
                    baseAssignSpecValueToCat = baseAssignSpecValueToCat.JsonParseList(ResSpecification);
                    List<AssignSpecValuesToCategoryLibrary> lstAssignSpecValueToCat = (List<AssignSpecValuesToCategoryLibrary>)baseAssignSpecValueToCat.Data;

                    #endregion

                    #region Comment
                    ///// Get Seller Warehouse data from database
                    //var ResWarehouse = helper.ApiCall(userUrl, EndPoints.Warehouse + "?PageIndex=0&PageSize=0", "GET", null, token);
                    //BaseResponse<Warehouse> baseWarehouse = new BaseResponse<Warehouse>();
                    //baseWarehouse = baseWarehouse.JsonParseList(ResWarehouse);
                    //List<Warehouse> LSTWarehosue = (List<Warehouse>)baseWarehouse.Data;

                    #endregion

                    #region Datatable To List 
                    List<ProductDetails> lstProductDetailBulk = new List<ProductDetails>();

                    lstProductDetailBulk = (from DataRow dr in dtProduct.Rows
                                            select new ProductDetails()
                                            {
                                                productId = dr.Table.Columns.Contains("Id") ? Convert.ToInt32(dr["Id"].ToString()) : 0,
                                                IsMasterProduct = dr.Table.Columns.Contains("Id") ? false : true,
                                                ParentId = 0,
                                                CategoryId = model.CategoryId,
                                                AssiCategoryId = assignSpec.Id,
                                                TaxValueId = LSTAssiTax.Where(p => p.DisplayName == dr["HSN Code"].ToString()).FirstOrDefault().TaxValueId,
                                                HSNCodeId = LSTAssiTax.Where(p => p.DisplayName == dr["HSN Code"].ToString()).FirstOrDefault().HsnCodeId,
                                                ProductName = "",
                                                CustomeProductName = dr["Custome Product Name"].ToString(),
                                                CompanySKUCode = dr["Company SKU Code"].ToString(),
                                                Description = dr["Description"].ToString(),
                                                Highlights = dr["Highlights"].ToString(),
                                                Keywords = dr["Keywords"].ToString(),
                                                BrandID = Convert.ToInt32(model.BrandId),
                                                SellerProducts = new SellerProductDTO()
                                                {
                                                    Id = 0,
                                                    ProductID = 0,
                                                    SellerID = model.SellerId,
                                                    BrandID = Convert.ToInt32(model.BrandId),
                                                    SellerSKU = dr["Product SKU"].ToString(),
                                                    IsExistingProduct = false,
                                                    Live = false,
                                                    Status = "Bulk Upload",
                                                    PackingLength = Convert.ToDecimal(dr["Packing_L"].ToString()),
                                                    PackingBreadth = Convert.ToDecimal(dr["Packing_B"].ToString()),
                                                    PackingHeight = Convert.ToDecimal(dr["Packing_H"].ToString()),
                                                    PackingWeight = Convert.ToDecimal(dr["PackingWeight"].ToString()),
                                                    WeightSlabId = LSTWeight.Where(p => p.WeightSlab == dr["Weight"].ToString()).FirstOrDefault().Id,

                                                    ProductPrices = new List<ProductPriceDTO>()
                                                {
                                                    new ProductPriceDTO()
                                                    {
                                                        Id = 0,
                                                        SellerProductId = 0,
                                                        MRP = Convert.ToDecimal(dr["MRP"].ToString()),
                                                        SellingPrice = Convert.ToDecimal(dr["Selling Price"].ToString()),
                                                        Discount = ((Convert.ToDecimal(dr["MRP"].ToString()) - Convert.ToDecimal(dr["Selling Price"].ToString())) / Convert.ToDecimal(dr["MRP"].ToString())) * 100,
                                                        Quantity = BProductQty(dr,model.SellerId),
                                                        SizeTypeName = dr["Size"].ToString().Split('-')[1].Replace("(","").Replace(")","").ToString(),
                                                        SizeName = dr["Size"].ToString().Split('-')[0].ToString(),
                                                        //SizeID = LSTSize.Where(p => p.SizeTypeName == dr["Size"].ToString().Split('-')[1].Replace("(","").Replace(")","").ToString() && p.SizeName == dr["Size"].ToString().Split('-')[0].ToString()).FirstOrDefault().Id,
                                                        SizeID = assignSpec.IsAllowSize ? LSTSize.Where(p => p.SizeTypeName == dr["Size"].ToString().Split('-')[1].Replace("(","").Replace(")","").ToString() && p.SizeName == dr["Size"].ToString().Split('-')[0].ToString()).FirstOrDefault().SizeId : null,

                                                        ProductWarehouses = BProductWarehouseQty(dr,model.SellerId),
                                                    },
                                                },
                                                },
                                                ProductColorMapping = assignSpec.IsAllowColors ? new List<ProductColorDTO>
                                            {
                                                new ProductColorDTO()
                                                {
                                                    ColorId = LSTColor.Where(p => p.Name == dr["Color"].ToString()).FirstOrDefault()?.Id ?? 0, // Using null coalescing operator
                                                    ColorName = dr["Color"].ToString(),
                                                }
                                            } : new List<ProductColorDTO>(),
                                                ProductImage = BProductImage(dr),
                                                //ProductVideoLinks = BProductVideo(dr),
                                                ProductSpecificationsMapp = assignSpec.IsAllowSpecifications ? BProductSpecification(dr, lstAssignSpecValueToCat) : new List<ProductSpecificationMappingDto>()
                                            }).ToList();

                    #endregion

                    #region Comment
                    //List<string> Exsting = new List<string>();

                    //List<string> LST_CompanySKU = (from DataRow dr in dtProduct.Rows select dr["Company SKU Code"].ToString()).ToList();

                    //bool has = LSTProduct.Any(cus => LST_CompanySKU.Contains(cus.CompanySKUCode.ToString()));
                    //if (has)
                    //{
                    //    isSheetValid = !has;
                    //    Exsting = LST_CompanySKU.Intersect(LSTProduct.Select(p => p.CompanySKUCode.ToString())).ToList();
                    //}

                    //if (isSheetValid == false)
                    //{
                    //    res.Message = "Company SKU code already exist";
                    //    res.code = 201;
                    //    res.Data = Exsting;
                    //    //return res;
                    //}
                    #endregion

            //        productExtraDetails.TradeName = item.LegalName;
            //        productExtraDetails.LegalName = item.TradeName;
            //        productExtraDetails.SellerId = item.UserID;
            //        productExtraDetails.Mode = "updateSellerGST";
            //        ExtraDetailsresponse = helper.ApiCall(CatalogueUrl, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
            //        ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);

                    List<ProductDetails> errorList = lstProductDetailBulk.Where(item => LSTProduct.Any(e => item.CompanySKUCode == e.CompanySKUCode && item.SellerProducts.Id != e.Id)).ToList();

                    var eclVarintSku = "";
                    foreach (var item in errorList)
                    {
                        eclVarintSku = string.IsNullOrEmpty(eclVarintSku) ? item.CompanySKUCode : eclVarintSku + " , " + item.CompanySKUCode;
                    }

                    if (errorList.Count > 0)
                    {
                        res.Message = "Error in Sheet.";
                        res.code = 201;
                        res.Data = null;
                        errorSKULST.Add("Company SKU code already exist. Product Sku is " + eclVarintSku);
                        isSheetValid = false;
                        //return res;
                    }
                    
                    #region Seller SKU Validation

                    errorList = lstProductDetailBulk.Where(item => LSTProduct.Any(e => item.SellerProducts.SellerID == model.SellerId && item.SellerProducts.SellerSKU == e.SKUCode && item.SellerProducts.Id != e.Id)).ToList();

                    eclVarintSku = "";
                    foreach (var item in errorList)
                    {
                        eclVarintSku = string.IsNullOrEmpty(eclVarintSku) ? item.CompanySKUCode : eclVarintSku + " , " + item.CompanySKUCode;
                    }

                    if (errorList.Count > 0)
                    {
                        res.Message = "Error in Sheet.";
                        res.code = 201;
                        res.Data = null;
                        errorSKULST.Add("Seller SKU code already exist. Product Sku is " + eclVarintSku);
                        isSheetValid = false;
                        //return res;
                    }

                    #endregion

                    #region MRP Validation

                    errorList = lstProductDetailBulk.Where(item => item.SellerProducts.ProductPrices.AsEnumerable().Any(e => e.SellingPrice > e.MRP)).ToList();

                    eclVarintSku = "";
                    foreach (var item in errorList)
                    {
                        eclVarintSku = string.IsNullOrEmpty(eclVarintSku) ? item.CompanySKUCode : eclVarintSku + " , " + item.CompanySKUCode;
                    }

                    if (errorList.Count > 0)
                    {
                        res.Message = "Error in Sheet.";
                        res.code = 201;
                        res.Data = errorList;
                        errorSKULST.Add("MRP must be greater than Selling Price. Product Sku is " + eclVarintSku);
                        isSheetValid = false;
                        //return res;
                    }

                    #endregion

                    #region Save Product in Database 
                    if (isSheetValid)
                    {
                        string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                        SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, userID);

                        string msg = null;
                        int i = 0;
                        foreach (ProductDetails item in lstProductDetailBulk)
                        {
                            try
                            {
                                var res1 = saveProduct.SaveData(item);
                            }
                            catch (Exception ex)
                            {
                                msg = "Exp " + i + " in SKU (" + item.CompanySKUCode + "):" + ex.Message;
                                errorSKULST.Add(msg);
                                i++;
                            }
                        }

                        res.Message = "Bulk uploaded";
                        res.code = 200;
                        res.Data = i == 0 ? 0 : msg;
                    }
                    #endregion

                }
                System.IO.File.Delete(fullPath);
                res.Data = errorSKULST;


                //return Ok();
            }
            catch
            {

                res.Message = "Error in Sheet.";
                res.code = 201;
                res.Data = null;
            }

            return Ok(res);

        }

        [NonAction]
        public int BProductQty(DataRow dr, string SellerId)
        {
            try
            {
                int Qty = 0;

                bool AllowWarehouseManagement = Convert.ToBoolean(_configuration.GetValue<string>("Allow_warehouse_management"));
                if (AllowWarehouseManagement)
                {
                    BaseResponse<Warehouse> WarebaseResponse = new BaseResponse<Warehouse>();
                    var response = helper.ApiCall(userUrl, EndPoints.Warehouse + "?UserID=" + SellerId, "GET", null);
                    WarebaseResponse = WarebaseResponse.JsonParseList(response);

                    List<Warehouse> warehouses = (List<Warehouse>)WarebaseResponse.Data;
                    int wcount = 1;
                    foreach (var item in warehouses)
                    {
                        Qty = Qty + Convert.ToInt32(dr[item.Name + " (WareHouse-" + wcount + ")"].ToString());
                        wcount++;
                    }
                }
                else
                {
                    Qty = Convert.ToInt32(dr["Quantity"].ToString());
                }


                return Qty;
            }
            catch (Exception ex)
            {

                throw;
            }


        }

        [NonAction]
        public List<ProductWarehouseDTO> BProductWarehouseQty(DataRow dr, string SellerId)
        {

            List<ProductWarehouseDTO> ProductWarehouseDTO = new List<ProductWarehouseDTO>();
            bool AllowWarehouseManagement = Convert.ToBoolean(_configuration.GetValue<string>("Allow_warehouse_management"));
            if (AllowWarehouseManagement)
            {
                BaseResponse<Warehouse> WarebaseResponse = new BaseResponse<Warehouse>();
                var response = helper.ApiCall(userUrl, EndPoints.Warehouse + "?UserID=" + SellerId, "GET", null);
                WarebaseResponse = WarebaseResponse.JsonParseList(response);

                List<Warehouse> warehouses = (List<Warehouse>)WarebaseResponse.Data;
                int wcount = 1;
                foreach (var item in warehouses)
                {
                    ProductWarehouseDTO obj = new ProductWarehouseDTO();
                    obj.Id = 0;
                    obj.WarehouseId = item.Id;
                    obj.WarehouseName = item.Name;
                    obj.Quantity = Convert.ToInt32(dr[item.Name + " (WareHouse-" + wcount + ")"].ToString());

                    wcount++;
                    ProductWarehouseDTO.Add(obj);
                }
            }



            return ProductWarehouseDTO;

        }

        [NonAction]
        public List<ProductImageDTO> BProductImage(DataRow dr)
        {
            List<ProductImageDTO> productImageDTOs = new List<ProductImageDTO>();

            for (int i = 1; i <= 7; i++)
            {
                if (!string.IsNullOrEmpty(dr["image url " + i].ToString()))
                {
                    var result = imageUpload.TempProductUploadMethodFromUrl(dr["image url " + i].ToString(), dr["Custome Product Name"].ToString()).GetAwaiter().GetResult();

                    if (!string.IsNullOrEmpty(result))
                    {
                        ProductImageDTO PI = new ProductImageDTO();
                        PI.Url = result.ToString();
                        PI.Type = "Image";
                        //PI.Image = dr["image url " + i].ToString();
                        PI.Sequence = i;
                        productImageDTOs.Add(PI);
                    }
                }
            }

            for (int i = 1; i <= 5; i++)
            {
                if (!string.IsNullOrEmpty(dr["Video ID " + i].ToString()))
                {
                    ProductImageDTO PV = new ProductImageDTO();
                    PV.Url = dr["Video ID " + i].ToString();
                    PV.Type = "Video";
                    PV.Sequence = i;
                    productImageDTOs.Add(PV);
                }
            }

            return productImageDTOs;

        }


        [NonAction]
        public List<ProductSpecificationMappingDto> BProductSpecification(DataRow dr, List<AssignSpecValuesToCategoryLibrary> lstAssignSpecValueToCat)
        {
            List<ProductSpecificationMappingDto> nn = new List<ProductSpecificationMappingDto>();

            List<AssignSpecValuesToCategoryLibrary> tmplst = lstAssignSpecValueToCat.GroupBy(p => p.SpecTypeID).Select(group => group.First()).ToList();
            foreach (var item in tmplst)
            {

                if (dr.Table.Columns.Contains(item.SpecificationName + "-" + item.SpecificationTypeName))
                {
                    ProductSpecificationMappingDto product = new ProductSpecificationMappingDto
                    {
                        Id = 0,
                        SpecId = item.SpecID,
                        SpecTypeId = item.SpecTypeID,
                        SpecValueId = item.FieldType.ToLower() == "dropdownlist" ? lstAssignSpecValueToCat.Where(p => p.SpecTypeID == item.SpecTypeID && p.SpecificationTypeValueName == dr[item.SpecificationName + "-" + item.SpecificationTypeName].ToString()).Select(p => p.SpecTypeValueID).FirstOrDefault() : null,
                        Value = dr[item.SpecificationName + "-" + item.SpecificationTypeName].ToString(),
                        SpecificationName = item.SpecificationName,
                        SpecificationTypeName = item.SpecificationTypeName

                    };
                    nn.Add(product);
                }
            }









            //foreach (DataColumn column in dr.Table.Columns)
            //{
            //    string columnName = column.ColumnName;
            //    string cellValue = dr[column].ToString();

            //    if (lstAssignSpecValueToCat.Where(p => p.SpecificationName + "-" + p.SpecificationTypeName == columnName).Count() > 0)
            //    {


            //        ProductSpecificationMappingDto product = new ProductSpecificationMappingDto
            //        {
            //            Id = 0,
            //            SpecId = lstAssignSpecValueToCat.Where(p => p.SpecificationName + "-" + p.SpecificationTypeName == columnName).FirstOrDefault().SpecID,
            //            SpecTypeId = lstAssignSpecValueToCat.Where(p => p.SpecificationName + "-" + p.SpecificationTypeName == columnName).FirstOrDefault().SpecTypeID,
            //            SpecValueId = lstAssignSpecValueToCat.Where(p => p.SpecificationName + "-" + p.SpecificationTypeName == columnName).FirstOrDefault().SpecTypeValueID,
            //            Value = cellValue,
            //            SpecificationName = lstAssignSpecValueToCat.Where(p => p.SpecificationName + "-" + p.SpecificationTypeName == columnName).FirstOrDefault().SpecificationName,
            //            SpecificationTypeName = lstAssignSpecValueToCat.Where(p => p.SpecificationName + "-" + p.SpecificationTypeName == columnName).FirstOrDefault().SpecificationTypeName

            //        };
            //        nn.Add(product);
            //    }

            //}


            return nn;


        }

        //[NonAction]
        //public List<ProductVideoLinkDTO> BProductVideo(DataRow dr)
        //{
        //    List<ProductVideoLinkDTO> productVideoLinkDTOs = new List<ProductVideoLinkDTO>();

        //    for (int i = 1; i <= 5; i++)
        //    {
        //        if (!string.IsNullOrEmpty(dr["Video ID " + i].ToString()))
        //        {
        //            ProductVideoLinkDTO PV = new ProductVideoLinkDTO();
        //            PV.Link = dr["Video ID " + i].ToString();
        //            productVideoLinkDTOs.Add(PV);
        //        }
        //    }

        //    return productVideoLinkDTOs;

        //}

        [HttpGet("downloadProduct")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> DownloadProduct(string categoryId, string? sellerId = null, string? brandId = null)
        {

            #region Spec to Cat

            var ResSpecToCat = helper.ApiCall(CatalogueUrl, EndPoints.AssignSpecToCat + "?CategoryID=" + Convert.ToInt32(categoryId), "GET", null);
            BaseResponse<AssignSpecificationToCategoryLibrary> baseSpecToCat = new BaseResponse<AssignSpecificationToCategoryLibrary>();
            baseSpecToCat = baseSpecToCat.JsonParseRecord(ResSpecToCat);
            AssignSpecificationToCategoryLibrary assignSpecToCat = new AssignSpecificationToCategoryLibrary();
            if (baseSpecToCat.code == 200)
            {
                assignSpecToCat = (AssignSpecificationToCategoryLibrary)baseSpecToCat.Data;

            }
            #endregion

            int rowCount = 0;
            #region XL Sheet Header
            List<int> totalCount = new List<int>();

            DataTable dtProduct = new DataTable();
            dtProduct.Columns.Add("Company SKU Code", typeof(string));
            dtProduct.Columns.Add("Product SKU", typeof(string));
            dtProduct.Columns.Add("Custome Product Name", typeof(string));
            dtProduct.Columns.Add("Description", typeof(string));
            dtProduct.Columns.Add("Highlights", typeof(string));
            dtProduct.Columns.Add("Keywords", typeof(string));

            //dtProduct.Columns.Add("Tax Value", typeof(string));
            dtProduct.Columns.Add("HSN Code", typeof(string));
            dtProduct.Columns.Add("Packing_L", typeof(string));
            dtProduct.Columns.Add("Packing_B", typeof(string));
            dtProduct.Columns.Add("Packing_H", typeof(string));
            dtProduct.Columns.Add("PackingWeight", typeof(string));
            dtProduct.Columns.Add("Weight", typeof(string));

            dtProduct.Columns.Add("MRP", typeof(string));
            dtProduct.Columns.Add("Selling Price", typeof(string));

            //dtProduct.Columns.Add("SizeType", typeof(string));
            if (assignSpecToCat.IsAllowSize)
            {
                dtProduct.Columns.Add("Size", typeof(string));
            }



            bool AllowWarehouseManagement = Convert.ToBoolean(_configuration.GetValue<string>("Allow_warehouse_management"));
            if (AllowWarehouseManagement)
            {
                BaseResponse<Warehouse> WarebaseResponse = new BaseResponse<Warehouse>();
                var response = helper.ApiCall(userUrl, EndPoints.Warehouse + "?UserID=" + sellerId, "GET", null);
                WarebaseResponse = WarebaseResponse.JsonParseList(response);

                List<Warehouse> warehouses = (List<Warehouse>)WarebaseResponse.Data;
                int wcount = 1;
                foreach (var item in warehouses)
                {
                    dtProduct.Columns.Add(item.Name + " (WareHouse-" + wcount + ")", typeof(string));
                    wcount++;
                }
            }
            else
            {
                dtProduct.Columns.Add("Quantity", typeof(string));
            }

            if (assignSpecToCat.IsAllowColors)
            {
                dtProduct.Columns.Add("Color", typeof(string));
            }

            dtProduct.Columns.Add("Image url 1", typeof(string));
            dtProduct.Columns.Add("Image url 2", typeof(string));
            dtProduct.Columns.Add("Image url 3", typeof(string));
            dtProduct.Columns.Add("Image url 4", typeof(string));
            dtProduct.Columns.Add("Image url 5", typeof(string));
            dtProduct.Columns.Add("Image url 6", typeof(string));
            dtProduct.Columns.Add("Image url 7", typeof(string));
            dtProduct.Columns.Add("Video ID 1", typeof(string));
            dtProduct.Columns.Add("Video ID 2", typeof(string));
            dtProduct.Columns.Add("Video ID 3", typeof(string));
            dtProduct.Columns.Add("Video ID 4", typeof(string));
            dtProduct.Columns.Add("Video ID 5", typeof(string));


            DataTable Product_Data = new DataTable();
            Product_Data.Columns.Add("Category", typeof(string));
            Product_Data.Columns.Add("HSN Code", typeof(string));
            Product_Data.Columns.Add("Weight", typeof(string));
            //Product_Data.Columns.Add("SizeType", typeof(string));
            #endregion

            #region Category data
            // Get Category data
            rowCount = 0;
            var ResCat = helper.ApiCall(CatalogueUrl, EndPoints.Category + "?Id=" + categoryId + "&PageIndex=0", "GET", null);
            BaseResponse<CategoryLibrary> baseCat = new BaseResponse<CategoryLibrary>();
            baseCat = baseCat.JsonParseRecord(ResCat);
            //List<CategoryLibrary> lstCat = (List<CategoryLibrary>)baseCat.Data;
            CategoryLibrary lstCat = (CategoryLibrary)baseCat.Data;

            totalCount.Add(1);

            //foreach (var item in lstCat)
            //{
            if (rowCount >= Product_Data.Rows.Count)
            {
                Product_Data.Rows.Add();
                Product_Data.Rows.Add();
            }
            Product_Data.Rows[rowCount]["Category"] = lstCat.PathNames;
            rowCount++;
            Product_Data.Rows[rowCount]["Category"] = lstCat.Id;
            rowCount++;
            //}
            #endregion

            #region HSN Code data
            // Get HSN Code data
            rowCount = 0;
            var ResAssiTax = helper.ApiCall(CatalogueUrl, EndPoints.AssignTaxRateToHSNCode + "?PageIndex=0&PageSize=0", "GET", null);
            BaseResponse<AssignTaxRateToHSNCode> baseAssiTax = new BaseResponse<AssignTaxRateToHSNCode>();
            baseAssiTax = baseAssiTax.JsonParseList(ResAssiTax);
            List<AssignTaxRateToHSNCode> lstAssiTax = (List<AssignTaxRateToHSNCode>)baseAssiTax.Data;

            totalCount.Add(lstAssiTax.Count());

            foreach (var item in lstAssiTax)
            {
                if (rowCount >= Product_Data.Rows.Count)
                {
                    Product_Data.Rows.Add();
                }
                Product_Data.Rows[rowCount]["HSN Code"] = item.DisplayName;
                rowCount++;
            }
            #endregion

            #region Weight data

            // Get Weight data
            rowCount = 0;
            var ResWeight = helper.ApiCall(CatalogueUrl, EndPoints.WeightSlab + "?PageIndex=0", "GET", null);
            BaseResponse<WeightSlabLibrary> baseWeight = new BaseResponse<WeightSlabLibrary>();
            baseWeight = baseWeight.JsonParseList(ResWeight);
            List<WeightSlabLibrary> lstWeight = (List<WeightSlabLibrary>)baseWeight.Data;

            totalCount.Add(lstWeight.Count());

            foreach (var item in lstWeight)
            {
                if (rowCount >= Product_Data.Rows.Count)
                {
                    Product_Data.Rows.Add();
                }
                Product_Data.Rows[rowCount]["Weight"] = item.WeightSlab;
                rowCount++;
            }
            #endregion

            #region Size data

            if (assignSpecToCat.IsAllowSize)
            {
                Product_Data.Columns.Add("Size", typeof(string));

                // Get Size data
                rowCount = 0;

                var ResSize = helper.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?AssignSpecID=" + assignSpecToCat.Id + "&PageIndex=0&PageSize=0", "GET", null);
                BaseResponse<AssignSizeValueToCategory> baseSize = new BaseResponse<AssignSizeValueToCategory>();
                baseSize = baseSize.JsonParseList(ResSize);
                List<AssignSizeValueToCategory> lstSize = (List<AssignSizeValueToCategory>)baseSize.Data;



                //List<SizeTypeDTO> lstSizeType = lstSize.GroupBy(x => new { x.SizeTypeID, x.SizeTypeName })
                //        .Select(x => new SizeTypeDTO
                //        {
                //            Id = Convert.ToInt32(x.Key.SizeTypeID),
                //            TypeName = x.Key.SizeTypeName
                //        }).ToList();

                //totalCount.Add(lstSizeType.Count());

                //foreach (var item in lstSizeType)
                //{
                //    if (rowCount >= Product_Data.Rows.Count)
                //    {
                //        Product_Data.Rows.Add();
                //    }
                //    Product_Data.Rows[rowCount]["SizeType"] = item.TypeName;
                //    rowCount++;
                //}

                rowCount = 0;

                totalCount.Add(lstSize.Count());

                foreach (var item in lstSize)
                {
                    if (rowCount >= Product_Data.Rows.Count)
                    {
                        Product_Data.Rows.Add();
                    }
                    Product_Data.Rows[rowCount]["Size"] = item.SizeName + "-(" + item.SizeTypeName + ")";
                    rowCount++;
                }
            }

            #endregion

            #region Color data

            if (assignSpecToCat.IsAllowColors)
            {
                Product_Data.Columns.Add("Color", typeof(string));

                // Get Color data
                rowCount = 0;
                var ResColor = helper.ApiCall(CatalogueUrl, EndPoints.Color + "?PageIndex=0", "GET", null);
                BaseResponse<ColorLibrary> baseColor = new BaseResponse<ColorLibrary>();
                baseColor = baseColor.JsonParseList(ResColor);
                List<ColorLibrary> lstColor = (List<ColorLibrary>)baseColor.Data;

                totalCount.Add(lstColor.Count());

                foreach (var item in lstColor)
                {
                    if (rowCount >= Product_Data.Rows.Count)
                    {
                        Product_Data.Rows.Add();
                    }
                    Product_Data.Rows[rowCount]["Color"] = item.Name;
                    rowCount++;
                }
            }
            #endregion



            #region XL Sheet Data bind

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dtProduct, "Product");
                var ws1 = wb.Worksheets.Add(Product_Data, "Product_Data");
                ws1.Protect("ksdjhf948ytoind8954ty9rjv8956u0wejfsdfjhouif02");


                var DeliveryRange = ws.RangeUsed();

                var DeliveryDataRange = ws1.RangeUsed();

                int count = 0;

                ws.Tables.FirstOrDefault().ShowAutoFilter = false;
                ws1.Tables.FirstOrDefault().ShowAutoFilter = false;


                foreach (DataColumn column in Product_Data.Columns)
                {
                    string[] ColumnName = new string[] { column.ToString() };

                    var Deliverycell = DeliveryRange.CellsUsed(c => c.Value.ToString() == ColumnName[0].ToString()).FirstOrDefault();
                    var DeliveryDatacell = DeliveryDataRange.CellsUsed(c => c.Value.ToString() == column.ToString()).FirstOrDefault();

                    string DeliveryRangLetter = "";

                    string DeliveryDataRangLetter = "";


                    if (Deliverycell != null)
                    {
                        DeliveryRangLetter = Deliverycell.WorksheetColumn().ColumnLetter().ToString();
                    }
                    if (DeliveryDatacell != null)
                    {
                        DeliveryDataRangLetter = DeliveryDatacell.WorksheetColumn().ColumnLetter().ToString();
                    }

                    if (DeliveryRangLetter != "" && DeliveryRangLetter != null)
                    {

                        if (totalCount[count].ToString() != "0")
                        {
                            ws.Cell(DeliveryRangLetter + "2").SetDataValidation().List(ws1.Range("Filter!" + DeliveryDataRangLetter + "2" + ":" + DeliveryDataRangLetter + (Convert.ToInt32(totalCount[count]) + 1)));

                        }
                    }

                    count++;
                }



                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", lstCat.PathNames.Replace('>', '_') + "_ProductSheet.xlsx");
                }
            }
            #endregion
        }


        [HttpGet("downloadwithProduct")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> DownloadWithProduct(string categoryId, string sellerId, string brandId)
        {

            #region Spec to Cat

            var ResSpecToCat = helper.ApiCall(CatalogueUrl, EndPoints.AssignSpecToCat + "?CategoryID=" + Convert.ToInt32(categoryId), "GET", null);
            BaseResponse<AssignSpecificationToCategoryLibrary> baseSpecToCat = new BaseResponse<AssignSpecificationToCategoryLibrary>();
            baseSpecToCat = baseSpecToCat.JsonParseRecord(ResSpecToCat);
            AssignSpecificationToCategoryLibrary assignSpecToCat = new AssignSpecificationToCategoryLibrary();
            if (baseSpecToCat.code == 200)
            {
                assignSpecToCat = (AssignSpecificationToCategoryLibrary)baseSpecToCat.Data;

            }
            #endregion

            int rowCount = 0;
            #region XL Sheet Header
            List<int> totalCount = new List<int>();

            DataTable dtProduct = new DataTable();
            dtProduct.Columns.Add("ProductID", typeof(string));
            dtProduct.Columns.Add("SellerProductId", typeof(string));
            dtProduct.Columns.Add("Company SKU Code", typeof(string));
            dtProduct.Columns.Add("Product SKU", typeof(string));
            dtProduct.Columns.Add("Custome Product Name", typeof(string));
            dtProduct.Columns.Add("Description", typeof(string));
            dtProduct.Columns.Add("Highlights", typeof(string));
            dtProduct.Columns.Add("Keywords", typeof(string));

            //dtProduct.Columns.Add("Tax Value", typeof(string));
            dtProduct.Columns.Add("HSN Code", typeof(string));
            dtProduct.Columns.Add("Packing_L", typeof(string));
            dtProduct.Columns.Add("Packing_B", typeof(string));
            dtProduct.Columns.Add("Packing_H", typeof(string));
            dtProduct.Columns.Add("PackingWeight", typeof(string));
            dtProduct.Columns.Add("Weight", typeof(string));

            dtProduct.Columns.Add("MRP", typeof(string));
            dtProduct.Columns.Add("Selling Price", typeof(string));

            //dtProduct.Columns.Add("SizeType", typeof(string));
            if (assignSpecToCat.IsAllowSize)
            {
                dtProduct.Columns.Add("Size", typeof(string));
            }



            List<Warehouse> warehouses = new List<Warehouse>();
            bool AllowWarehouseManagement = Convert.ToBoolean(_configuration.GetValue<string>("Allow_warehouse_management"));
            if (AllowWarehouseManagement)
            {
                BaseResponse<Warehouse> WarebaseResponse = new BaseResponse<Warehouse>();
                var response = helper.ApiCall(userUrl, EndPoints.Warehouse + "?UserID=" + sellerId, "GET", null);
                WarebaseResponse = WarebaseResponse.JsonParseList(response);

                warehouses = (List<Warehouse>)WarebaseResponse.Data;
                int wcount = 1;
                foreach (var item in warehouses)
                {
                    dtProduct.Columns.Add(item.Name + " (WareHouse-" + wcount + ")", typeof(string));
                    wcount++;
                }
            }
            else
            {
                dtProduct.Columns.Add("Quantity", typeof(string));
            }

            if (assignSpecToCat.IsAllowColors)
            {
                dtProduct.Columns.Add("Color", typeof(string));
            }

            dtProduct.Columns.Add("Image url 1", typeof(string));
            dtProduct.Columns.Add("Image url 2", typeof(string));
            dtProduct.Columns.Add("Image url 3", typeof(string));
            dtProduct.Columns.Add("Image url 4", typeof(string));
            dtProduct.Columns.Add("Image url 5", typeof(string));
            dtProduct.Columns.Add("Image url 6", typeof(string));
            dtProduct.Columns.Add("Image url 7", typeof(string));

            DataTable Product_Data = new DataTable();
            Product_Data.Columns.Add("Category", typeof(string));
            Product_Data.Columns.Add("HSN Code", typeof(string));
            Product_Data.Columns.Add("Weight", typeof(string));
            //Product_Data.Columns.Add("SizeType", typeof(string));
            #endregion

            #region Category data
            // Get Category data
            rowCount = 0;
            var ResCat = helper.ApiCall(CatalogueUrl, EndPoints.Category + "?Id=" + categoryId + "&PageIndex=0", "GET", null);
            BaseResponse<CategoryLibrary> baseCat = new BaseResponse<CategoryLibrary>();
            baseCat = baseCat.JsonParseRecord(ResCat);
            //List<CategoryLibrary> lstCat = (List<CategoryLibrary>)baseCat.Data;
            CategoryLibrary lstCat = (CategoryLibrary)baseCat.Data;

            totalCount.Add(1);

            //foreach (var item in lstCat)
            //{
            if (rowCount >= Product_Data.Rows.Count)
            {
                Product_Data.Rows.Add();
                Product_Data.Rows.Add();
            }
            Product_Data.Rows[rowCount]["Category"] = lstCat.PathNames;
            rowCount++;
            Product_Data.Rows[rowCount]["Category"] = lstCat.Id;
            rowCount++;
            //}
            #endregion

            #region HSN Code data
            // Get HSN Code data
            rowCount = 0;
            var ResAssiTax = helper.ApiCall(CatalogueUrl, EndPoints.AssignTaxRateToHSNCode + "?PageIndex=0&PageSize=0", "GET", null);
            BaseResponse<AssignTaxRateToHSNCode> baseAssiTax = new BaseResponse<AssignTaxRateToHSNCode>();
            baseAssiTax = baseAssiTax.JsonParseList(ResAssiTax);
            List<AssignTaxRateToHSNCode> lstAssiTax = (List<AssignTaxRateToHSNCode>)baseAssiTax.Data;

            totalCount.Add(lstAssiTax.Count());

            foreach (var item in lstAssiTax)
            {
                if (rowCount >= Product_Data.Rows.Count)
                {
                    Product_Data.Rows.Add();
                }
                Product_Data.Rows[rowCount]["HSN Code"] = item.DisplayName;
                rowCount++;
            }
            #endregion

            #region Weight data

            // Get Weight data
            rowCount = 0;
            var ResWeight = helper.ApiCall(CatalogueUrl, EndPoints.WeightSlab + "?PageIndex=0", "GET", null);
            BaseResponse<WeightSlabLibrary> baseWeight = new BaseResponse<WeightSlabLibrary>();
            baseWeight = baseWeight.JsonParseList(ResWeight);
            List<WeightSlabLibrary> lstWeight = (List<WeightSlabLibrary>)baseWeight.Data;

            totalCount.Add(lstWeight.Count());

            foreach (var item in lstWeight)
            {
                if (rowCount >= Product_Data.Rows.Count)
                {
                    Product_Data.Rows.Add();
                }
                Product_Data.Rows[rowCount]["Weight"] = item.WeightSlab;
                rowCount++;
            }
            #endregion

            #region Size data

            if (assignSpecToCat.IsAllowSize)
            {
                Product_Data.Columns.Add("Size", typeof(string));

                // Get Size data
                rowCount = 0;

                var ResSize = helper.ApiCall(CatalogueUrl, EndPoints.AssignSizeValueToCategory + "?AssignSpecID=" + assignSpecToCat.Id + "&PageIndex=0&PageSize=0", "GET", null);
                BaseResponse<AssignSizeValueToCategory> baseSize = new BaseResponse<AssignSizeValueToCategory>();
                baseSize = baseSize.JsonParseList(ResSize);
                List<AssignSizeValueToCategory> lstSize = (List<AssignSizeValueToCategory>)baseSize.Data;


                rowCount = 0;

                totalCount.Add(lstSize.Count());

                foreach (var item in lstSize)
                {
                    if (rowCount >= Product_Data.Rows.Count)
                    {
                        Product_Data.Rows.Add();
                    }
                    Product_Data.Rows[rowCount]["Size"] = item.SizeName + "-(" + item.SizeTypeName + ")";
                    rowCount++;
                }
            }

            #endregion

            #region Color data

            if (assignSpecToCat.IsAllowColors)
            {
                Product_Data.Columns.Add("Color", typeof(string));

                // Get Color data
                rowCount = 0;
                var ResColor = helper.ApiCall(CatalogueUrl, EndPoints.Color + "?PageIndex=0", "GET", null);
                BaseResponse<ColorLibrary> baseColor = new BaseResponse<ColorLibrary>();
                baseColor = baseColor.JsonParseList(ResColor);
                List<ColorLibrary> lstColor = (List<ColorLibrary>)baseColor.Data;

                totalCount.Add(lstColor.Count());

                foreach (var item in lstColor)
                {
                    if (rowCount >= Product_Data.Rows.Count)
                    {
                        Product_Data.Rows.Add();
                    }
                    Product_Data.Rows[rowCount]["Color"] = item.Name;
                    rowCount++;
                }
            }
            #endregion

            #region Spec data 

            if (assignSpecToCat.IsAllowSpecifications)
            {
                var ResSpecification = helper.ApiCall(CatalogueUrl, EndPoints.AssignSpecValuesToCategory + "?AssignSpecId=" + assignSpecToCat.Id + "&pageindex=0&PageSize=0", "GET", null);

                BaseResponse<AssignSpecValuesToCategoryLibrary> baseAssignSpecValueToCat = new BaseResponse<AssignSpecValuesToCategoryLibrary>();
                baseAssignSpecValueToCat = baseAssignSpecValueToCat.JsonParseList(ResSpecification);
                List<AssignSpecValuesToCategoryLibrary> lstAssignSpecValueToCat = (List<AssignSpecValuesToCategoryLibrary>)baseAssignSpecValueToCat.Data;

                List<AssignSpecValuesToCategoryLibrary> lstSpec = new List<AssignSpecValuesToCategoryLibrary>();
                List<AssignSpecValuesToCategoryLibrary> lstSpecType = new List<AssignSpecValuesToCategoryLibrary>();
                List<AssignSpecValuesToCategoryLibrary> lstSpecTypeValue = new List<AssignSpecValuesToCategoryLibrary>();

                lstSpec = lstAssignSpecValueToCat.GroupBy(x => new { x.SpecID, x.SpecificationName })
                        .Select(x => new AssignSpecValuesToCategoryLibrary
                        {
                            SpecID = Convert.ToInt32(x.Key.SpecID),
                            SpecificationName = x.Key.SpecificationName
                        }).ToList();

                lstSpecType = lstAssignSpecValueToCat.GroupBy(x => new { x.SpecID, x.SpecificationName, x.SpecTypeID, x.SpecificationTypeName, x.FieldType })
                        .Select(x => new AssignSpecValuesToCategoryLibrary
                        {
                            SpecID = Convert.ToInt32(x.Key.SpecID),
                            SpecificationName = x.Key.SpecificationName,
                            SpecTypeID = x.Key.SpecTypeID,
                            SpecificationTypeName = x.Key.SpecificationTypeName,
                            FieldType = x.Key.FieldType
                        }).ToList();

                lstSpecTypeValue = lstAssignSpecValueToCat.GroupBy(x => new { x.SpecID, x.SpecificationName, x.SpecTypeID, x.SpecificationTypeName, x.SpecTypeValueID, x.SpecificationTypeValueName, x.FieldType })
                        .Select(x => new AssignSpecValuesToCategoryLibrary
                        {
                            SpecID = Convert.ToInt32(x.Key.SpecID),
                            SpecificationName = x.Key.SpecificationName,
                            SpecTypeID = x.Key.SpecTypeID,
                            SpecificationTypeName = x.Key.SpecificationTypeName,
                            SpecTypeValueID = x.Key.SpecTypeValueID,
                            SpecificationTypeValueName = x.Key.SpecificationTypeValueName,
                            FieldType = x.Key.FieldType
                        }).ToList();


                foreach (var item in lstSpecType)
                {
                    dtProduct.Columns.Add(item.SpecificationName + "-" + item.SpecificationTypeName, typeof(string));

                    if (item.FieldType == "DropdownList")
                    {

                        totalCount.Add(lstSpecTypeValue.Where(p => p.SpecTypeID == item.SpecTypeID).ToList().Count());

                        Product_Data.Columns.Add(item.SpecificationName + "-" + item.SpecificationTypeName, typeof(string));
                        rowCount = 0;
                        foreach (var item1 in lstSpecTypeValue.Where(p => p.SpecTypeID == item.SpecTypeID).ToList())
                        {
                            if (rowCount >= Product_Data.Rows.Count)
                            {
                                Product_Data.Rows.Add();
                            }
                            Product_Data.Rows[rowCount][item.SpecificationName + "-" + item.SpecificationTypeName] = item1.SpecificationTypeValueName;
                            rowCount++;
                        }
                    }

                }
            }

            #endregion


            #region add products data 

            string url = string.Empty;
            bool isSellerList = false;
            if (categoryId != null && !string.IsNullOrEmpty(categoryId))
            {
                url += "CategoryID=" + Convert.ToInt32(categoryId);
            }
            if (brandId != null && !string.IsNullOrEmpty(brandId))
            {
                url += "&BrandId=" + Convert.ToInt32(brandId);
            }

            if (!string.IsNullOrEmpty(sellerId) && sellerId != "")
            {
                url += "&SellerId=" + sellerId;
                isSellerList = true;
            }

            BaseResponse<ProductBulkDownload> baseResponseProduct = new BaseResponse<ProductBulkDownload>();
            var responseproduct = helper.ApiCall(CatalogueUrl, EndPoints.ProductBulkDownload + "?" + url, "GET", null);

            baseResponseProduct = baseResponseProduct.JsonParseList(responseproduct);

            List<ProductBulkDownload> tempList = (List<ProductBulkDownload>)baseResponseProduct.Data;

            List<ProductBulkDownload> childProductList = tempList.Where(p => p.flag == "p").ToList();

            int cn = 0;
            foreach (var item in childProductList)
            {

                dtProduct.Rows.Add();
                dtProduct.Rows[cn]["ProductID"] = item.ProductID.ToString();
                dtProduct.Rows[cn]["SellerProductId"] = item.SellerProductId.ToString();
                dtProduct.Rows[cn]["Company SKU Code"] = item.CompanySKUCode.ToString();
                dtProduct.Rows[cn]["Product SKU"] = item.SellerSKUCode.ToString();
                dtProduct.Rows[cn]["Custome Product Name"] = item.CustomeProductName.ToString();
                dtProduct.Rows[cn]["Description"] = item.Description.ToString();
                dtProduct.Rows[cn]["Highlights"] = item.Highlights.ToString();
                dtProduct.Rows[cn]["Keywords"] = item.Keywords.ToString();
                dtProduct.Rows[cn]["HSN Code"] = item.HSNCode.ToString();
                dtProduct.Rows[cn]["Packing_L"] = item.PackingLength.ToString();
                dtProduct.Rows[cn]["Packing_B"] = item.PackingBreadth.ToString();
                dtProduct.Rows[cn]["Packing_H"] = item.PackingHeight.ToString();
                dtProduct.Rows[cn]["PackingWeight"] = item.PackingWeight.ToString();
                dtProduct.Rows[cn]["Weight"] = item.WeightSlabId.ToString();
                dtProduct.Rows[cn]["MRP"] = item.MRP.ToString();
                dtProduct.Rows[cn]["Selling Price"] = item.SellingPrice.ToString();

                List<ProductBulkDownload> imageProductList = tempList.Where(p => p.flag == "i" && p.ProductID == item.ProductID).ToList();
                int imgcount = 1;
                foreach (var itemimage in imageProductList)
                {
                    if (imgcount <= 7)
                    {
                        dtProduct.Rows[cn]["Image url " + imgcount] = itemimage.Image.ToString();
                    }
                    imgcount++;
                }

                if (assignSpecToCat.IsAllowSize)
                {
                    dtProduct.Rows[cn]["Size"] = item.SizeName.ToString();
                }

                if (AllowWarehouseManagement)
                {
                    List<ProductBulkDownload> warehouseProductList = tempList.Where(p => p.flag == "w" && p.ProductID == item.ProductID).ToList();

                    int wcount = 1;
                    foreach (var wareitem in warehouses)
                    {
                        dtProduct.Rows[cn][wareitem.Name + " (WareHouse-" + wcount + ")"] = warehouseProductList.Where(p => p.WarehouseId == wareitem.Id && p.ProductID == item.ProductID).Select(p => p.WarehouseProductQty).FirstOrDefault().ToString();
                        wcount++;
                    }
                }
                else
                {
                    dtProduct.Rows[cn]["Quantity"] = item.Quantity.ToString();

                }

                if (assignSpecToCat.IsAllowColors)
                {
                    dtProduct.Rows[cn]["Color"] = item.ColorName.ToString();
                }

                List<ProductBulkDownload> SpecProductList = tempList.Where(p => p.flag == "s" && p.ProductID == item.ProductID).Distinct().ToList();

                foreach (var speitem in SpecProductList)
                {
                    dtProduct.Rows[cn][speitem.SpecName + "-" + speitem.SpecTypeName] = speitem.Value;

                }


                cn++;
            }
            #endregion

            #region XL Sheet Data bind

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dtProduct, "Product");
                var ws1 = wb.Worksheets.Add(Product_Data, "Product_Data");
                ws1.Protect("ksdjhf948ytoind8954ty9rjv8956u0wejfsdfjhouif02");


                var DeliveryRange = ws.RangeUsed();

                var DeliveryDataRange = ws1.RangeUsed();

                int count = 0;

                ws.Tables.FirstOrDefault().ShowAutoFilter = false;
                ws1.Tables.FirstOrDefault().ShowAutoFilter = false;


                foreach (DataColumn column in Product_Data.Columns)
                {
                    string[] ColumnName = new string[] { column.ToString() };

                    var Deliverycell = DeliveryRange.CellsUsed(c => c.Value.ToString() == ColumnName[0].ToString()).FirstOrDefault();
                    var DeliveryDatacell = DeliveryDataRange.CellsUsed(c => c.Value.ToString() == column.ToString()).FirstOrDefault();

                    string DeliveryRangLetter = "";

                    string DeliveryDataRangLetter = "";


                    if (Deliverycell != null)
                    {
                        DeliveryRangLetter = Deliverycell.WorksheetColumn().ColumnLetter().ToString();
                    }
                    if (DeliveryDatacell != null)
                    {
                        DeliveryDataRangLetter = DeliveryDatacell.WorksheetColumn().ColumnLetter().ToString();
                    }

                    if (DeliveryRangLetter != "" && DeliveryRangLetter != null)
                    {

                        if (totalCount[count].ToString() != "0")
                        {
                            ws.Cell(DeliveryRangLetter + "2").SetDataValidation().List(ws1.Range("Filter!" + DeliveryDataRangLetter + "2" + ":" + DeliveryDataRangLetter + (Convert.ToInt32(totalCount[count]) + 1)));

                        }
                    }

                    count++;
                }



                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Product.xlsx");
                }
            }
            #endregion



        }


        [NonAction]
        public DataTable conExcelTODataTable(string fullPath, string SheetName)
        {
            ///Convert Excel to datatable with autometic header assign to datatabe colunm name

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            DataTable dtHSN;
            using (var stream = System.IO.File.Open(fullPath, FileMode.Open, FileAccess.Read))
            {

                IExcelDataReader excelDataReader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                var conf = new ExcelDataSetConfiguration()
                {
                    ConfigureDataTable = a => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };

                DataSet dataSet = excelDataReader.AsDataSet(conf);

                dtHSN = dataSet.Tables[SheetName];
                return dtHSN;
            }
        }

        [HttpPost]
        [Route("Bulkupload")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Bulkupload([FromForm] BulkProduct model)
        {
            BaseResponse<string> res = new BaseResponse<string>();

            try
            {
                #region Save File
                if (model.File == null)
                {
                    throw new Exception("File is Not Received...");
                }

                string FileName = Path.GetFileNameWithoutExtension(model.File.FileName);

                string extension = Path.GetExtension(model.File.FileName);
                string FullName = FileName + DateTime.Now.ToString("ddMMyyyyfffhhmsff") + extension;

                string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

                if (!allowedExtsnions.Contains(extension))
                {
                    throw new Exception("Sorry! This file is not allowed,make sure that file having extension as either.xls or.xlsx is uploaded.");

                }


                if (!Directory.Exists("Resources" + "\\Excel" + "\\Product"))
                {
                    Directory.CreateDirectory("Resources" + "\\Excel" + "\\Product");
                }
                var folderName = Path.Combine("Resources", "Excel", "Product");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);



                var fullPath = Path.Combine(pathToSave, FullName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }
                #endregion

                DataTable dtProduct = conExcelTODataTable(fullPath, "Product");
                DataTable dtProductData = conExcelTODataTable(fullPath, "Product_Data");

                List<string> errorSKULST = new List<string>();
                bool isSheetValid = true;

                if (Convert.ToInt32(dtProductData.Rows[1]["Category"].ToString()) != model.CategoryId)
                {
                    res.Message = "Error in Sheet.";
                    res.code = 201;
                    res.Data = null;
                    errorSKULST.Add("You are Uploading Products in Wrong Category.");
                    isSheetValid = false;
                    //return res;
                }
                else
                {

                    #region CheckValidation

                    #region MRP

                    var ReqMrp = dtProduct.AsEnumerable().Where(row => row["MRP"] == DBNull.Value || (row["MRP"] is string && string.IsNullOrWhiteSpace(row["MRP"].ToString()))).ToList();

                    if (ReqMrp.Any())
                    {
                        foreach (DataRow dr in ReqMrp)
                        {
                            int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                            errorSKULST.Add($"MRP is required found in row number {rowNumber}");
                        }
                        isSheetValid = false;
                    }

                    #endregion

                    #region Selling Price

                    var ReqSellingPrice = dtProduct.AsEnumerable().Where(row => row["Selling Price"] == DBNull.Value || (row["Selling Price"] is string && string.IsNullOrWhiteSpace(row["Selling Price"].ToString()))).ToList();

                    if (ReqSellingPrice.Any())
                    {
                        foreach (DataRow dr in ReqSellingPrice)
                        {
                            int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                            errorSKULST.Add($"Selling Price is required found in row number {rowNumber}");
                        }
                        isSheetValid = false;
                    }

                    #endregion

                    #region HSNCode

                    var ReqHSNCode = dtProduct.AsEnumerable().Where(row => row["HSN Code"] == DBNull.Value || (row["HSN Code"] is string && string.IsNullOrWhiteSpace(row["HSN Code"].ToString()))).ToList();

                    if (ReqHSNCode.Any())
                    {
                        foreach (DataRow dr in ReqHSNCode)
                        {
                            int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                            errorSKULST.Add($"HSN Code is required found in row number {rowNumber}");
                        }
                        isSheetValid = false;
                    }

                    #endregion

                    #region Weight

                    var ReqWeight = dtProduct.AsEnumerable().Where(row => row["Weight"] == DBNull.Value || (row["Weight"] is string && string.IsNullOrWhiteSpace(row["Weight"].ToString()))).ToList();

                    if (ReqWeight.Any())
                    {
                        foreach (DataRow dr in ReqWeight)
                        {
                            int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                            errorSKULST.Add($"Weight is required found in row number {rowNumber}");
                        }
                        isSheetValid = false;
                    }

                    #endregion

                    #region Size
                    if (dtProduct.Columns.Contains("Size"))
                    {
                        var ReqSize = dtProduct.AsEnumerable().Where(row => row["Size"] == DBNull.Value || (row["Size"] is string && string.IsNullOrWhiteSpace(row["Size"].ToString()))).ToList();

                        if (ReqSize.Any())
                        {
                            foreach (DataRow dr in ReqSize)
                            {
                                int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                                errorSKULST.Add($"Size is required found in row number {rowNumber}");
                            }
                            isSheetValid = false;
                        }
                    }

                    #endregion

                    #region Color
                    if (dtProduct.Columns.Contains("Color"))
                    {
                        var ReqColor = dtProduct.AsEnumerable().Where(row => row["Color"] == DBNull.Value || (row["Color"] is string && string.IsNullOrWhiteSpace(row["Color"].ToString()))).ToList();

                        if (ReqColor.Any())
                        {
                            foreach (DataRow dr in ReqColor)
                            {
                                int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                                errorSKULST.Add($"Color is required found in row number {rowNumber}");
                            }
                            isSheetValid = false;
                        }
                    }

                    #endregion

                    #region CompanySKU

                    var ReqCompanySKUs = dtProduct.AsEnumerable().Where(row => row["Company SKU Code"] == DBNull.Value || (row["Company SKU Code"] is string && string.IsNullOrWhiteSpace(row["Company SKU Code"].ToString()))).ToList();

                    if (ReqCompanySKUs.Any())
                    {
                        foreach (DataRow dr in ReqCompanySKUs)
                        {
                            int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                            errorSKULST.Add($"Company SKU Code is required found in row number {rowNumber}");
                        }
                        isSheetValid = false;
                    }

                    var duplicateCompanySKUs = dtProduct.Rows.Cast<DataRow>()
                                                .GroupBy(dr => dr["Company SKU Code"].ToString().ToLower())
                                                .Where(g => g.Count() > 1)
                                                .SelectMany(g => g)
                                                .ToList();

                    if (duplicateCompanySKUs.Any())
                    {
                        foreach (DataRow dr in duplicateCompanySKUs)
                        {
                            int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                            errorSKULST.Add($"Duplicate Company SKU Code '{dr["Company SKU Code"]}' found in row number {rowNumber}");
                        }
                        isSheetValid = false;
                    }
                    #endregion

                    #region ProductSKU

                    var ReqProductSKUs = dtProduct.AsEnumerable().Where(row => row["Product SKU"] == DBNull.Value || (row["Product SKU"] is string && string.IsNullOrWhiteSpace(row["Product SKU"].ToString()))).ToList();

                    if (ReqProductSKUs.Any())
                    {
                        foreach (DataRow dr in ReqProductSKUs)
                        {
                            int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                            errorSKULST.Add($"Product SKU is required found in row number {rowNumber}");
                        }
                        isSheetValid = false;
                    }

                    var duplicateProductSKUs = dtProduct.Rows.Cast<DataRow>()
                                                .GroupBy(dr => dr["Product SKU"].ToString().ToLower())
                                                .Where(g => g.Count() > 1)
                                                .SelectMany(g => g)
                                                .ToList();

                    if (duplicateProductSKUs.Any())
                    {
                        foreach (DataRow dr in duplicateProductSKUs)
                        {
                            int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                            errorSKULST.Add($"Duplicate Product SKU '{dr["Product SKU"]}' found in row number {rowNumber}");
                        }
                        isSheetValid = false;
                    }

                    #endregion

                    #endregion

                    if (isSheetValid)
                    {
                        #region Details 
                        /// Get ProductBulkDetails from database
                        var ResDetails = helper.ApiCall(CatalogueUrl, EndPoints.Product + "/getProductBulkDetails" + "?CategoryId=" + model.CategoryId + "&BrandId=" + model.BrandId, "GET", null);
                        BaseResponse<ProductBulkDetails> baseproductDetails = new BaseResponse<ProductBulkDetails>();
                        baseproductDetails = baseproductDetails.JsonParseList(ResDetails);

                        if (baseproductDetails.code == 200)
                        {
                            List<ProductBulkDetails> lstbulkDetails = (List<ProductBulkDetails>)baseproductDetails.Data;
                            ProductBulkDetails bulkDetail = lstbulkDetails.FirstOrDefault();

                            dynamic jsonCategory = JsonConvert.DeserializeObject<dynamic>(bulkDetail.Category);
                            List<CategoryLibrary> lstcategories = jsonCategory.categories.ToObject<List<CategoryLibrary>>();
                            CategoryLibrary category = lstcategories.FirstOrDefault();

                            AssignSpecificationToCategoryLibrary assignSpec = new AssignSpecificationToCategoryLibrary();
                            int TitleSequenceOfColor = 0;
                            if (bulkDetail.AssignSpectoCat != null)
                            {
                                dynamic jsonAssignSpec = JsonConvert.DeserializeObject<dynamic>(bulkDetail.AssignSpectoCat);
                                List<AssignSpecificationToCategoryLibrary> lstAssignSpec = jsonAssignSpec.AssignSpectoCat.ToObject<List<AssignSpecificationToCategoryLibrary>>();
                                assignSpec = lstAssignSpec.FirstOrDefault();
                                TitleSequenceOfColor = assignSpec.TitleSequenceOfColor != null ? Convert.ToInt32(assignSpec.TitleSequenceOfColor) : 0;
                            }

                            List<AssignSizeValueToCategory> LSTSize = new List<AssignSizeValueToCategory>();
                            if (bulkDetail.AssignSizeValtoCat != null)
                            {
                                dynamic jsonLSTSize = JsonConvert.DeserializeObject<dynamic>(bulkDetail.AssignSizeValtoCat);
                                LSTSize = jsonLSTSize.AssignSizeValtoCat.ToObject<List<AssignSizeValueToCategory>>();
                            }

                            List<AssignSpecValuesToCategoryLibrary> lstAssignSpecValueToCat = new List<AssignSpecValuesToCategoryLibrary>();
                            if (bulkDetail.AssignSpecValtoCat != null)
                            {
                                dynamic jsonlstAssignSpecValueToCat = JsonConvert.DeserializeObject<dynamic>(bulkDetail.AssignSpecValtoCat);
                                lstAssignSpecValueToCat = jsonlstAssignSpecValueToCat.AssignSpecValtoCat.ToObject<List<AssignSpecValuesToCategoryLibrary>>();
                            }


                            dynamic jsonLSTAssiTax = JsonConvert.DeserializeObject<dynamic>(bulkDetail.AssignTaxRateToHSNCode);
                            List<AssignTaxRateToHSNCode> LSTAssiTax = jsonLSTAssiTax.AssignTaxRateToHSNCode.ToObject<List<AssignTaxRateToHSNCode>>();

                            dynamic jsonLSTWeight = JsonConvert.DeserializeObject<dynamic>(bulkDetail.WeightSlab);
                            List<WeightSlabLibrary> LSTWeight = jsonLSTWeight.WeightSlab.ToObject<List<WeightSlabLibrary>>();

                            List<ColorLibrary> LSTColor = new List<ColorLibrary>();
                            if (bulkDetail.Color != null)
                            {

                                dynamic jsonColor = JsonConvert.DeserializeObject<dynamic>(bulkDetail.Color);
                                LSTColor = jsonColor.Color.ToObject<List<ColorLibrary>>();
                            }

                            List<SellerProduct> LSTProduct = new List<SellerProduct>();
                            if (bulkDetail.SellerProduct != null)
                            {
                                dynamic jsonLSTProduct = JsonConvert.DeserializeObject<dynamic>(bulkDetail.SellerProduct);
                                LSTProduct = jsonLSTProduct.SellerProduct.ToObject<List<SellerProduct>>();
                            }

                            #region  spec validation

                            List<AssignSpecValuesToCategoryLibrary> templstAssignSpecValueToCat = new List<AssignSpecValuesToCategoryLibrary>();

                            templstAssignSpecValueToCat = lstAssignSpecValueToCat.Where(p => p.IsAllowSpecInTitle == true).ToList();
                            templstAssignSpecValueToCat = templstAssignSpecValueToCat.GroupBy(p => new { p.SpecID, p.SpecTypeID }).Select(group => group.First()).ToList();

                            foreach (var item in templstAssignSpecValueToCat)
                            {
                                if (dtProduct.Columns.Contains(item.SpecificationName + "-" + item.SpecificationTypeName))
                                {
                                    var ReqSpe = dtProduct.AsEnumerable().Where(row => row[item.SpecificationName + "-" + item.SpecificationTypeName] == DBNull.Value || (row[item.SpecificationName + "-" + item.SpecificationTypeName] is string && string.IsNullOrWhiteSpace(row[item.SpecificationName + "-" + item.SpecificationTypeName].ToString()))).ToList();

                                    if (ReqSpe.Any())
                                    {
                                        foreach (DataRow dr in ReqSpe)
                                        {
                                            int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                                            errorSKULST.Add($"{item.SpecificationName + "-" + item.SpecificationTypeName} Specification is required found in row number {rowNumber}");
                                        }
                                        isSheetValid = false;
                                    }
                                }
                            }

                            #endregion
                            #region Datatable To List 
                            List<ProductDetails> lstProductDetailBulk = new List<ProductDetails>();
                            lstProductDetailBulk = (from DataRow dr in dtProduct.Rows
                                                    select new ProductDetails()
                                                    {
                                                        productId = dr.Table.Columns.Contains("Id") ? Convert.ToInt32(dr["Id"].ToString()) : 0,
                                                        IsMasterProduct = dr.Table.Columns.Contains("Id") ? false : true,
                                                        ParentId = 0,
                                                        CategoryId = model.CategoryId,
                                                        AssiCategoryId = assignSpec.Id,
                                                        TaxValueId = LSTAssiTax.Where(p => p.DisplayName == dr["HSN Code"].ToString()).FirstOrDefault().TaxValueId,
                                                        HSNCodeId = LSTAssiTax.Where(p => p.DisplayName == dr["HSN Code"].ToString()).FirstOrDefault().HsnCodeId,
                                                        ProductName = GenerateProductName(dr, model.BrandName, category.Name, LSTSize, lstAssignSpecValueToCat, dr.Table.Columns.Contains("Color") ? dr["Color"].ToString() : "", Convert.ToBoolean(assignSpec.IsAllowColorsInTitle), TitleSequenceOfColor),
                                                        CustomeProductName = dr["Custome Product Name"].ToString(),
                                                        CompanySKUCode = dr["Company SKU Code"].ToString(),
                                                        Description = dr["Description"].ToString(),
                                                        Highlights = dr["Highlights"].ToString(),
                                                        Keywords = dr["Keywords"].ToString(),
                                                        BrandID = Convert.ToInt32(model.BrandId),
                                                        SellerProducts = new SellerProductDTO()
                                                        {
                                                            Id = 0,
                                                            ProductID = 0,
                                                            SellerID = model.SellerId,
                                                            BrandID = Convert.ToInt32(model.BrandId),
                                                            SellerSKU = dr["Product SKU"].ToString(),
                                                            IsExistingProduct = false,
                                                            Live = false,
                                                            Status = "Bulk Upload",
                                                            PackingLength = !string.IsNullOrEmpty(dr["Packing_L"]?.ToString()) ? Convert.ToDecimal(dr["Packing_L"].ToString()) : 0,
                                                            PackingBreadth = !string.IsNullOrEmpty(dr["Packing_B"]?.ToString()) ? Convert.ToDecimal(dr["Packing_B"].ToString()) : 0,
                                                            PackingHeight = !string.IsNullOrEmpty(dr["Packing_H"]?.ToString()) ? Convert.ToDecimal(dr["Packing_H"].ToString()) : 0,
                                                            PackingWeight = !string.IsNullOrEmpty(dr["PackingWeight"]?.ToString()) ? Convert.ToDecimal(dr["PackingWeight"].ToString()) : 0,
                                                            WeightSlabId = LSTWeight.Where(p => p.WeightSlab == dr["Weight"].ToString()).FirstOrDefault().Id,

                                                            ProductPrices = new List<ProductPriceDTO>()
                                                                                        {
                                                                                            new ProductPriceDTO()
                                                                                            {
                                                                                                Id = 0,
                                                                                                SellerProductId = 0,
                                                                                                MRP = Convert.ToDecimal(dr["MRP"].ToString()),
                                                                                                SellingPrice = Convert.ToDecimal(dr["Selling Price"].ToString()),
                                                                                                Discount = ((Convert.ToDecimal(dr["MRP"].ToString()) - Convert.ToDecimal(dr["Selling Price"].ToString())) / Convert.ToDecimal(dr["MRP"].ToString())) * 100,
                                                                                                Quantity = BProductQty(dr,model.SellerId),
                                                                                                SizeTypeName =dr.Table.Columns.Contains("Size") ? dr["Size"].ToString().Split('-')[1].Replace("(","").Replace(")","").ToString() : null,
                                                                                                SizeName = dr.Table.Columns.Contains("Size") ? dr["Size"].ToString().Split('-')[0].ToString() : null,
                                                                                                //SizeID = LSTSize.Where(p => p.SizeTypeName == dr["Size"].ToString().Split('-')[1].Replace("(","").Replace(")","").ToString() && p.SizeName == dr["Size"].ToString().Split('-')[0].ToString()).FirstOrDefault().Id,
                                                                                                SizeID = assignSpec.IsAllowSize || dr.Table.Columns.Contains("Size") ? LSTSize.Where(p => p.SizeTypeName == dr["Size"].ToString().Split('-')[1].Substring(1,dr["Size"].ToString().Split('-')[1].ToString().Length-2).ToString()  && p.SizeName == dr["Size"].ToString().Split('-')[0].ToString()).FirstOrDefault().SizeId : null,

                                                                    ProductWarehouses = BProductWarehouseQty(dr,model.SellerId),
                                                                },
                                                            },
                                                        },
                                                        ProductColorMapping = assignSpec.IsAllowColors ? new List<ProductColorDTO>
                                                                                    {
                                                                                        new ProductColorDTO()
                                                                                        {
                                                                                            ColorId = LSTColor.Count > 0 || dr.Table.Columns.Contains("Color") ? LSTColor.Where(p => p.Name == dr["Color"].ToString()).FirstOrDefault()?.Id ?? 0 : 0, // Using null coalescing operator
                                                                                            ColorName = dr.Table.Columns.Contains("Color") ? dr["Color"].ToString() : null,
                                                                                        }

                                                                                    } : new List<ProductColorDTO>(),
                                                        ProductImage = BProductImage(dr),
                                                        //ProductVideoLinks = BProductVideo(dr),
                                                        ProductSpecificationsMapp = assignSpec.IsAllowSpecifications ? BProductSpecification(dr, lstAssignSpecValueToCat) : null
                                                    }).ToList();
                            #endregion

                            string eclVarintSku = "";


                            #region Company SKU Validation
                            List<ProductDetails> errorList = lstProductDetailBulk.Where(item => LSTProduct.Any(e => item.CompanySKUCode.ToLower() == e.CompanySKUCode.ToLower() && item.SellerProducts.Id != e.Id)).ToList();

                            eclVarintSku = "";
                            foreach (var item in errorList)
                            {
                                eclVarintSku = string.IsNullOrEmpty(eclVarintSku) ? item.CompanySKUCode : eclVarintSku + " , " + item.CompanySKUCode;
                            }

                            if (errorList.Count > 0)
                            {
                                res.Message = "Error in Sheet.";
                                res.code = 201;
                                res.Data = null;
                                errorSKULST.Add("Company SKU code already exist. Product Sku is " + eclVarintSku);
                                isSheetValid = false;
                                //return res;
                            }

                            #endregion

                            #region Seller SKU Validation

                            errorList = lstProductDetailBulk.Where(item => LSTProduct.Any(e => item.SellerProducts.SellerID == model.SellerId && item.SellerProducts.SellerSKU.ToLower() == e.SKUCode.ToLower() && item.SellerProducts.Id != e.Id)).ToList();

                            eclVarintSku = "";
                            foreach (var item in errorList)
                            {
                                eclVarintSku = string.IsNullOrEmpty(eclVarintSku) ? item.SellerProducts.SellerSKU : eclVarintSku + " , " + item.SellerProducts.SellerSKU;
                            }

                            if (errorList.Count > 0)
                            {
                                res.Message = "Error in Sheet.";
                                res.code = 201;
                                res.Data = null;
                                errorSKULST.Add("Seller SKU code already exist. Product Sku is " + eclVarintSku);
                                isSheetValid = false;
                                //return res;
                            }

                            #endregion

                            #region MRP Validation

                            errorList = lstProductDetailBulk.Where(item => item.SellerProducts.ProductPrices.AsEnumerable().Any(e => e.SellingPrice > e.MRP)).ToList();

                            eclVarintSku = "";
                            foreach (var item in errorList)
                            {
                                eclVarintSku = string.IsNullOrEmpty(eclVarintSku) ? item.CompanySKUCode : eclVarintSku + " , " + item.CompanySKUCode;
                            }

                            if (errorList.Count > 0)
                            {
                                res.Message = "Error in Sheet.";
                                res.code = 201;
                                res.Data = errorList;
                                errorSKULST.Add("MRP must be greater than Selling Price. Product Sku is " + eclVarintSku);
                                isSheetValid = false;
                                //return res;
                            }

                            #endregion

                            #region Save Product in Database 

                            if (isSheetValid)
                            {
                                string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                                SaveProduct saveProduct = new SaveProduct(_configuration, _httpContext, userID);

                                string msg = null;
                                int i = 0;
                                int mastId = 0;
                                foreach (ProductDetails item in lstProductDetailBulk)
                                {
                                    try
                                    {
                                        item.ParentId = item.IsMasterProduct ? 0 : mastId;

                                        var res1 = saveProduct.SaveData(item, true);
                                        mastId = Convert.ToInt32(res1.Data.ToString().Split(",")[1].Split(":")[1]);
                                    }
                                    catch (Exception ex)
                                    {
                                        msg = "Exp " + i + " in SKU (" + item.CompanySKUCode + "):" + ex.Message;

                                        errorSKULST.Add(msg);
                                        i++;
                                    }
                                }

                                res.Message = "Bulk uploaded.";
                                res.code = 200;
                                res.Data = null;
                            }
                            #endregion
                        }

                        #endregion
                    }
                    System.IO.File.Delete(fullPath);
                    res.Data = errorSKULST;
                }

            }
            catch
            {

                res.Message = "Error in Sheet.";
                res.code = 201;
                res.Data = null;
            }
            return Ok(res);
        }

        [NonAction]
        public string GenerateProductName(DataRow dr, string BrandName, string CategoryName, List<AssignSizeValueToCategory> LSTSize, List<AssignSpecValuesToCategoryLibrary> lstAssignSpecValueToCat, string colorName, bool IsAllowColorInTitle, int ColorInTitleSeq)
        {
            //Dictionary<int, string> titleSequenceNames = new Dictionary<int, string>();
            List<KeyValuePairItem> titleSequenceNames = new List<KeyValuePairItem>();

            LSTSize = LSTSize.Where(p => p.IsAllowSizeInTitle == true).ToList();
            if (LSTSize != null)
            {
                LSTSize = LSTSize.Where(p => p.SizeTypeName == (dr["Size"].ToString().Split('-')[1].Substring(1, dr["Size"].ToString().Split('-')[1].ToString().Length - 2).ToString()) && p.SizeName == (dr["Size"].ToString().Split('-')[0].ToString())).ToList();

                if (LSTSize.Count > 0)
                {
                    //titleSequenceNames.Add(Convert.ToString(LSTSize[0].TitleSequenceOfSize), LSTSize[0].SizeName);
                    KeyValuePairItem newItem = new KeyValuePairItem
                    {
                        Key = Convert.ToString(LSTSize[0].TitleSequenceOfSize),
                        Value = LSTSize[0].SizeName
                    };
                    titleSequenceNames.Add(newItem);

                }
            }

            if (IsAllowColorInTitle)
            {
                KeyValuePairItem newItem = new KeyValuePairItem
                {
                    Key = Convert.ToString(ColorInTitleSeq),
                    Value = colorName
                };
                titleSequenceNames.Add(newItem);
            }

            List<ProductSpecificationMappingDto> nn = new List<ProductSpecificationMappingDto>();

            List<AssignSpecValuesToCategoryLibrary> templstAssignSpecValueToCat = new List<AssignSpecValuesToCategoryLibrary>();

            templstAssignSpecValueToCat = lstAssignSpecValueToCat.Where(p => p.IsAllowSpecInTitle == true).ToList();
            templstAssignSpecValueToCat = templstAssignSpecValueToCat.GroupBy(p => new { p.SpecID, p.SpecTypeID }).Select(group => group.First()).ToList();

            foreach (var item in templstAssignSpecValueToCat)
            {
                int TitleSequenceOfSpecification = dr.Table.Columns.Contains(item.SpecificationName + "-" + item.SpecificationTypeName) ? Convert.ToInt32(item.TitleSequenceOfSpecification) : 0;
                string Value = dr.Table.Columns.Contains(item.SpecificationName + "-" + item.SpecificationTypeName) ? dr[item.SpecificationName + "-" + item.SpecificationTypeName].ToString() : "";

                if (!string.IsNullOrEmpty(Value))
                {
                    //titleSequenceNames.Add(Convert.ToInt32(TitleSequenceOfSpecification), Value);
                    KeyValuePairItem newItem = new KeyValuePairItem
                    {
                        Key = Convert.ToString(TitleSequenceOfSpecification),
                        Value = Value
                    };
                    titleSequenceNames.Add(newItem);
                }
            }


            List<string> sortedTitleSequenceNames = titleSequenceNames.OrderBy(x => x.Key).Select(item => item.Value).ToList();
            string concatenatedValues = string.Join(" ", sortedTitleSequenceNames);
            string Name = BrandName + " " + CategoryName + " " + concatenatedValues;
            return Name;


        }



        [HttpGet("downloadProductForStock")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> DownloadProductForStock(string? sellerId = null)
        {


            int rowCount = 0;
            #region XL Sheet Header
            List<int> totalCount = new List<int>();

            DataTable dtProduct = new DataTable();
            dtProduct.Columns.Add("ProductId", typeof(string));
            dtProduct.Columns.Add("SellerProductId", typeof(string));
            dtProduct.Columns.Add("Company SKU Code", typeof(string));
            dtProduct.Columns.Add("Product SKU", typeof(string));
            dtProduct.Columns.Add("Custome Product Name", typeof(string));
            dtProduct.Columns.Add("SizeId", typeof(string));
            dtProduct.Columns.Add("Size", typeof(string));
            dtProduct.Columns.Add("MRP", typeof(string));
            dtProduct.Columns.Add("Selling Price", typeof(string));


            BaseResponse<Warehouse> WarebaseResponse = new BaseResponse<Warehouse>();
            var response = helper.ApiCall(userUrl, EndPoints.Warehouse + "?UserID=" + sellerId, "GET", null);
            WarebaseResponse = WarebaseResponse.JsonParseList(response);

            List<Warehouse> warehouses = (List<Warehouse>)WarebaseResponse.Data;
            bool AllowWarehouseManagement = Convert.ToBoolean(_configuration.GetValue<string>("Allow_warehouse_management"));
            if (AllowWarehouseManagement)
            {

                int wcount = 1;
                foreach (var item in warehouses)
                {
                    dtProduct.Columns.Add(item.Name + " (WareHouse-" + wcount + ")", typeof(string));
                    wcount++;
                }
            }
            else
            {
                dtProduct.Columns.Add("Quantity", typeof(string));
            }

            #endregion

            #region add products data 

            string url = string.Empty;

            if (!string.IsNullOrEmpty(sellerId) && sellerId != "")
            {
                url += "&SellerId=" + sellerId;
            }

            BaseResponse<ProductBulkDownloadForStock> baseResponseProduct = new BaseResponse<ProductBulkDownloadForStock>();
            var responseproduct = helper.ApiCall(CatalogueUrl, EndPoints.ProductBulkDownloadForStock + "?" + url, "GET", null);

            baseResponseProduct = baseResponseProduct.JsonParseList(responseproduct);

            List<ProductBulkDownloadForStock> tempList = (List<ProductBulkDownloadForStock>)baseResponseProduct.Data;

            List<ProductBulkDownloadForStock> childProductList = tempList.Where(p => p.flag == "p").ToList();

            int cn = 0;
            foreach (var item in childProductList)
            {

                dtProduct.Rows.Add();
                dtProduct.Rows[cn]["ProductID"] = item.ProductID.ToString();
                dtProduct.Rows[cn]["SellerProductId"] = item.SellerProductId.ToString();
                dtProduct.Rows[cn]["Company SKU Code"] = item.CompanySKUCode.ToString();
                dtProduct.Rows[cn]["Product SKU"] = item.SellerSKUCode.ToString();
                dtProduct.Rows[cn]["Custome Product Name"] = item.CustomeProductName.ToString();
                dtProduct.Rows[cn]["MRP"] = item.MRP.ToString();
                dtProduct.Rows[cn]["Selling Price"] = item.SellingPrice.ToString();
                dtProduct.Rows[cn]["SizeId"] = item.SizeId.ToString();
                dtProduct.Rows[cn]["Size"] = item.SizeName.ToString();



                if (AllowWarehouseManagement)
                {
                    List<ProductBulkDownloadForStock> warehouseProductList = tempList.Where(p => p.flag == "w" && p.ProductID == item.ProductID).ToList();

                    int wcount = 1;
                    foreach (var wareitem in warehouses)
                    {
                        if (warehouseProductList.Where(p => p.WarehouseId == wareitem.Id && p.ProductID == item.ProductID).Count() > 0)
                        {

                            dtProduct.Rows[cn][wareitem.Name + " (WareHouse-" + wcount + ")"] = warehouseProductList.Where(p => p.WarehouseId == wareitem.Id && p.ProductID == item.ProductID).Select(p => p.WarehouseProductQty).FirstOrDefault().ToString();
                        }
                        else
                        {
                            dtProduct.Rows[cn][wareitem.Name + " (WareHouse-" + wcount + ")"] = 0;
                        }
                        wcount++;
                    }
                }
                else
                {
                    dtProduct.Rows[cn]["Quantity"] = item.Quantity.ToString();

                }

                cn++;
            }
            #endregion


            #region XL Sheet Data bind

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(dtProduct, "Product");



                var DeliveryRange = ws.RangeUsed();


                int count = 0;

                ws.Tables.FirstOrDefault().ShowAutoFilter = false;



                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "StockUpdate_ProductSheet.xlsx");
                }
            }
            #endregion
        }

        [HttpPost]
        [Route("BulkuploadForBulkStock")]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> BulkuploadForBulkStock([FromForm] BulkProduct model)
        {
            BaseResponse<string> res = new BaseResponse<string>();

            try
            {
                #region Save File
                if (model.File == null)
                {
                    throw new Exception("File is Not Received...");
                }

                string FileName = Path.GetFileNameWithoutExtension(model.File.FileName);

                string extension = Path.GetExtension(model.File.FileName);
                string FullName = FileName + DateTime.Now.ToString("ddMMyyyyfffhhmsff") + extension;

                string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

                if (!allowedExtsnions.Contains(extension))
                {
                    throw new Exception("Sorry! This file is not allowed,make sure that file having extension as either.xls or.xlsx is uploaded.");

                }


                if (!Directory.Exists("Resources" + "\\Excel" + "\\Product"))
                {
                    Directory.CreateDirectory("Resources" + "\\Excel" + "\\Product");
                }
                var folderName = Path.Combine("Resources", "Excel", "Product");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);



                var fullPath = Path.Combine(pathToSave, FullName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    model.File.CopyTo(stream);
                }
                #endregion

                DataTable dtProduct = conExcelTODataTable(fullPath, "Product");

                List<string> errorSKULST = new List<string>();
                bool isSheetValid = true;



                #region CheckValidation

                #region MRP

                var ReqMrp = dtProduct.AsEnumerable().Where(row => row["MRP"] == DBNull.Value || (row["MRP"] is string && string.IsNullOrWhiteSpace(row["MRP"].ToString()))).ToList();

                if (ReqMrp.Any())
                {
                    foreach (DataRow dr in ReqMrp)
                    {
                        int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                        errorSKULST.Add($"MRP is required found in row number {rowNumber}");
                    }
                    isSheetValid = false;
                }

                #endregion

                #region Selling Price

                var ReqSellingPrice = dtProduct.AsEnumerable().Where(row => row["Selling Price"] == DBNull.Value || (row["Selling Price"] is string && string.IsNullOrWhiteSpace(row["Selling Price"].ToString()))).ToList();

                if (ReqSellingPrice.Any())
                {
                    foreach (DataRow dr in ReqSellingPrice)
                    {
                        int rowNumber = dtProduct.Rows.IndexOf(dr) + 1; // Adding 1 to convert from zero-based index to 1-based row number
                        errorSKULST.Add($"Selling Price is required found in row number {rowNumber}");
                    }
                    isSheetValid = false;
                }

                #endregion





                #endregion

                if (isSheetValid)
                {

                    string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    UpdateProduct updateProduct = new UpdateProduct(_configuration, _httpContext, userID);


                    List<ProductBulkDownloadForStock> objProductBulk = new List<ProductBulkDownloadForStock>();


                    objProductBulk = (from DataRow dr in dtProduct.Rows
                                      select new ProductBulkDownloadForStock()
                                      {

                                          ProductID = Convert.ToInt32(dr["ProductID"].ToString()),
                                          SellerProductId = Convert.ToInt32(dr["SellerProductId"].ToString()),
                                          CompanySKUCode = dr["Company SKU Code"].ToString(),
                                          SellerSKUCode = dr["Product SKU"].ToString(),
                                          CustomeProductName = dr["Custome Product Name"].ToString(),
                                          SizeId = string.IsNullOrEmpty(dr["SizeId"].ToString()) ? 0 : Convert.ToInt32(dr["SizeId"].ToString()),
                                          SizeName = string.IsNullOrEmpty(dr["Size"].ToString()) ? null : dr["Size"].ToString(),
                                          MRP = Convert.ToDecimal(dr["MRP"].ToString()),
                                          SellingPrice = Convert.ToDecimal(dr["Selling Price"].ToString()),
                                          Discount = ((Convert.ToDecimal(dr["MRP"].ToString()) - Convert.ToDecimal(dr["Selling Price"].ToString())) / Convert.ToDecimal(dr["MRP"].ToString())) * 100,
                                          Quantity = Convert.ToInt32(dr["SellerProductId"].ToString()),
                                          WarehouseId = Convert.ToInt32(dr["SellerProductId"].ToString()),
                                          WarehouseProductQty = dr["SellerProductId"].ToString(),

                                      }).ToList();


                    List<ProductPriceDTO> tempobjProductBulk = (
                        from DataRow dr in dtProduct.Rows
                        select new ProductPriceDTO()
                        {

                            Id = 0,
                            SellerProductId = Convert.ToInt32(dr["SellerProductId"].ToString()),
                            MRP = Convert.ToDecimal(dr["MRP"].ToString()),
                            SellingPrice = Convert.ToDecimal(dr["Selling Price"].ToString()),
                            Discount = ((Convert.ToDecimal(dr["MRP"].ToString()) - Convert.ToDecimal(dr["Selling Price"].ToString())) / Convert.ToDecimal(dr["MRP"].ToString())) * 100,
                            Quantity = 0,
                            SizeID = string.IsNullOrEmpty(dr["SizeId"].ToString()) ? null : Convert.ToInt32(dr["SizeId"].ToString()),
                            SizeName = string.IsNullOrEmpty(dr["Size"].ToString()) ? null : dr["Size"].ToString(),
                            SizeTypeName = "",
                            ProductWarehouses = BProductWarehouseQty(dr, model.SellerId),

                        }).ToList();



                    objProductBulk = objProductBulk.DistinctBy(p => p.SellerProductId).ToList();


                    List<int> sellerProductId = objProductBulk.Select(p => p.SellerProductId).Distinct().ToList();

                    foreach (var item in sellerProductId)
                    {
                        List<ProductPriceDTO> obj = tempobjProductBulk.Where(p => p.SellerProductId == item).ToList();
                        List<ProductPrice> pps = updateProduct.BindProductPrices(obj, Convert.ToInt32(item));
                        foreach (var pp in pps)
                        {
                            BaseResponse<ProductPrice> PbaseResponse = new BaseResponse<ProductPrice>();

                            if (pp.Id != null && pp.Id != 0)
                            {
                                var response = helper.ApiCall(CatalogueUrl, EndPoints.ProductPriceMaster, "PUT", pp);
                            }

                        }
                        var ProductPrices = updateProduct.ProductPricesEntries(obj, Convert.ToInt32(item), objProductBulk.Where(p => p.SellerProductId == item).Select(p => p.ProductID).FirstOrDefault());

                    }


                    string dd = "";
                     
                    
                }
                System.IO.File.Delete(fullPath);
                res.Data = errorSKULST;


            }
            catch
            {

                res.Message = "Error in Sheet.";
                res.code = 201;
                res.Data = null;
            }
            return Ok(res);
        }

        [HttpPost("updateCorrection")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult updateCorrection()
        {
            BaseResponse<SellerListModel> baseResponse = new BaseResponse<SellerListModel>();
            BaseResponse<AssignBrandToSeller> bbaseResponse = new BaseResponse<AssignBrandToSeller>();
            BaseResponse<ProductExtraDetailsDto> ExtraDetailsbaseResponse = new BaseResponse<ProductExtraDetailsDto>();

            //var response = helper.ApiCall(IdentityServerUrl, EndPoints.SellerList + "?pageIndex=0&pageSize=0", "GET", null);
            //baseResponse = baseResponse.JsonParseList(response);
            //if (baseResponse.code == 200)
            //{
            //    List<SellerListModel> sellerLists = (List<SellerListModel>)baseResponse.Data;
            //    sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            //    sellerLists = sellerLists.Where(x => x.Status.ToLower() != "archived").ToList();
            //    //List<SellerKycList> lst = seller.bindSellerDetails(sellerLists);
            //    List<SellerKycList> lst = seller.bindSellerDetails(sellerLists);

            //    foreach (var item in lst)
            //    {
            //        ProductExtraDetailsDto productExtraDetails = new ProductExtraDetailsDto();
            //        productExtraDetails.FullName = item.FirstName + " " + item.LastName;
            //        productExtraDetails.UserName = item.EmailID;
            //        productExtraDetails.PhoneNumber = item.PhoneNumber;
            //        productExtraDetails.SellerStatus = item.SellerStatus;
            //        productExtraDetails.SellerId = item.Id;
            //        productExtraDetails.Mode = "updateSeller";
            //        var ExtraDetailsresponse = helper.ApiCall(CatalogueUrl, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
            //        ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);

            //        productExtraDetails.TradeName = item.LegalName;
            //        productExtraDetails.LegalName = item.TradeName;
            //        productExtraDetails.SellerId = item.UserID;
            //        productExtraDetails.Mode = "updateSellerGST";
            //        ExtraDetailsresponse = helper.ApiCall(CatalogueUrl, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
            //        ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);

            //        productExtraDetails.DisplayName = item.DisplayName;
            //        productExtraDetails.DigitalSign = item.DigitalSign;
            //        productExtraDetails.ShipmentBy = item.ShipmentBy;
            //        productExtraDetails.ShipmentChargesPaidBy = item.ShipmentChargesPaidBy.ToString();
            //        productExtraDetails.ShipmentChargesPaidByName = item.ShipmentChargesPaidByName;
            //        productExtraDetails.KycStatus = item.Status;
            //        productExtraDetails.SellerId = item.UserID;
            //        productExtraDetails.Mode = "updateSellerKyc";
            //        ExtraDetailsresponse = helper.ApiCall(CatalogueUrl, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
            //        ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);
            //    }

            //}

            var bresponse = helper.ApiCall(userUrl, EndPoints.AssignBrandToSeller + "?pageIndex=0&pageSize=0", "GET", null);
            bbaseResponse = bbaseResponse.JsonParseList(bresponse);
            if (bbaseResponse.code == 200)
            {
                List<AssignBrandToSeller> BrandLists = (List<AssignBrandToSeller>)bbaseResponse.Data;

                foreach (var item in BrandLists)
                {


                    ProductExtraDetailsDto productExtraDetails = new ProductExtraDetailsDto();
                    //productExtraDetails.AssignBrandStatus = item.Status;
                    //productExtraDetails.BrandId = item.BrandId;
                    //productExtraDetails.SellerId = item.SellerID;
                    //productExtraDetails.Mode = "updateAssignBrands";
                    //var ExtraDetailsresponse = helper.ApiCall(CatalogueUrl, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
                    //ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);

                    productExtraDetails.BrandStatus = item.BrandStatus;
                    productExtraDetails.BrandName = item.BrandName;
                    productExtraDetails.BrandLogo = item.Logo;
                    productExtraDetails.BrandId = item.BrandId;
                    productExtraDetails.Mode = "updateBrands";
                    var ExtraDetailsresponse = helper.ApiCall(CatalogueUrl, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
                    ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);
                }
            }

            return Ok(true);
        }

    }

}
public class KeyValuePairItem
{
    public string Key { get; set; }
    public string Value { get; set; }
}