namespace API_Gateway.Models.Entity.Catalogue
{
    public class ManageConfigValueLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int Id { get; set; }
        public int KeyId { get; set; }
        public string Value { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        public string? KeyName { get; set; }
        public string? SearchText { get; set; }
    }
}
