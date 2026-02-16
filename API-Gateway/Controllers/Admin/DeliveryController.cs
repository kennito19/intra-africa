using API_Gateway.Helper;
using API_Gateway.Models.Entity.User;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        BaseResponse<DeliveryLibrary> baseReponse = new BaseResponse<DeliveryLibrary>();
        public static string UserApi = string.Empty;
        private ApiHelper api;
        public DeliveryController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            UserApi = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            api = new ApiHelper(_httpContext);
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(DeliveryLibrary model)
        {
            var temp = api.ApiCall(UserApi, EndPoints.Delivery + "?countryid=" + model.CountryID + "&stateid=" + model.StateID + "&cityid=" + model.CityID + "&locality=" + model.Locality + "&pincode=" + model.Pincode, "GET", null);

            baseReponse = baseReponse.JsonParseList(temp);
            List<DeliveryLibrary> deliveries = (List<DeliveryLibrary>)baseReponse.Data;
            if (deliveries.Any())
            {
                baseReponse = baseReponse.AlreadyExists();
            }
            else
            {
                DeliveryLibrary delivery = new DeliveryLibrary();
                delivery.CityID = model.CityID;
                delivery.StateID = model.StateID;
                delivery.CountryID = model.CountryID;
                delivery.Locality = model.Locality;
                delivery.Pincode = model.Pincode;
                delivery.Status = "Active";
                delivery.IsCODActive = model.IsCODActive;
                delivery.DeliveryDays = model.DeliveryDays;
                delivery.CreatedAt = DateTime.Now;
                delivery.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value; ;

                var response = api.ApiCall(UserApi, EndPoints.Delivery, "POST", delivery);
                baseReponse = baseReponse.JsonParseInputResponse(response);
            }
            return Ok(baseReponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(DeliveryLibrary model)
        {
            var temp = api.ApiCall(UserApi, EndPoints.Delivery + "?countryid=" + model.CountryID + "&stateid=" + model.StateID + "&cityid=" + model.CityID + "&locality=" + model.Locality + "&pincode=" + model.Pincode, "GET", null);
            baseReponse = baseReponse.JsonParseList(temp);
            List<DeliveryLibrary> deliveries = (List<DeliveryLibrary>)baseReponse.Data;
            if (deliveries.Where(x => x.Id != model.Id).Any())
            {
                baseReponse = baseReponse.AlreadyExists();
            }
            else
            {
                temp = api.ApiCall(UserApi, EndPoints.Delivery + "?Id=" + model.Id, "GET", null);
                baseReponse = baseReponse.JsonParseRecord(temp);

                DeliveryLibrary delivery = (DeliveryLibrary)baseReponse.Data;
                //delivery.Id = model.Id;
                delivery.CityID = model.CityID;
                delivery.StateID = model.StateID;
                delivery.CountryID = model.CountryID;
                delivery.Locality = model.Locality;
                delivery.Pincode = model.Pincode;
                delivery.Status = model.Status;
                delivery.IsCODActive = model.IsCODActive;
                delivery.DeliveryDays = model.DeliveryDays;
                delivery.ModifiedAt = DateTime.Now;
                delivery.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                var response = api.ApiCall(UserApi, EndPoints.Delivery, "PUT", delivery);
                baseReponse = baseReponse.JsonParseInputResponse(response);
            }
            return Ok(baseReponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = api.ApiCall(UserApi, EndPoints.Delivery + "?Id=" + id, "GET", null);

            baseReponse = baseReponse.JsonParseList(temp);
            List<DeliveryLibrary> tempList = (List<DeliveryLibrary>)baseReponse.Data;

            if (tempList.Any())
            {
                var response = api.ApiCall(UserApi, EndPoints.Delivery + "?Id=" + id, "DELETE", null);
                baseReponse = baseReponse.JsonParseInputResponse(response);
            }
            else
            {
                baseReponse = baseReponse.NotExist();
            }

            return Ok(baseReponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(UserApi, EndPoints.Delivery + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseReponse.JsonParseList(response));
        }

        [HttpGet("byCityId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> GetByCityId(int id)
        {
            var response = api.ApiCall(UserApi, EndPoints.Delivery + "?cityid=" + id, "GET", null);
            return Ok(baseReponse.JsonParseList(response));
        }

        [HttpGet("byPincode")]
        [Authorize]
        public ActionResult<ApiHelper> GetByPincode(int pincode)
        {
            var response = api.ApiCall(UserApi, EndPoints.Delivery + "?pincode=" + pincode, "GET", null);
            return Ok(baseReponse.JsonParseRecord(response));
        }

        [HttpGet("byLocality")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> GetByLocality(string locality)
        {
            var response = api.ApiCall(UserApi, EndPoints.Delivery + "?locality=" + locality, "GET", null);
            return Ok(baseReponse.JsonParseList(response));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> Search(string? searchtext, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }
            var response = api.ApiCall(UserApi, EndPoints.Delivery + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseReponse.JsonParseList(response));
        }

        [HttpPost]
        [Route("Bulkupload")]
        public ActionResult<ApiHelper> Bulkupload(IFormFile file, string Mode)
        {
            if (file == null)
            {
                throw new Exception("File is Not Received...");
            }

            string FileName = Path.GetFileNameWithoutExtension(file.FileName);

            string extension = Path.GetExtension(file.FileName);
            string FullName = FileName + DateTime.Now.ToString("ddMMyyyyfffhhmsff") + extension;

            string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

            if (!allowedExtsnions.Contains(extension))
            {
                throw new Exception("Sorry! This file is not allowed,make sure that file having extension as either.xls or.xlsx is uploaded.");

            }


            if (!Directory.Exists("Resources" + "\\Excel" + "\\Delivery"))
            {
                Directory.CreateDirectory("Resources" + "\\Excel" + "\\Delivery");
            }
            var folderName = Path.Combine("Resources", "Excel", "Delivery");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);



            var fullPath = Path.Combine(pathToSave, FullName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }


            var response = api.ApiCall(UserApi, EndPoints.Delivery + "\\Bulkupload?fullPath=" + fullPath + "&Mode=" + Mode, "POST", null);

            System.IO.File.Delete(fullPath);


            return Ok(baseReponse.JsonParseInputResponse(response));

        }

        [HttpGet("downloadDelivery")]
        public ActionResult<ApiHelper> DownloadDelivery()
        {
            int rowCount = 0;
            List<int> totalCount = new List<int>();


            DataTable Delivery = new DataTable("Delivery");
            Delivery.Columns.Add("Country", typeof(string));
            Delivery.Columns.Add("State", typeof(string));
            Delivery.Columns.Add("City", typeof(string));
            Delivery.Columns.Add("Locality", typeof(string));
            Delivery.Columns.Add("Pincode", typeof(string));
            Delivery.Columns.Add("Delivery Days", typeof(string));
            Delivery.Columns.Add("COD Active", typeof(string));


            DataTable Delivery_Data = new DataTable();
            Delivery_Data.Columns.Add("Country", typeof(string));
            Delivery_Data.Columns.Add("State", typeof(string));
            Delivery_Data.Columns.Add("City", typeof(string));
            Delivery_Data.Columns.Add("COD Active", typeof(string));

            rowCount = 0;

            // Get City data
            var ResCity = api.ApiCall(UserApi, EndPoints.City + "?PageIndex=0", "GET", null);
            BaseResponse<CityLibrary> baseCity = new BaseResponse<CityLibrary>();
            baseCity = baseCity.JsonParseList(ResCity);
            List<CityLibrary> lstCity = (List<CityLibrary>)baseCity.Data;

            List<CityLibrary> lstCountry = lstCity.GroupBy(c => c.CountryID).Select(p => p.FirstOrDefault()).ToList();
            List<CityLibrary> lstState = lstCity.GroupBy(c => c.StateID).Select(p => p.FirstOrDefault()).ToList();


            totalCount.Add(lstCountry.Count());

            foreach (var item in lstCountry)
            {
                Delivery_Data.Rows.Add();
                Delivery_Data.Rows[rowCount]["Country"] = item.CountryName;
                rowCount++;
            }


            // Get state data
            //var ResState = api.ApiCall(UserApi, EndPoints.State + "?PageIndex=1", "GET", null, token);
            //BaseResponse<StateLibrary> baseState = new BaseResponse<StateLibrary>();
            //baseState = baseState.JsonParseList(ResState);
            //List<StateLibrary> lstState = (List<StateLibrary>)baseState.Data;

            rowCount = 0;

            totalCount.Add(lstState.Count());

            foreach (var item in lstState)
            {
                if (rowCount >= Delivery_Data.Rows.Count)
                {
                    Delivery_Data.Rows.Add();
                }
                Delivery_Data.Rows[rowCount]["State"] = item.StateName;
                rowCount++;
            }

            rowCount = 0;

            totalCount.Add(lstCity.Count());

            foreach (var item in lstCity)
            {
                if (rowCount >= Delivery_Data.Rows.Count)
                {
                    Delivery_Data.Rows.Add();
                }
                Delivery_Data.Rows[rowCount]["City"] = item.Name;
                rowCount++;
            }

            totalCount.Add(2);
            Delivery_Data.Rows[0]["COD Active"] = "True";
            Delivery_Data.Rows[1]["COD Active"] = "False";


            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(Delivery, "Delivery");
                var ws1 = wb.Worksheets.Add(Delivery_Data, "Delivery_Data");
                ws1.Protect("sdlkfjo4u3890ufrjv389g934v934938c4kj897349vcj3976");


                var DeliveryRange = ws.RangeUsed();

                var DeliveryDataRange = ws1.RangeUsed();

                int count = 0;

                ws.Tables.FirstOrDefault().ShowAutoFilter = false;
                ws1.Tables.FirstOrDefault().ShowAutoFilter = false;


                foreach (DataColumn column in Delivery_Data.Columns)
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
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Delivery.xlsx");
                }
            }
        }
    }
}
