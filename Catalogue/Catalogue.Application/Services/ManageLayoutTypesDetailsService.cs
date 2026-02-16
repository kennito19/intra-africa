using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class ManageLayoutTypesDetailsService : IManageLayoutTypesDetailsService
    {
        private readonly IManageLayoutTypesDetailsRepository _layouttypesDetailsRepository;

        public ManageLayoutTypesDetailsService(IManageLayoutTypesDetailsRepository layouttypesDetailsRepository)
        {
            _layouttypesDetailsRepository = layouttypesDetailsRepository;
        }

        public Task<BaseResponse<long>> Create(ManageLayoutTypesDetails typesDetails)
        {
            var response = _layouttypesDetailsRepository.Create(typesDetails);
            return response;
        }
        public Task<BaseResponse<long>> Update(ManageLayoutTypesDetails typesDetails)
        {
            var response = _layouttypesDetailsRepository.Update(typesDetails);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ManageLayoutTypesDetails typesDetails)
        {
            var reponse = _layouttypesDetailsRepository.Delete(typesDetails);
            return reponse;
        }

        public Task<BaseResponse<List<ManageLayoutTypesDetails>>> get(ManageLayoutTypesDetails typesDetails, int PageIndex, int PageSize, string Mode)
        {
            var response = _layouttypesDetailsRepository.get(typesDetails, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
