using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IAssignSpecValuesToCategoryRepository
    {
        Task<BaseResponse<long>> Create(AssignSpecValuesToCategory assignSpecValues);

        Task<BaseResponse<long>> Update(AssignSpecValuesToCategory assignSpecValues);

        Task<BaseResponse<long>> Delete(AssignSpecValuesToCategory assignSpecValues);

        Task<BaseResponse<List<AssignSpecValuesToCategory>>> get(AssignSpecValuesToCategory assignSpecValues, int PageIndex, int PageSize, string Mode);
    }
}
