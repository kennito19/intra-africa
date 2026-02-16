

namespace Catalogue.Infrastructure.Helper
{
    internal class Procedures
    {
        public static string Category { get; set; } = "sp_Category";
        public static string GetCategory { get; set; } = "sp_GetCategory";
        public static string Color { get; set; } = "sp_Color";
        public static string GetColor { get; set; } = "sp_GetColor";
        public static string Size { get; set; } = "sp_Size";
        public static string GetSize { get; set; } = "sp_GetSize";
        public static string HSNCode { get; set; } = "sp_HSNCode";
        public static string GetHSNCode { get; set; } = "sp_GetHSNCode";
        public static string TaxType { get; set; } = "sp_TaxType";
        public static string GetTaxType { get; set; } = "sp_GetTaxType";
        public static string TaxTypeValue { get; set; } = "sp_TaxTypeValue";
        public static string GetTaxTypeValue { get; set; } = "sp_GetTaxTypeValue";
        public static string WeightSlabs { get; set; } = "sp_WeightSlabs";
        public static string GetWeightSlabs { get; set; } = "sp_GetWeightSlabs";
        public static string ReturnPolicy { get; set; } = "sp_ReturnPolicy";
        public static string GetReturnPolicy { get; set; } = "sp_GetReturnPolicy";
        public static string ReturnPolicyDetail { get; set; } = "sp_ReturnPolicyDetail";
        public static string GetReturnPolicyDetail { get; set; } = "sp_GetReturnPolicyDetail";
        public static string AssignReturnPolicyToCategory { get; set; } = "sp_AssignReturnPolicyToCategory";
        public static string GetAssignReturnPolicyToCategory { get; set; } = "sp_GetAssignReturnPolicyToCategory";
        public static string AssignSpecificationToCategory { get; set; } = "sp_AssignSpecificationToCategory";
        public static string GetAssignSpecificationToCategory { get; set; } = "sp_GetAssignSpecificationToCategory";
        public static string AssignSizeValueToCategory { get; set; } = "sp_AssignSizeValueToCategory";
        public static string GetAssignSizeValueToCategory { get; set; } = "sp_GetAssignSizeValueToCategory";
        public static string ProductRollback { get; set; } = "sp_ProductRollback";
        public static string Products { get; set; } = "sp_Product";
        public static string GetProducts { get; set; } = "sp_GetProduct";
        public static string GetAddInExistingList { get; set; } = "sp_GetAddInExistingList";
        public static string GetUserProductDetails { get; set; } = "sp_GetUserProductDetails";
        public static string SellerProducts { get; set; } = "sp_SellerProduct";
        public static string GetSellerProducts { get; set; } = "sp_GetSellerProduct";
        public static string ProductWareHouse { get; set; } = "sp_ProductWareHouse";
        public static string UpdateWarehouseStock { get; set; } = "sp_UpdateWarehouseStock";
        public static string GetProductWareHouse { get; set; } = "sp_GetProductWareHouse";
        public static string GetSizeWiseWarehouse { get; set; } = "GetSizeWiseWarehouse";
        public static string ProductColorMapping { get; set; } = "sp_ProductColorMapping";
        public static string GetProductColorMapping { get; set; } = "sp_GetProductColorMapping";
        public static string ProductImages { get; set; } = "sp_ProductImages";
        public static string GetProductImages { get; set; } = "sp_GetProductImages";
        //public static string ProductVideoLinks { get; set; } = "sp_ProductVideoLinks";
        //public static string GetProductVideoLinks { get; set; } = "sp_GetProductVideoLinks";
        public static string ProductPrice { get; set; } = "sp_ProductPrice";
        public static string GetProductPrice { get; set; } = "sp_GetProductPrice";
        public static string GetArchiveProducts { get; set; } = "sp_GetArchiveProducts";

        // Home Page Section
        public static string ManageLayouts { get; set; } = "sp_ManageLayouts";
        public static string GetManageLayouts { get; set; } = "sp_GetManageLayouts";

        public static string ManageLayoutTypes { get; set; } = "sp_ManageLayoutTypes";
        public static string GetManageLayoutTypes { get; set; } = "sp_GetManageLayoutTypes";

        public static string ManageLayoutTypesDetails { get; set; } = "sp_ManageLayoutTypesDetails";
        public static string GetManageLayoutTypesDetails { get; set; } = "sp_GetManageLayoutTypesDetails";

        public static string ManageHomePageDetails { get; set; } = "sp_ManageHomePageDetails";
        public static string GetManageHomePageDetails { get; set; } = "sp_GetManageHomePageDetails";

        public static string ManageHomePageSections { get; set; } = "sp_ManageHomePageSections";
        public static string GetManageHomePageSections { get; set; } = "sp_GetManageHomePageSections";
        public static string GetFrontHomepageDetails { get; set; } = "sp_GetFrontHomepageDetails";

        public static string GetManageProductHomePageSections { get; set; } = "sp_GetProductForHomePage";

        public static string ManageHeadermenu { get; set; } = "sp_ManageHeadermenu";
		public static string GetManageHeadermenu { get; set; } = "sp_GetManageHeadermenu";

		public static string ManageSubmenu { get; set; } = "sp_ManageSubmenu";
		public static string GetManageSubmenu { get; set; } = "sp_GetManageSubmenu";

        public static string ManageStaticPages { get; set; } = "sp_ManageStaticPages";
        public static string GetManageStaticPages { get; set; } = "sp_GetManageStaticPages";

