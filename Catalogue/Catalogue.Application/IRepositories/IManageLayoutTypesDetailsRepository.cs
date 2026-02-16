using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IManageLayoutTypesDetailsRepository
    {
        Task<BaseResponse<long>> Create(ManageLayoutTypesDetails typesDetails);

        Task<BaseResponse<long>> Update(ManageLayoutTypesDetails typesDetails);

        Task<BaseResponse<long>> Delete(ManageLayoutTypesDetails typesDetails);

        Task<BaseResponse<List<ManageLayoutTypesDetails>>> get(ManageLayoutTypesDetails typesDetails, int PageIndex, int PageSize, string Mode);
    }
}
