using System.ComponentModel.DataAnnotations;

namespace API_Gateway.Models.Entity.IDServer
{
    public class ForgotPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
