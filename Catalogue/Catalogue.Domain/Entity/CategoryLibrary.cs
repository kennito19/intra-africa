using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
    public class CategoryLibrary
    {
        public int? RowNumber { get; set; }
        public int? PageCount { get; set; }
        public int? RecordCount { get; set; }
        public int Id { get; set; }
        public string? Guid { get; set; }
        public string Name { get; set; }
        public int CurrentLevel { get; set; }
        public int? ParentId { get; set; }
        public string? Image { get; set; }
        public string? PathIds { get; set; }
        public string? PathNames { get; set; }
        public string? MetaTitles { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }
        public string? Status { get; set; }
        public string? Color { get; set; }
        public string? Title { get; set; }
        public string? SubTitle { get; set; }
        public string? Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public string? ParentName { get; set; }
        public string? ParentPathIds { get; set; }
        public string? ParentPathNames { get; set; }
        public string? Searchtext { get; set; }

    }
}
