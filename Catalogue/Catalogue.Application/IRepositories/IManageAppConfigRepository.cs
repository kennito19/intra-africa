using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IManageAppConfigRepository
    {
        Task<BaseResponse<long>> Create(ManageAppConfig config);

        Task<BaseResponse<long>> Update(ManageAppConfig config);

        Task<BaseResponse<long>> Delete(ManageAppConfig config);

        Task<BaseResponse<List<ManageAppConfig>>> get(ManageAppConfig config, int PageIndex, int PageSize, string Mode);
    }
}
