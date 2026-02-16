namespace API_Gateway.Models.Dto
{
    public class ProductDelete
    {
        #region delete product details
        public int productId { get; set; }
        #endregion

        #region SellerProduct
        public int SellerProductId { get; set; }

        public string? SellerId { get; set; }

        #endregion
    }
}
