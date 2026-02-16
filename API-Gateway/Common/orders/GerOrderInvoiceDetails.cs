using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using BarcodeStandard;
using ClosedXML.Graphics;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json.Linq;
using SkiaSharp;
using System.Collections.Generic;
using System;

namespace API_Gateway.Common.orders
{
    public class GerOrderInvoiceDetails
    {
        private readonly IConfiguration _configuration;
        public string CatelogueURL = string.Empty;
        public string OrderURL = string.Empty;
        public string IdServerURL = string.Empty;
        public string UserURL = string.Empty;
        public string UserId = string.Empty;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        private GetOrders getorder;
        public GerOrderInvoiceDetails(IConfiguration configuration, HttpContext httpContext, string Userid)
        {
            UserId = Userid;
            _httpContext = httpContext;
            _configuration = configuration;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IdServerURL = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            UserURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            OrderURL = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            helper = new ApiHelper(_httpContext);
        }
        public InvoiceDetails GetInvoice(string Packageid)
        {

            InvoiceDetails invoicedataDTO = new InvoiceDetails();
            List<InvoiceDto> Invoice = new List<InvoiceDto>();
            BaseResponse<InvoiceDto> baseResponse = new BaseResponse<InvoiceDto>();

            BaseResponse<TaxTypeValueLibrary> taxValuebaseResponse = new BaseResponse<TaxTypeValueLibrary>();
            List<TaxTypeValueLibrary> taxlst = new List<TaxTypeValueLibrary>();
            var Taxresponse = helper.ApiCall(CatelogueURL, EndPoints.TaxTypeValue + "?PageIndex=0&PageSize=0", "GET", null);
            taxValuebaseResponse = taxValuebaseResponse.JsonParseList(Taxresponse);
            if(taxValuebaseResponse.code == 200)
            {
                taxlst = (List<TaxTypeValueLibrary>)taxValuebaseResponse.Data;
            }

            
            var response = helper.ApiCall(OrderURL, EndPoints.Orders + "/Invoice?Packageid=" + Packageid, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            if (baseResponse.code == 200)
            {
                Invoice = (List<InvoiceDto>)baseResponse.Data;

                InvoiceItemDetails InvoiceProduct = BindProduct(Invoice, taxlst);

                invoicedataDTO = Invoice.Select(x => new InvoiceDetails
                {
                    Id = Convert.ToInt32(x.Id),
                    OrderId = Convert.ToInt32(x.OrderID),
                    OrderNo = x.OrderNo,
                    OrderItemIDs = x.OrderItemIDs,
                    OrderDate = Convert.ToDateTime(x.OrderDate),
                    InvoiceNo = x.InvoiceNo,
                    InvoiceDate = Convert.ToDateTime(x.InvoiceDate),

                    SellerTradeName = x.SellerTradeName,
                    SellerLegalName = x.SellerLegalName,
                    RegisteredGSTNo = x.SellerGSTNo,
                    RegisteredAddressLine1 = x.SellerRegisteredAddressLine1,
                    RegisteredAddressLine2 = x.SellerRegisteredAddressLine2,
                    RegisteredLendmark = x.SellerRegisteredLandmark,
                    RegisteredPincode = x.SellerRegisteredPincode,
                    RegisteredCity = x.SellerRegisteredCity,
                    RegisteredState = x.SellerRegisteredState,
                    RegisteredCountry = x.SellerRegisteredCountry,

                    SellerPickupAddressLine1 = x.SellerPickupAddressLine1,
                    SellerPickupAddressLine2 = x.SellerPickupAddressLine2,
                    SellerPickupLandmark = x.SellerPickupLandmark,
                    SellerPickupPincode = x.SellerPickupPincode,
                    SellerPickupCity = x.SellerPickupCity,
                    SellerPickupState = x.SellerPickupState,
                    SellerPickupCountry = x.SellerPickupCountry,
                    SellerPickupTaxNo = x.SellerPickupTaxNo,

                    UserName = x.DropContactPersonName,
                    BillToMobileNo = x.DropContactPersonMobileNo,
                    BillToEmailID = x.DropContactPersonEmailID,
                    BillToAddressLine1 = x.DropAddressLine1,
                    BillToAddressLine2 = x.DropAddressLine2,
                    BillToLendmark = x.DropLandmark,
                    BillToPincode = Convert.ToInt32(x.DropPincode).ToString(),
                    BillToGSTNo = null,
                    BillToCity = x.DropCity,
                    BillToState = x.DropState,
                    BillToCountry = x.DropCountry,
                    BillToTaxNo = x.DropTaxNo,

                    ShipToMobileNo = x.DropContactPersonMobileNo,
                    ShipToAddressLine1 = x.DropAddressLine1,
                    ShipToAddressLine2 = x.DropAddressLine2,
                    ShipToLendmark = x.DropLandmark,
                    ShipToPincode = Convert.ToInt32(x.DropPincode).ToString(),
                    ShipToGSTNo = null,
                    ShipToCity = x.DropCity,
                    ShipToState = x.DropState,
                    ShipToCountry = x.DropCountry,
                    ShipToTaxNo = x.DropTaxNo,

                    ExtraCharges = Extracharges(Convert.ToDecimal(Convert.ToDecimal(x.TotalExtracharges).ToString("N0")), InvoiceProduct.ProductItems.FirstOrDefault().Taxes, taxlst),
                    TotalExtraCharges = Convert.ToDecimal(Convert.ToDecimal(x.TotalExtracharges).ToString("N0")),

                    CodCharges = Cod(Convert.ToDecimal(x.CODCharge), InvoiceProduct.ProductItems.FirstOrDefault().Taxes, taxlst),
                    TotalCodCharges = Convert.ToDecimal(x.CODCharge),

                    ShippingCharges = ShippingCharges(Convert.ToDecimal(Invoice.Sum(p => Convert.ToDecimal(p.ShippingCharge))), InvoiceProduct.ProductItems.FirstOrDefault().Taxes, taxlst),
                    TotalShippingCharges = Convert.ToDecimal(Invoice.Sum(p => Convert.ToDecimal(p.ShippingCharge))),


                    PaymentMode = x.PaymentMode,
                    IsCouponApplied = x.IsCouponApplied,
                    Coupon = x.Coupon,
                    CoupontDiscount = x.CoupontDiscount,
                    CoupontDetails = x.CoupontDetails,
                    ItemDetails = InvoiceProduct,
                    InvoiceAmount = Convert.ToDecimal(x.InvoiceAmount) + Convert.ToDecimal(x.CODCharge),

                }).FirstOrDefault();
            }

            return invoicedataDTO;

        }

        public List<ShippingLabelDto> GetShippingLabel(string Packageid)
        {

            List<ShippingLabelDto> shippingdataDTO = new List<ShippingLabelDto>();
            List<InvoiceDto> Invoice = new List<InvoiceDto>();
            BaseResponse<InvoiceDto> baseResponse = new BaseResponse<InvoiceDto>();


            var response = helper.ApiCall(OrderURL, EndPoints.Orders + "/Invoice?Packageid=" + Packageid, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            if (baseResponse.code == 200)
            {
                Invoice = (List<InvoiceDto>)baseResponse.Data;
                Invoice = Invoice.DistinctBy(p => p.Id).ToList();
                foreach (var x in Invoice)
                {
                    for (int i = 0; i < x.NoOfPackage; i++)
                    {
                        ShippingLabelDto labeldata = new ShippingLabelDto();
                        labeldata.OrderID = x.OrderID;
                        labeldata.OrderNo = x.OrderNo;
                        labeldata.SellerTradeName = x.SellerTradeName;
                        labeldata.SellerLegalName = x.SellerLegalName;
                        labeldata.SellerPickupAddressLine1 = x.SellerPickupAddressLine1;
                        labeldata.SellerPickupAddressLine2 = x.SellerPickupAddressLine2;
                        labeldata.SellerPickupLandmark = x.SellerPickupLandmark;
                        labeldata.SellerPickupPincode = x.SellerPickupPincode;
                        labeldata.SellerPickupCity = x.SellerPickupCity;
                        labeldata.SellerPickupState = x.SellerPickupState;
                        labeldata.SellerPickupCountry = x.SellerPickupCountry;
                        labeldata.SellerPickupContactPersonName = x.SellerPickupContactPersonName;
                        labeldata.SellerPickupContactPersonMobileNo = x.SellerPickupContactPersonMobileNo;
                        labeldata.SellerPickupTaxNo = x.SellerPickupTaxNo;
                        labeldata.InvoiceAmount = Convert.ToDecimal(x.InvoiceAmount) + Convert.ToDecimal(x.CODCharge);
                        labeldata.DropContactPersonName = x.DropContactPersonName;
                        labeldata.DropContactPersonMobileNo = x.DropContactPersonMobileNo;
                        labeldata.DropContactPersonEmailID = x.DropContactPersonEmailID;
                        labeldata.DropCompanyName = x.DropCompanyName;
                        labeldata.DropAddressLine1 = x.DropAddressLine1;
                        labeldata.DropAddressLine2 = x.DropAddressLine2;
                        labeldata.DropLandmark = x.DropLandmark;
                        labeldata.DropPincode = x.DropPincode;
                        labeldata.DropCity = x.DropCity;
                        labeldata.DropState = x.DropState;
                        labeldata.DropCountry = x.DropCountry;
                        labeldata.DropTaxNo = x.DropTaxNo;
                        labeldata.PaymentMode = x.PaymentMode;
                        labeldata.AwbNo = x.AwbNo;
                        labeldata.ShippingPartner = x.ShippingPartner;
                        labeldata.CourierName = x.CourierName;
                        labeldata.NoOfPackage = x.NoOfPackage;
                        labeldata.Weight = x.Weight;
                        labeldata.ShippingDate = x.ShippingDate;
                        labeldata.OrderDate = x.OrderDate;
                        shippingdataDTO.Add(labeldata);
                    }
                }

                //shippingdataDTO = Invoice.Select(x => new ShippingLabelDto
                //{

                //    OrderID = x.OrderID,
                //    OrderNo = x.OrderNo,
                //    SellerTradeName = x.SellerTradeName,
                //    SellerLegalName = x.SellerLegalName,
                //    SellerPickupAddressLine1 = x.SellerPickupAddressLine1,
                //    SellerPickupAddressLine2 = x.SellerPickupAddressLine2,
                //    SellerPickupLandmark = x.SellerPickupLandmark,
                //    SellerPickupPincode = x.SellerPickupPincode,
                //    SellerPickupCity = x.SellerPickupCity,
                //    SellerPickupState = x.SellerPickupState,
                //    SellerPickupCountry = x.SellerPickupCountry,
                //    SellerPickupContactPersonName = x.SellerPickupContactPersonName,
                //    SellerPickupContactPersonMobileNo = x.SellerPickupContactPersonMobileNo,
                //    SellerPickupTaxNo = x.SellerPickupTaxNo,
                //    InvoiceAmount = x.InvoiceAmount,
                //    DropContactPersonName = x.DropContactPersonName,
                //    DropContactPersonMobileNo = x.DropContactPersonMobileNo,
                //    DropContactPersonEmailID = x.DropContactPersonEmailID,
                //    DropCompanyName = x.DropCompanyName,
                //    DropAddressLine1 = x.DropAddressLine1,
                //    DropAddressLine2 = x.DropAddressLine2,
                //    DropLandmark = x.DropLandmark,
                //    DropPincode = x.DropPincode,
                //    DropCity = x.DropCity,
                //    DropState = x.DropState,
                //    DropCountry = x.DropCountry,
                //    DropTaxNo = x.DropTaxNo,
                //    PaymentMode = x.PaymentMode,
                //    AwbNo = x.AwbNo,
                //    ShippingPartner = x.ShippingPartner,
                //    CourierName = x.CourierName,
                //    NoOfPackage = x.NoOfPackage,
                //    Weight = x.Weight,
                //    ShippingDate = x.ShippingDate,
                //    OrderDate = x.OrderDate
                //}).ToList();

            }

            return shippingdataDTO;

        }

        public InvoiceItemDetails BindProduct(List<InvoiceDto> ss, List<TaxTypeValueLibrary> taxlst)
        {
            List<InvoiceDto> tempList = (List<InvoiceDto>)ss;
            List<InvoiceItems> invoiceItemslst = new List<InvoiceItems>();
            getorder = new GetOrders(_configuration, _httpContext, UserId);
            var had = string.Empty;
            foreach (var item in tempList)
            {
                List<TaxMapping> taxmaplist = GetTaxMappings(Convert.ToInt32(item.OrderTaxRateId), item.DropCity, item.DropState, item.DropCountry, item.SellerPickupCity, item.SellerPickupState, item.SellerPickupCountry);
                string taxdata = Tax(item.OrderTaxRate, Convert.ToDecimal(item.SellingPrice), item.DropCity, item.DropState, item.DropCountry, taxmaplist);
                InvoiceItems invoiceItems = new InvoiceItems();
                invoiceItems.ProductId = Convert.ToInt32(item.ProductID);
                invoiceItems.SellerProductId = Convert.ToInt32(item.SellerProductID);
                invoiceItems.SellerId = item.SellerID;
                invoiceItems.BrandId = Convert.ToInt32(item.BrandID);
                invoiceItems.Brand = getorder.getBrand(Convert.ToInt32(item.BrandID)).Name.ToString();
                invoiceItems.ProductName = item.ProductName;
                invoiceItems.SKUCode = item.ProductSKUCode;
                invoiceItems.ItemPrice = Convert.ToDecimal(item.MRP);
                invoiceItems.Discount = Convert.ToDecimal(item.Discount);
                invoiceItems.Amount = Convert.ToDecimal(item.SellingPrice);
                invoiceItems.Quantity = Convert.ToInt32(item.Qty);
                invoiceItems.Taxes = taxdata;
                invoiceItems.TotalAmount = Convert.ToDecimal(item.TotalAmount);
                invoiceItems.Size = item.SizeValue;
                invoiceItems.OrderTaxRate = item.OrderTaxRate;
                invoiceItems.OrderTaxRateId = Convert.ToInt32(item.OrderTaxRateId);
                invoiceItems.HSNCode = item.HSNCode;
                invoiceItems.Color = string.Join("-", getorder.GetColor(Convert.ToInt32(item.ProductID))?.Select(c => c.ColorName) ?? Enumerable.Empty<string>());
                invoiceItems.IsCouponApplied = item.IsCouponApplied;
                invoiceItems.Coupon = item.Coupon;
                invoiceItems.CoupontDiscount = item.CoupontDiscount;
                invoiceItems.CoupontDetails = item.CoupontDetails;
                invoiceItemslst.Add(invoiceItems);
            }

            InvoiceItemDetails itemDetails = new InvoiceItemDetails();
            itemDetails.ProductItems = invoiceItemslst.ToList();
            itemDetails.TotalAmount = invoiceItemslst.Sum(x => x.TotalAmount);

            return itemDetails;
        }

        public string Tax(string OrderTaxRate, decimal Price, string dropcity, string dropState, string dropCountry, List<TaxMapping> taxMappings)
        {
            JObject Taxes = new JObject();

            var Rate = JObject.Parse(OrderTaxRate)["Rate"]?.Value<decimal>();
            decimal TaxableAmount = Convert.ToDecimal(Convert.ToDecimal((Convert.ToDecimal(Price) / (1 + (Rate / 100)))).ToString("N2"));

            string json = OrderTaxRate;

            // Parse the JSON string into a JObject
            JObject TaxRate = JObject.Parse(json);
            Taxes["TaxAbleAmt"] = TaxableAmount;
            // Iterate over all key-value pairs
            foreach (var property in TaxRate.Properties())
            {
                string key = property.Name;
                JToken value = property.Value;

                foreach (var item in taxMappings)
                {
                    if (!string.IsNullOrEmpty(item.SpecificState))
                    {
                        if (item.SpecificTaxType == key)
                        {
                            if (item.SpecificState.Contains(dropcity))
                            {
                                Taxes[item.SpecificTaxType.ToString()] = Convert.ToDecimal(Convert.ToDecimal((TaxableAmount * Convert.ToDecimal(value)) / 100).ToString("N2")) + " @ " + value + "%";
                                //Taxes[item.SpecificTaxType.ToString() + "Rate"] = value;

                            }
                            else if (item.SpecificState.Contains(dropState))
                            {
                                Taxes[item.SpecificTaxType.ToString()] = Convert.ToDecimal(Convert.ToDecimal((TaxableAmount * Convert.ToDecimal(value)) / 100).ToString("N2")) + " @ " + value + "%";
                                //Taxes[item.SpecificTaxType.ToString() + "Rate"] = value;
                            }
                            else if (item.SpecificState.Contains(dropCountry))
                            {
                                Taxes[item.SpecificTaxType.ToString()] = Convert.ToDecimal(Convert.ToDecimal((TaxableAmount * Convert.ToDecimal(value)) / 100).ToString("N2")) + " @ " + value + "%";
                                //Taxes[item.SpecificTaxType.ToString() + "Rate"] = value;
                            }
                            else
                            {
                                if (item.TaxType == key)
                                {
                                    Taxes[item.TaxType.ToString()] = Convert.ToDecimal(Convert.ToDecimal((TaxableAmount * Convert.ToDecimal(value)) / 100).ToString("N2")) + " @ " + value + "%";
                                    //Taxes[item.TaxType.ToString() + "Rate"] = value;
                                }
                                else
                                {
                                    //if (key.ToLower() != "rate")
                                    //{
                                    //    Taxes[key.ToString()] = 0;
                                    //    Taxes[key.ToString() + " Rate"] = value;
                                    //}
                                }
                            }
                        }
                        else
                        {
                            //if (key.ToLower() != "rate")
                            //{
                            //    Taxes[key.ToString()] = 0;
                            //    Taxes[key.ToString() + " Rate"] = value;
                            //}
                        }
                    }
                    else
                    {
                        if (item.TaxType == key)
                        {
                            Taxes[item.TaxType.ToString()] = Convert.ToDecimal(Convert.ToDecimal((TaxableAmount * Convert.ToDecimal(value)) / 100).ToString("N2")) + " @ " + value + "%";
                            //Taxes[item.TaxType.ToString() + "Rate"] = value;
                        }
                        else
                        {
                            //if (key.ToLower() != "rate")
                            //{
                            //    Taxes[key.ToString()] = 0;
                            //    Taxes[key.ToString() + " Rate"] = value;
                            //}
                        }
                    }
                }

                //Taxes[key] = DropState == PickupState ? (TaxableAmount * Convert.ToDecimal(value)) / 100 : 0;
                //Taxes[key] = DropState != PickupState ? (TaxableAmount * Convert.ToDecimal(value)) / 100 : 0;

                //Console.WriteLine($"Key: {key}, Value: {value}");
            }


            //CGSTAmount = (DropState == PickupState ? (JObject.Parse(x.OrderTaxRate)["CGST"]?.Value<decimal>() / 100.0m) * x.SellingPrice : 0.00m),
            //SGSTAmount = (DropState == PickupState ? (JObject.Parse(x.OrderTaxRate)["SGST"]?.Value<decimal>() / 100.0m) * x.SellingPrice : 0.00m),
            //IGSTAmount = (DropState != PickupState ? (JObject.Parse(x.OrderTaxRate)["IGST"]?.Value<decimal>() / 100.0m) * x.SellingPrice : 0.00m),

            return Taxes.ToString();
        }
        public string Cod(decimal CodCharges, string Tax, List<TaxTypeValueLibrary> taxlst)
        {
            TaxTypeValueLibrary taxTypeValue = taxlst.Where(p => p.Name.ToLower() == "cod").FirstOrDefault();

            JObject Taxes = new JObject();
            
            var Rate = JObject.Parse(taxTypeValue.Value)["Rate"]?.Value<decimal>();
            decimal TaxableAmount = Convert.ToDecimal(Convert.ToDecimal((Convert.ToDecimal(CodCharges) / (1 + (Rate / 100)))).ToString("N2"));

            Taxes["TaxAbleAmt"] = TaxableAmount;

            var taxes = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(Tax);
            foreach (var i in taxes)
            {
                if (!i.Key.Contains("Rate") && !i.Key.Contains("TaxAbleAmt"))
                {
                    var VRate = JObject.Parse(taxTypeValue.Value)[i.Key.ToString()]?.Value<decimal>();
                    Taxes[i.Key.ToString()] = Convert.ToDecimal(Convert.ToDecimal((TaxableAmount * Convert.ToDecimal(VRate)) / 100).ToString("N2")) + " @ " + VRate + "%";
                }
            }

            return Taxes.ToString();
        }
        public string Extracharges(decimal Extracharges, string Tax, List<TaxTypeValueLibrary> taxlst)
        {
            TaxTypeValueLibrary taxTypeValue = taxlst.Where(p => p.Name.ToLower() == "extracharges").FirstOrDefault();

            JObject Taxes = new JObject();

            var Rate = JObject.Parse(taxTypeValue.Value)["Rate"]?.Value<decimal>();
            decimal TaxableAmount = Convert.ToDecimal(Convert.ToDecimal((Convert.ToDecimal(Extracharges) / (1 + (Rate / 100)))).ToString("N2"));

            Taxes["TaxAbleAmt"] = TaxableAmount;

            var taxes = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(Tax);
            foreach (var i in taxes)
            {
                if (!i.Key.Contains("Rate") && !i.Key.Contains("TaxAbleAmt"))
                {
                    var VRate = JObject.Parse(taxTypeValue.Value)[i.Key.ToString()]?.Value<decimal>();
                    Taxes[i.Key.ToString()] = Convert.ToDecimal(Convert.ToDecimal((TaxableAmount * Convert.ToDecimal(VRate)) / 100).ToString("N2")) + " @ " + VRate + "%";
                }
            }

            return Taxes.ToString();
        }
        public string ShippingCharges(decimal ShippingCharge, string Tax, List<TaxTypeValueLibrary> taxlst)
        {
            TaxTypeValueLibrary taxTypeValue = taxlst.Where(p => p.Name.ToLower() == "shipping").FirstOrDefault();

            JObject Taxes = new JObject();

            var Rate = JObject.Parse(taxTypeValue.Value)["Rate"]?.Value<decimal>();
            decimal TaxableAmount = Convert.ToDecimal(Convert.ToDecimal((Convert.ToDecimal(ShippingCharge) / (1 + (Rate / 100)))).ToString("N2"));

            Taxes["TaxAbleAmt"] = TaxableAmount;

            var taxes = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(Tax);
            foreach (var i in taxes)
            {
                if (!i.Key.Contains("Rate") && !i.Key.Contains("TaxAbleAmt"))
                {
                    var VRate = JObject.Parse(taxTypeValue.Value)[i.Key.ToString()]?.Value<decimal>();
                    Taxes[i.Key.ToString()] = Convert.ToDecimal(Convert.ToDecimal((TaxableAmount * Convert.ToDecimal(VRate)) / 100).ToString("N2")) + " @ " + VRate + "%";
                }
            }

            return Taxes.ToString();
        }
        public List<TaxMapping> GetTaxMappings(int taxValueId, string DropCity, string DropState, string DropCountry, string PickupCity, string PickupState, string PickupCountry)
        {
            BaseResponse<TaxMapping> TaxbaseResponse = new BaseResponse<TaxMapping>();
            var temp = helper.ApiCall(CatelogueURL, EndPoints.TaxMapping + "?taxValueId=" + taxValueId + "&PageIndex=0&PageSize=0", "GET", null);
            TaxbaseResponse = TaxbaseResponse.JsonParseList(temp);
            List<TaxMapping> tempList = (List<TaxMapping>)TaxbaseResponse.Data;

            List<TaxMapping> lsttaxmap = new List<TaxMapping>();

            if (DropCountry == PickupCountry)
            {
                lsttaxmap = tempList.Where(p => p.TaxMapBy.ToLower() == "same country").ToList();

                if (DropState == PickupState)
                {
                    lsttaxmap = tempList.Where(p => p.TaxMapBy.ToLower() == "same state").ToList();

                    if (DropCity == PickupCity)
                    {
                        lsttaxmap = tempList.Where(p => p.TaxMapBy.ToLower() == "same city").ToList();
                    }
                    else
                    {
                        lsttaxmap = tempList.Where(p => p.TaxMapBy.ToLower() == "different city").ToList();
                    }
                }
                else
                {
                    lsttaxmap = tempList.Where(p => p.TaxMapBy.ToLower() == "different state").ToList();
                }
            }
            else
            {
                lsttaxmap = tempList.Where(p => p.TaxMapBy.ToLower() == "different country").ToList();
            }


            return lsttaxmap;
        }

        public string InvoiceHtmlContent(InvoiceDetails res)
        {
            string htmlContent = "<div style = 'background-color: #FFFFFF; font-family: Arial, sans-serif;' >";

            int col = 6;
            int taxcol = 1;
            decimal TotalDiscount = 0;

            #region Table 1 

            htmlContent += "<table width='100%' style='font-size: 12px; border-top: 1px solid #000; border-bottom: 1px solid #000;'>";
            htmlContent += "<tbody>";
            htmlContent += "<tr>";

            htmlContent += "<td style = 'padding: 5px 7px 5px 0px; text-align: left; font-size: 20px; width: 33.33%'> ";
            htmlContent += "<div style= 'font-size: 13px;'> ";
            htmlContent += !string.IsNullOrEmpty(res.SellerLegalName) ? res.SellerLegalName + "<br />" : "";
            htmlContent += !string.IsNullOrEmpty(res.SellerPickupAddressLine1) ? res.SellerPickupAddressLine1 + ", "
                            + (!string.IsNullOrEmpty(res.SellerPickupAddressLine2) ? res.SellerPickupAddressLine2 + ", " : "")
                            + (!string.IsNullOrEmpty(res.SellerPickupLandmark) ? res.SellerPickupLandmark + ", " : "")
                            + "<br /> " + (!string.IsNullOrEmpty(res.SellerPickupCity) ? res.SellerPickupCity + ", " : "")
                            + (!string.IsNullOrEmpty(res.SellerPickupState) ? res.SellerPickupState + ", " : "")
                            + (!string.IsNullOrEmpty(res.SellerPickupCountry) ? res.SellerPickupCountry + " - " : "")
                            + (res.SellerPickupPincode != null ? res.SellerPickupPincode : "") : "";
            htmlContent += !string.IsNullOrEmpty(res.SellerPickupTaxNo) ? "<br /> GST No :" + res.SellerPickupTaxNo : "";
            htmlContent += "</div> ";
            htmlContent += "</td> ";

            htmlContent += "<td style = 'padding: 5px 7px 5px 0px; text-align: left; font-size: 20px; width: 33.33%'> TAX INVOICE";
            htmlContent += "<table width='100%' style= 'font-size: 13px;'> ";
            htmlContent += "<tbody>";

            htmlContent += "<tr>";
            htmlContent += "<td style = 'padding: 10px 0px 5px 0px;'> Invoice No. #: " + res.InvoiceNo + "<br /> "
                                                                    + "Invoice Date. : " + Convert.ToDateTime(res.InvoiceDate).ToString("dd/MM/yyyy") + "<br /> "
                                                                    + "Order No. : " + res.OrderNo + "<br /> "
                                                                    + "Order Date. : " + Convert.ToDateTime(res.OrderDate).ToString("dd/MM/yyyy");
            htmlContent += "</td> ";
            htmlContent += "</tr> ";

            htmlContent += "</tbody> ";
            htmlContent += "</table> ";
            htmlContent += "</td> ";

            htmlContent += "<td style = 'padding: 5px 7px 5px 0px; text-align: left; font-size: 20px; width: 33.33%'>";
            htmlContent += "<div style = 'margin-bottom: 20px; text-align: center;'>";
            htmlContent += "<img src = 'https://hashkartfrontend.hashtechy.space/_next/image?url=%2Fimages%2Flogo-hashkart.png&w=128&q=75' alt = 'Logo' style = 'max-width: 100px; margin-bottom: 10px;' >";
            htmlContent += "</div>";
            htmlContent += "</td> ";

            htmlContent += "</tr> ";
            htmlContent += "</tbody> ";
            htmlContent += "</table> ";

            #endregion

            #region Table 2

            htmlContent += "<table width='100%' style='font-size: 12px;'>";
            htmlContent += "<tbody>";

            htmlContent += "<tr>";

            htmlContent += "<td style = 'width: 50%; padding: 5px 7px 5px 0px; border-bottom: 1px solid #000;'> Bill To : ";
            htmlContent += "<div style= 'font-size: 13px;'> ";
            htmlContent += !string.IsNullOrEmpty(res.UserName) ? res.UserName + "<br />" : "";
            htmlContent += !string.IsNullOrEmpty(res.BillToAddressLine1) ? res.BillToAddressLine1 + ", "
                            + (!string.IsNullOrEmpty(res.BillToAddressLine2) ? res.BillToAddressLine2 + ", " : "")
                            + (!string.IsNullOrEmpty(res.BillToLendmark) ? res.BillToLendmark + ", " : "")
                            + "<br /> " + (!string.IsNullOrEmpty(res.BillToCity) ? res.BillToCity + ", " : "")
                            + (!string.IsNullOrEmpty(res.BillToState) ? res.BillToState + ", " : "")
                            + (!string.IsNullOrEmpty(res.BillToCountry) ? res.BillToCountry + " - " : "")
                            + (res.BillToPincode != null ? res.BillToPincode : "") : "";
            htmlContent += !string.IsNullOrEmpty(res.BillToTaxNo) ? "<br /> GST No :" + res.BillToTaxNo : "";
            htmlContent += "</div> ";
            htmlContent += "</td> ";

            htmlContent += "<td style = 'width: 50%; padding: 5px 0 5px 0px; border-bottom: 1px solid #000;'> Ship To : ";
            htmlContent += "<div style= 'font-size: 13px;'> ";
            htmlContent += !string.IsNullOrEmpty(res.UserName) ? res.UserName + "<br />" : "";
            htmlContent += !string.IsNullOrEmpty(res.ShipToAddressLine1) ? res.ShipToAddressLine1 + ", "
                            + (!string.IsNullOrEmpty(res.ShipToAddressLine2) ? res.ShipToAddressLine2 + ", " : "")
                            + (!string.IsNullOrEmpty(res.ShipToLendmark) ? res.ShipToLendmark + ", " : "")
                            + "<br /> " + (!string.IsNullOrEmpty(res.ShipToCity) ? res.ShipToCity + ", " : "")
                            + (!string.IsNullOrEmpty(res.ShipToState) ? res.ShipToState + ", " : "")
                            + (!string.IsNullOrEmpty(res.ShipToCountry) ? res.ShipToCountry + " - " : "")
                            + (res.ShipToPincode != null ? res.ShipToPincode : "") : "";
            htmlContent += !string.IsNullOrEmpty(res.ShipToTaxNo) ? "<br /> GST No :" + res.ShipToTaxNo : "";
            htmlContent += "</div> ";
            htmlContent += "</td> ";

            htmlContent += "</tr> ";

            htmlContent += "</tbody> ";
            htmlContent += "</table> ";

            #endregion

            htmlContent += "<table style = 'font-size: 12px; text-align: center;'>";
            htmlContent += "<thead>";
            htmlContent += "<tr>";
            htmlContent += "<th style = 'border: 1px solid #000; padding: 5px; background-color: #efefef; width: 25%;' > Items </th>";
            htmlContent += "<th style = 'border: 1px solid #000; padding: 5px; background-color: #efefef; width: 8%;' > ItemPrice </th>";
            htmlContent += "<th style = 'border: 1px solid #000; padding: 5px; background-color: #efefef; width: 5%;' > Disc. </th>";
            htmlContent += "<th style = 'border: 1px solid #000; padding: 5px; background-color: #efefef; width: 5%;' > Qty. </th>";
            htmlContent += "<th style = 'border: 1px solid #000; padding: 5px; background-color: #efefef; width: 10%;' > Amount </th>";

            if (res.ItemDetails.ProductItems.Count > 0)
            {
                var tax = res.ItemDetails.ProductItems.FirstOrDefault().Taxes;
                var taxes = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(tax);
                taxcol = Convert.ToInt32(taxes.Count());
                decimal c = Convert.ToDecimal((36 / Convert.ToInt32(taxes.Count())).ToString("N2"));
                col = col + Convert.ToInt32(taxes.Count());

                foreach (var i in taxes)
                {
                    if (!i.Key.Contains("Rate"))
                    {
                        htmlContent += "<th style = 'border: 1px solid #000; padding: 5px; background-color: #efefef; width: " + c + "%;' > " + i.Key + " </th>";
                    }
                }
            }

            htmlContent += "<th style = 'border: 1px solid #000; padding: 5px; background-color: #efefef; width: 11%;' > Total </th>";
            htmlContent += "</tr><hr/>";
            htmlContent += "</thead>";

            htmlContent += "<tbody>";
            if (res.ItemDetails.ProductItems.Count > 0)
            {
                res.ItemDetails.ProductItems.ForEach(x =>
                {
                    TotalDiscount = TotalDiscount + Convert.ToDecimal(x.CoupontDiscount);

                    htmlContent += "<tr>";
                    htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; text-align:left;' > "
                        + x.ProductName + " " +
                        "<br /> Brand : " + x.Brand + " " +
                        "<br /> Size : " + x.Size + " " +
                        "<br /> Color : " + x.Color + " " +
                        "<br /> HSN Code : " + x.HSNCode + " " +
                    "</td>";
                    htmlContent += "<td style = 'border: 1px solid #000; padding: 5px;' > " + x.ItemPrice + " </td>";
                    htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; ' > " + x.Discount + "</td>";
                    htmlContent += "<td style = 'border: 1px solid #000; padding: 5px;' > " + x.Quantity + "</td>";
                    htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; ' > " + x.Amount + "</td>";

                    var tax = x.Taxes;
                    var taxes = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(tax);
                    foreach (var i in taxes)
                    {
                        if (!i.Key.Contains("Rate"))
                        {
                            htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; ' > " + i.Value + " </td>";
                        }
                    }

                    htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; ' > " + x.TotalAmount + "</td>";

                    htmlContent += "</tr>";

                    if (!string.IsNullOrEmpty(x.ProductSeriesNo))
                    {
                        htmlContent += "<tr>";
                        htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; text-align:left;' colspan='" + col + "'> IMEINo : " + x.ProductSeriesNo + "</td>";
                        htmlContent += "</tr>";
                    }
                });

                htmlContent += "<tr>";
                htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; text-align: right;' colspan='" + (col - 1) + "'> Subtotal : </td>";
                htmlContent += "<td style = 'border: 1px solid #000; padding: 5px;' > " + res.ItemDetails.TotalAmount + " </td>";
                htmlContent += "</tr>";
                
                #region Extracharges

                htmlContent += "<tr>";
                htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; text-align: right;' colspan='" + (col - taxcol - 1) + "'> Extra Charges : </td>";

                var Extrachargestax = res.ExtraCharges;
                var Extrachargestaxs = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(Extrachargestax);
                foreach (var i in Extrachargestaxs)
                {
                    if (!i.Key.Contains("Rate"))
                    {
                        htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; ' > " + i.Value + " </td>";
                    }
                }

                htmlContent += "<td style = 'border: 1px solid #000; padding: 5px;' > " + res.TotalExtraCharges + " </td>";
                htmlContent += "</tr>";

                #endregion Extracharges


                #region Shipping Charges

                htmlContent += "<tr>";
                htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; text-align: right;' colspan='" + (col - taxcol - 1) + "'> Shipping Charges : </td>";

                var Shiptax = res.ShippingCharges;
                var Shiptaxes = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(Shiptax);
                foreach (var i in Shiptaxes)
                {
                    if (!i.Key.Contains("Rate"))
                    {
                        htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; ' > " + i.Value + " </td>";
                    }
                }

                htmlContent += "<td style = 'border: 1px solid #000; padding: 5px;' > " + res.TotalShippingCharges + " </td>";
                htmlContent += "</tr>";

                #endregion

                if (res.PaymentMode.ToLower() == "cod")
                {
                    htmlContent += "<tr>";
                    htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; text-align: right;' colspan='" + (col - taxcol - 1) + "'> Cod Charges : </td>";

                    var codtax = res.CodCharges;
                    var codtaxes = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(codtax);
                    foreach (var i in codtaxes)
                    {
                        if (!i.Key.Contains("Rate"))
                        {
                            htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; ' > " + i.Value + " </td>";
                        }
                    }

                    htmlContent += "<td style = 'border: 1px solid #000; padding: 5px;' > " + res.TotalCodCharges + " </td>";
                    htmlContent += "</tr>";
                }

                // Text to be encoded in the barcode

                //string barcodeText = "SW_0101010";

                //string base64String = GenerateBarcode(barcodeText);

                ////Include the barcode image in the HTML content
                //htmlContent += "<tr>";
                //htmlContent += "<td style='border: 1px solid #000; padding: 5px; text-align:right;' colspan='" + (col - 3) + "'>Other Cell Content</td>";
                //htmlContent += "<td style='border: 1px solid #000; padding: 5px width=250px; colspan=4'><img src='data:image/png;base64," + base64String + "' alt='Barcode' height='50px' width='150px' /></td>";
                //htmlContent += "</tr>";

                htmlContent += "<tr>";
                htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; text-align: right;' colspan='" + (col - 1) + "'> Coupon Discount : </td>";
                htmlContent += "<td style = 'border: 1px solid #000; padding: 5px;' > " + TotalDiscount + " </td>";
                htmlContent += "</tr>";

                htmlContent += "<tr>";
                htmlContent += "<td style = 'border: 1px solid #000; padding: 5px; text-align: right;' colspan='" + (col - 1) + "'> Grand Total : </td>";
                htmlContent += "<td style = 'border: 1px solid #000; padding: 5px;' > " + res.InvoiceAmount + " </td>";
                htmlContent += "</tr>";

                htmlContent += "</tbody>";
            }
            htmlContent += "</table>";

            htmlContent += "</div>";

            return htmlContent;
        }

        public string ShippingHtmlContent(ShippingLabelDto res, int Count)
        {
            string htmlContent = "<div style = 'background-color: #FFFFFF; font-family: Arial, sans-serif;' >";

            #region Table 1 

            htmlContent += "<table width='100%' style='font-size: 11px; border-top: 1px solid #000; padding: 5px 0 5px 0;'>";
            htmlContent += "<tbody>";
            htmlContent += "<tr>";

            htmlContent += "<td style = 'width: 50%; padding: 10px 0 0 0;'> ";
            htmlContent += "<div style = 'margin-bottom: 10px; text-align: left;'>";
            htmlContent += "<img src = 'https://hashkartfrontend.hashtechy.space/_next/image?url=%2Fimages%2Flogo-hashkart.png&w=128&q=75' alt = 'Logo' style = 'max-width: 75px;' >";
            htmlContent += "</div>";
            htmlContent += "</td> ";

            htmlContent += "<td style = 'width: 50%; padding: 10px 0 0 0; font-size:11px;'> <b>Order No. : </b>" + res.OrderNo + "<br /> "
                                                                    + "<b>Order Date. : </b>" + Convert.ToDateTime(res.OrderDate).ToString("dd/MM/yyyy");
            htmlContent += "</td> ";

            htmlContent += "</tr> ";
            htmlContent += "</tbody> ";
            htmlContent += "</table> ";

            #endregion

            #region Table 2 

            htmlContent += "<table width='100%' style='font-size: 12px;'>";
            htmlContent += "<tbody>";
            htmlContent += "<tr>";

            htmlContent += "<td style = 'padding: 0px 0 10px 0px; border-bottom: 1px solid #000;'> <b>To. :</b> <br /> ";
            htmlContent += "<div style= 'font-size: 11px;'> ";
            htmlContent += !string.IsNullOrEmpty(res.DropContactPersonName) ? res.DropContactPersonName + "<br />" : "";
            htmlContent += !string.IsNullOrEmpty(res.DropAddressLine1) ? res.DropAddressLine1 + ", "
                            + (!string.IsNullOrEmpty(res.DropAddressLine2) ? res.DropAddressLine2 + ", " : "")
                            + (!string.IsNullOrEmpty(res.DropLandmark) ? res.DropLandmark + ", " : "")
                            + "<br /> " + (!string.IsNullOrEmpty(res.DropCity) ? res.DropCity + ", " : "")
                            + (!string.IsNullOrEmpty(res.DropState) ? res.DropState + ", " : "")
                            + (!string.IsNullOrEmpty(res.DropCountry) ? res.DropCountry + " - " : "")
                            + (res.DropPincode != null ? res.DropPincode : "") : "";
            htmlContent += !string.IsNullOrEmpty(res.DropTaxNo) ? "<br /> GST No :" + res.DropTaxNo : "";
            htmlContent += "</div> ";
            htmlContent += "</td> ";

            htmlContent += "<td style = 'padding: 0px 0 10px 0px; border-bottom: 1px solid #000;'> <b>From. :</b> <br /> ";
            htmlContent += "<div style= 'font-size: 11px;'> ";
            htmlContent += !string.IsNullOrEmpty(res.SellerLegalName) ? res.SellerLegalName + "<br />" : "";
            htmlContent += !string.IsNullOrEmpty(res.SellerPickupAddressLine1) ? res.SellerPickupAddressLine1 + ", "
                            + (!string.IsNullOrEmpty(res.SellerPickupAddressLine2) ? res.SellerPickupAddressLine2 + ", " : "")
                            + (!string.IsNullOrEmpty(res.SellerPickupLandmark) ? res.SellerPickupLandmark + ", " : "")
                            + "<br /> " + (!string.IsNullOrEmpty(res.SellerPickupCity) ? res.SellerPickupCity + ", " : "")
                            + (!string.IsNullOrEmpty(res.SellerPickupState) ? res.SellerPickupState + ", " : "")
                            + (!string.IsNullOrEmpty(res.SellerPickupCountry) ? res.SellerPickupCountry + " - " : "")
                            + (res.SellerPickupPincode != null ? res.SellerPickupPincode : "") : "";
            htmlContent += !string.IsNullOrEmpty(res.SellerPickupTaxNo) ? "<br /> GST No :" + res.SellerPickupTaxNo : "";
            htmlContent += "</div> ";
            htmlContent += "</td> ";

            htmlContent += "</tr> ";


            htmlContent += "<tr>";
            string paymentMode = res.PaymentMode.ToLower() != "cod" ? "PREPAID" : "COD";
            htmlContent += "<td style = 'width: 50%; padding: 10px 0 10px 0; font-size:11px; border-bottom: 1px solid #000;'> <b>Shipping By : </b> " + res.ShippingPartner + " <br /> "
                                                                + "<b>Courier Name : </b> " + res.CourierName + " <br /> "
                                                                + "<b>Shipping Date : </b> " + Convert.ToDateTime(res.ShippingDate).ToString("dd/MM/yyyy") + " <br /> "
                                                                + "<b>Weight : </b> " + res.Weight;
            // + "<b>No. of Items : </b> " + res.ShippingPartner;
            htmlContent += "</td> ";
            htmlContent += "<td style = 'width: 50%; padding: 10px 0 10px 0; font-size:11px; border-bottom: 1px solid #000;text-align: center;'> <label style = 'font-size: 18px; padding: 0px 0 15px 0; margin-bottom:10px; font-weight:bold;'>" + paymentMode + " </label><br /> "
                                                                + "<label style = 'font-size: 16px; padding: 10px 0 5px 0; margin-top:15px; font-weight:bold;'>" + res.InvoiceAmount + " </label>";
            htmlContent += "</td> ";
            htmlContent += "</tr> ";


            htmlContent += "</tbody> ";
            htmlContent += "</table> ";

            #endregion

            #region Table 3

            // Text to be encoded in the barcode
            string barcodeText = res.AwbNo;
            string base64String = GenerateBarcode(barcodeText);

            htmlContent += "<table width='100%' style='font-size: 11px;'>";
            htmlContent += "<tbody>";
            htmlContent += "<tr>";
            //Include the barcode image in the HTML content
            htmlContent += "<td style='padding: 10px 0 7px 0; text-align: center; font-size: 10px; width=100%;'><img src='data:image/png;base64," + base64String + "' alt='Barcode' width='75%' /></td>";
            htmlContent += "</tr>";
            htmlContent += "<tr>";
            htmlContent += "<td style='padding: 10px 0 7px 0; text-align: center; font-size: 12px;'><label style = 'font-size: 14px; margin-bottom:10px; font-weight:bold;'> Pack " + Count + " of " + res.NoOfPackage + " </label></td>";
            htmlContent += "</tr>";
            htmlContent += "</tbody> ";
            htmlContent += "</table> ";

            #endregion

            htmlContent += "</div>";

            return htmlContent;
        }

        private string GenerateBarcode(string barcodeText)
        {
           
            // Create a BarcodeWriter from BarcodeLib
            Barcode barcodeWriter = new Barcode();
            barcodeWriter.IncludeLabel = true;

            // Generate the barcode as a SKImage
            SKImage barcodeImage = barcodeWriter.Encode(BarcodeStandard.Type.Code128, barcodeText);

            // Convert SKImage to SKBitmap
            SKBitmap barcodeBitmap = SKBitmap.FromImage(barcodeImage);

            // Resize the barcode bitmap
            int desiredWidth = 150; // Set your desired width
            float aspectRatio = (float)barcodeImage.Width / barcodeImage.Height;
            int desiredHeight = (int)(desiredWidth / aspectRatio);
            SKBitmap resizedBarcodeBitmap = barcodeBitmap.Resize(new SKImageInfo(desiredWidth, desiredHeight), SKFilterQuality.High);

            // Convert the SKImage to SKData
            //SKData barcodeData = barcodeImage.Encode();
            SKData barcodeData = SKImage.FromBitmap(resizedBarcodeBitmap).Encode();

            // Convert SKData to byte array
            byte[] barcodeBytes = barcodeData.ToArray();

            // Convert byte array to base64 string
            string base64String = Convert.ToBase64String(barcodeBytes);

            return base64String;
        }


        public List<InvoiceDetails> GetInvoiceByOrderNo(string OrderNo)
        {
            List<InvoiceDetails> InvoiceDetailsDTO = new List<InvoiceDetails>();
            List<InvoiceDto> Invoice = new List<InvoiceDto>();
            BaseResponse<InvoiceDto> baseResponse = new BaseResponse<InvoiceDto>();

            var response = helper.ApiCall(OrderURL, EndPoints.Orders + "/Invoice?OrderNo=" + OrderNo, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            if (baseResponse.code == 200)
            {
                Invoice = (List<InvoiceDto>)baseResponse.Data;

                if (Invoice != null && Invoice.Count > 0)
                {
                    
                    BaseResponse<TaxTypeValueLibrary> taxValuebaseResponse = new BaseResponse<TaxTypeValueLibrary>();
                    List<TaxTypeValueLibrary> taxlst = new List<TaxTypeValueLibrary>();
                    var Taxresponse = helper.ApiCall(CatelogueURL, EndPoints.TaxTypeValue + "?PageIndex=0&PageSize=0", "GET", null);
                    taxValuebaseResponse = taxValuebaseResponse.JsonParseList(Taxresponse);
                    if (taxValuebaseResponse.code == 200)
                    {
                        taxlst = (List<TaxTypeValueLibrary>)taxValuebaseResponse.Data;
                    }

                    var lstInvoice = Invoice.DistinctBy(p => p.Id).ToList();
                    foreach (var x in lstInvoice)
                    {
                        InvoiceDetails invoicedataDTO = new InvoiceDetails();
                        List<InvoiceDto> invdata = Invoice.Where(p => p.Id == x.Id).ToList();
                        InvoiceItemDetails InvoiceProduct = BindProduct(invdata, taxlst);
                        invoicedataDTO = invdata.Select(x => new InvoiceDetails
                        {
                            Id = Convert.ToInt32(x.Id),
                            OrderId = Convert.ToInt32(x.OrderID),
                            OrderNo = x.OrderNo,
                            OrderItemIDs = x.OrderItemIDs,
                            OrderDate = Convert.ToDateTime(x.OrderDate),
                            InvoiceNo = x.InvoiceNo,
                            InvoiceDate = Convert.ToDateTime(x.InvoiceDate),

                            SellerTradeName = x.SellerTradeName,
                            SellerLegalName = x.SellerLegalName,
                            RegisteredGSTNo = x.SellerGSTNo,
                            RegisteredAddressLine1 = x.SellerRegisteredAddressLine1,
                            RegisteredAddressLine2 = x.SellerRegisteredAddressLine2,
                            RegisteredLendmark = x.SellerRegisteredLandmark,
                            RegisteredPincode = x.SellerRegisteredPincode,
                            RegisteredCity = x.SellerRegisteredCity,
                            RegisteredState = x.SellerRegisteredState,
                            RegisteredCountry = x.SellerRegisteredCountry,

                            SellerPickupAddressLine1 = x.SellerPickupAddressLine1,
                            SellerPickupAddressLine2 = x.SellerPickupAddressLine2,
                            SellerPickupLandmark = x.SellerPickupLandmark,
                            SellerPickupPincode = x.SellerPickupPincode,
                            SellerPickupCity = x.SellerPickupCity,
                            SellerPickupState = x.SellerPickupState,
                            SellerPickupCountry = x.SellerPickupCountry,
                            SellerPickupTaxNo = x.SellerPickupTaxNo,

                            UserName = x.DropContactPersonName,
                            BillToMobileNo = x.DropContactPersonMobileNo,
                            BillToEmailID = x.DropContactPersonEmailID,
                            BillToAddressLine1 = x.DropAddressLine1,
                            BillToAddressLine2 = x.DropAddressLine2,
                            BillToLendmark = x.DropLandmark,
                            BillToPincode = Convert.ToInt32(x.DropPincode).ToString(),
                            BillToGSTNo = null,
                            BillToCity = x.DropCity,
                            BillToState = x.DropState,
                            BillToCountry = x.DropCountry,
                            BillToTaxNo = x.DropTaxNo,

                            ShipToMobileNo = x.DropContactPersonMobileNo,
                            ShipToAddressLine1 = x.DropAddressLine1,
                            ShipToAddressLine2 = x.DropAddressLine2,
                            ShipToLendmark = x.DropLandmark,
                            ShipToPincode = Convert.ToInt32(x.DropPincode).ToString(),
                            ShipToGSTNo = null,
                            ShipToCity = x.DropCity,
                            ShipToState = x.DropState,
                            ShipToCountry = x.DropCountry,
                            ShipToTaxNo = x.DropTaxNo,

                            ExtraCharges = Extracharges(Convert.ToDecimal(Convert.ToDecimal(x.TotalExtracharges).ToString("N0")), InvoiceProduct.ProductItems.FirstOrDefault().Taxes, taxlst),
                            TotalExtraCharges = Convert.ToDecimal(Convert.ToDecimal(x.TotalExtracharges).ToString("N0")),

                            CodCharges = Cod(Convert.ToDecimal(x.CODCharge), InvoiceProduct.ProductItems.FirstOrDefault().Taxes, taxlst),
                            TotalCodCharges = Convert.ToDecimal(x.CODCharge),

                            ShippingCharges = ShippingCharges(Convert.ToDecimal(Invoice.Sum(p => Convert.ToDecimal(p.ShippingCharge))), InvoiceProduct.ProductItems.FirstOrDefault().Taxes, taxlst),
                            TotalShippingCharges = Convert.ToDecimal(Invoice.Sum(p => Convert.ToDecimal(p.ShippingCharge))),


                            PaymentMode = x.PaymentMode,
                            IsCouponApplied = x.IsCouponApplied,
                            Coupon = x.Coupon,
                            CoupontDiscount = x.CoupontDiscount,
                            CoupontDetails = x.CoupontDetails,
                            ItemDetails = InvoiceProduct,
                            InvoiceAmount = Convert.ToDecimal(x.InvoiceAmount) + Convert.ToDecimal(x.CODCharge),

                        }).FirstOrDefault();

                        InvoiceDetailsDTO.Add(invoicedataDTO);
                    }
                }

                
            }

            return InvoiceDetailsDTO;

        }
    }
}
