using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Infrastructure.Helper
{
    internal class Procedures
    {
        public static string Country { get; set; } = "sp_Country";
        public static string GetCountry { get; set; } = "sp_GetCountry";
        public static string City { get; set; } = "sp_City";
        public static string GetCity { get; set; } = "sp_GetCity";
        public static string State { get; set; } = "sp_State";
        public static string GetState { get; set; } = "sp_GetState";
        public static string Delivery { get; set; } = "sp_Delivery";
        public static string GetDelivery { get; set; } = "sp_GetDelivery";
        public static string Brand { get; set; } = "sp_Brand";
        public static string GetBrand { get; set; } = "sp_GetBrand";
        public static string AssignBrandToSeller { get; set; } = "sp_AssignBrandToSeller";
        public static string GetAssignBrandToSeller { get; set; } = "sp_GetAssignBrandToSeller";

        public static string KYCDetails { get; set; } = "sp_KYCDetails";
        public static string GetKYCDetails { get; set; } = "sp_GetKYCDetails";
        public static string GetBasicKYCDetails { get; set; } = "sp_GetBasicKYCDetails";
        public static string Reports { get; set; } = "sp_Reports";
        public static string Warehouse { get; set; } = "sp_Warehouse";
        public static string GetWarehouse { get; set; } = "sp_GetWarehouse";
        public static string Address { get; set; } = "sp_Address";
        public static string GetAddress { get; set; } = "sp_GetAddress";
        public static string GSTInfo { get; set; } = "sp_GSTInfo";
        public static string GetGSTInfo { get; set; } = "sp_GetGSTInfo";
        public static string Cart { get; set; } = "sp_Cart";
        public static string GetCart { get; set; } = "sp_GetCart";
        public static string GetAbandonedCart { get; set; } = "sp_GetAbandonedCart";
        public static string Wishlist { get; set; } = "sp_Wishlist";
        public static string GetWishlist { get; set; } = "sp_GetWishlist";
        public static string GetKycCount { get; set; } = "sp_GetKycCounts";
        public static string GetBrandCount { get; set; } = "sp_GetBrandCounts";
        public static string sp_UserDetails { get; set; } = "sp_UserDetails";
        public static string sp_GetUserDetails { get; set; } = "sp_GetUserDetails";
    }
}
