using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IManageConfigValueRepository
    {
        Task<BaseResponse<long>> Create(ManageConfigValueLibrary configValue);

        Task<BaseResponse<long>> Update(ManageConfigValueLibrary configValue);

        Task<BaseResponse<long>> Delete(ManageConfigValueLibrary configValue);

        Task<BaseResponse<List<ManageConfigValueLibrary>>> get(ManageConfigValueLibrary configValue, int PageIndex, int PageSize, string Mode);

    }
}
