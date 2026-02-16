namespace API_Gateway.Models.Entity.Order
{
    public class OrderPackage
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }

        public int Id { get; set; }
        public int OrderID { get; set; }
        public string OrderItemIDs { get; set; }
        public string PackageNo { get; set; }
        public int TotalItems { get; set; }
        public int NoOfPackage { get; set; }
        public decimal PackageAmount { get; set; }
        public decimal CodCharges { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
