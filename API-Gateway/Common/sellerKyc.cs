using API_Gateway.Helper;
using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.Catalogue;
using API_Gateway.Models.Entity.User;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace API_Gateway.Common
{
    public class sellerKyc
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public string IdserverURL = string.Empty;
        public string UserURL = string.Empty;
        public string CatelogueURL = string.Empty;
        private readonly HttpContext _httpContext;
        private ApiHelper helper;
        public sellerKyc(IConfiguration configuration, HttpContext httpContext)
        {
            _httpContext = httpContext;
            _configuration = configuration;
            IdserverURL = _configuration.GetSection("ApiURLs").GetSection("IDServerApi").Value;
            UserURL = _configuration.GetSection("ApiURLs").GetSection("UserApi").Value;
            CatelogueURL = _configuration.GetSection("ApiURLs").GetSection("CatalogueApi").Value;
            helper = new ApiHelper(_httpContext);
        }

        public SellerKycDetails BindSeller(string sellerID)
        {
            //var response = bindSellerDetails(sellerID);
            //var response1 = bindSellerKYC(sellerID, response);
            //var response2 = bindGstinfo(sellerID, response1);
            //var response3 = bindWareHouse(sellerID, response2);
            //var response4 = bindBrandName(sellerID, response3);
            UserDetailsDTO sellerKycList = new UserDetailsDTO();
            sellerKycListDetail seller = new sellerKycListDetail(_configuration, _httpContext);
            List<UserDetailsDTO> lst = seller.bindSellerDetails(null, null, null, null, sellerID, 0, 0);
            sellerKycList = lst.FirstOrDefault();
            SellerKycDetails slst = new SellerKycDetails();
            slst.UserID = sellerKycList?.UserId;
            slst.KYCFor = sellerKycList?.KYCFor;
            slst.DisplayName = sellerKycList?.DisplayName;
            slst.OwnerName = sellerKycList?.OwnerName;
            slst.ContactPersonName = sellerKycList?.ContactPersonName;
            slst.ContactPersonMobileNo = sellerKycList?.ContactPersonMobileNo;
            slst.PanCardNo = sellerKycList?.PanCardNo;
            slst.NameOnPanCard = sellerKycList?.NameOnPanCard;
            slst.DateOfBirth = sellerKycList?.DateOfBirth;
            slst.AadharCardNo = sellerKycList?.AadharCardNo;
            slst.IsUserWithGST = sellerKycList?.IsUserWithGST ?? false;
            slst.TypeOfCompany = sellerKycList?.TypeOfCompany;
            slst.BussinessType = sellerKycList?.BussinessType;
            slst.MSMENo = sellerKycList?.MSMENo;
            slst.AccountNo = sellerKycList?.AccountNo;
            slst.AccountHolderName = sellerKycList?.AccountHolderName;
            slst.BankName = sellerKycList?.BankName;
            slst.AccountType = sellerKycList?.AccountType;
            slst.IFSCCode = sellerKycList?.IFSCCode;
            slst.Logo = sellerKycList?.Logo;
            slst.DigitalSign = sellerKycList?.DigitalSign;
            slst.CancelCheque = sellerKycList?.CancelCheque;
            slst.PanCardDoc = sellerKycList?.PanCardDoc;
            slst.MSMEDoc = sellerKycList?.MSMEDoc;
            slst.AadharCardFrontDoc = sellerKycList?.AadharCardFrontDoc;
            slst.AadharCardBackDoc = sellerKycList?.AadharCardBackDoc;
            slst.ShipmentBy = sellerKycList?.ShipmentBy;
            slst.ShipmentChargesPaidBy = sellerKycList?.ShipmentChargesPaidBy;
            slst.ShipmentChargesPaidByName = sellerKycList?.ShipmentChargesPaidByName;
            slst.Note = sellerKycList?.Note;
            slst.Status = sellerKycList?.KycStatus;
            slst.SellerStatus = sellerKycList?.UserStatus;
            slst.SellerId = sellerKycList?.UserId ?? "";
            slst.FullName = sellerKycList?.FirstName + " " + sellerKycList?.LastName;
            slst.UserName = sellerKycList?.Email ?? "";
            slst.PhoneNumber = sellerKycList?.Phone ?? "";
            slst.gSTInfos = sellerKycList.GSTInfoDetails.IsNullOrEmpty() ? null : JsonConvert.DeserializeObject<List<GSTInfo>>(sellerKycList.GSTInfoDetails.ToString());
            slst.wareHouses = sellerKycList.WarehouseDetails.IsNullOrEmpty() ? null : JsonConvert.DeserializeObject<List<Warehouse>>(sellerKycList.WarehouseDetails.ToString());
            slst.Brands = sellerKycList.SellerBrand.IsNullOrEmpty() ? null : JsonConvert.DeserializeObject<List<AssignBrandToSeller>>(sellerKycList.SellerBrand.ToString());

            return slst;
        }

        public SellerKycDetails bindSellerDetails(string sellerID)
        {
            SellerKycDetails sellerKycDetails = new SellerKycDetails();
            BaseResponse<SellerListModel> baseResponse = new BaseResponse<SellerListModel>();
            var response = helper.ApiCall(IdserverURL, EndPoints.SellerById + "?Id=" + sellerID + "&pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
            baseResponse = baseResponse.JsonParseRecord(response);

            SellerListModel sellerDetails = (SellerListModel)baseResponse.Data;
            sellerKycDetails.UserID = sellerDetails.Id;
            sellerKycDetails.FullName = sellerDetails.FirstName + " " + sellerDetails.LastName;
            sellerKycDetails.PhoneNumber = sellerDetails.MobileNo;
            sellerKycDetails.UserName = sellerDetails.UserName;
            sellerKycDetails.SellerStatus = sellerDetails.Status;

            return sellerKycDetails;
        }

        public SellerKycDetails bindSellerKYC(string sellerID, SellerKycDetails model)
        {

            BaseResponse<KYCDetails> baseResponse = new BaseResponse<KYCDetails>();
            var response = helper.ApiCall(UserURL, EndPoints.KYCDetails + "?UserID=" + sellerID + "&pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);

            var check = (List<KYCDetails>)baseResponse.Data;
            KYCDetails kycDetails = null;
            if (check.Any())
            {
                BaseResponse<ChargesPaidByLibrary> baseResponseCharges = new BaseResponse<ChargesPaidByLibrary>();
                var response1 = helper.ApiCall(CatelogueURL, EndPoints.ChargesPaidBy + "?pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
                baseResponseCharges = baseResponseCharges.JsonParseList(response1);
                List<ChargesPaidByLibrary> Charges = (List<ChargesPaidByLibrary>)baseResponseCharges.Data;
                kycDetails = check.FirstOrDefault();
                model.UserID = kycDetails.UserID;
                model.KYCFor = kycDetails.KYCFor;
                model.DisplayName = kycDetails.DisplayName;
                model.OwnerName = kycDetails.OwnerName;
                model.ContactPersonName = kycDetails.ContactPersonName;
                model.ContactPersonMobileNo = kycDetails.ContactPersonMobileNo;
                model.PanCardNo = kycDetails.PanCardNo;
                model.NameOnPanCard = kycDetails.NameOnPanCard;
                model.DateOfBirth = kycDetails.DateOfBirth;
                model.AadharCardNo = kycDetails.AadharCardNo;
                model.IsUserWithGST = kycDetails.IsUserWithGST;
                model.TypeOfCompany = kycDetails.TypeOfCompany;
                model.CompanyRegitrationNo = kycDetails.CompanyRegistrationNo;
                model.BussinessType = kycDetails.BussinessType;
                model.MSMENo = kycDetails.MSMENo;
                model.AccountNo = kycDetails.AccountNo;
                model.AccountHolderName = kycDetails.AccountHolderName;
                model.BankName = kycDetails.BankName;
                model.AccountType = kycDetails.AccountType;
                model.IFSCCode = kycDetails.IFSCCode;
                model.Logo = kycDetails.Logo;
                model.DigitalSign = kycDetails.DigitalSign;
                model.CancelCheque = kycDetails.CancelCheque;
                model.PanCardDoc = kycDetails.PanCardDoc;
                model.MSMEDoc = kycDetails.MSMEDoc;
                model.AadharCardFrontDoc = kycDetails.AadharCardFrontDoc;
                model.AadharCardBackDoc = kycDetails.AadharCardBackDoc;
                model.Note = kycDetails.Note;
                model.Status = kycDetails.Status;
                model.ShipmentBy = kycDetails.ShipmentBy;
                model.ShipmentChargesPaidBy = kycDetails.ShipmentChargesPaidBy;
                model.ShipmentChargesPaidByName = kycDetails.ShipmentChargesPaidBy != null ? Charges.Where(p => p.Id == kycDetails.ShipmentChargesPaidBy).FirstOrDefault().Name : null;
                model.SellerId = kycDetails.UserID;
            }





            return model;
        }

        public SellerKycDetails bindGstinfo(string sellerID, SellerKycDetails model)
        {

            BaseResponse<GSTInfo> baseResponse = new BaseResponse<GSTInfo>();
            var response = helper.ApiCall(UserURL, EndPoints.GSTInfo + "?UserID=" + sellerID + "&pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);

            List<GSTInfo> gSTInfos = (List<GSTInfo>)baseResponse.Data;

            model.gSTInfos = gSTInfos;

            return model;
        }

        public SellerKycDetails bindWareHouse(string sellerID, SellerKycDetails model)
        {

            BaseResponse<Warehouse> baseResponse = new BaseResponse<Warehouse>();
            var response = helper.ApiCall(UserURL, EndPoints.Warehouse + "?UserID=" + sellerID + "&pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);

            List<Warehouse> warehouses = (List<Warehouse>)baseResponse.Data;

            model.wareHouses = warehouses;

            return model;
        }

        public SellerKycDetails bindBrandName(string sellerID, SellerKycDetails model)
        {

            BaseResponse<AssignBrandToSeller> baseResponse = new BaseResponse<AssignBrandToSeller>();
            var response = helper.ApiCall(UserURL, EndPoints.AssignBrandToSeller + "?SellerID=" + sellerID + "&pageIndex=" + 0 + "&pageSize=" + 0, "GET", null);
            baseResponse = baseResponse.JsonParseList(response);

            List<AssignBrandToSeller> brandName = (List<AssignBrandToSeller>)baseResponse.Data;

            model.Brands = brandName;

            return model;
        }
    }
}
