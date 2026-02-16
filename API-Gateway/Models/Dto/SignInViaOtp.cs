namespace API_Gateway.Models.Dto
{
    public class SignInViaOtp
    {
        public string MobileNo { get; set; }
        public string otp { get; set; }
        public string DeviceId { get; set; }
    }

    public class GenerateMobileOtp
    {
        public string MobileNo { get; set; }
    }
}
