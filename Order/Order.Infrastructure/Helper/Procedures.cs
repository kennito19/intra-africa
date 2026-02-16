using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infrastructure.Helper
{
    public class Procedures
    {
        public static string OrderStatusLibrary { get; set; } = "sp_OrderStatusLibrary";
        public static string GetOrderStatusLibrary { get; set; } = "sp_GetOrderStatusLibrary";

        public static string RejectionType { get; set; } = "sp_RejectionType";
        public static string GetRejectionType { get; set; } = "sp_GetRejectionType";

        public static string IssueType { get; set; } = "sp_IssueType";
        public static string GetIssueType { get; set; } = "sp_GetIssueType";

        public static string IssueReason { get; set; } = "sp_IssueReason";
        public static string GetIssueReason { get; set; } = "sp_GetIssueReason";

        public static string Orders { get; set; } = "sp_Orders";
        public static string GetOrders { get; set; } = "sp_GetOrders";

        public static string OrderItems { get; set; } = "sp_OrderItems";
        public static string GetOrderItems { get; set; } = "sp_GetOrderItems";

        public static string OrderTaxInfo { get; set; } = "sp_OrderTaxInfo";
        public static string GetOrderTaxInfo { get; set; } = "sp_GetOrderTaxInfo";

        public static string OrderTrackDetails { get; set; } = "sp_OrderTrackDetails";
        public static string GetOrderTrackDetails { get; set; } = "sp_GetOrderTrackDetails";

        public static string OrderProductVariantMapping { get; set; } = "sp_OrderProductVariantMapping";
        public static string GetOrderProductVariantMapping { get; set; } = "sp_GetOrderProductVariantMapping";

        public static string OrderPackage { get; set; } = "sp_OrderPackage";
        public static string GetOrderPackage { get; set; } = "sp_GetOrderPackage";

        public static string OrderShipmentInfo { get; set; } = "sp_OrderShipmentInfo";
        public static string GetOrderShipmentInfo { get; set; } = "sp_GetOrderShipmentInfo";

        public static string OrderInvoice { get; set; } = "sp_OrderInvoice";
        public static string GetOrderInvoice { get; set; } = "sp_GetOrderInvoice";

        public static string OrderCancelReturn { get; set; } = "sp_OrderCancelReturn";
        public static string GetOrderCancelReturn { get; set; } = "sp_GetOrderCancelReturn";

        public static string OrderReturnAction { get; set; } = "sp_OrderReturnAction";
        public static string GetOrderReturnAction { get; set; } = "sp_GetOrderReturnAction";

        public static string OrderRefund { get; set; } = "sp_OrderRefund";
        public static string GetOrderRefund { get; set; } = "sp_GetOrderRefund";

        public static string ReturnShipmentInfo { get; set; } = "sp_ReturnShipmentInfo";
        public static string GetReturnShipmentInfo { get; set; } = "sp_GetReturnShipmentInfo";

        public static string OrderWiseExtraCharges { get; set; } = "sp_OrderWiseExtraCharges";
        public static string GetOrderWiseExtraCharges { get; set; } = "sp_GetOrderWiseExtraCharges";
        public static string OrderWiseProductSeriesNo { get; set; } = "sp_OrderWiseProductSeriesNo";
        public static string GetOrderWiseProductSeriesNo { get; set; } = "sp_GetOrderWiseProductSeriesNo";
        

        public static string GetInvoice { get; set; } = "sp_GetInvoice";
        public static string GetOrdersCount { get; set; } = "sp_GetOrdersCount";
        public static string Reports { get; set; } = "sp_Reports";
        public static string GetOrderDetails { get; set; } = "sp_GetOrderDetails";
    }
}