namespace API_Gateway.Models.Dto
{
    public class AbandonedCartDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string UserName { get; set; }
        public string? ProfileImage { get; set; }
        public string? MobileNo { get; set; }

        //-------------------  Cart  --------------------------------------//
       // public int Quantity { get; set; }

       // public int SellerProductMasterId { get; set; }
       // public decimal TempMRP { get; set; }
       // public decimal TempSellingPrice { get; set; }
       // public decimal SubTotal { get; set; }


        //------------------ Product -------------------------//

        public IEnumerable<ProductDetailsDTO> Productdetail { get; set; }


    }
}
