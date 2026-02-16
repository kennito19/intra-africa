using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IManageLayoutsService
    {
        Task<BaseResponse<long>> Create(ManageLayoutsLibrary layouts);

        Task<BaseResponse<long>> Update(ManageLayoutsLibrary layouts);

        Task<BaseResponse<long>> Delete(ManageLayoutsLibrary layouts);

        Task<BaseResponse<List<ManageLayoutsLibrary>>> get(ManageLayoutsLibrary layouts, int PageIndex, int PageSize, string Mode);
    }
}
