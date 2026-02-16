using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IManageLendingPageSectionsRepository
    {
        Task<BaseResponse<long>> Create(LendingPageSectionsLibrary lendingPageSection);

        Task<BaseResponse<long>> Update(LendingPageSectionsLibrary lendingPageSection);

        Task<BaseResponse<long>> Delete(LendingPageSectionsLibrary lendingPageSection);

        Task<BaseResponse<List<LendingPageSectionsLibrary>>> get(LendingPageSectionsLibrary lendingPageSection, int PageIndex, int PageSize, string Mode);
    }
}
