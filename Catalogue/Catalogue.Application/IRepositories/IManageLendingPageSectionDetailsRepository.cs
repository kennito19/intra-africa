using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IManageLendingPageSectionDetailsRepository
    {
        Task<BaseResponse<long>> Create(LendingPageSectionDetailsLibrary lendingPageSectionDetail);

        Task<BaseResponse<long>> Update(LendingPageSectionDetailsLibrary lendingPageSectionDetail);

        Task<BaseResponse<long>> Delete(LendingPageSectionDetailsLibrary lendingPageSectionDetail);

        Task<BaseResponse<List<LendingPageSectionDetailsLibrary>>> get(LendingPageSectionDetailsLibrary lendingPageSectionDetail, int PageIndex, int PageSize, string Mode);

    }
}
