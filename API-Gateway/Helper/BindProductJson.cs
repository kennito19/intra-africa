using Newtonsoft.Json.Linq;

namespace API_Gateway.Helper
{
    public class BindProductJson
    {
        public static string CatalogueUrl = string.Empty;
        public string userURL = string.Empty;
        private readonly IConfiguration _configuration;

        public BindProductJson(IConfiguration configuration)
        {

            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            userURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
        }
        public string Productjson()
        {

            JObject productInfo = new JObject();
            JObject product = new JObject();
            JObject taxValue = new JObject();
            JObject HSNCode = new JObject();
            JObject seller = new JObject();
            JObject brand = new JObject();
            JObject category = new JObject();
            JObject productWarehouse = new JObject();
            JObject livestatus = new JObject();
            JObject staus = new JObject();


            JArray livedata = new JArray();


            var st = new statusList();
            staus["data"] = JToken.FromObject(st.bindStaus());

            JObject live = new JObject();
            live["text"] = "Yes";
            live["value"] = "true";
            livedata.Add(live);

            JObject live1 = new JObject();
            live1["text"] = "No";
            live1["value"] = "false";
            livedata.Add(live1);

            livestatus["data"] = livedata;

            taxValue["bindlist"] = CatalogueUrl + EndPoints.TaxTypeValue;
            taxValue["selected_value"] = "";
            taxValue["type"] = "dropdown";

            HSNCode["bindlist"] = CatalogueUrl + EndPoints.HSNCode;
            HSNCode["selected_value"] = "";
            HSNCode["type"] = "dropdown";

            seller["bindlist"] = "";
            seller["selected_value"] = "";
            seller["type"] = "dropdown";

            brand["bindlist"] = userURL + EndPoints.Brand + "?SellerID={sellerid}";
            brand["selected_value"] = "";
            brand["type"] = "dropdown";

            category["bindlist"] = CatalogueUrl + EndPoints.Category;
            category["selected_value"] = "";
            category["type"] = "dropdown";

            productWarehouse["bindlist"] = userURL + EndPoints.Warehouse + "?SellerID={sellerid}";
            productWarehouse["selected_value"] = "";
            productWarehouse["type"] = "dropdown";

            product["productid"] = "null";
            product["productGuid"] = "";
            product["categoryId"] = "";
            product["assignCategoryId"] = "";
            product["productName"] = "";
            product["customeProductName"] = "";
            product["companySkuCode"] = "";
            product["longDescription"] = "";
            product["highlights"] = "";
            product["keywords"] = "";
            product["productLength"] = "";
            product["productBreadth"] = "";
            product["productWeight"] = "";
            product["productHeight"] = "";
            product["packingLength"] = "";
            product["packingBreadth"] = "";
            product["packingHeight"] = "";
            product["packingWeight"] = "";
            product["isMasterProduct"] = "";
            product["parentProductId"] = "";
            product["isExistingProduct"] = "";
            product["live"] = livestatus;
            product["status"] = staus;
            product["sellerProductID"] = "";
            product["mrp"] = "";
            product["sellingPrice"] = "";
            product["discount"] = "";
            product["quantity"] = "";
            product["sizeID"] = "";
            product["sellerWiseProductPriceMasterID"] = "";


            product["tax"] = taxValue;
            product["hsn"] = HSNCode;
            product["seller"] = seller;
            product["brand"] = brand;
            product["category"] = category;
            product["warehouse"] = productWarehouse;

            //JArray productarray = new JArray();
            //productarray.Add(product);

            productInfo["productinfo"] = product;
            return productInfo.ToString();
        }
    }

    public class statusList
    {
        public string value { get; set; }
        public string text { get; set; }

        public List<statusList> bindStaus()
        {
            List<statusList> status = new List<statusList>(){

                new statusList(){ value = "Active", text="Active"},
                new statusList(){ value = "Request for Approval", text="Request for Approval"},
                new statusList(){ value = "Bulk Stock Upload", text="Bulk Stock Upload"},
                new statusList(){ value = "SKU Missing", text="SKU Missing"},
                new statusList(){ value = "Poor Image Quality", text="Poor Image Quality"},
                new statusList(){ value = "Incorrect Category", text="Incorrect Category"},
                new statusList(){ value = "Pricing-Very High", text="Pricing-Very High"},
                new statusList(){ value = "Pricing-Very Low", text="Pricing-Very Low"},
                new statusList(){ value = "Pricing-High Discount", text="Pricing-High Discount"},
                new statusList(){ value = "Restricted Image", text="Restricted Image"},
                new statusList(){ value = "Restricted Keyword", text="Restricted Keyword"},
                new statusList(){ value = "Feature Missing", text="Feature Missing"},
                new statusList(){ value = "High Returns", text="High Returns"},
                new statusList(){ value = "Deleted", text="Deleted"},
                new statusList(){ value = "Multiple Issues", text="Multiple Issues"}
            };

            return status;

        }

    }



}
