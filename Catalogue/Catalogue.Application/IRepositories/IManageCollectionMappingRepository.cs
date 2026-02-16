using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.IRepositories
{
    public interface IManageCollectionMappingRepository
    {
        Task<BaseResponse<long>> Create(ManageCollectionMappingLibrary collectionMapping);

        Task<BaseResponse<long>> Update(ManageCollectionMappingLibrary collectionMapping);

        Task<BaseResponse<long>> Delete(ManageCollectionMappingLibrary collectionMapping);

        Task<BaseResponse<List<ManageCollectionMappingLibrary>>> get(ManageCollectionMappingLibrary collectionMapping, int PageIndex, int PageSize, string Mode);

    }
}
