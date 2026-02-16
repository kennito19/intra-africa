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
    public class ManageCollectionService : IManageCollectionService
    {
        private readonly IManageCollectionRepository _collectionRepository;

        public ManageCollectionService(IManageCollectionRepository manageCollection)
        {
            _collectionRepository = manageCollection;
        }

        public Task<BaseResponse<long>> Create(ManageCollectionLibrary collection)
        {
            var response = _collectionRepository.Create(collection);
            return response;
        }

        public Task<BaseResponse<long>> Update(ManageCollectionLibrary collection)
        {
            var response = _collectionRepository.Update(collection);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ManageCollectionLibrary collection)
        {
            var reponse = _collectionRepository.Delete(collection);
            return reponse;
        }

        public Task<BaseResponse<List<ManageCollectionLibrary>>> get(ManageCollectionLibrary collection, int PageIndex, int PageSize, string Mode)
        {
            var response = _collectionRepository.get(collection, PageIndex, PageSize, Mode);
            return response;
        }


    }
}
