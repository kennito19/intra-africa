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
    public class TaxTypeValueService : ITaxTypeValueService
    {
        private readonly ITaxTypeValueRepository _taxTypeValueRepository;

        public TaxTypeValueService(ITaxTypeValueRepository taxTypeValueRepository)
        {
            _taxTypeValueRepository = taxTypeValueRepository;
        }

        public Task<BaseResponse<long>> AddTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary)
        {
            var responce = _taxTypeValueRepository.AddTaxTypeValue(taxTypeValueLibrary);
            return responce;
        }

        public Task<BaseResponse<long>> DeleteTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary)
        {
            var responce = _taxTypeValueRepository.DeleteTaxTypeValue(taxTypeValueLibrary);
            return responce;
        }

        public Task<BaseResponse<List<TaxTypeValueLibrary>>> GetTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary, int PageIndex, int PageSize, string Mode)
        {
            var responce = _taxTypeValueRepository.GetTaxTypeValue(taxTypeValueLibrary, PageIndex, PageSize, Mode);
            return responce;
        }

        public Task<BaseResponse<long>> UpdateTaxTypeValue(TaxTypeValueLibrary taxTypeValueLibrary)
        {
            var responce = _taxTypeValueRepository.UpdateTaxTypeValue(taxTypeValueLibrary);
            return responce;
        }
    }
}
