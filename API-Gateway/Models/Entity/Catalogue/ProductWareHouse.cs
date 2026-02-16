namespace API_Gateway.Models.Entity.Catalogue
{
    public class ProductWareHouse
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int SellerProductID { get; set; }
        public int ProductID { get; set; }
        public int SellerWiseProductPriceMasterID { get; set; }
        public int WarehouseID { get; set; }
        public string? WarehouseName { get; set; }
        public int ProductQuantity { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? ProductName { get; set; }
    }
}
