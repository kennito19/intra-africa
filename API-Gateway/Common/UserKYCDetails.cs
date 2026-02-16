using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace API_Gateway.Common
{

    public class UserKYCDetails
    {
        private readonly HttpContext _httpContext;
        public string _URL = string.Empty;
        public string _CatalougeURL = string.Empty;
        BaseResponse<KYCDetails> baseResponse = new BaseResponse<KYCDetails>();
        private ApiHelper helper;
        private readonly IConfiguration _configuration;
        public static string IDServerUrl = string.Empty;
        public UserKYCDetails(HttpContext httpContext, string URL, string catalougeUrl, IConfiguration configuration)
        {
            _httpContext = httpContext;
            _URL = URL;
            _CatalougeURL = catalougeUrl;
            helper = new ApiHelper(_httpContext);
            _configuration = configuration;
            IDServerUrl = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
        }

        public BaseResponse<KYCDetails> SaveKYC(KYCDetails model, string UserId, bool AllowAadharcard)
        {
            if (AllowAadharcard)
            {
                if (string.IsNullOrEmpty(model.AadharCardNo))
                {
                    baseResponse = baseResponse.InvalidInput("Aadhar Card No is required!");
                }

                if (string.IsNullOrEmpty(model.AadharCardFrontDoc))
                {
                    baseResponse = baseResponse.InvalidInput("Aadhar Card Front Doc is required!");
                }

                if (string.IsNullOrEmpty(model.AadharCardBackDoc))
                {
                    baseResponse = baseResponse.InvalidInput("Aadhar Card Back Doc is required!");
                }
            }
            var temp = helper.ApiCall(_URL, EndPoints.KYCDetails + "?UserID=" + model.UserID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<KYCDetails> tmp = (List<KYCDetails>)baseResponse.Data;
            if (tmp.Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                KYCDetails kyc = new KYCDetails();
                kyc.UserID = model.UserID;
                kyc.KYCFor = model.KYCFor;
                kyc.DisplayName = model.DisplayName;
                kyc.OwnerName = model.OwnerName;
                kyc.ContactPersonName = model.ContactPersonName;
                kyc.ContactPersonMobileNo = model.ContactPersonMobileNo;
                kyc.PanCardNo = model.PanCardNo;
                kyc.NameOnPanCard = model.NameOnPanCard;
                kyc.DateOfBirth = model.DateOfBirth;
                kyc.AadharCardNo = model.AadharCardNo;
                kyc.IsUserWithGST = model.IsUserWithGST;
                kyc.TypeOfCompany = model.TypeOfCompany;
                kyc.CompanyRegistrationNo = model.CompanyRegistrationNo;
                kyc.BussinessType = model.BussinessType;
                kyc.MSMENo = model.MSMENo;
                kyc.AccountNo = model.AccountNo;
                kyc.AccountHolderName = model.AccountHolderName;
                kyc.BankName = model.BankName;
                kyc.AccountType = model.AccountType;
                kyc.IFSCCode = model.IFSCCode;
                kyc.Logo = model.Logo;
                kyc.DigitalSign = model.DigitalSign;
                kyc.CancelCheque = model.CancelCheque;
                kyc.PanCardDoc = model.PanCardDoc;
                kyc.MSMEDoc = model.MSMEDoc;
                kyc.AadharCardFrontDoc = model.AadharCardFrontDoc;
                kyc.AadharCardBackDoc = model.AadharCardBackDoc;
                kyc.ShipmentBy = model.ShipmentBy;
                kyc.ShipmentChargesPaidBy = model.ShipmentChargesPaidBy;
                kyc.Note = model.Note;
                kyc.Status = "Pending";
                kyc.CreatedBy = UserId;
                kyc.CreatedAt = DateTime.Now;

                ImageUpload img = new ImageUpload(_configuration);
                if (model.PanCardDoc != null && model.PanCardDoc != "")
                {
                    img.UploadDocs("PanCardDoc", model.PanCardDoc);
                }
                if (model.AadharCardBackDoc != null && model.AadharCardBackDoc != "")
                {
                    img.UploadDocs("AadharCardBackDoc", model.AadharCardBackDoc);
                }
                if (model.AadharCardFrontDoc != null && model.AadharCardFrontDoc != "")
                {
                    img.UploadDocs("AadharCardFrontDoc", model.AadharCardFrontDoc);
                }
                if (model.MSMEDoc != null && model.MSMEDoc != "")
                {
                    img.UploadDocs("MSMEDoc", model.MSMEDoc);
                }
                if (model.CancelCheque != null && model.CancelCheque != "")
                {
                    img.UploadDocs("CancelCheque", model.CancelCheque);
                }
                if (model.Logo != null && model.Logo != "")
                {
                    img.UploadDocs("Logo", model.Logo);
                }
                if (model.DigitalSign != null && model.DigitalSign != "")
                {
                    img.UploadDocs("DigitalSign", model.DigitalSign);
                }
                var response = helper.ApiCall(_URL, EndPoints.KYCDetails, "POST", kyc);
                baseResponse = baseResponse.JsonParseInputResponse(response);
            }

            return baseResponse;
        }

        public BaseResponse<KYCDetails> UpdateKYC(KYCDetails model, string UserId, bool AllowAadharcard)
        {
            var temp = helper.ApiCall(_URL, EndPoints.KYCDetails + "?UserID=" + model.UserID, "GET", null);
            baseResponse = baseResponse.JsonParseList(temp);
            List<KYCDetails> tmp = (List<KYCDetails>)baseResponse.Data;
            if (tmp.Where(x => x.Id != model.Id).Any())
            {
                baseResponse = baseResponse.AlreadyExists();
            }
            else
            {
                var response = helper.ApiCall(_URL, EndPoints.KYCDetails + "?Id=" + model.Id, "GET", null);
                baseResponse = baseResponse.JsonParseRecord(response);
                KYCDetails kyc = (KYCDetails)baseResponse.Data;

                if (AllowAadharcard)
                {
                    if (string.IsNullOrEmpty(kyc.AadharCardNo))
                    {
                        if (string.IsNullOrEmpty(model.AadharCardNo))
                        {
                            baseResponse = baseResponse.InvalidInput("Aadhar Card No is required!");
                        }
                    }

                    if (string.IsNullOrEmpty(kyc.AadharCardFrontDoc))
                    {
                        if (string.IsNullOrEmpty(model.AadharCardFrontDoc))
                        {
                            baseResponse = baseResponse.InvalidInput("Aadhar Card Front Doc is required!");
                        }
                    }

                    if (string.IsNullOrEmpty(kyc.AadharCardBackDoc))
                    {
                        if (string.IsNullOrEmpty(model.AadharCardBackDoc))
                        {
                            baseResponse = baseResponse.InvalidInput("Aadhar Card Back Doc is required!");
                        }
                    }
                }

                kyc.UserID = model.UserID;
                kyc.KYCFor = model.KYCFor;
                kyc.DisplayName = model.DisplayName;
                kyc.OwnerName = model.OwnerName;
                kyc.ContactPersonName = model.ContactPersonName;
                kyc.ContactPersonMobileNo = model.ContactPersonMobileNo;
                kyc.PanCardNo = model.PanCardNo;
                kyc.NameOnPanCard = model.NameOnPanCard;
                kyc.DateOfBirth = model.DateOfBirth;
                kyc.IsUserWithGST = model.IsUserWithGST;
                kyc.TypeOfCompany = model.TypeOfCompany;
                kyc.CompanyRegistrationNo = model.CompanyRegistrationNo;
                kyc.BussinessType = model.BussinessType;
                kyc.MSMENo = model.MSMENo;
                kyc.AccountNo = model.AccountNo;
                kyc.AccountHolderName = model.AccountHolderName;
                kyc.BankName = model.BankName;
                kyc.AccountType = model.AccountType;
                kyc.IFSCCode = model.IFSCCode;
                kyc.Logo = model.Logo;
                kyc.DigitalSign = model.DigitalSign;
                kyc.CancelCheque = model.CancelCheque;
                kyc.PanCardDoc = model.PanCardDoc;
                kyc.MSMEDoc = model.MSMEDoc;
                if (!string.IsNullOrEmpty(model.AadharCardNo))
                {
                    kyc.AadharCardNo = model.AadharCardNo;
                }
                if (!string.IsNullOrEmpty(model.AadharCardFrontDoc))
                {
                    kyc.AadharCardFrontDoc = model.AadharCardFrontDoc;
                }
                if (!string.IsNullOrEmpty(model.AadharCardBackDoc))
                {
                    kyc.AadharCardBackDoc = model.AadharCardBackDoc;
                }
                kyc.ShipmentBy = model.ShipmentBy;
                kyc.ShipmentChargesPaidBy = model.ShipmentChargesPaidBy;
                kyc.Note = model.Note;
                kyc.Status = model.Status;
                kyc.ModifiedBy = UserId;
                kyc.ModifiedAt = DateTime.Now;

                ImageUpload img = new ImageUpload(_configuration);
                if (CheckImage(model.PanCardDoc))
                {
                    img.UploadDocs("PanCardDoc", model.PanCardDoc);
                }
                if (CheckImage(model.AadharCardBackDoc))
                {
                    img.UploadDocs("AadharCardBackDoc", model.AadharCardBackDoc);
                }
                if (CheckImage(model.AadharCardFrontDoc))
                {
                    img.UploadDocs("AadharCardFrontDoc", model.AadharCardFrontDoc);
                }
                if (CheckImage(model.MSMEDoc))
                {
                    img.UploadDocs("MSMEDoc", model.MSMEDoc);
                }
                if (CheckImage(model.CancelCheque))
                {
                    img.UploadDocs("CancelCheque", model.CancelCheque);
                }
                if (CheckImage(model.Logo))
                {
                    img.UploadDocs("Logo", model.Logo);
                }
                if (CheckImage(model.DigitalSign))
                {
                    img.UploadDocs("DigitalSign", model.DigitalSign);
                }

                response = helper.ApiCall(_URL, EndPoints.KYCDetails, "PUT", kyc);
                baseResponse = baseResponse.JsonParseInputResponse(response);
                if (baseResponse.code == 200)
                {
                    BaseResponse<ProductExtraDetailsDto> ExtraDetailsbaseResponse = new BaseResponse<ProductExtraDetailsDto>();
                    ProductExtraDetailsDto productExtraDetails = new ProductExtraDetailsDto();
                    productExtraDetails.DisplayName = model.DisplayName;
                    productExtraDetails.DigitalSign = model.DigitalSign;
                    productExtraDetails.ShipmentBy = model.ShipmentBy;
                    productExtraDetails.ShipmentChargesPaidBy = model.ShipmentChargesPaidBy.ToString();
                    productExtraDetails.ShipmentChargesPaidByName = model.ShipmentChargesPaidByName;
                    productExtraDetails.KycStatus = model.Status;
                    productExtraDetails.SellerId = model.UserID;
                    productExtraDetails.Mode = "updateSellerKyc";
                    var ExtraDetailsresponse = helper.ApiCall(_CatalougeURL, EndPoints.SellerProduct + "/UpdateExtraDetails", "PUT", productExtraDetails);
                    ExtraDetailsbaseResponse = ExtraDetailsbaseResponse.JsonParseInputResponse(ExtraDetailsresponse);
                }
            }
            return baseResponse;
        }

        public BaseResponse<KYCDetails> DeleteKYC(int? id, string? userID)
        {
            if (id != 0)
            {
                var temp = helper.ApiCall(_URL, EndPoints.KYCDetails + "?Id=" + id, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<KYCDetails> tmp = (List<KYCDetails>)baseResponse.Data;
                if (tmp.Any())
                {
                    var response = helper.ApiCall(_URL, EndPoints.KYCDetails + "?Id=" + id, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
                else
                {
                    baseResponse = baseResponse.NotExist();
                }
            }
            else if (!string.IsNullOrEmpty(userID))
            {
                var temp = helper.ApiCall(_URL, EndPoints.KYCDetails + "?userID=" + userID, "GET", null);
                baseResponse = baseResponse.JsonParseList(temp);
                List<KYCDetails> tmp = (List<KYCDetails>)baseResponse.Data;
                if (tmp.Any())
                {
                    var response = helper.ApiCall(_URL, EndPoints.KYCDetails + "?userID=" + userID, "DELETE", null);
                    baseResponse = baseResponse.JsonParseInputResponse(response);
                }
                else
                {
                    baseResponse = baseResponse.NotExist();
                }
            }
            else
            {
                baseResponse.code = 201;
                baseResponse.Message = "Plesse Enter any one Id.";
                baseResponse.Data = "";
            }

            return baseResponse;
        }

        public BaseResponse<KYCDetails> GetKyc(int? PageIndex, int? PageSize)
        {
            var response = helper.ApiCall(_URL, EndPoints.KYCDetails + "?PageIndex=" + PageIndex + "&PageSize=" + PageSize, "GET", null);
            //return baseResponse.JsonParseList(response);
            baseResponse = baseResponse.JsonParseList(response);
            if (baseResponse.code == 200)
            {
                BaseResponse<ChargesPaidByLibrary> baseResponseCharges = new BaseResponse<ChargesPaidByLibrary>();
                var response1 = helper.ApiCall(_CatalougeURL, EndPoints.ChargesPaidBy + "?pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
                baseResponseCharges = baseResponseCharges.JsonParseList(response1);
                List<KYCDetails> kYCDetails = (List<KYCDetails>)baseResponse.Data;
                List<ChargesPaidByLibrary> Charges = (List<ChargesPaidByLibrary>)baseResponseCharges.Data;
                var result = kYCDetails.Select(x => new
                {
                    RowNumber = x.RowNumber,
                    PageCount = x.PageCount,
                    RecordCount = x.RecordCount,
                    Id = x.Id,
                    UserID = x.UserID,
                    KYCFor = x.KYCFor,
                    DisplayName = x.DisplayName,
                    OwnerName = x.OwnerName,
                    ContactPersonName = x.ContactPersonName,
                    ContactPersonMobileNo = x.ContactPersonMobileNo,
                    PanCardNo = x.PanCardNo,
                    NameOnPanCard = x.NameOnPanCard,
                    DateOfBirth = x.DateOfBirth,
                    AadharCardNo = x.AadharCardNo,
                    IsUserWithGST = x.IsUserWithGST,
                    TypeOfCompany = x.TypeOfCompany,
                    CompanyRegistrationNo = x.CompanyRegistrationNo,
                    BussinessType = x.BussinessType,
                    MSMENo = x.MSMENo,
                    AccountNo = x.AccountNo,
                    AccountHolderName = x.AccountHolderName,
                    BankName = x.BankName,
                    IFSCCode = x.IFSCCode,
                    Logo = x.Logo,
                    DigitalSign = x.DigitalSign,
                    CancelCheque = x.CancelCheque,
                    PanCardDoc = x.PanCardDoc,
                    MSMEDoc = x.MSMEDoc,
                    AadharCardFrontDoc = x.AadharCardFrontDoc,
                    AadharCardBackDoc = x.AadharCardBackDoc,
                    ShipmentBy = x.ShipmentBy,
                    ShipmentChargesPaidBy = x.ShipmentChargesPaidBy != null ? x.ShipmentChargesPaidBy : null,
                    ShipmentChargesPaidByName = x.ShipmentChargesPaidBy != null ? Charges.Where(p => p.Id == x.ShipmentChargesPaidBy).FirstOrDefault().Name.ToString() : null,
                    Note = x.Note,
                    Status = x.Status,
                    IsDeleted = x.IsDeleted,
                    CreatedBy = x.CreatedBy,
                    CreatedAt = x.CreatedAt,
                    ModifiedBy = x.ModifiedBy,
                    ModifiedAt = x.ModifiedAt,
                    DeletedBy = x.DeletedBy,
                    DeletedAt = x.DeletedAt
                }).ToList();
                baseResponse.Data = result;
            }
            return baseResponse;
        }
        public BaseResponse<KYCDetails> GetKycById(int id)
        {
            var response = helper.ApiCall(_URL, EndPoints.KYCDetails + "?Id=" + id, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            if (baseResponse.code == 200)
            {
                BaseResponse<ChargesPaidByLibrary> baseResponseCharges = new BaseResponse<ChargesPaidByLibrary>();
                var response1 = helper.ApiCall(_CatalougeURL, EndPoints.ChargesPaidBy + "?pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
                baseResponseCharges = baseResponseCharges.JsonParseList(response1);
                KYCDetails kYCDetails = (KYCDetails)baseResponse.Data;

                if (kYCDetails.ShipmentChargesPaidBy != null && kYCDetails.ShipmentChargesPaidBy != 0)
                {
                    if (baseResponseCharges.code == 200)
                    {
                        List<ChargesPaidByLibrary> Charges = (List<ChargesPaidByLibrary>)baseResponseCharges.Data;
                        string CName = kYCDetails.ShipmentChargesPaidBy != null ? Charges.Where(p => p.Id == kYCDetails.ShipmentChargesPaidBy).FirstOrDefault().Name.ToString() : null;
                        kYCDetails.ShipmentChargesPaidByName = CName;
                        baseResponse.Data = kYCDetails;
                    }
                    else
                    {
                        kYCDetails.ShipmentChargesPaidByName = null;
                        baseResponse.Data = kYCDetails;
                    }
                }
            }
            return baseResponse;
        }
        public BaseResponse<KYCDetails> GetKycByUserID(string UserID)
        {
            ApiHelper helper = new ApiHelper(_httpContext);
            var response = helper.ApiCall(_URL, EndPoints.KYCDetails + "?UserID=" + UserID, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);
            if (baseResponse.code == 200)
            {
                BaseResponse<ChargesPaidByLibrary> baseResponseCharges = new BaseResponse<ChargesPaidByLibrary>();
                var response1 = helper.ApiCall(_CatalougeURL, EndPoints.ChargesPaidBy + "?pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
                baseResponseCharges = baseResponseCharges.JsonParseList(response1);
                KYCDetails kYCDetails = (KYCDetails)baseResponse.Data;

                if (kYCDetails.ShipmentChargesPaidBy != null && kYCDetails.ShipmentChargesPaidBy != 0)
                {
                    if (baseResponseCharges.code == 200)
                    {
                        List<ChargesPaidByLibrary> Charges = (List<ChargesPaidByLibrary>)baseResponseCharges.Data;
                        string CName = kYCDetails.ShipmentChargesPaidBy != null ? Charges.Where(p => p.Id == kYCDetails.ShipmentChargesPaidBy).FirstOrDefault().Name.ToString() : null;
                        kYCDetails.ShipmentChargesPaidByName = CName;
                        baseResponse.Data = kYCDetails;
                    }
                    else
                    {
                        kYCDetails.ShipmentChargesPaidByName = null;
                        baseResponse.Data = kYCDetails;
                    }
                }
            }
            return baseResponse;
        }

        public bool CheckImage(string Name)
        {
            var tempFolder = Path.Combine("Resources", "Temp");
            string imagePath = Path.Combine(tempFolder, Name);
            if (File.Exists(imagePath))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
