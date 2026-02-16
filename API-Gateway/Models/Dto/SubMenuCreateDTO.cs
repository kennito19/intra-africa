namespace API_Gateway.Models.Dto
{
    public class SubMenuCreateDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string MenuType { get; set; }
        public int HeaderId { get; set; }
        public string? Image { get; set; }
        public string? ImageAlt { get; set; }
        public bool HasLink { get; set; }
        public string? RedirectTo { get; set; }
        public List<int> CategoryId { get; set; }
        public int Sequence { get; set; }
    }

    public class CategoryWiseSubMenuCreateDTO
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public List<int> CategoryIds { get; set; }

    }

    public class BrandWiseSubMenuCreateDTO
    {
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public List<int> BrandId { get; set; }
    }

}
