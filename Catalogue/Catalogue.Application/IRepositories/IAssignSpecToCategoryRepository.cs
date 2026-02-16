using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IAssignSpecToCategoryRepository
    {
        Task<BaseResponse<long>> Create(AssignSpecToCategory assignSpec);

        Task<BaseResponse<long>> Update(AssignSpecToCategory assignSpec);

        Task<BaseResponse<long>> Delete(AssignSpecToCategory assignSpec);

        Task<BaseResponse<List<AssignSpecToCategory>>> get(AssignSpecToCategory assignSpec, int PageIndex, int PageSize, string Mode);
    }
}
