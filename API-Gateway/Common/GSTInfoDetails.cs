using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Web;

namespace API_Gateway.Common
{
    public class GSTInfoDetails
    {
        private readonly HttpContext _httpContext;
        public string _URL = string.Empty;
        BaseResponse<GSTInfo> baseResponse = new BaseResponse<GSTInfo>();
        private readonly IConfiguration _configuration;
        private ApiHelper helper;
        public string CatelogueURL = string.Empty;
        public GSTInfoDetails(HttpContext httpContext, string URL, IConfiguration configuration)
        {
            _httpContext = httpContext;
            _URL = URL;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
        }

        public BaseResponse<GSTInfo> SaveGSTInfo([FromForm] GSTInfoDTO model, string UserId, bool? AllowMultipleGst = null)
        {
            var temp = helper.ApiCall(_URL, EndPoints.GSTInfo + "?GSTNo=" + model.GSTNo, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<GSTInfo> tmp = baseResponse.Data as List<GSTInfo>;
            if (tmp.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                if (AllowMultipleGst == true)
                {
                    if (model.IsHeadOffice)
                    {
                        BaseResponse<GSTInfo> gbaseResponse = new BaseResponse<GSTInfo>();
                        var response1 = helper.ApiCall(_URL, EndPoints.GSTInfo + "?UserID=" + model.UserID + "&IsHeadOffice=" + true, "GET", null);
                        gbaseResponse = gbaseResponse.JsonParseRecord(response1);
                        if (gbaseResponse.code == 200)
                        {
                            GSTInfo gstData = gbaseResponse.Data as GSTInfo;
                            gstData.IsHeadOffice = false;
                            response1 = helper.ApiCall(_URL, EndPoints.GSTInfo, "PUT", gstData);
                        }
                    }

                    GSTInfo gst = new GSTInfo();
                    gst.UserID = model.UserID;
                    gst.GSTNo = model.GSTNo;
                    gst.LegalName = model.LegalName;
                    gst.TradeName = model.TradeName;
                    gst.GSTType = model.GSTType;
                    gst.GSTDoc = UploadDoc(model.GSTNo, model.FileName);
                    gst.RegisteredAddressLine1 = model.RegisteredAddressLine1;
                    gst.RegisteredAddressLine2 = model.RegisteredAddressLine2;
                    gst.RegisteredLandmark = model.RegisteredLandmark;
                    gst.RegisteredPincode = model.RegisteredPincode;
                    gst.RegisteredStateId = model.RegisteredStateId;
                    gst.RegisteredCityId = model.RegisteredCityId;
                    gst.RegisteredCountryId = model.RegisteredCountryId;
                    gst.TCSNo = model.TCSNo;
                    gst.Status = model.Status;
                    gst.IsHeadOffice = model.IsHeadOffice;
                    gst.CreatedBy = UserId;
                    gst.CreatedAt = DateTime.Now;

                    var response = helper.ApiCall(_URL, EndPoints.GSTInfo, "POST", gst);
                    baseResponse = baseResponse.JsonParseInputResponse(response);

                }
                else
                {
                    temp = helper.ApiCall(_URL, EndPoints.GSTInfo + "?UserID=" + model.UserID, "GET", null);
                    baseResponse = baseResponse.JsonParseList(temp);
                    tmp = baseResponse.Data as List<GSTInfo>;
                    if (tmp.Any())
                    {
                        baseResponse = baseResponse.AlreadyExists();
                    }
                    else
                    {
                        GSTInfo gst = new GSTInfo();
                        gst.UserID = model.UserID;
                        gst.GSTNo = model.GSTNo;
                        gst.LegalName = model.LegalName;
                        gst.TradeName = model.TradeName;
                        gst.GSTType = model.GSTType;
                        gst.GSTDoc = UploadDoc(model.GSTNo, model.FileName);
                        gst.RegisteredAddressLine1 = model.RegisteredAddressLine1;
                        gst.RegisteredAddressLine2 = model.RegisteredAddressLine2;
                        gst.RegisteredLandmark = model.RegisteredLandmark;
                        gst.RegisteredPincode = model.RegisteredPincode;
                        gst.RegisteredStateId = model.RegisteredStateId;
                        gst.RegisteredCityId = model.RegisteredCityId;
                        gst.RegisteredCountryId = model.RegisteredCountryId;
                        gst.TCSNo = model.TCSNo;
                        gst.Status = model.Status;
                        gst.IsHeadOffice = model.IsHeadOffice;
                        gst.CreatedBy = UserId;
                        gst.CreatedAt = DateTime.Now;

                        var response = helper.ApiCall(_URL, EndPoints.GSTInfo, "POST", gst);
                        baseResponse = baseResponse.JsonParseInputResponse(response);
                    }
                }
            }
            return baseResponse;
        }

        public BaseResponse<GSTInfo> UpdateGSTInfo([FromForm] GSTInfoDTO model, string UserId, bool? AllowMultipleGst = null)
        {
            var temp = helper.ApiCall(_URL, EndPoints.GSTInfo + "?GSTNo=" + model.GSTNo, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<GSTInfo> tmp = baseResponse.Data as List<GSTInfo>;
            if (tmp.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                if (AllowMultipleGst == true)
                {
                    if (model.IsHeadOffice)
                    {
                        BaseResponse<GSTInfo> gbaseResponse = new BaseResponse<GSTInfo>();
                        var response1 = helper.ApiCall(_URL, EndPoints.GSTInfo + "?UserID=" + model.UserID + "&IsHeadOffice=" + true, "GET", null);
                        gbaseResponse = gbaseResponse.JsonParseRecord(response1);
                        if (gbaseResponse.code == 200)
                        {
                            GSTInfo gstData = gbaseResponse.Data as GSTInfo;
                            if (gstData.Id != model.Id)
                            {
                                gstData.IsHeadOffice = false;
                                response1 = helper.ApiCall(_URL, EndPoints.GSTInfo, "PUT", gstData);
                            }
                        }
                    }

                    var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?Id=" + model.Id, "GET", null);
                    baseResponse = baseResponse.JsonParseRecord(response);
                    GSTInfo gst = baseResponse.Data as GSTInfo;
                    gst.UserID = model.UserID;
                    gst.GSTNo = model.GSTNo;
                    gst.LegalName = model.LegalName;
                    gst.TradeName = model.TradeName;
                    gst.GSTType = model.GSTType;
                    gst.GSTDoc = UpdateDocFile(model.GSTDoc, model.GSTNo, model.FileName);
                    gst.RegisteredAddressLine1 = model.RegisteredAddressLine1;
                    gst.RegisteredAddressLine2 = model.RegisteredAddressLine2;
                    gst.RegisteredLandmark = model.RegisteredLandmark;
                    gst.RegisteredPincode = model.RegisteredPincode;
                    gst.RegisteredStateId = model.RegisteredStateId;
                    gst.RegisteredCityId = model.RegisteredCityId;
                    gst.RegisteredCountryId = model.RegisteredCountryId;
                    gst.TCSNo = model.TCSNo;
                    gst.Status = model.Status;
                    gst.IsHeadOffice = model.IsHeadOffice;
                    gst.ModifiedBy = UserId;
                    gst.ModifiedAt = DateTime.Now;

                    response = helper.ApiCall(_URL, EndPoints.GSTInfo, "PUT", gst);
                    baseResponse = baseResponse.JsonParseInputResponse(response);

                    if (baseResponse.code == 200)
                    {
                        BaseResponse<ProductExtraDetailsDto> ExtraDetailsbaseResponse = new BaseResponse<ProductExtraDetailsDto>();
                        ProductExtraDetailsDto productExtraDetails = new ProductExtraDetailsDto();
                        productExtraDetails.TradeName = model.LegalName;
                        productExtraDetails.LegalName = model.TradeName;
                        productExtraDetails.SellerId = model.UserID;
                        productExtraDetails.Mode = "updateSellerGST";
                        var ExtraDetailsresponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
                        ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);
                    }

                }
                else
                {
                    temp = helper.ApiCall(_URL, EndPoints.GSTInfo + "?UserID=" + model.UserID, "GET", null);
                    baseResponse = baseResponse.JsonParseList(temp);
                    tmp = baseResponse.Data as List<GSTInfo>;
                    if (tmp.Where(x => x.Id != model.Id).Any())
                    {
                        baseResponse = baseResponse.AlreadyExists();
                    }
                    else
                    {
                        var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?Id=" + model.Id, "GET", null);
                        baseResponse = baseResponse.JsonParseRecord(response);
                        GSTInfo gst = baseResponse.Data as GSTInfo;
                        gst.UserID = model.UserID;
                        gst.GSTNo = model.GSTNo;
                        gst.LegalName = model.LegalName;
                        gst.TradeName = model.TradeName;
                        gst.GSTType = model.GSTType;
                        gst.GSTDoc = UpdateDocFile(model.GSTDoc, model.GSTNo, model.FileName);
                        gst.RegisteredAddressLine1 = model.RegisteredAddressLine1;
                        gst.RegisteredAddressLine2 = model.RegisteredAddressLine2;
                        gst.RegisteredLandmark = model.RegisteredLandmark;
                        gst.RegisteredPincode = model.RegisteredPincode;
                        gst.RegisteredStateId = model.RegisteredStateId;
                        gst.RegisteredCityId = model.RegisteredCityId;
                        gst.RegisteredCountryId = model.RegisteredCountryId;
                        gst.TCSNo = model.TCSNo;
                        gst.Status = model.Status;
                        gst.IsHeadOffice = model.IsHeadOffice;
                        gst.ModifiedBy = UserId;
                        gst.ModifiedAt = DateTime.Now;

                        response = helper.ApiCall(_URL, EndPoints.GSTInfo, "PUT", gst);
                        baseResponse = baseResponse.JsonParseInputResponse(response);

                        if (baseResponse.code == 200)
                        {
                            BaseResponse<ProductExtraDetailsDto> ExtraDetailsbaseResponse = new BaseResponse<ProductExtraDetailsDto>();
                            ProductExtraDetailsDto productExtraDetails = new ProductExtraDetailsDto();
                            productExtraDetails.TradeName = model.LegalName;
                            productExtraDetails.LegalName = model.TradeName;
                            productExtraDetails.SellerId = model.UserID;
                            productExtraDetails.Mode = "updateSellerGST";
                            var ExtraDetailsresponse = helper.ApiCall(CatelogueURL, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
                            ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);
                        }
                    }
                }
            }

            return baseResponse;
        }

        public BaseResponse<GSTInfo> DeleteGSTInfo(int id)
        {
            var temp = helper.ApiCall(_URL, EndPoints.GSTInfo + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<GSTInfo> tmp = baseResponse.Data as List<GSTInfo>;
            if (tmp.Any())
            {
                //var userID = tmp.FirstOrDefault().UserID;
                //temp = helper.ApiCall(_URL, EndPoints.GSTInfo + "?UserID=" + userID, "GET", null);
                //baseResponse = baseResponse.JsonParseList(temp);
                //List<GSTInfo> userWiseData = baseResponse.Data as List<GSTInfo>;
                var gstData = tmp.FirstOrDefault();
                if (!Convert.ToBoolean(gstData.IsHeadOffice))
                {
                    var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);

                    response = helper.ApiCall(_URL, EndPoints.Warehouse + "?gstInfoId=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
                else
                {
                    BaseResponse<GSTInfo> tempRes = new BaseResponse<GSTInfo>();
                    tempRes.code = 201;
                    tempRes.Message = "You Can't Delete Primary GST Info";
                    tempRes.Data = "";
                    baseResponse = tempRes;
                }
            }
            else
            {
                baseResponse = baseResponse.NotExist();
            }
            return baseResponse;
        }

        public BaseResponse<GSTInfo> GetGSTInfo(int? PageIndex = 1, int? PageSize = 10)
        {
            var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<GSTInfo> GSTInfoById(int id)
        {
            var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?id=" + id, "GET", null);
            return baseResponse.JsonParseRecord(response);
        }

        public BaseResponse<GSTInfo> GSTInfoByUserID(string UserID, bool IsAllowMultiGST = false, bool? IsHeadOffice = null)
        {
            string url = string.Empty;

            if (IsHeadOffice != null)
            {
                url += "&IsHeadOffice=" + IsHeadOffice;
            }

            if (IsAllowMultiGST)
            {
                var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?UserID=" + UserID + url, "GET", null);
                return baseResponse.JsonParseList(response);
            }
            else
            {
                var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?UserID=" + UserID + url, "GET", null);
                return baseResponse.JsonParseRecord(response);
            }
        }

        public BaseResponse<GSTInfo> GSTInfoByGSTNo(string GSTNo)
        {
            var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?GSTNo=" + GSTNo, "GET", null);
            return baseResponse.JsonParseRecord(response);
        }

        public BaseResponse<GSTInfo> GSTInfoByPincode(string Pincode)
        {
            var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?Pincode=" + Pincode, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<GSTInfo> GSTInfoByCityId(string CityId)
        {
            var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?CityId=" + CityId, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<GSTInfo> GSTInfoByStateId(string StateId)
        {
            var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?StateId=" + StateId, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<GSTInfo> GSTInfoByCountryId(string CountryId)
        {
            var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?CountryId=" + CountryId, "GET", null);
            return baseResponse.JsonParseList(response);
        }

        public BaseResponse<GSTInfo> GetGSTInfobySearch(string? UserID = null, bool? IsHeadOffice = null, string? searchtext = null, int? PageIndex = 1, int? PageSize = 10)
        {
            string url = string.Empty;

            if (!string.IsNullOrEmpty(searchtext) && searchtext != "")
            {
                url += "&searchText=" + HttpUtility.UrlEncode(searchtext);
            }
            if (IsHeadOffice != null)
            {
                url += "&IsHeadOffice=" + IsHeadOffice;
            }
            if (!string.IsNullOrEmpty(UserID) && UserID != "")
            {
                url += "&UserID=" + UserID;
            }
            var response = helper.ApiCall(_URL, EndPoints.GSTInfo + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize + url, "GET", null);
            return baseResponse.JsonParseList(response);
        }


        [NonAction]
        public string UploadDoc(string Name, IFormFile FileName)
        {
            try
            {
                var file = FileName;

                var folderName = Path.Combine("Resources", "Kyc", "GSTInfo");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (file != null)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    var newFileName = Name + Guid.NewGuid().ToString();
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    newFileName = imageUpload.UploadImageAndDocs(newFileName, folderName, FileName);
                    return newFileName;
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
        public string UpdateDocFile(string OldName, string Name, IFormFile? FileName)
        {

            try
            {
                if (FileName != null)
                {
                    var file = FileName;
                    var fileExtension = Path.GetExtension(file.FileName);
                    var newFileName = Name + Guid.NewGuid().ToString();
                    var folderName = Path.Combine("Resources", "Kyc", "GSTInfo");
                    ImageUpload imageUpload = new ImageUpload(_configuration);
                    newFileName = imageUpload.UpdateUploadImageAndDocs(OldName, newFileName, folderName, FileName);

                    return newFileName;
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
