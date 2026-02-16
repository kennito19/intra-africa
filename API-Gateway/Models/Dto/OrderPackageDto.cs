namespace API_Gateway.Models.Dto
{
    public class OrderPackageDto
    {
        public int OrderID { get; set; }
        public string OrderItemIDs { get; set; }
        public int TotalItems { get; set; }
        public int NoOfPackage { get; set; }
        public decimal PackageAmount { get; set; }
        
    }
}
