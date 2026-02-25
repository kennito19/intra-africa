using API_Gateway.Common.products;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger;
using System.Data;

namespace API_Gateway.Controllers.Admin
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryWiseCommissionController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;

        private readonly IConfiguration _configuration;
        private ApiHelper api;
        public static string CatalogueUrl = string.Empty;
        public static string IdServerUrl = string.Empty;
        public static string UserUrl = string.Empty;
        BaseResponse<CommissionChargesLibrary> baseResponse = new BaseResponse<CommissionChargesLibrary>();

        public CategoryWiseCommissionController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            api = new ApiHelper(_httpContext);

            _configuration = configuration;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IdServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            UserUrl = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Create(CommissionDTO model)
        {
            string url = string.Empty;
            //if (model.CatID == null || model.CatID == 0)
            //{
            //    url = "?chargeson=" + model.ChargesOn + "&onlyCategory =" + true;
            //}
            //else
            //{
            //}
            url = "?CategoryID=" + model.CatID + "&onlyCategory=" + true;
            var temp = api.ApiCall(CatalogueUrl, EndPoints.CommissionCharges + url, "GET", null);

            baseResponse = baseResponse.JsonParseList(temp);
            List<CommissionChargesLibrary> charges = baseResponse.Data as List<CommissionChargesLibrary> ?? new List<CommissionChargesLibrary>();

            if (charges.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                CommissionChargesLibrary commission = new CommissionChargesLibrary();
                commission.CatID = model.CatID;
                commission.SellerID = null;
                commission.BrandID = null;
                commission.AmountValue = model.AmountValue;
                commission.IsCompulsary = false;
                commission.ChargesOn = "Specific Category";
                commission.ChargesIn = "Percentage";
                commission.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = api.ApiCall(CatalogueUrl, EndPoints.CommissionCharges, "POST", commission);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Update(CommissionDTO model)
        {
            string url = string.Empty;
            //if (model.CatID == null || model.CatID == 0)
            //{
            //    url = "?chargeson=" + model.ChargesOn + "&onlyCategory=" + true;
            //}
            //else
            //{
            //}
            url = "?CategoryID=" + model.CatID + "&onlyCategory=" + true;
            var temp = api.ApiCall(CatalogueUrl, EndPoints.CommissionCharges + url, "GET", null);

            baseResponse = baseResponse.JsonParseList(temp);
            List<CommissionChargesLibrary> charges = baseResponse.Data as List<CommissionChargesLibrary> ?? new List<CommissionChargesLibrary>();
            var tmp = charges.Where(x => x.ID != model.ID).FirstOrDefault();

            if (tmp != null)
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                CommissionChargesLibrary commission = new CommissionChargesLibrary();
                commission.ID = model.ID;
                commission.CatID = model.CatID;
                commission.SellerID = null;
                commission.BrandID = null;
                commission.AmountValue = model.AmountValue;
                commission.IsCompulsary = false;
                commission.ChargesOn = "Specific Category";
                commission.ChargesIn = "Percentage";
                commission.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                var response = api.ApiCall(CatalogueUrl, EndPoints.CommissionCharges, "PUT", commission);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult<ApiHelper> Delete(int id)
        {
            var temp = api.ApiCall(CatalogueUrl, EndPoints.CommissionCharges + "?ID=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<CommissionChargesLibrary> charges = baseResponse.Data as List<CommissionChargesLibrary> ?? new List<CommissionChargesLibrary>();

            if (!charges.Any())
            {
                baseResponse = baseResponse.NotExist();
            }
            else
            {
                BaseResponse<Products> probaseResponse = new BaseResponse<Products>();
                var GetResponse = api.ApiCall(CatalogueUrl, EndPoints.Product + "?PathIds=" + charges[0].CatID, "GET", null);
                probaseResponse = probaseResponse.JsonParseList(GetResponse);
                List<Products> proemplist = probaseResponse.Data as List<Products>;
                if (proemplist.Any())
                {
                    GetCommissionCharges commissionCharges = new GetCommissionCharges(_httpContextAccessor);
                    bool _availbaleInCategory = commissionCharges.AvailableCommissionInSpecificCategory(Convert.ToInt32(charges[0].CatID), CatalogueUrl,true);
                    if (!_availbaleInCategory)
                    {
                        //error
                        baseResponse.code = 204;
                        baseResponse.Message = "At least one Commission is required on Category level.";
                        baseResponse.Data = null;
                    }
                    else
                    {
                        var response = api.ApiCall(CatalogueUrl, EndPoints.CommissionCharges + "?Id=" + id, "Delete", null);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                }
                else
                {
                    var response = api.ApiCall(CatalogueUrl, EndPoints.CommissionCharges + "?Id=" + id, "Delete", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            return Ok(baseResponse);
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller")]
        public ActionResult<ApiHelper> BySearch(string? searchText = null, int pageIndex = 1, int pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(searchText))
            {
                url += "&Searchtext=" + searchText;
            }
            var response = api.ApiCall(CatalogueUrl, EndPoints.CommissionCharges + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&onlyCategory=" + true + url, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);
            List<CommissionChargesLibrary> templist = baseResponse.Data as List<CommissionChargesLibrary>;

            List<CommissionChargesLibrary> commissionChargeslst = new List<CommissionChargesLibrary>();
            commissionChargeslst = templist.Select(x => new CommissionChargesLibrary
            {
                ID = x.ID,
                BrandID = x.BrandID,
                SellerID = x.SellerID,
                CatID = x.CatID,
                CategoryName = x.CategoryName,
                ChargesIn = x.ChargesIn,
                //ChargesOn = x.ChargesOn,
                //IsCompulsary = x.IsCompulsary,
                AmountValue = x.AmountValue,
                OnlyBrands = x.OnlyBrands,
                OnlyCategories = x.OnlyCategories,
                OnlySellers = x.OnlySellers,
                BrandName = x.BrandID != null ? getBrand((int)x.BrandID).Name : null,
                SellerName = x.SellerID != null ? string.IsNullOrEmpty(getsellerKyc(x.SellerID).DisplayName) ? string.IsNullOrEmpty(getsellerKyc(x.SellerID).DisplayName) ? getsellerKyc(x.SellerID).FullName : getsellerKyc(x.SellerID).DisplayName : getsellerKyc(x.SellerID).DisplayName : null,
                CreatedBy = x.CreatedBy,
                CreatedAt = x.CreatedAt,
                ModifiedBy = x.ModifiedBy,
                ModifiedAt = x.ModifiedAt,
                IsDeleted = x.IsDeleted,
                DeletedBy = x.DeletedBy,
                DeletedAt = x.DeletedAt
            }).ToList();

            var datalist = commissionChargeslst.ToList();
            //datalist = commissionChargeslst.Where(x => x.CategoryName.ToLower().Contains(searchText.ToLower())).ToList();
            if (datalist.Count > 0)
            {
                //int totalCount = datalist.Count;
                //int TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                //List<CommissionChargesLibrary> _commissionChargeslst = new List<CommissionChargesLibrary>();
                //_commissionChargeslst = datalist.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                //baseResponse.Message = "Data bind suceessfully.";
                //baseResponse.Data = _commissionChargeslst;
                //baseResponse.code = 200;
                //baseResponse.pagination.PageCount = TotalPages;
                //baseResponse.pagination.RecordCount = totalCount;
                baseResponse.Data = commissionChargeslst.ToList();
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }

            //else
            //{
            //    var response = api.ApiCall(CatalogueUrl, EndPoints.CommissionCharges + "?PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&onlyCategory=" + true, "GET", null);
            //    baseResponse = baseResponse.JsonParseList(response);
            //    List<CommissionChargesLibrary> templist = baseResponse.Data as List<CommissionChargesLibrary>;

            //    var pag = baseResponse.pagination;
            //    var message = baseResponse.Message;
            //    var code = baseResponse.code;

            //    baseResponse.pagination = pag;
            //    baseResponse.Message = message;
            //    baseResponse.code = code;

            //    baseResponse.Data = templist.Select(x => new CommissionChargesLibrary
            //    {
            //        ID = x.ID,
            //        BrandID = x.BrandID,
            //        SellerID = x.SellerID,
            //        CatID = x.CatID,
            //        CategoryName = x.CategoryName,
            //        ChargesIn = x.ChargesIn,
            //        //ChargesOn = x.ChargesOn,
            //        //IsCompulsary = x.IsCompulsary,
            //        AmountValue = x.AmountValue,
            //        OnlyBrands = x.OnlyBrands,
            //        OnlyCategories = x.OnlyCategories,
            //        OnlySellers = x.OnlySellers,
            //        BrandName = x.BrandID != null ? getBrand((int)x.BrandID).Name : null,
            //        SellerName = x.SellerID != null ? string.IsNullOrEmpty(getsellerKyc(x.SellerID).TradeName) ? string.IsNullOrEmpty(getsellerKyc(x.SellerID).DisplayName) ? getsellerKyc(x.SellerID).FullName : getsellerKyc(x.SellerID).DisplayName : getsellerKyc(x.SellerID).TradeName : null,
            //        CreatedBy = x.CreatedBy,
            //        CreatedAt = x.CreatedAt,
            //        ModifiedBy = x.ModifiedBy,
            //        ModifiedAt = x.ModifiedAt,
            //        IsDeleted = x.IsDeleted,
            //        DeletedBy = x.DeletedBy,
            //        DeletedAt = x.DeletedAt
            //    }).ToList();
            //}
            return Ok(baseResponse);
        }

        #region Comment code of chargeson
        //[HttpGet("ByChargesOn")]
        //[Authorize(Roles = "Admin, Seller")]
        //public ActionResult<ApiHelper> GetByChargesOn(string chargesOn, int? pageIndex = 1, int? pageSize = 10)
        //{
        //    var response = api.ApiCall(CatalogueUrl, EndPoints.CommissionCharges + "?chargeson=" + chargesOn + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize + "&onlyCategory=" + true, "GET", null);
        //    baseResponse = baseResponse.JsonParseList(response);
        //    List<CommissionChargesLibrary> templist = baseResponse.Data as List<CommissionChargesLibrary>;
        //    var pag = baseResponse.pagination;
        //    var message = baseResponse.Message;
        //    var code = baseResponse.code;

        //    baseResponse.pagination = pag;
        //    baseResponse.Message = message;
        //    baseResponse.code = code;

        //    baseResponse.Data = templist.Select(x => new CommissionChargesLibrary
        //    {
        //        ID = x.ID,
        //        BrandID = x.BrandID,
        //        SellerID = x.SellerID,
        //        CatID = x.CatID,
        //        CategoryName = x.CategoryName,
        //        ChargesIn = x.ChargesIn,
        //        //ChargesOn = x.ChargesOn,
        //        //IsCompulsary = x.IsCompulsary,
        //        AmountValue = x.AmountValue,
        //        OnlyBrands = x.OnlyBrands,
        //        OnlyCategories = x.OnlyCategories,
        //        OnlySellers = x.OnlySellers,
        //        BrandName = x.BrandID != null ? getBrand((int)x.BrandID).Name : null,
        //        SellerName = x.SellerID != null ? getsellerKyc(x.SellerID).TradeName : null
        //    }).ToList();
        //    return Ok(baseResponse);
        //}
        #endregion Comment code of chargeson


        [NonAction]
        public SellerKycList getsellerKyc(string sellerId)
        {
            BaseResponse<KYCDetails> baseResponse = new BaseResponse<KYCDetails>();
            BaseResponse<GSTInfo> baseResponse1 = new BaseResponse<GSTInfo>();
            BaseResponse<SellerListModel> baseResponse2 = new BaseResponse<SellerListModel>();

            var response2 = api.ApiCall(IdServerUrl, EndPoints.SellerById + "?ID=" + sellerId, "GET", null);
            baseResponse2 = baseResponse2.JsonParseRecord(response2);
            SellerListModel seller = baseResponse2.Data as SellerListModel;


            var response = api.ApiCall(UserUrl, EndPoints.KYCDetails + "?UserID=" + sellerId, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            KYCDetails kycDetails = baseResponse.Data as KYCDetails;

            var response1 = api.ApiCall(UserUrl, EndPoints.GSTInfo + "?UserID=" + sellerId, "GET", null);
            baseResponse1 = baseResponse1.JsonParseRecord(response1);
            GSTInfo gstInfo = baseResponse1.Data as GSTInfo;


            // Create a new instance of SellerKycList
            SellerKycList sellerKycList = new SellerKycList();

            // Set Seller Details
            sellerKycList.UserID = seller.Id;
            sellerKycList.FullName = $"{seller.FirstName} {seller.LastName}";
            sellerKycList.EmailID = seller.UserName;
            sellerKycList.PhoneNumber = kycDetails.ContactPersonMobileNo;
            sellerKycList.CreatedAt = kycDetails.CreatedAt;

            // Set KYC Details
            sellerKycList.KYCFor = kycDetails.KYCFor;
            sellerKycList.DisplayName = kycDetails.DisplayName;
            sellerKycList.OwnerName = kycDetails.OwnerName;
            sellerKycList.ContactPersonName = kycDetails.ContactPersonName;
            sellerKycList.ContactPersonMobileNo = kycDetails.ContactPersonMobileNo;
            sellerKycList.Logo = kycDetails.Logo;
            sellerKycList.Status = kycDetails.Status;
            sellerKycList.ModifiedAt = kycDetails.ModifiedAt;

            // Set GST Details
            sellerKycList.LegalName = gstInfo.LegalName;
            sellerKycList.TradeName = gstInfo.TradeName;

            return sellerKycList;
        }

        [NonAction]
        public BrandLibrary getBrand(int brandId)
        {
            BaseResponse<BrandLibrary> brandResponse = new BaseResponse<BrandLibrary>();
            var response = api.ApiCall(UserUrl, EndPoints.Brand + "?Id=" + brandId, "GET", null);
            brandResponse = brandResponse.JsonParseRecord(response);

            BrandLibrary brand = new BrandLibrary();
            brand = brandResponse.Data as BrandLibrary;

            return brand;
        }
    }
}
