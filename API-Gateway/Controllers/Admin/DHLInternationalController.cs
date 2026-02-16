using Amazon.Runtime;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Cryptography.Xml;
using System.Text.Json.Nodes;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class DHLInternationalController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper api;
        public DHLInternationalController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpContext = _httpContextAccessor.HttpContext;
            api = new ApiHelper(_httpContext);
        }

        [HttpPost("Admin/Rate")]
        public async Task<IActionResult> Rate()
        {

            JsonObject rateObj = new JsonObject();
            JsonObject shipperDetails = new JsonObject();
            JsonObject receiverDetails = new JsonObject();
            JsonObject accounts = new JsonObject();
            JsonObject packages = new JsonObject();
            JsonObject dimensions = new JsonObject();


            shipperDetails["postalCode"] = "";
            shipperDetails["cityName"] = "Nairobi";
            shipperDetails["countryCode"] = "KE";
            shipperDetails["addressLine1"] = "Apple Wood Adams";
            shipperDetails["addressLine2"] = "Ngong Road";
            shipperDetails["countyName"] = "Nairobi";

            receiverDetails["postalCode"] = "";
            receiverDetails["cityName"] = "Kampala";
            receiverDetails["countryCode"] = "UG";
            receiverDetails["addressLine1"] = "Main Street";
            receiverDetails["addressLine2"] = "these address lines are not validated";

            accounts["typeCode"] = "shipper";
            accounts["number"] = "351341977";

            dimensions["length"] = 25;
            dimensions["width"] = 10;
            dimensions["height"] = 5;

            packages["weight"] = 2.5;
            packages["dimensions"] = dimensions;

            rateObj["customerDetails"] = new JsonObject {
                {"shipperDetails", shipperDetails },
                {"receiverDetails", receiverDetails}
            };

            rateObj["accounts"] = new JsonArray { accounts };

            rateObj["productCode"] = "P";
            rateObj["valueAddedServices"] = new JsonArray { };
            rateObj["payerCountryCode"] = "KE";
            rateObj["plannedShippingDateAndTime"] = DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "GMT+00:00";

            rateObj["unitOfMeasurement"] = "metric";
            rateObj["isCustomsDeclarable"] = true;
            rateObj["requestAllValueAddedServices"] = false;
            rateObj["returnStandardProductsOnly"] = false;
            rateObj["nextBusinessDay"] = true;
            rateObj["productTypeCode"] = "all";
            //rateObj["packages"] =new JsonArray {packages,dimensions };
            rateObj["packages"] = new JsonArray { packages };



            var response = api.ApiCallDHL("https://express.api.dhl.com/mydhlapi/test/", "rates", "POST", rateObj.ToString());
            var result = await response.Content.ReadAsStringAsync();

            return Ok(result);

        }

        [HttpPost("Admin/Shipments")]
        public async Task<IActionResult> Shipments()
        {

            JsonObject shipmentObj = new JsonObject();
            JsonObject pickup = new JsonObject();
            JsonObject accounts = new JsonObject();
            JsonObject shipperDetails = new JsonObject();
            JsonObject shipperPostalAddress = new JsonObject();
            JsonObject shippercontactInfo = new JsonObject();
            JsonObject receiverDetails = new JsonObject();
            JsonObject receiverPostalAddress = new JsonObject();
            JsonObject receivercontactInfo = new JsonObject();
            JsonObject content = new JsonObject();
            JsonObject exportDeclaration = new JsonObject();
            JsonArray lineItems = new JsonArray();
            JsonObject lineItemsData = new JsonObject();
            JsonObject lineItemsquantity = new JsonObject();
            JsonObject lineItemsDataweight = new JsonObject();
            JsonObject lineItemsDatainvoice = new JsonObject();

            JsonObject packages = new JsonObject();
            JsonObject dimensions = new JsonObject();

            pickup["isRequested"] = false;

            accounts["typeCode"] = "shipper";
            accounts["number"] = "351341977";


            shipperPostalAddress["postalCode"] = "382120";
            shipperPostalAddress["cityName"] = "Nairobi";
            shipperPostalAddress["countryCode"] = "KE";
            shipperPostalAddress["addressLine1"] = "DURUMA ROAD";
            shipperPostalAddress["addressLine2"] = "OFF KWALE ROAD";
            shipperPostalAddress["countyName"] = "Nairobi";


            shippercontactInfo["email"] = "tes2024@yopmail.com";
            shippercontactInfo["phone"] = "254711222333";
            shippercontactInfo["mobilePhone"] = "254711222333";
            shippercontactInfo["companyName"] = "AFRICA VENDORS HUB";
            shippercontactInfo["fullName"] = "TEST TESTER";

            shipperDetails = new JsonObject {
                { "postalAddress", shipperPostalAddress },
                { "contactInformation", shippercontactInfo }
            };


            receiverPostalAddress["cityName"] = "SWAKOPMUND";
            receiverPostalAddress["countryCode"] = "NA";
            receiverPostalAddress["provinceCode"] = "N/A";
            receiverPostalAddress["postalCode"] = "382421";
            receiverPostalAddress["addressLine1"] = "Mondesa Police Station, Mondelani street";
            receiverPostalAddress["addressLine1"] = "Mondelani street";

            receivercontactInfo["mobilePhone"] = "081222111";
            receivercontactInfo["phone"] = "081222111";
            receivercontactInfo["companyName"] = "Unknown";
            receivercontactInfo["fullName"] = "Hilma Kandali  Uugwanga";
            receivercontactInfo["email"] = "21120255@sbs.ac.za";

            receiverDetails = new JsonObject {
                {"postalAddress",receiverPostalAddress },
                {"contactInformation",receivercontactInfo }
            };

            lineItemsquantity["unitOfMeasurement"] = "PCS";
            lineItemsquantity["value"] = Convert.ToInt32(1);

            lineItemsDataweight["netValue"] = Convert.ToDecimal(Convert.ToDecimal(436.5).ToString("N1"));
            lineItemsDataweight["grossValue"] = Convert.ToDecimal(Convert.ToDecimal(436.5).ToString("N1"));


            lineItemsData["number"] = Convert.ToInt32(1);
            lineItemsData["quantity"] = lineItemsquantity;
            lineItemsData["price"] = Convert.ToDecimal(Convert.ToDecimal(100).ToString("N2"));
            lineItemsData["description"] = "9780627034923 - How to Succeed in Your Master's and Doctoral Studies : a South African Guide and Resource Book";
            lineItemsData["weight"] = lineItemsDataweight;
            lineItemsData["manufacturerCountry"] = "KE";
            lineItems.Add(lineItemsData);


            lineItemsDatainvoice["date"] = "2023-12-27";
            lineItemsDatainvoice["number"] = "WB376664";


            exportDeclaration["lineItems"] = lineItems;
            exportDeclaration["invoice"] = lineItemsDatainvoice;

            dimensions["length"] = Convert.ToInt32(25);
            dimensions["width"] = Convert.ToInt32(10);
            dimensions["height"] = Convert.ToInt32(5);

            packages["weight"] = Convert.ToDecimal(Convert.ToDecimal(2.5).ToString("N1"));
            packages["dimensions"] = dimensions;


            content["exportDeclaration"] = exportDeclaration;
            content["unitOfMeasurement"] = "metric";
            content["isCustomsDeclarable"] = true;
            content["incoterm"] = "DAP";
            content["description"] = "Books - Educational | ";
            content["packages"] = new JsonArray { packages };
            content["declaredValueCurrency"] = "KES";
            content["declaredValue"] = Convert.ToDecimal(Convert.ToDecimal(200).ToString("N2"));


            shipmentObj["productCode"] = "P";
            shipmentObj["plannedShippingDateAndTime"] = DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss") + "GMT+00:00";
            shipmentObj["pickup"] = pickup;
            shipmentObj["accounts"] = new JsonArray { accounts };
            shipmentObj["customerDetails"] = new JsonObject { { "shipperDetails", shipperDetails }, { "receiverDetails", receiverDetails } };
            shipmentObj["content"] = content;

            string ss = shipmentObj.ToString();

            var response = api.ApiCallDHL("https://express.api.dhl.com/mydhlapi/test/", "shipments", "POST", shipmentObj.ToString());
            var result = await response.Content.ReadAsStringAsync();

            return Ok(result);

        }


        [HttpPost("Admin/Pickup")]
        public async Task<IActionResult> Pickup()
        {
            JsonObject pickupObj = new JsonObject();
            JsonObject accounts = new JsonObject();
            JsonObject accounts1 = new JsonObject();


            JsonObject shipperDetails = new JsonObject();
            JsonObject shipperPostalAddress = new JsonObject();
            JsonObject shippercontactInfo = new JsonObject();
            JsonObject pickupDetails = new JsonObject();
            JsonObject pickupPostalAddress = new JsonObject();
            JsonObject pickupcontactInfo = new JsonObject();
            JsonObject shipmentDetailsData = new JsonObject();
            JsonObject packages = new JsonObject();
            JsonObject dimensions = new JsonObject();


            accounts["typeCode"] = "shipper";
            accounts["number"] = "351341977";
            accounts1["typeCode"] = "shipper";
            accounts1["number"] = "351341977";


            shipperPostalAddress["postalCode"] = "";
            shipperPostalAddress["cityName"] = "Nairobi";
            shipperPostalAddress["countryCode"] = "KE";
            shipperPostalAddress["addressLine1"] = "DURUMA ROAD";
            shipperPostalAddress["addressLine2"] = "OFF KWALE ROAD";
            shipperPostalAddress["countyName"] = "Nairobi";


            shippercontactInfo["email"] = "tes2024@yopmail.com";
            shippercontactInfo["phone"] = "254711222333";
            shippercontactInfo["mobilePhone"] = "254711222333";
            shippercontactInfo["companyName"] = "AFRICA VENDORS HUB";
            shippercontactInfo["fullName"] = "TEST TESTER";

            shipperDetails = new JsonObject {
                { "postalAddress", shipperPostalAddress },
                { "contactInformation", shippercontactInfo }
            };


            pickupPostalAddress["cityName"] = "SWAKOPMUND";
            pickupPostalAddress["countryCode"] = "NA";
            pickupPostalAddress["provinceCode"] = "N/A";
            pickupPostalAddress["postalCode"] = "";
            pickupPostalAddress["addressLine1"] = "Mondesa Police Station, Mondelani street";

            pickupcontactInfo["mobilePhone"] = "081222111";
            pickupcontactInfo["phone"] = "081222111";
            pickupcontactInfo["companyName"] = "Unknown";
            pickupcontactInfo["fullName"] = "Hilma Kandali  Uugwanga";
            pickupcontactInfo["email"] = "21120255@sbs.ac.za";

            pickupDetails = new JsonObject {
                {"postalAddress",pickupPostalAddress },
                {"contactInformation",pickupcontactInfo }
            };

            dimensions["length"] = Convert.ToInt32(25);
            dimensions["width"] = Convert.ToInt32(10);
            dimensions["height"] = Convert.ToInt32(5);
            packages["weight"] = Convert.ToDecimal(Convert.ToDecimal(2.5).ToString("N1"));
            packages["dimensions"] = dimensions;

            shipmentDetailsData["productCode"] = "P";
            shipmentDetailsData["accounts"] = new JsonArray { accounts };
            shipmentDetailsData["isCustomsDeclarable"] = true;
            shipmentDetailsData["unitOfMeasurement"] = "metric";
            shipmentDetailsData["declaredValue"] = 5500;
            shipmentDetailsData["declaredValueCurrency"] = "KES";
            shipmentDetailsData["packages"] = new JsonArray { packages };


            pickupObj["plannedPickupDateAndTime"] = DateTimeOffset.Now.ToString("yyyy-MM-ddT") + "00:00:00GMT+00:00";
            pickupObj["accounts"] = new JsonArray { accounts1 };
            pickupObj["customerDetails"] = new JsonObject { { "shipperDetails", shipperDetails }, { "pickupDetails", pickupDetails } };
            pickupObj["shipmentDetails"] = new JsonArray { shipmentDetailsData };

            var response = api.ApiCallDHL("https://express.api.dhl.com/mydhlapi/test/", "pickups", "POST", pickupObj.ToString());
            var result = await response.Content.ReadAsStringAsync();

            return Ok(result);
        }
    }
}
