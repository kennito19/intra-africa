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
    public class ManageConfigKeyService : IManageConfigKeyService
    {
        private readonly IManageConfigKeyRepository _manageConfigKeyRepository;

        public ManageConfigKeyService(IManageConfigKeyRepository manageConfigKeyRepository)
        {
            _manageConfigKeyRepository = manageConfigKeyRepository;
        }

        public Task<BaseResponse<long>> Create(ManageConfigKeyLibrary configKey)
        {
            var response = _manageConfigKeyRepository.Create(configKey);
            return response;
        }

        public Task<BaseResponse<long>> Update(ManageConfigKeyLibrary configKey)
        {
            var response = _manageConfigKeyRepository.Update(configKey);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ManageConfigKeyLibrary configKey)
        {
            var reponse = _manageConfigKeyRepository.Delete(configKey);
            return reponse;
        }

        public Task<BaseResponse<List<ManageConfigKeyLibrary>>> get(ManageConfigKeyLibrary configKey, int PageIndex, int PageSize, string Mode)
        {
            var response = _manageConfigKeyRepository.get(configKey, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
