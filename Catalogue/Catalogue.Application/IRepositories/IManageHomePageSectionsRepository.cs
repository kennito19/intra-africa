using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IManageHomePageSectionsRepository
    {
        Task<BaseResponse<long>> Create(ManageHomePageSectionsLibrary pageSections);

        Task<BaseResponse<long>> Update(ManageHomePageSectionsLibrary pageSections);

        Task<BaseResponse<long>> Delete(ManageHomePageSectionsLibrary pageSections);

        Task<BaseResponse<List<ManageHomePageSectionsLibrary>>> get(ManageHomePageSectionsLibrary pageSections, int PageIndex, int PageSize, string Mode);

        Task<BaseResponse<List<ProductHomePageSectionLibrary>>> getProudctHomePageSection(ProductHomePageSectionLibrary productPageSections, string Mode);
    }
}
