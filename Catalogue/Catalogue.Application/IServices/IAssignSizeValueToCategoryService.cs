using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IAssignSizeValueToCategoryService
    {
        Task<BaseResponse<long>> Create(AssignSizeValueToCategory assignSize);

        Task<BaseResponse<long>> Update(AssignSizeValueToCategory assignSize);

        Task<BaseResponse<long>> Delete(AssignSizeValueToCategory assignSize);

        Task<BaseResponse<List<AssignSizeValueToCategory>>> get(AssignSizeValueToCategory assignSize, int PageIndex, int PageSize, string Mode);
    }
}
