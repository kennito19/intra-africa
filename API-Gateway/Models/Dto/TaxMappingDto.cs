namespace API_Gateway.Models.Dto
{
    public class TaxMappingDto
    {
        public int Id { get; set; }
        public int TaxId { get; set; }
        public int TaxTypeId { get; set; }
        public string TaxMapBy { get; set; }
        public string? SpecificState { get; set; }
        public int? SpecificTaxTypeId { get; set; }
    }
}
