using API_Gateway.Common.cart;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using API_Gateway.Models.Entity.Catalogue;
using System;

namespace API_Gateway.Helper
{
    public class ClsCartCalcultion
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

        public ClsCartCalcultion(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
            JArray Itemarray = new JArray();
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
            
            checkOfferDto getoffers = new checkOfferDto();

            getChekoutCalculation getcartCalculation = new getChekoutCalculation();
            getcartCalculation.CartJson = jsonString;

            List<GetCheckOutDetailsList> lstCartDetails = getCartDetailslst(getcartCalculation);
            
            foreach (GetCheckOutDetailsList item in lstCartDetails)
            {
                JObject productItems = new JObject();

                int SizeId = 0;
                decimal TotalProductQty = 0;
                decimal itemMrp = Convert.ToDecimal(item.TempMRP);
                decimal itemSellprice = Convert.ToDecimal(item.TempSellingPrice);
                decimal itemDiscount = Convert.ToDecimal(item.TempDiscount);
                decimal itemQty = Convert.ToInt32(item.Quantity);
                decimal itemExtracharges = 0;

                decimal itemShipping = 0;

                decimal tempItemMrp = Convert.ToDecimal(item.ItemMRP);
                decimal tempItemSellprice = Convert.ToDecimal(item.ItemSellingPrice);
                decimal tempItemDiscount = Convert.ToDecimal(item.ItemDiscount);

                decimal totalItemMrp = Convert.ToDecimal(item.ItemMRP);
                decimal totalItemSellprice = Convert.ToDecimal(item.ItemSellingPrice);
                decimal totalItemDiscount = Convert.ToDecimal(item.ItemDiscount);
                decimal totalItemExtracharges = 0;

                var extraDetails = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(item.ExtraDetails);
                var sellerStatus = extraDetails["SellerDetails"]["SellerStatus"].Value<string>().ToLower();
                var assignBrandStatus = extraDetails["BrandDetails"]["AssignBrandStatus"].Value<string>().ToLower();
                var brandStatus = extraDetails["BrandDetails"]["BrandStatus"].Value<string>().ToLower();
                
                bool IsProductAvailable = (item.ProductStatus.ToLower() == "active" && item.LiveStatus == true && sellerStatus.ToLower() == "active" && assignBrandStatus.ToLower() == "active" && brandStatus.ToLower() == "active") ? true : false;

                if (IsProductAvailable)
                {
                    TotalProductQty = Convert.ToInt32(item.TotalQty);

                    string result = GetGstDetail(item.SellerId, cartCalculation.Pincode);

                    if (item.SizeId != null && item.SizeId != 0)
                    {
                        SizeId = Convert.ToInt32(item.SizeId);
                    }

                    BindProducts bindProducts = new BindProducts();
                    bindProducts = bindProductJson(item);
                    productItems = bindProducts.productItems;

                    JObject price = new JObject();
                    price["mrp"] = tempItemMrp;
                    price["selling_price"] = tempItemSellprice;
                    price["old_selling_price"] = itemSellprice;
                    price["discount"] = tempItemDiscount;
                    
                    List<CategoryLibrary> CategoryLibrary = JsonConvert.DeserializeObject<List<CategoryLibrary>>(item.ParentCategoryList);

                    if (CategoryLibrary.Count == CategoryLibrary.Where(p => p.Status.ToLower() == "active").Count())
                    {
                        if (itemQty != 0 && itemQty <= TotalProductQty)
                        {
                            totalItemMrp = (Convert.ToDecimal(tempItemMrp) * itemQty);
                            totalItemSellprice = (Convert.ToDecimal(tempItemSellprice) * itemQty);
                            totalItemDiscount = totalItemMrp - totalItemSellprice;

                            totalMrp = totalMrp + totalItemMrp;
                            totalSellingPrice = totalSellingPrice + totalItemSellprice;
                            totalDiscount = totalDiscount + totalItemDiscount;

                            decimal getSellingPrice = totalItemSellprice;
                            decimal _getSellingPrice = 0;

                            #region Count Extra Discount

                            if (!string.IsNullOrEmpty(cartCalculation.CouponCode))
                            {
                                checkOfferDto checkoffers = new checkOfferDto();
                                checkoffers = getCart.getOffers(cartCalculation.CouponCode, Convert.ToInt32(item.ProductId), Convert.ToInt32(item.BrandId), Convert.ToInt32(item.Categoryid), item.SellerId, cartCalculation.UserId, cartCalculation.PaymentMode, Catalougeurl, OrderUrl);


                                productItems["coupon_status"] = (checkoffers.CouponApplied) ? "success" : "failed";
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

                            #endregion Count Extra Discount

                            #region Item and cart Calculation

                            string taxrate = item.TaxRate.ToString();

                            var settings = new JsonSerializerSettings
                            {
                                Converters = { new CaseInsensitiveDictionaryConverter() }
                            };

                            var Taxvalue = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(taxrate, settings);

                            #region Shipping

                            SellerKycList seller = getCart.getsellerKyc(item.SellerId);

                            #endregion Shipping

                            bool setMarginProductLevel = Convert.ToBoolean(_configuration.GetValue<string>("allow_set_margin_on_product_level"));

                            DisplayPriceCalculation displayPriceCalculation = new DisplayPriceCalculation();
                            displayPriceCalculation.mrp = itemMrp;
                            displayPriceCalculation.sellingprice = itemSellprice;
                            displayPriceCalculation.weightSlabId = Convert.ToInt32(item.WeightSlabId);
                            displayPriceCalculation.categoryId = Convert.ToInt32(item.Categoryid);
                            displayPriceCalculation.BrandId = Convert.ToInt32(item.BrandId);
                            displayPriceCalculation.sellerId = item.SellerId;
                            displayPriceCalculation.shipmentBy = seller.ShipmentBy;
                            displayPriceCalculation.shippingPaidBy = seller.ShipmentChargesPaidByName;

                            if (setMarginProductLevel)
                            {
                                displayPriceCalculation.CommissionChargesIn = Convert.ToString(item.MarginIn);
                                displayPriceCalculation.CommissionCharges = Convert.ToString(item.MarginCost);
                                displayPriceCalculation.CommissionRate = Convert.ToString(item.MarginPercentage);
                            }

                            JObject itemdata = new JObject();
                            itemdata["productID"] = item.ProductId;
                            itemdata["productGuid"] = item.ProductGuid;
                            itemdata["sellerProductID"] = item.SellerProductId;
                            itemdata["orderTaxRateId"] = item.TaxValueId;
                            itemdata["orderTaxRate"] = taxrate;
                            itemdata["hsnCode"] = item.HSNCode;
                            itemdata["weightSalb"] = item.WeightSlab;

                            JObject ProducttaxInfo = GetProducttaxInfo(displayPriceCalculation, itemdata, Convert.ToInt32(itemQty), result);

                            //decimal gstval = Convert.ToDecimal(_sellingprice) * Convert.ToInt32(Taxvalue["igst"]) / (100 + Convert.ToInt32(Taxvalue["igst"]));
                            decimal gstval = Convert.ToDecimal(totalItemSellprice) * Convert.ToInt32(Taxvalue.Select(a => a.Value).Max()) / (100 + Convert.ToInt32(Taxvalue.Select(a => a.Value).Max()));
                            totalGst = totalGst + gstval;

                            if (ProducttaxInfo["shippingPaidBy"].ToString().ToLower() == "customer")
                            {
                                itemShipping = Convert.ToDecimal(ProducttaxInfo["actualShippingCharges"].ToString());
                                //_shipping = _shipping * tempQty;
                                totalShipping = totalShipping + itemShipping;
                            }

                            if (ProducttaxInfo["otherCharges"] != null && ProducttaxInfo["otherCharges"].Type == JTokenType.Object)
                            {
                                JObject localObject = (JObject)ProducttaxInfo["otherCharges"];
                                foreach (KeyValuePair<string, JToken> items in localObject)
                                {
                                    JObject localItem = (JObject)items.Value;
                                    if (localItem["charges_paid_by"] != null && (string)localItem["charges_paid_by"].ToString().ToLower() == "customer")
                                    {
                                        itemExtracharges += Convert.ToDecimal(Convert.ToDecimal(localItem["actual_Charges"]).ToString("N0"));
                                        localItem["total_charges"] = Convert.ToDecimal(localItem["actual_charges"]) * itemQty;
                                    }
                                    else
                                    {
                                        localItem["total_charges"] = 0;
                                    }

                                    localItem["actual_Charges"] = Convert.ToDecimal(localItem["actual_Charges"]) * itemQty;
                                }

                            }

                            totalItemExtracharges = itemExtracharges * itemQty;

                            totalExtracharges = totalExtracharges + totalItemExtracharges;

                            //Extradiscount = Extradiscount + _extradiscount;
                            //saveamt = saveamt + discount + Extradiscount;
                            totalSaveamt = totalSaveamt + totalItemDiscount;

                            decimal _totalamt = totalItemSellprice;
                            totalAmount = totalAmount + _totalamt;

                            //decimal _subtotal = sellprice - Extradiscount;
                            decimal _subtotal = totalItemSellprice + totalItemExtracharges+ itemShipping;
                            subTotal = subTotal + _subtotal;

                            #endregion Item and cart Calculation

                            //pricearray.Add(price);

                            productItems["status"] = "In stock";
                            productItems["gstIncluding"] = gstval.ToString("N2");
                            productItems["itemPrice"] = price;
                            productItems["taxinfo"] = ProducttaxInfo;
                            productItems["ItemShippingCharges"] = itemShipping.ToString("N0"); ;
                            productItems["ItemShippingPaidBy"] = ProducttaxInfo["shippingPaidBy"].ToString();
                            productItems["ItemShippingZone"] = result;
                            productItems["ItemSubTotal"] = _subtotal.ToString("N0");
                            productItems["ItemExtracharges"] = totalItemExtracharges;
                            productItems["ItemTotal"] = _totalamt.ToString("N0");
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
                    else
                    {
                        productItems["status"] = "Out of stock";
                        productItems["coupon_status"] = null;
                        productItems["coupon_message"] = null;
                        productItems["coupon_details"] = null;
                        productItems["coupon_amount"] = 0;
                    }
                }
                else
                {
                    BindProducts bindProducts = new BindProducts();
                    bindProducts = bindProductJson(item);
                    productItems = bindProducts.productItems;
                    JObject price = new JObject();
                    price["mrp"] = tempItemMrp;
                    price["selling_price"] = tempItemSellprice;
                    price["old_selling_price"] = itemSellprice;
                    price["discount"] = tempItemDiscount;
                    productItems["itemPrice"] = price;
                    productItems["status"] = "Seller Product is not available.";
                    productItems["coupon_status"] = null;
                    productItems["coupon_message"] = null;
                    productItems["coupon_details"] = null;
                    productItems["coupon_amount"] = 0;

                }

                Itemarray.Add(productItems);
            }

            totalActualShipping = totalShipping;
            CouponDiscount coupon = new CouponDiscount();
            coupon = SetCouponDiscount(Itemarray, getoffers, TempCartValue, totalExtradiscount, subTotal, totalSaveamt, totalShipping, totalSellingPrice);
            Itemarray = coupon.ProductItems;
            subTotal = coupon.Subtotal;
            totalSaveamt = coupon.SaveAmount;
            totalExtradiscount = coupon.ExtraDiscount;
            totalShipping = coupon.Shipping;
            totalActualShipping = coupon.ActualShipping;

            #region Set Cart Total

            products["items"] = Itemarray;
            JObject CartAmount = new JObject();
            CartAmount["total_mrp"] = totalMrp;
            CartAmount["total_selling_price"] = totalSellingPrice;
            CartAmount["total_discount"] = totalDiscount.ToString("N0");
            CartAmount["total_extra_charges"] = totalExtracharges.ToString("N0");
            CartAmount["total_SaveAmt"] = totalSaveamt.ToString("N0");
            CartAmount["total_inclusivegst"] = totalGst.ToString("N2");
            if (totalExtradiscount > 0)
            {
                CartAmount["total_extradiscount"] = totalExtradiscount;
            }
            CartAmount["shipping_charges"] = totalShipping;
            CartAmount["actual_shipping_charges"] = totalActualShipping;
            CartAmount["total_amount"] = totalAmount.ToString("N0");
            if (cartCalculation.PaymentMode.ToLower() == "cod")
            {
                JObject codobj = getCart.getCodCharge(Catalougeurl, subTotal.ToString());
                if (Convert.ToBoolean(codobj["codAvailable"]))
                {
                    if (!Convert.ToBoolean(codobj["hasFree"]))
                    {
                        if (Convert.ToBoolean(codobj["free_cod_on_mov"]))
                        {
                            decimal cod = Convert.ToDecimal(codobj["codCharges"]);
                            CartAmount["cod_charges"] = cod;
                            CartAmount["cod_message"] = codobj["message"];
                            subTotal = subTotal + cod;
                        }
                        else
                        {
                            decimal cod = Convert.ToDecimal(codobj["codCharges"]);
                            CartAmount["cod_charges"] = cod;
                            CartAmount["cod_message"] = codobj["message"];
                            subTotal = subTotal + cod;
                        }
                    }
                    else
                    {
                        decimal cod = Convert.ToDecimal(codobj["codCharges"]);
                        CartAmount["cod_charges"] = cod;
                        CartAmount["cod_message"] = codobj["message"];
                        subTotal = subTotal + cod;
                    }
                }
                else
                {
                    decimal cod = Convert.ToDecimal(codobj["codCharges"]);
                    subTotal = subTotal + cod;
                }

            }

            CartAmount["paid_amount"] = subTotal.ToString("N0"); ;

            products["CartAmount"] = CartAmount;

            #endregion Set Cart Total

            return products;
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

            BindProducts bindProducts = new BindProducts();
            bindProducts.productItems = productItems;
            return bindProducts;
        }

        public List<GetCheckOutDetailsList> getCartDetailslst(getChekoutCalculation calculation)
        {
            BaseResponse<GetCheckOutDetailsList> cartResponse = new BaseResponse<GetCheckOutDetailsList>();
            List<GetCheckOutDetailsList> lstCart = new List<GetCheckOutDetailsList>();
            var response = helper.ApiCall(Catalougeurl, EndPoints.CartGetCartList, "POST", calculation);
            cartResponse = cartResponse.JsonParseList(response);

            if (cartResponse.code == 200)
            {
                lstCart = cartResponse.Data as List<GetCheckOutDetailsList>;
            }
            return lstCart;
        }


        public JObject GetProducttaxInfo(DisplayPriceCalculation displaypriceCalculation, JObject itemdata, int qty, string result = "Local")
        {
            JObject OraderItemTaxInfo = new JObject();
            JObject ItemTaxInfo = new JObject();


            PriceCalculation priceCalculation = new PriceCalculation();

            bool shippingOnCategory = Convert.ToBoolean(_configuration.GetValue<string>("shipping_charges_on_Category"));
            ItemTaxInfo = priceCalculation.displayCalculation(displaypriceCalculation, _httpContextAccessor, Catalougeurl, true, shippingOnCategory);
            dynamic jsonObject = JsonConvert.DeserializeObject(ItemTaxInfo.ToString());

            OraderItemTaxInfo["productID"] = itemdata["productID"];
            OraderItemTaxInfo["sellerProductID"] = itemdata["sellerProductID"];
            decimal net_earn = 0;

            net_earn = result.ToLower() == "local" ? Convert.ToDecimal(jsonObject.customerPricing[0].columns.final_you_earn.values[0]) :
                       result.ToLower() == "zonal" ? Convert.ToDecimal(jsonObject.customerPricing[0].columns.final_you_earn.values[1]) :
                       result.ToLower() == "national" ? Convert.ToDecimal(jsonObject.customerPricing[0].columns.final_you_earn.values[2]) : 0;

            decimal actualShippingCharges = 0;

            actualShippingCharges = result.ToLower() == "local" ? Convert.ToDecimal(jsonObject.customerPricing[0].columns.shipping_charges.actualShippingCharges[0]) :
                       result.ToLower() == "zonal" ? Convert.ToDecimal(jsonObject.customerPricing[0].columns.shipping_charges.actualShippingCharges[1]) :
                       result.ToLower() == "national" ? Convert.ToDecimal(jsonObject.customerPricing[0].columns.shipping_charges.actualShippingCharges[2]) : 0;


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
        public CouponDiscount SetCouponDiscount(JArray Itemarray, checkOfferDto getoffers, decimal TempCartValue, decimal finaldiscount, decimal finalsubTotal, decimal finalsaveamt, decimal finalshipping, decimal totalSellingPrice)
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

                var filteredItems = new JArray(Itemarray.Where(item => item["coupon_status"] != null && item["coupon_status"].ToString().ToLower() == "success")).ToList();

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
                    foreach (JObject item in Itemarray)
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

                                NewTotalSellPrice = NewTotalSellPrice + _sellPrice;
                                decimal _subtotal = _sellPrice + _shipping;
                                //decimal _oldItemsubtotal = Convert.ToDecimal(item["ItemSubTotal"]) - _ItemsellPrice - _itemshipping;
                                subTotal = subTotal - _ItemsellPrice - _itemshipping;
                                subTotal = subTotal + _subtotal;
                                saveamt = saveamt + _discountAmount;

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

                    foreach (JObject item in Itemarray)
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

            }
            else
            {
                foreach (JObject item in Itemarray)
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
                            item["ItemShippingCharges"] = _shipping;
                            item["ItemSubTotal"] = Convert.ToDecimal(item["ItemSubTotal"]) - _itemshipping;

                        }
                    }
                }
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
            public JArray ProductItems { get; set; }
            public decimal Subtotal { get; set; }
            public decimal SaveAmount { get; set; }
            public decimal ExtraDiscount { get; set; }
            public decimal Shipping { get; set; }
            public decimal ActualShipping { get; set; }
        }
    }
}
