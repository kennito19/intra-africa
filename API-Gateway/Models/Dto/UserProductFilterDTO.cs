namespace API_Gateway.Models.Dto
{
    public class UserProductFilterDTO
    {
        public int? product_count { get; set; }

        public decimal? MinSellingPrice { get; set; }
        public decimal? MaxSellingPrice { get; set; }
        public decimal? MinDiscount { get; set; }
        public decimal? MaxDiscount { get; set; }

        public List<CategoryFilterDTO>? category_filter { get; set; }
        public List<BrandFilterDTO>? brand_filter { get; set; }
        public List<SizeFilterDTO>? size_filter { get; set; }
        public List<ColorFilterDTO>? color_filter { get; set; }

        public List<FilterTypeDTO>? filter_types { get; set; }
    }
}
