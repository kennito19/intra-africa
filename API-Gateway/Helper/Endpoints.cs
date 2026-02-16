namespace API_Gateway.Helper
{
    public class EndPoints
    {

        public static string GenerateNoAuth { get; set; } = "api/Token/GenerateNoAuthToken";
        public static string Category { get; set; } = "api/Category";
        public static string Country { get; set; } = "api/Country";
        public static string SizeLibrary { get; set; } = "api/Size";
        public static string Color { get; set; } = "api/Color";
        public static string ChargesPaidBy { get; set; } = "api/ChargesPaidBy";
        public static string State { get; set; } = "api/State";
        public static string City { get; set; } = "api/City";
        public static string Specification { get; set; } = "api/Specification";
        public static string HSNCode { get; set; } = "api/HSNCode";
        public static string ShippingCharges { get; set; } = "api/ShippingCharges";
        public static string WeightSlab { get; set; } = "api/WeightSlab";
        public static string AssignSpecToCat { get; set; } = "api/AssignSpecToCategory";
        public static string ExtraCharges { get; set; } = "api/ExtraCharges";
        public static string CatExtraCharges { get; set; } = "api/ExtraCharges/CatExtraCharges";
        public static string AssignSpecValuesToCategory { get; set; } = "api/AssignSpecValuesToCategory";
        public static string TaxType { get; set; } = "api/TaxType";
        public static string TaxTypeValue { get; set; } = "api/TaxTypeValue";
        public static string AssignSizeValueToCategory { get; set; } = "api/AssignSizeValueToCategory";
        public static string Delivery { get; set; } = "api/Delivery";
        public static string CommissionCharges { get; set; } = "api/CommissionCharges";
        public static string ProductSpeifications { get; set; } = "api/ProductSpeifications";
        public static string ReturnPolicy { get; set; } = "api/ReturnPolicy";
        public static string AssignReturnPolicyToCatagory { get; set; } = "api/AssignReturnPolicyToCatagory";
        public static string ReturnPolicyDetail { get; set; } = "api/ReturnPolicyDetail";
        public static string ProductsImage { get; set; } = "api/ProductsImage";
        public static string Product { get; set; } = "api/Product";
        public static string ProductRollback { get; set; } = "api/ProductRollback";
        public static string ProductsVideoLinks { get; set; } = "api/ProductsVideoLinks";
        public static string ProductPriceMaster { get; set; } = "api/ProductPriceMaster";
        public static string SellerProduct { get; set; } = "api/SellerProduct";
        public static string ProductWarehouse { get; set; } = "api/ProductWarehouse";
        public static string ProductColorMapping { get; set; } = "api/ProductsColorMapping";
        public static string ProductSpecificationMapping { get; set; } = "api/ProductSpecificationMapping";
        public static string GSTInfo { get; set; } = "api/GSTInfo";
        public static string KYCDetails { get; set; } = "api/KYCDetails";
        public static string Warehouse { get; set; } = "api/Warehouse";
        public static string Address { get; set; } = "api/Address";
        public static string AssignBrandToSeller { get; set; } = "api/AssignBrandToSeller";
        public static string Brand { get; set; } = "api/Brand";
        public static string Wishlist { get; set; } = "api/Wishlist";
        public static string Cart { get; set; } = "api/Cart";
        public static string OrderStatus { get; set; } = "api/OrderStatusLibrary";
        public static string RejectionType { get; set; } = "api/RejectionType";
        public static string IssueType { get; set; } = "api/IssueType";
        public static string IssueReason { get; set; } = "api/IssueReason";
        public static string AssignSellerHyperlocal { get; set; } = "api/AssignSellerHyperlocal";
        public static string Orders { get; set; } = "api/Orders";
        public static string OrderItems { get; set; } = "api/OrderItems";
        public static string OrderPaymentInfo { get; set; } = "api/OrderPaymentInfo";
        public static string PaymentInfoOrderMapping { get; set; } = "api/PaymentInfoOrderMapping";

        public static string OrderTaxInfo { get; set; } = "api/OrderTaxInfo";
        public static string OrderWiseExtraCharges { get; set; } = "api/OrderWiseExtraCharges";
        public static string OrderTrackDetails { get; set; } = "api/OrderTrackDetails";


        //public static string AdminSignIn { get; set; } = "api/Admin/Account/AdminSignIn";

        public static string AdminChangePassword { get; set; } = "api/Admin/changepassword";
        public static string ForgotPassword { get; set; } = "api/Account/forgotpassword";
        public static string SellerForgotPassword { get; set; } = "api/Account/sellerforgotpassword";
        public static string CustomerForgotPassword { get; set; } = "api/Account/customerforgotpassword";
        public static string ResetPassword { get; set; } = "reset-password";
        public static string GetNewAccessToken { get; set; } = "GetNewAccessToken";
        public static string CreatePageRoleModule { get; set; } = "api/Page/CreateModule/CreatePage";
        public static string EditPageRoleModule { get; set; } = "api/Page/EditModule/EditPage";
        public static string DeletePageRoleModule { get; set; } = "api/Page/DeleteModule/DeletePage";
        public static string AssignPageRoles { get; set; } = "api/Page/AssignPageRoles/AssignPageRoles";
        public static string GetAssignedPageByRoleType { get; set; } = "api/Page/GetAssignedPagesByRoleType/GetAssignedPagesByRoleType";
        public static string GetAssignedPageByUserId { get; set; } = "api/Page/GetAssignedPagesByUserId/GetAssignedPagesByUserId";
        public static string GetAssignedPagesByUserIdandRoleTypeId { get; set; } = "api/Page/GetAssignedPagesByUserIdandRoleTypeId";
        public static string CreateRoleType { get; set; } = "api/Page/CreateUserType/CreateRoleType";
        public static string EditRoleType { get; set; } = "api/Page/EditUserType/EditRoleType";
        public static string DeleteRoleType { get; set; } = "api/Page/DeleteUserType/DeleteRoleType";
        public static string GetAllRoleTypes { get; set; } = "api/Page/GetAllUserType/GetAllRoleTypes";
        public static string GetRoleTypeById { get; set; } = "api/Page/GetRoleTypeById/GetRoleTypeById";
        public static string GetPageAccessByRoleType { get; set; } = "api/Page/GetPageAccessByRoleType/GetPageAccessByRoleType";
        public static string GetAllPages { get; set; } = "api/Page/GetAllPages/GetAllPages";
        public static string GetPageById { get; set; } = "api/Page/GetPageModuleById/GetPageById";
        public static string AdminSignIn { get; set; } = "api/admin/signin";
        public static string AdminSignUp { get; set; } = "api/admin/signup";
        public static string AdminList { get; set; } = "api/Admin";
        public static string logout { get; set; } = "api/Account/logout";
        public static string userSession { get; set; } = "api/Account/userSession";



        public static string SellerSignIn { get; set; } = "api/seller/signin";
        public static string SellerSignUp { get; set; } = "api/seller/signup";
        public static string UpdateSellerSignUp { get; set; } = "api/seller/UpdateSellerSignUp";
        public static string SellerList { get; set; } = "api/Seller";
        public static string SellerById { get; set; } = "api/Seller/ById";
        public static string SellerSearch { get; set; } = "api/Seller/search";
        public static string SellerChangePassword { get; set; } = "api/Seller/changepassword";

        public static string CustomerSignIn { get; set; } = "api/customer/signin";
        public static string CustomerSignInViaOtp { get; set; } = "api/customer/signinViaOtp";
        public static string CustomerSignInViaEmail { get; set; } = "api/customer/signInViaEmailId";
        public static string GenerateMobileOtp { get; set; } = "api/customer/GenerateMobileOtp";
        public static string CustomerSignUp { get; set; } = "api/customer/signUp";
        public static string CustomerList { get; set; } = "api/customer";
        public static string CustomerById { get; set; } = "api/customer/ById";
        public static string CustomerByEmail { get; set; } = "api/customer/ByEmail";
        public static string CustomerSearch { get; set; } = "api/customer/search";
        public static string CustomerChangePassword { get; set; } = "api/Customer/changepassword";
        public static string CustomerUpdate { get; set; } = "api/Customer/Update";
        public static string GuestUser { get; set; } = "api/Customer/GuestUser";


        public static string GetUserRoleClaims { get; set; } = "api/Account/GetUserRoleClaims";
        public static string GetUserSpecificClaims { get; set; } = "api/Account/GetUserSpecificClaims";

        public static string ProductVariantMapping { get; set; } = "api/OrderProductVariantMapping";
        public static string ProductSeriesNo { get; set; } = "api/OrderWiseProductSeriesNo";

        public static string ShipmentInfo { get; set; } = "api/OrderShipmentInfo";
        public static string Invoice { get; set; } = "api/OrderInvoice";
        public static string CancelReturn { get; set; } = "api/OrderCancelReturn";
        public static string ReturnAction { get; set; } = "api/OrderReturnAction";
        public static string OrderPackage { get; set; } = "api/OrderPackage";
        public static string OrderRefund { get; set; } = "api/OrderRefund";
        public static string ReturnShipmentInfo { get; set; } = "api/ReturnShipmentInfo";
        public static string OrderReturnAction { get; set; } = "api/OrderReturnAction";
        public static string ManageLayouts { get; set; } = "api/ManageLayouts";
        public static string ManageLayoutTypes { get; set; } = "api/ManageLayoutTypes";
        public static string ManageLayoutTypesDetails { get; set; } = "api/ManageLayoutTypesDetails";
        public static string Staticpages { get; set; } = "api/ManageStaticPages";
        public static string ManageHomePageSections { get; set; } = "api/ManageHomePageSections";
        public static string ManageProductHomePageSections { get; set; } = "api/ManageHomePageSections/getProductHomePageSection";
        public static string ManageHomePageDetails { get; set; } = "api/ManageHomePageDetails";
        public static string ManageHeaderMenu { get; set; } = "api/ManageHeaderMenu";
        public static string ManageSubMenu { get; set; } = "api/ManageSubMenu";
        public static string ManageConfigKey { get; set; } = "api/ManageConfigKey";
        public static string ManageConfigValue { get; set; } = "api/ManageConfigValue";
        public static string ManageLendingPage { get; set; } = "api/LendingPage";
        public static string ManageLendingPageSection { get; set; } = "api/LendingPageSections";
        public static string ManageLendingPageSectionsDetail { get; set; } = "api/LendingPageSectionDetails";
        public static string MasterProductListDetail { get; set; } = "api/Product/getProductList";
        public static string ManageCollection { get; set; } = "api/ManageCollection";
        public static string ManageCollectionMapping { get; set; } = "api/ManageCollectionMapping";
        public static string FlashSalePriceMaster { get; set; } = "api/FlashSalePriceMaster";
        public static string ManageOffers { get; set; } = "api/ManageOffers";
        public static string ManageOffersMapping { get; set; } = "api/ManageOffersMapping";
        public static string ManageNotification { get; set; } = "api/Notification";
        public static string UserProductList { get; set; } = "api/UserProducts";
        public static string GetProductCounts { get; set; } = "api/ProductCounts";
        public static string GetKycCounts { get; set; } = "api/KycCount";
        public static string GetBrandCounts { get; set; } = "api/BrandCount";
        public static string GetCollectionProductsList { get; set; } = "api/CollectionProductsList";
        public static string AssignTaxRateToHSNCode { get; set; } = "api/AssignTaxRateToHSNCode";
        public static string CheckAssignSpecsToCategory { get; set; } = "api/CheckAssignSpecsToCategory";
        public static string log { get; set; } = "api/ActivityLog";
        public static string ManageLayoutOption { get; set; } = "api/ManageLayoutOption";
        public static string OrderCount { get; set; } = "api/OrderCount";
        public static string Reports { get; set; } = "api/Reports";
        public static string AppConfig { get; set; } = "api/AppConfig";
        public static string ProductView { get; set; } = "api/ProductView";
        public static string CartGetCartList { get; set; } = "api/getCartDetails/GetCartList";
        public static string TaxMapping { get; set; } = "api/TaxMapping";
        public static string ProductBulkDownload { get; set; } = "api/Product/getProductBulkDownload";
        public static string ProductBulkDownloadForStock { get; set; } = "api/Product/getProductBulkDownloadForStock";

        public static string UserDetails { get; set; } = "api/UserDetails";
        public static string ManageHomePage { get; set; } = "api/ManageHomePage";

    }
}

public class Mode
{
    public static string Get { get; set; } = "get";
    public static string Check { get; set; } = "check";

}

