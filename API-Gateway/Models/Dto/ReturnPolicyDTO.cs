namespace API_Gateway.Models.Dto
{
    public class ReturnPolicyDTO
    {
        public int Id { get; set; }
        public int ReturnPolicyID { get; set; }
        public int ValidityDays { get; set; }
        public string? Title { get; set; }
        public string? Covers { get; set; }
        public string? Description { get; set; }
        public string? ReturnPolicyName { get; set; }
    }
}
