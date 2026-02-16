using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IServices
{
    public interface IManageCollectionService
    {
        Task<BaseResponse<long>> Create(ManageCollectionLibrary collection);

        Task<BaseResponse<long>> Update(ManageCollectionLibrary collection);

        Task<BaseResponse<long>> Delete(ManageCollectionLibrary collection);

        Task<BaseResponse<List<ManageCollectionLibrary>>> get(ManageCollectionLibrary collection, int PageIndex, int PageSize, string Mode);

    }
}
