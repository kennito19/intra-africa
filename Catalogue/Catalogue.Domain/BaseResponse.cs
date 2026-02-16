namespace Catalogue.Domain
{
    public class BaseResponse<T>
    {
        public int? code { get; set; }
        //public object? extension { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }
    }
}
