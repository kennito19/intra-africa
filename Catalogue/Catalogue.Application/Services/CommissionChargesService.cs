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
    public class CommissionChargesService : ICommissionChargesService
    {
        private readonly ICommissionChargesRepository _commissionChargesRepository;
        public CommissionChargesService(ICommissionChargesRepository commissionChargesRepository)
        {
            _commissionChargesRepository = commissionChargesRepository;
        }
        public Task<BaseResponse<long>> Create(CommissionChargesLibrary commissionCharges)
        {
            var response = _commissionChargesRepository.Create(commissionCharges);
            return response;
        }
        public Task<BaseResponse<long>> Update(CommissionChargesLibrary commissionCharges)
        {
            var response = _commissionChargesRepository.Update(commissionCharges);
            return response;
        }
        public Task<BaseResponse<long>> Delete(CommissionChargesLibrary commissionCharges)
        {
            var response = _commissionChargesRepository.Delete(commissionCharges);
            return response;
        }

        public Task<BaseResponse<List<CommissionChargesLibrary>>> get(CommissionChargesLibrary commissionCharges, int PageIndex, int PageSize, string Mode)
        {
            var response = _commissionChargesRepository.get(commissionCharges, PageIndex, PageSize, Mode);
            return response;
        }

        public Task<BaseResponse<List<CommissionChargesLibrary>>> getCategoryWiseCommission(CommissionChargesLibrary commissionCharges)
        {
            var response = _commissionChargesRepository.getCategoryWiseCommission(commissionCharges);
            return response;
        }


    }
}
