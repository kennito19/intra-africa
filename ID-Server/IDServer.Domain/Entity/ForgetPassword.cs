using System.ComponentModel.DataAnnotations;

namespace IDServer.Domain.Entity
{
    public class ForgetPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
