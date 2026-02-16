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
    public class ChargesPaidByService : IChargesPaidByService
    {
        private readonly IChargesPaidByRepository _ChargesPaidByRepository;

        public ChargesPaidByService(IChargesPaidByRepository ChargesPaidByRepository)
        {
            _ChargesPaidByRepository = ChargesPaidByRepository;
        }
        public Task<BaseResponse<long>> Create(ChargesPaidByLibrary chargesPaidBy)
        {
            var response = _ChargesPaidByRepository.Create(chargesPaidBy);
            return response;
        }
        public Task<BaseResponse<long>> Update(ChargesPaidByLibrary chargesPaidBy)
        {
            var response = _ChargesPaidByRepository.Update(chargesPaidBy);
            return response;
        }
        public Task<BaseResponse<long>> Delete(ChargesPaidByLibrary chargesPaidBy)
        {
            var response = _ChargesPaidByRepository.Delete(chargesPaidBy);
            return response;
        }

        public Task<BaseResponse<List<ChargesPaidByLibrary>>> get(ChargesPaidByLibrary chargesPaidBy, int PageIndex, int PageSize, string Mode)
        {
            var response = _ChargesPaidByRepository.get(chargesPaidBy, PageIndex, PageSize, Mode);
            return response;
        }

        
    }
}
