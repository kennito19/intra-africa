namespace API_Gateway.Models.Dto
{
    public class TaxTypeDTO
    {
        public int Id { get; set; }
        public string TaxType { get; set; }
        public int? ParentId { get; set; }
    }
}
