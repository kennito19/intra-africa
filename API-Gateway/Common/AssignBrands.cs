using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Web;

namespace API_Gateway.Common
{
    public class AssignBrands
    {
        public string _URL = string.Empty;
        public string CatelogueURL = string.Empty;
        public string IDServerUrl = string.Empty;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly IConfiguration _configuration;
        private readonly ImageUpload imageUpload;
        BaseResponse<AssignBrandToSeller> baseResponse = new BaseResponse<AssignBrandToSeller>();
        private ApiHelper helper;
        public AssignBrands(string URL, HttpContext HttpContext, string _CatelogueURL, string _IDServerUrl, IConfiguration configuration)
        {
            _URL = URL;
            CatelogueURL = _CatelogueURL;
            IDServerUrl = _IDServerUrl;
            _httpContext = HttpContext;
            _configuration = configuration;
            imageUpload = new ImageUpload(_configuration);
            helper = new ApiHelper(_httpContext);
            _httpContextAccessor = new HttpContextAccessor();
        }
        public BaseResponse<AssignBrandToSeller> SaveAssignBrands([FromForm] AssignBrandToSellerDTO model, string UserID, bool AllowBrandCerti, bool isAdmin)
        {
            if (AllowBrandCerti)
            {
                if (string.IsNullOrEmpty(model.BrandCertificate))
                {
                    baseResponse = baseResponse.InvalidInput("Brand Certificate is required!");
                    return baseResponse;
                }
            }

            var temp = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?SellerID=" + model.SellerID + "&BrandId=" + model.BrandId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignBrandToSeller> tmp = (List<AssignBrandToSeller>)baseResponse.Data;
            if (tmp != null && tmp.Where(x => x.BrandId == (model.BrandId) && x.SellerID == (model.SellerID)).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var temp2 = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?SellerID=" + model.SellerID + "&BrandId=" + model.BrandId + "&isDeleted=" + true, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp2);
                List<AssignBrandToSeller> tmp2 = (List<AssignBrandToSeller>)baseResponse.Data;
                if (tmp2.Count > 0)
                {
                    var data = tmp2.FirstOrDefault();
                    AssignBrandToSeller abts = new AssignBrandToSeller();
                    abts.Id = data.Id;
                    abts.SellerID = model.SellerID;
                    abts.BrandId = model.BrandId;

                    if (isAdmin)
                    {
                        abts.Status = model.Status;
                    }
                    else
                    {
                        abts.Status = "Request For Approval";
                    }
                    //abts.BrandCertificate = model.BrandCertificate;
                    abts.BrandCertificate = UploadDoc(model.SellerID, model.BrandId.ToString(), model.FileName);
                    abts.CreatedAt = DateTime.Now;
                    abts.CreatedBy = UserID;
                    abts.ModifiedBy = null;
                    abts.ModifiedAt = null;
                    abts.IsDeleted = false;
                    abts.DeletedAt = null;
                    abts.DeletedBy = null;
                    var response = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller, "PUT", abts);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                    if (baseResponse.code == 200)
                    {
                        baseResponse.Message = "Record added successfully.";
                    }
                }
                else
                {
                    AssignBrandToSeller abts = new AssignBrandToSeller();
                    abts.SellerID = model.SellerID;
                    abts.BrandId = model.BrandId;

                    if (isAdmin)
                    {
                        abts.Status = model.Status;
                    }
                    else
                    {
                        abts.Status = "Request For Approval";


                        BaseResponse<SellerListModel> sellerResponse = new BaseResponse<SellerListModel>();

                        var tmpresponse = helper.ApiCall(IDServerUrl, EndPoints.SellerById + "?ID=" + model.SellerID, "GET", null);
                        sellerResponse = sellerResponse.JsonParseRecord(tmpresponse);

                        SellerListModel Resetdata = new SellerListModel();
                        Resetdata = (SellerListModel)sellerResponse.Data;

                        #region send mail
                        MailSendSES objses = new MailSendSES(_configuration);

                        string subject = model.BrandName + "Brand request approved - Hashkart";
                        string htmlBody = "";
                        List<string> ReceiverEmail = new List<string>();

                        ReceiverEmail.Add(Resetdata.Email);

                        StreamReader reader = new StreamReader("Resources" + "\\EmailTemplate" + "\\admin" + "\\brand_request.html");
                        string readFile = reader.ReadToEnd();

                        var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";


                        readFile = readFile.Replace("{{image_server_path}}", baseUrl);
                        readFile = readFile.Replace("{{admin_name}}", "Hashkart Team");
                        readFile = readFile.Replace("{{brand_name}}", model.BrandName);
                        readFile = readFile.Replace("{{seller_name}}", Resetdata.FirstName + " " + Resetdata.LastName);

                        htmlBody = readFile;

                        objses.sendMail(subject, htmlBody, ReceiverEmail);

                        #endregion



                    }
                    //abts.BrandCertificate = model.BrandCertificate;
                    abts.BrandCertificate = UploadDoc(model.SellerID, model.BrandId.ToString(), model.FileName);
                    abts.CreatedAt = DateTime.Now;
                    abts.CreatedBy = UserID;

                    var response = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller, "POST", abts);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }


            }
            return baseResponse;
        }

        public BaseResponse<AssignBrandToSeller> UpdateAssignBrands([FromForm] AssignBrandToSellerDTO model, string UserID, bool AllowBrandCerti)
        {
            BaseResponse<BrandLibrary> BrandbaseResponse = new BaseResponse<BrandLibrary>();
            var brandResponse = helper.ApiCall(_URL, EndPoints.Brand + "?Id=" + model.BrandId, "GET", null);
            BrandbaseResponse = BrandbaseResponse.JsonParseRecord(brandResponse);
            BrandLibrary brandsdata = (BrandLibrary)BrandbaseResponse.Data;

            if (brandsdata.Status.ToLower() == "request for approval")
            {
                baseResponse.code = 204;
                baseResponse.Message = "Requested brand is not approved.";
                baseResponse.Data = null;
                return baseResponse;
            }


            var temp = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?SellerID=" + model.SellerID + "&BrandId=" + model.BrandId, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignBrandToSeller> tmp = (List<AssignBrandToSeller>)baseResponse.Data;
            if (tmp.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(response);
                AssignBrandToSeller abts = (AssignBrandToSeller)baseResponse.Data;
                string OldName = abts.BrandCertificate;
                if (AllowBrandCerti)
                {
                    if (string.IsNullOrEmpty(abts.BrandCertificate))
                    {
                        if (string.IsNullOrEmpty(model.BrandCertificate))
                        {
                            baseResponse = baseResponse.InvalidInput("Brand Certificate is required!");
                        }
                    }
                }

                abts.SellerID = model.SellerID;
                abts.BrandId = model.BrandId;
                abts.Status = model.Status;
                if (!string.IsNullOrEmpty(model.BrandCertificate))
                {
                    abts.BrandCertificate = UpdateDocFile(OldName, model.SellerID, model.BrandId.ToString(), model.FileName);
                }
                abts.ModifiedBy = UserID;
                abts.ModifiedAt = DateTime.Now;
                abts.IsDeleted = false;
                abts.DeletedAt = null;
                abts.DeletedBy = null;
                response = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller, "PUT", abts);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                if (baseResponse.code == 200)
                {
                    BaseResponse<ProductExtraDetailsDto> ExtraDetailsbaseResponse = new BaseResponse<ProductExtraDetailsDto>();
                    ProductExtraDetailsDto productExtraDetails = new ProductExtraDetailsDto();
                    productExtraDetails.AssignBrandStatus = model.Status;
                    productExtraDetails.BrandId = model.BrandId;
                    productExtraDetails.SellerId = model.SellerID;
                    productExtraDetails.Mode = "updateAssignBrands";
                    var ExtraDetailsresponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
                    ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);
                }

                tmp = tmp.Where(p => p.Id == model.Id).ToList();


                BaseResponse<SellerListModel> sellerResponse = new BaseResponse<SellerListModel>();

                var tmpresponse = helper.ApiCall(IDServerUrl, EndPoints.SellerById + "?ID=" + model.SellerID, "GET", null);
                sellerResponse = sellerResponse.JsonParseRecord(tmpresponse);

                SellerListModel Resetdata = new SellerListModel();
                Resetdata = (SellerListModel)sellerResponse.Data;

                if (tmp[0].Status.ToString().ToLower() == "request for approval" && model.Status.ToString().ToLower() == "active")
                {
                    #region send mail
                    MailSendSES objses = new MailSendSES(_configuration);

                    string subject = model.BrandName + "Brand request approved - Hashkart";
                    string htmlBody = "";
                    List<string> ReceiverEmail = new List<string>();

                    ReceiverEmail.Add(Resetdata.Email);

                    StreamReader reader = new StreamReader("Resources" + "\\EmailTemplate" + "\\seller" + "\\brand_approved.html");
                    string readFile = reader.ReadToEnd();

                    var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";


                    readFile = readFile.Replace("{{image_server_path}}", baseUrl);
                    readFile = readFile.Replace("{{seller_name}}", Resetdata.FirstName + " " + Resetdata.LastName);
                    readFile = readFile.Replace("{{brand_name}}", model.BrandName);

                    htmlBody = readFile;

                    objses.sendMail(subject, htmlBody, ReceiverEmail);

                    #endregion
                }
                else if (tmp[0].Status.ToString().ToLower() == "active" && model.Status.ToString().ToLower() == "inactive")
                {
                    #region send mail
                    MailSendSES objses = new MailSendSES(_configuration);

                    string subject = model.BrandName + "Brand request not approved - Hashkart";
                    string htmlBody = "";
                    List<string> ReceiverEmail = new List<string>();

                    ReceiverEmail.Add(Resetdata.Email);

                    StreamReader reader = new StreamReader("Resources" + "\\EmailTemplate" + "\\seller" + "\\brand_notapproved.html");
                    string readFile = reader.ReadToEnd();

                    var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}/";


                    readFile = readFile.Replace("{{image_server_path}}", baseUrl);
                    readFile = readFile.Replace("{{seller_name}}", Resetdata.FirstName + " " + Resetdata.LastName);
                    readFile = readFile.Replace("{{brand_name}}", model.BrandName);

                    htmlBody = readFile;

                    objses.sendMail(subject, htmlBody, ReceiverEmail);

                    #endregion
                }

            }
            return baseResponse;
        }

        public BaseResponse<AssignBrandToSeller> DeleteAssignBrands(int? id)
        {
            var temp = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<AssignBrandToSeller> templist = (List<AssignBrandToSeller>)baseResponse.Data;
            if (templist.Any())
            {
                var _templist = templist.FirstOrDefault();

                BaseResponse<SellerProduct> sellerproduct = new BaseResponse<SellerProduct>();

                var _tempproducts = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "?sellerId=" + _templist.SellerID + "&brandId=" + _templist.BrandId + "&isDeleted=" + false, "GET", null);
                sellerproduct = sellerproduct.JsonParseList(_tempproducts);
                List<SellerProduct> _sellertemplist = (List<SellerProduct>)sellerproduct.Data;
                if (_sellertemplist.Count > 0)
                {
                    baseResponse = baseResponse.ChildAlreadyExists("Products", "AssignBrandsToSeller");
                }
                else
                {
                    var response = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return baseResponse;
        }

        public BaseResponse<AssignBrandToSeller> GetAssignBrands(int? PageIndex, int? PageSize)
        {
            var response = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize, "GET", null);
            return baseResponse.JsonParseList(response);
        }
        public BaseResponse<AssignBrandToSeller> GetAssignBrandsById(int id)
        {
            var response = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?Id=" + id, "GET", null);
            return baseResponse.JsonParseRecord(response);
        }
        public BaseResponse<AssignBrandToSeller> GetAssignBrandsByUserID(string SellerID, int BrandID, string? Status, int? pageIndex = 1, int? pageSize = 10)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(Status))
            {
                url += "&Status=" + Status;
            }
            if (BrandID != 0)
            {
                url += "&BrandId=" + BrandID;
            }

            var response = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?SellerID=" + SellerID + "&PageIndex=" + pageIndex + "&PageSize=" + pageSize + url, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<AssignBrandToSeller> GetAssignBrandsByUserIDandBrandId(string SellerID, int BrandID, string? Status)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(Status))
            {
                url = "&Status=" + Status;
            }
            var response = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?SellerID=" + SellerID + "&BrandId=" + BrandID + url, "GET", null);
            return baseResponse.JsonParseRecord(response);
        }

        public BaseResponse<AssignBrandToSeller> GetAssignBrandsByBrandId(int BrandID, string? Status)
        {
            string url = string.Empty;
            if (!string.IsNullOrEmpty(Status))
            {
                url = "&Status=" + Status;
            }
            var response = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?BrandId=" + BrandID + url, "GET", null);
            return baseResponse.JsonParseRecord(response);
        }

        public BaseResponse<AssignBrandToSeller> GetAssignBrandsByStatus(string status, int? PageIndex, int? PageSize)
        {
            var response = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + "&status=" + status, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<AssignBrandToSeller> Search(string? searchtext, string? sellerID, string? status, int? PageIndex, int? PageSize)
        {
            string url = string.Empty;

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url = "&searchtext=" + HttpUtility.UrlEncode(searchtext);
            }

            if (!string.IsNullOrEmpty(sellerID) && sellerID != "")
            {
                url = "&SellerID=" + sellerID;
            }

            if (!string.IsNullOrEmpty(status))
            {
                url += "&status=" + status;
            }

            var response = helper.ApiCall(_URL, EndPoints.AssignBrandToSeller + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        [NonAction]
        public string UploadDoc(string SellerName, string BrandName, IFormFile FileName)
        {
            try
            {
                var file = FileName;

                var folderName = Path.Combine("Resources", "Brandcertificate");

                if (file != null)
                {
                    string a = DateTime.Now.ToString("_ddMMyyyyHHmmssfff");
                    var fileName = SellerName + "_" + BrandName + "_Certificate" + a;

                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    fileName = imageUpload.UploadImageAndDocs(fileName, folderName, FileName);

                    return fileName;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [NonAction]
        public string UpdateDocFile(string OldName, string SellerName, string BrandName, IFormFile? FileName)
        {
            try
            {
                if (FileName != null)
                {
                    var file = FileName;
                    string a = DateTime.Now.ToString("_ddMMyyyyHHmmssfff");
                    var fileName = SellerName + "_" + BrandName + "_Certificate" + a;
                    var folderName = Path.Combine("Resources", "Brandcertificate");
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    fileName = imageUpload.UpdateUploadImageAndDocs(OldName, fileName, folderName, FileName);

                    return fileName;
                }
                else
                {
                    string fileName = null;
                    if (OldName != null)
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
