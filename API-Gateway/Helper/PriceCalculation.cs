using API_Gateway.Common.products;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace API_Gateway.Helper
{
    public class PriceCalculation
    {

        private readonly IConfiguration _configuration;



        public string sellingPriceOnMRP(decimal mrp)
        {
            decimal _sellingPrice = mrp;
            return _sellingPrice.ToString("N0");
        }

        public string sellingPriceOnDiscount(decimal mrp, decimal discount)
        {
            decimal _discountedAmount = (Convert.ToDecimal(mrp) * Convert.ToDecimal(discount)) / 100;
            decimal _sellingPrice = (mrp - _discountedAmount);
            return _sellingPrice.ToString("N0");
        }

        public string CountDiscount(decimal mrp, decimal sellingprice)
        {
            decimal _discountRate = ((Convert.ToDecimal(mrp) - Convert.ToDecimal(sellingprice)) / (Convert.ToDecimal(mrp))) * 100;
            return _discountRate.ToString("N2");
        }

        public string CountPackagingWeight(PackagingWeightDTO packagingWeightDTO)
        {
            decimal result = (packagingWeightDTO.length * packagingWeightDTO.breadth * packagingWeightDTO.height) / 5000;
            return result.ToString();
        }
        public JObject CountNetSellerEarn(decimal mrp, decimal sellingprice, JObject commission, JObject extraCharges, JObject shipping)
        {

            decimal commissionCharges = Convert.ToDecimal(commission["commission_charges"].ToString());

            JObject youEarn = new JObject();

            #region Start Calculate You Earn
            decimal TotalExtraCharges = 0;
            if (extraCharges.Count > 0)
            {
                TotalExtraCharges = Convert.ToDecimal(extraCharges["totalExtraCharges"].ToString());
            }
            decimal _sellerearn = Convert.ToDecimal(sellingprice) - Convert.ToDecimal(commissionCharges) - TotalExtraCharges;
            youEarn["youEarn"] = _sellerearn;

            #endregion End  Calculate You Earn
            if (shipping.Count > 0)
            {
                #region Start Calculate Local You Earn

                decimal LocalshippingCharges = Convert.ToDecimal(shipping["LocalCharges"].ToString());
                decimal _Localsellerearn = Convert.ToDecimal(_sellerearn) - Convert.ToDecimal(LocalshippingCharges);
                youEarn["localYouEarn"] = _Localsellerearn;

                #endregion End  Calculate Local You Earn

                #region Start Calculate Zonal You Earn

                decimal ZonalshippingCharges = Convert.ToDecimal(shipping["ZonalCharges"].ToString());
                decimal _Zonalsellerearn = Convert.ToDecimal(_sellerearn) - Convert.ToDecimal(ZonalshippingCharges);
                youEarn["zonalYouEarn"] = _Zonalsellerearn;

                #endregion End  Calculate Zonal You Earn

                #region Start Calculate National You Earn

                decimal NationalshippingCharges = Convert.ToDecimal(shipping["NationalCharges"].ToString());
                decimal _Nationalsellerearn = Convert.ToDecimal(_sellerearn) - Convert.ToDecimal(NationalshippingCharges);
                youEarn["nationalYouEarn"] = _Nationalsellerearn;

                #endregion End  Calculate National You Earn
            }
            return youEarn;

        }

        public JObject displayCalculation(DisplayPriceCalculation priceCalculation, IHttpContextAccessor httpContextAccessor, string url, bool forCart = false, bool shippingOnCategory = false)
        {

            decimal mrp = priceCalculation.mrp.ToString() != null ? Convert.ToDecimal(priceCalculation.mrp) : 0;
            decimal sellingprice = priceCalculation.sellingprice.ToString() != null ? Convert.ToDecimal(priceCalculation.sellingprice) : 0;

            #region shipping start

            JObject shipping = new JObject();

            if (!shippingOnCategory && Convert.ToInt32(priceCalculation.weightSlabId) > 0)
            {
                shipping = getShippingChargesOnWeightSalb(httpContextAccessor, url, Convert.ToInt32(priceCalculation.weightSlabId), priceCalculation.shipmentBy, priceCalculation.shippingPaidBy);
            }

            #endregion shipping end


            #region commission start

            JObject commission = new JObject();
            if (!string.IsNullOrEmpty(priceCalculation.CommissionCharges) && !string.IsNullOrEmpty(priceCalculation.CommissionRate))
            {
                commission = countCommissionCost(Convert.ToDecimal(priceCalculation.sellingprice), priceCalculation.CommissionChargesIn, Convert.ToDecimal(priceCalculation.CommissionRate), Convert.ToDecimal(priceCalculation.CommissionCharges));
            }
            else
            {
                commission = getcommissionObject(httpContextAccessor, url, Convert.ToInt32(priceCalculation.categoryId), priceCalculation.sellerId, Convert.ToInt32(priceCalculation.BrandId), Convert.ToDecimal(priceCalculation.sellingprice));
            }

            #endregion shipping end

            #region extraCharges start
            JObject extraCharges = new JObject();
            if (Convert.ToInt32(priceCalculation.categoryId) > 0)
            {
                extraCharges = getExtraChargesObject(httpContextAccessor, url, Convert.ToInt32(priceCalculation.categoryId), Convert.ToDecimal(priceCalculation.sellingprice));
            }
            #endregion extraCharges end

            JObject youEarn = CountNetSellerEarn(mrp, sellingprice, commission, extraCharges, shipping);

            JObject pricing = new JObject();

            #region Set Format start

            pricing["customerPricing"] = SetJsonFormat(mrp, sellingprice, youEarn, commission,extraCharges, shipping, forCart);

            #endregion Set Format End

            return pricing;
        }

        public JArray SetJsonFormat(decimal mrp, decimal sellingPrice, JObject youEarn, JObject commission, JObject extraCharges, JObject shipping, bool forCart)
        {
            JObject customer = new JObject();

            #region local start

            JObject _heading = new JObject();

            _heading["values"] = new JArray("item_details", "local", "zonal", "national");

            customer["heading"] = _heading;

            JObject _columns = new JObject();

            JObject _mrp = new JObject();
            _mrp["values"] = new JArray(mrp, mrp, mrp);

            JObject _sellingprice = new JObject();
            _sellingprice["values"] = new JArray(sellingPrice, sellingPrice, sellingPrice);

            string commission_charges = Convert.ToDecimal(commission["commission_charges"]).ToString("N2");
            string commission_per = Convert.ToDecimal(commission["commission_rate"]).ToString("N2");

            JObject _commission = new JObject();
            _commission["values"] = new JArray(" (-) " + commission_charges + " (" + commission_per + "%)");
            _commission["commissionvalues"] = new JArray(commission_charges);
            _commission["commissionRate"] = commission_per;
            _commission["commissionIn"] = commission["commission_charges_in"];
            JObject _youearn = new JObject();
            _youearn["values"] = new JArray(youEarn["youEarn"], youEarn["youEarn"], youEarn["youEarn"]);

            _columns["mrp"] = _mrp;
            _columns["selling_price"] = _sellingprice;
            if (forCart)
            {
                _columns["other_charges"] = extraCharges["extra_charges_obj"];
            }
            else
            {
                _columns.Merge(extraCharges["extra_charges_obj"]);
            }
            _columns["platform_fees"] = _commission;
            _columns["you_earn"] = _youearn;

            if (shipping.Count > 0)
            {
                JObject _shipping = new JObject();

                string LocalCharges = Convert.ToDecimal(shipping["LocalCharges"]).ToString("N2");
                string ZonalCharges = Convert.ToDecimal(shipping["ZonalCharges"]).ToString("N2");
                string NationalCharges = Convert.ToDecimal(shipping["NationalCharges"]).ToString("N2");

                _shipping["values"] = new JArray(LocalCharges, ZonalCharges, NationalCharges);
                _shipping["subtext"] = "Shipment by " + shipping["shipmentBy"];
                _shipping["shipmentBy"] = shipping["shipmentBy"];
                _shipping["shippingPaidBy"] = shipping["shippingPaidBy"];

                string actualLocalCharges = Convert.ToDecimal(shipping["actualLocalCharges"]).ToString("N2");
                string actualZonalCharges = Convert.ToDecimal(shipping["actualZonalCharges"]).ToString("N2");
                string actualNationalCharges = Convert.ToDecimal(shipping["actualNationalCharges"]).ToString("N2");

                _shipping["actualShippingCharges"] = new JArray(actualLocalCharges, actualZonalCharges, actualNationalCharges);

                JObject _final_you_earn = new JObject();

                string localYouEarn = Convert.ToDecimal(youEarn["localYouEarn"]).ToString("N0");
                string zonalYouEarn = Convert.ToDecimal(youEarn["zonalYouEarn"]).ToString("N0");
                string nationalYouEarn = Convert.ToDecimal(youEarn["nationalYouEarn"]).ToString("N0");

                _final_you_earn["values"] = new JArray(localYouEarn, zonalYouEarn, nationalYouEarn);
                _columns["shipping_charges"] = _shipping;
                _columns["final_you_earn"] = _final_you_earn;
            }

            #endregion local End

            customer["columns"] = _columns;

            JArray pricingArray = new JArray();
            pricingArray.Add(customer);

            return pricingArray;
        }

        public JObject countCommissionCost(decimal sellingprice, string chargesIn, decimal percentageValue, decimal Amount)
        {
            JObject commission = new JObject();
            decimal _commissionCharges = 0;
            decimal _commissionPercentage = 0;
            if (chargesIn.ToLower() == "percentage")
            {
                _commissionCharges = (Convert.ToDecimal(sellingprice) * Convert.ToDecimal(percentageValue)) / 100;
                _commissionPercentage = percentageValue;
            }
            else
            {
                if (Amount != 0 && sellingprice != 0)
                {
                    _commissionCharges = Amount;
                    _commissionPercentage = (Convert.ToDecimal(Amount) / Convert.ToDecimal(sellingprice)) * 100;

                    _commissionCharges = (Convert.ToDecimal(sellingprice) * Convert.ToDecimal(_commissionPercentage)) / 100;
                }
                else
                {
                    _commissionPercentage = 0;
                    _commissionCharges = 0;
                }
            }
            commission["commission_charges_in"] = chargesIn;
            commission["commission_charges"] = Convert.ToDecimal(_commissionCharges).ToString("N2");
            commission["commission_rate"] = Convert.ToDecimal(_commissionPercentage).ToString("N2");
            return commission;
        }

        public JObject getcommissionObject(IHttpContextAccessor httpContextAccessor, string url, int CategoryId, string sellerId, int brandId, decimal sellingPrice)
        {
            JObject commission = new JObject();

            #region Start Commission

            GetCommissionCharges getCommissionCharges = new GetCommissionCharges(httpContextAccessor);
            CommissionChargesLibrary commissionCharges = new CommissionChargesLibrary();
            decimal _commissionCharges = 0;
            decimal _commissionPercentage = 0;
            if (CategoryId != 0 && !string.IsNullOrEmpty(sellerId) && brandId != 0)
            {
                commissionCharges = getCommissionCharges.GetCommission(CategoryId, sellerId, brandId, url);


                if (commissionCharges.ID != null && commissionCharges.ID != 0)
                {
                    if (commissionCharges.ChargesIn.ToLower() == "percentage")
                    {
                        _commissionCharges = (Convert.ToDecimal(sellingPrice) * Convert.ToDecimal(commissionCharges.AmountValue)) / 100;
                        _commissionPercentage = Convert.ToDecimal(commissionCharges.AmountValue);
                    }
                    else
                    {
                        _commissionCharges = Convert.ToDecimal(commissionCharges.AmountValue);
                        _commissionPercentage = (Convert.ToDecimal(_commissionCharges) / Convert.ToDecimal(sellingPrice)) * 100;
                    }
                    commission["commission_charges_in"] = commissionCharges.ChargesIn;
                    commission["commission_charges"] = _commissionCharges;
                    commission["commission_rate"] = _commissionPercentage;
                }
                else
                {
                    commission["commission_charges_in"] = null;
                    commission["commission_charges"] = 0;
                    commission["commission_rate"] = 0;
                }
            }
            else
            {
                commission["commission_charges_in"] = null;
                commission["commission_charges"] = 0;
                commission["commission_rate"] = 0;
            }

            #endregion End Commission

            return commission;
        }


        public JObject getExtraChargesObject(IHttpContextAccessor httpContextAccessor, string url, int categoryId, decimal sellingPrice)
        {
            JObject onjExtraChargs = new JObject();
            JObject _ExtraChargesObj = new JObject();
            GetAllExtraCharges getExtraCharges = new GetAllExtraCharges(httpContextAccessor);
            List<ExtraChargesLibrary> lstExtraChargesLibrary = getExtraCharges.GetExtraCharges(categoryId, url);

            decimal TotalExtraCharges = 0;
            foreach (var charges in lstExtraChargesLibrary)
            {
                JObject ExtraChargesObj = new JObject();

                #region Start Calculation

                decimal extraCharge = 0;

                if (!string.IsNullOrEmpty(charges.ChargesPaidByName))
                {
                    ExtraChargesObj["charges_paid_by"] = charges.ChargesPaidByName;

                    if (charges.ChargesIn.ToLower() == "percentage")
                    {
                        decimal Charge = Convert.ToDecimal(Convert.ToDecimal((Convert.ToDecimal(sellingPrice) * Convert.ToDecimal(charges.PercentageValue)) / 100).ToString("N2"));

                        if (Charge <= Convert.ToDecimal(charges.AmountValue))
                        {
                            extraCharge = Convert.ToDecimal(charges.AmountValue);
                        }
                        else if (charges.MaxAmountValue != null && charges.MaxAmountValue > 0)
                        {
                            if (Charge >= Convert.ToDecimal(charges.MaxAmountValue))
                            {
                                extraCharge = Convert.ToDecimal(charges.MaxAmountValue);
                            }
                            else
                            {
                                extraCharge = Charge;
                            }
                        }
                        else
                        {
                            extraCharge = Charge;
                        }
                    }
                    else
                    {
                        extraCharge = Convert.ToDecimal(charges.AmountValue);
                    }

                    ExtraChargesObj["charges_in"] = charges.ChargesIn;


                    if (charges.ChargesPaidByName.ToLower() == "seller")
                    {
                        ExtraChargesObj["charges"] = extraCharge;
                    }
                    else
                    {
                        ExtraChargesObj["charges"] = 0;
                    }

                    ExtraChargesObj["actual_charges"] = extraCharge;

                    string extracharges = Convert.ToDecimal(ExtraChargesObj["charges"]).ToString("N2");
                    string paidby = charges.ChargesPaidByName;
                    if (charges.ChargesIn.ToLower() == "percentage")
                    {
                        string perval = Convert.ToDecimal(charges.PercentageValue).ToString("N2");
                        extracharges = extracharges + " (" + perval + "%)";
                    }
                    JObject _newObject = new JObject();
                    _newObject["values"] = new JArray(extracharges);
                    _newObject["subtext"] = "Charges paid by " + paidby;
                    _newObject["charges_type"] = charges.Name;
                    _newObject["charges_paid_by"] = paidby;
                    _newObject["charges_in"] = charges.ChargesIn;
                    _newObject["charges_percentage_value"] = charges.PercentageValue;
                    _newObject["charges_amount_value"] = charges.AmountValue;
                    _newObject["charges_max_value"] = charges.MaxAmountValue;
                    _newObject["actual_charges"] = ExtraChargesObj["actual_charges"];
                    _ExtraChargesObj[charges.Name.ToLower().Replace(" ", "_")] = _newObject;
                }
                else
                {
                    ExtraChargesObj["charges_paid_by"] = null;
                    ExtraChargesObj["charges_in"] = null;
                    ExtraChargesObj["charges"] = 0;
                    ExtraChargesObj["actual_charges"] = 0;
                }

                #endregion End Calculation

                TotalExtraCharges += Convert.ToDecimal(ExtraChargesObj["charges"]);
                //onjExtraChargs[charges.Name] = ExtraChargesObj;
            }
            onjExtraChargs["totalExtraCharges"] = TotalExtraCharges;
            onjExtraChargs["extra_charges_obj"] = _ExtraChargesObj;
            return onjExtraChargs;
        }

        public JObject getShippingChargesOnWeightSalb(IHttpContextAccessor httpContextAccessor, string url, int weightSlabId, string shipmentBy, string shippingPaidBy)
        {
            JObject shipping = new JObject();

            #region start Calculation

            GetShippingChargesOnWeightSlab getShippingChargesOnWeightSlab = new GetShippingChargesOnWeightSlab(httpContextAccessor);
            WeightSlabLibrary weightSlab = getShippingChargesOnWeightSlab.GetShippingCharges(weightSlabId, url);


            if (!string.IsNullOrEmpty(shippingPaidBy) && !string.IsNullOrEmpty(shipmentBy))
            {
                shipping["shipmentBy"] = shipmentBy;
                shipping["shippingPaidBy"] = shippingPaidBy;
                if (shippingPaidBy.ToLower() == "seller")
                {
                    shipping["LocalCharges"] = weightSlab.LocalCharges;
                    shipping["ZonalCharges"] = weightSlab.ZonalCharges;
                    shipping["NationalCharges"] = weightSlab.NationalCharges;

                }
                else
                {
                    shipping["LocalCharges"] = 0;
                    shipping["ZonalCharges"] = 0;
                    shipping["NationalCharges"] = 0;
                }
                shipping["actualLocalCharges"] = weightSlab.LocalCharges;
                shipping["actualZonalCharges"] = weightSlab.ZonalCharges;
                shipping["actualNationalCharges"] = weightSlab.NationalCharges;

            }
            else
            {
                shipping["shipmentBy"] = null;
                shipping["shippingPaidBy"] = null;
                shipping["LocalCharges"] = 0;
                shipping["ZonalCharges"] = 0;
                shipping["NationalCharges"] = 0;
                shipping["actualLocalCharges"] = 0;
                shipping["actualZonalCharges"] = 0;
                shipping["actualNationalCharges"] = 0;
            }

            #endregion End Calculation

            return shipping;
        }


        public List<TaxTypeValueLibrary> taxTypeValuelst(IHttpContextAccessor httpContextAccessor, string url)
        {
            BaseResponse<TaxTypeValueLibrary> TaxbaseResponse = new BaseResponse<TaxTypeValueLibrary>();
            List<TaxTypeValueLibrary> taxTypeValuelst = new List<TaxTypeValueLibrary>();

            ApiHelper apiHelper = new ApiHelper(httpContextAccessor.HttpContext);
            var response = apiHelper.ApiCall(url, EndPoints.TaxTypeValue + "?PageIndex=0&PageSize=0", "GET");
            TaxbaseResponse = TaxbaseResponse.JsonParseList(response);

            if (TaxbaseResponse.code == 200)
            {
                taxTypeValuelst = (List<TaxTypeValueLibrary>)TaxbaseResponse.Data;
            }

            return taxTypeValuelst;
        }
    }
}
