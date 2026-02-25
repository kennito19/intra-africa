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
    public class CityController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper api;
        private readonly IConfiguration _configuration;
        BaseResponse<CityLibrary> baseResponse = new BaseResponse<CityLibrary>();
        public static string UserApi = string.Empty;

        public CityController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            UserApi = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            api = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(CityLibrary model)
        {
            var temp = api.ApiCall(UserApi, EndPoints.City + "?name=" + model.Name + "&countryId=" + model.CountryID + "&stateId=" + model.StateID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<CityLibrary> tempList = baseResponse.Data as List<CityLibrary> ?? new List<CityLibrary>();
            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                CityLibrary city = new CityLibrary();
                city.Id = 0;
                city.StateID = model.StateID;
                city.CountryID = model.CountryID;
                city.Name = model.Name;
                city.Status = model.Status;
                city.CreatedAt = DateTime.Now;
                city.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                var response = api.ApiCall(UserApi, EndPoints.City, "POST", city);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(CityLibrary model)
        {
            var temp = api.ApiCall(UserApi, EndPoints.City + "?Name=" + model.Name + "&countryId=" + model.CountryID + "&stateId=" + model.StateID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<CityLibrary> tempList = baseResponse.Data as List<CityLibrary> ?? new List<CityLibrary>();
            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var responseMessage = api.ApiCall(UserApi, EndPoints.City + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(responseMessage);
                var record = baseResponse.Data as CityLibrary;
                record.Name = model.Name;
                record.Status = model.Status;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                record.ModifiedAt = DateTime.Now;

                var response = api.ApiCall(UserApi, EndPoints.City, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int Id)
        {
            var tempCity = api.ApiCall(UserApi, EndPoints.City + "?id=" + Id, "GET", null);

            baseResponse = baseResponse.JsonParseList(tempCity);
            List<CityLibrary> tempList = baseResponse.Data as List<CityLibrary> ?? new List<CityLibrary>();

            if (tempList.Any())
            {
                var response = api.ApiCall(UserApi, EndPoints.City + "?id=" + Id, "DELETE", null);
                return Ok(baseResponse.JsonParseInputResponse(response));
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet("byStateId")]
        [Authorize]
        public ActionResult<ApiHelper> GetByStateId(int id, int? pageIndex = 1, int? pageSize = 10)
        {
            var response = api.ApiCall(UserApi, EndPoints.City + "?stateId=" + id + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("byStateandCountryIds")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public ActionResult<ApiHelper> GetbyStateandCountryIds(string? stateIds = null, string? countryIds = null, int? PageIndex = 1, int? PageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(stateIds) && stateIds != "")
            {
                url += "&stateIds=" + stateIds;
            }
            if (!string.IsNullOrEmpty(countryIds) && countryIds != "")
            {
                url += "&countryIds=" + countryIds;
            }

            var response = api.ApiCall(UserApi, EndPoints.City + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        [HttpGet("search")]
        [Authorize]
        public ActionResult<ApiHelper> Search(string? searchtext, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }
            var response = api.ApiCall(UserApi, EndPoints.City + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
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


            if (!System.IO.Directory.Exists("Resources" + "\\Excel" + "\\Delivery"))
            {
                System.IO.Directory.CreateDirectory("Resources" + "\\Excel" + "\\Delivery");
            }
            var folderName = Path.Combine("Resources", "Excel", "Delivery");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);



            var fullPath = Path.Combine(pathToSave, FullName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }


            var response = api.ApiCall(UserApi, EndPoints.City + "\\Bulkupload?fullPath=" + fullPath + "&Mode=" + Mode, "POST", null);

            System.IO.File.Delete(fullPath);


            return Ok(baseResponse.JsonParseInputResponse(response));

        }

        [HttpGet("downloadCity")]
        public ActionResult<ApiHelper> DownloadCity()
        {
            int rowCount = 0;
            List<int> totalCount = new List<int>();


            DataTable City = new DataTable("City");
            City.Columns.Add("Country", typeof(string));
            City.Columns.Add("State", typeof(string));
            City.Columns.Add("City", typeof(string));


            DataTable City_Data = new DataTable();
            City_Data.Columns.Add("Country", typeof(string));
            City_Data.Columns.Add("State", typeof(string));


            // Get country data
            //var ResCountry = api.ApiCall(UserApi, EndPoints.Country + "?PageIndex=1", "GET", null, token);
            //BaseResponse<CountryLibrary> baseCountry = new BaseResponse<CountryLibrary>();
            //baseCountry = baseCountry.JsonParseList(ResCountry);
            //List<CountryLibrary> lstCountry = baseCountry.Data as List<CountryLibrary> ?? new List<CountryLibrary>();

            // Get state data
            var ResState = api.ApiCall(UserApi, EndPoints.State + "?PageIndex=1", "GET", null);
            BaseResponse<StateLibrary> baseState = new BaseResponse<StateLibrary>();
            baseState = baseState.JsonParseList(ResState);
            List<StateLibrary> lstState = baseState.Data as List<StateLibrary> ?? new List<StateLibrary>();

            List<StateLibrary> lstCountry = lstState.GroupBy(c => c.CountryID).Select(p => p.FirstOrDefault()).ToList();


            totalCount.Add(lstCountry.Count());

            foreach (var item in lstCountry)
            {
                City_Data.Rows.Add();
                City_Data.Rows[rowCount]["Country"] = item.CountryName;
                rowCount++;
            }


            rowCount = 0;

            totalCount.Add(lstState.Count());

            foreach (var item in lstState)
            {
                if (rowCount >= City_Data.Rows.Count)
                {
                    City_Data.Rows.Add();
                }
                City_Data.Rows[rowCount]["State"] = item.Name;
                rowCount++;
            }



            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add(City, "City");
                var ws1 = wb.Worksheets.Add(City_Data, "City_Data");
                ws1.Protect("nmvrekjnvo548y94ghv9eorhg8945tgjo0pwvmj95y43trj");


                var CityRange = ws.RangeUsed();

                var CityDataRange = ws1.RangeUsed();

                int count = 0;

                ws.Tables.FirstOrDefault().ShowAutoFilter = false;
                ws1.Tables.FirstOrDefault().ShowAutoFilter = false;


                foreach (DataColumn column in City_Data.Columns)
                {
                    string[] ColumnName = new string[] { column.ToString() };

                    var Citycell = CityRange.CellsUsed(c => c.Value.ToString() == ColumnName[0].ToString()).FirstOrDefault();
                    var CityDatacell = CityDataRange.CellsUsed(c => c.Value.ToString() == column.ToString()).FirstOrDefault();

                    string CityRangLetter = "";

                    string CityDataRangLetter = "";


                    if (Citycell != null)
                    {
                        CityRangLetter = Citycell.WorksheetColumn().ColumnLetter().ToString();
                    }
                    if (CityDatacell != null)
                    {
                        CityDataRangLetter = CityDatacell.WorksheetColumn().ColumnLetter().ToString();
                    }

                    if (CityRangLetter != "" && CityRangLetter != null)
                    {

                        if (totalCount[count].ToString() != "0")
                        {
                            ws.Cell(CityRangLetter + "2").SetDataValidation().List(ws1.Range("Filter!" + CityDataRangLetter + "2" + ":" + CityDataRangLetter + (Convert.ToInt32(totalCount[count]) + 1)));

                        }
                    }

                    count++;
                }



                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "City.xlsx");
                }
            }

        }
    }



}
