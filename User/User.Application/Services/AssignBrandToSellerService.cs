using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.IRepositories;
using User.Application.IServices;
using User.Domain;
using User.Domain.Entity;

namespace User.Application.Services
{
    public class AssignBrandToSellerService : IAssignBrandToSellerService
    {
        private readonly IAssignBrandToSellerRepository _assignBrandToSellerRepository;

        public AssignBrandToSellerService(IAssignBrandToSellerRepository assignBrandToSellerRepository)
        {
            _assignBrandToSellerRepository = assignBrandToSellerRepository;
        }

        public Task<BaseResponse<long>> Create(AssignBrandToSeller assignBrandToSeller)
        {
            var data = _assignBrandToSellerRepository.Create(assignBrandToSeller);
            return data;
        }
        public Task<BaseResponse<long>> Update(AssignBrandToSeller assignBrandToSeller)
        {
            var data = _assignBrandToSellerRepository.Update(assignBrandToSeller);
            return data;
        }
        public Task<BaseResponse<long>> Delete(AssignBrandToSeller assignBrandToSeller)
        {
            var data = _assignBrandToSellerRepository.Delete(assignBrandToSeller);
            return data;
        }

        public Task<BaseResponse<List<AssignBrandToSeller>>> Get(AssignBrandToSeller assignBrandToSeller, int PageIndex, int PageSize, string Mode)
        {
            var data = _assignBrandToSellerRepository.Get(assignBrandToSeller, PageIndex, PageSize, Mode);
            return data;
        }
    }
}
