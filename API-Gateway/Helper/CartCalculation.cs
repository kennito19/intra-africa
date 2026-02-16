using API_Gateway.Common.cart;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace API_Gateway.Helper
{
    public class CartCalculation
    {
        private readonly HttpContext _httpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public static string Catalougeurl = string.Empty;
        public static string IdentityUrl = string.Empty;
        public static string Userurl = string.Empty;
        public static string OrderUrl = string.Empty;
        private readonly IConfiguration _configuration;
        private readonly GetCartDetails getCart;
        private ApiHelper helper;

        public CartCalculation(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            Catalougeurl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IdentityUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            OrderUrl = _configuration.GetSection("ApiURLs").GetSection("OrderAPI").Value;
            Userurl = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            getCart = new GetCartDetails(_configuration, _httpContext);
            helper = new ApiHelper(_httpContext);
        }

        public JObject GetCartCalculation(CartCalculationDTO cartCalculation)
        {
            BaseResponse<Cart> cartResponse = new BaseResponse<Cart>();
            List<Cart> lstCart = new List<Cart>();

            string url = "";
            if (cartCalculation.CartId != 0 && cartCalculation.CartId != null)
            {
                url += "&id=" + cartCalculation.CartId;
            }

            if (!string.IsNullOrEmpty(cartCalculation.UserId))
            {
                url += "&userId=" + cartCalculation.UserId;
            }

            var response = helper.ApiCall(Userurl, EndPoints.Cart + "?sessionId=" + cartCalculation.CartSessionId + url, "GET", null);
            cartResponse = cartResponse.JsonParseList(response);
            string jsonString = JsonConvert.SerializeObject(cartResponse.Data, Formatting.None);

            JObject products = new JObject();
            JObject Sellerproducts = new JObject();

            JArray SellerproductsArray = new JArray();

            decimal totalMrp = 0;
            decimal totalSellingPrice = 0;
            decimal totalDiscount = 0;
            decimal totalGst = 0;
            decimal totalSaveamt = 0;
            decimal totalExtradiscount = 0;
            decimal totalShipping = 0;
            decimal totalActualShipping = 0;
            decimal totalExtracharges = 0;
            decimal subTotal = 0;
            decimal totalAmount = 0;
            decimal TempCartValue = 0;
            string CouponApplyOn = null;
            decimal cod = 0;

            checkOfferDto getoffers = new checkOfferDto();

            getChekoutCalculation getcartCalculation = new getChekoutCalculation();
            getcartCalculation.CartJson = jsonString;

            List<GetCheckOutDetailsList> lstCartDetails = getCartDetailslst(getcartCalculation);

            List<string> sellerid = lstCartDetails.GroupBy(p => p.SellerId).Select(group => group.Key).ToList();
            int count = 0;

            foreach (var seller in sellerid)
            {
                JObject sellerObject = new JObject();
                JObject productItems = new JObject();
                JObject SellerItems = new JObject();
                JArray Itemarray = new JArray();
                JArray SellerItemarray = new JArray();

                decimal Seller_totalMrp = 0;
                decimal Seller_totalSellingPrice = 0;
                decimal Seller_totalDiscount = 0;
                decimal Seller_totalGst = 0;
                decimal Seller_totalSaveamt = 0;
                decimal Seller_totalExtradiscount = 0;
                decimal Seller_totalExtraCharges = 0;
                decimal Seller_totalShipping = 0;
                decimal Seller_subTotal = 0;
                decimal Seller_totalAmount = 0;

                List<GetCheckOutDetailsList> _lstSellerCartDetails = lstCartDetails.Where(p => p.SellerId == seller).ToList();

                foreach (var sellerItem in _lstSellerCartDetails)
                {
                    int SizeId = 0;
                    decimal TotalProductQty = 0;

                    decimal itemMrp = Convert.ToDecimal(sellerItem.ItemMRP);
                    decimal itemSellprice = Convert.ToDecimal(sellerItem.ItemSellingPrice);
                    decimal itemDiscount = Convert.ToDecimal(sellerItem.ItemDiscount);
                    decimal itemQty = Convert.ToInt32(sellerItem.Quantity);
                    decimal itemExtracharges = 0;
                    decimal totalItemExtracharges = 0;

                    decimal itemShipping = 0;

                    // decimal tempItemMrp = Convert.ToDecimal(sellerItem.TempMRP);
                    decimal tempItemSellprice = Convert.ToDecimal(sellerItem.TempSellingPrice);
                    //decimal tempItemDiscount = Convert.ToDecimal(sellerItem.TempDiscount);

                    decimal totalItemMrp = 0;
                    decimal totalItemSellprice = 0;
                    decimal totalItemDiscount = 0;

                    string result = GetGstDetail(sellerItem.SellerId, cartCalculation.Pincode);

                    var extraDetails = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(sellerItem.ExtraDetails);
                    var sellerStatus = extraDetails["SellerDetails"]["SellerStatus"].Value<string>().ToLower();
                    var assignBrandStatus = extraDetails["BrandDetails"]["AssignBrandStatus"].Value<string>().ToLower();
                    var brandStatus = extraDetails["BrandDetails"]["BrandStatus"].Value<string>().ToLower();

                    bool IsProductAvailable = (sellerItem.ProductStatus.ToLower() == "active" && sellerItem.LiveStatus == true && sellerStatus.ToLower() == "active" && assignBrandStatus.ToLower() == "active" && brandStatus.ToLower() == "active") ? true : false;

                    BindProducts bindProducts = new BindProducts();
                    bindProducts = bindProductJson(sellerItem);
                    productItems = bindProducts.productItems;

                    JObject price = new JObject();
                    price["mrp"] = itemMrp;
                    price["selling_price"] = itemSellprice;
                    price["old_selling_price"] = tempItemSellprice;
                    price["discount"] = itemDiscount;

                    if (IsProductAvailable)
                    {
                        TotalProductQty = Convert.ToInt32(sellerItem.TotalQty);

                        if (sellerItem.SizeId != null && sellerItem.SizeId != 0)
                        {
                            SizeId = Convert.ToInt32(sellerItem.SizeId);
                        }

                        List<CategoryLibrary> _categoryLibrary = JsonConvert.DeserializeObject<List<CategoryLibrary>>(sellerItem.ParentCategoryList);
                        if (_categoryLibrary.Count == _categoryLibrary.Where(p => p.Status.ToLower() == "active").Count())
                        {
                            if (itemQty != 0 && itemQty <= TotalProductQty)
                            {
                                totalItemMrp = (Convert.ToDecimal(itemMrp) * itemQty);
                                totalItemSellprice = (Convert.ToDecimal(itemSellprice) * itemQty);
                                totalItemDiscount = totalItemMrp - totalItemSellprice;

                                totalMrp = totalMrp + totalItemMrp;
                                totalSellingPrice = totalSellingPrice + totalItemSellprice;
                                totalDiscount = totalDiscount + totalItemDiscount;

                                Seller_totalMrp = Seller_totalMrp + totalItemMrp;
                                Seller_totalSellingPrice = Seller_totalSellingPrice + totalItemSellprice;
                                Seller_totalDiscount = Seller_totalDiscount + totalItemDiscount;

                                #region Count Extra Discount

                                if (!string.IsNullOrEmpty(cartCalculation.CouponCode))
                                {
                                    checkOfferDto checkoffers = new checkOfferDto();
                                    checkoffers = getCart.getOffers(cartCalculation.CouponCode, Convert.ToInt32(sellerItem.ProductId), Convert.ToInt32(sellerItem.BrandId), Convert.ToInt32(sellerItem.Categoryid), sellerItem.SellerId, cartCalculation.UserId, cartCalculation.PaymentMode, Catalougeurl, OrderUrl);

                                    productItems["coupon_status"] = checkoffers.CouponApplied ? "success" : "failed";
                                    productItems["coupon_message"] = checkoffers.message;
                                    productItems["coupon_amount"] = 0;

                                    if (checkoffers != null && checkoffers.CouponApplied)
                                    {
                                        productItems["coupon_details"] = checkoffers.offers.offerType;
                                        if (getoffers.offers == null)
                                        {
                                            getoffers = checkoffers;
                                        }

                                        CouponApplyOn = checkoffers.offers.applyOn.ToString();
                                        TempCartValue = TempCartValue + totalItemSellprice;
                                    }
                                }
                                else
                                {
                                    productItems["coupon_status"] = null;
                                    productItems["coupon_message"] = null;
                                    productItems["coupon_details"] = null;
                                    productItems["coupon_amount"] = 0;
                                }

                                #endregion


                                #region Item and cart Calculation

                                string taxrate = sellerItem.TaxRate.ToString();

                                var settings = new JsonSerializerSettings
                                {
                                    Converters = { new CaseInsensitiveDictionaryConverter() }
                                };

                                var Taxvalue = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(taxrate, settings);

                                #region Shipping

                                SellerKycList sellerkyc = getCart.getsellerKyc(sellerItem.SellerId);

                                #endregion Shipping

                                #region Display Price

                                bool setMarginProductLevel = Convert.ToBoolean(_configuration.GetValue<string>("allow_set_margin_on_product_level"));

                                DisplayPriceCalculation displayPriceCalculation = new DisplayPriceCalculation();
                                displayPriceCalculation.mrp = itemMrp;
                                displayPriceCalculation.sellingprice = itemSellprice;
                                displayPriceCalculation.weightSlabId = Convert.ToInt32(sellerItem.WeightSlabId);
                                displayPriceCalculation.categoryId = Convert.ToInt32(sellerItem.Categoryid);
                                displayPriceCalculation.BrandId = Convert.ToInt32(sellerItem.BrandId);
                                displayPriceCalculation.sellerId = sellerItem.SellerId;

                                if (setMarginProductLevel)
                                {
                                    displayPriceCalculation.CommissionChargesIn = Convert.ToString(sellerItem.MarginIn);
                                    displayPriceCalculation.CommissionCharges = Convert.ToString(sellerItem.MarginCost);
                                    displayPriceCalculation.CommissionRate = Convert.ToString(sellerItem.MarginPercentage);
                                }

                                displayPriceCalculation.shipmentBy = sellerkyc.ShipmentBy;
                                displayPriceCalculation.shippingPaidBy = sellerkyc.ShipmentChargesPaidByName;

                                JObject itemdata = new JObject();
                                itemdata["productID"] = sellerItem.ProductId;
                                itemdata["productGuid"] = sellerItem.ProductGuid;
                                itemdata["sellerProductID"] = sellerItem.SellerProductId;
                                itemdata["orderTaxRateId"] = sellerItem.TaxValueId;
                                itemdata["orderTaxRate"] = taxrate;
                                itemdata["hsnCode"] = sellerItem.HSNCode;
                                itemdata["weightSalb"] = sellerItem.WeightSlab;

                                JObject ProducttaxInfo = GetProducttaxInfo(displayPriceCalculation, itemdata, Convert.ToInt32(itemQty), result);

                                decimal gstval = Convert.ToDecimal(totalItemSellprice) * Convert.ToInt32(Taxvalue.Select(a => a.Value).Max()) / (100 + Convert.ToInt32(Taxvalue.Select(a => a.Value).Max()));
                                totalGst = totalGst + gstval;
                                Seller_totalGst = Seller_totalGst + gstval;
                                
                                if (ProducttaxInfo["shippingPaidBy"].ToString().ToLower() == "customer")
                                {
                                    itemShipping = Convert.ToDecimal(ProducttaxInfo["actualShippingCharges"].ToString());
                                    totalShipping = totalShipping + Convert.ToDecimal(Convert.ToDecimal(itemShipping.ToString("N2")).ToString("N0"));
                                    Seller_totalShipping = Seller_totalShipping + Convert.ToDecimal(Convert.ToDecimal(itemShipping.ToString("N2")).ToString("N0"));
                                }

                                if (ProducttaxInfo["otherCharges"] != null && ProducttaxInfo["otherCharges"].Type == JTokenType.Object)
                                {
                                    JObject localObject = (JObject)ProducttaxInfo["otherCharges"];
                                    foreach (KeyValuePair<string, JToken> items in localObject)
                                    {
                                        JObject localItem = (JObject)items.Value;
                                        if (localItem["charges_paid_by"] != null && (string)localItem["charges_paid_by"].ToString().ToLower() == "customer")
                                        {
                                            itemExtracharges += Convert.ToDecimal(Convert.ToDecimal(localItem["actual_charges"]).ToString("N2"));
                                            localItem["total_charges"] = Convert.ToDecimal(Convert.ToDecimal(Convert.ToDecimal(localItem["actual_charges"]) * itemQty).ToString("N2"));
                                        }
                                        else
                                        {
                                            localItem["total_charges"] = 0;
                                        }

                                        localItem["actual_charges"] = Convert.ToDecimal(localItem["actual_charges"]) * itemQty;
                                    }

                                }

                                #endregion

                                totalItemExtracharges = itemExtracharges * itemQty;

                                totalExtracharges = totalExtracharges + Convert.ToDecimal(Convert.ToDecimal(totalItemExtracharges.ToString("N2")).ToString("N0"));
                                Seller_totalExtraCharges = Seller_totalExtraCharges + Convert.ToDecimal(Convert.ToDecimal(totalItemExtracharges.ToString("N2")).ToString("N0"));


                                totalSaveamt = totalSaveamt + totalItemDiscount;
                                Seller_totalSaveamt = Seller_totalSaveamt + totalItemDiscount;

                                decimal _totalamt = totalItemSellprice;
                                totalAmount = totalAmount + _totalamt;
                                Seller_totalAmount = Seller_totalAmount + _totalamt;

                                decimal _subtotal = totalItemSellprice + totalItemExtracharges + itemShipping;
                                subTotal = subTotal + Convert.ToDecimal(Convert.ToDecimal(_subtotal.ToString("N2")).ToString("N0"));
                                Seller_subTotal = Seller_subTotal + Convert.ToDecimal(Convert.ToDecimal(_subtotal.ToString("N2")).ToString("N0"));

                                #endregion

                                productItems["status"] = "In stock";
                                productItems["gstIncluding"] = gstval.ToString("N2");
                                productItems["itemPrice"] = price;
                                productItems["taxinfo"] = ProducttaxInfo;
                                productItems["ItemShippingCharges"] = Convert.ToDecimal(itemShipping.ToString("N2")).ToString("N0");
                                productItems["ItemShippingPaidBy"] = ProducttaxInfo["shippingPaidBy"].ToString();
                                productItems["ItemShippingZone"] = result;
                                productItems["ItemExtracharges"] = totalItemExtracharges;
                                productItems["ItemSubTotal"] = Convert.ToDecimal(_subtotal.ToString("N2")).ToString("N0");
                                productItems["ItemTotal"] = Convert.ToDecimal(_totalamt.ToString("N2")).ToString("N0");


                            }
                            else if (TotalProductQty != 0 && itemQty >= TotalProductQty)
                            {
                                productItems["status"] = "Maximum quantity reached";
                                productItems["coupon_status"] = null;
                                productItems["coupon_message"] = null;
                                productItems["coupon_details"] = null;
                                productItems["coupon_amount"] = 0;
                            }
                            else
                            {
                                productItems["status"] = "Out of stock";
                                productItems["coupon_status"] = null;
                                productItems["coupon_message"] = null;
                                productItems["coupon_details"] = null;
                                productItems["coupon_amount"] = 0;
                            }
                        }

                    }
                    else
                    {


                        productItems["itemPrice"] = price;
                        productItems["status"] = "Seller Product is not available.";
                        productItems["coupon_status"] = null;
                        productItems["coupon_message"] = null;
                        productItems["coupon_details"] = null;
                        productItems["coupon_amount"] = 0;

                    }

                    Itemarray.Add(productItems);
                }

                SellerItems["Items"] = Itemarray;


                JObject SellerCartAmount = new JObject();

                SellerCartAmount["total_mrp"] = Seller_totalMrp;
                SellerCartAmount["total_selling_price"] = Seller_totalSellingPrice;
                SellerCartAmount["total_discount"] = Convert.ToDecimal(Seller_totalDiscount.ToString("N2")).ToString("N0");
                SellerCartAmount["total_SaveAmt"] = Convert.ToDecimal(Seller_totalSaveamt.ToString("N2")).ToString("N0");
                SellerCartAmount["total_extra_charges"] = Convert.ToDecimal(Seller_totalExtraCharges.ToString("N2")).ToString("N0");
                SellerCartAmount["total_inclusivegst"] = Seller_totalGst.ToString("N2");
                SellerCartAmount["shipping_charges"] = Seller_totalShipping;
                SellerCartAmount["paid_amount"] = Convert.ToDecimal(Seller_subTotal.ToString("N2")).ToString("N0");

                SellerCartAmount["total_amount"] = Convert.ToDecimal(Seller_totalAmount.ToString("N2")).ToString("N0"); 

                SellerItems["SellerCartAmount"] = SellerCartAmount;
                SellerItemarray.Add(SellerItems);


                Sellerproducts[count.ToString()] = SellerItemarray;
                //SellerproductsArray.Add(Sellerproducts);
                count++;
            }

            totalActualShipping = totalShipping;
            CouponDiscount coupon = new CouponDiscount();
            coupon = SetCouponDiscount(Sellerproducts, getoffers, TempCartValue, totalExtradiscount, subTotal, totalSaveamt, totalShipping, totalSellingPrice);
            Sellerproducts = coupon.ProductItems;
            subTotal = coupon.Subtotal;
            totalSaveamt = coupon.SaveAmount;
            totalExtradiscount = coupon.ExtraDiscount;
            totalShipping = coupon.Shipping;
            totalActualShipping = coupon.ActualShipping;

            products["items"] = Sellerproducts;
            #region Set Cart Total

            JObject CartAmount = new JObject();
            CartAmount["total_mrp"] = totalMrp;
            CartAmount["total_selling_price"] = totalSellingPrice;
            CartAmount["total_discount"] = Convert.ToDecimal(totalDiscount.ToString("N2")).ToString("N0");
            CartAmount["total_SaveAmt"] = Convert.ToDecimal(totalSaveamt.ToString("N2")).ToString("N0");
            CartAmount["total_extra_charges"] = Convert.ToDecimal(totalExtracharges.ToString("N2")).ToString("N0");
            CartAmount["total_inclusivegst"] = totalGst.ToString("N2");
            if (totalExtradiscount > 0)
            {
                CartAmount["total_extradiscount"] = totalExtradiscount;
            }
            CartAmount["shipping_charges"] = totalShipping;
            CartAmount["actual_shipping_charges"] = totalActualShipping;
            CartAmount["total_amount"] = Convert.ToDecimal(totalAmount.ToString("N2")).ToString("N0");
            if (cartCalculation.PaymentMode.ToLower() == "cod")
            {
                JObject codobj = getCart.getCodCharge(Catalougeurl, subTotal.ToString());
                if (Convert.ToBoolean(codobj["codAvailable"]))
                {
                    if (!Convert.ToBoolean(codobj["hasFree"]))
                    {
                        if (Convert.ToBoolean(codobj["free_cod_on_mov"]))
                        {
                            cod = Convert.ToDecimal(codobj["codCharges"]);
                            CartAmount["cod_charges"] = cod;
                            CartAmount["cod_message"] = codobj["message"];
                            subTotal = subTotal + cod;
                        }
                        else
                        {
                            cod = Convert.ToDecimal(codobj["codCharges"]);
                            CartAmount["cod_charges"] = cod;
                            CartAmount["cod_message"] = codobj["message"];
                            subTotal = subTotal + cod;
                        }
                    }
                    else
                    {
                        cod = Convert.ToDecimal(codobj["codCharges"]);
                        CartAmount["cod_charges"] = cod;
                        CartAmount["cod_message"] = codobj["message"];
                        subTotal = subTotal + cod;
                    }
                }
                else
                {
                    cod = Convert.ToDecimal(codobj["codCharges"]);
                    subTotal = subTotal + cod;
                }

                for (int i = 0; i < Sellerproducts.Count; i++)
                {
                    decimal tempCod = 0;
                    tempCod = Convert.ToDecimal(Convert.ToDecimal(cod / Sellerproducts.Count).ToString("N2"));
                    foreach (var item1 in Sellerproducts[i.ToString()])
                    {
                        decimal Seller_subTotal = 0;
                        JObject selleritemPrice = (JObject)item1["SellerCartAmount"];
                        Seller_subTotal = Convert.ToDecimal(selleritemPrice["paid_amount"]);
                        Seller_subTotal = Seller_subTotal + tempCod;
                        selleritemPrice["cod_charges"] = tempCod;
                        selleritemPrice["cod_message"] = codobj["message"];
                        selleritemPrice["paid_amount"] = Seller_subTotal;
                    }
                }

            }
            CartAmount["paid_amount"] = subTotal.ToString("N0");
            
            products["items"] = Sellerproducts;
            products["CartAmount"] = CartAmount;

            #endregion Set Cart Total
            return products;

        }

        public List<GetCheckOutDetailsList> getCartDetailslst(getChekoutCalculation calculation)
        {
            BaseResponse<GetCheckOutDetailsList> cartResponse = new BaseResponse<GetCheckOutDetailsList>();
            List<GetCheckOutDetailsList> lstCart = new List<GetCheckOutDetailsList>();
            var response = helper.ApiCall(Catalougeurl, EndPoints.CartGetCartList, "POST", calculation);
            cartResponse = cartResponse.JsonParseList(response);

            if (cartResponse.code == 200)
            {
                lstCart = (List<GetCheckOutDetailsList>)cartResponse.Data;
            }
            return lstCart;
        }

        public BindProducts bindProductJson(GetCheckOutDetailsList sellerProduct)
        {
            JObject productItems = new JObject();
            productItems["productName"] = sellerProduct.ProductName;
            productItems["ProductGuid"] = sellerProduct.ProductGuid;
            productItems["productID"] = sellerProduct.ProductId;
            productItems["categoryId"] = sellerProduct.Categoryid;
            productItems["sellerProductID"] = sellerProduct.SellerProductId;
            productItems["sellerId"] = sellerProduct.SellerId;
            productItems["brandid"] = sellerProduct.BrandId;
            productItems["sellerName"] = !string.IsNullOrEmpty(sellerProduct.ExtraDetails) ? JObject.Parse(sellerProduct.ExtraDetails)["SellerDetails"]?["Display"]?.ToString() ?? JObject.Parse(sellerProduct.ExtraDetails)["SellerDetails"]?["FullName"]?.ToString() ?? "Unknown Seller" : null;
            productItems["brandName"] = !string.IsNullOrEmpty(sellerProduct.ExtraDetails) ? JObject.Parse(sellerProduct.ExtraDetails)["BrandDetails"]?["Name"]?.ToString() ?? sellerProduct.ExtraDetails : null;
            productItems["productSKU"] = sellerProduct.ProductSkuCode;
            productItems["sellerSKU"] = sellerProduct.SellerSkuCode;
            productItems["size"] = sellerProduct.Size;
            productItems["sizeId"] = sellerProduct.SizeId;
            productItems["cartId"] = sellerProduct.Id;
            productItems["color"] = sellerProduct.Color;
            productItems["Image"] = sellerProduct.Image;
            productItems["qty"] = sellerProduct.Quantity;
            productItems["Extradetails"] = sellerProduct.ExtraDetails;

            BindProducts bindProducts = new BindProducts();
            bindProducts.productItems = productItems;
            return bindProducts;
        }

        public JObject GetProducttaxInfo(DisplayPriceCalculation displaypriceCalculation, JObject itemdata, int qty, string result = "Local")
        {
            JObject OraderItemTaxInfo = new JObject();
            JObject ItemTaxInfo = new JObject();


            PriceCalculation priceCalculation = new PriceCalculation();
            bool shippingOnCategory = Convert.ToBoolean(_configuration.GetValue<string>("shipping_charges_on_Category"));
            ItemTaxInfo = priceCalculation.displayCalculation(displaypriceCalculation, _httpContextAccessor, Catalougeurl, true, shippingOnCategory);
            dynamic jsonObject = JsonConvert.DeserializeObject(ItemTaxInfo.ToString());

            decimal net_earn = 0;

            net_earn = result.ToLower() == "local" ? Convert.ToDecimal(jsonObject.customerPricing[0].columns.final_you_earn.values[0]) :
                       result.ToLower() == "zonal" ? Convert.ToDecimal(jsonObject.customerPricing[0].columns.final_you_earn.values[1]) :
                       result.ToLower() == "national" ? Convert.ToDecimal(jsonObject.customerPricing[0].columns.final_you_earn.values[2]) : 0;

            decimal actualShippingCharges = 0;

            actualShippingCharges = result.ToLower() == "local" ? Convert.ToDecimal(jsonObject.customerPricing[0].columns.shipping_charges.actualShippingCharges[0]) :
                       result.ToLower() == "zonal" ? Convert.ToDecimal(jsonObject.customerPricing[0].columns.shipping_charges.actualShippingCharges[1]) :
                       result.ToLower() == "national" ? Convert.ToDecimal(jsonObject.customerPricing[0].columns.shipping_charges.actualShippingCharges[2]) : 0;


            OraderItemTaxInfo["productID"] = itemdata["productID"];
            OraderItemTaxInfo["sellerProductID"] = itemdata["sellerProductID"];
            OraderItemTaxInfo["netEarn"] = net_earn * qty;
            OraderItemTaxInfo["orderTaxRateId"] = itemdata["orderTaxRateId"];
            OraderItemTaxInfo["orderTaxRate"] = itemdata["orderTaxRate"];
            OraderItemTaxInfo["hsnCode"] = itemdata["hsnCode"];
            OraderItemTaxInfo["weightSalb"] = itemdata["weightSalb"];
            OraderItemTaxInfo["commissionIn"] = jsonObject.customerPricing[0].columns.platform_fees.commissionIn;
            OraderItemTaxInfo["commissionRate"] = jsonObject.customerPricing[0].columns.platform_fees.commissionRate;
            OraderItemTaxInfo["commissionAmount"] = Convert.ToDecimal(jsonObject.customerPricing[0].columns.platform_fees.commissionvalues[0]) * qty;

            OraderItemTaxInfo["otherCharges"] = jsonObject.customerPricing[0].columns.other_charges;

            OraderItemTaxInfo["shipmentBy"] = jsonObject.customerPricing[0].columns.shipping_charges.shipmentBy;
            OraderItemTaxInfo["actualShippingCharges"] = actualShippingCharges;
            OraderItemTaxInfo["shippingPaidBy"] = jsonObject.customerPricing[0].columns.shipping_charges.shippingPaidBy;

            return OraderItemTaxInfo;
        }

        public CouponDiscount SetCouponDiscount(JObject Itemarray, checkOfferDto getoffers, decimal TempCartValue, decimal finaldiscount, decimal finalsubTotal, decimal finalsaveamt, decimal finalshipping, decimal totalSellingPrice)
        {
            CouponDiscount coupon = new CouponDiscount();
            decimal Extradiscount = finaldiscount;
            decimal subTotal = finalsubTotal;
            decimal saveamt = finalsaveamt;
            decimal shipping = finalshipping;
            decimal NewTotalSellPrice = 0;
            bool shippingfree = false;

            if (getoffers != null && getoffers.CouponApplied)
            {
                decimal _extradiscount = 0;
                decimal _Totalextradiscount = 0;

                //foreach (JObject item2 in Itemarray)
                //{
                for (int i = 0; i < Itemarray.Count; i++)
                {
                    foreach (var item1 in Itemarray[i.ToString()])
                    {
                        decimal Seller_Extradiscount = 0;
                        decimal Seller_subTotal = 0;
                        decimal Seller_saveamt = 0;
                        decimal Seller_shipping = 0;
                        decimal ActualSeller_shipping = 0;

                        JObject selleritemPrice = (JObject)item1["SellerCartAmount"];
                        Seller_subTotal = Convert.ToDecimal(selleritemPrice["paid_amount"]);
                        Seller_saveamt = Convert.ToDecimal(selleritemPrice["total_SaveAmt"]);
                        Seller_shipping = Convert.ToDecimal(selleritemPrice["shipping_charges"]);
                        ActualSeller_shipping = Convert.ToDecimal(selleritemPrice["shipping_charges"]);

                        var _itemsArray = (JArray)item1["Items"];

                        var filteredItems = new JArray(_itemsArray.Where(item => item["coupon_status"] != null && item["coupon_status"].ToString().ToLower() == "success")).ToList();

                        if (getoffers.offers.offerType.ToLower() == "flat discount")
                        {
                            if (TempCartValue >= Convert.ToDecimal(getoffers.offers.minimumOrderValue))
                            {
                                _Totalextradiscount = Convert.ToDecimal(getoffers.offers.value);
                            }
                        }
                        else if (getoffers.offers.offerType == "percentage discount")
                        {
                            decimal tempprice = Convert.ToDecimal(Convert.ToDecimal((TempCartValue) * (Convert.ToDecimal(getoffers.offers.value) / 100)).ToString("N0"));
                            if (TempCartValue >= Convert.ToDecimal(getoffers.offers.minimumOrderValue))
                            {
                                if (tempprice > Convert.ToDecimal(getoffers.offers.maximumDiscountAmount))
                                {
                                    _Totalextradiscount = Convert.ToDecimal(getoffers.offers.maximumDiscountAmount);
                                }
                                else
                                {
                                    _Totalextradiscount = tempprice;
                                }
                            }
                        }
                        else if (getoffers.offers.offerType == "free shipping")
                        {
                            if (TempCartValue >= Convert.ToDecimal(getoffers.offers.minimumOrderValue))
                            {
                                shippingfree = true;
                            }
                        }

                        int Count = filteredItems.Count();
                        if (Count > 0)
                        {
                            foreach (JObject item in item1["Items"])
                            {
                                if (item["status"].ToString().ToLower() == "in stock")
                                {

                                    JObject itemPrice = (JObject)item["itemPrice"];
                                    decimal _itemshipping = Convert.ToDecimal(item["ItemShippingCharges"].ToString());
                                    decimal _ItemsellPrice = Convert.ToDecimal(itemPrice["selling_price"]) * Convert.ToInt32(item["qty"]);

                                    if (item["coupon_status"].ToString().ToLower() == "success")
                                    {
                                        decimal _sellPrice = Convert.ToDecimal(itemPrice["selling_price"]) * Convert.ToInt32(item["qty"]);
                                        decimal _discountAmount = 0;
                                        decimal oldship = 0;
                                        decimal _shipping = Convert.ToDecimal(item["ItemShippingCharges"].ToString());

                                        if (getoffers.offers.offerType == "free shipping")
                                        {
                                            if (shippingfree)
                                            {
                                                _shipping = 0;
                                                oldship = Convert.ToDecimal(item["ItemShippingCharges"].ToString());
                                                shipping = shipping - oldship;
                                                _discountAmount = oldship;
                                            }
                                            else
                                            {
                                                item["coupon_status"] = "failed";
                                                item["coupon_message"] = "Invalid coupon code.";
                                                item["coupon_amount"] = 0;

                                            }
                                        }
                                        else
                                        {
                                            decimal ratio = _sellPrice / TempCartValue;
                                            _discountAmount = ratio * _Totalextradiscount;

                                            _sellPrice = _sellPrice - _discountAmount;
                                            if (_sellPrice < 0)
                                            {
                                                _sellPrice = 0;
                                                _discountAmount = Convert.ToDecimal(itemPrice["selling_price"]) * Convert.ToInt32(item["qty"]);
                                            }
                                        }

                                        Extradiscount = Extradiscount + _discountAmount;
                                        Seller_Extradiscount = Seller_Extradiscount + _discountAmount;

                                        NewTotalSellPrice = NewTotalSellPrice + _sellPrice;
                                        decimal _subtotal = _sellPrice + _shipping;
                                        //decimal _oldItemsubtotal = Convert.ToDecimal(item["ItemSubTotal"]) - _ItemsellPrice - _itemshipping;
                                        subTotal = subTotal - _ItemsellPrice - _itemshipping;
                                        subTotal = subTotal + _subtotal;
                                        saveamt = saveamt + _discountAmount;

                                        Seller_shipping = Seller_shipping - _itemshipping;
                                        Seller_subTotal = Seller_subTotal - _ItemsellPrice - _itemshipping;
                                        Seller_subTotal = Seller_subTotal + _subtotal;
                                        Seller_saveamt = Seller_saveamt + _discountAmount;

                                        if (_Totalextradiscount == 0 && _discountAmount == 0)
                                        {
                                            item["coupon_status"] = "failed";
                                            item["coupon_message"] = "Invalid coupon code.";
                                            item["coupon_details"] = null;
                                            item["coupon_amount"] = 0;

                                        }
                                        else
                                        {
                                            item["coupon_amount"] = _discountAmount;
                                        }
                                        item["ItemShippingCharges"] = _shipping;
                                        item["ItemSubTotal"] = Convert.ToDecimal(item["ItemSubTotal"]) - _discountAmount;
                                    }
                                    else
                                    {
                                        decimal _sellPrice = Convert.ToDecimal(itemPrice["selling_price"]) * Convert.ToInt32(item["qty"]);
                                        NewTotalSellPrice = NewTotalSellPrice + _sellPrice;
                                    }

                                }
                            }

                            foreach (JObject item in item1["Items"])
                            {
                                if (item["status"].ToString().ToLower() == "in stock")
                                {
                                    JObject codobj = getCart.getshippingfree(Catalougeurl, NewTotalSellPrice.ToString(), finalshipping);
                                    decimal _itemshipping = Convert.ToDecimal(item["ItemShippingCharges"].ToString());
                                    decimal _shipping = Convert.ToDecimal(item["ItemShippingCharges"].ToString());
                                    if (item["coupon_status"].ToString().ToLower() == "success")
                                    {
                                        if (shippingfree && Convert.ToBoolean(codobj["hasFree"]))
                                        {
                                            item["coupon_status"] = null;
                                            item["coupon_message"] = null;
                                            item["coupon_details"] = null;
                                            item["coupon_amount"] = 0;
                                        }
                                        else if (Convert.ToBoolean(codobj["hasFree"]))
                                        {
                                            _shipping = 0;
                                            shipping = shipping - _itemshipping;
                                            subTotal = subTotal - _itemshipping;
                                            saveamt = saveamt + _itemshipping;

                                            Seller_shipping = Seller_shipping - _itemshipping;
                                            Seller_subTotal = Seller_subTotal - _itemshipping;
                                            Seller_saveamt = Seller_saveamt + _itemshipping;

                                            item["ItemShippingCharges"] = _shipping;
                                            item["ItemSubTotal"] = Convert.ToDecimal(item["ItemSubTotal"]) - _itemshipping;
                                        }
                                    }
                                    else
                                    {
                                        decimal _oldItemsubtotal = Convert.ToDecimal(item["ItemSubTotal"]);
                                        if (Convert.ToBoolean(codobj["hasFree"]))
                                        {
                                            _shipping = 0;
                                            shipping = shipping - _itemshipping;
                                            subTotal = subTotal - _itemshipping;
                                            saveamt = saveamt + _itemshipping;

                                            Seller_shipping = Seller_shipping - _itemshipping;
                                            Seller_subTotal = Seller_subTotal - _itemshipping;
                                            Seller_saveamt = Seller_saveamt + _itemshipping;

                                            item["ItemShippingCharges"] = _shipping;
                                            item["ItemSubTotal"] = Convert.ToDecimal(item["ItemSubTotal"]) - _itemshipping;
                                            item["coupon_status"] = null;
                                            item["coupon_message"] = null;
                                            item["coupon_amount"] = 0;
                                        }
                                    }
                                }
                            }


                        }
                        selleritemPrice["paid_amount"] = Seller_subTotal;
                        selleritemPrice["total_SaveAmt"] = Seller_saveamt;
                        selleritemPrice["shipping_charges"] = Seller_shipping;
                        selleritemPrice["actual_shipping_charges"] = ActualSeller_shipping;
                        selleritemPrice["shipping_charges"] = Seller_shipping;
                        selleritemPrice["total_extradiscount"] = Seller_Extradiscount;

                    }
                }
                //}

            }
            else
            {
                int count = 0;
                //foreach (JObject item2 in Itemarray)
                //{
                if (Itemarray.Count > 0)
                {
                    foreach (var item1 in Itemarray[count.ToString()])
                    {
                        decimal Seller_Extradiscount = 0;
                        decimal Seller_subTotal = 0;
                        decimal Seller_saveamt = 0;
                        decimal Seller_shipping = 0;
                        decimal ActualSeller_shipping = 0;

                        JObject selleritemPrice = (JObject)item1["SellerCartAmount"];
                        Seller_subTotal = Convert.ToDecimal(selleritemPrice["paid_amount"]);
                        Seller_saveamt = Convert.ToDecimal(selleritemPrice["total_SaveAmt"]);
                        Seller_shipping = Convert.ToDecimal(selleritemPrice["shipping_charges"]);
                        ActualSeller_shipping = Convert.ToDecimal(selleritemPrice["shipping_charges"]);
                        foreach (var item in item1["Items"])
                        {
                            if (item["status"].ToString().ToLower() == "in stock")
                            {
                                JObject itemPrice = (JObject)item["itemPrice"];
                                decimal _itemshipping = Convert.ToDecimal(item["ItemShippingCharges"].ToString());
                                decimal _ItemsellPrice = Convert.ToDecimal(itemPrice["selling_price"]) * Convert.ToInt32(item["qty"]);

                                JObject codobj = getCart.getshippingfree(Catalougeurl, totalSellingPrice.ToString(), finalshipping);
                                decimal _shipping = Convert.ToDecimal(item["ItemShippingCharges"].ToString());
                                decimal _oldItemsubtotal = Convert.ToDecimal(item["ItemSubTotal"]);
                                if (Convert.ToBoolean(codobj["hasFree"]))
                                {
                                    _shipping = 0;
                                    shipping = shipping - _itemshipping;
                                    subTotal = subTotal - _itemshipping;
                                    saveamt = saveamt + _itemshipping;

                                    Seller_shipping = Seller_shipping - _itemshipping;
                                    Seller_subTotal = Seller_subTotal - _itemshipping;
                                    Seller_saveamt = Seller_saveamt + _itemshipping;

                                    item["ItemShippingCharges"] = _shipping;
                                    item["ItemSubTotal"] = Convert.ToDecimal(item["ItemSubTotal"]) - _itemshipping;

                                }
                            }
                        }
                        selleritemPrice["paid_amount"] = Seller_subTotal;
                        selleritemPrice["total_SaveAmt"] = Seller_saveamt;
                        selleritemPrice["shipping_charges"] = Seller_shipping;
                        selleritemPrice["actual_shipping_charges"] = ActualSeller_shipping;
                        selleritemPrice["shipping_charges"] = Seller_shipping;
                        selleritemPrice["total_extradiscount"] = Seller_Extradiscount;
                        count++;
                    }
                }
                //}
            }
            coupon.Subtotal = subTotal;
            coupon.SaveAmount = saveamt;
            coupon.ExtraDiscount = Extradiscount;
            coupon.ProductItems = Itemarray;
            coupon.Shipping = shipping;
            coupon.ActualShipping = finalshipping;
            return coupon;
        }

        public string GetGstDetail(string SellerId, string? Pincode)
        {
            string result = "Local";

            List<GSTInfo> gstInfo = getCart.GetGstDetail(SellerId, Userurl);

            if (!string.IsNullOrEmpty(Pincode))
            {
                DeliveryLibrary delivery = getCart.GetDelivery(Pincode, Userurl);


                var count = 0;


                count = gstInfo.Where(x => x.RegisteredCityId == delivery.CityID
                && x.RegisteredStateId == delivery.StateID
                && x.RegisteredCountryId == delivery.CountryID)
                    .ToList().Count();

                if (count > 0)
                {
                    result = "Local";
                }
                else
                {
                    count = gstInfo.Where(x => x.RegisteredCityId != delivery.CityID
                    && x.RegisteredStateId == delivery.StateID
                    && x.RegisteredCountryId == delivery.CountryID)
                    .ToList().Count();
                    if (count > 0)
                    {
                        result = "Zonal";
                    }
                    else
                    {
                        count = gstInfo.Where(x => x.RegisteredCityId != delivery.CityID
                        && x.RegisteredStateId != delivery.StateID
                        && x.RegisteredCountryId == delivery.CountryID)
                        .ToList().Count();
                        if (count > 0)
                        {
                            result = "National";
                        }
                        else
                        {
                            result = "National";
                        }
                    }
                }
            }

            return result;
        }



        public class BindProducts
        {
            public JObject productItems { get; set; }
        }
        public class CouponDiscount
        {
            public JObject ProductItems { get; set; }
            public decimal Subtotal { get; set; }
            public decimal SaveAmount { get; set; }
            public decimal ExtraDiscount { get; set; }
            public decimal Shipping { get; set; }
            public decimal ActualShipping { get; set; }
        }

    }
}
