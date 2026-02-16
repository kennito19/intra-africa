using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IManageLayoutTypesService
    {
        Task<BaseResponse<long>> Create(ManageLayoutTypesLibrary layoutTypes);

        Task<BaseResponse<long>> Update(ManageLayoutTypesLibrary layoutTypes);

        Task<BaseResponse<long>> Delete(ManageLayoutTypesLibrary layoutTypes);

        Task<BaseResponse<List<ManageLayoutTypesLibrary>>> get(ManageLayoutTypesLibrary layoutTypes, int PageIndex, int PageSize, string Mode);
    }
}
