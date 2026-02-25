using API_Gateway.Common;
using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace API_Gateway.Controllers.Seller
{
    [Route("api/seller/[controller]")]
    [ApiController]
    public class KYCController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        public string URL = string.Empty;
        public string CatelogueURL = string.Empty;
        public static string IDServerUrl = string.Empty;
        private UserKYCDetails UserKYCdetails;
        private ImageUpload imageUpload;
        private ApiHelper helper;
        BaseResponse<KYCDetails> baseResponse = new BaseResponse<KYCDetails>();
        public KYCController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;

            _httpContext = _httpContextAccessor.HttpContext;
            URL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            UserKYCdetails = new UserKYCDetails(_httpContext, URL, CatelogueURL, _configuration);
            imageUpload = new ImageUpload(_configuration);
            helper = new ApiHelper(_httpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> Save(KYCDetails model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            bool AllowAadharcard = Convert.ToBoolean(_configuration.GetValue<string>("allow_aadharcard"));
            model.ShipmentBy = model.ShipmentBy;
            model.ShipmentChargesPaidBy = model.ShipmentChargesPaidBy;
            model.KYCFor = "Seller";
            baseResponse = UserKYCdetails.SaveKYC(model, userID, AllowAadharcard);

            return Ok(baseResponse);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> Update(KYCDetails model)
        {
            string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
            bool AllowAadharcard = Convert.ToBoolean(_configuration.GetValue<string>("allow_aadharcard"));
            model.ShipmentBy = model.ShipmentBy;
            model.ShipmentChargesPaidBy = model.ShipmentChargesPaidBy;
            model.KYCFor = "Seller";
            baseResponse = UserKYCdetails.UpdateKYC(model, userID, AllowAadharcard);

            BaseResponse<SellerListModel> SellerbaseResponse = new BaseResponse<SellerListModel>();
            var getresponse = helper.ApiCall(IDServerUrl, EndPoints.SellerById + "?ID=" + model.UserID, "GET", null);
            SellerbaseResponse = SellerbaseResponse.JsonParseRecord(getresponse);

            if (SellerbaseResponse.code == 200)
            {
                SellerListModel seller = SellerbaseResponse.Data as SellerListModel;
                if (seller != null)
                {
                    if (model.Status.ToLower() == "approved")
                    {
                        if (seller.Status.ToLower() == "pending")
                        {
                            seller.Status = "Active";
                        }

                    }
                    else if (model.Status.ToLower() == "pending")
                    {
                        seller.Status = "In Progress";
                    }
                    else
                    {
                        seller.Status = "Pending";
                    }
                    var updateresponse = helper.ApiCall(IDServerUrl, EndPoints.SellerList, "PUT", seller);
                    SellerbaseResponse = SellerbaseResponse.JsonParseInputResponse(updateresponse);

                    if (SellerbaseResponse.code == 200)
                    {
                        BaseResponse<ProductExtraDetailsDto> ExtraDetailsbaseResponse = new BaseResponse<ProductExtraDetailsDto>();
                        ProductExtraDetailsDto productExtraDetails = new ProductExtraDetailsDto();
                        productExtraDetails.FullName = seller.FirstName + " " + seller.LastName;
                        productExtraDetails.UserName = seller.UserName;
                        productExtraDetails.PhoneNumber = seller.MobileNo;
                        productExtraDetails.SellerStatus = seller.Status;
                        productExtraDetails.SellerId = seller.Id;
                        productExtraDetails.Mode = "updateSeller";
                        var ExtraDetailsresponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
                        ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);
                    }
                }
            }

            return Ok(baseResponse);
        }

        [HttpPut("UpdateSellerKyc")]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> UpdateSellerKyc(UpdateSellerKycData sellerkyc)
        {
            baseResponse = UserKYCdetails.GetKycByUserID(sellerkyc.SellerId);
            KYCDetails model = new KYCDetails();
            if (baseResponse.code == 200)
            {
                model = baseResponse.Data as KYCDetails;
                string userID = HttpContext.User.Claims.Where(x => x.Type.Equals("UserID")).FirstOrDefault().Value;
                bool AllowAadharcard = Convert.ToBoolean(_configuration.GetValue<string>("allow_aadharcard"));
                model.Status = sellerkyc.Status;
                model.Note = sellerkyc.Note;
                baseResponse = UserKYCdetails.UpdateKYC(model, userID, AllowAadharcard);

                BaseResponse<SellerListModel> SellerbaseResponse = new BaseResponse<SellerListModel>();
                var getresponse = helper.ApiCall(IDServerUrl, EndPoints.SellerById + "?ID=" + model.UserID, "GET", null);
                SellerbaseResponse = SellerbaseResponse.JsonParseRecord(getresponse);

                if (SellerbaseResponse.code == 200)
                {
                    SellerListModel seller = SellerbaseResponse.Data as SellerListModel;
                    if (seller != null)
                    {
                        if (model.Status.ToLower() == "approved")
                        {
                            if (seller.Status.ToLower() == "pending")
                            {
                                seller.Status = "Active";
                            }
                        }
                        else
                        {
                            seller.Status = "Pending";
                        }
                        var updateresponse = helper.ApiCall(IDServerUrl, EndPoints.SellerList, "PUT", seller);
                        SellerbaseResponse = SellerbaseResponse.JsonParseInputResponse(updateresponse);

                        if (SellerbaseResponse.code == 200)
                        {
                            BaseResponse<ProductExtraDetailsDto> ExtraDetailsbaseResponse = new BaseResponse<ProductExtraDetailsDto>();
                            ProductExtraDetailsDto productExtraDetails = new ProductExtraDetailsDto();
                            productExtraDetails.FullName = seller.FirstName + " " + seller.LastName;
                            productExtraDetails.UserName = seller.UserName;
                            productExtraDetails.PhoneNumber = seller.MobileNo;
                            productExtraDetails.SellerStatus = seller.Status;
                            productExtraDetails.SellerId = seller.Id;
                            productExtraDetails.Mode = "updateSeller";
                            var ExtraDetailsresponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
                            ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);


                            #region send mail
                            BaseResponse<KYCDetails> KycbaseResponse = new BaseResponse<KYCDetails>();
                            var response = helper.ApiCall(URL, EndPoints.KYCDetails + "?Id=" + model.Id, "GET", null);
                            KycbaseResponse = KycbaseResponse.JsonParseRecord(response);
                            KYCDetails kyc = KycbaseResponse.Data as KYCDetails;

                            MailSendSES objses = new MailSendSES(_configuration);

                            string subject = "Welcome on Hashkart";
                            string htmlBody = "";
                            List<string> ReceiverEmail = new List<string>();

                            ReceiverEmail.Add(seller.Email);
                            StreamReader reader;
                            if (model.Status.ToLower() == "approved")
                            {

                                reader = new StreamReader("Resources" + "\\EmailTemplate" +"\\seller" + "\\store_approved.html");
                                string readFile = reader.ReadToEnd();

                                var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";

                                readFile = readFile.Replace("{{seller_name}}", seller.FirstName + " " + seller.LastName);
                                readFile = readFile.Replace("{{Store_Name}}", kyc.DisplayName);
                                readFile = readFile.Replace("{{seller_email_id}}", seller.Email);


                                htmlBody = readFile;
                            }
                            else if (model.Status.ToLower() == "not approved")
                            {
                                reader = new StreamReader("Resources" + "\\EmailTemplate" +"\\seller" + "\\store_not_approved.html");
                                string readFile = reader.ReadToEnd();

                                var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";

                                readFile = readFile.Replace("{{seller_name}}", seller.FirstName + " " + seller.LastName);
                                readFile = readFile.Replace("{{Store_Name}}", kyc.DisplayName);
                                readFile = readFile.Replace("{{seller_email_id}}", seller.Email);


                                htmlBody = readFile;
                            }
                            else if (model.Status.ToLower() == "rejected")
                            {
                                reader = new StreamReader("Resources" + "\\EmailTemplate" + "\\seller" + "\\store_rejected.html");
                                string readFile = reader.ReadToEnd();

                                var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";

                                readFile = readFile.Replace("{{seller_name}}", seller.FirstName + " " + seller.LastName);
                                readFile = readFile.Replace("{{Store_Name}}", kyc.DisplayName);
                                readFile = readFile.Replace("{{seller_email_id}}", seller.Email);


                                htmlBody = readFile;
                            }
                            objses.sendMail(subject, htmlBody, ReceiverEmail);

                            #endregion
                        }
                    }
                }
            }
            return Ok(baseResponse);
        }

        [HttpDelete]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> Delete(int? id = 0, string? userId = "")
        {
            baseResponse = UserKYCdetails.DeleteKYC(id, userId);
            return Ok(baseResponse);
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> Get(int? pageIndex = 1, int? pageSize = 10)
        {
            baseResponse = UserKYCdetails.GetKyc(pageIndex, pageSize);
            return Ok(baseResponse);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> ById(int id)
        {
            baseResponse = UserKYCdetails.GetKycById(id);
            return Ok(baseResponse);
        }

        [HttpGet("byUserId")]
        [Authorize(Roles = "Admin,Seller")]
        public ActionResult<ApiHelper> ByUserID(string userId)
        {
            baseResponse = UserKYCdetails.GetKycByUserID(userId);
            return Ok(baseResponse);
        }

        [HttpPost("TempImage")]
        public ActionResult<ApiHelper> TempImage(string sellerId, string docName, IFormFile Image, string? sellerLeagleName)
        {
            var result = imageUpload.TempUploadMethod(sellerId, docName, Image, sellerLeagleName);
            return Ok(result);
        }

        public class UpdateSellerKycData
        {
            public string SellerId { get; set; }
            public string? Status { get; set; }
            public string? Note { get; set; }
        }
    }
}
