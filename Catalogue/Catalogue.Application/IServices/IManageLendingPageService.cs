using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IManageLendingPageService
    {
        Task<BaseResponse<long>> Create(LendingPageLibrary lendingPage);

        Task<BaseResponse<long>> Update(LendingPageLibrary lendingPage);

        Task<BaseResponse<long>> Delete(LendingPageLibrary lendingPage);

        Task<BaseResponse<List<LendingPageLibrary>>> get(LendingPageLibrary lendingPage, int PageIndex, int PageSize, string Mode);

    }
}
