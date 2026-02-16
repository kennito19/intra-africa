using API_Gateway.Common;
using API_Gateway.Common.products;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger;
using System.Data;

namespace API_Gateway.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellerDataController : ControllerBase
    {

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        BaseResponse<SellerListModel> baseResponse = new BaseResponse<SellerListModel>();
        public static string IDServerUrl = string.Empty;
        public static string CatalogueUrl = string.Empty;
        public static string UserUrl = string.Empty;
        private ApiHelper api;
        public SellerDataController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpContext = _httpContextAccessor.HttpContext;
            api = new ApiHelper(_httpContext);
            _configuration = configuration;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            CatalogueUrl = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            UserUrl = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public async Task<IActionResult> Update(SellerListModel model)
        {
            BaseResponse<SellerListModel> baseResponse = new BaseResponse<SellerListModel>();

            sellerKyc seller = new sellerKyc(_configuration, _httpContext);
            SellerKycDetails sellerkyc = seller.BindSeller(model.Id);
            if (model.Status.ToLower() == "active")
            {
                if (sellerkyc != null)
                {
                    if (string.IsNullOrEmpty(sellerkyc.DisplayName) || string.IsNullOrEmpty(sellerkyc.AccountNo))
                    {
                        baseResponse.code = 204;
                        baseResponse.Message = "Seller KYC Is not Approved";
                    }
                    else if (sellerkyc.gSTInfos.Count() == 0)
                    {
                        baseResponse.code = 204;
                        baseResponse.Message = "Seller KYC Is not Approved";
                    }
                    else if (sellerkyc.wareHouses.Count() == 0)
                    {
                        baseResponse.code = 204;
                        baseResponse.Message = "Seller KYC Is not Approved";
                    }
                    else if (sellerkyc.Status.ToLower() != "approved")
                    {
                        baseResponse.code = 204;
                        baseResponse.Message = "Seller KYC Is not Approved";
                    }
                    else
                    {
                        var response = api.ApiCall(IDServerUrl, EndPoints.SellerList, "PUT", model);
                        baseResponse = baseResponse.JsonParseInputResponse(response);

                        if (baseResponse.code == 200)
                        {
                            baseResponse.code = 200;
                            baseResponse.Message = "Succeeded. Seller Updated Successfully";

                            BaseResponse<ProductExtraDetailsDto> ExtraDetailsbaseResponse = new BaseResponse<ProductExtraDetailsDto>();
                            ProductExtraDetailsDto productExtraDetails = new ProductExtraDetailsDto();
                            productExtraDetails.FullName = model.FirstName + " " + model.LastName;
                            productExtraDetails.UserName = model.UserName;
                            productExtraDetails.PhoneNumber = model.MobileNo;
                            productExtraDetails.SellerStatus = model.Status;
                            productExtraDetails.SellerId = model.Id;
                            productExtraDetails.Mode = "updateSeller";
                            var ExtraDetailsresponse = api.ApiCall(CatalogueUrl, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
                            ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);
                        }

                        var res = api.ApiCall(UserUrl, EndPoints.UserDetails + "/getUserDetails" + "?UserId=" + model.Id, "GET",null);
                        BaseResponse<UserDetails> userBaseResponse  = new BaseResponse<UserDetails>();

                        if(userBaseResponse.Data != null)
                        {
                            userBaseResponse = userBaseResponse.JsonParseRecord(res);
                            UserDetails user = (UserDetails)userBaseResponse.Data;
                            user.FirstName = model.FirstName;
                            user.LastName = model.LastName;
                            user.Email = model.Email;
                            user.Phone = model.MobileNo;
                            user.ProfileImage = model.ProfileImage;
                            user.Gender = model.Gender;
                            user.Status = model.Status;
                            user.ModifiedAt = DateTime.Now;
                            user.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                            var updateres = api.ApiCall(UserUrl, EndPoints.UserDetails, "PUT", user);
                        }
                        else
                        {
                            UserDetails user = new UserDetails();
                            user.UserId = model.Id;
                            user.FirstName = model.FirstName;
                            user.LastName = model.LastName;
                            user.Email = model.Email;
                            user.Phone = model.MobileNo;
                            user.ProfileImage = model.ProfileImage;
                            user.Gender = model.Gender;
                            user.Status = model.Status;
                            user.UserType = "seller";
                            user.CreatedAt = DateTime.Now;
                            user.CreatedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                            var updateres = api.ApiCall(UserUrl, EndPoints.UserDetails, "PUT", user);
                        }
                    }
                }
                else
                {
                    baseResponse.code = 204;
                    baseResponse.Message = "Seller KYC Is not Approved";
                }
            }
            else
            {
                var response = api.ApiCall(IDServerUrl, EndPoints.SellerList, "PUT", model);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                if (baseResponse.code == 200)
                {
                    baseResponse.code = 200;
                    baseResponse.Message = "Succeeded. Seller Updated Successfully";

                    BaseResponse<ProductExtraDetailsDto> ExtraDetailsbaseResponse = new BaseResponse<ProductExtraDetailsDto>();
                    ProductExtraDetailsDto productExtraDetails = new ProductExtraDetailsDto();
                    productExtraDetails.FullName = model.FirstName + " " + model.LastName;
                    productExtraDetails.UserName = model.UserName;
                    productExtraDetails.PhoneNumber = model.MobileNo;
                    productExtraDetails.SellerStatus = model.Status;
                    productExtraDetails.SellerId = model.Id;
                    productExtraDetails.Mode = "updateSeller";
                    var ExtraDetailsresponse = api.ApiCall(CatalogueUrl, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
                    ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);

                }
            }


            return Ok(baseResponse);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("archived")]
        public async Task<IActionResult> archived(string userId)
        {
            var response = api.ApiCall(IDServerUrl, EndPoints.SellerById + "?ID=" + userId, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            if (baseResponse.code == 200)
            {
                SellerListModel seller = new SellerListModel();
                seller = (SellerListModel)baseResponse.Data;
                seller.Status = "Archived";
                seller.ModifiedAt = DateTime.Now;
                seller.ModifiedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;

                var responseData = api.ApiCall(IDServerUrl, EndPoints.SellerList, "PUT", seller);
                if (responseData.IsSuccessStatusCode)
                {
                    baseResponse.code = 200;
                    baseResponse.Message = "Seller archived successfully";

                    string uid = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                    ArchiveProduct archiveProduct = new ArchiveProduct(_httpContext, uid, CatalogueUrl);
                    ProductDelete productDelete = new ProductDelete();
                    productDelete.productId = 0;
                    productDelete.SellerProductId = 0;
                    productDelete.SellerId = userId;

                    var res = archiveProduct.ArchiveProductData(productDelete);
                }
                else
                {
                    baseResponse = baseResponse.APICallFailed(responseData);
                }
            }

            //AdminListModel admin = new AdminListModel();

            //BaseResponse<AdminListModel> baseResponse = new BaseResponse<AdminListModel>();


            return Ok(baseResponse);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("delete")]
        public async Task<IActionResult> delete(string userId)
        {
            sellerKyc seller = new sellerKyc(_configuration, _httpContext);
            SellerKycDetails sellerkyc = seller.BindSeller(userId);

            if (sellerkyc != null)
            {
                if (!string.IsNullOrEmpty(sellerkyc.DisplayName) || !string.IsNullOrEmpty(sellerkyc.AccountNo))
                {
                    baseResponse = baseResponse.ChildAlreadyExists("KYC Info", "Seller");
                }
                else if (sellerkyc.gSTInfos.Count() > 0)
                {
                    baseResponse = baseResponse.ChildAlreadyExists("GST Info", "Seller");
                }
                else if (sellerkyc.wareHouses.Count() > 0)
                {
                    baseResponse = baseResponse.ChildAlreadyExists("Warehouse Details", "Seller");
                }
                else
                {
                    var response = api.ApiCall(IDServerUrl, EndPoints.SellerById + "?ID=" + userId, "GET", null);
                    baseResponse = baseResponse.JsonParseRecord(response);
                    if (baseResponse.code == 200)
                    {
                        SellerListModel _seller = new SellerListModel();
                        _seller = (SellerListModel)baseResponse.Data;
                        _seller.Status = "Deleted";
                        string url = "/delete";
                        var responseData = api.ApiCall(IDServerUrl, EndPoints.SellerList + url, "PUT", seller);
                        if (responseData.IsSuccessStatusCode)
                        {
                            UserDetails userDetails = new UserDetails();
                            userDetails.UserId = userId;
                            userDetails.DeletedAt = DateTime.Now;
                            userDetails.DeletedBy = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                            var responseData1 = api.ApiCall(UserUrl, EndPoints.UserDetails, "Delete", userDetails);

                            baseResponse.code = 200;
                            baseResponse.Message = "Seller deleted successfully";
                        }
                        else
                        {
                            baseResponse = baseResponse.APICallFailed(responseData);
                        }
                    }
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            //        var response = api.ApiCall(IDServerUrl, EndPoints.SellerById + "?ID=" + userId, "GET", null);
            //baseResponse = baseResponse.JsonParseRecord(response);
            //if (baseResponse.code == 200)
            //{
            //    SellerListModel seller = new SellerListModel();
            //    seller = (SellerListModel)baseResponse.Data;
            //    seller.Status = "Deleted";
            //    string url = "/delete";
            //    var responseData = api.ApiCall(IDServerUrl, EndPoints.SellerList + url, "PUT", seller);
            //    if (responseData.IsSuccessStatusCode)
            //    {
            //        baseResponse.code = 200;
            //        baseResponse.Message = "Seller deleted successfully";
            //    }
            //    else
            //    {
            //        baseResponse = baseResponse.APICallFailed(responseData);
            //    }
            //}

            //AdminListModel admin = new AdminListModel();

            //BaseResponse<AdminListModel> baseResponse = new BaseResponse<AdminListModel>();


            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public IActionResult getList(int pageIndex = 1, int pageSize = 10, string? status = null, string? KycStatus = null, bool GetArchived = false)
        {
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext); 
            baseResponse.Data = seller.bindSellerDetails(GetArchived, status, KycStatus);
            baseResponse.code = 200;
            baseResponse.Message = "Seller list bind successfully";

            return Ok(baseResponse);
        }

        [HttpGet("bindActiveSeller")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public IActionResult bindActiveSeller()
        {
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            List<UserDetailsDTO> lst = seller.bindSellerDetails(false,"Active","Approved");
            baseResponse.code = 200;
            baseResponse.Message = "Seller list bind successfully";
            baseResponse.Data = lst;
            baseResponse.pagination = null;

            return Ok(baseResponse);
        }

        [HttpGet("bindAllSeller")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public IActionResult bindAllSeller()
        {
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            List<UserDetailsDTO> lst = seller.bindSellerDetails(false);
            baseResponse.Data = lst;
            baseResponse.code = 200;
            baseResponse.Message = "Seller list bind successfully";
            baseResponse.pagination = null;

            return Ok(baseResponse);
        }

        [HttpGet("bindArchivedSellers")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public IActionResult bindArchivedSellers(int pageIndex = 1, int pageSize = 10)
        {
            
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            List<UserDetailsDTO> lst = seller.bindSellerDetails(true, "Archived");
            baseResponse.pagination = null;
            baseResponse.Data = lst;
            baseResponse.code = 200;
            baseResponse.Message = "Seller list bind successfully";
            
            return Ok(baseResponse);
        }

        [HttpGet("bindSellersBybrandId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public IActionResult bindSellersByProductId(int brandId)
        {
            BaseResponse<AssignBrandToSeller> AssignbaseResponse = new BaseResponse<AssignBrandToSeller>();
            var AssignBrandresponse = api.ApiCall(UserUrl, EndPoints.AssignBrandToSeller + "?PageIndex=" + 0 + "&PageSize=" + 0 + "&BrandId=" + brandId + "&status=" + "Active" + "&brandStatus=" + "Active", "GET", null);
            AssignbaseResponse = AssignbaseResponse.JsonParseList(AssignBrandresponse);
            if (AssignbaseResponse.code == 200)
            {
                List<AssignBrandToSeller> AssiBrandLists = (List<AssignBrandToSeller>)AssignbaseResponse.Data;
                sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
                List<UserDetailsDTO> lst = seller.bindSellerDetails(false);
                lst = lst.Where(p => p.KycStatus != null && p.KycStatus != "" && p.KycStatus.ToLower() != "deleted" && p.KycStatus.ToLower() != "rejected" && p.KycStatus != "Archived").ToList();

                var _response = (from s in lst join b in AssiBrandLists on s.UserId equals b.SellerID select s).ToList();

                baseResponse.pagination = null;
                baseResponse.Data = _response;
            }
            else
            {
                baseResponse.code = AssignbaseResponse.code;
                baseResponse.Message = AssignbaseResponse.Message;
                baseResponse.Data = AssignbaseResponse.Data;
                baseResponse.pagination = AssignbaseResponse.pagination;
            }
            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public IActionResult getById(string id)
        {
            var response = api.ApiCall(IDServerUrl, EndPoints.SellerById + "?ID=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            return Ok(baseResponse);
        }

        [HttpGet("Details")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public IActionResult getSellerDetails(string id)
        {
            sellerKyc seller = new sellerKyc(_configuration, _httpContext);
            return Ok(seller.BindSeller(id));
        }

        [HttpGet("search")]
        [Authorize(Roles = "Admin, Seller, Customer")]
        public IActionResult search(string? searchtext = null, string? status = null, string? KycStatus = null, int pageIndex = 1, int pageSize = 10)
        {
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            baseResponse.Data = seller.bindSellerDetails(true, status, KycStatus,searchtext,null,pageIndex,pageSize);
            baseResponse.code = 200;
            baseResponse.Message = "Seller list bind successfully";

            return Ok(baseResponse);

        }

        [NonAction]
        public string UpdateProfile(string OldName, string Name, IFormFile? FileName)
        {

            try
            {
                if (FileName != null)
                {
                    var file = FileName;

                    string a = DateTime.Now.ToString("_dd-MM-yyyy_HH_mm_ss_fff");
                    var fileName = Name + a;
                    var folderName = Path.Combine("Resources", "UserProfile");
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    fileName = imageUpload.UpdateUploadImageAndDocs(OldName, fileName, folderName, FileName);

                    return fileName;
                }
                else
                {
                    string fileName = null;
                    if (OldName != string.Empty)
                    {
                        fileName = OldName;

                    }
                    return fileName;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
