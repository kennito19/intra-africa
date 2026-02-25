using API_Gateway.Helper;
using API_Gateway.Models.Entity.Catalogue;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Web;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class WeightSlabController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        BaseResponse<WeightSlabLibrary> baseResponse = new BaseResponse<WeightSlabLibrary>();

        public WeightSlabController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            URL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Save(WeightSlabLibrary model)
        {
            //var temp = helper.ApiCall(URL, EndPoints.WeightSlab + "?WeightSlab=" + model.WeightSlab, "GET", null);
            var temp = helper.ApiCall(URL, EndPoints.WeightSlab + "?PageIndex=0&PageSize=0", "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<WeightSlabLibrary> weight = baseResponse.Data as List<WeightSlabLibrary> ?? new List<WeightSlabLibrary>();
            if (weight.Any())
            {
                if (weight.Where(x => x.WeightSlab == model.WeightSlab).ToList().Count > 0)
                {
                    baseResponse = baseResponse.AlreadyExists();
                }
                else
                {
                    string[] value = model.WeightSlab.Split('-');
                    int count = 0;
                    List<slabs> slablst = new List<slabs>();
                    foreach (WeightSlabLibrary item in weight)
                    {
                        string[] itemValue = item.WeightSlab.Split('-');

                        slabs slab = new slabs();
                        slab.fromslab = Convert.ToDecimal(itemValue[0]);
                        slab.toslab = Convert.ToDecimal(itemValue[1]);
                        slablst.Add(slab);
                    }
                    if (slablst.Count > 0)
                    {
                        var data = slablst.Where(p => p.toslab >= Convert.ToDecimal(value[0]) && p.fromslab <= Convert.ToDecimal(value[1])).ToList();
                        if (data.Any())
                        {
                            baseResponse = baseResponse.AlreadyExists();
                        }
                        else
                        {
                            WeightSlabLibrary ws = new WeightSlabLibrary();
                            ws.WeightSlab = model.WeightSlab;
                            ws.LocalCharges = model.LocalCharges;
                            ws.ZonalCharges = model.ZonalCharges;
                            ws.NationalCharges = model.NationalCharges;
                            ws.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                            var response = helper.ApiCall(URL, EndPoints.WeightSlab, "POST", ws);
                            baseResponse = baseResponse.JsonParseInputResponse(response);
                        }
                    }
                    else
                    {
                        WeightSlabLibrary ws = new WeightSlabLibrary();
                        ws.WeightSlab = model.WeightSlab;
                        ws.LocalCharges = model.LocalCharges;
                        ws.ZonalCharges = model.ZonalCharges;
                        ws.NationalCharges = model.NationalCharges;
                        ws.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                        var response = helper.ApiCall(URL, EndPoints.WeightSlab, "POST", ws);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }


                }
            }
            else
            {
                WeightSlabLibrary ws = new WeightSlabLibrary();
                ws.WeightSlab = model.WeightSlab;
                ws.LocalCharges = 0;
                ws.ZonalCharges = 0;
                ws.NationalCharges = 0;
                ws.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = helper.ApiCall(URL, EndPoints.WeightSlab, "POST", ws);
                baseResponse = baseResponse.JsonParseInputResponse(response);

            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(WeightSlabLibrary model)
        {
            var temp = helper.ApiCall(URL, EndPoints.WeightSlab + "?PageIndex=0&PageSize=0", "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<WeightSlabLibrary> weight = baseResponse.Data as List<WeightSlabLibrary> ?? new List<WeightSlabLibrary>();
            if (weight.Any())
            {
                if (weight.Where(x => x.WeightSlab == model.WeightSlab && x.Id != model.Id).ToList().Count > 0)
                {
                    baseResponse = baseResponse.AlreadyExists();
                }
                else
                {
                    string[] value = model.WeightSlab.Split('-');
                    int count = 0;
                    List<slabs> slablst = new List<slabs>();
                    foreach (WeightSlabLibrary item in weight.Where(x => x.Id != model.Id).ToList())
                    {
                        string[] itemValue = item.WeightSlab.Split('-');

                        slabs slab = new slabs();
                        slab.fromslab = Convert.ToDecimal(itemValue[0]);
                        slab.toslab = Convert.ToDecimal(itemValue[1]);
                        slablst.Add(slab);
                    }
                    if (slablst.Count > 0)
                    {
                        var data = slablst.Where(p => p.toslab >= Convert.ToDecimal(value[0]) && p.fromslab <= Convert.ToDecimal(value[1])).ToList();
                        if (data.Any())
                        {
                            baseResponse = baseResponse.AlreadyExists();
                        }
                        else
                        {
                            WeightSlabLibrary record = new WeightSlabLibrary();
                            record = weight.Where(x => x.Id == model.Id).FirstOrDefault();
                            record.Id = model.Id;
                            record.WeightSlab = model.WeightSlab;
                            record.LocalCharges = model.LocalCharges;
                            record.ZonalCharges = model.ZonalCharges;
                            record.NationalCharges = model.NationalCharges;
                            record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                            var response = helper.ApiCall(URL, EndPoints.WeightSlab, "PUT", record);
                            baseResponse = baseResponse.JsonParseInputResponse(response);
                        }
                    }
                    else
                    {
                        WeightSlabLibrary record = new WeightSlabLibrary();
                        record = weight.Where(x => x.Id == model.Id).FirstOrDefault();
                        record.Id = model.Id;
                        record.WeightSlab = model.WeightSlab;
                        record.LocalCharges = model.LocalCharges;
                        record.ZonalCharges = model.ZonalCharges;
                        record.NationalCharges = model.NationalCharges;
                        record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                        var response = helper.ApiCall(URL, EndPoints.WeightSlab, "PUT", record);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }


                }
            }
            return Ok(baseResponse);


            //var temp = helper.ApiCall(URL, EndPoints.WeightSlab + "?WeightSlab=" + model.WeightSlab, "GET", null);
            //baseResponse = baseResponse.JsonParseList(temp);
            //List<WeightSlabLibrary> templist = baseResponse.Data as List<WeightSlabLibrary> ?? new List<WeightSlabLibrary>();
            //if (templist.Where(x => x.Id != model.Id).Any())
            //{
            //    baseResponse = baseResponse.AlreadyExists();
            //}
            //else
            //{
            //    WeightSlabLibrary record = new WeightSlabLibrary();
            //    record.Id = model.Id;
            //    record.WeightSlab = model.WeightSlab;
            //    record.LocalCharges = model.LocalCharges;
            //    record.ZonalCharges = model.ZonalCharges;
            //    record.NationalCharges = model.NationalCharges;
            //    record.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            //    var response = helper.ApiCall(URL, EndPoints.WeightSlab, "PUT", record,  token);
            //    baseResponse = baseResponse.JsonParseInputResponse(response);
            //}
            //return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = helper.ApiCall(URL, EndPoints.WeightSlab + "?id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<WeightSlabLibrary> templist = baseResponse.Data as List<WeightSlabLibrary> ?? new List<WeightSlabLibrary>();
            if (templist.Any())
            {
                var tempSellerProduct = helper.ApiCall(URL, EndPoints.SellerProduct + "?WeightSlabId=" + id, "GET", null);
                BaseResponse<SellerProduct> baseSellerProduct = new BaseResponse<SellerProduct>();
                var sellerProduct = baseSellerProduct.JsonParseList(tempSellerProduct);
                List<SellerProduct> sellerProducts = sellerProduct.Data as List<SellerProduct> ?? new List<SellerProduct>();
                if (sellerProducts.Any())
                {
                    baseResponse = baseResponse.ChildAlreadyExists("SellerProduct", "WeightSlab");
                }
                else
                {
                    var response = helper.ApiCall(URL, EndPoints.WeightSlab + "?Id=" + id, "DELETE", null);
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

            var response = helper.ApiCall(URL, EndPoints.WeightSlab + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return Ok(baseResponse.JsonParseList(response));
        }

        public class slabs
        {
            public decimal? fromslab { get; set; } = null;
            public decimal? toslab { get; set; } = null;
        }
    }
}
