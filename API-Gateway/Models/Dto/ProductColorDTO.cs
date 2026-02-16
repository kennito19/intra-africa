namespace API_Gateway.Models.Dto
{
    public class ProductColorDTO
    {
        public int Id { get; set; }
        public int ColorId { get; set; }

        public string? ColorName { get; set; }
        public string? ColorCode { get; set; }
    }
}
