using Catalogue.Application.IRepositories;
using Catalogue.Domain.Entity;
using Catalogue.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Catalogue.Application.IServices;

namespace Catalogue.Application.Services
{
    public class ManageLayoutsService : IManageLayoutsService
    {
        private readonly IManageLayoutsRepository _layoutsRepository;

        public ManageLayoutsService(IManageLayoutsRepository layoutsRepository)
        {
            _layoutsRepository = layoutsRepository;
        }

        public Task<BaseResponse<long>> Create(ManageLayoutsLibrary layouts)
        {
            var response = _layoutsRepository.Create(layouts);
            return response;
        }

        public Task<BaseResponse<long>> Update(ManageLayoutsLibrary layouts)
        {
            var response = _layoutsRepository.Update(layouts);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ManageLayoutsLibrary layouts)
        {
            var reponse = _layoutsRepository.Delete(layouts);
            return reponse;
        }

        public Task<BaseResponse<List<ManageLayoutsLibrary>>> get(ManageLayoutsLibrary layouts, int PageIndex, int PageSize, string Mode)
        {
            var response = _layoutsRepository.get(layouts, PageIndex, PageSize, Mode);
            return response;
        }
    }
}
