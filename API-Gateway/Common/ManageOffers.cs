using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.Order;
using API_Gateway.Models.Entity.User;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Linq;
using System.Web;

namespace API_Gateway.Common
{
    public class ManageOffers
    {
        private readonly IConfiguration _configuration;
        private string _token = string.Empty;
        public string _URL = string.Empty;
        public string UserURL = string.Empty;
        public string OrderURL = string.Empty;
        private readonly HttpContext _httpContext;
        BaseResponse<ManageOffersLibrary> baseResponse = new BaseResponse<ManageOffersLibrary>();
        private ApiHelper helper;
        public ManageOffers(IConfiguration configuration, string URL, HttpContext httpContext)
        {
            _configuration = configuration;
            _URL = URL;
            UserURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            OrderURL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            _httpContext = httpContext;
            helper = new ApiHelper(_httpContext);
        }

        public BaseResponse<ManageOffersLibrary> SaveOffer(ManageOffersDTO model, string UserId, bool IsAdmin)
        {
            BaseResponse<ManageOffersMapping> mappingResponse = new BaseResponse<ManageOffersMapping>();
            var temp = helper.ApiCall(_URL, EndPoints.ManageOffers + "?Code=" + model.code, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageOffersLibrary> tempList = (List<ManageOffersLibrary>)baseResponse.Data;

            var temp1 = helper.ApiCall(_URL, EndPoints.ManageOffers + "?Name=" + model.name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp1);
            List<ManageOffersLibrary> tempList1 = (List<ManageOffersLibrary>)baseResponse.Data;


            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else if(tempList1.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                DateTime date;
                DateTime time;
                ManageOffersLibrary manageOffers = new ManageOffersLibrary();
                manageOffers.name = model.name;
                manageOffers.code = model.code;
                manageOffers.terms = model.terms;
                manageOffers.offerCreatedBy = IsAdmin == true ? "Admin" : "Seller";
                manageOffers.offerType = model.offerType;
                manageOffers.usesType = model.usesType;
                manageOffers.usesPerCustomer = model.usesPerCustomer;
                manageOffers.value = model.value;
                manageOffers.minimumOrderValue = model.minimumOrderValue;
                manageOffers.maximumDiscountAmount = model.maximumDiscountAmount;
                manageOffers.buyQty = null;
                manageOffers.getQty = null;
                manageOffers.applyOn = model.applyOn;
                manageOffers.hasShippingFree = model.hasShippingFree;
                manageOffers.showToCustomer = model.showToCustomer;
                manageOffers.onlyForOnlinePayments = model.onlyForOnlinePayments;
                manageOffers.onlyForNewCustomers = model.onlyForNewCustomers;

                date = DateTime.Parse(model.startDate);
                time = DateTime.Parse(model.startTime);
                manageOffers.startDate = date.Add(time.TimeOfDay);

                date = DateTime.Parse(model.endDate);
                time = DateTime.Parse(model.endTime);
                manageOffers.endDate = date.Add(time.TimeOfDay);
                manageOffers.status = model.status;
                manageOffers.CreatedAt = DateTime.Now;
                manageOffers.CreatedBy = UserId;
                var response = helper.ApiCall(_URL, EndPoints.ManageOffers, "POST", manageOffers);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                int offerId = (int)baseResponse.Data;

                ManageOffersMapping mapping = new ManageOffersMapping();
                if (manageOffers.applyOn != "All Products")
                {
                    foreach (var item in model.offerItems)
                    {
                        mapping.offerId = offerId;
                        mapping.categoryId = item.categoryId;
                        mapping.sellerId = IsAdmin == true ? item.sellerId : UserId;
                        mapping.brandId = item.brandId;
                        mapping.productId = item.productId;
                        mapping.getProductId = null;
                        mapping.userId = item.userId;
                        mapping.getDiscountType = item.getDiscountType;
                        mapping.getDiscountValue = item.getDiscountValue;
                        mapping.getProductPrice = item.getProductPrice;
                        mapping.sellerOptIn = item.sellerOptIn;
                        mapping.optInSellerIds = item.optInSellerIds;
                        mapping.status = item.status;
                        mapping.CategoryIds = item.CategoryIds;
                        mapping.SellerIds = item.SellerIds;
                        mapping.Brandids = item.Brandids;
                        mapping.ProductIds = item.ProductIds;
                        mapping.CreatedAt = DateTime.Now;
                        mapping.CreatedBy = UserId;

                        SellerKycDetails sellerdetails = new SellerKycDetails();

                        if (!string.IsNullOrEmpty(item.sellerId) && item.sellerId != "")
                        {
                            sellerKyc seller = new sellerKyc(_configuration, _httpContext);
                            sellerdetails = seller.BindSeller(item.sellerId);
                        }

                        CustomerListModel customerDetails = new CustomerListModel();
                        if (!string.IsNullOrEmpty(item.userId) && item.userId != "")
                        {
                            UsersDetails usersData = new UsersDetails(_configuration, _httpContext);
                            customerDetails = usersData.GetCustomerById(item.userId).Data as CustomerListModel;
                        }

                        BrandLibrary brandLibrary = new BrandLibrary();

                        if (item.brandId != null && item.brandId != 0)
                        {

                            Brands brandDetails = new Brands(UserURL, _configuration, _httpContext);
                            brandLibrary = brandDetails.GetBrandById(Convert.ToInt32(item.brandId)).Data as BrandLibrary;
                        }

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

                        var SellerDetails = new { FullName = sellerdetails.FullName, Display = sellerdetails.DisplayName, LegalName = legalName, TradeName = tradeName, SellerId = sellerdetails.SellerId };
                        var BrandDetails = new { Name = brandLibrary.Name, BrandId = brandLibrary.ID };
                        var CustomerDetails = new { FullName = customerDetails.FirstName + " " + customerDetails.LastName, UserId = customerDetails.Id };

                        var Extradetails = new
                        {
                            SellerDetails = SellerDetails,
                            BrandDetails = BrandDetails,
                            CustomerDetails = CustomerDetails
                        };


                        var jsonString = JsonConvert.SerializeObject(Extradetails);

                        mapping.ExtraDetails = jsonString;

                        response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping, "POST", mapping);
                        mappingResponse = mappingResponse.JsonParseInputResponse(response);
                    }
                }
            }
            return baseResponse;
        }

