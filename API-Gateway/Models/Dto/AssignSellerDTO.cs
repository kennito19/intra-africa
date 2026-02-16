using System.ComponentModel.DataAnnotations;

namespace API_Gateway.Models.Dto
{
    public class AssignSellerDTO
    {
        public int? Id { get; set; }

        [Required]
        public string SellerId { get; set; }

        public int? BrandId { get; set; }

        [Required]
        public int? CountryID { get; set; }

        [Required]
        public int? StateID { get; set; }

        [Required]
        public int? CityID { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
