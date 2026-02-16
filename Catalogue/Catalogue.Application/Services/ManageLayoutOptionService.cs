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
    public class ManageLayoutOptionService : IManageLayoutOptionService
    {
        private readonly IManageLayoutOptionRepository _layoutOptionRepository;

        public ManageLayoutOptionService(IManageLayoutOptionRepository layoutOptionRepository)
        {
            _layoutOptionRepository = layoutOptionRepository;
        }

        public Task<BaseResponse<long>> Create(ManageLayoutOption layoutOptions)
        {
            var response = _layoutOptionRepository.Create(layoutOptions);
            return response;
        }

        public Task<BaseResponse<long>> Update(ManageLayoutOption layoutOptions)
        {
            var response = _layoutOptionRepository.Update(layoutOptions);
            return response;
        }

        public Task<BaseResponse<List<ManageLayoutOption>>> get(ManageLayoutOption layoutOptions, int PageIndex, int PageSize, string Mode)
        {
            var response = _layoutOptionRepository.get(layoutOptions, PageIndex, PageSize, Mode);
            return response;
        }

        public Task<BaseResponse<long>> Delete(ManageLayoutOption layoutOptions)
        {
            var reponse = _layoutOptionRepository.Delete(layoutOptions);
            return reponse;
        }
        
    }
}
