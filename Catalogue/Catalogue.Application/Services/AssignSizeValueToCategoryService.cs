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
    public class AssignSizeValueToCategoryService : IAssignSizeValueToCategoryService
    {
        private readonly IAssignSizeValueToCategoryRepository _assignSizeRepository;
        public AssignSizeValueToCategoryService(IAssignSizeValueToCategoryRepository assignSizeRepository)
        {
            _assignSizeRepository = assignSizeRepository;
        }
        public Task<BaseResponse<long>> Create(AssignSizeValueToCategory assignSize)
        {
            var response = _assignSizeRepository.Create(assignSize);
            return response;
        }

        public Task<BaseResponse<long>> Update(AssignSizeValueToCategory assignSize)
        {
            var response = _assignSizeRepository.Update(assignSize);
            return response;
        }

        public Task<BaseResponse<long>> Delete(AssignSizeValueToCategory assignSize)
        {
            var response = _assignSizeRepository.Delete(assignSize);
            return response;
        }

        public Task<BaseResponse<List<AssignSizeValueToCategory>>> get(AssignSizeValueToCategory assignSize, int PageIndex, int PageSize, string Mode)
        {
            var response = _assignSizeRepository.get(assignSize, PageIndex, PageSize, Mode);
            return response;
        }

        
    }
}
