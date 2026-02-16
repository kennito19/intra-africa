

namespace API_Gateway.Models.Dto
{
    public class PageModuleSignInResponse
    {
        public int PageId { get; set; }
        public string? PageName { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }
}
