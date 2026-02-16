using System.ComponentModel.DataAnnotations;

namespace API_Gateway.Models.Entity.IDServer
{
    public class ChangePassword
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}
