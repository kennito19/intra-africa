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
    public class ManageConfigValueService : IManageConfigValueService
    {
        private readonly IManageConfigValueRepository _manageConfigValueRepository;

        public ManageConfigValueService(IManageConfigValueRepository manageConfigValueRepository)
        {
            _manageConfigValueRepository = manageConfigValueRepository;
        }

        public Task<BaseResponse<long>> Create(ManageConfigValueLibrary configValue)
        {
            var response = _manageConfigValueRepository.Create(configValue);
            return response;
        }

        public Task<BaseResponse<long>> Update(ManageConfigValueLibrary configValue)
        {
            var response = _manageConfigValueRepository.Update(configValue);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ManageConfigValueLibrary configValue)
        {
            var reponse = _manageConfigValueRepository.Delete(configValue);
            return reponse;
        }

        public Task<BaseResponse<List<ManageConfigValueLibrary>>> get(ManageConfigValueLibrary configValue, int PageIndex, int PageSize, string Mode)
        {
            var response = _manageConfigValueRepository.get(configValue, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