        public BaseResponse<ManageOffersLibrary> UpdateOffer(ManageOffersDTO model, string UserId, bool IsAdmin)
        {
            var temp = helper.ApiCall(_URL, EndPoints.ManageOffers + "?Code=" + model.code, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageOffersLibrary> tempList = (List<ManageOffersLibrary>)baseResponse.Data;

            var temp1 = helper.ApiCall(_URL, EndPoints.ManageOffers + "?Name=" + model.name, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp1);
            List<ManageOffersLibrary> tempList1 = (List<ManageOffersLibrary>)baseResponse.Data;

            temp = helper.ApiCall(_URL, EndPoints.ManageOffers + "?id=" + model.id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(temp);
            ManageOffersLibrary tempRecord = (ManageOffersLibrary)baseResponse.Data;

            if (tempList.Where(x => x.id != model.id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else if (tempList1.Where(x => x.id != model.id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else if (tempRecord.endDate < DateTime.Now)
            {
                baseResponse = baseResponse.AlreadyExists();
                baseResponse.Message = "You can't update running/completed offers.";
            }
            else
            {
                DateTime date;
                DateTime time;
                tempRecord.name = model.name;
                tempRecord.code = model.code;
                tempRecord.terms = model.terms;
                tempRecord.usesType = model.usesType;
                tempRecord.usesPerCustomer = model.usesPerCustomer;
                tempRecord.value = model.value;
                tempRecord.minimumOrderValue = model.minimumOrderValue;
                tempRecord.maximumDiscountAmount = model.maximumDiscountAmount;
                tempRecord.buyQty = null;
                tempRecord.getQty = null;
                tempRecord.applyOn = model.applyOn;
                tempRecord.hasShippingFree = model.hasShippingFree;
                tempRecord.showToCustomer = model.showToCustomer;
                tempRecord.onlyForOnlinePayments = model.onlyForOnlinePayments;
                tempRecord.onlyForNewCustomers = model.onlyForNewCustomers;

                date = DateTime.Parse(model.startDate).Date;
                time = DateTime.Parse(model.startTime);
                tempRecord.startDate = date.Add(time.TimeOfDay);

                date = DateTime.Parse(model.endDate).Date;
                time = DateTime.Parse(model.endTime);
                tempRecord.endDate = date.Add(time.TimeOfDay);

                tempRecord.status = model.status;
                tempRecord.ModifiedAt = DateTime.Now;
                tempRecord.ModifiedBy = UserId;
                var response = helper.ApiCall(_URL, EndPoints.ManageOffers, "PUT", tempRecord);
                baseResponse = baseResponse.JsonParseInputResponse(response);

                temp = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + tempRecord.id, "GET", null);
                BaseResponse<ManageOffersMapping> baseResponse1 = new BaseResponse<ManageOffersMapping>();

                baseResponse1 = baseResponse1.JsonParseList(temp);
                List<ManageOffersMapping> templst = (List<ManageOffersMapping>)baseResponse1.Data;
                if (templst.Any())
                {
                    for (int i = 0; i < templst.Count; i++)
                    {
                        response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?Id=" + templst[i].id, "DELETE", null);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                }

                ManageOffersMapping mapping = new ManageOffersMapping();
                if (model.applyOn != "All Products")
                {
                    foreach (var item in model.offerItems)
                    {
                        mapping.offerId = tempRecord.id;
                        mapping.categoryId = item.categoryId;
                        mapping.sellerId = IsAdmin == true ? item.sellerId : UserId;
                        mapping.brandId = item.brandId;
                        mapping.productId = item.productId;
                        mapping.getProductId = item.getProductId;
                        mapping.userId = item.userId;
                        mapping.getDiscountType = item.getDiscountType;
                        mapping.getDiscountValue = item.getDiscountValue;
                        mapping.getProductPrice = item.getProductPrice;
                        mapping.sellerOptIn = item.sellerOptIn;
                        mapping.optInSellerIds = item.optInSellerIds;
                        mapping.status = item.status;
                        mapping.CategoryIds = item.CategoryIds;
                        mapping.SellerIds = item.SellerIds;
                        mapping.Brandids = item.Brandids;
                        mapping.ProductIds = item.ProductIds;
                        mapping.CreatedAt = DateTime.Now;
                        mapping.CreatedBy = UserId;

                        SellerKycDetails sellerdetails = new SellerKycDetails();

                        if (!string.IsNullOrEmpty(item.sellerId) && item.sellerId != "")
                        {
                            sellerKyc seller = new sellerKyc(_configuration, _httpContext);
                            sellerdetails = seller.BindSeller(item.sellerId);
                        }

                        CustomerListModel customerDetails = new CustomerListModel();
                        if (!string.IsNullOrEmpty(item.userId) && item.userId != "")
                        {
                            UsersDetails usersData = new UsersDetails(_configuration, _httpContext);
                            customerDetails = usersData.GetCustomerById(item.userId).Data as CustomerListModel;
                        }

                        BrandLibrary brandLibrary = new BrandLibrary();

                        if (item.brandId != null && item.brandId != 0)
                        {

                            Brands brandDetails = new Brands(UserURL, _configuration, _httpContext);
                            brandLibrary = brandDetails.GetBrandById(Convert.ToInt32(item.brandId)).Data as BrandLibrary;
                        }

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

                        var SellerDetails = new { FullName = sellerdetails.FullName, Display = sellerdetails.DisplayName, LegalName = legalName, TradeName = tradeName, SellerId = sellerdetails.SellerId };
                        var BrandDetails = new { Name = brandLibrary.Name, BrandId = brandLibrary.ID };
                        var CustomerDetails = new { FullName = customerDetails.FirstName + " " + customerDetails.LastName, UserId = customerDetails.Id };
                        var Extradetails = new
                        {
                            SellerDetails = SellerDetails,
                            BrandDetails = BrandDetails,
                            CustomerDetails = CustomerDetails
                        };


                        var jsonString = JsonConvert.SerializeObject(Extradetails);

                        mapping.ExtraDetails = jsonString;

                        response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping, "POST", mapping);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                }
            }
            return baseResponse;
        }

        public BaseResponse<ManageOffersLibrary> UpdateOfferStatus(int offerId, string status, string userId)
        {
            var temp = helper.ApiCall(_URL, EndPoints.ManageOffers + "?id=" + offerId, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(temp);
            if (baseResponse.code == 200)
            {
                ManageOffersLibrary tempRecord = (ManageOffersLibrary)baseResponse.Data;
                tempRecord.status = status;
                tempRecord.ModifiedAt = DateTime.Now;
                tempRecord.ModifiedBy = userId;
                var response = helper.ApiCall(_URL, EndPoints.ManageOffers, "PUT", tempRecord);
                baseResponse = baseResponse.JsonParseInputResponse(response);

            }
            return baseResponse;
        }

        public BaseResponse<ManageOffersMapping> optinOffer(int offerId, bool optIn, string SellerId, string userId)
        {
            BaseResponse<ManageOffersMapping> baseResponse = new BaseResponse<ManageOffersMapping>();
            var temp = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + offerId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            BaseResponse<ManageOffersMapping> OffersResponse = new BaseResponse<ManageOffersMapping>();
            if (baseResponse.code == 200)
            {
                List<ManageOffersMapping> tempRecordlist = (List<ManageOffersMapping>)baseResponse.Data;
                if (tempRecordlist.Count > 0)
                {

                    foreach (var item in tempRecordlist)
                    {
                        if (optIn)
                        {
                            item.sellerOptIn = optIn;
                            item.optInSellerIds = !string.IsNullOrEmpty(item.optInSellerIds) && item.optInSellerIds != "" ? item.optInSellerIds + "," + SellerId : SellerId;
                            item.ModifiedAt = DateTime.Now;
                            item.ModifiedBy = userId;
                            var response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping, "PUT", item);
                            OffersResponse = OffersResponse.JsonParseInputResponse(response);
                        }
                        else
                        {
                            string[] optInSellerIdsArray = item.optInSellerIds.Split(',');
                            optInSellerIdsArray = optInSellerIdsArray.Where(s => s.Trim() != SellerId).ToArray();

                            int count = optInSellerIdsArray.Length;
                            if (count > 0)
                            {
                                item.optInSellerIds = string.Join(",", optInSellerIdsArray);

                            }
                            else
                            {
                                item.sellerOptIn = optIn;
                                item.optInSellerIds = string.Join(",", optInSellerIdsArray);
                            }
                            item.ModifiedAt = DateTime.Now;
                            item.ModifiedBy = userId;
                            var response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping, "PUT", item);
                            OffersResponse = OffersResponse.JsonParseInputResponse(response);
                        }
                    }

                }
            }
            return OffersResponse;
        }

        public BaseResponse<ManageOffersLibrary> DeleteOffer(int? id = 0)
        {
            var temp = helper.ApiCall(_URL, EndPoints.ManageOffers + "?id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<ManageOffersLibrary> templist = (List<ManageOffersLibrary>)baseResponse.Data;
            if (templist.Any())
            {
                if (templist[0].startDate < DateTime.Now)
                {
                    baseResponse = baseResponse.AlreadyExists();
                    baseResponse.Message = "Running/Completed Offer Can't Deleted";
                }
                else
                {
                    temp = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + id, "GET", null);
                    BaseResponse<ManageOffersMapping> baseResponse1 = new BaseResponse<ManageOffersMapping>();

                    baseResponse1 = baseResponse1.JsonParseList(temp);
                    List<ManageOffersMapping> templst = (List<ManageOffersMapping>)baseResponse1.Data;
                    if (templst.Any())
                    {
                        for (int i = 0; i < templst.Count; i++)
                        {
                            temp = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?Id=" + templst[i].id, "DELETE", null);
                            baseResponse = baseResponse.JsonParseInputResponse(temp);
                        }
                    }

                    temp = helper.ApiCall(_URL, EndPoints.ManageOffers + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(temp);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return baseResponse;
        }

        public BaseResponse<ManageOffersLibrary> GetOffer(int? PageIndex, int? PageSize)
        {
            var response = helper.ApiCall(_URL, EndPoints.ManageOffers + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            if (baseResponse.code == 200)
            {
                List<ManageOffersLibrary> data = (List<ManageOffersLibrary>)baseResponse.Data;

                BaseResponse<ManageOffersMapping> mapping = new BaseResponse<ManageOffersMapping>();
                BaseResponse<ManageOffersMapping> mapping1 = new BaseResponse<ManageOffersMapping>();
                BaseResponse<Orders> orderlst = new BaseResponse<Orders>();
                for (int i = 0; i < data.Count; i++)
                {
                    response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + data[i].id, "GET", null);
                    mapping1 = mapping1.JsonParseList(response);
                    var dataresponse = (List<ManageOffersMapping>)mapping1.Data;

                    var response1 = helper.ApiCall(OrderURL, EndPoints.Orders + "?Coupon=" + data[i].code, "GET", null);
                    orderlst = orderlst.JsonParseList(response1);
                    decimal totalsales = 0;
                    decimal totalused = 0;
                    if (orderlst.code == 200)
                    {
                        var dataorderresponse = (List<Orders>)orderlst.Data;
                        totalsales = dataorderresponse.Sum(p => Convert.ToDecimal(p.CoupontDiscount));
                        totalused = dataorderresponse.Count();
                    }
                    data[i].totalsales = totalsales;
                    data[i].totalused = totalused;

                    data[i].offerItems = dataresponse.Select(details => new ManageOffersMapping
                    {
                        id = details.id,
                        offerId = details.offerId,
                        categoryId = (int)details.categoryId,
                        sellerId = details.sellerId,
                        brandId = (int)details.brandId,
                        productId = details.productId,
                        getProductId = details.getProductId,
                        userId = details.userId,
                        getDiscountType = details.getDiscountType,
                        getDiscountValue = (decimal)details.getDiscountValue,
                        getProductPrice = (decimal)details.getProductPrice,
                        sellerOptIn = (bool)details.sellerOptIn,
                        optInSellerIds = !string.IsNullOrEmpty(details.optInSellerIds) ? details.optInSellerIds : null,
                        status = details.status,
                        CategoryIds = details.CategoryIds,
                        SellerIds = details.SellerIds,
                        Brandids = details.Brandids,
                        ProductIds = details.ProductIds,
                        CreatedBy = details.CreatedBy,
                        CreatedAt = details.CreatedAt,
                        ModifiedBy = details.ModifiedBy,
                        ModifiedAt = details.ModifiedAt,
                        offerName = details.offerName,
                        productName = details.productName,
                        categoryName = details.categoryName,
                        categoryPathNames = details.categoryPathNames,
                        SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                        BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                        UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                    }).ToList();
                }
                baseResponse.Data = data;
            }
            return baseResponse;
        }

        public BaseResponse<ManageOffersLibrary> GetSellerOffer(string sellerId, string? searchtext = null, string? offerType = null, string? status = null, int? PageIndex = 0, int? PageSize = 0)
        {
            BaseResponse<ManageOffersMapping> OfferMap = new BaseResponse<ManageOffersMapping>();
            List<ManageOffersMapping> offermapList = new List<ManageOffersMapping>();
            string commaSeparatedIds = string.Empty;
            
            string url = string.Empty;
            
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url += "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }

            if (!string.IsNullOrEmpty(status))
            {
                url += "&status=" + status;
            }

            if (!string.IsNullOrEmpty(offerType))
            {
                url += "&OfferType=" + offerType;
            }

            var OfferMapresponse = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?PageIndex=0&PageSize=0&sellerId=" + sellerId, "GET", null);
            OfferMap = OfferMap.JsonParseList(OfferMapresponse);
            if (OfferMap.code == 200)
            {
                offermapList = (List<ManageOffersMapping>)OfferMap.Data;
                //offermapList = offermapList.Where(p => p.sellerOptIn == true && p.optInSellerIds.Contains(sellerId)).OrderByDescending(p => p.CreatedAt).ToList();
                if (offermapList.Count > 0)
                {
                    var offerIds = offermapList.Select(b => new { b.offerId }).Distinct().ToList();
                    if (offerIds.Count > 0)
                    {
                        List<int> offerIdValues = offerIds.Select(item => item.offerId).ToList();
                        commaSeparatedIds = string.Join(",", offerIdValues);
                        //url += "&Mode=" + "check";
                        url += "&offerIds=" + commaSeparatedIds;
                    }
                    else
                    {
                        url += "&offerCreatedBy=Seller&CreatedBy=" + sellerId;
                    }
                }

                var response = helper.ApiCall(_URL, EndPoints.ManageOffers + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
                baseResponse = baseResponse.JsonParseList(response);
                if (baseResponse.code == 200)
                {
                    List<ManageOffersLibrary> data = (List<ManageOffersLibrary>)baseResponse.Data;

                    BaseResponse<ManageOffersMapping> mapping = new BaseResponse<ManageOffersMapping>();
                    BaseResponse<ManageOffersMapping> mapping1 = new BaseResponse<ManageOffersMapping>();
                    BaseResponse<Orders> orderlst = new BaseResponse<Orders>();
                    for (int i = 0; i < data.Count; i++)
                    {
                        List<ManageOffersMapping> OfferMappingRespone = offermapList;
                        OfferMappingRespone = OfferMappingRespone.Where(p => p.offerId == data[i].id).ToList();
                        //response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + data[i].id + "&sellerId="+sellerId, "GET", null);
                        //mapping1 = mapping1.JsonParseList(response);
                        //var dataresponse = (List<ManageOffersMapping>)mapping1.Data;

                        var response1 = helper.ApiCall(OrderURL, EndPoints.Orders + "?Coupon=" + data[i].code, "GET", null);
                        orderlst = orderlst.JsonParseList(response1);
                        decimal totalsales = 0;
                        decimal totalused = 0;
                        if (orderlst.code == 200)
                        {
                            var dataorderresponse = (List<Orders>)orderlst.Data;
                            totalsales = dataorderresponse.Sum(p => Convert.ToDecimal(p.CoupontDiscount));
                            totalused = dataorderresponse.Count();
                        }
                        data[i].totalsales = totalsales;
                        data[i].totalused = totalused;

                        data[i].offerItems = OfferMappingRespone.Select(details => new ManageOffersMapping
                        {
                            id = details.id,
                            offerId = details.offerId,
                            categoryId = (int)details.categoryId,
                            sellerId = details.sellerId,
                            brandId = (int)details.brandId,
                            productId = details.productId,
                            getProductId = details.getProductId,
                            userId = details.userId,
                            getDiscountType = details.getDiscountType,
                            getDiscountValue = (decimal)details.getDiscountValue,
                            getProductPrice = (decimal)details.getProductPrice,
                            sellerOptIn = (bool)details.sellerOptIn,
                            optInSellerIds = details.optInSellerIds,
                            status = details.status,
                            CategoryIds = details.CategoryIds,
                            SellerIds = details.SellerIds,
                            Brandids = details.Brandids,
                            ProductIds = details.ProductIds,
                            CreatedBy = details.CreatedBy,
                            CreatedAt = details.CreatedAt,
                            ModifiedBy = details.ModifiedBy,
                            ModifiedAt = details.ModifiedAt,
                            offerName = details.offerName,
                            productName = details.productName,
                            categoryName = details.categoryName,
                            categoryPathNames = details.categoryPathNames,
                            SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                            BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                            UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                        }).OrderBy(p => p.CreatedAt).ToList();
                    }
                    baseResponse.Data = data;
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return baseResponse;
        }


        public BaseResponse<ManageOffersLibrary> GetOptInSellerOffer(string sellerId, string? searchtext = null, string? offerType = null, string? status = null, int PageIndex = 0, int PageSize = 0)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }

            if (!string.IsNullOrEmpty(status))
            {
                url += "&status=" + status;
            }

            if (!string.IsNullOrEmpty(offerType))
            {
                url += "&OfferType=" + offerType;
            }
            string commaSeparatedIds = string.Empty;
            string getcommaSeparatedIds = string.Empty;
            var response = helper.ApiCall(_URL, EndPoints.ManageOffers + "?PageIndex=0&PageSize=0&offerCreatedBy=Admin" + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            if (baseResponse.code == 200)
            {
                List<ManageOffersLibrary> data = (List<ManageOffersLibrary>)baseResponse.Data;
                BaseResponse<ManageOffersMapping> mapping1 = new BaseResponse<ManageOffersMapping>();
                List<ManageOffersMapping> offermapList = new List<ManageOffersMapping>();
                BaseResponse<Orders> orderlst = new BaseResponse<Orders>();
                string _urls = string.Empty;
                if (data.Count > 0)
                {
                    var _offerIds = data.Select(b => new { b.id }).Distinct().ToList();
                    if (_offerIds.Count > 0)
                    {
                        List<int> offerIdValues = _offerIds.Select(item => item.id).ToList();
                        commaSeparatedIds = string.Join(",", offerIdValues);
                        
                        var offresponse = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + commaSeparatedIds, "GET", null);
                        mapping1 = mapping1.JsonParseList(offresponse);
                        if (mapping1.code == 200)
                        {
                            offermapList = (List<ManageOffersMapping>)mapping1.Data;
                            var offerData = offermapList.Where(p => p.sellerOptIn == true && p.sellerId != sellerId).ToList();
                            if (offerData.Count > 0)
                            {
                                offermapList = offermapList.Where(p => (p.optInSellerIds != null && !p.optInSellerIds.Split(',').Contains(sellerId)) || (p.SellerIds != null && !p.SellerIds.Split(',').Contains(sellerId))).ToList();

                                if (offermapList.Count > 0)
                                {
                                    var _getofferIds = offermapList.Select(b => new { b.offerId }).Distinct().ToList();
                                    if (_getofferIds.Count > 0)
                                    {
                                        List<int> getofferIdValues = _getofferIds.Select(item => item.offerId).ToList();
                                        data = data.Where(p => !getofferIdValues.Contains(p.id)).ToList();
                                        for (int i = 0; i < data.Count; i++)
                                        {
                                            var response1 = helper.ApiCall(OrderURL, EndPoints.Orders + "?Coupon=" + data[i].code, "GET", null);
                                            orderlst = orderlst.JsonParseList(response1);
                                            decimal totalsales = 0;
                                            decimal totalused = 0;
                                            if (orderlst.code == 200)
                                            {
                                                var dataorderresponse = (List<Orders>)orderlst.Data;
                                                totalsales = dataorderresponse.Sum(p => Convert.ToDecimal(p.CoupontDiscount));
                                                totalused = dataorderresponse.Count();
                                            }
                                            data[i].totalsales = totalsales;
                                            data[i].totalused = totalused;

                                            data[i].offerItems = offermapList.Where(p=>p.offerId == data[i].id).Select(details => new ManageOffersMapping
                                            {
                                                id = details.id,
                                                offerId = details.offerId,
                                                categoryId = (int)details.categoryId,
                                                sellerId = details.sellerId,
                                                brandId = (int)details.brandId,
                                                productId = details.productId,
                                                getProductId = details.getProductId,
                                                userId = details.userId,
                                                getDiscountType = details.getDiscountType,
                                                getDiscountValue = (decimal)details.getDiscountValue,
                                                getProductPrice = (decimal)details.getProductPrice,
                                                sellerOptIn = (bool)details.sellerOptIn,
                                                optInSellerIds = details.optInSellerIds,
                                                status = details.status,
                                                CategoryIds = details.CategoryIds,
                                                SellerIds = details.SellerIds,
                                                Brandids = details.Brandids,
                                                ProductIds = details.ProductIds,
                                                CreatedBy = details.CreatedBy,
                                                CreatedAt = details.CreatedAt,
                                                ModifiedBy = details.ModifiedBy,
                                                ModifiedAt = details.ModifiedAt,
                                                offerName = details.offerName,
                                                productName = details.productName,
                                                categoryName = details.categoryName,
                                                categoryPathNames = details.categoryPathNames,
                                                SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["TradeName"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                                                BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                                                UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                                                VisibleOptInOption = true
                                            }).ToList();
                                        }

                                    }
                                    else
                                    {
                                        data = new List<ManageOffersLibrary>();
                                    }
                                }
                                else
                                {
                                    data = new List<ManageOffersLibrary>();
                                }
                            }
                            else
                            {
                                data = new List<ManageOffersLibrary>();
                            }
                        }
                        else
                        {
                            data = new List<ManageOffersLibrary>();
                        }
                    }
                }
                else
                {
                    data = new List<ManageOffersLibrary>();
                }



                ////List<ManageOffersMapping> offermapList = new List<ManageOffersMapping>();
                //BaseResponse<ManageOffersMapping> mapping = new BaseResponse<ManageOffersMapping>();
                ////BaseResponse<ManageOffersMapping> mapping1 = new BaseResponse<ManageOffersMapping>();

                //string offerIds = string.Empty;

                //for (int i = 0; i < data.Count; i++)
                //{
                //    //response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + data[i].id, "GET", null);
                //    //mapping1 = mapping1.JsonParseList(response);
                //    //offermapList = (List<ManageOffersMapping>)mapping1.Data;
                //    bool isvisible = false;
                //    //var offerData = offermapList.Where(p => p.sellerOptIn == true).ToList();
                //    if (offerData.Count > 0)
                //    {
                //        offermapList = offermapList.Where(p => p.optInSellerIds != null && p.optInSellerIds.Split(',').Contains(sellerId)).ToList();
                //        if (offermapList.Count > 0)
                //        {
                //            data[i].offerItems = offermapList.Select(details => new ManageOffersMapping
                //            {
                //                id = details.id,
                //                offerId = details.offerId,
                //                categoryId = (int)details.categoryId,
                //                sellerId = details.sellerId,
                //                brandId = (int)details.brandId,
                //                productId = details.productId,
                //                getProductId = details.getProductId,
                //                userId = details.userId,
                //                getDiscountType = details.getDiscountType,
                //                getDiscountValue = (decimal)details.getDiscountValue,
                //                getProductPrice = (decimal)details.getProductPrice,
                //                sellerOptIn = (bool)details.sellerOptIn,
                //                optInSellerIds = details.optInSellerIds,
                //                status = details.status,
                //                CategoryIds = details.CategoryIds,
                //                SellerIds = details.SellerIds,
                //                Brandids = details.Brandids,
                //                ProductIds = details.ProductIds,
                //                CreatedBy = details.CreatedBy,
                //                CreatedAt = details.CreatedAt,
                //                ModifiedBy = details.ModifiedBy,
                //                ModifiedAt = details.ModifiedAt,
                //                offerName = details.offerName,
                //                productName = details.productName,
                //                categoryName = details.categoryName,
                //                categoryPathNames = details.categoryPathNames,
                //                SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["TradeName"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                //                BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                //                UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                //                VisibleOptInOption = isvisible
                //            }).ToList();
                //        }
                //        else
                //        {
                //            offerIds += data[i].id + ",";
                //        }
                //    }
                //    else
                //    {
                //        offerIds += data[i].id + ",";
                //    }

                //}

                //if (data.Count > 0)
                //{
                //    offerIds = offerIds.Trim(',');
                //    List<string> offerIdList = offerIds.Split(',').ToList();
                //    if (offerIdList.Count > 0)
                //    {
                //        data = data.Where(p => offerIds.Split(',').Contains(Convert.ToString(p.id).ToString())).ToList();
                //    }
                //}

                if(data.Count > 0)
                {
                    int totalCount = data.Count;
                    int TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
                    var items = data.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                    baseResponse.Message = "Data bind suceessfully.";
                    baseResponse.Data = items;
                    baseResponse.code = 200;
                    baseResponse.pagination.PageCount = TotalPages;
                    baseResponse.pagination.RecordCount = totalCount;
                }
                else
                {
                    baseResponse = baseResponse.NotExist();
                }

                
                //baseResponse.Data = data;
            }
            return baseResponse;
        }

        public BaseResponse<ManageOffersLibrary> GetAdminOffer(string sellerId, string? searchtext = null, string? offerType = null, string? status = null, int PageIndex = 0, int PageSize = 0)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }

            if (!string.IsNullOrEmpty(status))
            {
                url += "&status=" + status;
            }

            if (!string.IsNullOrEmpty(offerType))
            {
                url += "&OfferType=" + offerType;
            }

            string commaSeparatedIds = string.Empty;
            string getcommaSeparatedIds = string.Empty;
            var response = helper.ApiCall(_URL, EndPoints.ManageOffers + "?PageIndex=0&PageSize=0&offerCreatedBy=Admin" + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            if (baseResponse.code == 200)
            {
                List<ManageOffersLibrary> data = (List<ManageOffersLibrary>)baseResponse.Data;
                BaseResponse<ManageOffersMapping> mapping1 = new BaseResponse<ManageOffersMapping>();
                List<ManageOffersMapping> offermapList = new List<ManageOffersMapping>();
                BaseResponse<Orders> orderlst = new BaseResponse<Orders>();
                string _urls = string.Empty;
                if (data.Count > 0)
                {
                    var _offerIds = data.Select(b => new { b.id }).Distinct().ToList();
                    if (_offerIds.Count > 0)
                    {
                        List<int> offerIdValues = _offerIds.Select(item => item.id).ToList();
                        commaSeparatedIds = string.Join(",", offerIdValues);

                        var offresponse = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + commaSeparatedIds, "GET", null);
                        mapping1 = mapping1.JsonParseList(offresponse);
                        if (mapping1.code == 200)
                        {
                            offermapList = (List<ManageOffersMapping>)mapping1.Data;
                            //var offerData = offermapList.Where(p => p.sellerOptIn == true || p.sellerId == sellerId).ToList();
                            var offerData = offermapList.Where(p => p.sellerOptIn == true).ToList();
                            if (offerData.Count > 0)
                            {
                                //var visibleOptin = offerData.Where(p=>p.sellerOptIn == true).ToList();
                                var _getoptinofferIds = offerData.Select(b => new { b.offerId }).Distinct().ToList();
                                offermapList = offermapList.Where(p => (p.sellerId == sellerId) || (p.optInSellerIds != null && p.optInSellerIds.Split(',').Contains(sellerId)) || (p.SellerIds != null && p.SellerIds.Split(',').Contains(sellerId))).ToList();

                                if (offermapList.Count > 0)
                                {
                                    var _getofferIds = offermapList.Select(b => new { b.offerId }).Distinct().ToList();
                                    if (_getofferIds.Count > 0)
                                    {
                                        List<int> getofferIdValues = _getofferIds.Select(item => item.offerId).ToList();
                                        //data = data.Where(p => getofferIdValues.Contains(p.id)).ToList();
                                        bool isVisible = false;
                                        for (int i = 0; i < data.Count; i++)
                                        {
                                            var response1 = helper.ApiCall(OrderURL, EndPoints.Orders + "?Coupon=" + data[i].code, "GET", null);
                                            orderlst = orderlst.JsonParseList(response1);
                                            decimal totalsales = 0;
                                            decimal totalused = 0;
                                            if (orderlst.code == 200)
                                            {
                                                var dataorderresponse = (List<Orders>)orderlst.Data;
                                                totalsales = dataorderresponse.Sum(p => Convert.ToDecimal(p.CoupontDiscount));
                                                totalused = dataorderresponse.Count();
                                            }
                                            data[i].totalsales = totalsales;
                                            data[i].totalused = totalused;

                                            var _getoffId = _getofferIds.Where(p => p.offerId == data[i].id).ToList();
                                            //var _getoffId = _getoptinofferIds.Where(p => p.offerId == data[i].id).ToList();
                                            
                                            if (_getoffId.Count > 0)
                                            {
                                                isVisible = true;
                                            }
                                            else
                                            {
                                                isVisible = false;
                                            }
                                            data[i].offerItems = offermapList.Where(p => p.offerId == data[i].id).Select(details => new ManageOffersMapping
                                            {
                                                id = details.id,
                                                offerId = details.offerId,
                                                categoryId = (int)details.categoryId,
                                                sellerId = details.sellerId,
                                                brandId = (int)details.brandId,
                                                productId = details.productId,
                                                getProductId = details.getProductId,
                                                userId = details.userId,
                                                getDiscountType = details.getDiscountType,
                                                getDiscountValue = (decimal)details.getDiscountValue,
                                                getProductPrice = (decimal)details.getProductPrice,
                                                sellerOptIn = (bool)details.sellerOptIn,
                                                optInSellerIds = details.optInSellerIds,
                                                status = details.status,
                                                CategoryIds = details.CategoryIds,
                                                SellerIds = details.SellerIds,
                                                Brandids = details.Brandids,
                                                ProductIds = details.ProductIds,
                                                CreatedBy = details.CreatedBy,
                                                CreatedAt = details.CreatedAt,
                                                ModifiedBy = details.ModifiedBy,
                                                ModifiedAt = details.ModifiedAt,
                                                offerName = details.offerName,
                                                productName = details.productName,
                                                categoryName = details.categoryName,
                                                categoryPathNames = details.categoryPathNames,
                                                SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["TradeName"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                                                BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                                                UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                                                VisibleOptInOption = true
                                            }).ToList();
                                        }

                                    }
                                    else
                                    {
                                        bool isVisible = false;
                                        for (int i = 0; i < data.Count; i++)
                                        {
                                            var _getoffId = _getoptinofferIds.Where(p => p.offerId == data[i].id).ToList();

                                            if (_getoffId.Count > 0)
                                            {
                                                isVisible = true;
                                            }
                                            else
                                            {
                                                isVisible = false;
                                            }
                                            var response1 = helper.ApiCall(OrderURL, EndPoints.Orders + "?Coupon=" + data[i].code, "GET", null);
                                            orderlst = orderlst.JsonParseList(response1);
                                            decimal totalsales = 0;
                                            decimal totalused = 0;
                                            if (orderlst.code == 200)
                                            {
                                                var dataorderresponse = (List<Orders>)orderlst.Data;
                                                totalsales = dataorderresponse.Sum(p => Convert.ToDecimal(p.CoupontDiscount));
                                                totalused = dataorderresponse.Count();
                                            }
                                            data[i].totalsales = totalsales;
                                            data[i].totalused = totalused;

                                            data[i].offerItems = offermapList.Where(p => p.offerId == data[i].id).Select(details => new ManageOffersMapping
                                            {
                                                id = details.id,
                                                offerId = details.offerId,
                                                categoryId = (int)details.categoryId,
                                                sellerId = details.sellerId,
                                                brandId = (int)details.brandId,
                                                productId = details.productId,
                                                getProductId = details.getProductId,
                                                userId = details.userId,
                                                getDiscountType = details.getDiscountType,
                                                getDiscountValue = (decimal)details.getDiscountValue,
                                                getProductPrice = (decimal)details.getProductPrice,
                                                sellerOptIn = (bool)details.sellerOptIn,
                                                optInSellerIds = details.optInSellerIds,
                                                status = details.status,
                                                CategoryIds = details.CategoryIds,
                                                SellerIds = details.SellerIds,
                                                Brandids = details.Brandids,
                                                ProductIds = details.ProductIds,
                                                CreatedBy = details.CreatedBy,
                                                CreatedAt = details.CreatedAt,
                                                ModifiedBy = details.ModifiedBy,
                                                ModifiedAt = details.ModifiedAt,
                                                offerName = details.offerName,
                                                productName = details.productName,
                                                categoryName = details.categoryName,
                                                categoryPathNames = details.categoryPathNames,
                                                SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["TradeName"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                                                BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                                                UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                                                VisibleOptInOption = true
                                            }).ToList();
                                        }
                                        //data = new List<ManageOffersLibrary>();
                                    }
                                }
                                else
                                {
                                    List<int> getofferIdValues = _getoptinofferIds.Select(item => item.offerId).ToList();
                                    data = data.Where(p => !getofferIdValues.Contains(p.id)).ToList();
                                    //data = new List<ManageOffersLibrary>();
                                }
                            }
                            else
                            {
                                var _getoptinofferIds = offermapList.Select(b => new { b.offerId }).Distinct().ToList();
                                bool isVisible = false;
                                for (int i = 0; i < data.Count; i++)
                                {
                                    var _getoffId = _getoptinofferIds.Where(p => p.offerId == data[i].id).ToList();

                                    if (_getoffId.Count > 0)
                                    {
                                        isVisible = true;
                                    }
                                    else
                                    {
                                        isVisible = false;
                                    }
                                    var response1 = helper.ApiCall(OrderURL, EndPoints.Orders + "?Coupon=" + data[i].code, "GET", null);
                                    orderlst = orderlst.JsonParseList(response1);
                                    decimal totalsales = 0;
                                    decimal totalused = 0;
                                    if (orderlst.code == 200)
                                    {
                                        var dataorderresponse = (List<Orders>)orderlst.Data;
                                        totalsales = dataorderresponse.Sum(p => Convert.ToDecimal(p.CoupontDiscount));
                                        totalused = dataorderresponse.Count();
                                    }
                                    data[i].totalsales = totalsales;
                                    data[i].totalused = totalused;

                                    data[i].offerItems = offermapList.Where(p => p.offerId == data[i].id).Select(details => new ManageOffersMapping
                                    {
                                        id = details.id,
                                        offerId = details.offerId,
                                        categoryId = (int)details.categoryId,
                                        sellerId = details.sellerId,
                                        brandId = (int)details.brandId,
                                        productId = details.productId,
                                        getProductId = details.getProductId,
                                        userId = details.userId,
                                        getDiscountType = details.getDiscountType,
                                        getDiscountValue = (decimal)details.getDiscountValue,
                                        getProductPrice = (decimal)details.getProductPrice,
                                        sellerOptIn = (bool)details.sellerOptIn,
                                        optInSellerIds = details.optInSellerIds,
                                        status = details.status,
                                        CategoryIds = details.CategoryIds,
                                        SellerIds = details.SellerIds,
                                        Brandids = details.Brandids,
                                        ProductIds = details.ProductIds,
                                        CreatedBy = details.CreatedBy,
                                        CreatedAt = details.CreatedAt,
                                        ModifiedBy = details.ModifiedBy,
                                        ModifiedAt = details.ModifiedAt,
                                        offerName = details.offerName,
                                        productName = details.productName,
                                        categoryName = details.categoryName,
                                        categoryPathNames = details.categoryPathNames,
                                        SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["TradeName"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                                        BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                                        UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                                        VisibleOptInOption = true
                                    }).ToList();
                                }

                                //data = new List<ManageOffersLibrary>();
                            }
                        }
                    }
                }
                else
                {
                    data = new List<ManageOffersLibrary>();
                }


                //var response = helper.ApiCall(_URL, EndPoints.ManageOffers + "?PageIndex=0&PageSize=0" + url, "GET", null);
                //baseResponse = baseResponse.JsonParseList(response);
                //if (baseResponse.code == 200)
                //{
                //    List<ManageOffersLibrary> data = (List<ManageOffersLibrary>)baseResponse.Data;
                //    data = data.Where(p => p.CreatedBy != sellerId).ToList();
                //    List<ManageOffersMapping> offermapList = new List<ManageOffersMapping>();
                //    BaseResponse<ManageOffersMapping> mapping = new BaseResponse<ManageOffersMapping>();
                //    BaseResponse<ManageOffersMapping> mapping1 = new BaseResponse<ManageOffersMapping>();
                //    for (int i = 0; i < data.Count; i++)
                //    {
                //        response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + data[i].id, "GET", null);
                //        mapping1 = mapping1.JsonParseList(response);
                //        offermapList = (List<ManageOffersMapping>)mapping1.Data;
                //        bool isvisible = false;
                //        var offerData = offermapList.Where(p => p.sellerOptIn == true).ToList();
                //        if(offerData.Count > 0)
                //        {
                //            isvisible = true;
                //            offermapList = offermapList.Where(p => p.optInSellerIds != null && !p.optInSellerIds.Split(',').Contains(sellerId)).ToList();
                //            if (offermapList.Count > 0)
                //            {
                //                isvisible = true;
                //            }
                //        }
                //        else
                //        {

                //        }
                //        data[i].offerItems = offermapList.Select(details => new ManageOffersMapping
                //        {
                //            id = details.id,
                //            offerId = details.offerId,
                //            categoryId = (int)details.categoryId,
                //            sellerId = details.sellerId,
                //            brandId = (int)details.brandId,
                //            productId = details.productId,
                //            getProductId = details.getProductId,
                //            userId = details.userId,
                //            getDiscountType = details.getDiscountType,
                //            getDiscountValue = (decimal)details.getDiscountValue,
                //            getProductPrice = (decimal)details.getProductPrice,
                //            sellerOptIn = details.sellerOptIn,
                //            optInSellerIds = !string.IsNullOrEmpty(details.optInSellerIds) ? details.optInSellerIds : null,
                //            status = details.status,
                //            CategoryIds = details.CategoryIds,
                //            SellerIds = details.SellerIds,
                //            Brandids = details.Brandids,
                //            ProductIds = details.ProductIds,
                //            CreatedBy = details.CreatedBy,
                //            CreatedAt = details.CreatedAt,
                //            ModifiedBy = details.ModifiedBy,
                //            ModifiedAt = details.ModifiedAt,
                //            offerName = details.offerName,
                //            productName = details.productName,
                //            categoryName = details.categoryName,
                //            categoryPathNames = details.categoryPathNames,
                //            SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["TradeName"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                //            BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                //            UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                //            VisibleOptInOption = isvisible
                //        }).ToList();
                //    }
                if (data.Count > 0)
                {
                    int totalCount = data.Count;
                    int TotalPages = PageSize == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)PageSize);
                    var items = PageIndex == 0 ? data.ToList() : data.Skip((PageIndex - 1) * PageSize).Take(PageSize).ToList();
                    baseResponse.Message = "Data bind suceessfully.";
                    baseResponse.Data = items;
                    baseResponse.code = 200;
                    baseResponse.pagination.PageCount = TotalPages;
                    baseResponse.pagination.RecordCount = totalCount;
                }
                else
                {
                    baseResponse = baseResponse.NotExist();
                }
                //baseResponse.Data = data;
            }
            return baseResponse;
        }

        public BaseResponse<ManageOffersDTO> GetOfferList(string? userId = null, bool? showToCustomer = null, int? categoryId = 0, int? productId = 0, int? brandId = 0, string? sellerId = null, string? applyOn = null, string? Mode = "get")
        {
            string url = string.Empty;
            if (showToCustomer != null)
            {
                url += "&showToCustomer=" + showToCustomer;
            }
            BaseResponse<ManageOffersDTO> offerResponse = new BaseResponse<ManageOffersDTO>();
            var response = helper.ApiCall(_URL, EndPoints.ManageOffers + "?PageIndex=0&PageSize=0" + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ManageOffersLibrary> data = (List<ManageOffersLibrary>)baseResponse.Data;

            if (data.Count > 0)
            {
                DateTime currentDate = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                data = data.Where(p => p.status.ToLower() == "active" && (p.offerType != "bogo" || p.offerType != "boga") && p.startDate <= currentDate && p.endDate >= currentDate).ToList();

                string url1 = string.Empty;

                if (categoryId != 0)
                {
                    url1 += "&categoryId=" + categoryId;
                }
                if (productId != 0)
                {
                    url1 += "&productId=" + productId;
                }
                if (brandId != 0)
                {
                    url1 += "&brandId=" + brandId;
                }
                if (!string.IsNullOrEmpty(sellerId))
                {
                    url1 += "&sellerId=" + sellerId;
                }

                if (!string.IsNullOrEmpty(applyOn))
                {
                    url1 += "&ApplyOn=" + HttpUtility.UrlEncode(applyOn);
                }
                if (!string.IsNullOrEmpty(Mode))
                {
                    url1 += "&Mode=" + Mode;
                }


                //if (categoryId != 0 && productId != 0 && !string.IsNullOrEmpty(sellerId))
                //{
                string sss = string.Join(", ", data.Select(p => p.id));
                var response1 = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + sss + url1, "GET", null);
                BaseResponse<ManageOffersMapping> baseResponse1 = new BaseResponse<ManageOffersMapping>();

                baseResponse1 = baseResponse1.JsonParseList(response1);
                List<ManageOffersMapping> data1 = (List<ManageOffersMapping>)baseResponse1.Data;
                var offerIdList = data1.Select(p => p.offerId).ToList();
                //data = data.Where(num => offerIdList.Contains(num.id)).ToList();
                //}

                foreach (var item in data)
                {
                    var matchingItems = data1.Where(p => p.offerId == item.id).ToList();
                    item.offerItems = matchingItems;
                }

                List<ManageOffersLibrary> cartofferList = new List<ManageOffersLibrary>();
                if (!string.IsNullOrEmpty(userId))
                {

                    var offerList = data.Where(p => p.applyOn.ToLower() != "specific users").ToList();
                    cartofferList = offerList;

                    var userList = data.Where(p => p.applyOn.ToLower() == "specific users").ToList();
                    foreach (var item in userList)
                    {
                        var useroffers = item.offerItems.Where(p => p.userId == userId).ToList();
                        if (useroffers.Count > 0)
                        {
                            cartofferList.Add(item);
                        }
                    }
                }
                else
                {
                    var offerList = data.Where(p => p.applyOn.ToLower() != "specific users").ToList();
                    cartofferList = offerList;
                    //cartofferList = data.ToList();
                }

                offerResponse.Data = cartofferList;
            }

            return offerResponse;
        }

        public BaseResponse<ManageOffersLibrary> GetOfferById(int id, string? SellerId=null)
        {
            var response = helper.ApiCall(_URL, EndPoints.ManageOffers + "?id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            ManageOffersLibrary data = (ManageOffersLibrary)baseResponse.Data;

            BaseResponse<ManageOffersMapping> mapping = new BaseResponse<ManageOffersMapping>();
            BaseResponse<ManageOffersMapping> mapping1 = new BaseResponse<ManageOffersMapping>();
            if (data.id != 0 && data.id != null)
            {
                response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + data.id, "GET", null);
                mapping1 = mapping1.JsonParseList(response);
                var dataresponse = (List<ManageOffersMapping>)mapping1.Data;
                data.offerItems = dataresponse.Select(details => new ManageOffersMapping
                {
                    id = details.id,
                    offerId = details.offerId,
                    categoryId = (int)details.categoryId,
                    sellerId = details.sellerId,
                    brandId = (int)details.brandId,
                    productId = details.productId,
                    getProductId = details.getProductId,
                    userId = details.userId,
                    getDiscountType = details.getDiscountType,
                    getDiscountValue = (decimal)details.getDiscountValue,
                    getProductPrice = (decimal)details.getProductPrice,
                    sellerOptIn = (bool)details.sellerOptIn,
                    optInSellerIds = details.optInSellerIds,
                    status = details.status,
                    CategoryIds = details.CategoryIds,
                    SellerIds = details.SellerIds,
                    Brandids = details.Brandids,
                    ProductIds = details.ProductIds,
                    CreatedBy = details.CreatedBy,
                    CreatedAt = details.CreatedAt,
                    ModifiedBy = details.ModifiedBy,
                    ModifiedAt = details.ModifiedAt,
                    offerName = details.offerName,
                    productName = details.productName,
                    categoryName = details.categoryName,
                    categoryPathNames = details.categoryPathNames,
                    SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                    BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                    UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                }).ToList();
            }

            bool isvalid = false;
            
            if (!string.IsNullOrEmpty(SellerId))
            {
                if(data.offerCreatedBy.ToLower() == "seller")
                {
                    isvalid = data.offerItems.Where(x => x.sellerId == SellerId).ToList().Count > 0 ? true : false;
                    
                    if (!isvalid)
                    {
                        if (data.offerType.ToLower() != "all products")
                        {
                            var datamap = data.offerItems.FirstOrDefault();

                            if (Convert.ToBoolean(datamap.sellerOptIn) && !string.IsNullOrEmpty(datamap.optInSellerIds))
                            {
                                isvalid = data.offerItems.Where(p => p.optInSellerIds.Split(',').Any(id => id == SellerId.ToString())).ToList().Count > 0 ? true : false;
                            }
                            else
                            {
                                isvalid = data.offerItems.Where(p => p.SellerIds.Split(',').Any(id => id == SellerId.ToString())).ToList().Count > 0 ? true : false;
                            }
                        }
                        else
                        {
                            isvalid = true;
                        }
                    }
                }
                else
                {
                    var datamap = data.offerItems.FirstOrDefault();

                    if (data.offerType.ToLower()!= "all products")
                    {
                        if (Convert.ToBoolean(datamap.sellerOptIn) && !string.IsNullOrEmpty(datamap.optInSellerIds))
                        {
                            isvalid = data.offerItems.Where(p => p.optInSellerIds.Split(',').Any(id => id == SellerId.ToString())).ToList().Count > 0 ? true : false;
                        }
                        else
                        {
                            isvalid = data.offerItems.Where(p => p.SellerIds.Split(',').Any(id => id == SellerId.ToString())).ToList().Count > 0 ? true : false;
                        }
                    }
                    else
                    {
                        isvalid = true;
                    }
                }
            }
            else
            {
                isvalid = true;
            }

            if (isvalid)
            {
                baseResponse.Data = data;
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return baseResponse;
        }

        public BaseResponse<ManageOffersLibrary> GetOfferByName(string name)
        {
            var response = helper.ApiCall(_URL, EndPoints.ManageOffers + "?name=" + name, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            ManageOffersLibrary data = (ManageOffersLibrary)baseResponse.Data;

            BaseResponse<ManageOffersMapping> mapping = new BaseResponse<ManageOffersMapping>();
            BaseResponse<ManageOffersMapping> mapping1 = new BaseResponse<ManageOffersMapping>();
            if (data.id != 0 && data.id != null)
            {
                response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + data.id, "GET", null);
                mapping1 = mapping1.JsonParseList(response);
                var dataresponse = (List<ManageOffersMapping>)mapping1.Data;
                data.offerItems = dataresponse.Select(details => new ManageOffersMapping
                {
                    id = details.id,
                    offerId = details.offerId,
                    categoryId = (int)details.categoryId,
                    sellerId = details.sellerId,
                    brandId = (int)details.brandId,
                    productId = details.productId,
                    getProductId = details.getProductId,
                    userId = details.userId,
                    getDiscountType = details.getDiscountType,
                    getDiscountValue = (decimal)details.getDiscountValue,
                    getProductPrice = (decimal)details.getProductPrice,
                    sellerOptIn = (bool)details.sellerOptIn,
                    optInSellerIds = details.optInSellerIds,
                    status = details.status,
                    CategoryIds = details.CategoryIds,
                    SellerIds = details.SellerIds,
                    Brandids = details.Brandids,
                    ProductIds = details.ProductIds,
                    CreatedBy = details.CreatedBy,
                    CreatedAt = details.CreatedAt,
                    ModifiedBy = details.ModifiedBy,
                    ModifiedAt = details.ModifiedAt,
                    offerName = details.offerName,
                    productName = details.productName,
                    categoryName = details.categoryName,
                    categoryPathNames = details.categoryPathNames,
                    SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                    BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                    UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                }).OrderBy(p => p.CreatedAt).ToList();
            }
            baseResponse.Data = data;
            return baseResponse;
        }

        public BaseResponse<ManageOffersLibrary> GetOfferByOfferType(string offerType)
        {
            var response = helper.ApiCall(_URL, EndPoints.ManageOffers + "?OfferType=" + offerType, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ManageOffersLibrary> data = (List<ManageOffersLibrary>)baseResponse.Data;

            BaseResponse<ManageOffersMapping> mapping = new BaseResponse<ManageOffersMapping>();
            BaseResponse<ManageOffersMapping> mapping1 = new BaseResponse<ManageOffersMapping>();
            for (int i = 0; i < data.Count; i++)
            {
                response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + data[i].id, "GET", null);
                mapping1 = mapping1.JsonParseList(response);
                var dataresponse = (List<ManageOffersMapping>)mapping1.Data;
                data[i].offerItems = dataresponse.Select(details => new ManageOffersMapping
                {
                    id = details.id,
                    offerId = details.offerId,
                    categoryId = (int)details.categoryId,
                    sellerId = details.sellerId,
                    brandId = (int)details.brandId,
                    productId = details.productId,
                    getProductId = details.getProductId,
                    userId = details.userId,
                    getDiscountType = details.getDiscountType,
                    getDiscountValue = (decimal)details.getDiscountValue,
                    getProductPrice = (decimal)details.getProductPrice,
                    sellerOptIn = (bool)details.sellerOptIn,
                    optInSellerIds = details.optInSellerIds,
                    status = details.status,
                    CategoryIds = details.CategoryIds,
                    SellerIds = details.SellerIds,
                    Brandids = details.Brandids,
                    ProductIds = details.ProductIds,
                    CreatedBy = details.CreatedBy,
                    CreatedAt = details.CreatedAt,
                    ModifiedBy = details.ModifiedBy,
                    ModifiedAt = details.ModifiedAt,
                    offerName = details.offerName,
                    productName = details.productName,
                    categoryName = details.categoryName,
                    categoryPathNames = details.categoryPathNames,
                    SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                    BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                    UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                }).OrderBy(p => p.CreatedAt).ToList();
            }
            baseResponse.Data = data;
            return baseResponse;
        }

        public BaseResponse<ManageOffersLibrary> GetOfferByCode(string code, string? status=null)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(status))
            {
                url += "&Status=" + status;
            }
            var response = helper.ApiCall(_URL, EndPoints.ManageOffers + "?Code=" + code + url, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            if (baseResponse.code == 200)
            {
                ManageOffersLibrary data = (ManageOffersLibrary)baseResponse.Data;

                BaseResponse<ManageOffersMapping> mapping = new BaseResponse<ManageOffersMapping>();
                BaseResponse<ManageOffersMapping> mapping1 = new BaseResponse<ManageOffersMapping>();
                if (data.id != 0 && data.id != null)
                {
                    response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + data.id, "GET", null);
                    mapping1 = mapping1.JsonParseList(response);
                    var dataresponse = (List<ManageOffersMapping>)mapping1.Data;
                    data.offerItems = dataresponse.Select(details => new ManageOffersMapping
                    {
                        id = details.id,
                        offerId = details.offerId,
                        categoryId = (int)details.categoryId,
                        sellerId = details.sellerId,
                        brandId = (int)details.brandId,
                        productId = details.productId,
                        getProductId = details.getProductId,
                        userId = details.userId,
                        getDiscountType = details.getDiscountType,
                        getDiscountValue = (decimal)details.getDiscountValue,
                        getProductPrice = (decimal)details.getProductPrice,
                        sellerOptIn = (bool)details.sellerOptIn,
                        optInSellerIds = details.optInSellerIds,
                        status = details.status,
                        CategoryIds = details.CategoryIds,
                        SellerIds = details.SellerIds,
                        Brandids = details.Brandids,
                        ProductIds = details.ProductIds,
                        CreatedBy = details.CreatedBy,
                        CreatedAt = details.CreatedAt,
                        ModifiedBy = details.ModifiedBy,
                        ModifiedAt = details.ModifiedAt,
                        offerName = details.offerName,
                        productName = details.productName,
                        categoryName = details.categoryName,
                        categoryPathNames = details.categoryPathNames,
                        SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                        BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                        UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                    }).OrderBy(p => p.CreatedAt).ToList();
                }
                baseResponse.Data = data;
            }
            return baseResponse;
        }

        public BaseResponse<ManageOffersLibrary> GetOfferByStatus(string status)
        {
            var response = helper.ApiCall(_URL, EndPoints.ManageOffers + "?status=" + status, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ManageOffersLibrary> data = (List<ManageOffersLibrary>)baseResponse.Data;

            BaseResponse<ManageOffersMapping> mapping = new BaseResponse<ManageOffersMapping>();
            BaseResponse<ManageOffersMapping> mapping1 = new BaseResponse<ManageOffersMapping>();
            for (int i = 0; i < data.Count; i++)
            {
                response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + data[i].id, "GET", null);
                mapping1 = mapping1.JsonParseList(response);
                var dataresponse = (List<ManageOffersMapping>)mapping1.Data;
                data[i].offerItems = dataresponse.Select(details => new ManageOffersMapping
                {
                    id = details.id,
                    offerId = details.offerId,
                    categoryId = (int)details.categoryId,
                    sellerId = details.sellerId,
                    brandId = (int)details.brandId,
                    productId = details.productId,
                    getProductId = details.getProductId,
                    userId = details.userId,
                    getDiscountType = details.getDiscountType,
                    getDiscountValue = (decimal)details.getDiscountValue,
                    getProductPrice = (decimal)details.getProductPrice,
                    sellerOptIn = (bool)details.sellerOptIn,
                    optInSellerIds = details.optInSellerIds,
                    status = details.status,
                    CategoryIds = details.CategoryIds,
                    SellerIds = details.SellerIds,
                    Brandids = details.Brandids,
                    ProductIds = details.ProductIds,
                    CreatedBy = details.CreatedBy,
                    CreatedAt = details.CreatedAt,
                    ModifiedBy = details.ModifiedBy,
                    ModifiedAt = details.ModifiedAt,
                    offerName = details.offerName,
                    productName = details.productName,
                    categoryName = details.categoryName,
                    categoryPathNames = details.categoryPathNames,
                    SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                    BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                    UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                }).OrderBy(p => p.CreatedAt).ToList();
            }
            baseResponse.Data = data;
            return baseResponse;
        }

        public BaseResponse<ManageOffersLibrary> Search(string? searchtext = null, bool? showToCustomer = null, string? offerType = null, string? status = null, int? pageindex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }

            if (!string.IsNullOrEmpty(status))
            {
                url += "&status=" + status;
            }
            if (showToCustomer != null)
            {
                url += "&showToCustomer=" + showToCustomer;
            }

            if (!string.IsNullOrEmpty(offerType))
            {
                url += "&OfferType=" + offerType;
            }

            var response = helper.ApiCall(_URL, EndPoints.ManageOffers + "?PageIndex=" + pageindex + "&PageSize=" + pageSize + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<ManageOffersLibrary> data = (List<ManageOffersLibrary>)baseResponse.Data;

            BaseResponse<ManageOffersMapping> mapping = new BaseResponse<ManageOffersMapping>();
            BaseResponse<ManageOffersMapping> mapping1 = new BaseResponse<ManageOffersMapping>();
            for (int i = 0; i < data.Count; i++)
            {
                response = helper.ApiCall(_URL, EndPoints.ManageOffersMapping + "?offerId=" + data[i].id, "GET", null);
                mapping1 = mapping1.JsonParseList(response);
                var dataresponse = (List<ManageOffersMapping>)mapping1.Data;
                data[i].offerItems = dataresponse.Select(details => new ManageOffersMapping
                {
                    id = details.id,
                    offerId = details.offerId,
                    categoryId = (int)details.categoryId,
                    sellerId = details.sellerId,
                    brandId = (int)details.brandId,
                    productId = details.productId,
                    getProductId = details.getProductId,
                    userId = details.userId,
                    getDiscountType = details.getDiscountType,
                    getDiscountValue = (decimal)details.getDiscountValue,
                    getProductPrice = (decimal)details.getProductPrice,
                    sellerOptIn = (bool)details.sellerOptIn,
                    optInSellerIds = details.optInSellerIds,
                    status = details.status,
                    CategoryIds = details.CategoryIds,
                    SellerIds = details.SellerIds,
                    Brandids = details.Brandids,
                    ProductIds = details.ProductIds,
                    CreatedBy = details.CreatedBy,
                    CreatedAt = details.CreatedAt,
                    ModifiedBy = details.ModifiedBy,
                    ModifiedAt = details.ModifiedAt,
                    offerName = details.offerName,
                    productName = details.productName,
                    categoryName = details.categoryName,
                    categoryPathNames = details.categoryPathNames,
                    SellerName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(details.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null,
                    BrandName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? null : null,
                    UserName = !string.IsNullOrEmpty(details.ExtraDetails) ? JObject.Parse(details.ExtraDetails)["CustomerDetails"]?["FullName"]?.ToString() ?? null : null,
                }).OrderBy(p => p.CreatedAt).ToList();
            }
            baseResponse.Data = data;
            return baseResponse;
        }

    }
}
