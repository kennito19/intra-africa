using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IManageConfigKeyRepository
    {
        Task<BaseResponse<long>> Create(ManageConfigKeyLibrary configKey);

        Task<BaseResponse<long>> Update(ManageConfigKeyLibrary configKey);

        Task<BaseResponse<long>> Delete(ManageConfigKeyLibrary configKey);

        Task<BaseResponse<List<ManageConfigKeyLibrary>>> get(ManageConfigKeyLibrary configKey, int PageIndex, int PageSize, string Mode);

    }
}
