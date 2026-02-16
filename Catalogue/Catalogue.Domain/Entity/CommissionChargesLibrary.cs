using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class CommissionChargesLibrary
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int ID { get; set; }
        public int? CatID { get; set; }
        public string? SellerID { get; set; }
        public int? BrandID { get; set; }
        public string? ChargesOn { get; set; }

        //Absolute/Percentage
        //[JsonIgnore]
        public string? ChargesIn { get; set; }
        //[JsonIgnore]
        public bool? IsCompulsary { get; set; } = null;
        public decimal? AmountValue { get; set; } = 0;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string? CategoryName { get; set; } = null;
        public string? SellerName { get; set; } = null;
        public string? BrandName { get; set; } = null;
        public bool? OnlyCategories { get; set; } = null;
        public bool? OnlySellers { get; set; } = null;
        public bool? OnlyBrands { get; set; } = null;
        public string? Searchtext { get; set; } = null;
    }
}
