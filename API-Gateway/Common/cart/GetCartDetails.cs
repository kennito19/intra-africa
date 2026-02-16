using API_Gateway.Common.products;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.Order;
using API_Gateway.Models.Entity.User;
using Irony.Parsing;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace API_Gateway.Common.cart
{
    public class GetCartDetails
    {
        BaseResponse<Cart> cartResponse = new BaseResponse<Cart>();
        BaseResponse<SellerProductDetails> sellerProductResponse = new BaseResponse<SellerProductDetails>();
        BaseResponse<GSTInfo> gstInfoResponse = new BaseResponse<GSTInfo>();
        BaseResponse<SellerListModel> sellerResponse = new BaseResponse<SellerListModel>();
        BaseResponse<SellerKycList> sellerKycResponse = new BaseResponse<SellerKycList>();
        BaseResponse<BrandLibrary> brandResponse = new BaseResponse<BrandLibrary>();
        BaseResponse<ColorLibrary> colorResponse = new BaseResponse<ColorLibrary>();
        BaseResponse<TaxTypeValueDTO> taxResponse = new BaseResponse<TaxTypeValueDTO>();
        BaseResponse<ManageCollectionDTO> collectionResponse = new BaseResponse<ManageCollectionDTO>();
        BaseResponse<ManageCollectionMappingDTO> collectionmappResponse = new BaseResponse<ManageCollectionMappingDTO>();
        BaseResponse<FlashSalePriceMasterDTO> flashSaleResponse = new BaseResponse<FlashSalePriceMasterDTO>();
        BaseResponse<ManageOffersDTO> offerResponse = new BaseResponse<ManageOffersDTO>();
        BaseResponse<checkOfferDto> checkofferResponse = new BaseResponse<checkOfferDto>();
        private readonly IConfiguration _configuration;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        public string IdserverURL = string.Empty;
        public string Catalougeurl = string.Empty;
        private ManageOffers manageOffers;
        public GetCartDetails(IConfiguration configuration, HttpContext httpContext)
        {
            _configuration = configuration;
            _httpContext = httpContext;
            helper = new ApiHelper(_httpContext);
            IdserverURL = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            Catalougeurl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            manageOffers = new ManageOffers(_configuration, Catalougeurl, _httpContext);
        }
        public List<Cart> GetCartList(string CartSessionId, string URL, string? UserId = null, int? CartID = null)
        {
            List<Cart> lstCart = new List<Cart>();
            var response = new HttpResponseMessage();
            string url = "";
            if (CartID != null || CartID != 0)
            {
                url = "&id=" + CartID;
            }
            if (CartSessionId != null)
            {
                response = helper.ApiCall(URL, EndPoints.Cart + "?sessionId=" + CartSessionId + url, "GET", null);
                cartResponse = cartResponse.JsonParseList(response);
                lstCart = (List<Cart>)cartResponse.Data;
            }
            else if (UserId != null)
            {
                response = helper.ApiCall(URL, EndPoints.Cart + "?sessionId=" + CartSessionId + "&UserId=" + UserId + url, "GET", null);
                cartResponse = cartResponse.JsonParseList(response);
                lstCart = (List<Cart>)cartResponse.Data;
            }
            return lstCart;
        }

        public List<SellerProductDetails> GetSellerproductDetails(int SellerProductId, string URL)
        {
            List<SellerProductDetails> sellerProduct = new List<SellerProductDetails>();
            var response = new HttpResponseMessage();
            if (SellerProductId != null)
            {
                response = helper.ApiCall(URL, EndPoints.SellerProduct + "/getSellerProductDetails?sellerProductId=" + SellerProductId, "GET", null);
                sellerProductResponse = sellerProductResponse.JsonParseList(response);
                sellerProduct = (List<SellerProductDetails>)sellerProductResponse.Data;

            }
            return sellerProduct;
        }

        public List<GSTInfo> GetGstDetail(string UserId, string URL)
        {
            GSTInfo GSTInfo = new GSTInfo();
            List<GSTInfo> collectionlst = new List<GSTInfo>();
            var response = helper.ApiCall(URL, EndPoints.GSTInfo + "?UserID=" + UserId, "GET", null);
            gstInfoResponse = gstInfoResponse.JsonParseList(response);
            collectionlst = (List<GSTInfo>)gstInfoResponse.Data;
            return collectionlst;
        }

        public DeliveryLibrary GetDelivery(string pincode, string URL)
        {
            BaseResponse<DeliveryLibrary> baseResponse = new BaseResponse<DeliveryLibrary>();
            DeliveryLibrary DeliveryData = new DeliveryLibrary();

            var response = helper.ApiCall(URL, EndPoints.Delivery + "?pincode=" + pincode, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            DeliveryData = (DeliveryLibrary)baseResponse.Data;
            return DeliveryData;
        }


        public SellerListModel getseller(string sellerId, string URL)
        {
            var response = helper.ApiCall(URL, EndPoints.SellerById + "?ID=" + sellerId, "GET", null);
            sellerResponse = sellerResponse.JsonParseRecord(response);

            SellerListModel seller = new SellerListModel();
            seller = (SellerListModel)sellerResponse.Data;

            return seller;
        }

        //public SellerKycList getsellerKyc(string sellerId)
        //{
        //    BaseResponse<SellerListModel> baseResponse2 = new BaseResponse<SellerListModel>();
        //    SellerKycList sellerKyc = new SellerKycList();
        //    var response2 = helper.ApiCall(IdserverURL, EndPoints.SellerById + "?ID=" + sellerId, "GET", null);
        //    baseResponse2 = baseResponse2.JsonParseList(response2);

        //    if (baseResponse2.code == 200)
        //    {
        //        List<SellerListModel> sellerLists = (List<SellerListModel>)baseResponse2.Data;
        //        sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
        //        List<SellerKycList> lst = seller.bindSellerDetails(sellerLists);
        //        sellerKyc = lst.FirstOrDefault();
        //    }

        //    //var response = helper.ApiCall(URL, EndPoints.KYCDetails + "?UserID=" + sellerId, "GET", null);
        //    //sellerKycResponse = sellerKycResponse.JsonParseRecord(response);

        //    //SellerKycList sellerKyc = new SellerKycList();
        //    //sellerKyc = (SellerKycList)sellerKycResponse.Data;

        //    //BaseResponse<ChargesPaidByLibrary> baseResponseCharges = new BaseResponse<ChargesPaidByLibrary>();
        //    //var response1_1 = helper.ApiCall(CatelogueURL, EndPoints.ChargesPaidBy + "?pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
        //    //baseResponseCharges = baseResponseCharges.JsonParseList(response1_1);
        //    //List<ChargesPaidByLibrary> Charges = (List<ChargesPaidByLibrary>)baseResponseCharges.Data;
        //    //string CName = sellerKyc.ShipmentChargesPaidBy != null ? Charges.Where(p => p.Id == sellerKyc.ShipmentChargesPaidBy).FirstOrDefault().Name : null;
        //    //sellerKyc.ShipmentChargesPaidByName = CName;

        //    return sellerKyc;
        //}

        public SellerKycList getsellerKyc(string sellerId)
        {
            UserDetailsDTO sellerKycList = new UserDetailsDTO();
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            List<UserDetailsDTO> lst = seller.bindSellerDetails(null, null, null, null, sellerId, 0, 0);
            sellerKycList = lst.FirstOrDefault();

            SellerKycList slst = new SellerKycList();

            List<GSTInfo> gSTInfolst = new List<GSTInfo>();
            GSTInfo GSTDetails = new GSTInfo();
            string json = sellerKycList.GSTInfoDetails.ToString();
            gSTInfolst = JsonConvert.DeserializeObject<List<GSTInfo>>(json);
            if (gSTInfolst.Count > 0)
            {
                GSTDetails = gSTInfolst.Where(p => p.IsHeadOffice == true).FirstOrDefault();
            }

            slst.Id = sellerKycList.Id;
            slst.FirstName = sellerKycList.FirstName;
            slst.LastName = sellerKycList.LastName;
            slst.FullName = sellerKycList.FirstName + " " + sellerKycList.LastName;
            slst.EmailID = sellerKycList.Email;
            slst.PhoneNumber = sellerKycList.Phone;
            slst.ProfileImage = sellerKycList.ProfileImage;
            slst.SellerStatus = sellerKycList.UserStatus;
            slst.IsEmailConfirmed = sellerKycList.IsEmailConfirmed;
            slst.IsPhoneConfirmed = sellerKycList.IsPhoneConfirmed;
            slst.KycId = sellerKycList.KYCDetailsId;
            slst.KYCFor = sellerKycList.KYCFor;
            slst.DisplayName = sellerKycList.DisplayName;
            slst.OwnerName = sellerKycList.OwnerName;
            slst.ContactPersonName = sellerKycList.ContactPersonName;
            slst.ContactPersonMobileNo = sellerKycList.ContactPersonMobileNo;
            slst.PanCardNo = sellerKycList.PanCardNo;
            slst.NameOnPanCard = sellerKycList.NameOnPanCard;
            slst.DateOfBirth = sellerKycList.DateOfBirth;
            slst.AadharCardNo = sellerKycList.AadharCardNo;
            slst.IsUserWithGST = sellerKycList.IsUserWithGST;
            slst.TypeOfCompany = sellerKycList.TypeOfCompany;
            slst.CompanyRegistrationNo = sellerKycList.CompanyRegistrationNo;
            slst.BussinessType = sellerKycList.BussinessType;
            slst.MSMENo = sellerKycList.MSMENo;
            slst.Logo = sellerKycList.Logo;
            slst.DigitalSign = sellerKycList.DigitalSign;
            slst.ShipmentBy = sellerKycList.ShipmentBy;
            slst.ShipmentChargesPaidBy = sellerKycList.ShipmentChargesPaidBy;
            slst.ShipmentChargesPaidByName = sellerKycList.ShipmentChargesPaidByName;
            slst.Note = sellerKycList.Note;
            slst.Status = sellerKycList.UserStatus;
            slst.TradeName = GSTDetails.TradeName;
            slst.LegalName = GSTDetails.LegalName;
            slst.GSTNo = GSTDetails.GSTNo;
            slst.GSTType = GSTDetails.GSTType;
            slst.RegisteredAddressLine1 = GSTDetails.RegisteredAddressLine1;
            slst.RegisteredAddressLine2 = GSTDetails.RegisteredAddressLine2;
            slst.RegisteredLandmark = GSTDetails.RegisteredLandmark;
            slst.RegisteredPincode = GSTDetails.RegisteredPincode;
            slst.City = GSTDetails.CityName;
            slst.State = GSTDetails.StateName;
            slst.Country = GSTDetails.CountryName;
            slst.GSTStatus = GSTDetails.Status;


            return slst;
        }
        
        public BrandLibrary getBrand(int brandId, string URL)
        {
            var response = helper.ApiCall(URL, EndPoints.Brand + "?Id=" + brandId, "GET", null);
            brandResponse = brandResponse.JsonParseRecord(response);

            BrandLibrary brand = new BrandLibrary();
            brand = (BrandLibrary)brandResponse.Data;

            return brand;
        }

        public ColorLibrary getColor(int colorId, string URL)
        {
            var response = helper.ApiCall(URL, EndPoints.Color + "?Id=" + colorId, "GET", null);
            colorResponse = colorResponse.JsonParseRecord(response);

            ColorLibrary color = new ColorLibrary();
            color = (ColorLibrary)colorResponse.Data;

            return color;
        }

        public TaxTypeValueDTO getTax(int taxValueId, string URL)
        {
            var response = helper.ApiCall(URL, EndPoints.TaxTypeValue + "?Id=" + taxValueId, "GET", null);
            taxResponse = taxResponse.JsonParseRecord(response);

            TaxTypeValueDTO taxTypeValue = new TaxTypeValueDTO();
            taxTypeValue = (TaxTypeValueDTO)taxResponse.Data;

            return taxTypeValue;
        }

        public FlashSalePriceMasterDTO getFlashsalePrice(int ProductId, int SellerProductPriceId, string URL)
        {
            FlashSalePriceMasterDTO flashsale = new FlashSalePriceMasterDTO();

            var response = helper.ApiCall(URL, EndPoints.ManageCollection + "?PageIndex=0&PageSize=0&Type=Flashsale&Status=Active", "GET", null);
            collectionResponse = collectionResponse.JsonParseList(response);

            List<ManageCollectionDTO> collectionlst = new List<ManageCollectionDTO>();
            collectionlst = (List<ManageCollectionDTO>)collectionResponse.Data;

            if (collectionlst.Count > 0)
            {
                DateTime currentDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

                collectionlst = collectionlst.Where(p => p.StartDate <= currentDate && p.EndDate >= currentDate).OrderBy(p => p.StartDate).ToList();

                foreach (var item in collectionlst)
                {
                    var response1 = helper.ApiCall(URL, EndPoints.ManageCollectionMapping + "?CollectionId=" + item.Id + "&ProductId=" + ProductId, "GET", null);
                    collectionmappResponse = collectionmappResponse.JsonParseRecord(response1);
                    if (collectionmappResponse.code == 200)
                    {
                        ManageCollectionMappingDTO collectionmapp = new ManageCollectionMappingDTO();
                        collectionmapp = (ManageCollectionMappingDTO)collectionmappResponse.Data;

                        if (collectionmapp != null)
                        {
                            if (collectionmapp.Status.ToLower() == "active")
                            {
                                var response2 = helper.ApiCall(URL, EndPoints.FlashSalePriceMaster + "?CollectionId=" + item.Id + "&CollectionMappingId=" + collectionmapp.Id + "&SellerWiseProductPriceMasterId=" + SellerProductPriceId, "GET", null);
                                flashSaleResponse = flashSaleResponse.JsonParseList(response2);

                                FlashSalePriceMasterDTO _flashsale = new FlashSalePriceMasterDTO();
                                if (flashSaleResponse.code == 200)
                                {
                                    var _flashsalelst = (List<FlashSalePriceMasterDTO>)flashSaleResponse.Data;
                                    _flashsale = _flashsalelst.FirstOrDefault();
                                    if (_flashsale.Status.ToLower() == "active")
                                    {
                                        flashsale = _flashsale;
                                    }
                                    else
                                    {
                                        flashsale = null;
                                    }
                                }
                                else
                                {
                                    flashsale = null;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                flashsale = null;
            }


            return flashsale;
        }

        public checkOfferDto getOffers(string couponCode, int productId, int brandId, int categoryId, string sellerId, string userId, string paymentMode, string URL, string OrderURL)
        {
            checkOfferDto disOffers = new checkOfferDto();

            BaseResponse<ManageOffersLibrary> OfferItembaseResponse = new BaseResponse<ManageOffersLibrary>();
            OfferItembaseResponse = manageOffers.GetOfferByCode(couponCode, "Active");
            ManageOffersLibrary _OffersLibrary = new ManageOffersLibrary();
            if (OfferItembaseResponse.code == 200)
            {
                _OffersLibrary = (ManageOffersLibrary)OfferItembaseResponse.Data;
                DateTime currentDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                if (_OffersLibrary != null && !string.IsNullOrEmpty(_OffersLibrary.code) && (_OffersLibrary.startDate <= currentDate && _OffersLibrary.endDate >= currentDate))
                {
                    bool paymentoption = Convert.ToBoolean(_OffersLibrary.onlyForOnlinePayments) ? paymentMode == "online" ? true : false : false;

                    if (Convert.ToBoolean(_OffersLibrary.onlyForOnlinePayments))
                    {
                        if (paymentMode.ToLower() != "online")
                        {
                            disOffers.offers = null;
                            disOffers.CouponApplied = false;
                            disOffers.ValidOnlyOnline = paymentoption;
                            disOffers.message = "Coupon only applicable on online payment";
                        }
                        else
                        {
                            disOffers = checkOffer(_OffersLibrary, paymentoption, couponCode, productId, brandId, categoryId, sellerId, userId, paymentMode, URL, OrderURL);
                        }
                    }
                    else
                    {
                        disOffers = checkOffer(_OffersLibrary, paymentoption, couponCode, productId, brandId, categoryId, sellerId, userId, paymentMode, URL, OrderURL);
                    }

                }
                else
                {
                    disOffers.offers = null;
                    disOffers.CouponApplied = false;
                    disOffers.ValidOnlyOnline = false;
                    disOffers.message = "Coupon is invalid";
                }

            }
            else
            {
                disOffers.offers = null;
                disOffers.CouponApplied = false;
                disOffers.ValidOnlyOnline = false;
                disOffers.message = "Coupon is invalid";
            }

            return disOffers;
        }

        public GetProductDTO getProductDetails(int ProductId, int SellerProductId, string userID, int? sizeId = 0)
        {
            GetProduct product = new GetProduct(_configuration, _httpContext);
            GetProductDTO productDetails = new GetProductDTO();
            productDetails = product.GetProductDetails(ProductId, SellerProductId, sizeId, null, true, null, "active").Data as GetProductDTO;
            return productDetails;
        }

        public JObject getCodCharge(string URL, string OrderAmount)
        {
            decimal cod = 0;
            JObject CODObj = new JObject();
            BaseResponse<ManageConfigValueLibrary> configResponse = new BaseResponse<ManageConfigValueLibrary>();
            List<ManageConfigValueLibrary> manageConfigValue = new List<ManageConfigValueLibrary>();
            var response = helper.ApiCall(URL, EndPoints.ManageConfigValue + "?PageIndex=0&PageSize=0", "GET", null);

            configResponse = configResponse.JsonParseList(response);
            if (configResponse.code == 200)
            {
                manageConfigValue = (List<ManageConfigValueLibrary>)configResponse.Data;

                if (manageConfigValue.Count > 0)
                {
                    ManageConfigValueLibrary Hascodavailable = new ManageConfigValueLibrary();
                    Hascodavailable = manageConfigValue.Where(p => p.KeyName == "hasCOD").FirstOrDefault();
                    if (!string.IsNullOrEmpty(Hascodavailable.Value))
                    {
                        bool hascod = !string.IsNullOrEmpty(Hascodavailable.Value) ? true : false;
                        if (hascod)
                        {
                            ManageConfigValueLibrary GetMOVofCOD = new ManageConfigValueLibrary();
                            GetMOVofCOD = manageConfigValue.Where(p => p.KeyName == "free_cod_on_mov").FirstOrDefault();
                            if (!string.IsNullOrEmpty(GetMOVofCOD.Value) && Convert.ToDecimal(GetMOVofCOD.Value) != 0)
                            {
                                decimal movValue = Convert.ToDecimal(GetMOVofCOD.Value);
                                decimal Ordervalue = Convert.ToDecimal(OrderAmount);
                                if (Ordervalue <= movValue)
                                {
                                    ManageConfigValueLibrary GetCOD = new ManageConfigValueLibrary();
                                    GetCOD = manageConfigValue.Where(p => p.KeyName == "cod").FirstOrDefault();
                                    if (!string.IsNullOrEmpty(GetCOD.Value))
                                    {
                                        CODObj["codAvailable"] = true;
                                        CODObj["codCharges"] = Convert.ToDecimal(GetCOD.Value);
                                        CODObj["free_cod_on_mov"] = true;
                                        CODObj["movCharge"] = movValue;
                                        CODObj["hasFree"] = false;
                                        CODObj["message"] = "If your minimum order value is " + movValue + ", COD charges will be free.";
                                    }
                                    else
                                    {
                                        CODObj["codAvailable"] = true;
                                        CODObj["codCharges"] = 0;
                                        CODObj["hasFree"] = true;
                                        CODObj["free_cod_on_mov"] = false;
                                        CODObj["message"] = "COD charges are not applicable.";
                                    }
                                }
                                else
                                {
                                    CODObj["codAvailable"] = true;
                                    CODObj["codCharges"] = 0;
                                    CODObj["hasFree"] = true;
                                    CODObj["free_cod_on_mov"] = false;
                                    CODObj["message"] = "COD charges are free.";
                                }
                            }
                            else
                            {
                                ManageConfigValueLibrary GetCOD = new ManageConfigValueLibrary();
                                GetCOD = manageConfigValue.Where(p => p.KeyName.ToLower() == "hascod").FirstOrDefault();
                                if (!string.IsNullOrEmpty(GetCOD.Value))
                                {
                                    CODObj["codAvailable"] = true;
                                    CODObj["codCharges"] = Convert.ToDecimal(GetCOD.Value);
                                    CODObj["free_cod_on_mov"] = false;
                                    CODObj["hasFree"] = false;
                                    CODObj["message"] = "COD charges are applicable.";
                                }
                                else
                                {
                                    CODObj["codAvailable"] = true;
                                    CODObj["codCharges"] = 0;
                                    CODObj["hasFree"] = true;
                                    CODObj["free_cod_on_mov"] = false;
                                    CODObj["message"] = "COD charges are not applicable.";
                                }
                            }
                        }
                        else
                        {
                            CODObj["codAvailable"] = false;
                            CODObj["codCharges"] = 0;
                            CODObj["hasFree"] = true;
                            CODObj["free_cod_on_mov"] = false;
                            CODObj["message"] = "COD charges are not applicable.";
                        }

                    }
                    else
                    {
                        CODObj["codAvailable"] = false;
                        CODObj["codCharges"] = 0;
                        CODObj["hasFree"] = true;
                        CODObj["free_cod_on_mov"] = false;
                        CODObj["message"] = "COD charges are not applicable.";
                    }
                }
            }

            return CODObj;
        }

        public CategoryLibrary getCategories(int categoryId, string URL)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "?Id=" + categoryId, "GET", null);
            CategoryLibrary categoryLibrary = new CategoryLibrary();
            if (response != null)
            {
                BaseResponse<CategoryLibrary> baseResponse = new BaseResponse<CategoryLibrary>();
                baseResponse = baseResponse.JsonParseRecord(response);
                categoryLibrary = (CategoryLibrary)baseResponse.Data;
            }
            return categoryLibrary;
        }

        public List<ManageOffersMapping> checkSpecificCategory(int categoryId, string URL, List<ManageOffersMapping> _checkoffersMapping)
        {
            List<ManageOffersMapping> OffersMappinglst = new List<ManageOffersMapping>();
            List<ManageOffersMapping> _tempOffersMappinglst = new List<ManageOffersMapping>();

            CategoryLibrary category = getCategories(categoryId, URL);

            if (category != null)
            {
                string[] paths = category.ParentPathIds.Split('>');
                for (int i = 0; i < paths.Length; i++)
                {
                    if (!string.IsNullOrEmpty(paths[i].ToString()))
                    {

                        _tempOffersMappinglst = _checkoffersMapping.Where(p => p.categoryId == Convert.ToInt32(paths[i])).ToList();
                        if (_tempOffersMappinglst.Count > 0)
                        {
                            OffersMappinglst = _tempOffersMappinglst;
                            break;
                        }
                        else
                        {
                            _tempOffersMappinglst = _checkoffersMapping.Where(p => p.CategoryIds != null).ToList();
                            if (_tempOffersMappinglst.Count > 0)
                            {
                                _tempOffersMappinglst = _tempOffersMappinglst.Where(p => p.CategoryIds.Split(',').Any(id => id == paths[i].ToString())).ToList();
                                if (_tempOffersMappinglst.Count > 0)
                                {
                                    OffersMappinglst = _tempOffersMappinglst;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return OffersMappinglst;
        }

        public List<ProductColorDTO> getproductColors(int productId, string URL)
        {
            var response = helper.ApiCall(URL, EndPoints.ProductColorMapping + "?ProductID=" + productId, "GET", null);
            List<ProductColorDTO> lstProductColorDTO = new List<ProductColorDTO>();
            if (response != null)
            {
                BaseResponse<ProductColorDTO> baseResponse = new BaseResponse<ProductColorDTO>();
                baseResponse = baseResponse.JsonParseList(response);
                lstProductColorDTO = (List<ProductColorDTO>)baseResponse.Data;
            }
            return lstProductColorDTO;
        }

        public checkOfferDto checkOffer(ManageOffersLibrary _OffersLibrary, bool paymentoption, string couponCode, int productId, int brandId, int categoryId, string sellerId, string userId, string paymentMode, string URL, string OrderURL)
        {
            checkOfferDto disOffers = new checkOfferDto();
            BaseResponse<OrderDetails> orderResponse = new BaseResponse<OrderDetails>();
            List<OrderDetails> order = new List<OrderDetails>();
            var responseItem = helper.ApiCall(OrderURL, EndPoints.Orders + "?UserId=" + userId, "GET", null);
            orderResponse = orderResponse.JsonParseList(responseItem);

            int couponusage = 0;
            if (_OffersLibrary.usesPerCustomer.ToLower() != "nolimits")
            {
                List<OrderDetails> orderdata = new List<OrderDetails>();
                orderdata = (List<OrderDetails>)orderResponse.Data;
                if (Convert.ToBoolean(_OffersLibrary.onlyForNewCustomers) && orderdata.Count > 0)
                {
                    couponusage = 1;
                }
                else if (orderdata!=null && orderdata.Count > 0)
                {
                    var items = orderdata.Where(p => p.Coupon.ToLower() == couponCode.ToLower() && p.Status.ToLower() != "canceled" && p.Status.ToLower() != "failed").ToList();
                    couponusage = items.Count();
                }
            }

            List<ManageOffersMapping> offersMapping = new List<ManageOffersMapping>();
            //offersMapping = offers.offerItems.ToList();

            List<ManageOffersMapping> lstoffersMapping = new List<ManageOffersMapping>();
            List<ManageOffersMapping> checkoffersMapping = new List<ManageOffersMapping>();
            List<ManageOffersMapping> _checkoffersMapping = new List<ManageOffersMapping>();

            if (_OffersLibrary.offerType.ToLower() == "flat discount" || _OffersLibrary.offerType.ToLower() == "percentage discount" || _OffersLibrary.offerType.ToLower() == "free shipping")
            {
                if (_OffersLibrary.applyOn.ToLower() != "all products")
                {
                    if (_OffersLibrary.offerItems.ToList().Count > 0)
                    {
                        offersMapping = _OffersLibrary.offerItems.ToList();

                        if (_OffersLibrary.applyOn.ToLower() == "specific brands")
                        {
                            lstoffersMapping = offersMapping.Where(p => p.brandId == brandId).ToList();

                            if (lstoffersMapping.Count > 0)
                            {
                                var datamap = lstoffersMapping.FirstOrDefault();

                                _checkoffersMapping = lstoffersMapping.ToList();

                                if (_checkoffersMapping.Count > 0 && !string.IsNullOrEmpty(datamap.CategoryIds))
                                {
                                    var _tempdatalist = _checkoffersMapping.ToList();

                                    _tempdatalist = _checkoffersMapping.Where(p => p.CategoryIds.Split(',').Any(id => id == categoryId.ToString())).ToList();

                                    if (_tempdatalist.Count > 0)
                                    {
                                        _checkoffersMapping = _tempdatalist;
                                    }
                                    else
                                    {
                                        _checkoffersMapping = checkSpecificCategory(categoryId, URL, _checkoffersMapping);
                                    }
                                }


                            }

                            if (_checkoffersMapping.Count > 0)
                            {
                                var datamap = _checkoffersMapping.FirstOrDefault();

                                if (!string.IsNullOrEmpty(datamap.sellerId))
                                {
                                    _checkoffersMapping = _checkoffersMapping.Where(p => p.sellerId == sellerId).ToList();

                                    if (_checkoffersMapping.Count <= 0)
                                    {
                                        _checkoffersMapping = new List<ManageOffersMapping>();
                                    }
                                }

                                if (Convert.ToBoolean(datamap.sellerOptIn) && !string.IsNullOrEmpty(datamap.optInSellerIds))
                                {
                                    _checkoffersMapping = _checkoffersMapping.Where(p => p.optInSellerIds.Split(',').Any(id => id == sellerId.ToString())).ToList();

                                    if (_checkoffersMapping.Count <= 0)
                                    {
                                        _checkoffersMapping = new List<ManageOffersMapping>();
                                    }
                                }
                            }

                            if (_checkoffersMapping.Count > 0)
                            {
                                var check = _checkoffersMapping.FirstOrDefault();

                                if (_OffersLibrary.usesPerCustomer.ToLower() == "nolimits")
                                {
                                    disOffers.offers = _OffersLibrary;
                                    disOffers.CouponApplied = true;
                                    disOffers.ValidOnlyOnline = paymentoption;
                                    disOffers.message = "Coupon Applied Successfully";
                                }
                                else if (couponusage < Convert.ToInt32(_OffersLibrary.usesPerCustomer))
                                {
                                    disOffers.offers = _OffersLibrary;
                                    disOffers.CouponApplied = true;
                                    disOffers.ValidOnlyOnline = paymentoption;
                                    disOffers.message = "Coupon Applied Successfully";
                                }
                                else
                                {
                                    disOffers.offers = null;
                                    disOffers.CouponApplied = false;
                                    disOffers.ValidOnlyOnline = false;
                                    disOffers.message = "Coupon usage limit exceeded";
                                }

                            }

                            else
                            {
                                disOffers.offers = null;
                                disOffers.CouponApplied = false;
                                disOffers.ValidOnlyOnline = false;
                                disOffers.message = "Coupon is invalid";
                            }
                        }

                        else if (_OffersLibrary.applyOn.ToLower() == "specific sellers")
                        {
                            lstoffersMapping = offersMapping.Where(p => p.sellerId == sellerId).ToList();

                            if (lstoffersMapping.Count > 0)
                            {
                                var datamap = lstoffersMapping.FirstOrDefault();

                                _checkoffersMapping = lstoffersMapping.ToList();

                                if (_checkoffersMapping.Count > 0 && !string.IsNullOrEmpty(datamap.Brandids))
                                {
                                    _checkoffersMapping = _checkoffersMapping.Where(p => p.Brandids.Split(',').Any(id => id == brandId.ToString())).ToList();

                                }

                                if (_checkoffersMapping.Count > 0 && !string.IsNullOrEmpty(datamap.CategoryIds))
                                {

                                    var _tempdatalist = _checkoffersMapping.ToList();

                                    _tempdatalist = _checkoffersMapping.Where(p => p.CategoryIds.Split(',').Any(id => id == categoryId.ToString())).ToList();

                                    if (_tempdatalist.Count > 0)
                                    {
                                        _checkoffersMapping = _tempdatalist;
                                    }
                                    else
                                    {
                                        _checkoffersMapping = checkSpecificCategory(categoryId, URL, _checkoffersMapping);
                                    }
                                }
                            }

                            if (_checkoffersMapping.Count > 0)
                            {
                                var check = _checkoffersMapping.FirstOrDefault();

                                if (_OffersLibrary.usesPerCustomer.ToLower() == "nolimits")
                                {
                                    disOffers.offers = _OffersLibrary;
                                    disOffers.CouponApplied = true;
                                    disOffers.ValidOnlyOnline = paymentoption;
                                    disOffers.message = "Coupon Applied Successfully";
                                }
                                else if (couponusage < Convert.ToInt32(_OffersLibrary.usesPerCustomer))
                                {
                                    disOffers.offers = _OffersLibrary;
                                    disOffers.CouponApplied = true;
                                    disOffers.ValidOnlyOnline = paymentoption;
                                    disOffers.message = "Coupon Applied Successfully";
                                }
                                else
                                {
                                    disOffers.offers = null;
                                    disOffers.CouponApplied = false;
                                    disOffers.ValidOnlyOnline = false;
                                    disOffers.message = "Coupon usage limit exceeded";
                                }
                            }
                            else
                            {
                                disOffers.offers = null;
                                disOffers.CouponApplied = false;
                                disOffers.ValidOnlyOnline = false;
                                disOffers.message = "Coupon is invalid";
                            }
                        }

                        else if (_OffersLibrary.applyOn.ToLower() == "specific users")
                        {
                            lstoffersMapping = offersMapping.Where(p => p.userId == userId).ToList();

                            if (lstoffersMapping.Count > 0)
                            {
                                var datamap = lstoffersMapping.FirstOrDefault();

                                _checkoffersMapping = lstoffersMapping.ToList();

                                if (!string.IsNullOrEmpty(datamap.sellerId))
                                {
                                    _checkoffersMapping = _checkoffersMapping.Where(p => p.sellerId == sellerId).ToList();

                                    if (_checkoffersMapping.Count <= 0)
                                    {
                                        _checkoffersMapping = new List<ManageOffersMapping>();
                                    }
                                }

                                if (_checkoffersMapping.Count > 0 && !string.IsNullOrEmpty(datamap.Brandids))
                                {
                                    _checkoffersMapping = _checkoffersMapping.Where(p => p.Brandids.Split(',').Any(id => id == brandId.ToString())).ToList();

                                }

                                if (_checkoffersMapping.Count > 0 && !string.IsNullOrEmpty(datamap.CategoryIds))
                                {
                                    var _tempdatalist = _checkoffersMapping.ToList();

                                    _tempdatalist = _checkoffersMapping.Where(p => p.CategoryIds.Split(',').Any(id => id == categoryId.ToString())).ToList();

                                    if (_tempdatalist.Count > 0)
                                    {
                                        _checkoffersMapping = _tempdatalist;
                                    }
                                    else
                                    {
                                        _checkoffersMapping = checkSpecificCategory(categoryId, URL, _checkoffersMapping);
                                    }
                                }

                                if (_checkoffersMapping.Count > 0 && !string.IsNullOrEmpty(datamap.SellerIds))
                                {
                                    _checkoffersMapping = _checkoffersMapping.Where(p => p.SellerIds.Split(',').Any(id => id == sellerId.ToString())).ToList();

                                }

                                if (_checkoffersMapping.Count > 0 && !string.IsNullOrEmpty(datamap.ProductIds))
                                {
                                    _checkoffersMapping = _checkoffersMapping.Where(p => p.ProductIds.Split(',').Any(id => id == productId.ToString())).ToList();

                                }
                            }


                            if (_checkoffersMapping.Count > 0)
                            {
                                var check = _checkoffersMapping.FirstOrDefault();

                                if (_OffersLibrary.usesPerCustomer.ToLower() == "nolimits")
                                {
                                    disOffers.offers = _OffersLibrary;
                                    disOffers.CouponApplied = true;
                                    disOffers.ValidOnlyOnline = paymentoption;
                                    disOffers.message = "Coupon Applied Successfully";
                                }
                                else if (couponusage < Convert.ToInt32(_OffersLibrary.usesPerCustomer))
                                {
                                    disOffers.offers = _OffersLibrary;
                                    disOffers.CouponApplied = true;
                                    disOffers.ValidOnlyOnline = paymentoption;
                                    disOffers.message = "Coupon Applied Successfully";
                                }
                                else
                                {
                                    disOffers.offers = null;
                                    disOffers.CouponApplied = false;
                                    disOffers.ValidOnlyOnline = false;
                                    disOffers.message = "Coupon usage limit exceeded";
                                }
                            }
                            else
                            {
                                disOffers.offers = null;
                                disOffers.CouponApplied = false;
                                disOffers.ValidOnlyOnline = false;
                                disOffers.message = "Coupon is invalid";
                            }
                        }

                        else if (_OffersLibrary.applyOn.ToLower() == "specific categories")
                        {
                            lstoffersMapping = offersMapping.Where(p => p.categoryId == categoryId).ToList();
                            if (lstoffersMapping.Count > 0)
                            {
                                //var datamap = lstoffersMapping.FirstOrDefault();

                                _checkoffersMapping = lstoffersMapping.ToList();

                            }
                            else
                            {
                                _checkoffersMapping = checkSpecificCategory(categoryId, URL, offersMapping);
                            }

                            if (_checkoffersMapping.Count > 0)
                            {
                                var datamap = _checkoffersMapping.FirstOrDefault();

                                if (!string.IsNullOrEmpty(datamap.sellerId))
                                {
                                    _checkoffersMapping = _checkoffersMapping.Where(p => p.sellerId == sellerId).ToList();

                                    if (_checkoffersMapping.Count <= 0)
                                    {
                                        _checkoffersMapping = new List<ManageOffersMapping>();
                                    }
                                }

                                if (Convert.ToBoolean(datamap.sellerOptIn) && !string.IsNullOrEmpty(datamap.optInSellerIds))
                                {
                                    _checkoffersMapping = _checkoffersMapping.Where(p => p.optInSellerIds.Split(',').Any(id => id == sellerId.ToString())).ToList();

                                    if (_checkoffersMapping.Count <= 0)
                                    {
                                        _checkoffersMapping = new List<ManageOffersMapping>();
                                    }
                                }
                            }

                            if (_checkoffersMapping.Count > 0)
                            {
                                var check = _checkoffersMapping.FirstOrDefault();

                                if (_OffersLibrary.usesPerCustomer.ToLower() == "nolimits")
                                {
                                    disOffers.offers = _OffersLibrary;
                                    disOffers.CouponApplied = true;
                                    disOffers.ValidOnlyOnline = paymentoption;
                                    disOffers.message = "Coupon Applied Successfully";
                                }
                                else if (couponusage < Convert.ToInt32(_OffersLibrary.usesPerCustomer))
                                {
                                    disOffers.offers = _OffersLibrary;
                                    disOffers.CouponApplied = true;
                                    disOffers.ValidOnlyOnline = paymentoption;
                                    disOffers.message = "Coupon Applied Successfully";
                                }
                                else
                                {
                                    disOffers.offers = null;
                                    disOffers.CouponApplied = false;
                                    disOffers.ValidOnlyOnline = false;
                                    disOffers.message = "Coupon usage limit exceeded";
                                }
                            }
                            else
                            {
                                disOffers.offers = null;
                                disOffers.CouponApplied = false;
                                disOffers.ValidOnlyOnline = false;
                                disOffers.message = "Coupon is invalid";
                            }

                        }

                        else if (_OffersLibrary.applyOn.ToLower() == "specific products")
                        {
                            lstoffersMapping = offersMapping.Where(p => p.productId == productId).ToList();

                            if (lstoffersMapping.Count > 0)
                            {
                                var datamap = lstoffersMapping.FirstOrDefault();

                                _checkoffersMapping = lstoffersMapping.ToList();

                                if (Convert.ToBoolean(datamap.sellerOptIn) && !string.IsNullOrEmpty(datamap.optInSellerIds))
                                {
                                    _checkoffersMapping = _checkoffersMapping.Where(p => p.optInSellerIds.Split(',').Any(id => id == sellerId.ToString())).ToList();

                                    if (_checkoffersMapping.Count <= 0)
                                    {
                                        _checkoffersMapping = new List<ManageOffersMapping>();
                                    }
                                }
                            }

                            if (_checkoffersMapping.Count > 0)
                            {
                                var check = _checkoffersMapping.FirstOrDefault();

                                if (!string.IsNullOrEmpty(check.sellerId) && check.sellerId != "")
                                {

                                    if (check.sellerId == sellerId)
                                    {
                                        if (_OffersLibrary.usesPerCustomer.ToLower() == "nolimits")
                                        {
                                            disOffers.offers = _OffersLibrary;
                                            disOffers.CouponApplied = true;
                                            disOffers.ValidOnlyOnline = paymentoption;
                                            disOffers.message = "Coupon Applied Successfully";
                                        }
                                        else if (couponusage < Convert.ToInt32(_OffersLibrary.usesPerCustomer))
                                        {
                                            disOffers.offers = _OffersLibrary;
                                            disOffers.CouponApplied = true;
                                            disOffers.ValidOnlyOnline = paymentoption;
                                            disOffers.message = "Coupon Applied Successfully";
                                        }
                                        else
                                        {
                                            disOffers.offers = null;
                                            disOffers.CouponApplied = false;
                                            disOffers.ValidOnlyOnline = paymentoption;
                                            disOffers.message = "Coupon usage limit exceeded";
                                        }

                                    }
                                    else if (Convert.ToBoolean(check.sellerOptIn) && check.optInSellerIds.Contains(sellerId))
                                    {
                                        if (_OffersLibrary.usesPerCustomer.ToLower() == "nolimits")
                                        {
                                            disOffers.offers = _OffersLibrary;
                                            disOffers.CouponApplied = true;
                                            disOffers.ValidOnlyOnline = paymentoption;
                                            disOffers.message = "Coupon Applied Successfully";
                                        }
                                        else if (couponusage < Convert.ToInt32(_OffersLibrary.usesPerCustomer))
                                        {
                                            disOffers.offers = _OffersLibrary;
                                            disOffers.CouponApplied = true;
                                            disOffers.ValidOnlyOnline = paymentoption;
                                            disOffers.message = "Coupon Applied Successfully";
                                        }
                                        else
                                        {
                                            disOffers.offers = null;
                                            disOffers.CouponApplied = false;
                                            disOffers.ValidOnlyOnline = paymentoption;
                                            disOffers.message = "Coupon usage limit exceeded";
                                        }
                                    }
                                    else
                                    {
                                        disOffers.offers = null;
                                        disOffers.CouponApplied = false;
                                        disOffers.ValidOnlyOnline = false;
                                        disOffers.message = "Coupon is invalid";
                                    }

                                }
                                else
                                {

                                    if (_OffersLibrary.usesPerCustomer.ToLower() == "nolimits")
                                    {
                                        disOffers.offers = _OffersLibrary;
                                        disOffers.CouponApplied = true;
                                        disOffers.ValidOnlyOnline = paymentoption;
                                        disOffers.message = "Coupon Applied Successfully";
                                    }
                                    else if (couponusage < Convert.ToInt32(_OffersLibrary.usesPerCustomer))
                                    {
                                        disOffers.offers = _OffersLibrary;
                                        disOffers.CouponApplied = true;
                                        disOffers.message = "Coupon Applied Successfully";
                                        disOffers.ValidOnlyOnline = paymentoption;
                                    }
                                    else
                                    {
                                        disOffers.offers = null;
                                        disOffers.CouponApplied = false;
                                        disOffers.ValidOnlyOnline = paymentoption;
                                        disOffers.message = "Coupon usage limit exceeded";
                                    }
                                }
                            }
                            else
                            {
                                disOffers.offers = null;
                                disOffers.CouponApplied = false;
                                disOffers.ValidOnlyOnline = false;
                                disOffers.message = "Coupon is invalid";
                            }
                        }

                        else
                        {
                            disOffers.offers = null;
                            disOffers.CouponApplied = false;
                            disOffers.ValidOnlyOnline = false;
                            disOffers.message = "Coupon is invalid";
                        }
                    }
                    else
                    {
                        disOffers.offers = null;
                        disOffers.CouponApplied = false;
                        disOffers.ValidOnlyOnline = false;
                        disOffers.message = "Coupon is invalid";
                    }
                }
                else
                {
                    if (_OffersLibrary.usesPerCustomer.ToLower() == "nolimits")
                    {
                        disOffers.offers = _OffersLibrary;
                        disOffers.CouponApplied = true;
                        disOffers.ValidOnlyOnline = paymentoption;
                        disOffers.message = "Coupon Applied Successfully";
                    }
                    else if (couponusage < Convert.ToInt32(_OffersLibrary.usesPerCustomer))
                    {
                        disOffers.offers = _OffersLibrary;
                        disOffers.CouponApplied = true;
                        disOffers.ValidOnlyOnline = paymentoption;
                        disOffers.message = "Coupon Applied Successfully";
                    }
                    else
                    {
                        disOffers.offers = null;
                        disOffers.CouponApplied = false;
                        disOffers.ValidOnlyOnline = false;
                        disOffers.message = "Coupon usage limit exceeded";
                    }
                }
            }
            //else if (_OffersLibrary.offerType.ToLower() == "free shipping")
            //{
            //    lstoffersMapping = offersMapping.ToList();

            //    if (lstoffersMapping.Count > 0)
            //    {
            //        var datamap = lstoffersMapping.FirstOrDefault();

            //        _checkoffersMapping = lstoffersMapping.ToList();

            //        if (Convert.ToBoolean(datamap.sellerOptIn) && !string.IsNullOrEmpty(datamap.optInSellerIds))
            //        {
            //            _checkoffersMapping = _checkoffersMapping.Where(p => p.optInSellerIds.Split(',').Any(id => id == sellerId.ToString())).ToList();

            //            if (_checkoffersMapping.Count <= 0)
            //            {
            //                _checkoffersMapping = new List<ManageOffersMapping>();
            //            }
            //        }

            //        if (_checkoffersMapping.Count > 0 && !string.IsNullOrEmpty(datamap.Brandids))
            //        {
            //            _checkoffersMapping = _checkoffersMapping.Where(p => p.Brandids.Split(',').Any(id => id == brandId.ToString())).ToList();

            //        }

            //        if (_checkoffersMapping.Count > 0 && !string.IsNullOrEmpty(datamap.CategoryIds))
            //        {
            //            var _tempdatalist = _checkoffersMapping.ToList();

            //            _tempdatalist = _checkoffersMapping.Where(p => p.CategoryIds.Split(',').Any(id => id == categoryId.ToString())).ToList();

            //            if (_tempdatalist.Count > 0)
            //            {
            //                _checkoffersMapping = _tempdatalist;
            //            }
            //            else
            //            {
            //                _checkoffersMapping = checkSpecificCategory(categoryId, URL, _checkoffersMapping);
            //            }
            //        }

            //        if (_checkoffersMapping.Count > 0 && !string.IsNullOrEmpty(datamap.SellerIds))
            //        {
            //            _checkoffersMapping = _checkoffersMapping.Where(p => p.SellerIds.Split(',').Any(id => id == sellerId.ToString())).ToList();

            //        }

            //        if (_checkoffersMapping.Count > 0 && !string.IsNullOrEmpty(datamap.ProductIds))
            //        {
            //            _checkoffersMapping = _checkoffersMapping.Where(p => p.ProductIds.Split(',').Any(id => id == productId.ToString())).ToList();

            //        }
            //    }


            //    if (_checkoffersMapping.Count > 0)
            //    {
            //        var check = _checkoffersMapping.FirstOrDefault();

            //        if (_OffersLibrary.usesPerCustomer.ToLower() != "nolimits")
            //        {
            //            disOffers.offers = _OffersLibrary;
            //            disOffers.CouponApplied = true;
            //            disOffers.message = "Coupon Applied Successfully";
            //        }
            //        else if (couponusage <= Convert.ToInt32(_OffersLibrary.usesPerCustomer))
            //        {
            //            disOffers.offers = _OffersLibrary;
            //            disOffers.CouponApplied = true;
            //            disOffers.message = "Coupon Applied Successfully";
            //        }
            //        else
            //        {
            //            disOffers.offers = null;
            //            disOffers.CouponApplied = false;
            //            disOffers.message = "Coupon usage limit exceeded";
            //        }
            //    }
            //    else
            //    {
            //        disOffers.offers = null;
            //        disOffers.CouponApplied = false;
            //        disOffers.message = "Coupon is invalid";
            //    }
            //}
            else
            {
                disOffers.offers = null;
                disOffers.CouponApplied = false;
                disOffers.message = "Coupon is invalid";
            }

            return disOffers;
        }

        public JObject getshippingfree(string URL, string OrderAmount, decimal totalshipping)
        {
            decimal cod = 0;
            JObject ShipObj = new JObject();
            BaseResponse<ManageConfigValueLibrary> configResponse = new BaseResponse<ManageConfigValueLibrary>();
            List<ManageConfigValueLibrary> manageConfigValue = new List<ManageConfigValueLibrary>();
            var response = helper.ApiCall(URL, EndPoints.ManageConfigValue + "?PageIndex=0&PageSize=0", "GET", null);

            configResponse = configResponse.JsonParseList(response);
            if (configResponse.code == 200)
            {
                manageConfigValue = (List<ManageConfigValueLibrary>)configResponse.Data;

                if (manageConfigValue.Count > 0)
                {
                    ManageConfigValueLibrary GetMOVofCOD = new ManageConfigValueLibrary();
                    GetMOVofCOD = manageConfigValue.Where(p => p.KeyName == "free_shiping_on_mov").FirstOrDefault();
                    if (!string.IsNullOrEmpty(GetMOVofCOD.Value) && Convert.ToDecimal(GetMOVofCOD.Value) != 0)
                    {
                        decimal movValue = Convert.ToDecimal(GetMOVofCOD.Value);
                        decimal Ordervalue = Convert.ToDecimal(OrderAmount);
                        if (Ordervalue < movValue)
                        {
                            ShipObj["shipCharges"] = totalshipping;
                            ShipObj["free_shiping_on_mov"] = true;
                            ShipObj["movCharge"] = movValue;
                            ShipObj["hasFree"] = false;
                            ShipObj["message"] = "If your minimum order value is " + movValue + ", shipping charges will be free.";
                        }
                        else
                        {
                            ShipObj["shipCharges"] = 0;
                            ShipObj["hasFree"] = true;
                            ShipObj["free_shiping_on_mov"] = true;
                            ShipObj["message"] = "shipping charges are free.";
                        }
                    }
                    else
                    {
                        ShipObj["shipCharges"] = totalshipping;
                        ShipObj["free_shiping_on_mov"] = false;
                        ShipObj["hasFree"] = false;
                        ShipObj["message"] = "shipping charges are applicable.";
                    }

                }
                
            }

            return ShipObj;
        }

        public List<CategoryLibrary> GetCategoryWithParent(int Categoryid, string URL)
        {
            var response = helper.ApiCall(URL, EndPoints.Category + "/GetCategoryWithParent?Categoryid=" + Categoryid, "GET", null);
            List<CategoryLibrary> lstCategoryLibrary = new List<CategoryLibrary>();
            if (response != null)
            {
                BaseResponse<CategoryLibrary> baseResponse = new BaseResponse<CategoryLibrary>();
                baseResponse = baseResponse.JsonParseList(response);
                lstCategoryLibrary = (List<CategoryLibrary>)baseResponse.Data;
            }
            return lstCategoryLibrary;

        }
    }
}
