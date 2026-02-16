namespace API_Gateway.Models.Dto
{
    public class UserProductListWithFilterDTO
    {
        public UserProductFilterDTO? filterList { get; set; }
        public List<UserProductsDTO>? Products { get; set; }
    }
}
