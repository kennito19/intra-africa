using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Domain;
using Catalogue.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalogue.Application.Services
{
    public class ManageLayoutTypesService : IManageLayoutTypesService
    {
        private readonly IManageLayoutTypesRepository _layouttypesRepository;

        public ManageLayoutTypesService(IManageLayoutTypesRepository layouttypeRepository)
        {
            _layouttypesRepository = layouttypeRepository;
        }

        public Task<BaseResponse<long>> Create(ManageLayoutTypesLibrary layoutTypes)
        {
            var response = _layouttypesRepository.Create(layoutTypes);
            return response;
        }

        public Task<BaseResponse<long>> Update(ManageLayoutTypesLibrary layoutTypes)
        {
            var response = _layouttypesRepository.Update(layoutTypes);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ManageLayoutTypesLibrary layoutTypes)
        {
            var reponse = _layouttypesRepository.Delete(layoutTypes);
            return reponse;
        }

        public Task<BaseResponse<List<ManageLayoutTypesLibrary>>> get(ManageLayoutTypesLibrary layoutTypes, int PageIndex, int PageSize, string Mode)
        {
            var response = _layouttypesRepository.get(layoutTypes, PageIndex, PageSize, Mode);
            return response;
        }

    }
}
