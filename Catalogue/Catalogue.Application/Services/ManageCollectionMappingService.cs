using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class ManageCollectionMappingService : IManageCollectionMappingService
    {
        private readonly IManageCollectionMappingRepository _collectionMappingRepository;

        public ManageCollectionMappingService(IManageCollectionMappingRepository CollectionMapping)
        {
            _collectionMappingRepository = CollectionMapping;
        }

        public Task<BaseResponse<long>> Create(ManageCollectionMappingLibrary collectionMapping)
        {
            var response = _collectionMappingRepository.Create(collectionMapping);
            return response;
        }

        public Task<BaseResponse<long>> Update(ManageCollectionMappingLibrary collectionMapping)
        {
            var response = _collectionMappingRepository.Update(collectionMapping);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ManageCollectionMappingLibrary collectionMapping)
        {
            var reponse = _collectionMappingRepository.Delete(collectionMapping);
            return reponse;
        }

        public Task<BaseResponse<List<ManageCollectionMappingLibrary>>> get(ManageCollectionMappingLibrary collectionMapping, int PageIndex, int PageSize, string Mode)
        {
            var response = _collectionMappingRepository.get(collectionMapping, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
