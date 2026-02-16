using System.ComponentModel.DataAnnotations;

namespace API_Gateway.Models.Entity.IDServer
{
    public class ResetPassword
    {
        public string uid { get; set; }
        public string token { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
