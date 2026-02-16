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
    public class AssignSpecToCategoryService : IAssignSpecToCategoryService
    {
        private readonly IAssignSpecToCategoryRepository _assignSpecRepository;
        public AssignSpecToCategoryService(IAssignSpecToCategoryRepository assignSpecRepository)
        {
            _assignSpecRepository = assignSpecRepository;
        }
        public Task<BaseResponse<long>> Create(AssignSpecToCategory assignSpec)
        {
            var response = _assignSpecRepository.Create(assignSpec);
            return response;
        }

        public Task<BaseResponse<long>> Update(AssignSpecToCategory assignSpec)
        {
            var response = _assignSpecRepository.Update(assignSpec);
            return response;
        }

        public Task<BaseResponse<long>> Delete(AssignSpecToCategory assignSpec)
        {
            var response = _assignSpecRepository.Delete(assignSpec);
            return response;
        }

        public Task<BaseResponse<List<AssignSpecToCategory>>> get(AssignSpecToCategory assignSpec, int PageIndex, int PageSize, string Mode)
        {
            var response = _assignSpecRepository.get(assignSpec, PageIndex, PageSize, Mode);
            return response;
        }

        
    }
}
