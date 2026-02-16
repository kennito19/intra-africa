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
    public class ManageAppConfigService : IManageAppConfigService
    {
        private readonly IManageAppConfigRepository _manageConfigRepository;

        public ManageAppConfigService(IManageAppConfigRepository manageConfigRepository)
        {
            _manageConfigRepository = manageConfigRepository;
        }

        public Task<BaseResponse<long>> Create(ManageAppConfig config)
        {
            var response = _manageConfigRepository.Create(config);
            return response;
        }

        public Task<BaseResponse<long>> Update(ManageAppConfig config)
        {
            var response = _manageConfigRepository.Update(config);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ManageAppConfig config)
        {
            var reponse = _manageConfigRepository.Delete(config);
            return reponse;
        }

        public Task<BaseResponse<List<ManageAppConfig>>> get(ManageAppConfig config, int PageIndex, int PageSize, string Mode)
        {
            var response = _manageConfigRepository.get(config, PageIndex, PageSize, Mode);
            return response;
        }
    }
}
