using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IManageHomePageService
    {
        Task<BaseResponse<long>> Create(ManageHomePage manageHomePage);

        Task<BaseResponse<long>> Update(ManageHomePage manageHomePage);

        Task<BaseResponse<long>> Delete(ManageHomePage manageHomePage);

        Task<BaseResponse<List<ManageHomePage>>> get(ManageHomePage manageHomePage, int PageIndex, int PageSize, string Mode);
    }
}
