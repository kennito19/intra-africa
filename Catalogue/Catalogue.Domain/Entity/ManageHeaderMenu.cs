using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Domain.Entity
{
	public class ManageHeaderMenu
	{
		public int RowNumber { get; set; }
		public int PageCount { get; set; }
		public int RecordCount { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? ImageAlt { get; set; }
        public bool HasLink { get; set; }
        public string? RedirectTo { get; set; }
        public int LendingPageId { get; set; }
        public int CategoryId { get; set; }
        public int StaticPageId { get; set; }
        public int CollectionId { get; set; }
        public string? CustomLink { get; set; }
        public int Sequence { get; set; }
        public string? color { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime? CreatedAt { get; set; }
		public string? ModifiedBy { get; set; }
		public DateTime? ModifiedAt { get; set; }
		public string? Searchtext { get; set; }
	}
}
