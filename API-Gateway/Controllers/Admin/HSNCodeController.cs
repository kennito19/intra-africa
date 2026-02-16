using API_Gateway.Helper;
using API_Gateway.Models.Entity;
using API_Gateway.Models.Entity.Catalogue;
using ClosedXML.Excel;
//using DocumentFormat.OpenXml.Drawing;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class HSNCodeController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public static string CatalogueUrl = string.Empty;
        BaseResponse<HSNCodeLibrary> baseResponse = new BaseResponse<HSNCodeLibrary>();
        private ApiHelper api;
        public HSNCodeController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            api = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(HSNCodeLibrary model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.HSNCode + "?HSNCode=" + model.HSNCode, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<HSNCodeLibrary> tempList = (List<HSNCodeLibrary>)baseResponse.Data;

            if (tempList.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                HSNCodeLibrary hsn = new HSNCodeLibrary();
                hsn.HSNCode = model.HSNCode;
                hsn.Description = model.Description;
                hsn.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = api.ApiCall(CatalogueUrl, EndPoints.HSNCode, "POST", hsn);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(HSNCodeLibrary model)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.HSNCode + "?HSNCode=" + model.HSNCode, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<HSNCodeLibrary> tempList = (List<HSNCodeLibrary>)baseResponse.Data;

            if (tempList.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var recordCall = api.ApiCall(CatalogueUrl, EndPoints.HSNCode + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(recordCall);
                HSNCodeLibrary record = (HSNCodeLibrary)baseResponse.Data;
                record.HSNCode = model.HSNCode;
                record.Description = model.Description;
                record.ModifiedAt = DateTime.Now;
                record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = api.ApiCall(CatalogueUrl, EndPoints.HSNCode, "PUT", record);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.HSNCode + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<HSNCodeLibrary> tempList = (List<HSNCodeLibrary>)baseResponse.Data;

            if (tempList.Any())
            {
                var temptaxratetohsncode = api.ApiCall(CatalogueUrl, EndPoints.AssignTaxRateToHSNCode + "?hsncodeId=" + id, "GET", null);
                BaseResponse<AssignTaxRateToHSNCode> baseResTaxRateToHsnCode = new BaseResponse<AssignTaxRateToHSNCode>();
                var TaxRateToHsnCodeResponse = baseResTaxRateToHsnCode.JsonParseList(temptaxratetohsncode);
                List<AssignTaxRateToHSNCode> taxRateToHSNCodes = (List<AssignTaxRateToHSNCode>)TaxRateToHsnCodeResponse.Data;

                var productHsncode = api.ApiCall(CatalogueUrl, EndPoints.Product + "?HSNCode=" + tempList[0].HSNCode, "Get", null);
                BaseResponse<Products> baseProductResponse = new BaseResponse<Products>();
                var ProudctBaseresponse = baseProductResponse.JsonParseList(productHsncode);
                List<Products> products = (List<Products>)ProudctBaseresponse.Data;
                if (taxRateToHSNCodes.Any() || products.Any())
                {
                    if (taxRateToHSNCodes.Any())
                    {
                        baseResponse = baseResponse.ChildAlreadyExists("TaxRateToHSNCode", "HsnCode");
                    }
                    if (products.Any())
                    {
                        baseResponse = baseResponse.ChildAlreadyExists("Products", "HsnCode");
                    }
                }
                else
                {
                    var response = api.ApiCall(CatalogueUrl, EndPoints.HSNCode + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> Search(string? searchtext = null, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&Searchtext=" + HttpUtility.UrlEncode(searchtext);
            }
            var response = api.ApiCall(CatalogueUrl, EndPoints.HSNCode + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }



        [HttpPost]
        [Route("Bulkupload")]
        public ActionResult<ApiHelper> Bulkupload(IFormFile file)
        {
            if (file == null)
            {
                throw new Exception("File is Not Received...");
            }

            string dataFileName = Path.GetFileName(file.FileName);

            string extension = Path.GetExtension(dataFileName);

            string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

            if (!allowedExtsnions.Contains(extension))
            {
                throw new Exception("Sorry! This file is not allowed,make sure that file having extension as either.xls or.xlsx is uploaded.");

            }


            if (!System.IO.Directory.Exists("Resources" + "\\Excel" + "\\HSN"))
            {
                System.IO.Directory.CreateDirectory("Resources" + "\\Excel" + "\\HSN");
            }
            var folderName = Path.Combine("Resources", "Excel", "HSN");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);



            var fullPath = Path.Combine(pathToSave, dataFileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }


            var response = api.ApiCall(CatalogueUrl, EndPoints.HSNCode + "\\Bulkupload?fullPath=" + fullPath, "POST", null);

            System.IO.File.Delete(fullPath);


            return Ok(baseResponse.JsonParseInputResponse(response));

        }


        [HttpGet("downloadHSN")]
        public ActionResult<ApiHelper> DownloadHSN()
        {
            int rowCount = 0;
            List<int> totalCount = new List<int>();


            DataTable HSN = new DataTable("HSN");
            HSN.Columns.Add("HSNCode", typeof(string));
            HSN.Columns.Add("Description", typeof(string));
            HSN.Columns.Add("Tax Type", typeof(string));
            HSN.Columns.Add("Tax%", typeof(string));


            DataTable HSN_Data = new DataTable();
            HSN_Data.Columns.Add("Tax Type", typeof(string));
            HSN_Data.Columns.Add("Tax%", typeof(string));



            var tempRes_Tax = api.ApiCall(CatalogueUrl, EndPoints.TaxType + "?PageIndex=1", "GET", null);
            BaseResponse<TaxTypeLibrary> basetax = new BaseResponse<TaxTypeLibrary>();
            basetax = basetax.JsonParseList(tempRes_Tax);
            List<TaxTypeLibrary> tempTax = (List<TaxTypeLibrary>)basetax.Data;

            tempTax = tempTax.Where(p => p.ParentId == 0 || p.ParentId == null).ToList();


            totalCount.Add(tempTax.Count());

            foreach (var item in tempTax)
            {
                HSN_Data.Rows.Add();
                HSN_Data.Rows[rowCount]["Tax Type"] = item.TaxType;
                rowCount++;
            }



            var tempRes_TaxValue = api.ApiCall(CatalogueUrl, EndPoints.TaxTypeValue + "?PageIndex=1", "GET", null);
            BaseResponse<TaxTypeValueLibrary> basetaxValue = new BaseResponse<TaxTypeValueLibrary>();
            basetaxValue = basetaxValue.JsonParseList(tempRes_TaxValue);
            List<TaxTypeValueLibrary> tempTaxValue = (List<TaxTypeValueLibrary>)basetaxValue.Data;

            rowCount = 0;

            totalCount.Add(tempTaxValue.Count());

            foreach (var item in tempTaxValue)
            {
                if (tempTaxValue.Count() > HSN_Data.Rows.Count)
                {
                    HSN_Data.Rows.Add();
                }
                HSN_Data.Rows[rowCount]["Tax%"] = item.Name;
                rowCount++;
            }



            using (XLWorkbook wb = new XLWorkbook())
            {
                //wb.Worksheets.Add(HSN, "HSN");

                var ws = wb.Worksheets.Add(HSN, "HSN");
                var ws1 = wb.Worksheets.Add(HSN_Data, "HSN_Data");
                ws1.Protect("872346hfsdjk0jslfksddioc89w6e7r78");


                var TaxRange = ws.RangeUsed();

                var TaxDataRange = ws1.RangeUsed();

                int count = 0;

                ws.Tables.FirstOrDefault().ShowAutoFilter = false;
                ws1.Tables.FirstOrDefault().ShowAutoFilter = false;


                foreach (DataColumn column in HSN_Data.Columns)
                {
                    string[] ColumnName = new string[] { column.ToString() };

                    var Taxcell = TaxRange.CellsUsed(c => c.Value.ToString() == ColumnName[0].ToString()).FirstOrDefault();
                    var TaxDatacell = TaxDataRange.CellsUsed(c => c.Value.ToString() == column.ToString()).FirstOrDefault();

                    string TaxRangLetter = "";

                    string TaxDataRangLetter = "";


                    if (Taxcell != null)
                    {
                        TaxRangLetter = Taxcell.WorksheetColumn().ColumnLetter().ToString();
                    }
                    if (TaxDatacell != null)
                    {
                        TaxDataRangLetter = TaxDatacell.WorksheetColumn().ColumnLetter().ToString();
                    }

                    if (TaxRangLetter != "" && TaxRangLetter != null)
        {

                        if (totalCount[count].ToString() != "0")
            {
                            ws.Cell(TaxRangLetter + "2").SetDataValidation().List(ws1.Range("Filter!" + TaxDataRangLetter + "2" + ":" + TaxDataRangLetter + (Convert.ToInt32(totalCount[count]) + 1)));

                        }
            }


                    count++;
        }



                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "HSN.xlsx");
                }
            }

    }
}
}
