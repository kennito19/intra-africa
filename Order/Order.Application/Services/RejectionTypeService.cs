using Order.Application.IRepositories;
using Order.Application.IServices;
using Order.Domain;
using Order.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Services
{
    public class RejectionTypeService : IRejectionTypeService
    {
        private readonly IRejectionTypeRepository _rejectionTypeRepository;

        public RejectionTypeService(IRejectionTypeRepository rejectionTypeRepository)
        {
            _rejectionTypeRepository = rejectionTypeRepository;
        }

        Task<BaseResponse<long>> IRejectionTypeService.Create(RejectionType rejectionType)
        {
            var response = _rejectionTypeRepository.Create(rejectionType);
            return response;
        }

        Task<BaseResponse<long>> IRejectionTypeService.Delete(RejectionType rejectionType)
        {
            var response = _rejectionTypeRepository.Delete(rejectionType);
            return response;
        }

        Task<BaseResponse<List<RejectionType>>> IRejectionTypeService.Get(RejectionType rejectionType, int PageIndex, int PageSize, string Mode)
        {
            var response = _rejectionTypeRepository.Get(rejectionType, PageIndex, PageSize, Mode);
            return response;
        }

        Task<BaseResponse<long>> IRejectionTypeService.Update(RejectionType rejectionType)
        {
            var response = _rejectionTypeRepository.Update(rejectionType);
            return response;
        }
    }
}