        public static string ManageConfigKey { get; set; } = "sp_ManageConfigKey";
        public static string GetManageConfigKey { get; set; } = "sp_GetManageConfigKey";

        public static string ManageConfigValue { get; set; } = "sp_ManageConfigValues";
        public static string GetManageConfigValue { get; set; } = "sp_GetManageConfigValues";

        public static string LendingPage { get; set; } = "sp_ManageLendingPageType";
        public static string GetLendingPage { get; set; } = "sp_GetManageLendingPageType";

        public static string LendingPageSections { get; set; } = "sp_ManageLendingPageSections";
        public static string GetLendingPageSections { get; set; } = "sp_GetManageLendingPageSections";

        public static string LendingPageSectionDetails { get; set; } = "sp_ManageLendingPageSectionDetails";
        public static string GetLendingPageSectionDetails { get; set; } = "sp_GetManageLendingPageSectionDetails";

        public static string ManageCollection { get; set; } = "sp_ManageCollection";
        public static string GetManageCollection { get; set; } = "sp_GetManageCollection";

        public static string ManageCollectionMapping { get; set; } = "sp_ManageCollectionMapping";
        public static string GetManageCollectionMapping { get; set; } = "sp_GetManageCollectionMapping";

        public static string ManageFlashSalePriceMaster { get; set; } = "sp_ManageFlashSalePriceMaster";
        public static string GetManageFlashSalePriceMaster { get; set; } = "sp_GetManageFlashSalePriceMaster";

        public static string ManageOffers { get; set; } = "sp_ManageOffers";
        public static string GetManageOffers { get; set; } = "sp_GetManageOffers";

        public static string ManageOffersMapping { get; set; } = "sp_ManageOffersMapping";
        public static string GetManageOffersMapping { get; set; } = "sp_GetManageOffersMapping";

        public static string GetMasterProductListDetail { get; set; } = "sp_GetMasterProductsWithDetail";
        public static string GetUserProducts { get; set; } = "sp_GetUserProductList";
        public static string Specification { get; set; } = "sp_Specification";
        public static string GetSpecification { get; set; } = "sp_GetSpecification";
        //public static string AssignSpecificationToCategory { get; set; } = "sp_AssignSpecificationToCategory";
        //public static string GetAssignSpecificationToCategory { get; set; } = "sp_GetAssignSpecificationToCategory";
        public static string AssignSpecValuesToCategory { get; set; } = "sp_AssignSpecValuesToCategory";
        public static string GetAssignSpecValuesToCategory { get; set; } = "sp_GetAssignSpecValuesToCategory";
        public static string ProductSpecificationMapping { get; set; } = "sp_ProductSpecificationMapping";
        public static string GetProductSpecificationMapping { get; set; } = "sp_GetProductSpecificationMapping";
        public static string Commission { get; set; } = "sp_Commission";
        public static string GetCommission { get; set; } = "sp_GetCommission";
        public static string Extracharges { get; set; } = "sp_Extracharges";
        public static string GetExtracharges { get; set; } = "sp_GetExtracharges";
        public static string GetCatExtracharges { get; set; } = "sp_GetCatExtraCharges";
        public static string ChargesPaidBy { get; set; } = "sp_ChargesPaidBy";
        public static string GetChargesPaidBy { get; set; } = "sp_GetChargesPaidBy";
       
        public static string GetCommissionByCategoryId { get; set; } = "sp_GetCommissionByCategoryId";
        public static string GetProductsCount { get; set; } = "sp_GetProductsCount";
        public static string GetCollectionProductList { get; set; } = "sp_GetCollectionProductList";
        public static string AssignTaxRateToHSNCode { get; set; } = "sp_AssignTaxRateToHSNCode";
        public static string GetAssignTaxRateToHSNCode { get; set; } = "sp_GetAssignTaxRateToHSNCode";
        public static string CheckAssignSpecsToCategory { get; set; } = "sp_CheckAssignSpecsToCategory";

        public static string ManageLayoutOptions { get; set; } = "sp_ManageLayoutOptions";
        public static string GetManageLayoutOptions { get; set; } = "sp_GetManageLayoutOptions";
        public static string GetSellerProductDetails { get; set; } = "sp_GetSellerProductDetails";
        public static string GetProductCompare { get; set; } = "sp_GetProductCompare";
        public static string GetBrandsAndProductForProductCompare { get; set; } = "sp_GetBrandsAndProductForProductCompare";
        public static string Reports { get; set; } = "sp_Reports";
        public static string ManageAppConfig { get; set; } = "sp_ManageAppConfig";
        public static string GetManageAppConfig { get; set; } = "sp_GetManageAppConfig";
        public static string GetCategoryWithParent { get; set; } = "sp_GetCategoryWithParent";
        public static string ProductView { get; set; } = "sp_ProductView";
        public static string GetProductView { get; set; } = "sp_GetProductView";
        public static string GetCartDetails { get; set; } = "sp_GetCartDetails";
        public static string TaxMapping { get; set; } = "sp_TaxMapping";
        public static string GetTaxMapping { get; set; } = "sp_GetTaxMapping";
        public static string GetProductBulkDetails { get; set; } = "sp_GetProductBulkDetails";
        public static string GetProductListForBulkDownload { get; set; } = "sp_GetProductListForBulkDownload";
        public static string GetProductListForBulkDownloadForStock { get; set; } = "sp_GetProductListForBulkDownloadForStock";

        public static string ManageHomePage { get; set; } = "sp_ManageHomePage";
        public static string GetManageHomePage { get; set; } = "sp_GetManageHomePage";
    }
}
