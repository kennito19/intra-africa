namespace API_Gateway.Models
{
    public class Response<T>
    {
        public int code { get; set; }
        public List<T>? data { get; set; }
        public string? message { get; set; }
    }
}
