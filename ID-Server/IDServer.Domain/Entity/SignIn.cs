

namespace IDServer.Domain.Entity
{
    public class SignIn
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public bool IsRemember { get; set; } = false;
        public string DeviceId { get; set; }
    }
}
