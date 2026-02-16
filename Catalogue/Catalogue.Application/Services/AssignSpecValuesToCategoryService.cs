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
    public class AssignSpecValuesToCategoryService : IAssignSpecValuesToCategoryService
    {
        private readonly IAssignSpecValuesToCategoryRepository _assignSpecValuesRepository;
        public AssignSpecValuesToCategoryService(IAssignSpecValuesToCategoryRepository assignSpecValuesRepository)
        {
            _assignSpecValuesRepository = assignSpecValuesRepository;
        }
        public Task<BaseResponse<long>> Create(AssignSpecValuesToCategory assignSpecValues)
        {
            var response = _assignSpecValuesRepository.Create(assignSpecValues);
            return response;
        }

        public Task<BaseResponse<long>> Update(AssignSpecValuesToCategory assignSpecValues)
        {
            var response = _assignSpecValuesRepository.Update(assignSpecValues);
            return response;
        }

        public Task<BaseResponse<long>> Delete(AssignSpecValuesToCategory assignSpecValues)
        {
            var response = _assignSpecValuesRepository.Delete(assignSpecValues);
            return response;
        }

        public Task<BaseResponse<List<AssignSpecValuesToCategory>>> get(AssignSpecValuesToCategory assignSpecValues, int PageIndex, int PageSize, string Mode)
        {
            var response = _assignSpecValuesRepository.get(assignSpecValues, PageIndex, PageSize, Mode);
            return response;
        }

        
    }
}
