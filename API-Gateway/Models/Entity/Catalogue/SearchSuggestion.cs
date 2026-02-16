using API_Gateway.Models.Dto;
using API_Gateway.Models.Entity.User;

namespace API_Gateway.Models.Entity.Catalogue
{
    public class SearchSuggestion
    {
        public IEnumerable<UserProductList> Products { get; set; }
        public IEnumerable<BrandLibrary> Brands { get; set; }
        public IEnumerable<CategoryLibrary> Categories { get; set; }
    }
}
