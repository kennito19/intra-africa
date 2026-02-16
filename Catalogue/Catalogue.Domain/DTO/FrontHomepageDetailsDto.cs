using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.DTO
{
    public class FrontHomepageDetailsDto
    {
        public int? HomePageSectionId { get; set; }
        public int? LayoutId { get; set; }
        public int? LayoutTypeId { get; set; }
        public string? HomePageSectionName { get; set; }
        public int? HomePageSectionSequence { get; set; }
        public int? SectionColumns { get; set; }
        public bool? HomePageSectionIsTitleVisible { get; set; }
        public string? HomePageSectionTitle { get; set; }
        public string? HomePageSectionSubTitle { get; set; }
        public string? HomePageSectionTitlePosition { get; set; }
        public string? HomePageSectionLinkIn { get; set; }
        public string? HomePageSectionLinkText { get; set; }
        public string? HomePageSectionLink { get; set; }
        public string? HomePageSectionLinkPosition { get; set; }
        public string? HomePageSectionStatus { get; set; }
        public string? ListType { get; set; } = null;
        public int? TopProducts { get; set; } = null;
        public int? TotalRowsInSection { get; set; } = null;
        public bool? IsCustomGrid { get; set; } = null;
        public int? NumberOfImages { get; set; } = null;
        public int? Column1 { get; set; } = null;
        public int? Column2 { get; set; } = null;
        public int? Column3 { get; set; } = null;
        public int? Column4 { get; set; } = null;
        public int? CategoryId { get; set; } = null;
        public string? BackgroundColor { get; set; } = null;
        public bool? InContainer { get; set; } = null;
        public string? TitleColor { get; set; } = null;
        public string? TextColor { get; set; } = null;

        public int? HomePageSectionDetailsId { get; set; }
        public int? LayoutTypeDetailsId { get; set; }
        public int? OptionId { get; set; }
        public string? Image { get; set; }
        public string? ImageAlt { get; set; }
        public bool? IsTitleVisible { get; set; }
        public string? HomePageSectionDetailsTitle { get; set; }
        public string? HomePageSectionDetailsSubTitle { get; set; }
        public string? HomePageSectionDetailsTitlePosition { get; set; }
        public int? HomePageSectionDetailsSequence { get; set; }
        public int? Columns { get; set; }
        public string? RedirectTo { get; set; }
        public int? HomePageSectionDetailsCategoryId { get; set; }
        public string? BrandIds { get; set; }
        public string? SizeIds { get; set; }
        public string? SpecificationIds { get; set; }
        public string? ColorIds { get; set; }
        public int? CollectionId { get; set; }
        public string? ProductId { get; set; }
        public int? StaticPageId { get; set; }
        public int? LendingPageId { get; set; }
        public string? CustomLinks { get; set; }
        public string? HomePageSectionDetailsTitleColor { get; set; }
        public string? HomePageSectionDetailsSubTitleColor { get; set; }
        public string? TitleSize { get; set; }
        public string? SubTitleSize { get; set; }
        public bool? ItalicSubTitle { get; set; }
        public bool? ItalicTitle { get; set; }
        public string? Description { get; set; }
        public string? SliderType { get; set; }
        public string? VideoLinkType { get; set; }
        public string? VideoId { get; set; }
        public string? Name { get; set; }
        public string? AssignCity { get; set; }
        public string? AssignState { get; set; }
        public string? AssignCountry { get; set; }
        public string? HomePageSectionDetailsStatus { get; set; }
        public string? LayoutName { get; set; }
        public string? LayoutTypeName { get; set; }
        public string? ClassName { get; set; }
        public string? Options { get; set; }
        public bool? HasInnerColumns { get; set; }
        public int? LayoutTypeColumns { get; set; }
        public string? MinImage { get; set; }
        public string? MaxImage { get; set; }
        public string? LayoutTypeDetailsName { get; set; }
        public string? SectionType { get; set; }
        public string? InnerColumns { get; set; }
        public string? LayoutOptionName { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryPathName { get; set; }
    }
}
